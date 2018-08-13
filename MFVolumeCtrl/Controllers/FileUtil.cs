using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace MFVolumeCtrl.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <param name="append"></param>
        /// <param name="formatString"></param>
        /// <returns></returns>
        public static Task ExportObj(object obj, string filePath, bool append = false, string formatString = ",\r\n")
        {
            return Task.Run(() =>
            {
                var fileinfo = new FileInfo(filePath);
                if (fileinfo.Directory != null && !fileinfo.Directory.Exists) fileinfo.Directory?.Create();

                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                if (append)
                {
                    if (File.Exists(filePath)) json = formatString + json;
                    File.AppendAllText(filePath, json);
                }
                else File.WriteAllText(filePath, json);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Task<T> ImportObj<T>(string filePath)
        {
            return Task.Run(() =>
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            });
        }
    }
}
