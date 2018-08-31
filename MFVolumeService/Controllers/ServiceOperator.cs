using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;

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
        public ServiceOperator(ConfigModel configModel)
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
            while (true)
            {
                var client = Socketv4.Accept();
                var binary = new byte[client.SendBufferSize];
                client.Receive(binary);
                try
                {
                    using (var message = BinaryUtil.DeserializeObject<SocketMessage>(binary))
                    {
                        if (message.Headers.BodyType != typeof(ServiceGroupModel)
                            && message.Headers.MessageType != MessageType.ServiceMsg)
                            throw new SocketException();
                        HandleServices(message);
                    }
                }
                catch (Exception e)
                {
                    Message.Body = e;
                    ErrorUtil.WriteError(e).GetAwaiter().GetResult();
                }
                finally
                {
                    //client.Send(BinaryUtil.SerializeObject(Message));
                }

                client.Close();
            }
            // ReSharper disable once FunctionNeverReturns
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
        }

        #endregion

        #region Private

        private static void HandleServices(SocketMessage request)
        {
            var services = ServiceController.GetServices();
            if (!(request.Body is ServiceGroupModel servicegroup))
                throw new ArgumentNullException(nameof(servicegroup));
            foreach (var service in servicegroup.Services)
            {
                var controller = services.FirstOrDefault(tmp => tmp.ServiceName == service);
                if (controller is null) throw new NullReferenceException($"There is no : {service}");
                if (servicegroup.Enabled) controller.Start();
                else controller.Stop();
            }
        }

        #endregion

        #endregion

    }
}
