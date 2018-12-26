using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        private BackgroundWorker Worker { get; set; }

        public Action<ICollection<ScriptModel>> AcScript { get; set; }

        public ScheduleWindow(ICollection<ScriptModel> scripts)
        {
            InitializeComponent();
            Scripts = scripts;
            Worker = new BackgroundWorker();
        }

        private void BtnEnterShadow_Click(object sender, RoutedEventArgs e)
        {
            var pwd = string.Empty;
            var inputWindow = new InputWindow("请输入Shadow Defender密码", string.Empty)
            {
                AcAddItem = input => pwd = input
            };
            var flag = inputWindow.ShowDialog();
            if (flag ?? false)
            {
                Worker.RunWorkerAsync((true, pwd));
            }
        }

        private void BtnExitShadow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DoProgress(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is ValueTuple<bool, string> args)
            {
                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = @"C:\Program Files\Shadow Defender\CmdTool.exe",
                        Arguments = $"/pwd:{args.Item2}"
                    }
                };
                proc.StartInfo.Arguments += args.Item1 ? "/enter:C /now" : "/exit:C /reboot";
                proc.Start();
                proc.WaitForExit();
            }
            else throw new ArgumentException(nameof(e));
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void ProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += DoProgress;
            Worker.ProgressChanged += ProgressChanged;
            Worker.RunWorkerCompleted += ProgressCompleted;
        }

        private void BtnSendCmd_Click(object sender, RoutedEventArgs e)
        {
            var inputWindow = new InputWindow("请输入指令", string.Empty)
            {
                AcAddItem = str =>
                {
                    string file;
                    string args;
                    if (str.StartsWith("\""))
                    {
                        file = str.Substring(0, str.IndexOf('\"', 1));
                        args = str.Substring(str.IndexOf('\"')).Trim();
                    }
                    else
                    {
                        file = str.Substring(0, str.IndexOf(' '));
                        args = str.Substring(str.IndexOf(' ')).Trim();
                    }
                    var proc = new Process
                    {
                        StartInfo =
                        {
                            FileName = file,
                            Arguments = args
                        }
                    };
                }
            };
        }
    }
}
