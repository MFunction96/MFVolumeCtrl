using MFVolumeCtrl.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MFVolumeCtrl
{
    [Serializable]
    public class ServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<string> Services { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ServiceModel()
        {
            Services = new List<string>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> CheckStatus()
        {
            return Task.Run(() =>
            {
                var services = ServiceController.GetServices();
                foreach (var service in Services)
                {
                    var controller = services.First(tmp => tmp.ServiceName == service);
                    if (controller is null) throw new NullReferenceException($"{Resources.NonService} : {service}");
                    if (controller.Status != ServiceControllerStatus.Running) return false;
                }
                return true;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> SwapStatus()
        {
            return Task.Run(() =>
            {
                var services = ServiceController.GetServices();
                foreach (var service in Services)
                {
                    var controller = services.First(tmp => tmp.ServiceName == service);
                    if (controller is null) throw new NullReferenceException($"{Resources.NonService} : {service}");
                    if (controller.Status == ServiceControllerStatus.Running && Enabled) controller.Stop();
                    else if (controller.Status == ServiceControllerStatus.Stopped && !Enabled) controller.Start();
                    else throw new InvalidOperationException($"{service}{Resources.ExceptStatus}");
                }
                return Enabled = !Enabled;
            });
        }
    }
}
