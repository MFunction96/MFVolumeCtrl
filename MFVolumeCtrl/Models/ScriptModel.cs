using System;
using System.Diagnostics;

namespace MFVolumeCtrl.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum ScheduleInterval
    {
        OnStart = 0,
        Daily = 1,
        Monthly = 2,
        Once = 3
    }
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [Serializable]
    public class ScriptModel : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Activated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool NoWindow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool WaitforExit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Silent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Lock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Restart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileFullPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Arguments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScheduleInterval Interval { get; set; }
        /// <summary>
        /// 
        /// </summary>
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

        public void Dispose()
        {
        }
    }
}
