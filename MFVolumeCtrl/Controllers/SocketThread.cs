﻿using System.Net.Sockets;

namespace MFVolumeCtrl.Controllers
{
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
        #endregion

        #region Construction

        /// <inheritdoc />
        /// <summary>
        /// Socket监听器默认构造函数。
        /// </summary>
        protected SocketThread()
        {
            Socketv4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4);
            Socketv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.IPv6);
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

        #endregion

        #region Private



        #endregion

        #endregion
    }
}