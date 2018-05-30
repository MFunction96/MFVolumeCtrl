using MFVolumeCtrl.Properties;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MFVolumeCtrl
{
    public static class ErrorUtil
    {
        public static Task WriteError(Exception e)
        {
            return Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(e);
                File.AppendAllText($"{Resources.ConfigPath}\\{Resources.ErrorFile}", json);
            });
        }
    }
}
