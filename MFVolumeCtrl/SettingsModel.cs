using MFVolumeCtrl.Properties;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace MFVolumeCtrl
{
    [Serializable]
    public class SettingsModel : IConfig
    {
        public bool Enabled { get; set; }
        public int Interval { get; set; }
        public int Check { get; set; }
        public int Port { get; set; }
        public bool Activation { get; set; }
        public string KmsServer { get; set; }

        public void Read()
        {

        }

        public void Write()
        {

        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual void Read(string path)
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
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual void Write(string path)
        {
            path = path == string.Empty ? $"{Resources.ConfigPath}\\{Resources.SettingFile}" : path;
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, json, Encoding.UTF8);
        }
    }
}
