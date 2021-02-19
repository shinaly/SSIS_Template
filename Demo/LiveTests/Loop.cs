using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.LiveTests
{
    [ActionClass("DEMO2", "Loop.dtsx")]
    class Loop : BaseLiveTest
    {
        [ActionMethod(@"\[Loop]")]
        public void TestWholePackage(ActionContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);
        }

        [ActionMethod(@"\[Loop]\[FOR]")]
        public void TestLoop(ActionContext context)
        {
            Assert.AreEqual(true, context.Package.IsExecutionSuccess);
            DtsVariable index = context.Package.GetVariableForPath(@"\[Loop]\[FOR].[index]");
            DtsVariable maxIterations = context.Package.GetVariableForPath(@"\[Loop]\[FOR].[maxIterations]");

            var indexVal = (int)index.GetValue();
            var maxIterationsVal = (int)maxIterations.GetValue();

            Assert.IsTrue(indexVal <= maxIterationsVal);
        }

        [ActionMethod(@"\[Loop]\[FOR]\[SCR Check index]")]
        public void TestScript(ActionContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
        }
    }
}
