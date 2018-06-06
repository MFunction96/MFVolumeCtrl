using MFVolumeCtrl;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MFVolumeTool
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        public Action<IList<ServiceModel>> AcConfig { get; set; }

        protected IList<ServiceModel> Services { get; }

        public ServiceWindow(IList<ServiceModel> services)
        {
            InitializeComponent();
            Services = services ?? new List<ServiceModel>();
        }

        private void CbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnModifyGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnModifyService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCommit_Click(object sender, RoutedEventArgs e)
        {
            AcConfig(Services);
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
