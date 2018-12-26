using System;
using System.ComponentModel;
using System.Windows;

namespace MFVolumePanel
{
    /// <summary>
    /// Interaction logic for BackgroundWorkWindow.xaml
    /// </summary>
    public partial class BackgroundWorkWindow : Window
    {
        public BackgroundWorkWindow()
        {
            InitializeComponent();
        }

        public void SetMessage(string msg)
        {
            LbProcess.Content = msg;
        }
    }
}
