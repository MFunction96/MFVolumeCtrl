using System;
using MFVolumeCtrl.Controllers;
using MFVolumeCtrl.Models;
using MFVolumeService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                var config = FileUtil.ImportObj<ConfigModel>(@"C:\ProgramData\MFVolumeCtrl\config.json").GetAwaiter().GetResult();
                //Ticker = new TimeWatcher(ref Config);
                var servicectrl = new ServiceOperator(config);
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
                var config = FileUtil.ImportObj<ConfigModel>(@"C:\ProgramData\MFVolumeCtrl\config.json").GetAwaiter().GetResult();
                var op = new ServiceOperator(config);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}