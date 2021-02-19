using System.Configuration;

namespace SSIS.Test.Template.Demo
{
    class Constants
    {
        public static string DbConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString; }
        }

        public static string SsisDbConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SsisDbConnectionString"].ConnectionString; }
        }

        private static readonly string PathToProjectFiles = new System.IO.DirectoryInfo(
            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).Parent.Parent.FullName;
        public static readonly string ProductsFileSource = System.IO.Path.Combine(PathToProjectFiles, @"Files\Products.txt");
        public static readonly string CustomersFileSource = System.IO.Path.Combine(PathToProjectFiles, @"Files\Customers.txt");
        public static readonly string CustomersWithErrorFileSource = System.IO.Path.Combine(PathToProjectFiles, @"Files\CustomersWithError.txt");
        public static readonly string CustomersNewFileSource = System.IO.Path.Combine(PathToProjectFiles, @"Files\NewCustomers.txt");
        public static readonly string CustomersFileDestination = System.IO.Path.Combine(PathToProjectFiles, @"Files\Destination\Customers.txt");
        public static readonly string CustomersFileConverted = System.IO.Path.Combine(PathToProjectFiles, @"Files\Destination\ConvertedCustomers.txt");
        public static readonly string PathToDtsConfig = System.IO.Path.Combine(PathToProjectFiles, @"Files\CopyCustomers.dtsConfig");

        public static readonly string PathToPackages = System.IO.Path.Combine(PathToProjectFiles, @"Demo\Packages\");
        public static readonly string PathToIspac = System.IO.Path.Combine(PathToProjectFiles, @"Demo\Packages\Demo.ispac");
        public static readonly string PathToMainPackage = System.IO.Path.Combine(PathToPackages, "Main.dtsx");
        public static readonly string PathToCopyCustomersPackage = System.IO.Path.Combine(PathToPackages, "CopyCustomers.dtsx");
        public static readonly string PathToLoadCustomersPackage = System.IO.Path.Combine(PathToPackages, "LoadCustomers.dtsx");        
    }
}
