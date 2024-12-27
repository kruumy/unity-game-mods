@echo off

set "targetFolder=%USERPROFILE%\Documents\My Games\Terraria\tModLoader"

if not exist "%targetFolder%" (
    echo The target folder "%targetFolder%" does not exist.
    pause
    exit /b 1
)

mklink /D "%targetFolder%\ModSources" "%CD%"

pause
