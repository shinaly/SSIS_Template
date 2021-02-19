using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomers.dtsx", ExecutableName = @"\[CopyCustomers]\[FST Copy Source File]")]
    class CopyCustomersFileCopy : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable custSrc = context.Package.GetVariableForPath(@"\[CopyCustomers].[SourcePath]");
            custSrc.SetValue(Constants.CustomersFileSource);

            DtsVariable custDest = context.Package.GetVariable(@"DestinationPath");
            custDest.SetValue(Constants.CustomersFileDestination);
        }

        protected override void Verify(VerificationContext context)
        {
            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
        }
    }

    [UnitTest("DEMOISPAC", "CopyCustomers.dtsx", ExecutableName = @"\[CopyCustomers]\[FST Copy Source File]")]
    class CopyCustomersFileCopyIspac : BaseUnitTest
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
            bool exists =
               File.Exists(Constants.CustomersFileDestination);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
        }
    }
}
