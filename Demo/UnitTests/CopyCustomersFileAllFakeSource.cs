using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomers.dtsx", ConnectionsToRemove = "Customers Dst")]
    [DataTap(@"\[CopyCustomers]\[DFT Convert customer names]\[DER Convert names to upper string]", @"\[CopyCustomers]\[DFT Convert customer names]\[FFD Customers converted]")]
    [FakeSource(@"\[CopyCustomers]\[DFT Convert customer names]\[FFS Customers]",
        new[] { "Flat File Source Output", "Flat File Source Error Output" },
        new[] { "1908,pg\r\n2401,irg\r\n0911,lg", "error,1908,1\r\nerror,2401,2" },
        RowDelimiter = "\r\n", ColumnDelimiter = ",")]
    class CopyCustomersFileAllFakeSource : BaseUnitTest
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

                string[] rows = data.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1908;PG;", rows[0]);
                Assert.AreEqual("2401;IRG;", rows[1]);
                Assert.AreEqual("911;LG;", rows[2]);
            }

            DtsVariable sourceCustomersErrorCount = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[SourceCustomersErrorCount]");
            var val = (int)sourceCustomersErrorCount.GetValue();
            Assert.AreEqual(2, val);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }
    }
}
