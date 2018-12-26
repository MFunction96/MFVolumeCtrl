using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MFVolumeCtrl.Models
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket WorkSocket;
        // Size of receive buffer.  
        public int BufferSize;
        // Receive buffer.  
        public byte[] Buffer;
        /// <summary>
        /// 
        /// </summary>
        public int ReceiveSize;

        public StateObject(Socket socket, ConfigModel config)
        {
            WorkSocket = socket;
            BufferSize = config.BufferSize;
            Buffer = new byte[BufferSize];
            ReceiveSize = 0;
        }
    }

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
