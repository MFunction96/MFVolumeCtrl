using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using MFVolumeService.Controllers.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MFVolumeService.Controllers.Threads;

namespace MFVolumeServiceTests.Controllers
{
    [TestClass()]
    public class ServiceOperatorTests
    {
        [TestMethod()]
        public void ServiceOperatorTest()
        {
            try
            {
                var config = FileUtil.ImportObj<ConfigModel>($"{ConfigModel.ConfigPath}\\{ConfigModel.ConfigName}")
                    .GetAwaiter().GetResult();
                //Ticker = new TimeWatcher(ref Config);
                var servicectrl = new NetworkThread(config);
                //servicectrl.Start();
                //servicectrl.Interrupt();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        [TestMethod()]
        public void OperationTest()
        {
            try
            {
                var config = FileUtil.ImportObj<ConfigModel>($"{ConfigModel.ConfigPath}\\{ConfigModel.ConfigName}")
                    .GetAwaiter().GetResult();
                var op = new NetworkThread(config);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}