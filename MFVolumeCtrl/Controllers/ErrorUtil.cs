﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MFVolumeCtrl.Properties;
using Newtonsoft.Json;

namespace MFVolumeCtrl.Controllers
{
    public static class ErrorUtil
    {
        public static Task WriteError(Exception e)
        {
            return Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(e, Formatting.Indented);
                if (File.Exists($"{Resources.ConfigPath}\\{Resources.ErrorFile}")) json = $",\n{json}";
                File.AppendAllText($"{Resources.ConfigPath}\\{Resources.ErrorFile}", json, Encoding.UTF8);
            });
        }
    }
}