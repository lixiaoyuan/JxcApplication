﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models.Report;
using DevExpress.Mvvm;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    public class ReportPaymentedViewModel : ViewModelTabItem
    {
        private ReportManager reportManager;
        public bool ShowLoadingPanel { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public ObservableCollection<PaymentModel> Datas { get; set; }
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
                var result = reportManager.Paymented(StartDate.Value, EndDate.Value);
                DispatcherService.BeginInvoke((() =>
                {
                    ShowLoadingPanel = false;
                    Datas = result;
                    RaisePropertiesChanged("Datas", "ShowLoadingPanel");
                }));
            }));
        }
        public ReportPaymentedViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
