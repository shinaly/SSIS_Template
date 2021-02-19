using SSIS.Test.Report;
using SSIS.Test.Template.Demo;

namespace SSIS.Test.Template
{
    class TestRunner
    {
        /// <summary>
        /// Executes demo unit tests.
        /// </summary>
        internal static void RunUnitTests()
        {
            // create engine for unit testing
            var engine = EngineFactory.GetClassInstance<IUnitTestEngine>();

            // load packages and relate them to the logical group - repository
            engine.LoadPackages("DEMO", Constants.PathToPackages);

            // load unit tests from the current assembly
            engine.LoadRepositoryUnitTests("DEMO");

            // execute unit tests
            engine.ExecuteUnitTestsWithGui();
        }

        /// <summary>
        /// Executes demo2 unit tests.
        /// </summary>
        internal static void RunUnitTests2()
        {
            // create engine for unit testing
            var engine = EngineFactory.GetClassInstance<IUnitTestEngine>();

            // load packages and relate them to the logical group - repository
            engine.LoadPackages("DEMO2", Constants.PathToPackages);

            // load unit tests from the current assembly
            engine.LoadRepositoryUnitTests("DEMO2");

            // execute unit tests
            engine.ExecuteUnitTestsWithGui();
        }

        /// <summary>
        /// Executes demo unit tests.
        /// </summary>
        internal static void RunUnitTests3()
        {
            // create engine for unit testing
            var engine = EngineFactory.GetClassInstance<IUnitTestEngine>();

            // load packages and relate them to the logical group - repository
            engine.LoadPackages("DEMOISPAC", Constants.PathToIspac);

            // load unit tests from the current assembly
            engine.LoadRepositoryUnitTests("DEMOISPAC");

            // execute unit tests
            engine.ExecuteUnitTestsWithGui();
        }

        /// <summary>
        /// Executes demo live tests.
        /// </summary>
        internal static void RunLiveTests()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();

            // add variables and their values to be set when the package has been loaded
            parameters.AddVariable(@"\[Main].[ConnectionString]", Constants.SsisDbConnectionString);
            parameters.AddVariable(@"\[Main].[CopyCustomersPath]", Constants.PathToCopyCustomersPackage);
            parameters.AddVariable(@"\[Main].[LoadCustomersPath]", Constants.PathToLoadCustomersPackage);
            parameters.AddVariable(@"\[Main].[ConvertDestinationPath]", Constants.CustomersFileConverted);
            parameters.AddVariable(@"\[Main].[DestinationPath]", Constants.CustomersFileDestination);
            parameters.AddVariable(@"\[Main].[SourcePath]", Constants.CustomersFileSource);

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository            
            engine.LoadPackages("DEMO", Constants.PathToPackages);

            // load live tests from the current assembly
            engine.LoadRepositoryActions("DEMO");

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests            
            engine.ExecuteLiveTestsWithGui("DEMO", "Main.dtsx");
        }

        /// <summary>
        /// Executes demo2 live tests with loop.
        /// </summary>
        internal static void RunLiveTests2()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository
            engine.LoadPackages("DEMO2", Constants.PathToPackages);

