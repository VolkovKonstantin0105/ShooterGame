\xef\xbb\xbf#define AppName "SGame"
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
