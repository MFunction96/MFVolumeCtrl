using System;
using MFVolumeCtrl.Interfaces;
using MFVolumeCtrl.Models;
using MFVolumeCtrl.Properties;
using MFVolumeService.Controllers;
using System.ServiceProcess;
using MFVolumeCtrl.Controllers;

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
            Config = FileUtil.ImportObj<ConfigModel>(@"C:\ProgramData\MFVolumeCtrl\config.json").GetAwaiter().GetResult();
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
                ServiceCtrl = new ServiceOperator(ref Config);
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
