using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomers.dtsx")]
    class CopyCustomersFileAll : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {           
            DtsVariable custSrc = context.Package.GetVariableForPath(@"\[CopyCustomers].[SourcePath]");
            custSrc.SetValue(Constants.CustomersFileSource);

            DtsVariable custDest = context.Package.GetVariableForPath(@"\[CopyCustomers].[DestinationPath]");
            custDest.SetValue(Constants.CustomersFileDestination);

            DtsVariable custDestNew = context.Package.GetVariable(@"ConvertDestinationPath");
            custDestNew.SetValue(Constants.CustomersFileConverted);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[CustomerCount]");
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

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[CustomerCount]");
            var count = (int)variable.GetValue();
            Assert.AreEqual(3, count);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }
    }

    [UnitTest("DEMOISPAC", "CopyCustomers.dtsx")]
    class CopyCustomersFileAllIspac : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsParameter custSrcParam = context.Package.Project.GetParameter(@"SourcePath");
            custSrcParam.SetValue(Constants.CustomersFileSource);

            DtsParameter custDesParam = context.Package.Project.GetParameter(@"DestinationPath");
            custDesParam.SetValue(Constants.CustomersFileDestination);

            DtsParameter custDesConvertedParam = context.Package.Project.GetParameter(@"ConvertDestinationPath");
            custDesConvertedParam.SetValue(Constants.CustomersFileConverted);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[CustomerCount]");
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

            DtsVariable variable = context.Package.GetVariableForPath(@"\[CopyCustomers]\[DFT Convert customer names].[CustomerCount]");
            var count = (int)variable.GetValue();
            Assert.AreEqual(3, count);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }
    }
}
