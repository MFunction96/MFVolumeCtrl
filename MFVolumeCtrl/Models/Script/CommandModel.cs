using System;
using System.Diagnostics;

namespace MFVolumeCtrl.Models.Script
{
    [Serializable]
    public class CommandModel
    {
        public bool Activated { get; set; }
        public bool NoWindow { get; set; }
        public bool WaitforExit { get; set; }
        public int Restart { get; set; }
        public string FileFullPath { get; set; }
        public string Arguments { get; set; }

        public void Run()
        {
            if (Restart > 0)
            {
                --Restart;
                return;
            }
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FileFullPath,
                    Arguments = Arguments,
                    CreateNoWindow = NoWindow
                }
            };
            process.Start();
            if (WaitforExit) process.WaitForExit();
        }
    }
}
