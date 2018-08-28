using System;
using MFVolumeCtrl.Interfaces;
using System.Threading;

namespace MFVolumeCtrl.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// 服务线程。
    /// 每一服务及任务抽离为单一线程，互不相干。
    /// </summary>
    public abstract class ServiceThread : IServiceThread
    {
        /// <summary>
        /// 服务主线程。
        /// </summary>
        protected Thread MainThread { get; set; }
        /// <summary>
        /// 服务线程默认构造函数。
        /// </summary>
        protected ServiceThread()
        {
            MainThread = new Thread(Operation);
        }
        /// <inheritdoc />
        public void Start()
        {
            try
            {

                MainThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <inheritdoc />
        public void Interrupt()
        {
            MainThread.Interrupt();
        }
        /// <inheritdoc />
        public abstract void Operation();
        /// <inheritdoc />
        public void Dispose()
        {
            MainThread.Interrupt();
        }
    }
}
