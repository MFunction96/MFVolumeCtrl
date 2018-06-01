using MFVolumeCtrl;
using MFVolumeCtrl.Properties;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading;

namespace MFVolumeService
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public partial class MfVolumeService : ServiceBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected string ConfigPath { get; }
        /// <summary>
        /// 
        /// </summary>
        protected SettingsModel Settings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected ServiceModel ServiceGroup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected Socket Listener { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected Thread ListenThread { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public MfVolumeService()
        {
            InitializeComponent();
            ConfigPath = Resources.ConfigPath;
            ListenThread = new Thread(Listen)
            {
                IsBackground = true
            };
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                Settings.Read();
                var port = Settings.Port;
                for (var i = 0; i < args.Length; i++)
                {
                    if (args[i] == Properties.Resources.ArgPort) port = int.Parse(args[++i]);
                }
                if (!Directory.Exists(ConfigPath)) Directory.CreateDirectory(ConfigPath);
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Listener.Bind(new IPEndPoint(IPAddress.Parse(Resources.Localhost), port));
                ListenThread.Start();
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e);
            }
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void OnStop()
        {
            ListenThread.Interrupt();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Listen()
        {
            while (Settings.Enabled)
            {
                try
                {
                    var client = Listener.Accept();
                    ServiceGroup = ServiceModel.Receive(client);
                    ServiceGroup.SetStatus(Settings);
                    ServiceGroup.Send(client);
                    client.Close();
                }
                catch (Exception e)
                {
                    ErrorUtil.WriteError(e);
                }
            }
        }
    }
}
