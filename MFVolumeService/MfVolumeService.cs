using MFVolumeCtrl.Interfaces;
using MFVolumeCtrl.Models;
using MFVolumeCtrl.Properties;
using MFVolumeService.Controllers;
using System.ServiceProcess;

namespace MFVolumeService
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public partial class MfVolumeService : ServiceBase
    {
        #region Proporties
        /// <summary>
        /// 
        /// </summary>
        protected string ConfigPath { get; }

        /// <summary>
        /// 
        /// </summary>
        protected ConfigModel Config;
        /// <summary>
        /// 
        /// </summary>
        protected IServiceThread Ticker { get; set; }
        #endregion

        #region Initial
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public MfVolumeService()
        {
            InitializeComponent();
            ConfigPath = Resources.ConfigPath;
            Ticker = new TimeWatcher(ref Config);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected void HandleArgs(string[] args)
        {
            /*var port = Config.Port;
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] == Properties.Resources.ArgPort) port = int.Parse(args[++i]);
            }*/
            /*Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Parse(Resources.Localhost), port));*/
        }
        #endregion

        #region BasicEvent
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            /*try
            {
                Config.Read();
                HandleArgs(args);
                Config.Initialize();
                ListenThread.Start();
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e);
            }*/
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void OnStop()
        {
            //ListenThread.Interrupt();
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void Listen()
        {
            /*while (Config.Enabled)
            {
                try
                {
                    var client = Listener.Accept();
                    var service = ServiceModel.Receive(client);
                    service.SetStatus(client, Config.CountDown, service.Enabled);
                    client.Close();
                }
                catch (Exception e)
                {
                    ErrorUtil.WriteError(e);
                }
            }*/
        }

        public new void Dispose()
        {
            Ticker.Dispose();
            base.Dispose();
        }
    }
}
