using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;

namespace MFVolumeService.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// 时间监视器，用于处理计划任务。
    /// </summary>
    public class TimeWatcher : ServiceThread
    {

        #region Properties

        /// <summary>
        /// 间隔时间。
        /// </summary>
        private const int Interval = 1000 * 60;
        /// <summary>
        /// 服务配置信息。
        /// </summary>
        private ConfigModel Config { get; }

        #endregion


        #region Construction

        /// <inheritdoc />
        /// <summary>
        /// 根据服务配置信息构造时间监视器。
        /// </summary>
        /// <param name="configModel"></param>
        public TimeWatcher(ref ConfigModel configModel)
        {
            Config = configModel;
        }

        #endregion

        #region Methods

        #region Implement

        /// <inheritdoc />
        public override void Operation()
        {
            for (; ; )
            {
                ConfigModel config;
                lock (Config)
                {
                    config = new ConfigModel(Config);
                }

                RunScript(config).GetAwaiter().GetResult();

                config.Dispose();
                Thread.Sleep(Interval);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        #endregion

        #region Private
        /// <summary>
        /// 运行脚本。
        /// </summary>
        /// <param name="configModel">
        /// 配置文件数据模型。
        /// </param>
        /// <returns>
        /// 异步运行结果。
        /// </returns>
        private static async Task RunScript(ConfigModel configModel)
        {
            if (!configModel.RunScript) return;
            var scripts = configModel.Scripts.Where(tmp => tmp.StartTime.Minute == DateTime.Now.Minute);
            foreach (var script in scripts)
            {
                await script.Run();
            }
        }

        #endregion

        #region Protected



        #endregion

        #region Public



        #endregion

        #endregion

    }
}
