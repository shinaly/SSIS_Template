using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.UnitTests
{
    [UnitTest("DEMO", "LoadCustomers.dtsx", PrecedenceConstraintsTestOnly = true)]
    class LoadCustomersConstraints : BaseUnitTest
    {
        private DtsPrecedenceConstraint _dummyConstraint;
        private DtsPrecedenceConstraint _countConstraint;

        protected override void Setup(SetupContext context)
        {
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);

            _dummyConstraint = context.Package.GetPrecedingConstraintForPath(@"\[LoadCustomers]\[SCR Dummy].[DummyConstraint]");
            _dummyConstraint.SetExecutionResult(DtsExecutionResult.Success);

            _countConstraint = context.Package.GetPrecedingConstraintForPath(@"\[LoadCustomers]\[SEQC Load]\[SQL Get Count of data in destination].[CountConstraint]");
            _countConstraint.SetExecutionResult(DtsExecutionResult.Success);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(false, _dummyConstraint.Evaluate());
            Assert.AreEqual(true, _countConstraint.Evaluate());
        }

        protected override void Teardown(TeardownContext context)
        {
        }
    }

    [UnitTest("DEMOISPAC", "LoadCustomers.dtsx", PrecedenceConstraintsTestOnly = true)]
    class LoadCustomersConstraintsIspac : BaseUnitTest
    {
        private DtsPrecedenceConstraint _dummyConstraint;
        private DtsPrecedenceConstraint _countConstraint;

        protected override void Setup(SetupContext context)
        {
            DtsVariable variable = context.Package.GetVariableForPath(@"\[LoadCustomers].[CustomerCount]");
            variable.SetValue(0);

            _dummyConstraint = context.Package.GetPrecedingConstraintForPath(@"\[LoadCustomers]\[SCR Dummy].[DummyConstraint]");
            _dummyConstraint.SetExecutionResult(DtsExecutionResult.Success);

            _countConstraint = context.Package.GetPrecedingConstraintForPath(@"\[LoadCustomers]\[SEQC Load]\[SQL Get Count of data in destination].[CountConstraint]");
            _countConstraint.SetExecutionResult(DtsExecutionResult.Success);
        }

        protected override void Verify(VerificationContext context)
        {
            Assert.AreEqual(false, _dummyConstraint.Evaluate());
            Assert.AreEqual(true, _countConstraint.Evaluate());
        }

        protected override void Teardown(TeardownContext context)
        {
        }
    }
}
