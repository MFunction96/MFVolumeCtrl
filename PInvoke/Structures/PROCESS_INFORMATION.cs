using System;
using System.Runtime.InteropServices;
using PInvoke.Methods;

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
            hProcess = MemoryCtrl.CopyMemoryEx(processInformation.hProcess);
            hThread = MemoryCtrl.CopyMemoryEx(processInformation.hThread);
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            MemoryCtrl.FreeMemoryEx(hProcess);
            MemoryCtrl.FreeMemoryEx(hThread);
        }
    }
}
