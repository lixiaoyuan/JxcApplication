using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models.Report;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    [POCOViewModel]
    public class ReportNoPaymentViewModel: ViewModelTabItem
    {
        private ReportManager reportManager;
        private int _showType = 0;
        private int _payType = 0;
        public bool ShowLoadingPanel { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public object Datas { get; set; }
        public DelegateCommand<int> ShowTypeIndexChangedCommand { get; set; }
        public DelegateCommand<int> PayTypeIndexChangedCommand { get; set; }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            reportManager = ReportManager.Create();
            ShowTypeIndexChangedCommand = new DelegateCommand<int>(ShowTypeIndexChanged);
            PayTypeIndexChangedCommand = new DelegateCommand<int>(PayTypeIndexChanged);
        }

        protected void OnStartDateChanged(DateTime? newDate)
        {
            LoadingData(_showType,_payType);
        }

        protected void OnEndDateChanged(DateTime? newDate)
        {
            LoadingData(_showType, _payType);
        }
        private void ShowTypeIndexChanged(int index)
        {
            _showType = index;
            LoadingData(_showType, _payType);
        }
        private void PayTypeIndexChanged(int index)
        {
            _payType = index;
            LoadingData(_showType, _payType);
        }
        private void LoadingData(int showType,int payType)
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
                object result = null;
                if (payType == 0)//未付款
                {
                    if (showType == 0)//明细
                    {
                        result = reportManager.PaymentedV2(StartDate.Value, EndDate.Value, "NotPaymentModel");
                    }
                    else if (showType == 1)//客户
                    {
                        result = reportManager.PaymentedV2(StartDate.Value, EndDate.Value, "PaymentNotModelCustomer");
                    }
                    else if (showType == 2)//业务员
                    {
                        result = reportManager.PaymentedV2(StartDate.Value, EndDate.Value, "PaymentNotModelUser");
                    }
                }
                else if (payType == 1)//已付款
                {
                    if (showType == 0)//明细
                    {
                        result = reportManager.PaymentedV2(StartDate.Value, EndDate.Value, "PaymentModel");
                    }
                    else if (showType == 1)//客户
                    {
                        result = reportManager.PaymentedV2(StartDate.Value, EndDate.Value, "PaymentModelCustomer");
                    }
                    else if (showType == 2)//业务员
                    {
                        result = reportManager.PaymentedV2(StartDate.Value, EndDate.Value, "PaymentModelUser");
                    }
                }
                DispatcherService.BeginInvoke((() =>
                {
                    ShowLoadingPanel = false;
                    Datas = result;
                    RaisePropertiesChanged("Datas", "ShowLoadingPanel");
                }));
            }));
        }

        public ReportNoPaymentViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}