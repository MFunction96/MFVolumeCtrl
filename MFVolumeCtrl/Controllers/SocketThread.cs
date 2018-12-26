using MFVolumeCtrl.Models;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MFVolumeCtrl.Controllers
{
    #region SocketEventArgs

    public abstract class ConnectCallbackEventArgs : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public abstract class AcceptCallbackEventArgs : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public abstract class ReceiveEventArgs : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public abstract class ReceiveCallbackEventArgs : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public abstract class SendEventArgs : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public abstract class SendCallbackEventArgs : IDisposable
    {
        public void Dispose()
        {
        }
    }

    #endregion
    
    /// <inheritdoc />
    /// <summary>
    /// Socket线程。用于Socket相关任务线程，
    /// </summary>
    public abstract class SocketThread : ServiceThread
    {
        #region Properties
        /// <summary>
        /// Ipv4协议Socket。
        /// </summary>
        protected Socket Socketv4 { get; set; }
        /// <summary>
        /// Ipv6协议Socket。
        /// </summary>
        protected Socket Socketv6 { get; set; }
        /// <summary>
        /// 请求或反馈信息。
        /// </summary>
        public SocketMessage Message { get; set; }

        public event EventHandler<ConnectCallbackEventArgs> ConnectCallbackHandler;

        public event EventHandler<AcceptCallbackEventArgs> AcceptCallbackHandler;

        public event EventHandler<ReceiveEventArgs> ReceiveHandler;

        public event EventHandler<ReceiveCallbackEventArgs> ReceiveCallbackHandler;

        public event EventHandler<SendEventArgs> SendHandler;

        public event EventHandler<SendCallbackEventArgs> SendCallbackHandler;

        #endregion

        #region Construction

        /// <inheritdoc />
        /// <summary>
        /// Socket监听器默认构造函数。
        /// </summary>
        protected SocketThread()
        {
            Socketv4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socketv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            Message = new SocketMessage();
            MainThread.IsBackground = true;
        }

        #endregion

        #region Methods

        #region Implement



        #endregion

        #region Public



        #endregion

        #region Protected

        /// <summary>
        /// 初始化Socket监听器。
        /// 请在此方法内初始化Socket。
        /// </summary>
        protected abstract void Initialization();

        protected abstract void ConnectCallback(IAsyncResult ar);

        protected abstract void AcceptCallback(IAsyncResult ar);

        protected abstract void Receive(Socket client);

        protected abstract void ReceiveCallback(IAsyncResult ar);

        protected abstract void Send(Socket handler, SocketMessage message);

        protected abstract void SendCallback(IAsyncResult ar);

        #endregion

        #region Private



        #endregion

        #endregion
    }
}
