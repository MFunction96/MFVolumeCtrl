using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace MFVolumeCtrl.Models
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [Serializable]
    public class ServiceModel : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> Services { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ServiceModel()
        {
            Services = new HashSet<string>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckStatus()
        {
            var services = ServiceController.GetServices();
            foreach (var service in Services)
            {
                var controller = services.First(tmp => tmp.ServiceName == service);
                if (controller is null) throw new NullReferenceException($"{Resources.NonService} : {service}");
                if (controller.Status != ServiceControllerStatus.Running) return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public Task SetStatus(bool status)
        {
            return Task.Run(() =>
            {
                var services = ServiceController.GetServices();
                foreach (var service in Services)
                {
                    var controller = services.First(tmp => tmp.ServiceName == service);
                    if (controller is null) throw new NullReferenceException($"{Resources.NonService} : {service}");
                    if (status) controller.Start();
                    else controller.Stop();
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="countDown"></param>
        protected async Task SendCallback(Socket source, int countDown)
        {
            for (var i = 0; i < countDown; i++)
            {
                await Send(source);
                if (CheckStatus() != Enabled)
                {
                    Enabled = !Enabled;
                    break;
                }

                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="countDown"></param>
        /// <param name="status"></param>
        public async Task SetStatus(Socket source, int countDown, bool status)
        {
            await SetStatus(status);
            await SendCallback(source, countDown);
        }
        /// <summary>
        /// 
        /// </summary>
        public async void SwitchStatus()
        {
            await SetStatus(!Enabled);
            Enabled = !Enabled;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="countDown"></param>
        /// <returns></returns>
        public async Task SwitchStatus(Socket source, int countDown)
        {
            SwitchStatus();
            await SendCallback(source, countDown);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public async Task Send(Socket socket)
        {
            try
            {
                var buffer = await BinaryUtil.SerializeObject(this);
                socket.Send(buffer);
            }
            catch (Exception e)
            {
                await ErrorUtil.WriteError(e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static async Task<ServiceModel> Receive(Socket socket)
        {
            try
            {
                var length = Marshal.SizeOf(typeof(ServiceModel));
                var buffer = new byte[length];
                socket.Receive(buffer, length, 0);
                return await BinaryUtil.DeserializeObject<ServiceModel>(buffer);
            }
            catch (Exception e)
            {
                await ErrorUtil.WriteError(e);
                return null;
            }
        }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            Services.Clear();
        }
    }
}
