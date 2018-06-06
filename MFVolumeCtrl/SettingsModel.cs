using MFVolumeCtrl.Properties;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace MFVolumeCtrl
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [Serializable]
    public class SettingsModel : IConfig
    {
        public bool Enabled { get; set; }
        public int CountDown { get; set; }
        public int Port { get; set; }
        public bool Activation { get; set; }
        public string KmsServer { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Read()
        {
            Read(string.Empty);
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Write()
        {
            Write(string.Empty);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="filepath"></param>
        public void Create(string filepath)
        {
            var dir = filepath.Substring(0, filepath.LastIndexOf('\\'));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.Create(filepath);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void Read(string path)
        {
            if (path == string.Empty) path = Resources.ConfigPath;
            var filepath = $"{path}\\{Resources.SettingFile}";
            var settings = new SettingsModel();
            if (File.Exists(filepath))
            {
                var json = File.ReadAllText(filepath, Encoding.UTF8);
                settings = JsonConvert.DeserializeObject<SettingsModel>(json);
            }
            else
            {
                Create(filepath);
            }
            if (settings is null) settings = new SettingsModel();
            Enabled = settings.Enabled;
            CountDown = settings.CountDown;
            Port = settings.Port;
            Activation = settings.Activation;
            KmsServer = settings.KmsServer;
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void Write(string path)
        {
            path = path == string.Empty ? $"{Resources.ConfigPath}\\{Resources.SettingFile}" : path;
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, json, Encoding.UTF8);
        }
    }
}
