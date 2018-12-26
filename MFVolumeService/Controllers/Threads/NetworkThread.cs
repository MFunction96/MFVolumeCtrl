using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using MFVolumeService.Controllers.Operators;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MFVolumeService.Controllers.Threads
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class NetworkThread : SocketThread
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
        public NetworkThread(ConfigModel configModel)
        {
            Config = configModel;
            _allDone = new ManualResetEvent(false);
            Initialization();
        }

        #endregion

        #region Methods

        #region Implement

        public override void Operation()
        {
            while (true)
            {
                try
                {
                    while (true)
                    {
                        _allDone.Reset();

                        // Start an asynchronous socket to listen for connections.  
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

        protected override void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            _allDone.Set();

            // Get the socket that handles the client request.  
            if (!(ar.AsyncState is Socket listener)) return;
            var handler = listener.EndAccept(ar);

            // Create the state object.  
            var state = new StateObject(handler, Config);
            handler.BeginReceive(state.Buffer, 0, state.BufferSize, 0,
                ReceiveCallback, state);
        }

        protected override void Receive(Socket client)
        {
            throw new NotImplementedException();
        }

        protected override void ReceiveCallback(IAsyncResult ar)
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
                using (var msg = BinaryUtil.DeserializeObject<SocketMessage>(state.Buffer, 0, state.ReceiveSize))
                {
                    var response = SortSocketMessage(msg);
                    Send(handler, response);
                }
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
                throw;
            }

        }

        protected override void Send(Socket handler, SocketMessage message)
        {
            var byteData = BinaryUtil.SerializeObject(Message);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                SendCallback, handler);
        }

        protected override void SendCallback(IAsyncResult ar)
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

        protected SocketMessage SortSocketMessage(SocketMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Headers.BodyType == typeof(ServiceGroupModel))
            {
                using (var op = new ServiceOperator(request))
                {
                    return op.Operate();
                }
            }

            if (request.Headers.BodyType == typeof(ScriptModel))
            {
                using (var op = new ScriptOperator(request))
                {
                    return op.Operate();
                }
            }
            throw new ArgumentException("Unknown Message Type!", nameof(request.Headers.BodyType));
        }

        protected override void ConnectCallback(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }
}
