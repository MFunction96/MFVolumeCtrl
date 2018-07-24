using System;
using System.Collections.Generic;
using System.IO;
using MFVolumeCtrl.Interfaces;

namespace MFVolumeCtrl.Models.Script
{
    [Serializable]
    public class ScriptModel : IConfig, IDisposable
    {
        public IList<ScheduleModel> Scheduled { get; set; }

        public ScriptModel()
        {
            Scheduled = new List<ScheduleModel>();
        }

        public void Run()
        {
            foreach (var task in Scheduled)
            {
                
            }
        }

        public void Create(string filepath)
        {
            try
            {
                var path = filepath.Substring(0, filepath.LastIndexOf('\\'));
                var dir = new DirectoryInfo(path);
                if (!dir.Exists) dir.Create();
                dir.Attributes = FileAttributes.System | FileAttributes.Directory | FileAttributes.Hidden;
                File.Create(filepath);
            }
            catch (Exception)
            {
                var config = new ConfigModel();
                config.Read();
                config.RunScript = false;
                config.Write();
            }
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public void Write()
        {
            throw new NotImplementedException();
        }

        public void Read(string path)
        {
            throw new NotImplementedException();
        }

        public void Write(string path)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
