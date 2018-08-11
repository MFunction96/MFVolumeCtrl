using System.Threading;
using MFVolumeCtrl.Interfaces;

namespace MFVolumeCtrl.Models
{
    public abstract class ServiceThread : IServiceThread
    {
        protected Thread MainThread { get; set; }

        protected ServiceThread()
        {
            MainThread = new Thread(Operation);
        }

        public void Start()
        {
            MainThread.Start();
        }

        public void Interrupt()
        {
            MainThread.Interrupt();
        }

        public abstract void Operation();

        public void Dispose()
        {
            MainThread.Interrupt();
        }
    }
}
