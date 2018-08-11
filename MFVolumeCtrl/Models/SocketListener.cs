using System.Net.Sockets;

namespace MFVolumeCtrl.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SocketListener : ServiceThread
    {
        /// <summary>
        /// 
        /// </summary>
        protected Socket Socketv4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected Socket Socketv6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SocketListener()
        {
            Socketv4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4);
            Socketv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.IPv6);
        }
    }
}
