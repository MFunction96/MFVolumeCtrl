using System;
using System.Collections.Generic;

namespace MFVolumeCtrl.Models
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class ConfigModel
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
        public ICollection<ServiceModel> Services { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<ScriptModel> Scripts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConfigModel()
        {
            Services = new HashSet<ServiceModel>();
            Scripts = new HashSet<ScriptModel>();
        }
    }
}
