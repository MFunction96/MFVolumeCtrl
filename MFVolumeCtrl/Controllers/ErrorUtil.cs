using MFVolumeCtrl.Properties;
using System;
using System.Threading.Tasks;

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
