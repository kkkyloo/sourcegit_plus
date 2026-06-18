# SourceGit Plus v2026.13.3 (Plus 3)

**English** | [Русский](#русский)

## English

Fork of [SourceGit](https://github.com/sourcegit-scm/sourcegit) with extra features and UX fixes.  
Based on upstream **v2026.13** (Avalonia 12).

### Download

| Platform | File (from [Releases](https://github.com/kkkyloo/sourcegit_plus/releases/tag/v2026.13.plus.3)) |
|----------|--------------------------------------------------------------------------------------------------|
| Windows  | `SourceGit-2026.13.3-win-x64.zip` or `.setup.exe` |
| Linux    | `SourceGit-2026.13.3-linux-x64.AppImage`, `.deb`, or `.rpm` |
| macOS    | `SourceGit-2026.13.3-osx-arm64.dmg` / `osx-x64.dmg` |

Latest build: https://github.com/kkkyloo/sourcegit_plus/releases/latest

### What's new in Plus 3

* **Tree / list selection** — after selecting a file or folder, clicking another row (or a parent folder in *file tree* mode) now switches selection reliably.
* **Expander chevrons** — branch, tag, submodule, and change trees only capture the click when you actually expand/collapse a folder.

### Also included from Plus 2

* **Title bar buttons** (Windows) — Minimize / Maximize / Close are clickable across the full button height (Avalonia 12 fix).
* **Update check** — notifications come only from this fork; no false “update available” when you already run the latest Plus build.

### Plus features (since Plus 1)

* **Drag-and-drop local changes** — move files between **UNSTAGED** and **STAGED**; multi-select drag supported.
* **Ahead / Behind counters** on Pull and Push toolbar buttons.
* **Direct commit to branch** — commit selected changes to any local branch without checking it out.
* **Rebranded** — About dialog, metadata, and update URL point to `kkkyloo/sourcegit_plus`.

### Requirements

* Git **≥ 2.25.1** ([Git for Windows](https://git-scm.com/download/win) on Windows; MSYS Git is not supported).

---

## Русский

Форк [SourceGit](https://github.com/sourcegit-scm/sourcegit) с дополнительными возможностями и исправлениями UX.  
Основа — upstream **v2026.13** (Avalonia 12).

### Скачать

| Платформа | Файл ([Releases](https://github.com/kkkyloo/sourcegit_plus/releases/tag/v2026.13.plus.3)) |
|-----------|-------------------------------------------------------------------------------------------|
| Windows   | `SourceGit-2026.13.3-win-x64.zip` или `.setup.exe` |
| Linux     | `SourceGit-2026.13.3-linux-x64.AppImage`, `.deb` или `.rpm` |
| macOS     | `SourceGit-2026.13.3-osx-arm64.dmg` / `osx-x64.dmg` |

Актуальная сборка: https://github.com/kkkyloo/sourcegit_plus/releases/latest

### Новое в Plus 3

* **Выделение в деревьях и списках** — после выбора файла или папки клик по другой строке (или по родительской папке в режиме *file tree*) теперь нормально переключает выделение.
* **Стрелки раскрытия** — в деревьях веток, тегов, submodules и изменений клик перехватывается только при сворачивании/разворачивании папки.

### Также из Plus 2

* **Кнопки заголовка окна** (Windows) — «Свернуть / Развернуть / Закрыть» нажимаются по всей высоте кнопки (исправление для Avalonia 12).
* **Проверка обновлений** — уведомления только из этого форка; ложное «есть обновление» при актуальной версии Plus больше не показывается.

### Возможности Plus (с Plus 1)

* **Drag-and-drop в Local Changes** — перетаскивание файлов между **UNSTAGED** и **STAGED**, поддержка нескольких файлов.
* **Счётчики Ahead / Behind** на кнопках Pull и Push.
* **Прямой коммит в ветку** — коммит выбранных изменений в любую локальную ветку без переключения на неё.
* **Ребрендинг** — About, метаданные и URL обновлений ведут на `kkkyloo/sourcegit_plus`.

### Требования

* Git **≥ 2.25.1** (на Windows — [Git for Windows](https://git-scm.com/download/win); MSYS Git не поддерживается).

---

Upstream changelog: https://github.com/sourcegit-scm/sourcegit/releases/tag/v2026.13
