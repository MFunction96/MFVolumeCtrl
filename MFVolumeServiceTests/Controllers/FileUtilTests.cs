using MFVolumeCtrl.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MFVolumeServiceTests.Controllers
{
    [TestClass()]
    public class FileUtilTests
    {
        [TestMethod()]
        public async Task ExportObjTest()
        {
            const string test = @"Go!";
            var path =
                $"{MFVolumeCtrl.Properties.Resources.ConfigPath}\\{MFVolumeCtrl.Properties.Resources.ConfigFile}";
            await FileUtil.ExportObj(test, path);
            var result = await FileUtil.ImportObj<string>(path);
            Assert.AreEqual(test, result, $"Result = {result}\r\nExpect = {test}\r\n");
        }

        [TestMethod()]
        public void ImportObjTest()
        {
            //Assert.Fail();
        }
    }
}