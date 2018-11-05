using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Interfaces;
using MFVolumeCtrl.Models;
using MFVolumeService.Controllers.Threads;
using System;
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
        protected ConfigModel Config;
        /// <summary>
        /// 
        /// </summary>
        protected IServiceThread Ticker { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected IServiceThread ServiceCtrl { get; set; }
        #endregion

        #region Initial
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public MfVolumeService()
        {
            InitializeComponent();
            Config = FileUtil.ImportObj<ConfigModel>($"{ConfigModel.ConfigPath}\\{ConfigModel.ConfigName}").GetAwaiter().GetResult();
            //Ticker = new TimeWatcher(ref Config);
        }
        #endregion

        #region BasicEvent
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                ServiceCtrl = new NetworkThread(Config);
                ServiceCtrl.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void OnStop()
        {
            ServiceCtrl.Interrupt();
        }
        #endregion
        public new void Dispose()
        {
            ServiceCtrl.Dispose();
            base.Dispose();
        }
    }
}
