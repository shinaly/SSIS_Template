using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "LoadCustomers.dtsx", ExecutableName = @"\[LoadCustomers]\[SEQC Load]\[SQL Truncate CustomersStaging]")]
    class LoadCustomersTruncate : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            context.Package.GetConnection("CustomerDB").SetConnectionString(Constants.SsisDbConnectionString);

            const string query = @"insert into [dbo].[CustomersStaging]
                                    ([Id], [Name])
                                    values
                                    (1, 'ACME')";

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery(query);
            context.DataAccess.CloseConnection();
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();

            Assert.AreEqual(0, rowCount);
        }

        protected override void Teardown(TeardownContext context)
        {
            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }

    [UnitTest("DEMOISPAC", "LoadCustomers.dtsx", ExecutableName = @"\[LoadCustomers]\[SEQC Load]\[SQL Truncate CustomersStaging]")]
    class LoadCustomersTruncateIspac : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            context.Package.GetConnection("CustomerDB").SetConnectionString(Constants.SsisDbConnectionString);

            const string query = @"insert into [dbo].[CustomersStaging]
                                    ([Id], [Name])
                                    values
                                    (1, 'ACME')";

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery(query);
            context.DataAccess.CloseConnection();
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();

            Assert.AreEqual(0, rowCount);
        }

        protected override void Teardown(TeardownContext context)
        {
            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }
}
