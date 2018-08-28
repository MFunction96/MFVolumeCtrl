using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;

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
            /*try
            {
                ServiceController.GetServices().First(sv => sv.ServiceName == Properties.Resources.MFVolumeService).Stop();
            }
            catch (Exception)
            {
                //ignore
            }
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Properties.Resources.InstallUtil,
                    Arguments = $"-u \"{Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe\"",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            string line = null;
            while (!process.StandardOutput.EndOfStream)
            {
                line += process.StandardOutput.ReadLine() + Environment.NewLine;
            }
            Console.WriteLine(line);
            process.WaitForExit();
            
            var tmp = new DirectoryInfo(Properties.Resources.ProgramPath).GetFiles();
            foreach (var file in tmp)
            {
                file.Delete();
            }

            var dir = new DirectoryInfo(MFVolumeCtrl.Properties.Resources.ConfigPath);
            if (!dir.Exists) return;
            dir.Delete(true);*/
        }
        /// <summary>
        /// 
        /// </summary>
        protected void Install()
        {
            /*var dir = new DirectoryInfo(Environment.CurrentDirectory);
            var des = new DirectoryInfo(Properties.Resources.ProgramPath);
            if (!des.Exists)
            {
                des.Create();
                Console.WriteLine($@"Creating folder {des.Name}");
            }
            foreach (var file in dir.GetFiles())
            {
                file.CopyTo($"{Properties.Resources.ProgramPath}\\{file.Name}");
                Console.WriteLine($@"Copy {file.Name} to {des.Name}");
            }

            dir = new DirectoryInfo(MFVolumeCtrl.Properties.Resources.ConfigPath);
            if (!dir.Exists) dir.Create();

            //MFVolumeService.Program.Install(true, null);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Properties.Resources.InstallUtil,
                    Arguments = $"\"{Properties.Resources.ProgramPath}\\{Properties.Resources.MFVolumeService}.exe\"",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
                
            };
            process.Start();
            string line = null;
            while (!process.StandardOutput.EndOfStream)
            {
                line += process.StandardOutput.ReadLine() + Environment.NewLine;
            }
            Console.WriteLine(line);
            process.WaitForExit();
            ServiceController.GetServices().First(sv => sv.ServiceName == Properties.Resources.MFVolumeService).Start();*/
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
