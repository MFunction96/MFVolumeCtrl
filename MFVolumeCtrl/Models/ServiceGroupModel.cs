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
    public class ServiceGroupModel : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> Services { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ServiceGroupModel()
        {
            Services = new HashSet<string>();
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            Services.Clear();
        }
    }
}
