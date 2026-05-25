using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace Installer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string installDir = @"C:\Program Files\TIA Portal AutoSave";
            var asm = Assembly.GetExecutingAssembly();

            try
            {
                if (!Directory.Exists(installDir))
                    Directory.CreateDirectory(installDir);

                // 释放嵌入的文件
                ExtractResource(asm, "Installer.files.TiaPortalAutoSave.exe",
                    Path.Combine(installDir, "TiaPortalAutoSave.exe"));
                ExtractResource(asm, "Installer.files.TiaPortalLauncher.exe",
                    Path.Combine(installDir, "TiaPortalLauncher.exe"));
                ExtractResource(asm, "Installer.files.favicon.ico",
                    Path.Combine(installDir, "favicon.ico"));

                // 创建桌面快捷方式
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string lnkPath = Path.Combine(desktop, "TIA Portal自动保存工具.lnk");

                CreateShortcut(lnkPath,
                    Path.Combine(installDir, "TiaPortalLauncher.exe"),
                    installDir,
                    Path.Combine(installDir, "favicon.ico"),
                    "TIA Portal自动保存工具");

                // 自动添加用户到 Siemens TIA Openness 用户组
                string groupResult = EnsureOpennessGroup();

                string msg = "安装完成！\n\n" +
                    "桌面快捷方式「TIA Portal自动保存工具」已创建。\n" +
                    "安装目录: " + installDir;
                if (!string.IsNullOrEmpty(groupResult))
                    msg += "\n\n" + groupResult;

                MessageBox.Show(msg, "TIA Portal自动保存工具", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("安装失败:\n" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void ExtractResource(Assembly asm, string resourceName, string destPath)
        {
            using (var stream = asm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new Exception($"找不到嵌入资源: {resourceName}");
                using (var fs = File.Create(destPath))
                    stream.CopyTo(fs);
            }
        }

        static void CreateShortcut(string lnkPath, string target, string workDir, string icon, string desc)
        {
            // 通过 WSH COM 创建快捷方式
            Type t = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(t);
            dynamic lnk = shell.CreateShortcut(lnkPath);
            lnk.TargetPath = target;
            lnk.WorkingDirectory = workDir;
            lnk.IconLocation = icon;
            lnk.Description = desc;
            lnk.Save();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(lnk);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(shell);
        }

        static string EnsureOpennessGroup()
        {
            const string groupName = "Siemens TIA Openness";

            // 检查组是否存在
            try
            {
                var checkPsi = new ProcessStartInfo
                {
                    FileName = "net",
                    Arguments = $"localgroup \"{groupName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(checkPsi))
                {
                    proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    if (proc.ExitCode != 0)
                        return $"提示: 用户组 \"{groupName}\" 不存在，请确认博途已安装且勾选了 Openness 组件。";
                }
            }
            catch { return ""; }

            // 检查用户是否已在组中
            try
            {
                string userName = Environment.UserName;
                var checkPsi = new ProcessStartInfo
                {
                    FileName = "net",
                    Arguments = $"localgroup \"{groupName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(checkPsi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    if (output.IndexOf(userName, StringComparison.OrdinalIgnoreCase) >= 0)
                        return $"当前用户已在 \"{groupName}\" 组中，无需重复添加。";
                }
            }
            catch { }

            // 添加用户到组
            try
            {
                string fullUserName = Environment.UserDomainName + "\\" + Environment.UserName;
                var addPsi = new ProcessStartInfo
                {
                    FileName = "net",
                    Arguments = $"localgroup \"{groupName}\" \"{fullUserName}\" /add",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(addPsi))
                {
                    proc.WaitForExit();
                    if (proc.ExitCode == 0)
                        return $"已自动将当前用户添加到 \"{groupName}\" 组。\n首次添加需注销或重启电脑后生效。";
                    else
                        return $"自动添加用户组失败，请手动执行:\n  net localgroup \"{groupName}\" \"%USERNAME%\" /add";
                }
            }
            catch (Exception ex)
            {
                return $"自动添加用户组出错: {ex.Message}";
            }
        }
    }
}
