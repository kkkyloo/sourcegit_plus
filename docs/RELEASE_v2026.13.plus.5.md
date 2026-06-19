# SourceGit Plus v2026.13.5 (Plus 5)

**English** | [Русский](#русский)

## English

Fork of [SourceGit](https://github.com/sourcegit-scm/sourcegit) with extra features and UX fixes.  
Based on upstream **v2026.13** (Avalonia 12).

### Download

| Platform | Typical files on [Releases](https://github.com/kkkyloo/sourcegit_plus/releases/tag/v2026.13.plus.5) |
|----------|-----------------------------------------------------------------------------------------------------|
| Windows  | `sourcegit_2026.13.plus.5.win-x64.zip`, `sourcegit_2026.13.plus.5.win-arm64.zip` |
| Linux    | `sourcegit_2026.13.plus.5.linux.amd64.AppImage`, `sourcegit_2026.13.plus.5-1_amd64.deb`, `.rpm` |
| macOS    | `sourcegit_2026.13.plus.5.osx-arm64.zip`, `sourcegit_2026.13.plus.5.osx-x64.zip` |

Latest build: https://github.com/kkkyloo/sourcegit_plus/releases/latest

### What's new in Plus 5

* **Caption buttons on Windows** — the Minimize / Maximize / Close buttons now use Avalonia's interactive `User` chrome role instead of Win32 `HTMINBUTTON` / `HTMAXBUTTON` / `HTCLOSE`. That keeps hover and click handling on the button at the very top screen edge when the window is maximized.

### Also included from Plus 4

* **Titlebar geometry** — the draggable titlebar area no longer overlaps the caption button column.

### Plus features

* **Drag-and-drop local changes** — move files between **UNSTAGED** and **STAGED**; multi-select drag supported.
* **Ahead / Behind counters** on Pull and Push toolbar buttons.
* **Direct commit to branch** — commit selected changes to any local branch without checking it out.
* **Rebranded** — About dialog, metadata, and update URL point to `kkkyloo/sourcegit_plus`.

### Requirements

* Git **>= 2.25.1** ([Git for Windows](https://git-scm.com/download/win) on Windows; MSYS Git is not supported).

---

## Русский

Форк [SourceGit](https://github.com/sourcegit-scm/sourcegit) с дополнительными возможностями и исправлениями UX.  
Основа — upstream **v2026.13** (Avalonia 12).

### Скачать

| Платформа | Файлы на [Releases](https://github.com/kkkyloo/sourcegit_plus/releases/tag/v2026.13.plus.5) |
|-----------|-------------------------------------------------------------------------------------------|
| Windows   | `sourcegit_2026.13.plus.5.win-x64.zip`, `sourcegit_2026.13.plus.5.win-arm64.zip` |
| Linux     | `sourcegit_2026.13.plus.5.linux.amd64.AppImage`, `sourcegit_2026.13.plus.5-1_amd64.deb`, `.rpm` |
| macOS     | `sourcegit_2026.13.plus.5.osx-arm64.zip`, `sourcegit_2026.13.plus.5.osx-x64.zip` |

Актуальная сборка: https://github.com/kkkyloo/sourcegit_plus/releases/latest

### Новое в Plus 5

* **Кнопки заголовка окна в Windows** — «Свернуть / Развернуть / Закрыть» теперь используют интерактивную chrome-роль Avalonia `User` вместо Win32 `HTMINBUTTON` / `HTMAXBUTTON` / `HTCLOSE`. Поэтому наведение и клик по самому верхнему краю экрана в развернутом окне остаются на кнопке.

### Также из Plus 4

* **Геометрия titlebar** — область перетаскивания заголовка больше не перекрывает колонку кнопок.

### Возможности Plus

* **Drag-and-drop в Local Changes** — перетаскивание файлов между **UNSTAGED** и **STAGED**, поддержка нескольких выбранных файлов.
* **Счётчики Ahead / Behind** на кнопках Pull и Push.
* **Прямой коммит в ветку** — коммит выбранных изменений в любую локальную ветку без переключения на неё.
* **Ребрендинг** — About, метаданные и URL обновлений ведут на `kkkyloo/sourcegit_plus`.

### Требования

* Git **>= 2.25.1** (на Windows — [Git for Windows](https://git-scm.com/download/win); MSYS Git не поддерживается).

---

Upstream changelog: https://github.com/sourcegit-scm/sourcegit/releases/tag/v2026.13
