@echo off
setlocal enabledelayedexpansion

set "HOOKS_DIR=.git\hooks"
set "GITHOOKS_DIR=githooks"

echo 🔧 Installing Git hooks...

REM Vérifie que .git existe
IF NOT EXIST ".git" (
    echo ❌ Not a Git repository (missing .git/ folder).
    exit /b 1
)

REM Crée le dossier hooks s'il n'existe pas
IF NOT EXIST "%HOOKS_DIR%" (
    echo 📁 Creating hooks directory...
    mkdir "%HOOKS_DIR%"
)

REM Copie tous les hooks
for %%f in ("%GITHOOKS_DIR%\*") do (
    set "HOOK_NAME=%%~nxf"
    copy /Y "%%f" "%HOOKS_DIR%\!HOOK_NAME!" >nul
    echo ✅ Hook !HOOK_NAME! installed.
)

echo 🎉 All hooks installed successfully!
pause
