using System;
using System.IO;
using System.Threading.Tasks;
using MFVolumeCtrl.Properties;
using Newtonsoft.Json;

namespace MFVolumeCtrl
{
    [Serializable]
    public class SettingsModel
    {
        public bool Enabled { get; set; }
        public int Interval { get; set; }
        public int Check { get; set; }
        public int Port { get; set; }
        public bool Activation { get; set; }
        public string KmsServer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task ReadAsync()
        {
            return ReadAsync(string.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task WriteAsync()
        {
            return WriteAsync(string.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task ReadAsync(string path)
        {
            return Task.Run(() =>
            {
                path = path == string.Empty ? $"{Resources.ConfigPath}\\{Resources.SettingFile}" : path;
                var json = File.ReadAllText(path);
                var settings = JsonConvert.DeserializeObject<SettingsModel>(json);
                Enabled = settings.Enabled;
                Interval = settings.Interval;
                Check = settings.Check;
                Port = settings.Port;
                Activation = settings.Activation;
                KmsServer = settings.KmsServer;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task WriteAsync(string path)
        {
            return Task.Run(() =>
            {
                path = path == string.Empty ? $"{Resources.ConfigPath}\\{Resources.SettingFile}" : path;
                var json = JsonConvert.SerializeObject(this);
                File.WriteAllText(path, json);
            });
        }
    }
}
