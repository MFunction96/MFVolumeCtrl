using System.Windows;
using MFVolumeCtrl;

namespace MFVolumeTool
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ConfigModel Config { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnService_Click(object sender, RoutedEventArgs e)
        {
            var wdw = new ServiceWindow();
            wdw.ShowDialog();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
