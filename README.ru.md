# SourceGit Plus — Git-клиент с GUI

[English (README.md)](README.md) | **Русский**

> Форк [SourceGit](https://github.com/sourcegit-scm/sourcegit) с дополнительными функциями и исправлениями интерфейса.  
> Скачать: **[Последний релиз](https://github.com/kkkyloo/sourcegit_plus/releases/latest)** · Заметки к релизу: [v2026.13.plus.4](docs/RELEASE_v2026.13.plus.4.md)

## Только в Plus

* **Drag-and-Drop в Local Changes** — перетаскивание файлов и папок между **UNSTAGED** и **STAGED** для stage/unstage; поддержка нескольких выбранных файлов.
* **Счётчики Ahead / Behind** — на кнопках Pull и Push видно, на сколько коммитов ветка впереди/позади upstream.
* **Прямой коммит в ветку** — коммит выбранных изменений в любую локальную ветку без checkout (проверка конфликтов + опция сброса из рабочей копии).
* **Обновления только из форка** — приложение читает `data/version.json` из **этого** репозитория; уведомления upstream SourceGit не показываются.

## История релизов Plus

### v2026.13.plus.4 — актуальный

* **Кнопки заголовка окна (Windows)** — область перетаскивания titlebar больше не перекрывает «Свернуть / Развернуть / Закрыть», поэтому самый верхний край кнопок в развернутом окне нажимается.

### v2026.13.plus.3

* **Выделение в деревьях и списках** — клик по другой строке или родительской папке в режиме file tree нормально переключает выделение (без «залипания»).
* **Стрелки раскрытия папок** — в деревьях веток, тегов, submodules и local changes клик перехватывается только при expand/collapse.

### v2026.13.plus.2

* **Кнопки заголовка окна (Windows)** — «Свернуть / Развернуть / Закрыть» нажимаются по всей высоте кнопки (Avalonia 12).
* **Проверка обновлений** — `VERSION` синхронизирован с `data/version.json`; ложное «есть обновление» на актуальной сборке Plus не показывается.

### v2026.13.plus.1

Первый релиз Plus на базе upstream **v2026.13** (Avalonia 12):

* Счётчики Ahead/Behind, ребрендинг SourceGit Plus, URL обновлений форка.
* Drag-and-drop и Direct Commit адаптированы под API Avalonia 12.

Подробности upstream v2026.13: [release notes](https://github.com/sourcegit-scm/sourcegit/releases/tag/v2026.13).

---

## Скачать и установить

**Нужен Git ≥ 2.25.1.**

| Платформа | Где взять |
|-----------|-----------|
| Windows   | [Releases](https://github.com/kkkyloo/sourcegit_plus/releases/latest) — `sourcegit_2026.13.plus.4.win-x64.zip`. Используйте [Git for Windows](https://git-scm.com/download/win) (MSYS Git не поддерживается). |
| Linux     | AppImage / `.deb` / `.rpm` — см. `sourcegit_2026.13.plus.4.*` в [Releases](https://github.com/kkkyloo/sourcegit_plus/releases/latest). |
| macOS     | `sourcegit_2026.13.plus.4.osx-*.zip` в [Releases](https://github.com/kkkyloo/sourcegit_plus/releases/latest). После распаковки: `sudo xattr -cr /Applications/SourceGit.app` |

Сборки из CI (последние коммиты): [GitHub Actions](https://github.com/kkkyloo/sourcegit_plus/actions).

### Папка с данными приложения

| ОС      | Путь |
|---------|------|
| Windows | `%APPDATA%\SourceGit` |
| Linux   | `~/.sourcegit` |
| macOS   | `~/Library/Application Support/SourceGit` |

Портативный режим: папка `data` рядом с исполняемым файлом (Windows zip / Linux AppImage).

---

## Полная документация

Остальные разделы (скриншоты, внешние редакторы, AI, переводы, сборка из исходников) — в [README.md](README.md) на английском (наследовано от upstream SourceGit).

[![stars](https://img.shields.io/github/stars/kkkyloo/sourcegit_plus.svg)](https://github.com/kkkyloo/sourcegit_plus/stargazers)
[![latest](https://img.shields.io/github/v/release/kkkyloo/sourcegit_plus.svg)](https://github.com/kkkyloo/sourcegit_plus/releases/latest)
[![downloads](https://img.shields.io/github/downloads/kkkyloo/sourcegit_plus/total)](https://github.com/kkkyloo/sourcegit_plus/releases)
