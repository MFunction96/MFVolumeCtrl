using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MFVolumeService.Controllers
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
            Initialization();
        }

        #endregion

        #region Methods

        #region Implement

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override void Operation()
        {
            Socketv4.BeginAccept(Callback, Socketv4);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected sealed override void Initialization()
        {
            Socketv4.Bind(new IPEndPoint(IPAddress.Any, Config.Port));
            //Socketv6.Bind(new IPEndPoint(IPAddress.IPv6Any, Config.Port));
            Socketv4.Listen(Config.PendingQueue);
            //Socketv6.Listen(Config.PendingQueue);
            MainThread.Start();
        }

        protected override void Callback(IAsyncResult asyncResult)
        {
            if (!(asyncResult.AsyncState is Socket server))
                throw new ArgumentNullException(nameof(asyncResult));
            var client = server.EndAccept(asyncResult);
            client.Send(BinaryUtil.SerializeObject(Message).GetAwaiter().GetResult());
            // ReSharper disable once FunctionNeverReturns
        }

        #endregion

        #region Private

        private async Task HandleServices(byte[] request)
        {
            try
            {
                using (var serviceModel = await BinaryUtil.DeserializeObject<ServiceGroupModel>(request))
                {
                    var services = ServiceController.GetServices();
                    foreach (var service in serviceModel.Services)
                    {
                        var controller = services.FirstOrDefault(tmp => tmp.ServiceName == service);
                        if (controller is null) throw new NullReferenceException($"There is no : {service}");
                        if (serviceModel.Enabled) controller.Start();
                        else controller.Stop();
                    }
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }

        #endregion

        #endregion

    }
}
