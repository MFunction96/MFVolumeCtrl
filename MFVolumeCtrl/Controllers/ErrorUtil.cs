using System;
using System.Threading.Tasks;
using MFVolumeCtrl.Properties;

namespace MFVolumeCtrl.Controllers
{
    public static class ErrorUtil
    {
        public static async Task WriteError(Exception e)
        {
            var path = $"{Resources.ConfigPath}\\{Resources.ErrorFile}";
            await FileUtil.ExportObj(e, path, true);
        }
    }
}
