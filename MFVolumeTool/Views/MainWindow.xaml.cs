using System;
using System.Windows;
using MFVolumeCtrl.Models;

namespace MFVolumeTool.Views
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected ConfigModel Config;

        public MainWindow()
        {
            InitializeComponent();
            Config = new ConfigModel();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            /*Config.Write();
            MessageBox.Show(Properties.Resources.SuccessInfo);*/
        }

        private void BtnService_Click(object sender, RoutedEventArgs e)
        {
            var wdw = new ServiceWindow(Config.Services)
            {
                AcConfig = services => Config.Services = services
            };
            wdw.ShowDialog();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Config.Read();
            
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            var wdw = new ScheduleWindow(Config.Scripts)
            {
                AcScript = scripts => Config.Scripts = scripts
            };
            wdw.ShowDialog();
        }
    }
}
