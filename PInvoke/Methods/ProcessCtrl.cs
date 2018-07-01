﻿using System;
using System.Runtime.InteropServices;
using PInvoke.Structures;

namespace PInvoke.Methods
{
    /// <summary>
    /// 进程控制器，用于启动或终止进程。
    /// </summary>
    public static class ProcessCtrl
    {
        /// <summary>
        /// 以当前进程权限运行新程序。
        /// </summary>
        /// <param name="appName">
        /// 应用程序名称。
        /// </param>
        /// <param name="cmdLine">
        /// 程序运行命令行。
        /// </param>
        /// <param name="si">
        /// 程序启动信息及状态。
        /// </param>
        /// <param name="waitforExit"></param>
        /// <returns>
        /// 新进程的运行信息。
        /// </returns>
        public static PROCESS_INFORMATION CreateProcessEx(string appName = null, string cmdLine = null, STARTUPINFO si = new STARTUPINFO(), bool waitforExit = false)
        {
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
            var cmd = cmdLine != null ? string.Copy(cmdLine) : string.Empty;
            if (NativeMethods.CreateProcess(appName, cmd, ptrsap, ptrsat, true, 0, IntPtr.Zero, null,
                    ref si, out var pi))
            {
                return pi;
            }
            throw new Exception($"Error!\nCode: {NativeMethods.GetLastError()}");
        }
    }
}
