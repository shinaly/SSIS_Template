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
    [UnitTest("DEMO", "CopyCustomers4.dtsx", DisableLogging = true)]
    [DumpPackage]
    [DataTap(@"\[CopyCustomers4]\[DFT Convert customer names]\[MRGJ Customers Products]", @"\[CopyCustomers4]\[DFT Convert customer names]\[RCNT Count Customers]")]
    public class CopyCustomersFileAll4 : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable custSrc = context.Package.GetVariableForPath(@"\[CopyCustomers4].[SourcePath]");
            custSrc.SetValue(Constants.CustomersFileSource);

            DtsVariable custDest = context.Package.GetVariableForPath(@"\[CopyCustomers4].[DestinationPath]");
            custDest.SetValue(Constants.CustomersFileDestination);

            DtsVariable custDestNew = context.Package.GetVariableForPath(@"\[CopyCustomers4].[ConvertDestinationPath]");
            custDestNew.SetValue(Constants.CustomersFileConverted);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers4]\[DFT Convert customer names].[CustomerCount]");
            variable.SetValue(0);

            DtsConnection newCustomersConnection = context.Package.GetConnection("New Customers Src");
            newCustomersConnection.SetConnectionString(Constants.CustomersNewFileSource);

            DtsConnection productConnection = context.Package.GetConnection("Products Src");
            productConnection.SetConnectionString(Constants.ProductsFileSource);
        }

        protected override void Verify(VerificationContext context)
        {
            context.Package.ThrowExceptionOnError();

            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, exists);

            exists =
               File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, exists);

            DtsVariable customerCountVariable = context.Package.GetVariableForPath(@"\[CopyCustomers4]\[DFT Convert customer names].[CustomerCount]");
            var customersCount = (int)customerCountVariable.GetValue();
            Assert.AreEqual(13, customersCount);

            DtsVariable sourceCustomersErrorCountVariable = context.Package.GetVariableForPath(@"\[CopyCustomers4]\[DFT Convert customer names].[SourceCustomersErrorCount]");
            var errorsCount = (int)sourceCustomersErrorCountVariable.GetValue();
            Assert.AreEqual(0, errorsCount);

            ReadOnlyCollection<DataTap> dataTaps = context.DataTaps;

            Assert.AreEqual(1, dataTaps.Count);

            CheckDataTap1(dataTaps);
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
                        @"\[CopyCustomers4]\[DFT Convert customer names]\[MRGJ Customers Products]"
                        &&
                        tap.PathToDestinationComponent ==
                        @"\[CopyCustomers4]\[DFT Convert customer names]\[RCNT Count Customers]");

            Assert.AreEqual(dataTap.Snapshots.Count, dataTap.SnapshotCount);
            Assert.AreEqual(1, dataTap.SnapshotCount);

            foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
            {
                string data = snapshot.LoadData();
                Assert.IsFalse(string.IsNullOrEmpty(data));
                Assert.AreEqual(13, snapshot.RowCount);

                string[] rows = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                int i = 0;
                Assert.AreEqual("1;Company1;table;1;", rows[i]);
                Assert.AreEqual("1;Company1;chair;4;", rows[++i]);
                Assert.AreEqual("1;Company1;glass;5;", rows[++i]);
                Assert.AreEqual("2;furniture co;;;", rows[++i]);
                Assert.AreEqual("5;Company2;car;2;", rows[++i]);
                Assert.AreEqual("5;Company2;motor;6;", rows[++i]);
                Assert.AreEqual("5;Company2;radio;9;", rows[++i]);
                Assert.AreEqual("5;Company2;navi;10;", rows[++i]);
                Assert.AreEqual("7;used cars;;;", rows[++i]);
                Assert.AreEqual("11;Company3;laptop;3;", rows[++i]);
                Assert.AreEqual("11;Company3;monitor;7;", rows[++i]);
                Assert.AreEqual("11;Company3;mouse;8;", rows[++i]);
                Assert.AreEqual("18;computers;;;", rows[++i]);
            }
        }
    }
}
