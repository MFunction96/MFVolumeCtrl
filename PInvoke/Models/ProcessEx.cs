using System;
using System.Runtime.InteropServices;
using PInvoke.Structures;

namespace PInvoke.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessEx
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
            Start(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wait"></param>
        public void Start(bool wait)
        {
            if (string.IsNullOrEmpty(AppPath)) throw new InvalidOperationException();
            var cmd = $"{AppPath} {Arguments}";
            var sap = new SECURITY_ATTRIBUTES
            {
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };
            sap.nLength = Marshal.SizeOf(sap);
            var ptrsap = Marshal.AllocHGlobal(sap.nLength);
            Marshal.StructureToPtr(sap, ptrsap, false);
            var sat = new SECURITY_ATTRIBUTES
            {
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };
            sat.nLength = Marshal.SizeOf(sat);
            var ptrsat = Marshal.AllocHGlobal(sat.nLength);
            Marshal.StructureToPtr(sat, ptrsat, false);
            if (!NativeMethods.CreateProcess(null, cmd, ptrsap, ptrsat, true, 0, IntPtr.Zero, null,
                ref Startupinfo, out var pi))
                throw new Exception($"Error!\nCode: {NativeMethods.GetLastError()}");

            ProcessInformation = pi;
        }
    }
}
