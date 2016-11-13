using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models.Report;
using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    public class ReportAccountInfoViewModel : ViewModelTabItem
    {
        private ReportManager reportManager;
        public ReportAccountInfoViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
            CheckBoxChangedCommand=new DelegateCommand<EditValueChangedEventArgs>(CheckBoxChanged);
            ShowDateChangedCommand=new DelegateCommand<EditValueChangedEventArgs>(ShowDateChanged);
        }

        public bool ShowLoadingPanel { get; set; }
        public ObservableCollection<ReportAccountInfoModel> Datas { get; set; }
        public virtual DateTime SnapshotDate { get; set; }
        public virtual bool ShowCheck { get; set; }

        public DelegateCommand<EditValueChangedEventArgs> CheckBoxChangedCommand { get; set; }
        public DelegateCommand<EditValueChangedEventArgs> ShowDateChangedCommand { get; set; }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            reportManager = ReportManager.Create();
        }

        private void CheckBoxChanged(EditValueChangedEventArgs e)
        {
            bool b;
            if (bool.TryParse(e.NewValue.ToString(), out b))
            {
                LoadingData(b ? 1 : 0);
            }
            else
            {
                LoadingData();
            }
        }

        private void ShowDateChanged(EditValueChangedEventArgs e)
        {
            DateTime dt;
            if (DateTime.TryParse(e.NewValue.ToString(), out dt))
            {
                if (dt == DateTime.MinValue)
                {
                    dt = DateTime.Now;
                    SnapshotDate = dt.AddHours(23 - dt.Hour).AddMinutes(59 - dt.Minute).AddSeconds(59 - dt.Second);
                    RaisePropertyChanged("SnapshotDate");
                    return;
                }
                SnapshotDate = dt.AddHours(23 - dt.Hour).AddMinutes(59 - dt.Minute).AddSeconds(59 - dt.Second);
                LoadingData(ShowCheck ? 1 : 0);
            }
        }
        public void Loaded()
        {
            LoadingData();
        }

        private void LoadingData(int flag = 0)
        {
            ShowLoadingPanel = true;
            RaisePropertyChanged("ShowLoadingPanel");
            Task.Factory.StartNew(() =>
            {
                ObservableCollection<ReportAccountInfoModel> result = null;
                if (flag == 0)
                {
                    result = reportManager.QueReportAccountInfo();
                }
                else if (flag == 1)
                {
                    result = reportManager.QueReportSnapshotAccountInfo(SnapshotDate);
                }
                DispatcherService.BeginInvoke(() =>
                {
                    ShowLoadingPanel = false;
                    Datas = result;
                    RaisePropertiesChanged("Datas", "ShowLoadingPanel");
                });
            });
        }
    }
}