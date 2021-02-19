using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "LoadCustomers.dtsx", ExecutableName = @"\[LoadCustomers]\[SCR Check for new data fail if not ok]")]
    class LoadCustomersCheckScript : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);
            DtsVariable variable2 = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomersImported]");
            variable2.SetValue(true);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(false, context.Package.IsExecutionSuccess);
            Assert.AreEqual(false, context.ActiveExecutable.IsExecutionSuccess);

            DtsVariable variable = context.Package.GetVariable(@"CustomersImported");
            var value = (bool)variable.GetValue();

            Assert.AreEqual(false, value);
        }

        protected override void Teardown(TeardownContext context)
        {
            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }

    [UnitTest("DEMOISPAC", "LoadCustomers.dtsx", ExecutableName = @"\[LoadCustomers]\[SCR Check for new data fail if not ok]")]
    class LoadCustomersCheckScriptIspac : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);
            DtsVariable variable2 = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomersImported]");
            variable2.SetValue(true);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(false, context.Package.IsExecutionSuccess);
            Assert.AreEqual(false, context.ActiveExecutable.IsExecutionSuccess);

            DtsVariable variable = context.Package.GetVariable(@"CustomersImported");
            var value = (bool)variable.GetValue();

            Assert.AreEqual(false, value);
        }

        protected override void Teardown(TeardownContext context)
        {
            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            context.DataAccess.ExecuteNonQuery("truncate table [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();
        }
    }
}
