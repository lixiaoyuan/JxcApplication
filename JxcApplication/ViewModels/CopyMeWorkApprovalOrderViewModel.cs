using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Helper;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    
    public class CopyMeWorkApprovalOrderViewModel:ViewModelTabItem
    {
        private WorkApprovalOrderManager _approvalOrderManager;
        private WorkApprovalOrder _selectedItem;
        public ObservableCollection<WorkApproval> ApprovalLookUp { get; set; }

        public ObservableCollection<WorkApprovalOrder> WorkApprovalOrders { get; set; }

        public bool ShowLoadingPanel { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DelegateCommand<WorkApprovalOrder> ReadCommand { get; set; }
        public virtual DelegateCommand<SelectedItemChangedEventArgs> SelectedItemChangedCommand { get; set; }
        public virtual DelegateCommand<EditValueChangedEventArgs> EditValueChangedCommand { get; set; }
        public virtual bool IsShowRead { get; set; }
        private Task LoadingDataAsync()
        {
            if (!StartDate.HasValue || !EndDate.HasValue || StartDate > EndDate)
            {
                WorkApprovalOrders = null;
                RaisePropertyChanged("WorkApprovalOrders");
                return null;
            }
            return Task.Factory.StartNew(() =>
            {
                DispatcherService.BeginInvoke(() =>
                {
                    ShowLoadingPanel = true;
                    RaisePropertyChanged("ShowLoadingPanel");
                });
                var orders = _approvalOrderManager.QueryCopyMeWorkApprovalOrders(App.GlobalApp.LoginUser.Id, StartDate.Value,EndDate.Value,IsShowRead);

                DispatcherService.BeginInvoke(() =>
                {
                    ShowLoadingPanel = false;
                    WorkApprovalOrders = orders;
                    RaisePropertiesChanged("ShowLoadingPanel", "WorkApprovalOrders");
                });
            });
        }

        private Task InitDataAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                DispatcherService.BeginInvoke(() =>
                {
                    ShowLoadingPanel = true;
                    RaisePropertyChanged("ShowLoadingPanel");
                });

                var time = DBUnit.GetDbTime();
                _approvalOrderManager = WorkApprovalOrderManager.Create();
                var worklookup = WorkApprovalManager.QueryLookUp();
                DispatcherService.BeginInvoke(() =>
                {
                    StartDate = time.AddDays(-(time.Day - 1));
                    EndDate = time.AddDays(-time.Day).AddMonths(1);
                    ApprovalLookUp = worklookup;
                    ShowLoadingPanel = false;
                    ReadCommand = new DelegateCommand<WorkApprovalOrder>(Read, CanRead);
                    SelectedItemChangedCommand = new DelegateCommand<SelectedItemChangedEventArgs>(SelectedItemChanged);
                    EditValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>((args => LoadingDataAsync()));
                    RaisePropertiesChanged("ShowLoadingPanel", "StartDate", "EndDate", "ApprovalLookUp");
                });
            });
        }

        public void Loaded()
        {
            InitDataAsync();
        }

        protected void OnStartDateChanged(DateTime? newDate)
        {
            LoadingDataAsync();
        }

        protected void OnEndDateChanged(DateTime? newDate)
        {
            LoadingDataAsync();
        }

        private void SelectedItemChanged(SelectedItemChangedEventArgs e)
        {
            _selectedItem = e.NewItem as WorkApprovalOrder;
            ReadCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        ///     已阅
        /// </summary>
        private async void Read(WorkApprovalOrder row)
        {
            Debug.Write("已阅");
            var result = await _approvalOrderManager.AddedRead(row, App.GlobalApp.LoginUser.Id);
            if (!string.IsNullOrWhiteSpace(result))
            {
                ShowNotification(result);
            }
            else
            {
                ShowNotification("保存成功!");
                await LoadingDataAsync();
            }
        }

        private bool CanRead(WorkApprovalOrder row)
        {
            return _selectedItem != null ;
        }

        public CopyMeWorkApprovalOrderViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
