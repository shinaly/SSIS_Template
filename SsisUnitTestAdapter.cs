using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Metadata;
using SSIS.Test.Template.Demo;
using SSIS.Test.Template.Demo.UnitTests;

namespace SSIS.Test.Template
{
    [TestClass]
    public class SsisUnitTestAdapter    
    {
        public void Ut<T>(bool loadFromIspacPackage = false)
            where T : BaseUnitTest
        {
            Type utClass = typeof(T);
            var customAttributes = utClass.GetCustomAttributes(false);
            UnitTestAttribute attribute = customAttributes.OfType<UnitTestAttribute>().Single();

            string repository = attribute.Repository;
            string packageName = Path.GetFileName(attribute.PackageName);
            if (string.IsNullOrEmpty(packageName))
                throw new InvalidOperationException("Unit test class must have its PackageName set.");

            var engine = EngineFactory.GetClassInstance<IUnitTestEngine>();

            if (loadFromIspacPackage)
                engine.LoadPackage(repository, Constants.PathToIspac, packageName);
            else
            {
                string pathToTargetPackage = Path.Combine(Constants.PathToPackages, packageName);
                engine.LoadPackage(repository, pathToTargetPackage);
            }

            engine.LoadUnitTest(utClass);
            UnitTestResult result = engine.ExecuteUnitTest(utClass);
        }

        [TestMethod]
        public void CopyCustomersFileAll()
        {
            Ut<CopyCustomersFileAll>();
        }

        [TestMethod]
        public void CopyCustomersFileAll2()
        {
            Ut<CopyCustomersFileAll2>();
        }

        [TestMethod]
        public void CopyCustomersFileAll3()
        {
            Ut<CopyCustomersFileAll3>();
        }

        [TestMethod]
        public void CopyCustomersFileAll3FakeDestination()
        {
            Ut<CopyCustomersFileAll3FakeDestination>();
        }

        [TestMethod]
        public void CopyCustomersFileAll4()
        {
            Ut<CopyCustomersFileAll4>();
        }

        [TestMethod]
        public void CopyCustomersFileAll4FakeDestination()
        {
            Ut<CopyCustomersFileAll4FakeDestination>();
        }

        [TestMethod]
        public void CopyCustomersFileAllDataTaps()
        {
            Ut<CopyCustomersFileAllDataTaps>();
        }

        [TestMethod]
        public void CopyCustomersFileAllFakeDestination()
        {
            Ut<CopyCustomersFileAllFakeDestination>();
        }

        [TestMethod]
        public void CopyCustomersFileAllFakeSource()
        {
            Ut<CopyCustomersFileAllFakeSource>();
        }

        [TestMethod]
        public void CopyCustomersFileAllFakeSourceAndFakeDestination()
        {
            Ut<CopyCustomersFileAllFakeSourceAndFakeDestination>();
        }

        [TestMethod]
        public void CopyCustomersFileCopy()
        {
            Ut<CopyCustomersFileCopy>();
        }

        [TestMethod]
        public void CopyCustomersFileEvent()
        {
            Ut<CopyCustomersFileEvent>();
        }

        [TestMethod]
        public void CopyCustomersFileTransform()
        {
            Ut<CopyCustomersFileTransform>();
        }

        [TestMethod]
        public void CopyCustomersWithConfigFileAll()
        {
            Ut<CopyCustomersWithConfigFileAll>();
        }

        [TestMethod]
        public void LoadCustomersCheckScript()
        {
            Ut<LoadCustomersCheckScript>();
        }

        [TestMethod]
        public void LoadCustomersConstraints()
        {
            Ut<LoadCustomersConstraints>();
        }

        [TestMethod]
        public void LoadCustomersCount()
        {
            Ut<LoadCustomersCount>();
        }

        [TestMethod]
        public void LoadCustomersLoad()
        {
            Ut<LoadCustomersLoad>();
        }

        [TestMethod]
        public void LoadCustomersPackage()
        {
            Ut<LoadCustomersPackage>();
        }

        [TestMethod]
        public void LoadCustomersPackageFakeDestination()
        {
            Ut<LoadCustomersPackageFakeDestination>();
        }

        [TestMethod]
        public void LoadCustomersTruncate()
        {
            Ut<LoadCustomersTruncate>();
        }

        [TestMethod]
        public void LoopWithDataFlow()
        {
            Ut<LoopWithDataFlow1>();
        }

        [TestMethod]
        public void LoopWithDataFlow2()
        {
            Ut<LoopWithDataFlow2>();
        }

        [TestMethod]
        public void CopyCustomersFileAllIspac()
        {
            Ut<CopyCustomersFileAllIspac>(true);
        }

        [TestMethod]
        public void CopyCustomersFileCopyIspac()
        {
            Ut<CopyCustomersFileCopyIspac>(true);
        }

        [TestMethod]
        public void CopyCustomersFileEventIspac()
        {
            Ut<CopyCustomersFileEventIspac>(true);
        }

        [TestMethod]
        public void CopyCustomersFileTransformIspac()
        {
            Ut<CopyCustomersFileTransformIspac>(true);
        }

        [TestMethod]
        public void LoadCustomersCheckScriptIspac()
        {
            Ut<LoadCustomersCheckScriptIspac>(true);
        }

        [TestMethod]
        public void LoadCustomersConstraintsIspac()
        {
            Ut<LoadCustomersConstraintsIspac>(true);
        }

        [TestMethod]
        public void LoadCustomersCountIspac()
        {
            Ut<LoadCustomersCountIspac>(true);
        }

        [TestMethod]
        public void LoadCustomersLoadIspac()
        {
            Ut<LoadCustomersLoadIspac>(true);
        }

        [TestMethod]
        public void LoadCustomersPackageIspac()
        {
            Ut<LoadCustomersPackageIspac>(true);
        }

        [TestMethod]
        public void LoadCustomersTruncateIspac()
        {
            Ut<LoadCustomersTruncateIspac>(true);
        }

        [TestMethod]
        public void MainWithParameter()
        {
            Ut<MainWithParameter>();
        }
    }
}
