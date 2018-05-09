using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFVolumeCtrl.Properties;
using Newtonsoft.Json;

namespace MFVolumeCtrl
{
    public static class ErrorCtrl
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
