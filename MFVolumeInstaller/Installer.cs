using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using PInvoke.Methods;

namespace MFVolumeInstaller
{
    /// <summary>
    /// 
    /// </summary>
    public class Installer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public Installer(string[] args)
        {
            HandleArgs(args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected void HandleArgs(string[] args)
        {

        }
        /// <summary>
        /// 
        /// </summary>
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
            var process =
                Process.Start($"{Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe",
                    "-uninstall");
            process?.WaitForExit();
            //MFVolumeService.Program.Install(false, null);
            /*var process = Process.Start(Properties.Resources.InstallUtil, 
                $"-u {Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe");
            process?.WaitForExit();*/
            var tmp = new DirectoryInfo(Properties.Resources.ProgramPath).GetFiles();
            foreach (var file in tmp)
            {
                file.Delete();
            }

            var dir = new DirectoryInfo(MFVolumeCtrl.Properties.Resources.ConfigPath);
            if (!dir.Exists) return;
            dir.Delete(true);
        }
        /// <summary>
        /// 
        /// </summary>
        protected void Install()
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory);
            var des = new DirectoryInfo(Properties.Resources.ProgramPath);
            if (!des.Exists)
            {
                des.Create();
                Console.Out.WriteLineAsync($"Creating folder {des.Name}");
            }
            foreach (var file in dir.GetFiles())
            {
                file.CopyTo($"{Properties.Resources.ProgramPath}\\{file.Name}");
                Console.Out.WriteLineAsync($"Copy {file.Name} to {des.Name}");
            }

            dir = new DirectoryInfo(MFVolumeCtrl.Properties.Resources.ConfigPath);
            if (!dir.Exists) dir.Create();

            //MFVolumeService.Program.Install(true, null);

            var process = ProcessCtrl.CreateProcessEx($"{Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe",
                    "-install");
            

            /*var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Properties.Resources.InstallUtil,
                    Arguments = $"{Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe",
                    UseShellExecute = true,
                    Verb = "runas"
                }
            };
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var ti = new TransactedInstaller();
            ti.Installers.Add(new ProjectInstaller());*/
            ServiceController.GetServices().First(sv => sv.ServiceName == Properties.Resources.MFVolumeService).Start();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            if (Directory.Exists(Properties.Resources.ProgramPath)) Uninstall();
            Install();
        }
    }
}
