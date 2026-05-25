@echo off
chcp 65001 >nul
echo ==============================
echo   TIA Portal自动保存工具 构建脚本
echo ==============================
echo.

set BACKUP_DIR=%~dp0
set MAIN_DIR=C:\Users\Li Bin\TiaPortalAutoSave

echo [1/4] 构建主程序...
cd /d "%MAIN_DIR%"
dotnet build -c Release
if errorlevel 1 (
    echo 构建主程序失败！
    pause
    exit /b 1
)
echo 主程序构建完成
echo.

echo [2/4] 构建启动器...
cd /d "%BACKUP_DIR%\launcher"
dotnet build -c Release
if errorlevel 1 (
    echo 构建启动器失败！
    pause
    exit /b 1
)
echo 启动器构建完成
echo.

echo [3/4] 构建安装程序...
cd /d "%BACKUP_DIR%\installer"
dotnet build -c Release
if errorlevel 1 (
    echo 构建安装程序失败！
    pause
    exit /b 1
)
echo 安装程序构建完成
echo.

echo [4/4] 构建卸载程序...
cd /d "%BACKUP_DIR%\uninstaller"
dotnet build -c Release
if errorlevel 1 (
    echo 构建卸载程序失败！
    pause
    exit /b 1
)
echo 卸载程序构建完成
echo.

echo ==============================
echo   所有项目构建完成！
echo ==============================
echo.
echo 生成的文件：
echo - 主程序: %MAIN_DIR%\bin\Release\net48\TiaPortalAutoSave.exe
echo - 启动器: %BACKUP_DIR%\launcher\bin\Release\net48\TiaPortalLauncher.exe
echo - 安装程序: %BACKUP_DIR%\installer\bin\Release\net48\Installer.exe
echo - 卸载程序: %BACKUP_DIR%\uninstaller\bin\Release\net48\Uninstall_TIA_Portal_AutoSave.exe
echo.
pause
