using System;
using System.Runtime.InteropServices;
using PInvoke.Structures;

namespace PInvoke
{
    public static class NativeMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hHandle"></param>
        /// <param name="dwMilliseconds"></param>
        /// <param name="bAlertable"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WaitForSingleObjectEx(
            IntPtr hHandle,
            int dwMilliseconds,
            bool bAlertable);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpApplicationName"></param>
        /// <param name="lpCommandLine"></param>
        /// <param name="lpProcessAttributes"></param>
        /// <param name="lpThreadAttributes"></param>
        /// <param name="bInheritHandles"></param>
        /// <param name="dwCreationFlags"></param>
        /// <param name="lpEnvironment"></param>
        /// <param name="lpCurrentDirectory"></param>
        /// <param name="lpStartupInfo"></param>
        /// <param name="lpProcessInformation"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateProcessW")]
        public static extern bool CreateProcess(
            [MarshalAs(UnmanagedType.LPWStr)] string lpApplicationName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            IntPtr lpEnvironment,
            [MarshalAs(UnmanagedType.LPWStr)] string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="length"></param>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, int length);
    }
}
