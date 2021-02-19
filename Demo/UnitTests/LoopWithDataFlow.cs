using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO2", "LoopWithDataFlow.dtsx")]
    [DumpPackage]
    [DataTap(@"\[LoopWithDataFlow]\[FOR]\[DFT Convert customer names]\[FFS Customers]", @"\[LoopWithDataFlow]\[FOR]\[DFT Convert customer names]\[DER Add index column]")]
    [DataTap(@"\[LoopWithDataFlow]\[FOR]\[DFT Convert customer names]\[DER Add index column]", @"\[LoopWithDataFlow]\[FOR]\[DFT Convert customer names]\[RCNT Count  customers]")]
    class LoopWithDataFlow1 : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            DtsVariable custSrc = context.Package.GetVariableForPath(@"\[LoopWithDataFlow].[SourcePath]");
            custSrc.SetValue(Constants.CustomersFileSource);

            DtsVariable custDest = context.Package.GetVariableForPath(@"\[LoopWithDataFlow].[DestinationPath]");
            custDest.SetValue(Constants.CustomersFileDestination);

            DtsVariable custDestNew = context.Package.GetVariable(@"ConvertDestinationPath");
            custDestNew.SetValue(Constants.CustomersFileConverted);

            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoopWithDataFlow]\[FOR]\[DFT Convert customer names].[CustomerCount]");
            variable.SetValue(0);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.IsTrue(context.Package.IsExecutionSuccess);
            
            DtsVariable indexVar = context.Package.GetVariable("index");
            var indexVarVal = (int)indexVar.GetValue();
            Assert.IsTrue(indexVarVal == 3);

            DtsVariable customerCntVar = context.Package.GetVariableForPath(@"\[LoopWithDataFlow]\[FOR]\[DFT Convert customer names].[CustomerCount]");
            var customerCntVarVal = (int)customerCntVar.GetValue();
            Assert.IsTrue(customerCntVarVal == 3);

            var dt = context.DataTaps;
        }

        protected override void Teardown(TeardownContext context)
        {
        }
    }
}
