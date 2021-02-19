using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "LoadCustomers.dtsx", ExecutableName = @"\[LoadCustomers]\[SEQC Load]\[SQL Get Count of data in destination]")]
    class LoadCustomersCount : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            context.Package.GetConnection("CustomerDB").SetConnectionString(Constants.SsisDbConnectionString);
            context.Package.GetConnection("Customers Src").SetConnectionString(Constants.CustomersFileSource);
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);

            const string query = @"insert into [dbo].[CustomersStaging]
                                    ([Id], [Name])
                                    values
                                    (1, 'ACME')
                                    ,(2, 'ALL')";

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
            
            DtsVariable variable = context.Package.GetVariable(@"CustomerCount");
            var countInsert = (int) variable.GetValue();

            Assert.AreEqual(2, rowCount);
            Assert.AreEqual(2, countInsert);
        }

        protected override void Teardown(TeardownContext context)
        {
            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }

    [UnitTest("DEMOISPAC", "LoadCustomers.dtsx", ExecutableName = @"\[LoadCustomers]\[SEQC Load]\[SQL Get Count of data in destination]")]
    class LoadCustomersCountIspac : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            context.Package.GetConnection("CustomerDB").SetConnectionString(Constants.SsisDbConnectionString);
            context.Package.GetConnection("Customers Src").SetConnectionString(Constants.CustomersFileSource);
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);

            const string query = @"insert into [dbo].[CustomersStaging]
                                    ([Id], [Name])
                                    values
                                    (1, 'ACME')
                                    ,(2, 'ALL')";

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

            DtsVariable variable = context.Package.GetVariable(@"CustomerCount");
            var countInsert = (int)variable.GetValue();

            Assert.AreEqual(2, rowCount);
            Assert.AreEqual(2, countInsert);
        }

        protected override void Teardown(TeardownContext context)
        {
            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }
}
