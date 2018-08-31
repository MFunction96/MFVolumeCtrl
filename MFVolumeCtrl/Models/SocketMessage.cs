using MFVolumeCtrl.Controllers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MFVolumeCtrl.Models
{
    public enum MessageType : byte
    {
        ConfigMsg = 0,
        ServiceMsg = 1,
        ScriptMsg = 2,
        ImageMsg = 3,
        ResponseMsg = 4
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class SocketHeader : IDisposable
    {
        public MessageType MessageType { get; set; }

        public Type BodyType { get; set; }

        public void Dispose()
        {
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class SocketMessage : IDisposable
    {
        public SocketHeader Headers { get; set; }

        public object Body { get; set; }

        public SocketMessage()
        {
            Headers = new SocketHeader();
        }

        public void Dispose()
        {
        }
    }
}
