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
using DevExpress.Xpf.Grid;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{

   public class MeWorkApprovalOrderViewModel: ViewModelTabItem
    {
        private WorkApprovalOrderManager _approvalOrderManager;
        private WorkApprovalOrder _selectedItem;

        public MeWorkApprovalOrderViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
        public ObservableCollection<WorkApproval> ApprovalLookUp { get; set; }

        public ObservableCollection<WorkApprovalOrder> WorkApprovalOrders { get; set; }

        public bool ShowLoadingPanel { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DelegateCommand<WorkApprovalOrder> AgreeCommand { get; set; }
        public virtual DelegateCommand<WorkApprovalOrder> RefuseCommand { get; set; }
        public virtual DelegateCommand<SelectedItemChangedEventArgs> SelectedItemChangedCommand { get; set; }

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
                var orders = _approvalOrderManager.QueryMeApprovalOrders(App.GlobalApp.LoginUser.Id, StartDate.Value,EndDate.Value);

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
                    AgreeCommand = new DelegateCommand<WorkApprovalOrder>(Agree, CanAgree);
                    RefuseCommand = new DelegateCommand<WorkApprovalOrder>(Refuse, CanRefuse);
                    SelectedItemChangedCommand = new DelegateCommand<SelectedItemChangedEventArgs>(SelectedItemChanged);
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
            AgreeCommand.RaiseCanExecuteChanged();
            RefuseCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        ///     同意
        /// </summary>
        private async void Agree(WorkApprovalOrder row)
        {
            Debug.Write("同意");
            var result = await _approvalOrderManager.AddedResultsAsync(row, App.GlobalApp.LoginUser.Id, WorkApprovalOrderResult.Agree);
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

        private bool CanAgree(WorkApprovalOrder row)
        {
            //return _selectedItem != null;
            return false;
        }

        /// <summary>
        ///     拒绝
        /// </summary>
        private async void Refuse(WorkApprovalOrder row)
        {
            Debug.Write("拒绝");
            var result = await _approvalOrderManager.AddedResultsAsync(row, App.GlobalApp.LoginUser.Id, WorkApprovalOrderResult.Refuse);
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

        private bool CanRefuse(WorkApprovalOrder row)
        {
            //return _selectedItem != null;
            return false;
        }
    }
}
