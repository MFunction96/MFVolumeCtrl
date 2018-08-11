using System;

namespace MFVolumeCtrl.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// 服务线程。
    /// 每一服务及任务抽离为单一线程，互不相干。
    /// </summary>
    public interface IServiceThread : IDisposable
    {
        /// <summary>
        /// 启动服务线程。
        /// </summary>
        void Start();
        /// <summary>
        /// 结束服务线程。
        /// </summary>
        void Interrupt();
        /// <summary>
        /// 服务线程核心逻辑。
        /// </summary>
        void Operation();
    }
}
