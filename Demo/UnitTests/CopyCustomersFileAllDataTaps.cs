using System.Collections.ObjectModel;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomers.dtsx")]
    [DataTap(@"\[CopyCustomers]\[DFT Convert customer names]\[RCNT Count  customers]", @"\[CopyCustomers]\[DFT Convert customer names]\[DER Convert names to upper string]")]
    [DataTap(@"\[CopyCustomers]\[DFT Convert customer names]\[DER Convert names to upper string]", @"\[CopyCustomers]\[DFT Convert customer names]\[FFD Customers converted]")]
    class CopyCustomersFileAllDataTaps : BaseUnitTest
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

            exists =
               File.Exists(Constants.CustomersFileConverted);
            
            Assert.AreEqual(true, exists);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[CustomerCount]");
            var count = (int)variable.GetValue();
            Assert.AreEqual(3, count);

            ReadOnlyCollection<DataTap> dataTaps = context.DataTaps;

            DataTap dataTap = dataTaps[0];
            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
            }

            DataTap dataTap1 = dataTaps[1];
            foreach (DataTapSnapshot snapshot in dataTap1.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
            }
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }
    }
}
