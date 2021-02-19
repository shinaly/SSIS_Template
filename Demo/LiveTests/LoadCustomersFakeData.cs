using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.LiveTests
{
    [ActionClass("DEMO3", "LoadCustomers.dtsx")]
    [FakeDestination(@"\[LoadCustomers]\[SEQC Load]\[DFT Load customers]\[OLEDST CustomersStaging]")]
    class LoadCustomersFakeData : BaseLiveTest
    {
        [ActionMethod(@"\[LoadCustomers]")]
        public void TestWholePackage(ActionContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);
        }

        [ActionMethod(@"\[LoadCustomers]\[SEQC Load]\[DFT Load customers]")]
        public void TestLoadCustomers(ActionContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();

            // here we expect 0 since we use FakeData
            Assert.AreEqual(0, rowCount);

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
    }
}
