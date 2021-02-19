namespace SSIS.Test.Template.UnitTests
{
    /*

     Each unit test is implemented in a class which derives from BaseUnitTest.
     Setup, Verify and Teardown methods have to be implemented. You can use Setup method 
     to prepare test. In Verify method you can check the results. Purpose of Teardown is 
     to allow you to clean test environment after the package has executed. 

     In order for a class to be recognized as a unit test, you have to decorate it with a 
     UnitTest attribute. UnitTest attribute defines the package or control flow element to 
     be tested, and repository where the package resides. The name of the class implementing 
     BaseUnitTest is not important.

     */

    /*
    [UnitTest("<repository name>", "<package name>", ExecutableName = "<path to executable>")]
    class UnitTestImplementation1 : BaseUnitTest
    {
        // TODO: define your instance variables here

        protected override void Setup(SetupContext context)
        {
            // TODO: setup your test
        }

        protected override void Verify(VerificationContext context)
        {
            // TODO: verify the test results
        }

        protected override void Teardown(TeardownContext context)
        {
            // TODO: cleanup database, files...
        }
    }
    */
}
