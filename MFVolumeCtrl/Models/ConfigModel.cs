using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MFVolumeCtrl.Interfaces;
using MFVolumeCtrl.Models.Service;
using MFVolumeCtrl.Properties;
using Newtonsoft.Json;

namespace MFVolumeCtrl.Models
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [Serializable]
    public class ConfigModel : IConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CountDown { get; set; }
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
        public IList<ServiceModel> Services { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConfigModel()
        {
            Services = new List<ServiceModel>();
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
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (var service in Services)
            {
                service.SetStatus(service.Enabled);
            }
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
                if (path == string.Empty) path = Resources.ConfigPath;
                var filepath = $"{path}\\{Resources.ConfigFile}";
                var config = new ConfigModel();
                if (File.Exists(filepath))
                {
                    var json = File.ReadAllText(filepath, Encoding.UTF8);
                    config = JsonConvert.DeserializeObject<ConfigModel>(json);
                }
                else
                {
                    Create(filepath);
                }
                if (config is null) config = new ConfigModel();
                Enabled = config.Enabled;
                Activation = config.Activation;
                CountDown = config.CountDown;
                KmsServer = config.KmsServer;
                Port = config.Port;
                RunScript = config.RunScript;
                Services = config.Services;
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
            var filepath = $"{path}\\{Resources.ConfigFile}";
            var json = JsonConvert.SerializeObject(Services, Formatting.Indented);
            File.WriteAllText(filepath, json, Encoding.UTF8);
        }
    }
}
