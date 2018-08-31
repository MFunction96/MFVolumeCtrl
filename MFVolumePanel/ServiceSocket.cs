using System;
using System.Net;
using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;

namespace MFVolumePanel
{
    internal class ServiceSocket : SocketThread
    {
        public ConfigModel Config { get; }

        public ServiceSocket(ConfigModel config)
        {
            Config = config;
        }

        public override void Operation()
        {
            try
            {
                Socketv4.Connect(new IPEndPoint(IPAddress.Loopback, Config.Port));
                Socketv4.Send(BinaryUtil.SerializeObject(Message));
                /*var buffer = new byte[Socketv4.ReceiveBufferSize];
                Socketv4.Receive(buffer);
                using (var message = BinaryUtil.DeserializeObject<SocketMessage>(buffer))
                {
                    if (message.Headers["MsgType"].ToString() != "Response") throw new SocketException();

                }*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
            

        }

        protected override void Initialization()
        {
            
        }
    }
}
