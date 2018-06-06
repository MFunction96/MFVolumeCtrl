using System;
using MFVolumeCtrl.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MFVolumeCtrl
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class ConfigModel : IConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<ServiceModel> Services { get; set; }

        public SettingsModel Settings { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public ConfigModel()
        {
            Services = new List<ServiceModel>();
            Settings = new SettingsModel();
        }
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
        public void Read(string path)
        {
            try
            {
                Settings.Read();
                if (path == string.Empty) path = Resources.ConfigPath;
                var filepath = $"{path}\\{Resources.ServiceFile}";
                if (File.Exists(filepath))
                {
                    var json = File.ReadAllText(filepath, Encoding.UTF8);
                    Services = JsonConvert.DeserializeObject<List<ServiceModel>>(json);
                }
                else
                {
                    Create(filepath);
                }
                if (Services is null) Services = new List<ServiceModel>();
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
        /// <param name="path"></param>
        public void Write(string path)
        {
            if (path == string.Empty) path = Resources.ConfigPath;
            var filepath = $"{path}\\{Resources.ServiceFile}";
            var json = JsonConvert.SerializeObject(Services, Formatting.Indented);
            File.WriteAllText(filepath, json, Encoding.UTF8);
        }
    }
}
