using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using DevExpress.Mvvm;
using JxcApplication.ViewModels.Inherit;
using Utilities;

namespace JxcApplication.ViewModels
{
    public class ReportSalesSummaryMonthViewModel : ViewModelTabItem
    {
        private ReportManager reportManager;
        
        public object Datas { get; set; }
        public bool ShowLoadingPanel { get; set; }
        public virtual string Year { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public ReportSalesSummaryMonthViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
            QueryCommand = new DelegateCommand(Query);
            reportManager = ReportManager.Create();
        }

        private void Query()
        {
            LoadingData();
        }

        private void LoadingData()
        {
            if (string.IsNullOrWhiteSpace(Year))
            {
                ShowNotification("请输入年份");
                return;
            }
            int year = 2017;
            if (!int.TryParse(Year, out year))
            {
                ShowNotification("年份输入错误!");
                return;
            }
            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);
            ShowLoadingPanel = true;
            RaisePropertyChanged("ShowLoadingPanel");

            Task.Factory.StartNew((() =>
            {
                try
                {
                    var result = reportManager.ReportSalesSummaryMonth(startDate, endDate);

                    DispatcherService.BeginInvoke((() =>
                    {
                        ShowLoadingPanel = false;
                        Datas = result;
                        RaisePropertiesChanged("Datas", "ShowLoadingPanel");
                    }));
                }
                catch (Exception e)
                {
                   DispatcherService.BeginInvoke((() =>
                   {
                       ShowNotification(e.Message);
                       ShowLoadingPanel = false;
                   }));
                }
            }));
        }
    }
}
