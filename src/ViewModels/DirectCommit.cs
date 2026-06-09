using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SourceGit.ViewModels
{
    public class DirectCommit : Popup
    {
        public Repository Repository { get; }
        public List<Models.Change> Changes { get; }
        public List<Models.Branch> Branches { get; }

        [Required(ErrorMessage = "Commit message is required!")]
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value, true);
        }

        public Models.Branch SelectedBranch
        {
            get => _selectedBranch;
            set => SetProperty(ref _selectedBranch, value);
        }

        public bool DiscardAfterCommit
        {
            get => _discardAfterCommit;
            set => SetProperty(ref _discardAfterCommit, value);
        }

        public bool HasConflicts
        {
            get => _hasConflicts;
            set => SetProperty(ref _hasConflicts, value);
        }

        public bool ForceOverwrite
        {
            get => _forceOverwrite;
            set => SetProperty(ref _forceOverwrite, value);
        }

        public List<string> Conflicts
        {
            get => _conflicts;
            set => SetProperty(ref _conflicts, value);
        }

        public DirectCommit(Repository repo, List<Models.Change> changes)
        {
            Repository = repo;
            Changes = changes;

            Branches = new List<Models.Branch>();
            foreach (var b in repo.Branches)
            {
                if (b.IsLocal)
                    Branches.Add(b);
            }

            if (repo.CurrentBranch != null && repo.CurrentBranch.IsLocal)
                SelectedBranch = repo.CurrentBranch;
            else if (Branches.Count > 0)
                SelectedBranch = Branches[0];
        }

        private string GetFileMode(string repo, string branch, string path)
        {
            var result = new Commands.Command()
            {
                WorkingDirectory = repo,
                Args = $"ls-tree {branch} \"{path}\""
            }.ReadToEnd();

            if (result.IsSuccess && !string.IsNullOrEmpty(result.StdOut))
            {
                var parts = result.StdOut.Split(' ', 2);
                if (parts.Length > 0 && parts[0].Length == 6)
                    return parts[0];
            }

            result = new Commands.Command()
            {
                WorkingDirectory = repo,
                Args = $"ls-tree HEAD \"{path}\""
            }.ReadToEnd();

            if (result.IsSuccess && !string.IsNullOrEmpty(result.StdOut))
            {
                var parts = result.StdOut.Split(' ', 2);
                if (parts.Length > 0 && parts[0].Length == 6)
                    return parts[0];
            }

            return "100644";
        }

        public override async Task<bool> Sure()
        {
            if (SelectedBranch == null)
                return false;

            if (!Check())
                return false;

            using var lockWatcher = Repository.LockWatcher();

            ProgressDescription = "Checking for conflicts ...";

            if (!ForceOverwrite)
            {
                var conflictsList = new List<string>();
                foreach (var change in Changes)
                {
                    var diffCmd = new Commands.Command()
                    {
                        WorkingDirectory = Repository.FullPath,
                        Args = $"diff --quiet {SelectedBranch.Name} -- \"{change.Path}\""
                    };
                    var res = diffCmd.ReadToEnd();
                    if (!res.IsSuccess)
                    {
                        conflictsList.Add(change.Path);
                    }
                }

                if (conflictsList.Count > 0)
                {
                    Conflicts = conflictsList;
                    HasConflicts = true;
                    ProgressDescription = "Conflicts detected!";
                    OnPropertyChanged(nameof(Conflicts));
                    OnPropertyChanged(nameof(HasConflicts));
                    OnPropertyChanged(nameof(ProgressDescription));
                    return false;
                }
            }

            ProgressDescription = "Preparing commit ...";
            var log = Repository.CreateLog($"Commit directly to '{SelectedBranch.Name}'");
            Use(log);

            var tempIndexFile = Path.Combine(Path.GetTempPath(), "sourcegit_index_" + Guid.NewGuid().ToString("N"));

            try
            {
                var readTreeCmd = new Commands.Command()
                {
                    WorkingDirectory = Repository.FullPath,
                    Args = $"read-tree {SelectedBranch.Name}"
                };
                readTreeCmd.Envs.Add("GIT_INDEX_FILE", tempIndexFile);
                var readTreeSuccess = await readTreeCmd.ExecAsync();
                if (!readTreeSuccess)
                {
                    log.Complete();
                    if (File.Exists(tempIndexFile)) File.Delete(tempIndexFile);
                    return true;
                }

                foreach (var change in Changes)
                {
                    if (change.WorkTree == Models.ChangeState.Deleted || change.Index == Models.ChangeState.Deleted)
                    {
                        var removeCmd = new Commands.Command()
                        {
                            WorkingDirectory = Repository.FullPath,
                            Args = $"update-index --force-remove \"{change.Path}\""
                        };
                        removeCmd.Envs.Add("GIT_INDEX_FILE", tempIndexFile);
                        await removeCmd.ExecAsync();
                    }
                    else
                    {
                        var hashCmd = new Commands.Command()
                        {
                            WorkingDirectory = Repository.FullPath,
                            Args = $"hash-object -w \"{change.Path}\""
                        };
                        var hashRes = hashCmd.ReadToEnd();
                        if (!hashRes.IsSuccess || string.IsNullOrEmpty(hashRes.StdOut))
                        {
                            log.Complete();
                            if (File.Exists(tempIndexFile)) File.Delete(tempIndexFile);
                            return true;
                        }

                        var fileSha = hashRes.StdOut.Trim();
                        var fileMode = GetFileMode(Repository.FullPath, SelectedBranch.Name, change.Path);

                        var updateCmd = new Commands.Command()
                        {
                            WorkingDirectory = Repository.FullPath,
                            Args = $"update-index --add --cacheinfo {fileMode} {fileSha} \"{change.Path}\""
                        };
                        updateCmd.Envs.Add("GIT_INDEX_FILE", tempIndexFile);
                        await updateCmd.ExecAsync();
                    }
                }

                var writeTreeCmd = new Commands.Command()
                {
                    WorkingDirectory = Repository.FullPath,
                    Args = "write-tree"
                };
                writeTreeCmd.Envs.Add("GIT_INDEX_FILE", tempIndexFile);
                var writeTreeRes = writeTreeCmd.ReadToEnd();
                if (!writeTreeRes.IsSuccess || string.IsNullOrEmpty(writeTreeRes.StdOut))
                {
                    log.Complete();
                    if (File.Exists(tempIndexFile)) File.Delete(tempIndexFile);
                    return true;
                }

                var treeSha = writeTreeRes.StdOut.Trim();

                var tmpMsgFile = Path.GetTempFileName();
                await File.WriteAllTextAsync(tmpMsgFile, Message);

                var commitTreeCmd = new Commands.Command()
                {
                    WorkingDirectory = Repository.FullPath,
                    Args = $"commit-tree {treeSha} -p {SelectedBranch.Name} -F \"{tmpMsgFile}\""
                };
                var commitTreeRes = commitTreeCmd.ReadToEnd();
                File.Delete(tmpMsgFile);

                if (!commitTreeRes.IsSuccess || string.IsNullOrEmpty(commitTreeRes.StdOut))
                {
                    log.Complete();
                    if (File.Exists(tempIndexFile)) File.Delete(tempIndexFile);
                    return true;
                }

                var newCommitSha = commitTreeRes.StdOut.Trim();

                var updateRefCmd = new Commands.Command()
                {
                    WorkingDirectory = Repository.FullPath,
                    Args = $"update-ref refs/heads/{SelectedBranch.Name} {newCommitSha}"
                };
                var updateRefSuccess = await updateRefCmd.ExecAsync();
                if (updateRefSuccess)
                {
                    if (DiscardAfterCommit)
                    {
                        await Commands.Discard.ChangesAsync(Repository.FullPath, Changes, log);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempIndexFile))
                    File.Delete(tempIndexFile);
            }

            log.Complete();
            Repository.MarkWorkingCopyDirtyManually();
            return true;
        }

        private string _message = string.Empty;
        private Models.Branch _selectedBranch = null;
        private bool _discardAfterCommit = false;
        private bool _hasConflicts = false;
        private bool _forceOverwrite = false;
        private List<string> _conflicts = new();
    }
}
