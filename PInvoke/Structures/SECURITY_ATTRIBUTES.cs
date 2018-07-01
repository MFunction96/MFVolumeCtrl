using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace PInvoke.Structures
{
    /// <summary>
    /// 安全标识符结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES : IDisposable
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;

        public SECURITY_ATTRIBUTES(SECURITY_ATTRIBUTES securityAttributes) : this()
        {
            nLength = securityAttributes.nLength;
            bInheritHandle = securityAttributes.bInheritHandle;
            lpSecurityDescriptor = Marshal.AllocHGlobal(securityAttributes.lpSecurityDescriptor);

        }

        public void Dispose()
        {
            if (lpSecurityDescriptor != IntPtr.Zero) Marshal.FreeHGlobal(lpSecurityDescriptor);
        }
    }
}
