using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "CopyCustomers.dtsx", ExecutableName = @"\[CopyCustomers]\[FST Copy Source File]\[OnPostExecute]")]
    class CopyCustomersFileEvent : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {
            
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            var val = (int)
                context.ActiveExecutable.GetVariable(@"ExecutedId").GetValue();

            Assert.AreEqual(1908, val);
        }

        protected override void Teardown(TeardownContext context)
        {

        }
    }

    [UnitTest("DEMOISPAC", "CopyCustomers.dtsx", ExecutableName = @"\[CopyCustomers]\[FST Copy Source File]\[OnPostExecute]")]
    class CopyCustomersFileEventIspac : BaseUnitTest
    {
        protected override void Setup(SetupContext context)
        {

        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);
            var val = (int)
                context.ActiveExecutable.GetVariable(@"ExecutedId").GetValue();

            Assert.AreEqual(1908, val);
        }

        protected override void Teardown(TeardownContext context)
        {

        }
    }
}
