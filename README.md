# SGame (ShooterGame)

2D-игра в пиксельном стиле для Windows. Ты играешь за жреца: собираешь монеты, отбиваешься от вампира и черепов, ищешь ключ и открываешь сундук. Статистика (монеты и убийства) сохраняется в локальную базу данных.

> Статус: ранняя версия (`0.1.0`). В планах: онлайн-режим через API на Blazor.

## Управление

| Клавиша | Действие |
|---|---|
| `W` `A` `S` `D` | движение |
| `Пробел` | выстрел |
| `B` | открыть сундук (нужно стоять рядом) |
| ЛКМ | взаимодействие (поиск ключа, в разработке) |

## Технологии

- .NET 10, WPF
- MonoGame (`MonoGame.Framework.WpfInterop`) для отрисовки игры внутри WPF-окна
- Entity Framework Core + SQLite для сохранения статистики

## Структура проекта

```
SGame/
├── App.xaml, MainWindow.xaml      стартовое окно (меню)
├── Windows/                       окна: GameOverWindow (экран смерти)
├── Models/
│   ├── Game.cs, GameWindow.xaml   игровой цикл и окно с игрой
│   ├── Enemy/                     Player, Vampir, Employee (персонажи)
│   ├── GameObject/                Bullet, Coins, Chest, Key
│   ├── BoundsModel/               препятствия и вода
│   └── FactoryModels/             FactoryPerson (создание объектов и текстур)
├── GameLogic/                     LogicGame, Poison (правила игры)
├── ViewModels/                    ApplicationContext (БД), PlayerEntity (модель статистики)
└── Content/Texture/               спрайты и фон
```

## Запуск из исходников

