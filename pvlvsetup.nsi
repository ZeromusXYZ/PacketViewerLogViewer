; pvlvsetup.nsi

;--------------------------------

; The name of the installer
Name "PVLV Setup"

; The file to write
OutFile "pvlvsetup.exe"

; Request application privileges for Windows Vista
RequestExecutionLevel user

; Build Unicode installer
Unicode True

; The default installation directory
InstallDir $DESKTOP\PVLV

SetCompressor /SOLID lzma

;--------------------------------

; Pages

Page directory
Page instfiles

;--------------------------------

; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Main exe files (excluding the installer)
  File /x pvlvsetup.* *.*

  ; Extra library files
  ; x64
  SetOutPath $INSTDIR\runtimes\win-x64\native
  File runtimes\win-x64\native\*.*
  ; x86
  SetOutPath $INSTDIR\runtimes\win-x86\native
  File runtimes\win-x86\native\*.*

  ; Program Data
  SetOutPath $INSTDIR\data\lookup
  File /r data\lookup\*.*
  SetOutPath $INSTDIR\data\parse
  File /r data\parse\*.*
  SetOutPath $INSTDIR\data\filter
  File /r data\filter\*.*

  SetOutPath $INSTDIR
  
SectionEnd ; end the section
