using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;

namespace MFVolumePanel
{
    public class ServiceSocket : SocketThread
    {
        // ManualResetEvent instances signal completion.  
        private readonly ManualResetEvent _connectDone;
        private readonly ManualResetEvent _sendDone;
        private readonly ManualResetEvent _receiveDone;

        public ConfigModel Config { get; }

        public ServiceSocket(ConfigModel config)
        {
            Config = config;
            _connectDone = new ManualResetEvent(false);
            _sendDone = new ManualResetEvent(false);
            _receiveDone = new ManualResetEvent(false);
        }

        public override void Operation()
        {
            try
            {
                // Connect to the remote endpoint.  
                Socketv4.BeginConnect(new IPEndPoint(IPAddress.Loopback, Config.Port),
                    ConnectCallback, Socketv4);
                _connectDone.WaitOne();

                // Send test data to the remote device.  
                Send(Socketv4);
                _sendDone.WaitOne();

                // Receive the response from the remote device.  
                //Receive(Socketv4);
                //_receiveDone.WaitOne();

                // Write the response to the console.  
                //Console.WriteLine("Response received : {0}", response);

                // Release the socket.  
                Socketv4.Shutdown(SocketShutdown.Both);
                Socketv4.Close();
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
        }

/*        private static void StartClient()
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the   
                // remote device is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry("host.contoso.com");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                Socket client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                _connectDone.WaitOne();

                // Send test data to the remote device.  
                Send(client, "This is a test<EOF>");
                _sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                _receiveDone.WaitOne();

                // Write the response to the console.  
                Console.WriteLine("Response received : {0}", response);

                // Release the socket.  
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }*/

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  

                // Complete the connection.  
                if (ar.AsyncState is Socket client) client.EndConnect(ar);
                else throw new ArgumentException(nameof(ar));
                //Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                _connectDone.Set();
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                var state = new StateObject(client, Config);

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.Buffer, 0, state.BufferSize, 0,
                    ReceiveCallback, state);
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                if (!(ar.AsyncState is StateObject state)) throw new ArgumentException(nameof(ar));
                var client = state.WorkSocket;

                // Read data from the remote device.  
                state.ReceiveSize = client.EndReceive(ar);

/*                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    _receiveDone.Set();
                }*/
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
        }

        private void Send(Socket client)
        {
            // Convert the string data to byte data using ASCII encoding.  
            var byteData = BinaryUtil.SerializeObject(Message);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                SendCallback, client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  

                // Complete sending the data to the remote device.  
                if (ar.AsyncState is Socket client)
                {
                    client.EndSend(ar);
                    //.WriteLine("Sent {0} bytes to server.", bytesSent);
                }
                else throw new ArgumentException(nameof(ar));
                // Signal that all bytes have been sent.  
                _sendDone.Set();
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }
        }
        /*public override void Operation()
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

                }#1#
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorUtil.WriteError(e).GetAwaiter().GetResult();
            }

        }*/

        protected override void Initialization()
        {
            
        }
    }
}
