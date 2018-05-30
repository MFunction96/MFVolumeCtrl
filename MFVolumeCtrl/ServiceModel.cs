﻿using MFVolumeCtrl.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;

namespace MFVolumeCtrl
{
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
        /// <param name="settings"></param>
        /// <returns></returns>
        public bool SetStatus(SettingsModel settings)
        {
            var flag = false;
            var services = ServiceController.GetServices();
            foreach (var service in Services)
            {
                var controller = services.First(tmp => tmp.ServiceName == service);
                if (controller is null) throw new NullReferenceException($"{Resources.NonService} : {service}");
                if (controller.Status != ServiceControllerStatus.Stopped)
                {
                    if (Enabled) continue;
                    controller.Stop();
                    flag = true;
                }
                else if (controller.Status != ServiceControllerStatus.Running)
                {
                    if (!Enabled) continue;
                    controller.Start();
                    flag = true;
                }
            }

            if (!flag) return false;
            flag = false;
            for (var i = 0; i < settings.Check; i++)
            {
                if (CheckStatus() != Enabled)
                {
                    Enabled = !Enabled;
                    flag = true;
                    break;
                }
                Thread.Sleep(settings.Interval);
            }
            
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public void Send(Socket socket)
        {
            try
            {
                var buffer = BinaryUtil.SerializeObject(this);
                socket.Send(buffer);
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static ServiceModel Receive(Socket socket)
        {
            try
            {
                var length = Marshal.SizeOf(typeof(ServiceModel));
                var buffer = new byte[length];
                socket.Receive(buffer, length, 0);
                return BinaryUtil.DeserializeObject<ServiceModel>(buffer);
            }
            catch (Exception e)
            {
                ErrorUtil.WriteError(e);
                return null;
            }
        }

        public void Dispose()
        {
            Services.Clear();
        }
    }
}
