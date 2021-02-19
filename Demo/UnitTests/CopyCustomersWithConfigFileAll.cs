using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomersWithConfig.dtsx")]
    class CopyCustomersWithConfigFileAll : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            ReplaceConnectionStrings();

            var config = context.Package.GetConfiguration("ConnectionStrings");
            config.SetConfigurationString(Constants.PathToDtsConfig);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomersWithConfig]\[DFT Convert customer names].[CustomerCount]");
            variable.SetValue(0);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);

            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, exists);

            exists =
               File.Exists(Constants.CustomersFileConverted);
            
            Assert.AreEqual(true, exists);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomersWithConfig]\[DFT Convert customer names].[CustomerCount]");
            var count = (int)variable.GetValue();
            Assert.AreEqual(3, count);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }

        private static void ReplaceConnectionStrings()
        {
            string content = "<?xml version=\"1.0\"?>" +
                             "<DTSConfiguration>" +
                             "<DTSConfigurationHeading><DTSConfigurationFileInfo GeneratedBy=\"pavgud\" GeneratedFromPackageName=\"CopyCustomersWithConfig\" GeneratedFromPackageID=\"{8B09978A-50D5-409F-B2C7-D73413E0CDB9}\" GeneratedDate=\"22.07.2014 20:44:53\"/></DTSConfigurationHeading>" +
                             "<Configuration ConfiguredType=\"Property\" Path=\"\\Package.Connections[Convert Customers Dst].Properties[ConnectionString]\" ValueType=\"String\"><ConfiguredValue>" + Constants.CustomersFileConverted + "</ConfiguredValue></Configuration>" +
                             "<Configuration ConfiguredType=\"Property\" Path=\"\\Package.Connections[Customers Dst].Properties[ConnectionString]\" ValueType=\"String\"><ConfiguredValue>" + Constants.CustomersFileDestination + "</ConfiguredValue></Configuration>" +
                             "<Configuration ConfiguredType=\"Property\" Path=\"\\Package.Connections[Customers Src].Properties[ConnectionString]\" ValueType=\"String\"><ConfiguredValue>" + Constants.CustomersFileSource + "</ConfiguredValue></Configuration>" +
                             "</DTSConfiguration>";

            File.WriteAllText(Constants.PathToDtsConfig, content);

        }
    }
}
