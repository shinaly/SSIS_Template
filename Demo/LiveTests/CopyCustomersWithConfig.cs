using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.LiveTests
{
    [ActionClass("DEMO4", "CopyCustomersWithConfig.dtsx")]
    class CopyCustomersWithConfig : BaseLiveTest
    {
        public CopyCustomersWithConfig()
        {
            // reset the environment to allow this to be runnable
            if (File.Exists(Constants.CustomersFileDestination))
                File.Delete(Constants.CustomersFileDestination);

            if (File.Exists(Constants.CustomersFileConverted))
                File.Delete(Constants.CustomersFileConverted);
        }

        [ActionMethod(@"\[CopyCustomersWithConfig]")]
        public void TestWholePackage(ActionContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, exists);

            exists =
               File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, exists);
        }
    }
}
