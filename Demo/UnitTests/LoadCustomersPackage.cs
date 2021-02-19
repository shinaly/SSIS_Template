using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "LoadCustomers.dtsx")]
    class LoadCustomersPackage : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            File.Copy(Constants.CustomersFileSource, Constants.CustomersFileDestination);

            context.Package.GetConnection("CustomerDB").SetConnectionString(Constants.SsisDbConnectionString);
            context.Package.GetConnection("Customers Src").SetConnectionString(Constants.CustomersFileSource);
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);
            DtsVariable variable2 = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomersImported]");
            variable2.SetValue(false);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
            
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            var countInsert = (int) variable.GetValue();

            DtsVariable variable2 = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomersImported]");
            var value = (bool)variable2.GetValue();

            Assert.AreEqual(3, rowCount);
            Assert.AreEqual(3, countInsert);
            Assert.AreEqual(true, value);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }

    [UnitTest("DEMOISPAC", "LoadCustomers.dtsx")]
    class LoadCustomersPackageIspac : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            File.Copy(Constants.CustomersFileSource, Constants.CustomersFileDestination);

            context.Package.GetConnection("CustomerDB").SetConnectionString(Constants.SsisDbConnectionString);
            context.Package.GetConnection("Customers Src").SetConnectionString(Constants.CustomersFileSource);
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);
            DtsVariable variable2 = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomersImported]");
            variable2.SetValue(false);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();

            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            var countInsert = (int)variable.GetValue();

            DtsVariable variable2 = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomersImported]");
            var value = (bool)variable2.GetValue();

            Assert.AreEqual(3, rowCount);
            Assert.AreEqual(3, countInsert);
            Assert.AreEqual(true, value);
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
