using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.EntityModels;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    public class WageViewModel : ViewModelTabItem
    {
        private WageInsertManager _wageInsertManager;
        private WageUpdateManager _wageUpdateManager;
        private WageManager _wageManager;
        private bool _isHistory;

        public bool IsHistory
        {
            get { return _isHistory; }
            set
            {
                _isHistory = value;
                SaveWageAsCommand.RaiseCanExecuteChanged();
                SaveWageCommand.RaiseCanExecuteChanged();
                PrintPreviewCommand.RaiseCanExecuteChanged();
            }
        }

        public Wage Wage { get; set; }
        public ObservableCollection<WageDetail> WageDetail { get; set; }

        public ObservableCollection<Account> AccountsLookUp { get; set; }
        public ObservableCollection<SystemUser> SystemUsersLookUp { get; set; }


        public DelegateCommand<GridControl> SaveWageCommand { get; set; }
        public DelegateCommand<GridControl> SaveWageAsCommand { get; set; }
        public DelegateCommand PrintPreviewCommand { get; set; }
        public WageViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
            AccountsLookUp = AccountManager.QueryLookUp();
            SystemUsersLookUp = SystemAccountManager.QueryLookUp();

            _wageManager = new WageManager();
            _wageInsertManager = WageInsertManager.Create();
            _wageUpdateManager = WageUpdateManager.Create();

            SaveWageCommand = new DelegateCommand<GridControl>(SaveWage, (control => !IsHistory));
            SaveWageAsCommand = new DelegateCommand<GridControl>(SaveWageAs, (control => IsHistory));
            PrintPreviewCommand = new DelegateCommand(PrintPreview, () => IsHistory);
        }

        public void WageDateChanged(EditValueChangedEventArgs e)
        {
            if (e.NewValue!=null)
            {
                DateTime dt ;
                if (DateTime.TryParse(e.NewValue.ToString(),out dt))
                {
                    if (!_wageManager.HasExisWage(dt))
                    {
                        var insertWageOrder = _wageInsertManager.GetInsertWageOrder();
                        Wage = insertWageOrder.MasterStorage;
                        Wage.WageDate = dt;
                        WageDetail = insertWageOrder.Details;
                        IsHistory = false;
                        RaisePropertiesChanged("Wage", "WageDetail");
                    }
                    else
                    {
                        var updateWageOrder = _wageUpdateManager.GetUpdateWageOrder(dt);
                        Wage = updateWageOrder.MasterStorage;
                        WageDetail = updateWageOrder.Details;
                        IsHistory = true;
                        RaisePropertiesChanged("Wage", "WageDetail");
                    }
                }
                else
                {
                    IsHistory = true;
                    Clear();
                }
            }
            else
            {
                IsHistory = true;
                Clear();
            }
        }

        public void InitNewRow(InitNewRowEventArgs e)
        {
            TableView tableView = e.OriginalSource as TableView;
            if (tableView != null)
            {
                var newRow = tableView.Grid.GetRow(e.RowHandle) as WageDetail;
                if (newRow != null)
                {
                    if (Wage != null && Wage.Id == Guid.Empty)
                    {
                        Wage.Id = Guid.NewGuid();
                    }
                    if (Wage != null)
                        newRow.WageId = Wage.Id;
                    newRow.Id = Guid.NewGuid();
                }
            }
        }

        private bool SaveBefor()
        {
            var sum = WageDetail.Where(a => a.AfterTaxSum.HasValue).Sum(a => a.AfterTaxSum);
            if (sum != null)
                Wage.SumPrice = sum.Value;
            if (Wage.SumPrice == 0)
            {
                ShowNotification("工资总金额为0", "保存失败:");
                return false;
            }
            if (Wage.PaymentAccountId==Guid.Empty)
            {
                ShowNotification("请选择账户!");
                return false;
            }
            Wage.CreateUserId = App.GlobalApp.LoginUser.Id;
            return true;
        }

        public void DeleteSelectRow(GridControl opControl)
        {
            if (opControl == null)
            {
                return;
            }

            var view = opControl.View as TableView;
            if (view == null)
            {
                return;
            }
            var handles = opControl.GetSelectedRowHandles();
            foreach (var row in handles)
            {
                view.DeleteRow(row);
            }
        }

        public void SaveWage(GridControl opControl)
        {
            if (Wage == null || WageDetail == null)
            {
                return;
            }
            if (_wageManager.HasExisWage(Wage.WageDate))
            {
                IsHistory = true;
                ShowNotification("本月工资已保存，不能修改保存!","保存失败:");
            }
            else
            {
                if (WageDetail.Count == 0)
                {
                    ShowNotification("记录为空不需要保存!", "保存失败:");
                    return;
                }
                if (SaveBefor() && !IsHistory)
                {
                    var result = _wageInsertManager.InsertWageInOrder(Wage, WageDetail);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        ShowNotification("保存成功!");
                        IsHistory = true;
                    }
                    else
                    {
                        ShowNotification(result, "保存失败:");
                    }
                }
            }
        }
        public void SaveWageAs(GridControl opControl)
        {
            if (WageDetail.Count == 0)
            {
                ShowNotification("记录为空不需要保存!", "保存失败:");
                return;
            }
            if (SaveBefor() && IsHistory)
            {
                var editObject = ViewModelSource.Create(() => new EditData());
                editObject.EditDateTime = DateTime.Now;
                var diag = CreateDialog("DataTemplateSaveSaDate", editObject, "选择另存工资月份").ShowDialog();
                if (!diag.HasValue || !diag.Value)
                {
                    IsHistory = true;
                    return;
                }
                if (_wageManager.HasExisWage(editObject.EditDateTime))
                {
                    IsHistory = true;
                    ShowNotification("本月工资已保存，不能修改保存!", "保存失败:");
                }
                else
                {
                    Wage.Id = Guid.NewGuid();
                    Wage.WageDate = editObject.EditDateTime;
                    foreach (WageDetail detail in WageDetail)
                    {
                        detail.Id = Guid.NewGuid();
                        detail.WageId = Wage.Id;
                    }
                    var result = _wageUpdateManager.InsertWageAsInOrder(Wage, WageDetail);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        IsHistory = true;
                        ShowNotification("保存成功!");
                    }
                    else
                    {
                        ShowNotification(result, "保存失败:");
                    }
                }
                RaisePropertiesChanged("Wage", "WageDetail");
            }
        }

        public void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName.Contains("X") || e.Column.FieldName.Contains("C") || e.Column.FieldName == "PreTaxSum" || e.Column.FieldName == "AfterTaxSum")
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
        }

        public  void PrintPreview()
        {
            if (!IsHistory)
            {
                ShowNotification("单据没有保存，不能打印!");
                return;
            }
            Report.Report.ShowPreviewDialog("WG", Wage.Id);
        }

        private void Clear()
        {
            WageDetail = null;
        }
        public void CellValueChanged(CellValueChangedEventArgs e)
        {
            WageDetail row = e.Row as WageDetail;
            if (row == null)
            {
                return;
            }
            if (e.Column.FieldName == "UserId")
            {
                row.Name = null;
            }
            else if (e.Column.FieldName == "Name")
            {
                row.UserId = null;
            }
            else if (e.Column.FieldName == "WorkDay")
            {
                return;
            }
            else
            {
                //计算
                CalculateRow(row);
            }
        }
        private void CalculateRow(WageDetail row)
        {
            if (row==null)
            {
                return;
            }
            row.PreTaxSum = 0;
            if (row.C1.HasValue)
            {
                row.PreTaxSum += row.C1;
            }
            if (row.C2.HasValue)
            {
                row.PreTaxSum += row.C2;
            }
            if (row.C3.HasValue)
            {
                row.PreTaxSum += row.C3;
            }
            if (row.C4.HasValue)
            {
                row.PreTaxSum += row.C4;
            }
            if (row.C5.HasValue)
            {
                row.PreTaxSum += row.C5;
            }
            if (row.C6.HasValue)
            {
                row.PreTaxSum += row.C6;
            }
            if (row.C7.HasValue)
            {
                row.PreTaxSum += row.C7;
            }
            if (row.C8.HasValue)
            {
                row.PreTaxSum += row.C8;
            }
            if (row.C9.HasValue)
            {
                row.PreTaxSum += row.C9;
            }
            if (row.C10.HasValue)
            {
                row.PreTaxSum += row.C10;
            }
            if (row.C11.HasValue)
            {
                row.PreTaxSum += row.C11;
            }
            if (row.C12.HasValue)
            {
                row.PreTaxSum += row.C12;
            }
            if (row.C13.HasValue)
            {
                row.PreTaxSum += row.C13;
            }
            if (row.C14.HasValue)
            {
                row.PreTaxSum += row.C14;
            }
            if (row.C15.HasValue)
            {
                row.PreTaxSum += row.C15;
            }
            if (row.C16.HasValue)
            {
                row.PreTaxSum += row.C16;
            }
            if (row.X1.HasValue)
            {
                row.PreTaxSum -= row.X1;
            }
            if (row.X2.HasValue)
            {
                row.PreTaxSum -= row.X2;
            }
            if (row.X3.HasValue)
            {
                row.PreTaxSum -= row.X3;
            }
            //放弃计算税
            //if (row.PreTaxSum>3500)
            //{
            //    row.C17 = (row.PreTaxSum - 3500) * 0.03m;
            //}
            //else
            //{
            //    row.C17 = null;
            //}
            if (row.C17.HasValue && row.PreTaxSum.HasValue)
            {
                row.AfterTaxSum = row.PreTaxSum - row.C17;
            }
            else
            {
                row.AfterTaxSum = row.PreTaxSum;
            }
        }
    }

    public class EditData : ViewModelBase
    {
        public DateTime EditDateTime { get; set; }
    }
}