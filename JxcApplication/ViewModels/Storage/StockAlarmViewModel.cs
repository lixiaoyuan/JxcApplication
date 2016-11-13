using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models.Report;
using DevExpress.Mvvm;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels.Storage
{
    public class StockAlarmViewModel : ViewModelTabItem
    {
        private readonly ReportManager reportManager;

        public StockAlarmViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
            reportManager = ReportManager.Create();
            LoadedCommand = new DelegateCommand(Loaded);
            QueryCommand = new DelegateCommand(Query);
        }

        public ObservableCollection<StockAlarmModel> Datas { get; set; }
        public DelegateCommand LoadedCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }

        private void Loaded()
        {
            Query();
        }

        private void Query()
        {
            Task.Factory.StartNew(() =>
            {
                var datas = reportManager.QueStockAlarm();
                DispatcherService.BeginInvoke(() =>
                {
                    Datas = datas;
                    RaisePropertyChanged("Datas");
                });
            });
        }
    }
}