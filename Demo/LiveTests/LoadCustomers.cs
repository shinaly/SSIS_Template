using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.LiveTests
{
    [ActionClass("DEMO", "LoadCustomers.dtsx")]
    class LoadCustomers : BaseLiveTest
    {
        [ActionMethod(@"\[LoadCustomers]")]
        public void TestWholePackage(ActionContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            var countInsert = (int)variable.GetValue();

            Assert.AreEqual(3, countInsert);
        }

        [ActionMethod(@"\[LoadCustomers]\[SEQC Load]\[DFT Load customers]", Description = "Needs database access.")]
        public void TestLoadCustomers(ActionContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();

            Assert.AreEqual(3, rowCount);
        }

        [ActionMethod(@"\[LoadCustomers]\[SEQC Load]\[SQL Truncate CustomersStaging]")]
        public void TestTruncateCustomerStaging(ActionContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();

            Assert.AreEqual(0, rowCount);
        }
    }
}
