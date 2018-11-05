using System;
using MFVolumeCtrl.Models;

namespace MFVolumeService.Interfaces
{
    internal interface IOperator : IDisposable
    {
        SocketMessage Operate();
    }
}
