!include "MUI2.nsh"

Name "Egram"
OutFile "egram-setup.exe"

InstallDir "$APPDATA\Egram"

InstallDirRegKey HKCU "Software\Egram64" ""

RequestExecutionLevel highest

!define MUI_ABORTWARNING

!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Russian"

Section "Egram" SecInstall

  SetOutPath "$INSTDIR"
  
  File /r ".\*"
  
  ;Store installation folder
  WriteRegStr HKCU "Software\Egram64" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

Section "Shortcut"
  CreateShortCut "$DESKTOP\Egram.lnk" "$INSTDIR\Tel.Egram.exe"
SectionEnd

Section "Uninstall"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\Egram64"

SectionEnd

Function .onInit
  SectionSetFlags 0 17
FunctionEnd
