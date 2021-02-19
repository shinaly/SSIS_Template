using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomers.dtsx")]
    [FakeDestination(@"\[CopyCustomers]\[DFT Convert customer names]\[FFD Customers converted]")]
    class CopyCustomersFileAllFakeDestination : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable custSrc = context.Package.GetVariableForPath(@"\[CopyCustomers].[SourcePath]");
            custSrc.SetValue(Constants.CustomersFileSource);

            DtsVariable custDest = context.Package.GetVariableForPath(@"\[CopyCustomers].[DestinationPath]");
            custDest.SetValue(Constants.CustomersFileDestination);

            DtsVariable custDestNew = context.Package.GetVariable(@"ConvertDestinationPath");
            custDestNew.SetValue(Constants.CustomersFileConverted);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[CustomerCount]");
            variable.SetValue(0);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, exists);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[CustomerCount]");
            var count = (int)variable.GetValue();
            Assert.AreEqual(3, count);

            Assert.AreEqual(1, context.FakeDestinations.Count);

            FakeDestination fakeDestination = context.FakeDestinations[0];

            foreach (FakeDestinationSnapshot snapshot in fakeDestination.Snapshots)
            {
                string data = snapshot.LoadData();

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1;COMPANY1;", rows[0]);
                Assert.AreEqual("5;COMPANY2;", rows[1]);
                Assert.AreEqual("11;COMPANY3;", rows[2]);
            }
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }
    }
}
