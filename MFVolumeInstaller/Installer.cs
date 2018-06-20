using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;

namespace MFVolumeInstaller
{
    public class Installer
    {
        public Installer(string[] args)
        {
            HandleArgs(args);
        }

        protected void HandleArgs(string[] args)
        {

        }

        protected void Uninstall()
        {
            try
            {
                ServiceController.GetServices().First(sv => sv.ServiceName == Properties.Resources.MFVolumeService).Stop();
            }
            catch (Exception)
            {
                //ignore
            }
            var process = Process.Start(Properties.Resources.InstallUtil, 
                $"-u {Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe");
            process?.WaitForExit();
            var tmp = new DirectoryInfo(Properties.Resources.ProgramPath).GetFiles();
            foreach (var file in tmp)
            {
                file.Delete();
            }

            var dir = new DirectoryInfo(MFVolumeCtrl.Properties.Resources.ConfigPath);
            if (!dir.Exists) return;
            dir.Delete(true);
        }

        protected void Install()
        {
            var files = new DirectoryInfo(Environment.CurrentDirectory).GetFiles();
            foreach (var file in files)
            {
                file.CopyTo($"{Properties.Resources.ProgramPath}\\{file.Name}");
            }
            var process = Process.Start(Properties.Resources.InstallUtil, 
                $"{Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe");
            process?.WaitForExit();
            var dir = new DirectoryInfo(MFVolumeCtrl.Properties.Resources.ConfigPath);
            if (!dir.Exists) dir.Create();
            ServiceController.GetServices().First(sv => sv.ServiceName == Properties.Resources.MFVolumeService).Start();
        }

        public void Run()
        {
            if (Directory.Exists(Properties.Resources.ProgramPath)) Uninstall();
            Install();
        }
    }
}
