using PInvoke.Methods;
using PInvoke.Structures;
using System;
using System.Runtime.InteropServices;

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
        public STARTUPINFO StartupInfo;

        /// <summary>
        /// 
        /// </summary>
        public PROCESS_INFORMATION ProcessInformation;
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
        public void Start(int milliSeconds = 0)
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
                ref StartupInfo, out ProcessInformation))
                throw new Exception($"Error!\nCode: {NativeMethods.GetLastError()}");

            MemoryCtrl.FreeMemoryEx(ptrsat);
            MemoryCtrl.FreeMemoryEx(ptrsap);

            if (milliSeconds > 0) WaitForExit(milliSeconds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliSeconds"></param>
        protected void WaitForExit(int milliSeconds)
        {
            NativeMethods.WaitForSingleObjectEx(ProcessInformation.hProcess, 0, false);
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            StartupInfo.Dispose();
            ProcessInformation.Dispose();
        }
    }
}
