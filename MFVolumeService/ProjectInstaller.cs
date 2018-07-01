using System.ComponentModel;
using System.Configuration.Install;

namespace MFVolumeService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
