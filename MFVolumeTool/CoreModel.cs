using MFVolumeCtrl;

namespace MFVolumeTool
{
    public class CoreModel
    {
        public ConfigModel Config { get; set; }

        public CoreModel()
        {
            Config.Read();
        }
    }
}
