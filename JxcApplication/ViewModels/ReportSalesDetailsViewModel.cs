﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models.Report;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using JxcApplication.ViewModels.Inherit;
using Utilities;

namespace JxcApplication.ViewModels
{
    [POCOViewModel]
    public class ReportSalesDetailsViewModel : ViewModelTabItem
    {
        private ReportManager reportManager;
	    private RoleManager _roleManager;

        private int _showType = 0;
        public bool ShowLoadingPanel { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public ObservableCollection<object> Datas { get; set; }

        public DelegateCommand<int> ShowTypeIndexChangedCommand { get; set; }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
			reportManager = ReportManager.Create();
			_roleManager = RoleManager.Create();
			ShowTypeIndexChangedCommand = new DelegateCommand<int>(ShowTypeIndexChanged);
        }

        protected void OnStartDateChanged(DateTime? newDate)
        {
            LoadingData(_showType);
        }

        protected  void OnEndDateChanged(DateTime? newDate)
        {
            LoadingData(_showType);
        }

        private void ShowTypeIndexChanged(int index)
        {
            _showType = index;
            LoadingData(_showType);
        }
        private void LoadingData(int type = 0)
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
                try
                {
                    ObservableCollection<object> result = null;
                    Guid busId = Guid.Empty;
                    if (RoleManager.IsSalesmanGroup(App.GlobalApp.LoginUser))
                    {
                        busId = App.GlobalApp.LoginUser.Id;
                    }

                    if (type == 0)
                    {
                        result = reportManager.QueReportSalesDetail(StartDate.Value, EndDate.Value, busId)
                            .Select(a => (object)a).ToObservableCollection();
                    }
                    else if (type == 1)
                    {
                        result = reportManager.QueReportSalesDetailCustomer(StartDate.Value, EndDate.Value,busId)
                            .Select(a => (object)a).ToObservableCollection();
                    }
                    else if (type == 2)
                    {
                        result = reportManager.QueReportSalesDetailUser(StartDate.Value, EndDate.Value,busId)
                            .Select(a => (object)a).ToObservableCollection();
                    }
                    DispatcherService.BeginInvoke((() =>
                    {
                        ShowLoadingPanel = false;
                        Datas = result;
                        RaisePropertiesChanged("Datas", "ShowLoadingPanel");
                    }));
                }
                catch (Exception e)
                {
                    ShowLoadingPanel = false;
                    RaisePropertyChanged("ShowLoadingPanel");
                }
            }));
        }

        public ReportSalesDetailsViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}