            // load live tests from the current assembly
            engine.LoadRepositoryActions("DEMO2");

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests
            engine.ExecuteLiveTestsWithGui("DEMO2", "Loop.dtsx");
        }

        /// <summary>
        /// Executes demo2 live tests with main package and data flow.
        /// </summary>
        internal static void RunLiveTests3()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();

            // add variables and their values to be set when the package has been loaded
            parameters.AddVariable(@"\[MainWithDataFlow].[ConnectionString]", Constants.SsisDbConnectionString);
            parameters.AddVariable(@"\[MainWithDataFlow].[CopyCustomersPath]", Constants.PathToCopyCustomersPackage);
            parameters.AddVariable(@"\[MainWithDataFlow].[LoadCustomersPath]", Constants.PathToLoadCustomersPackage);
            parameters.AddVariable(@"\[MainWithDataFlow].[ConvertDestinationPath]", Constants.CustomersFileConverted);
            parameters.AddVariable(@"\[MainWithDataFlow].[DestinationPath]", Constants.CustomersFileDestination);
            parameters.AddVariable(@"\[MainWithDataFlow].[SourcePath]", Constants.CustomersFileSource);

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository
            engine.LoadPackages("DEMO2", Constants.PathToPackages);

            // load live tests from the current assembly
            engine.LoadRepositoryActions("DEMO2");

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests
            engine.ExecuteLiveTestsWithGui("DEMO2", "MainWithDataFlow.dtsx");

        }

        /// <summary>
        /// Executes demo live tests with main package that has a parameter.
        /// </summary>
        internal static void RunLiveTests4()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();
            
            // add variables and their values to be set when the package has been loaded
            parameters.AddVariable(@"\[MainWithParameter].[ConnectionString]", Constants.SsisDbConnectionString);
            parameters.AddVariable(@"\[MainWithParameter].[CopyCustomersPath]", Constants.PathToCopyCustomersPackage);
            parameters.AddVariable(@"\[MainWithParameter].[LoadCustomersPath]", Constants.PathToLoadCustomersPackage);
            parameters.AddVariable(@"\[MainWithParameter].[ConvertDestinationPath]", Constants.CustomersFileConverted);
            parameters.AddVariable(@"\[MainWithParameter].[DestinationPath]", Constants.CustomersFileDestination);

            // add  package parameters
            parameters.AddPackageParameter(@"SourcePath", Constants.CustomersFileSource);

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository
            engine.LoadPackages("DEMO", Constants.PathToPackages);

            // load live tests from the current assembly
            engine.LoadRepositoryActions("DEMO");

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests
            engine.ExecuteLiveTestsWithGui("DEMO", "MainWithParameter.dtsx");
        }

        /// <summary>
        /// Executes demo live tests.
        /// </summary>
        internal static void RunLiveTests5()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();

            // add variables and their values to be set when the package has been loaded
            parameters.AddVariable(@"\[Main].[ConnectionString]", Constants.SsisDbConnectionString);
            parameters.AddVariable(@"\[Main].[CopyCustomersPath]", Constants.PathToCopyCustomersPackage);
            parameters.AddVariable(@"\[Main].[LoadCustomersPath]", Constants.PathToLoadCustomersPackage);
            parameters.AddVariable(@"\[Main].[ConvertDestinationPath]", Constants.CustomersFileConverted);
            parameters.AddVariable(@"\[Main].[DestinationPath]", Constants.CustomersFileDestination);
            parameters.AddVariable(@"\[Main].[SourcePath]", Constants.CustomersFileSource);

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository            
            engine.LoadPackages("DEMO3", Constants.PathToPackages);

            // load live tests from the current assembly
            engine.LoadRepositoryActions("DEMO3");

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests            
            engine.ExecuteLiveTestsWithGui("DEMO3", "Main.dtsx");
        }

        /// <summary>
        /// Executes demo live tests.
        /// </summary>
        internal static void RunLiveTests6()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();

            // add variables and their values to be set when the package has been loaded
            parameters.AddConfigurationString("ConnectionStrings", Constants.PathToDtsConfig);

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository
            engine.LoadPackages("DEMO4", Constants.PathToPackages);

            // load live tests from the current assembly
            engine.LoadRepositoryActions("DEMO4");

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests
            engine.ExecuteLiveTestsWithGui("DEMO4", "CopyCustomersWithConfig.dtsx");
        }

        /// <summary>
        /// Executes demo live tests.
        /// </summary>
        internal static void RunLiveTests7()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();

            // add variables and their values to be set when the package has been loaded           
            parameters.AddProjectConnectionString("Customers Src", Constants.CustomersFileSource);
            parameters.AddProjectParameter("ConvertDestinationPath", Constants.CustomersFileConverted);
            parameters.AddProjectParameter("DestinationPath", Constants.CustomersFileDestination);
            parameters.AddProjectParameter("SourcePath", Constants.CustomersFileSource);

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository
            const string repositoryName = "DEMO5";

            int cnt = engine.LoadPackages(repositoryName, Constants.PathToIspac);

            // load live tests from the current assembly
            engine.LoadRepositoryActions(repositoryName);

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests
            engine.ExecuteLiveTestsWithGui(repositoryName, "Main.dtsx");
        }
		
		/// <summary>
        /// Executes demo live tests.
        /// </summary>
        internal static void RunLiveTests8()
        {
            // set package execution parameters eg. variables, connections
            var parameters = new ExecutionParameters();

            // add variables and their values to be set when the package has been loaded
            parameters.AddVariable(@"\[Parent].[ConvertDestinationPath]", Constants.CustomersFileConverted);
            parameters.AddVariable(@"\[Parent].[DestinationPath]", Constants.CustomersFileDestination);
            parameters.AddVariable(@"\[Parent].[SourcePath]", Constants.CustomersFileSource);
            parameters.AddVariable(@"\[Parent].[PathToChildPackage]", Constants.PathToCopyCustomersPackage);

            // create engine for live testing
            var engine = EngineFactory.GetClassInstance<ILiveTestEngine>();

            // load packages and relate them to the logical group - repository
            const string repositoryName = "DEMO6";

            int cnt = engine.LoadPackages(repositoryName, Constants.PathToPackages);

            // load live tests from the current assembly
            engine.LoadRepositoryActions(repositoryName);

            // set execution parameters
            engine.SetExecutionParameters(parameters);

            // execute the package and attach live tests
            engine.ExecuteLiveTestsWithGui(repositoryName, "Parent.dtsx");
        }
    }
}
