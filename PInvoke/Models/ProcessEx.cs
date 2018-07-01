using System;
using System.Runtime.InteropServices;
using PInvoke.Methods;
using PInvoke.Structures;

namespace PInvoke.Models
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class ProcessEx : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Arguments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected STARTUPINFO Startupinfo;
        /// <summary>
        /// 
        /// </summary>
        public STARTUPINFO StartupInfo
        {
            get => Startupinfo;
            set => Startupinfo = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public PROCESS_INFORMATION ProcessInformation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ProcessEx()
        {
            StartupInfo = new STARTUPINFO();
            ProcessInformation = new PROCESS_INFORMATION();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (string.IsNullOrEmpty(AppPath)) throw new InvalidOperationException();
            var cmd = $"{AppPath} {Arguments}";
            var sap = new SECURITY_ATTRIBUTES
            {
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };
            sap.nLength = Marshal.SizeOf(sap);
            var ptrsap = MemoryCtrl.CopyMemoryEx(sap);
            var sat = new SECURITY_ATTRIBUTES
            {
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };
            sat.nLength = Marshal.SizeOf(sat);
            var ptrsat = MemoryCtrl.CopyMemoryEx(sat);
            if (!NativeMethods.CreateProcess(null, cmd, ptrsap, ptrsat, true, 0, IntPtr.Zero, null,
                ref Startupinfo, out var pi))
                throw new Exception($"Error!\nCode: {NativeMethods.GetLastError()}");

            ProcessInformation = pi;
            MemoryCtrl.FreeMemoryEx(ptrsat);
            MemoryCtrl.FreeMemoryEx(ptrsap);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliSeconds"></param>
        public void WaitforExit(int milliSeconds)
        {
            NativeMethods.WaitForSingleObjectEx(ProcessInformation.hProcess, 0, false);
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            Startupinfo.Dispose();
            ProcessInformation.Dispose();
        }
    }
}
