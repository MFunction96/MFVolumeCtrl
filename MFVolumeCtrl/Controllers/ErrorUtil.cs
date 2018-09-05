using MFVolumeCtrl.Models;
using System;
using System.Threading.Tasks;

namespace MFVolumeCtrl.Controllers
{
    public static class ErrorUtil
    {
        public static async Task WriteError(Exception e)
        {
            var path = $"{ConfigModel.ConfigPath}\\{ConfigModel.ErrorName}";
            await FileUtil.ExportObj(e, path, true);
        }
    }
}
