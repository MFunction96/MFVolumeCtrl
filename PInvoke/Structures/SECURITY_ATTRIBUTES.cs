using System;
using System.Runtime.InteropServices;
using PInvoke.Methods;

// ReSharper disable InconsistentNaming

namespace PInvoke.Structures
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// 安全标识符结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public int nLength;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr lpSecurityDescriptor;
        /// <summary>
        /// 
        /// </summary>
        public bool bInheritHandle;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityAttributes"></param>
        public SECURITY_ATTRIBUTES(SECURITY_ATTRIBUTES securityAttributes) : this()
        {
            nLength = securityAttributes.nLength;
            bInheritHandle = securityAttributes.bInheritHandle;
            lpSecurityDescriptor = MemoryCtrl.CopyMemoryEx(securityAttributes.lpSecurityDescriptor);
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            MemoryCtrl.FreeMemoryEx(lpSecurityDescriptor);
        }
    }
}
