using System;

namespace SSIS.Test.Template
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SetTitle();

            TestRunner.RunUnitTests();
            //TestRunner.RunUnitTests2();
            //TestRunner.RunUnitTests3();

            // to run live tests, comment the RunUnitTests and uncomment the RunLiveTests
            //TestRunner.RunLiveTests();
            //TestRunner.RunLiveTests2();
            //TestRunner.RunLiveTests3();
            //TestRunner.RunLiveTests4();
            //TestRunner.RunLiveTests5();
            //TestRunner.RunLiveTests6();
            //TestRunner.RunLiveTests7();
			//TestRunner.RunLiveTests8();

            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

        private static void SetTitle()
        {
            string title = "SSISTester ver. " + EngineFactory.EngineVersion + " SSIS ver. " + EngineFactory.RuntimeVersion;
            Console.Title = title;
        }
    }
}
