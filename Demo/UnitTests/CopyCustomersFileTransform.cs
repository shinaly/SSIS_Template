using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomers.dtsx", ExecutableName = @"\[CopyCustomers]\[DFT Convert customer names]")]
    class CopyCustomersFileTransform : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            File.Copy(Constants.CustomersFileSource, Constants.CustomersFileDestination);

            DtsVariable custDest = context.Package.GetVariableForPath(@"\[CopyCustomers].[DestinationPath]");
            custDest.SetValue(Constants.CustomersFileDestination);

            DtsVariable custDestNew = context.Package.GetVariable(@"ConvertDestinationPath");
            custDestNew.SetValue(Constants.CustomersFileConverted);
        }

        protected override void Verify(VerificationContext context)
        {
            bool exists =
               File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }
    }

    [UnitTest("DEMOISPAC", "CopyCustomers.dtsx", ExecutableName = @"\[CopyCustomers]\[DFT Convert customer names]")]
    class CopyCustomersFileTransformIspac : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            File.Copy(Constants.CustomersFileSource, Constants.CustomersFileDestination);

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
               File.Exists(Constants.CustomersFileConverted);

            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            Assert.AreEqual(true, exists);
        }

        protected override void Teardown(TeardownContext context)
        {
            File.Delete(Constants.CustomersFileDestination);
            File.Delete(Constants.CustomersFileConverted);
        }
    }
}
