using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using System;
using System.Linq;
using System.ServiceProcess;
using System.Windows;

namespace MFVolumePanel
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ConfigModel Config { get; }

        public SocketThread SocketService { get; protected set; }

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Config = FileUtil
                    .ImportObj<ConfigModel>($"{ConfigModel.ConfigPath}\\{ConfigModel.ConfigName}")
                    .GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var services = ServiceController.GetServices();
                foreach (var service in Config.Services)
                {
                    CbGroup.Items.Add(service.Nickname);
                    var flag = true;
                    foreach (var serviceName in service.Services)
                    {
                        var serv = services.FirstOrDefault(tmp => tmp.ServiceName == serviceName);
                        if (serv == null) throw new NullReferenceException();
                        if (serv.Status == ServiceControllerStatus.Running) continue;
                        flag = false;
                        break;
                    }

                    service.Enabled = flag;
                }

                LblServiceStatus.Content += Config.Enabled ? "启用" : "关闭";
                LblActivate.Content += Config.Activation ? "启用" : "关闭";
                LblKmsServer.Content += Config.KmsServer;
                CbGroup.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                ErrorUtil.WriteError(exception).GetAwaiter().GetResult();
            }
            
        }

        private void CbGroup_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TogBtn.IsChecked = Config.Services.FirstOrDefault(tmp =>
                tmp.Nickname == CbGroup.SelectedItem.ToString())?.Enabled;
            TogBtn.Content = TogBtn.IsChecked ?? false ? "停止服务" : "启用服务";
        }

        private void TogBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = Config.Services.FirstOrDefault(tmp => tmp.Nickname == CbGroup.SelectedItem.ToString());
                if (service == null) throw new NullReferenceException();
                service.Enabled = TogBtn.IsChecked ?? false;
                SocketService = new ServiceSocket(Config);
                SocketService.Message.Headers.MessageType = MessageType.ServiceMsg;
                SocketService.Message.Body = service;
                SocketService.Message.Headers.BodyType = typeof(ServiceGroupModel);
                SocketService.Operation();
                MessageBox.Show("操作执行成功!", "消息", MessageBoxButton.OK);
                TogBtn.Content = TogBtn.IsChecked ?? false ? "停止服务" : "启用服务";
            }
            catch (Exception exception)
            {
                ErrorUtil.WriteError(exception).GetAwaiter().GetResult();
            }
            
        }


    }
}
