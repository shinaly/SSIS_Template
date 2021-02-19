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
    [UnitTest("DEMO", "CopyCustomers2.dtsx", DisableLogging = true, Description = "This is the test for CopyCustomers with data tap & fake source.")]
    [DumpPackage]
    [DataTap(@"\[CopyCustomers2]\[DFT Convert customer names]\[RCNT Count  customers]", @"\[CopyCustomers2]\[DFT Convert customer names]\[DER Convert names to upper string]")]
    [DataTap(@"\[CopyCustomers2]\[DFT Convert customer names]\[DER Convert names to upper string]", @"\[CopyCustomers2]\[DFT Convert customer names]\[FFD Customers converted]")]
    [FakeSource(@"\[CopyCustomers2]\[DFT Convert customer names]\[SCR Customers]",
        new[] { "Even Ids", "Odd Ids" },
        new[] { "1,Company A\r\n3,Company B\r\n5,Company C", "2,Company D\r\n4,Company E\r\n6,Company F" },
        RowDelimiter = "\r\n", ColumnDelimiter = ",")]
    public class CopyCustomersFileAll2 : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable custDestNew = context.Package.GetVariableForPath(@"\[CopyCustomers2].[ConvertDestinationPath]");
            custDestNew.SetValue(Constants.CustomersFileConverted);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers2]\[DFT Convert customer names].[CustomerCount]");
            variable.SetValue(0);
        }

        protected override void Verify(VerificationContext context)
        {
            context.Package.ThrowExceptionOnError();

            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            bool exists =
               File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, exists);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers2]\[DFT Convert customer names].[CustomerCount]");
            int count = (int)variable.GetValue();
            Assert.AreEqual(6, count);

            ReadOnlyCollection<DataTap> dataTaps = context.DataTaps;

            CheckDataTap1(dataTaps);
            CheckDataTap2(dataTaps);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileConverted);
        }

        private static void CheckDataTap1(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap =
                dataTaps.Single(
                    tap =>
                        tap.PathToSourceComponent ==
                        @"\[CopyCustomers2]\[DFT Convert customer names]\[RCNT Count  customers]"
                        &&
                        tap.PathToDestinationComponent ==
                        @"\[CopyCustomers2]\[DFT Convert customer names]\[DER Convert names to upper string]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(6, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1;Company A;", rows[0]);
                Assert.AreEqual("3;Company B;", rows[1]);
                Assert.AreEqual("5;Company C;", rows[2]);
                Assert.AreEqual("2;Company D;", rows[3]);
                Assert.AreEqual("4;Company E;", rows[4]);
                Assert.AreEqual("6;Company F;", rows[5]);
            }
        }

        private static void CheckDataTap2(IEnumerable<DataTap> dataTaps)
        {
            DataTap dataTap = dataTaps.Single(
                tap =>
                    tap.PathToSourceComponent ==
                    @"\[CopyCustomers2]\[DFT Convert customer names]\[DER Convert names to upper string]"
                    &&
                    tap.PathToDestinationComponent ==
                    @"\[CopyCustomers2]\[DFT Convert customer names]\[FFD Customers converted]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(6, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1;COMPANY A;", rows[0]);
                Assert.AreEqual("3;COMPANY B;", rows[1]);
                Assert.AreEqual("5;COMPANY C;", rows[2]);
                Assert.AreEqual("2;COMPANY D;", rows[3]);
                Assert.AreEqual("4;COMPANY E;", rows[4]);
                Assert.AreEqual("6;COMPANY F;", rows[5]);
            }
        }
    }
}
