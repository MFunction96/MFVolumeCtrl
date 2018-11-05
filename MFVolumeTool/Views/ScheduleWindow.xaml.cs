using System;
using System.Collections.Generic;
using System.Windows;
using MFVolumeCtrl.Models;

namespace MFVolumeTool.Views
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        protected ICollection<ScriptModel> Scripts;

        

        public Action<ICollection<ScriptModel>> AcScript { get; set; }

        public ScheduleWindow(ICollection<ScriptModel> scripts)
        {
            InitializeComponent();
            Scripts = scripts;
        }

        private void BtnEnterShadow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExitShadow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
