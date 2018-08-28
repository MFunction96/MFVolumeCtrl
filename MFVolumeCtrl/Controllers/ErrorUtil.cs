using MFVolumeCtrl.Properties;
using System;
using System.Threading.Tasks;

namespace MFVolumeCtrl.Controllers
{
    public static class ErrorUtil
    {
        public static async Task WriteError(Exception e)
        {
            const string path = @"C:\ProgramData\MFVolumeCtrl\\error.log";
            await FileUtil.ExportObj(e, path, true);
        }
    }
}
