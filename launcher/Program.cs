using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TiaPortalLauncher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 获取启动器所在目录
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // 1) 先启动自动保存工具（隐藏窗口）
            string autoSavePath = Path.Combine(baseDir, "TiaPortalAutoSave.exe");
            if (File.Exists(autoSavePath))
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = autoSavePath,
                        WorkingDirectory = baseDir,
                        WindowStyle = ProcessWindowStyle.Minimized
                    };
                    Process.Start(psi);
                }
                catch { }
            }

            // 2) 不再自动启动博途，由用户自行打开
            //    自动保存工具会检测博途进程并自动连接
        }

        static string FindTiaExe()
        {
            string[] candidates =
            {
                @"D:\Program Files\Siemens\Automation\Portal V16\Bin\Siemens.Automation.Portal.exe",
                @"C:\Program Files\Siemens\Automation\Portal V16\Bin\Siemens.Automation.Portal.exe",
                @"D:\Program Files (x86)\Siemens\Automation\Portal V16\Bin\Siemens.Automation.Portal.exe",
                @"C:\Program Files (x86)\Siemens\Automation\Portal V16\Bin\Siemens.Automation.Portal.exe",
            };
            foreach (var p in candidates)
                if (File.Exists(p)) return p;

            // 通过注册表查找
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine
                    .OpenSubKey(@"SOFTWARE\WOW6432Node\Siemens\Automation\_InstalledSW\TIA_OAF-Portal");
                if (key != null)
                {
                    var path = key.GetValue("Path") as string;
                    if (path != null)
                    {
                        var full = Path.Combine(path, "Bin", "Siemens.Automation.Portal.exe");
                        if (File.Exists(full)) return full;
                    }
                }
            }
            catch { }

            return null;
        }
    }
}
