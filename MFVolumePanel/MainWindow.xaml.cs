using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
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

        protected BackgroundWorker Workers { get; set; }

        protected BackgroundWorkWindow BgwWindow { get; set; }

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
            Workers = new BackgroundWorker();
            BgwWindow = new BackgroundWorkWindow();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var services = ServiceController.GetServices();

                var host = services.FirstOrDefault(tmp => tmp.ServiceName == "MFVolumeService");
                if (host is null || host.Status != ServiceControllerStatus.Running)
                {
                    MessageBox.Show("MFVolumeService未启用！请先启动MFVolumeService服务！", "错误", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Environment.Exit(0);
                }

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

            Workers.WorkerReportsProgress = true;
            Workers.WorkerSupportsCancellation = true;
            Workers.DoWork += DoProgress;
            Workers.ProgressChanged += ProgressChanged;
            Workers.RunWorkerCompleted += ProgressCompleted;
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
                Workers.RunWorkerAsync((TogBtn.IsChecked ?? false, Config, CbGroup.SelectedItem.ToString()));
                BgwWindow.ShowDialog();
                /*PbWait.IsIndeterminate = true;
                CbGroup.IsEnabled = false;
                TogBtn.IsEnabled = false;*/
            }
            catch (Exception exception)
            {
                ErrorUtil.WriteError(exception).GetAwaiter().GetResult();
            }
            
        }

        private void DoProgress(object sender, DoWorkEventArgs e)
        {
            if (sender is BackgroundWorker worker)
            {
                if (e.Argument is ValueTuple<bool, ConfigModel, string> tuple)
                {
                    var service = tuple.Item2.Services.FirstOrDefault(tmp => tmp.Nickname == tuple.Item3);
                    if (service == null) throw new NullReferenceException(nameof(service));
                    service.Enabled = tuple.Item1;
                    var socketService = new ServiceSocket(tuple.Item2);
                    socketService.Message.Headers.MessageType = MessageType.ServiceMsg;
                    socketService.Message.Body = service;

                    socketService.Message.Headers.BodyType = typeof(ServiceGroupModel);

                    socketService.Operation();

                    var countdown = Config.CheckCount;
                    while (countdown > 0)
                    {
                        var s = ServiceController.GetServices();
                        var flag = true;
                        foreach (var serviceName in service.Services)
                        {
                            var target = s.FirstOrDefault(tmp => tmp.ServiceName == serviceName);
                            if (target is null) throw new NullReferenceException(nameof(target));
                            if ((target.Status == ServiceControllerStatus.Running && tuple.Item1) ||
                                (target.Status == ServiceControllerStatus.Stopped && !tuple.Item1)) continue;
                            flag = false;
                            break;
                        }
                        if (flag) break;
                        countdown--;
                        worker.ReportProgress(countdown);
                        Thread.Sleep(1000);
                    }
                }
                else throw new ArgumentException(nameof(e));
            }
            else
            {
                throw new ArgumentException(nameof(e));
            }
            
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //TogBtn.Content = $"{e.ProgressPercentage}";
            BgwWindow.SetMessage(e.ProgressPercentage.ToString());
        }

        private void ProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(e.Result.ToString());
                return;
            }
            BgwWindow.Close();
            /*PbWait.IsIndeterminate = false;
            TogBtn.IsEnabled = true;
            CbGroup.IsEnabled = true;*/
            MessageBox.Show("操作执行成功!", "消息", MessageBoxButton.OK);
            TogBtn.Content = TogBtn.IsChecked ?? false ? "停止服务" : "启用服务";
        }
    }
}
