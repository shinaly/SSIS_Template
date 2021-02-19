namespace SSIS.Test.Template.LiveTests
{
    /*
     
     Each live test is implemented in a class which derives from LiveUnitTest.
     In order for a class to be recognized as a live test, you have to decorate 
     it with a ActionClass attribute. ActionClass attribute defines the package 
     and repository where the package resides. The name of the class implementing 
     BaseLiveTest is not important. For each tested control flow element or a 
     package, one test method has to be implemented. Test methods are decorated 
     with the ActionMethod attribute and may have only one parameter of the type 
     ActionContext. ActionMethod defines which control flow element is tested. It 
     can be the package as well. The name of the test method is not important.

     */

    /*
    [ActionClass("<repository name>", "<package name>")]
    class LiveTestImplementation1 : BaseLiveTest
    {
        // TODO: define your instance variables here

        [ActionMethod("<path to executable>")]
        public void TestDataFlow(ActionContext context)
        {
            // TODO: verify the test results for a control flow element
        }

        [ActionMethod("<package name>")]
        public void TestPackage(ActionContext context)
        {
            // TODO: verify the test results for a package
        }
    }
    */
}
