using System;
using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;

namespace MFVolumeService.Controller
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class ServiceOperator : SocketThread
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        protected ConfigModel Config { get; }
        #endregion

        #region Construction
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="configModel"></param>
        public ServiceOperator(ref ConfigModel configModel)
        {
            Config = configModel;
        }

        #endregion

        #region Methods

        #region Implement

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override void Operation()
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void Initialization()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }
}
