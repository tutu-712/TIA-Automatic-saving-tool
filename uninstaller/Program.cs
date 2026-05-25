using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Uninstaller
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string installDir = @"C:\Program Files\TIA Portal AutoSave";
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string lnkPath = Path.Combine(desktop, "TIA Portal自动保存工具.lnk");

            // 确认卸载
            var result = MessageBox.Show(
                "确定要卸载 TIA Portal自动保存工具 吗？\n\n" +
                "将删除以下内容：\n" +
                "- 安装目录: " + installDir + "\n" +
                "- 桌面快捷方式: TIA Portal自动保存工具.lnk",
                "卸载确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                // 先关闭正在运行的程序
                CloseRunningProcesses();

                int deleted = 0;

                // 删除桌面快捷方式
                if (File.Exists(lnkPath))
                {
                    File.Delete(lnkPath);
                    deleted++;
                    Console.WriteLine("已删除桌面快捷方式");
                }

                // 删除安装目录
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, true);
                    deleted++;
                    Console.WriteLine("已删除安装目录: " + installDir);
                }

                if (deleted > 0)
                {
                    MessageBox.Show(
                        "卸载完成！\n\n" +
                        "已清理所有相关文件和快捷方式。\n" +
                        "提示: Siemens TIA Openness 用户组未删除（可能被其他程序使用）。",
                        "卸载完成",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "未找到已安装的文件，可能已被删除。",
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "卸载过程中出错:\n" + ex.Message + "\n\n" +
                    "请确保程序未在运行，然后重试。",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        static void CloseRunningProcesses()
        {
            try
            {
                // 关闭主程序
                foreach (var proc in Process.GetProcessesByName("TiaPortalAutoSave"))
                {
                    proc.Kill();
                    proc.WaitForExit(3000);
                }

                // 关闭启动器
                foreach (var proc in Process.GetProcessesByName("TiaPortalLauncher"))
                {
                    proc.Kill();
                    proc.WaitForExit(3000);
                }
            }
            catch { }
        }
    }
}
