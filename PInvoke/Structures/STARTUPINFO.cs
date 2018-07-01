using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace PInvoke.Structures
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// 进程启动选项结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public int cb;
        /// <summary>
        /// 
        /// </summary>
        public string lpReserved;
        /// <summary>
        /// 
        /// </summary>
        public string lpDesktop;
        /// <summary>
        /// 
        /// </summary>
        public string lpTitle;
        /// <summary>
        /// 
        /// </summary>
        public int dwX;
        /// <summary>
        /// 
        /// </summary>
        public int dwY;
        /// <summary>
        /// 
        /// </summary>
        public int dwXSize;
        /// <summary>
        /// 
        /// </summary>
        public int dwYSize;
        /// <summary>
        /// 
        /// </summary>
        public int dwXCountChars;
        /// <summary>
        /// 
        /// </summary>
        public int dwYCountChars;
        /// <summary>
        /// 
        /// </summary>
        public int dwFillAttribute;
        /// <summary>
        /// 
        /// </summary>
        public int dwFlags;
        /// <summary>
        /// 
        /// </summary>
        public short wShowWindow;
        /// <summary>
        /// 
        /// </summary>
        public short cbReserved2;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr lpReserved2;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr hStdInput;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr hStdOutput;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr hStdError;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startupInfo"></param>
        public STARTUPINFO(STARTUPINFO startupInfo) : this()
        {
            cb = startupInfo.cb;
            cbReserved2 = startupInfo.cbReserved2;
            lpDesktop = startupInfo.lpDesktop;
            lpReserved = startupInfo.lpReserved;
            lpTitle = startupInfo.lpTitle;
            dwFillAttribute = startupInfo.dwFillAttribute;
            dwFlags = startupInfo.dwFlags;
            dwX = startupInfo.dwX;
            dwXCountChars = startupInfo.dwXCountChars;
            dwXSize = startupInfo.dwXSize;
            dwY = startupInfo.dwY;
            dwYCountChars = startupInfo.dwYCountChars;
            dwYSize = startupInfo.dwYSize;
            lpReserved2 = Marshal.AllocHGlobal(startupInfo.lpReserved2);
            NativeMethods.CopyMemory(lpReserved2, startupInfo.lpReserved2, Marshal.SizeOf(startupInfo.lpReserved2));
            hStdError = Marshal.AllocHGlobal(startupInfo.hStdError);
            NativeMethods.CopyMemory(hStdError, startupInfo.hStdError, Marshal.SizeOf(startupInfo.hStdError));
            hStdInput = Marshal.AllocHGlobal(startupInfo.hStdInput);
            NativeMethods.CopyMemory(hStdInput, startupInfo.hStdInput, Marshal.SizeOf(startupInfo.hStdInput));
            hStdOutput = Marshal.AllocHGlobal(startupInfo.hStdOutput);
            NativeMethods.CopyMemory(hStdOutput, startupInfo.hStdOutput, Marshal.SizeOf(startupInfo.hStdOutput));
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            if (lpReserved2 != IntPtr.Zero) Marshal.FreeHGlobal(lpReserved2);
            if (hStdError != IntPtr.Zero) Marshal.FreeHGlobal(hStdError);
            if (hStdInput != IntPtr.Zero) Marshal.FreeHGlobal(hStdInput);
            if (hStdOutput != IntPtr.Zero) Marshal.FreeHGlobal(hStdOutput);
        }
    }
}
