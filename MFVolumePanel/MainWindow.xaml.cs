using MFVolumeCtrl;
using System.Windows;

namespace MFVolumePanel
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ConfigModel Config { get; protected set; }

        public MainWindow()
        {
            InitializeComponent();
            Config = new ConfigModel();
        }

        private void TogBtn_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Config.Read();
        }
    }
}