Нужно: Windows 10/11, [.NET 10 SDK](https://dotnet.microsoft.com/download) и Visual Studio 2022+ (по желанию).

```
git clone https://github.com/VolkovKonstantin0105/ShooterGame.git
cd ShooterGame
dotnet run --project SGame.csproj
```

Или открой `SGame.slnx` в Visual Studio и нажми F5.

## Как собрать игру для других людей

### 1. Публикация (папка с готовой игрой)

```
dotnet publish SGame.csproj -c Release -r win-x64 --self-contained true -o publish
```

В Visual Studio то же самое: ПКМ по проекту, "Опубликовать", "Папка".

- `--self-contained true` кладет .NET внутрь, и игрокам не нужно ничего ставить. Папка получится большой (около 150 МБ), в установщике она сожмется.
- Если убрать этот флаг, папка будет маленькой, но на компьютере игрока должен стоять .NET 10 Desktop Runtime.
- Папка `publish/` уже в `.gitignore`, в репозиторий она не попадет.

Проверь результат: запусти `publish\SGame.exe` не из Visual Studio, а двойным кликом. Если игра стартует, текстуры на месте и статистика сохраняется, можно делать установщик.

### 2. Установщик (Inno Setup)

1. Скачай и установи [Inno Setup](https://jrsoftware.org/isinfo.php).
2. Создай в репозитории папку `installer` и файл `installer/SGame.iss` с таким содержимым:

```
#define AppName "SGame"
#define AppVersion "0.1.0"

[Setup]
AppId={{B7A1E2D4-5C3F-4E8A-9D61-2F0C7A9E4B15}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=Konstantin Volkov
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
PrivilegesRequired=lowest
OutputDir=Output
OutputBaseFilename=SGameSetup-{#AppVersion}
Compression=lzma2
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible
WizardStyle=modern

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "Создать ярлык на рабочем столе"; GroupDescription: "Дополнительно:"

[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\SGame"; Filename: "{app}\SGame.exe"; WorkingDir: "{app}"
Name: "{autodesktop}\SGame"; Filename: "{app}\SGame.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\SGame.exe"; Description: "Запустить SGame"; Flags: nowait postinstall skipifsilent
```

3. Открой `SGame.iss` в Inno Setup и нажми Build, "Compile" (или F9). Установщик появится в `installer/Output/SGameSetup-0.1.0.exe`.
4. Добавь в `.gitignore` строку `installer/Output/`, чтобы готовый `.exe` не попал в репозиторий.

Заметки:

- Если Inno Setup ругается на `Russian.isl`, удали секцию `[Languages]`, установщик будет на английском.
- `AppId` это уникальный номер приложения. Оставь его одинаковым во всех версиях, тогда новая версия обновит старую, а не поставится рядом.
- Windows SmartScreen может показать предупреждение "неизвестный издатель": так бывает со всеми неподписанными установщиками. Нажми "Подробнее", "Выполнить в любом случае".

## Как залить на GitHub

Первый раз (уже сделано для этого репозитория):

```
git remote set-url origin https://github.com/VolkovKonstantin0105/ShooterGame.git
git add .
git commit -m "описание изменений"
git push
```

Дальше цикл всегда один:

```
git status          что изменилось
git add .           подготовить изменения
git commit -m "..." сохранить в историю
git push            отправить на GitHub
```

В репозиторий кладем только исходники и текстуры. Не кладем: `bin/`, `obj/`, `.vs/`, `publish/`, готовые `.exe`.

## Как выкладывать готовую игру (релизы)

Установщик не хранят в коде, его прикладывают к релизу на GitHub.

1. Убедись, что все закоммичено и запушено.
2. Поставь метку версии и отправь ее:

```
git tag -a v0.1.0 -m "Первая версия"
git push origin v0.1.0
```

3. На странице репозитория: "Releases", "Draft a new release", выбери тег `v0.1.0`, напиши, что нового, перетащи в окно файл `SGameSetup-0.1.0.exe`, нажми "Publish release".
4. Ссылку на релиз можно отправлять друзьям: они скачают установщик оттуда.

## Как вести разные версии игры

**Номера версий** делаем по схеме `мажор.минор.патч`:

- `0.1.0` первая рабочая версия;
- `0.1.1` исправили баги, новых возможностей нет;
- `0.2.0` добавили что-то новое (босс, уровни, магазин);
- `1.0.0` игра готова.

Версию меняем в двух местах: в `SGame.csproj` (`<Version>`) и в `installer/SGame.iss` (`AppVersion`).

**Ветки.** `master` это всегда рабочая версия, которую не стыдно показать. Новое делаем в отдельных ветках:

```
git switch -c feature/boss-fight      создать ветку и перейти в нее
(работаешь, коммитишь)
git push -u origin feature/boss-fight
git switch master                     вернуться в основную
git merge feature/boss-fight          влить готовую функцию
git push
```

Если что-то пошло не так, `master` остается целым, а ветку можно просто выбросить.

**Исправить старую версию**, когда `master` уже ушел вперед:

```
git switch -c hotfix/0.1.1 v0.1.0     ветка от метки старой версии
(правишь, коммитишь, собираешь, делаешь тег v0.1.1)
```

**Посмотреть, какие версии уже есть:** `git tag` (список меток) и `git log --oneline` (история).

## Чек-лист перед каждым релизом

- [ ] игра запускается из `publish\SGame.exe` двойным кликом
- [ ] после смерти игрока показывается экран "ВЫ ПОГИБЛИ" и окно закрывается
- [ ] статистика сохраняется в базу данных
- [ ] поднята версия в `SGame.csproj` и `SGame.iss`
- [ ] все закоммичено, `git push` выполнен
- [ ] поставлен тег `vX.Y.Z` и отправлен на GitHub
- [ ] установщик приложен к релизу

## Данные игрока

Статистика (монеты, убийства) хранится в `SGAME1.db` в папке `%LocalAppData%\SGame`. Удалишь файл, и статистика обнулится.

## Авторы и ресурсы

- Автор: Константин Волков
- Графика: 2D Pixel Dungeon Asset Pack v2.0. Перед публикацией репозитория или игры проверь, что лицензия набора разрешает распространять картинки вместе с игрой.
