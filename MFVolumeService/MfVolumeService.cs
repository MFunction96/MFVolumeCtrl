using MFVolumeCtrl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

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
        protected IList<ServiceModel> Services { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected TcpListener Listener { get; set; }
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
            ConfigPath = MFVolumeCtrl.Properties.Resources.ConfigPath;
            ListenThread = new Thread(Listen);
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                Settings.ReadAsync();
                var port = Settings.Port;
                for (var i = 0; i < args.Length; i++)
                {
                    if (args[i] == Properties.Resources.ArgPort) port = int.Parse(args[++i]);
                }
                if (!Directory.Exists(ConfigPath)) Directory.CreateDirectory(ConfigPath);
                Listener = TcpListener.Create(port);
                Listener.Start();
                ListenThread.Start();
            }
            catch (Exception e)
            {
                ErrorCtrl.WriteError(e);
            }
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void OnStop()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public async void Listen()
        {
            for (; ; )
            {
                try
                {
                    var client = Listener.AcceptTcpClient();
                    await Receive(client.GetStream());
                    client.Close();
                }
                catch (Exception e)
                {
                    await ErrorCtrl.WriteError(e);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected async Task Receive(NetworkStream stream)
        {
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);
            var ptr = Marshal.AllocHGlobal((int)stream.Length);
            Marshal.Copy(buffer, 0, ptr, buffer.Length);
            Services = Marshal.PtrToStructure<List<ServiceModel>>(ptr);
            Marshal.FreeHGlobal(ptr);
        }
    }
}
