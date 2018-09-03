using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MFVolumeCtrl.Models
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class ConfigModel : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Activation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool RunScript { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string KmsServer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PendingQueue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<ServiceGroupModel> Services { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<ScriptModel> Scripts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConfigModel()
        {
            Services = new HashSet<ServiceGroupModel>();
            Scripts = new HashSet<ScriptModel>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configModel"></param>
        public ConfigModel(ConfigModel configModel)
        {
            Enabled = configModel.Enabled;
            Port = configModel.Port;
            Activation = configModel.Activation;
            RunScript = configModel.RunScript;
            KmsServer = configModel.KmsServer;
            PendingQueue = configModel.PendingQueue;
            Services = new HashSet<ServiceGroupModel>(configModel.Services);
            Scripts = new HashSet<ScriptModel>(configModel.Scripts);
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {

        }
    }
}
