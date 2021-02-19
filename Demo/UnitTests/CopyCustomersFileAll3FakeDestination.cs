using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    /*

     Each unit test is implemented in a class which derives from BaseUnitTest.
     Setup, Verify and Teardown methods have to be implemented. You can use Setup method 
     to prepare test. In Verify method you can check the results. Purpose of Teardown is 
     to allow you to clean test environment after the package has executed. 

     In order for a class to be recognized as a unit test, you have to decorate it with a 
     UnitTest attribute. UnitTest attribute defines the package or control flow element to 
     be tested, and repository where the package resides. The name of the class implementing 
     BaseUnitTest is not important.

     */
    [UnitTest("DEMO", "CopyCustomers3.dtsx", DisableLogging = true, ConnectionsToRemove = "Customers Dst")]
    [DumpPackage]
    [DataTap(@"\[CopyCustomers3]\[DFT Convert customer names]\[FFS Customers]", @"\[CopyCustomers3]\[DFT Convert customer names]\[RCNT Count error customers]")]
    [DataTap(@"\[CopyCustomers3]\[DFT Convert customer names]\[MLT Customers]", @"\[CopyCustomers3]\[DFT Convert customer names]\[ALL Dummy]")]
    [DataTap(@"\[CopyCustomers3]\[DFT Convert customer names]\[CSPL Customers]", @"\[CopyCustomers3]\[DFT Convert customer names]\[SCR First Customer]")]
    [DataTap(@"\[CopyCustomers3]\[DFT Convert customer names]\[CSPL Customers]", @"\[CopyCustomers3]\[DFT Convert customer names]\[ALL Customers]", PathName = "Third Customer")]
    [DataTap(@"\[CopyCustomers3]\[DFT Convert customer names]\[SCR Second Customer]", @"\[CopyCustomers3]\[DFT Convert customer names]\[ALL Customers]")]
    [DataTap(@"\[CopyCustomers3]\[DFT Convert customer names]\[RCNT Count  customers]", @"\[CopyCustomers3]\[DFT Convert customer names]\[DER Convert names to upper string]")]
    [DataTap(@"\[CopyCustomers3]\[DFT Convert customer names]\[DER Convert names to upper string]", @"\[CopyCustomers3]\[DFT Convert customer names]\[FFD Customers converted]")]
    [FakeSource(@"\[CopyCustomers3]\[DFT Convert customer names]\[FFS Customers]",
        new[] { "Flat File Source Output", "Flat File Source Error Output" },
        new[] { "0911,lg\r\n1908,pg\r\n1924,pil\r\n2401,irg", "error,1908,1\r\nerror,2401,2" },
        RowDelimiter = "\r\n", ColumnDelimiter = ",")]
    [FakeDestination(@"\[CopyCustomers3]\[DFT Convert customer names]\[FFD Customers converted]")]
    public class CopyCustomersFileAll3FakeDestination : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable custSrc = context.Package.GetVariableForPath(@"\[CopyCustomers3].[SourcePath]");
            custSrc.SetValue(Constants.CustomersFileSource);

            DtsVariable custDest = context.Package.GetVariableForPath(@"\[CopyCustomers3].[DestinationPath]");
            custDest.SetValue(Constants.CustomersFileDestination);

            DtsVariable custDestNew = context.Package.GetVariableForPath(@"\[CopyCustomers3].[ConvertDestinationPath]");
            custDestNew.SetValue(Constants.CustomersFileConverted);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers3]\[DFT Convert customer names].[CustomerCount]");
            variable.SetValue(0);
        }

        protected override void Verify(VerificationContext context)
        {
            context.Package.ThrowExceptionOnError();

            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, exists);

            DtsVariable customerCountVariable = context.Package.GetVariableForPath(@"\[CopyCustomers3]\[DFT Convert customer names].[CustomerCount]");
            var customersCount = (int)customerCountVariable.GetValue();
            Assert.AreEqual(4, customersCount);

            DtsVariable sourceCustomersErrorCountVariable = context.Package.GetVariableForPath(@"\[CopyCustomers3]\[DFT Convert customer names].[SourceCustomersErrorCount]");
            var errorsCount = (int)sourceCustomersErrorCountVariable.GetValue();
            Assert.AreEqual(2, errorsCount);

            DtsVariable firstCustomerVariable = context.Package.GetVariableForPath(@"\[CopyCustomers3]\[DFT Convert customer names].[FirstCustomer]");
            var customerName = (string)firstCustomerVariable.GetValue();
            Assert.AreEqual("pg", customerName);

            DtsVariable secondCustomerVariable = context.Package.GetVariableForPath(@"\[CopyCustomers3]\[DFT Convert customer names].[SecondCustomer]");
            customerName = (string)secondCustomerVariable.GetValue();
            Assert.AreEqual("irg", customerName);

            ReadOnlyCollection<DataTap> dataTaps = context.DataTaps;

            Assert.AreEqual(7, dataTaps.Count);

            CheckDataTap1(dataTaps);
            CheckDataTap2(dataTaps);
            CheckDataTap3(dataTaps);
            CheckDataTap4(dataTaps);
            CheckDataTap5(dataTaps);
            CheckDataTap6(dataTaps);
            CheckDataTap7(dataTaps);

            Assert.AreEqual(1, context.FakeDestinations.Count);

            FakeDestination fakeDestination = context.FakeDestinations[0];

            foreach (FakeDestinationSnapshot snapshot in fakeDestination.Snapshots)
            {
                string data = snapshot.LoadData();

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("911;LG;", rows[0]);
                Assert.AreEqual("1908;PG;", rows[1]);
                Assert.AreEqual("1924;PIL;", rows[2]);
                Assert.AreEqual("2401;IRG;", rows[3]);
            }
        }      

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }

        private static void CheckDataTap1(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap =
                dataTaps.Single(
                    tap =>
                        tap.PathToSourceComponent ==
                        @"\[CopyCustomers3]\[DFT Convert customer names]\[RCNT Count  customers]"
                        &&
                        tap.PathToDestinationComponent ==
                        @"\[CopyCustomers3]\[DFT Convert customer names]\[DER Convert names to upper string]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(4, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("911;lg;", rows[0]);
                Assert.AreEqual("1908;pg;", rows[1]);
                Assert.AreEqual("1924;pil;", rows[2]);
                Assert.AreEqual("2401;irg;", rows[3]);
            }
        }

        private static void CheckDataTap2(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap = dataTaps.Single(
                tap =>
                    tap.PathToSourceComponent ==
                    @"\[CopyCustomers3]\[DFT Convert customer names]\[DER Convert names to upper string]"
                    &&
                    tap.PathToDestinationComponent ==
                    @"\[CopyCustomers3]\[DFT Convert customer names]\[FFD Customers converted]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(4, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("911;LG;", rows[0]);
                Assert.AreEqual("1908;PG;", rows[1]);
                Assert.AreEqual("1924;PIL;", rows[2]);
                Assert.AreEqual("2401;IRG;", rows[3]);  
            }
        }

        private static void CheckDataTap3(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap = dataTaps.Single(
                tap =>
                    tap.PathToSourceComponent ==
                    @"\[CopyCustomers3]\[DFT Convert customer names]\[FFS Customers]"
                    &&
                    tap.PathToDestinationComponent ==
                    @"\[CopyCustomers3]\[DFT Convert customer names]\[RCNT Count error customers]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(2, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                StringAssert.Contains(rows[0], "1908;1;");
                StringAssert.Contains(rows[1], "2401;2;");
            }
        }

        private static void CheckDataTap4(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap = dataTaps.Single(
                tap =>
                    tap.PathToSourceComponent ==
                   @"\[CopyCustomers3]\[DFT Convert customer names]\[MLT Customers]"
                    &&
                    tap.PathToDestinationComponent ==
                   @"\[CopyCustomers3]\[DFT Convert customer names]\[ALL Dummy]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(4, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("911;lg;", rows[0]);
                Assert.AreEqual("1908;pg;", rows[1]);
                Assert.AreEqual("1924;pil;", rows[2]);
                Assert.AreEqual("2401;irg;", rows[3]);
            }
        }

        private static void CheckDataTap5(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap = dataTaps.Single(
                tap =>
                    tap.PathToSourceComponent ==
                   @"\[CopyCustomers3]\[DFT Convert customer names]\[CSPL Customers]"
                    &&
                    tap.PathToDestinationComponent ==
                   @"\[CopyCustomers3]\[DFT Convert customer names]\[SCR First Customer]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(1, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1908;pg;", rows[0]);
            }
        }

        private static void CheckDataTap6(IEnumerable<DataTap> dataTaps)
        {
            List<DataTap> dataTapResults = (from tap in dataTaps
                let a = tap.PathToSourceComponent == @"\[CopyCustomers3]\[DFT Convert customer names]\[CSPL Customers]"
                let b =
                    tap.PathToDestinationComponent == @"\[CopyCustomers3]\[DFT Convert customer names]\[ALL Customers]"
                let c = tap.PathName == "Third Customer"
                where a && b && c
                select tap).ToList();


            var dataTap = dataTapResults[0];
            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(1, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("911;lg;", rows[0]);
            }
            
        }

        private static void CheckDataTap7(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap = dataTaps.Single(
                tap =>
                    tap.PathToSourceComponent ==
                 @"\[CopyCustomers3]\[DFT Convert customer names]\[SCR Second Customer]"
                    &&
                    tap.PathToDestinationComponent ==
                   @"\[CopyCustomers3]\[DFT Convert customer names]\[ALL Customers]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(1, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("2401;irg;", rows[0]);
            }
        }
    }
}
