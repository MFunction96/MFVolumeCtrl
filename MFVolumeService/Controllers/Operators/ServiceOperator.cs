using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using MFVolumeService.Interfaces;
using System;
using System.Linq;
using System.ServiceProcess;

namespace MFVolumeService.Controllers.Operators
{
    public class ServiceOperator : IOperator
    {
        protected ServiceGroupModel ServiceGroup { get; }

        public ServiceOperator(SocketMessage message)
        {
            if (!(message.Body is ServiceGroupModel serviceGroup))
                throw new ArgumentException(nameof(message.Body));
            ServiceGroup = serviceGroup;
        }

        public void Dispose()
        {
            ServiceGroup?.Dispose();
        }

        public SocketMessage Operate()
        {
            var services = ServiceController.GetServices();
            foreach (var service in ServiceGroup.Services)
            {
                var controller = services.FirstOrDefault(tmp => tmp.ServiceName == service);
                if (controller is null)
                {
                    var exception = new NullReferenceException($"There is no : {service}");
                    ErrorUtil.WriteError(exception).GetAwaiter().GetResult();
                    continue;
                }
                if (ServiceGroup.Enabled) controller.Start();
                else controller.Stop();
            }
            return new SocketMessage();
        }
    }
}
