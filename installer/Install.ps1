# ============================================
# 博途自动保存工具 安装程序
# ============================================
Add-Type -AssemblyName System.Windows.Forms

# --- 选择安装目录 ---
$defaultDir = "C:\Program Files\TIA Portal AutoSave"
$folderDlg = New-Object System.Windows.Forms.FolderBrowserDialog
$folderDlg.Description = "选择安装目录"
$folderDlg.SelectedPath = $defaultDir
$folderDlg.ShowNewFolderButton = $true

if ($folderDlg.ShowDialog() -ne 'OK') {
    [System.Windows.Forms.MessageBox]::Show("安装已取消。", "TIA Portal自动保存工具", 'OK', 'Info')
    exit
}
$installDir = $folderDlg.SelectedPath

# --- 复制文件 ---
try {
    if (-not (Test-Path $installDir)) { New-Item -ItemType Directory -Path $installDir -Force | Out-Null }

    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
    $filesDir = Join-Path $scriptDir "files"

    Copy-Item -Path (Join-Path $filesDir "TiaPortalAutoSave.exe") -Destination $installDir -Force
    Copy-Item -Path (Join-Path $filesDir "TiaPortalLauncher.exe") -Destination $installDir -Force
    Copy-Item -Path (Join-Path $filesDir "favicon.ico") -Destination $installDir -Force
} catch {
    [System.Windows.Forms.MessageBox]::Show("复制文件失败: $_", "错误", 'OK', 'Error')
    exit 1
}

# --- 创建桌面快捷方式 ---
$desktop = [Environment]::GetFolderPath('Desktop')
$shortcutPath = Join-Path $desktop "TIA Portal自动保存工具.lnk"

$wsh = New-Object -ComObject WScript.Shell
$lnk = $wsh.CreateShortcut($shortcutPath)
$lnk.TargetPath = Join-Path $installDir "TiaPortalLauncher.exe"
$lnk.WorkingDirectory = $installDir
$lnk.IconLocation = Join-Path $installDir "favicon.ico"
$lnk.Description = "TIA Portal自动保存工具"
$lnk.Save()

# --- 完成 ---
[System.Windows.Forms.MessageBox]::Show(
    "安装完成！`n`n桌面快捷方式「TIA Portal自动保存工具」已创建。`n安装目录: $installDir",
    "TIA Portal自动保存工具", 'OK', 'Info')
