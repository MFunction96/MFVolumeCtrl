using System;
using System.Runtime.InteropServices;

namespace PInvoke.Methods
{
    public class MemoryCtrl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IntPtr CopyMemoryEx<TType>(TType source)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(source));
            Marshal.StructureToPtr(source, ptr, false);
            return ptr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IntPtr CopyMemoryEx(IntPtr source)
        {
            if (source == IntPtr.Zero) return IntPtr.Zero;
            var ptr = Marshal.AllocHGlobal(source);
            NativeMethods.CopyMemory(ptr, source, Marshal.SizeOf(source));
            return ptr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intPtr"></param>
        /// <returns></returns>
        public static bool FreeMemoryEx(IntPtr intPtr)
        {
            if (intPtr == IntPtr.Zero) return false;
            Marshal.FreeHGlobal(intPtr);
            return true;
        }
    }
}
