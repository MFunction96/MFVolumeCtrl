using MFVolumeCtrl.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MFVolumeCtrl
{
    public class ConfigModel : SettingsModel
    {
        public IList<ServiceModel> Services { get; protected set; }

        public ConfigModel()
        {
            Services = new List<ServiceModel>();
        }

        public override void Read(string path)
        {
            base.Read(path);
            path = path == string.Empty ? $"{Resources.ConfigPath}\\{Resources.ServiceFile}" : path;
            var json = File.ReadAllText(path);
            Services = JsonConvert.DeserializeObject<List<ServiceModel>>(json);
        }

        public override void Write(string path)
        {
            base.Write(path);
            path = path == string.Empty ? $"{Resources.ConfigPath}\\{Resources.ServiceFile}" : path;
            var json = JsonConvert.SerializeObject(Services);
            File.WriteAllText(path, json, Encoding.UTF8);
        }
    }
}
