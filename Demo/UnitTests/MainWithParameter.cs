using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO2", "MainWithParameter.dtsx")]    
    class MainWithParameter : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsParameter param = context.Package.GetParameter("SourcePath");
            object val = param.GetValue();
            param.SetValue(Constants.CustomersFileSource);

            DtsVariable var1 = context.Package.GetVariable("DestinationPath");
            var1.SetValue(Constants.CustomersFileDestination);

            DtsVariable var2 = context.Package.GetVariable("ConvertDestinationPath");
            var2.SetValue(Constants.CustomersFileConverted);

            DtsVariable var3 = context.Package.GetVariable("CopyCustomersPath");
            var3.SetValue(Constants.PathToCopyCustomersPackage);

            DtsVariable var4 = context.Package.GetVariable("LoadCustomersPath");
            var4.SetValue(Constants.PathToLoadCustomersPackage);

            DtsVariable var5 = context.Package.GetVariable("ConnectionString");
            var5.SetValue(Constants.SsisDbConnectionString);
        }

        protected override void Verify(VerificationContext context)
        {
            var conn = context.Package.GetConnection("Customers Src");
            DtsParameter param = context.Package.GetParameter("SourcePath");
            object val = param.GetValue();

            Assert.IsTrue(File.Exists(Constants.CustomersFileDestination));

            context.DataAccess.OpenConnection(Constants.DbConnectionString);
            var rowCount = (int)context.DataAccess.ExecuteScalar(@"select count(*) from [dbo].[CustomersStaging]");
            context.DataAccess.CloseConnection();

            Assert.AreEqual(3, rowCount);
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
