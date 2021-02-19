using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO2", "LoopWithDataFlow.dtsx", ExecutableName = @"\[LoopWithDataFlow]\[FOR]")]
    class LoopWithDataFlow2 : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            context.Package.GetVariable("index").SetValue(0);
            context.Package.GetVariable("maxIterations").SetValue(5);
            context.Package.GetVariable("SourcePath").SetValue(Constants.CustomersFileSource);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.IsTrue(context.ActiveExecutable.IsExecutionSuccess);
            
            DtsVariable indexVar = context.Package.GetVariable("index");
            var indexVarVal = (int)indexVar.GetValue();
            Assert.IsTrue(indexVarVal == 5);

            DtsVariable customerCntVar = context.Package.GetVariableForPath(@"\[LoopWithDataFlow]\[FOR]\[DFT Convert customer names].[CustomerCount]");
            var customerCntVarVal = (int)customerCntVar.GetValue();
            Assert.IsTrue(customerCntVarVal == 3);
        }

        protected override void Teardown(TeardownContext context)
        {
        }
    }
}