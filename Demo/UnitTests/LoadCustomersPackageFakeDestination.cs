using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "LoadCustomers.dtsx", ConnectionsToRemove = "CustomerDB")]
    [DumpPackage]
    [DisableExecutable(@"\LoadCustomers\SEQC Load\SQL Truncate CustomersStaging")]
    [DisableExecutable(@"\LoadCustomers\SEQC Load\SQL Get Count of data in destination")]
    [DisableExecutable(@"\LoadCustomers\SCR Check for new data fail if not ok")]
    [FakeDestination(@"\[LoadCustomers]\[SEQC Load]\[DFT Load customers]\[OLEDST CustomersStaging]", SendRowsToErrorOutput = false)]
    class LoadCustomersPackageFakeDestination : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            File.Copy(Constants.CustomersWithErrorFileSource, Constants.CustomersFileDestination);

            context.Package.GetConnection("Customers Src").SetConnectionString(Constants.CustomersWithErrorFileSource);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            Assert.AreEqual(1, context.FakeDestinations.Count);

            FakeDestination fakeDestination = context.FakeDestinations[0];

            foreach (FakeDestinationSnapshot snapshot in fakeDestination.Snapshots)
            {
                string data = snapshot.LoadData();

                string[] rows = data.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual("1;Company1;", rows[0]);
                Assert.AreEqual("5;Company2;", rows[1]);
                Assert.AreEqual("11;Company3;", rows[2]);
            }

            // enable this part, if SendRowsToErrorOutput = true
            //DtsVariable variable = context.Package.GetVariable("ErrorCustomersCount");
            //Assert.AreEqual(4, (int)variable.GetValue());
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }
}
