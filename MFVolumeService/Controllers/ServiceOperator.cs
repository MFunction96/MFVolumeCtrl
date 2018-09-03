using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading;

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

        /// <summary>
        /// 
        /// </summary>
        private readonly ManualResetEvent _allDone;

        #endregion

        #region Construction

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="configModel"></param>
        public ServiceOperator(ConfigModel configModel)
        {
            Config = configModel;
            _allDone = new ManualResetEvent(false);
            Initialization();
        }

        #endregion

        #region Methods

        #region Implement

        /*/// <inheritdoc />
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
        }*/

        public override void Operation()
        {
            while (true)
            {
                /*var client = Socketv4.Accept();
                var binary = new byte[client.SendBufferSize];
                client.Receive(binary);*/
                try
                {
                    while (true)
                    {
                        _allDone.Reset();

                        // Start an asynchronous socket to listen for connections.  
                        //Console.WriteLine("Waiting for a connection...");
                        Socketv4.BeginAccept(
                            AcceptCallback,
                            Socketv4);

                        // Wait until a connection is made before continuing.  
                        _allDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Message.Body = e;
                    ErrorUtil.WriteError(e).GetAwaiter().GetResult();
                }
                Socketv4.Close();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected sealed override void Initialization()
        {
            Socketv4.Bind(new IPEndPoint(IPAddress.Any, Config.Port));

            //Socket v6.Bind(new IPEndPoint(IPAddress.IPv6Any, Config.Port));
            Socketv4.Listen(Config.PendingQueue);
            //Socket v6.Listen(Config.PendingQueue);
        }

        #endregion

        #region Private

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            _allDone.Set();

            // Get the socket that handles the client request.  
            if (!(ar.AsyncState is Socket listener)) return;
            var handler = listener.EndAccept(ar);

            // Create the state object.  
            var state = new StateObject(handler, Config);
            handler.BeginReceive(state.Buffer, 0, state.BufferSize, 0,
                ReadCallback, state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            if (!(ar.AsyncState is StateObject state)) return;
            var handler = state.WorkSocket;

            // Read data from the client socket.   
            state.ReceiveSize = handler.EndReceive(ar);

            if (state.ReceiveSize <= 0) return;
            // There  might be more data, so store the data received so far.  
            /*state.Sb.Append(Encoding.ASCII.GetString(
                state.Buffer, 0, bytesRead));*/

            try
            {
                var msg = BinaryUtil.DeserializeObject<SocketMessage>(state.Buffer, 0, state.ReceiveSize);
                HandleServices(msg);
                Send(handler);
                //Send(handler, content);
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
                throw;
            }

            // Check for end-of-file tag. If it is not there, read   
            // more data.  
/*            var content = state.Sb.ToString();
            if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
            {
                // All the data has been read from the   
                // client. Display it on the console.  
                Console.WriteLine($"Read {content.Length} bytes from socket. \n Data : {content}");
                // Echo the data back to the client.  
                Send(handler, content);
            }
            else
            {
                // Not all data received. Get more.  
                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    ReadCallback, state);
            }*/
        }

        //private void Send(Socket handler, string data)
        private void Send(Socket handler)
        {
            // Convert the string data to byte data using ASCII encoding.  
            //var byteData = Encoding.ASCII.GetBytes(data);
            var byteData = BinaryUtil.SerializeObject(Message);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                SendCallback, handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                if (!(ar.AsyncState is Socket handler)) throw new ArgumentException(nameof(ar));
                // Complete sending the data to the remote device.  
                handler.EndSend(ar);
                //Console.WriteLine($"Sent {bytesSent} bytes to client.");

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
        }

        private void HandleServices(SocketMessage request)
        {
            var services = ServiceController.GetServices();
            if (!(request.Body is ServiceGroupModel serviceGroup))
                throw new ArgumentNullException(nameof(serviceGroup));
            foreach (var service in serviceGroup.Services)
            {
                var controller = services.FirstOrDefault(tmp => tmp.ServiceName == service);
                if (controller is null) throw new NullReferenceException($"There is no : {service}");
                if (serviceGroup.Enabled) controller.Start();
                else controller.Stop();
            }
        }

        #endregion

        #endregion

    }
}
