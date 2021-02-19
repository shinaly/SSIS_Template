using System.Collections.ObjectModel;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSIS.Test.Dts;
using SSIS.Test.Metadata;

namespace SSIS.Test.Template.Demo.LiveTests
{
    [ActionClass("DEMO2", "MainWithDataFlow.dtsx")]
    [DataTap(@"\[MainWithDataFlow]\[DFT Convert customer names]\[FFS Customers]", @"\[MainWithDataFlow]\[DFT Convert customer names]\[RCNT Count  customers]")]
    class MainWithDataFlow : BaseLiveTest
    {
        public MainWithDataFlow()
        {
            // reset the environment to allow this to be runnable
            if (File.Exists(Constants.CustomersFileDestination))
                File.Delete(Constants.CustomersFileDestination);

            if (File.Exists(Constants.CustomersFileConverted))
                File.Delete(Constants.CustomersFileConverted);
        }

        [ActionMethod(@"\[MainWithDataFlow]\[DFT Convert customer names]")]
        public void TestConvertCustomerNames(ActionContext context)
        {
            Assert.AreEqual(true, context.ActiveExecutable.IsExecutionSuccess);

            DtsVariable variable = context.ActiveExecutable.GetVariable(@"CustomerCount");
            var count = (int)variable.GetValue();
            Assert.AreEqual(3, count);

            CheckDataTaps(context);
        }

        private void CheckDataTaps(ActionContext context)
        {
            ReadOnlyCollection<DataTap> dataTaps = context.DataTaps;

            if (dataTaps == null)
                return;

            if (dataTaps.Count > 0)
            {
                DataTap dataTap = dataTaps[0];
                foreach (DataTapSnapshot snapshot in dataTap.Snapshots)
                {
                    string data = snapshot.LoadData();
                }
            }
        }
    }
}
