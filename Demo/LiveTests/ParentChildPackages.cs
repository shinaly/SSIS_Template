using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.LiveTests
{
    [ActionClass("DEMO6", "CopyCustomers.dtsx")]
    [DataTap(@"\[CopyCustomers]\[DFT Convert customer names]\[RCNT Count  customers]", @"\[CopyCustomers]\[DFT Convert customer names]\[DER Convert names to upper string]")]
    [DataTap(@"\[CopyCustomers]\[DFT Convert customer names]\[DER Convert names to upper string]", @"\[CopyCustomers]\[DFT Convert customer names]\[FFD Customers converted]")]
    [FakeSource(@"\[CopyCustomers]\[DFT Convert customer names]\[FFS Customers]"
        , new[] { "Flat File Source Output" }
        , new[] { "1908,pg;2401,irg;0911,lg" }, RowDelimiter = ";", ColumnDelimiter = ",")]
    class ParentChildPackages : BaseLiveTest
    {
        public ParentChildPackages()
        {
            // reset the environment to allow this to be runnable
            if (File.Exists(Constants.CustomersFileDestination))
                File.Delete(Constants.CustomersFileDestination);

            if (File.Exists(Constants.CustomersFileConverted))
                File.Delete(Constants.CustomersFileConverted);
        }

        [ActionMethod(@"\[CopyCustomers]", Description = "Test for the whole package.")]
        public void TestWholePackage(ActionContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, exists);

            exists =
               File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, exists);

            CheckDataTaps(context);
        }

        [ActionMethod(@"\[CopyCustomers]\[FST Copy Source File]", Description = "Checks the existance of the file.")]
        public void TestCopySourceFile(ActionContext context)
        {
            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);

            CheckDataTaps(context);
        }

        [ActionMethod(@"\[CopyCustomers]\[DFT Convert customer names]", Description = "Customer count must be 3.")]
        public void TestConvertCustomerNames(ActionContext context)
        {
            bool exists =
                File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);

            DtsVariable variable = context.ActiveExecutable.GetVariable(@"CustomerCount");
            var count = (int)variable.GetValue();
            Assert.AreEqual(3, count);

            CheckDataTaps(context);
        }

        [ActionMethod(@"\[CopyCustomers]\[FST Copy Source File]\[OnPostExecute]\[SEQC Container]")]
        public void TestEvent(ActionContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            DtsVariable variable = context.ActiveExecutable.GetVariable(@"ExecutedId");
            var id = (int)variable.GetValue();
            Assert.AreEqual(1908, id);

            CheckDataTaps(context);
        }

        private void CheckDataTaps(ActionContext context)
        {
            ReadOnlyCollection<DataTap> dataTaps = context.DataTaps;

            if (dataTaps == null)
                return;

            DataTap dataTap =
                dataTaps.Single(
                    tap =>
                        tap.PathToSourceComponent ==
                        @"\[CopyCustomers]\[DFT Convert customer names]\[RCNT Count  customers]"
                        &&
                        tap.PathToDestinationComponent ==
                        @"\[CopyCustomers]\[DFT Convert customer names]\[DER Convert names to upper string]");

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();

                Assert.AreEqual(3, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1908;pg;", rows[0]);
                Assert.AreEqual("2401;irg;", rows[1]);
                Assert.AreEqual("911;lg;", rows[2]);
            }

            dataTap =
                dataTaps.Single(
                    tap =>
                        tap.PathToSourceComponent ==
                       @"\[CopyCustomers]\[DFT Convert customer names]\[DER Convert names to upper string]"
                        &&
                        tap.PathToDestinationComponent ==
                        @"\[CopyCustomers]\[DFT Convert customer names]\[FFD Customers converted]");
            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();

                Assert.AreEqual(3, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1908;PG;", rows[0]);
                Assert.AreEqual("2401;IRG;", rows[1]);
                Assert.AreEqual("911;LG;", rows[2]);
            }
        }
    }
}
