using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MFVolumeCtrl.Interfaces
{
    interface IServiceThread : IDisposable
    {
        void Start();
        void Interrupt();
        void Operation();
    }
}
