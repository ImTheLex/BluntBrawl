@echo off
SET HOOKS_DIR=.git\hooks
SET GITHOOKS_DIR=githooks

echo Installing Git hooks...

IF NOT EXIST ".git" (
    echo Not a Git repository.
    exit /b 1
)

IF NOT EXIST "%HOOKS_DIR%" (
    mkdir "%HOOKS_DIR%"
)

FOR %%f IN (%GITHOOKS_DIR%\*) DO (
    copy /Y "%%f" "%HOOKS_DIR%\%%~nxf" >nul
)

echo Hooks installed successfully!
pause
