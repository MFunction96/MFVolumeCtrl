using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace PInvoke.Structures
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// 进程信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public IntPtr hProcess;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr hThread;
        /// <summary>
        /// 
        /// </summary>
        public int ProcessId;
        /// <summary>
        /// 
        /// </summary>
        public int ThreadId;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processInformation"></param>
        public PROCESS_INFORMATION(PROCESS_INFORMATION processInformation) : this()
        {
            ProcessId = processInformation.ProcessId;
            ThreadId = processInformation.ThreadId;
            hProcess = Marshal.AllocHGlobal(processInformation.hProcess);
            NativeMethods.CopyMemory(hProcess,processInformation.hProcess,Marshal.SizeOf(hProcess));
            hThread = Marshal.AllocHGlobal(processInformation.hThread);
            NativeMethods.CopyMemory(hThread, processInformation.hThread, Marshal.SizeOf(hThread));
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            if (hProcess != IntPtr.Zero) Marshal.FreeHGlobal(hProcess);
            if (hThread != IntPtr.Zero) Marshal.FreeHGlobal(hThread);
        }
    }
}
