@echo off
cd /d "%~dp0"

setlocal enabledelayedexpansion

SET HOOKS_DIR=.git\hooks
SET GITHOOKS_DIR=githooks

echo Current directory: %CD%
echo Installing Git hooks

IF NOT EXIST .git (
    echo ❌ Not a Git repository (missing git/ folder)
   
)

IF NOT EXIST "%HOOKS_DIR%" (
    echo 📁 Creating hooks directory...
    md "%HOOKS_DIR%"
)

REM Désactive temporairement delayed expansion pour la boucle FOR
setlocal disabledelayedexpansion

for %%f in ("%GITHOOKS_DIR%\*") do (
    set "HOOK_NAME=%%~nxf"
    REM Réactive delayed expansion pour afficher la variable
    setlocal enabledelayedexpansion
    copy /Y "%%f" "%HOOKS_DIR%\!HOOK_NAME!" >nul
    echo ✅ Hook !HOOK_NAME! installed
    endlocal
)

endlocal

echo 🎉 All hooks installed successfully!
pause
