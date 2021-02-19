using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.LiveTests
{
    [ActionClass("DEMO5", "CopyCustomers.dtsx")]
    class CopyCustomersWithoutDataTaps : BaseLiveTest
    {
        public CopyCustomersWithoutDataTaps()
        {
            // reset the environment to allow this to be runnable
            if (File.Exists(Constants.CustomersFileDestination))
                File.Delete(Constants.CustomersFileDestination);

            if (File.Exists(Constants.CustomersFileConverted))
                File.Delete(Constants.CustomersFileConverted);
        }

        [ActionMethod(@"\[CopyCustomers]")]
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

        [ActionMethod(@"\[CopyCustomers]\[FST Copy Source File]")]
        public void TestCopySourceFile(ActionContext context)
        {
            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);
        }

        [ActionMethod(@"\[CopyCustomers]\[DFT Convert customer names]")]
        public void TestConvertCustomerNames(ActionContext context)
        {
            bool exists =
                File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);

            DtsVariable variable = context.ActiveExecutable.GetVariable(@"CustomerCount");
            var count = (int) variable.GetValue();
            Assert.AreEqual(3, count);
        }

        [ActionMethod(@"\[CopyCustomers]\[FST Copy Source File]\[OnPostExecute]\[SEQC Container]")]
        public void TestEvent(ActionContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            DtsVariable variable = context.ActiveExecutable.GetVariable(@"ExecutedId");
            var id = (int)variable.GetValue();
            Assert.AreEqual(1908, id);
        }
    }
}
