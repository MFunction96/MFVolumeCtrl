using MFVolumeCtrl.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MFVolumeTool
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow
    {
        /// <summary>
        /// 
        /// </summary>
        public Action<IList<ServiceModel>> AcConfig { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected IList<ServiceModel> Services { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        public ServiceWindow(IList<ServiceModel> services)
        {
            InitializeComponent();
            Services = services ?? new List<ServiceModel>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbService.Items.Clear();
            CbEnabled.IsEnabled = false;
            BtnAddService.IsEnabled = Services.Count != 0;
            try
            {
                var model = Services?.First(tmp => tmp.Nickname == CbGroup.SelectedItem.ToString());
                if (model is null) return;
                CbEnabled.IsChecked = model.Enabled;
                foreach (var service in model.Services)
                {
                    LbService.Items.Add(service);
                }

                if (LbService.Items.Count == 0) return;
                CbEnabled.IsEnabled = true;
                BtnModifyService.IsEnabled = true;
                BtnDeleteService.IsEnabled = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputWindow(string.Empty)
            {
                AcAddItem = str =>
                {
                    CbGroup.Items.Add(str);
                    CbEnabled.IsChecked = true;
                    BtnModifyGroup.IsEnabled = true;
                    BtnDeleteGroup.IsEnabled = true;
                    Services.Add(new ServiceModel
                    {
                        Nickname = str,
                        Enabled = true
                    });
                    CbGroup.SelectedItem = str;
                }
            };
            dialog.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifyGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputWindow(CbGroup.SelectionBoxItem.ToString())
            {
                AcAddItem = str =>
                {
                    var selected = Services.First(tmp => tmp.Nickname == CbGroup.SelectedValue.ToString());
                    var services = selected.Services;
                    Services.Remove(selected);
                    CbGroup.Items.Remove(selected.Nickname);

                    Services.Add(new ServiceModel
                    {
                        Nickname = str,
                        Enabled = CbEnabled.IsChecked ?? true,
                        Services = services
                    });
                    CbGroup.Items.Add(str);
                    CbGroup.SelectedItem = str;
                }
            };
            dialog.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            CbGroup.Items.RemoveAt(CbGroup.SelectedIndex);
            LbService.Items.Clear();
            if (!CbGroup.Items.IsEmpty)
            {
                CbGroup.SelectedIndex = 0;
                return;
            }
            CbEnabled.IsEnabled = false;
            CbEnabled.IsChecked = true;
            BtnModifyGroup.IsEnabled = false;
            BtnDeleteGroup.IsEnabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputWindow(string.Empty)
            {
                AcAddItem = str =>
                {
                    LbService.Items.Add(str);
                    BtnModifyService.IsEnabled = true;
                    BtnDeleteService.IsEnabled = true;
                }
            };
            dialog.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifyService_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputWindow(LbService.Items[LbService.SelectedIndex].ToString())
            {
                AcAddItem = str => LbService.Items[LbService.SelectedIndex] = str
            };
            dialog.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            LbService.Items.RemoveAt(LbService.SelectedIndex);
            if (!LbService.Items.IsEmpty) return;
            BtnModifyService.IsEnabled = false;
            BtnDeleteService.IsEnabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var service in Services)
            {
                CbGroup.Items.Add(service.Nickname);
            }

            CbGroup.Items.IsLiveSorting = true;
            LbService.Items.IsLiveSorting = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCommit_Click(object sender, RoutedEventArgs e)
        {
            AcConfig(Services);
            Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
