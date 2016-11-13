using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{

    [POCOViewModel]
    public class ReportHasReceivableViewModel : ViewModelTabItem
    {
        private ReportManager reportManager;
        public bool ShowLoadingPanel { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public ObservableCollection<HasReceivableModel> Datas { get; set; }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            reportManager = ReportManager.Create();
        }

        protected void OnStartDateChanged(DateTime? newDate)
        {
            LoadingData();
        }

        protected void OnEndDateChanged(DateTime? newDate)
        {
            LoadingData();
        }

        private void LoadingData()
        {
            if (StartDate == null || EndDate == null || StartDate > EndDate)
            {
                Datas = null;
                RaisePropertyChanged("Datas");
                return;
            }
            ShowLoadingPanel = true;
            RaisePropertyChanged("ShowLoadingPanel");
            Task.Factory.StartNew((() =>
            {
                var result = reportManager.HasReceivable(StartDate.Value, EndDate.Value);
                DispatcherService.BeginInvoke((() =>
                {
                    ShowLoadingPanel = false;
                    Datas = result;
                    RaisePropertiesChanged("Datas", "ShowLoadingPanel");
                }));
            }));
        }
        public ReportHasReceivableViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
