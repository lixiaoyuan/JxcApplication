using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels.Relation
{
    public class CustomerViewModel : ViewModelTabItem
    {
        private CustomerManager _manager;
        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            _manager = CustomerManager.Create();
            InitData();
        }

        private void InitData()
        {
            SystemUserLookUp = SystemAccountManager.QueryLookUp();
            if (RoleManager.IsSalesmanGroup(App.GlobalApp.LoginUser))
            {
                Customers = _manager.QueCustomers(App.GlobalApp.LoginUser.Id);
            }else
            {
                Customers = _manager.QueCustomers();
            }
        }

        public void InitNewRow(InitNewRowEventArgs args)
        {
            var tableview = args.Source as TableView;
            if (tableview != null)
            {
                var newDataObject = (Customer)tableview.Grid.GetRow(args.RowHandle);
                newDataObject.Id = Guid.NewGuid();
                //获取最新编码
                string maxCode = _manager.GetMaxCode();
                newDataObject.Code = SortCodeGenerate.GetCode("KH", maxCode);
                newDataObject.Balance = 0;
                newDataObject.Enable = true;
            }
        }

        public void Refresh(GridControl opControl)
        {
            if (RoleManager.IsSalesmanGroup(App.GlobalApp.LoginUser))
            {
                Customers = _manager.QueCustomers(App.GlobalApp.LoginUser.Id);
            }
            else
            {
                Customers = _manager.QueCustomers();
            }
            RaisePropertyChanged("Customers");
            opControl.RefreshData();
        }

        public void Save(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var result = _manager.Save();
            ShowNotification(result ? "保存成功!" : "保存失败!");
        }

        public void Add(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var tableView = opControl.View as TableView;
            if (tableView == null)
            {
                ShowNotification("操作失败!该窗口不是TableView！");
                return;
            }
            tableView.AddNewRow();
            tableView.MoveLastRow();
        }

        public void Del(GridControl opControl)
        {
            opControl.View.CommitEditing();
            if (opControl.SelectedItems.Count == 0)
            {
                ShowNotification("没有选择删除数据!");
                return;
            }
            _manager.Remove(opControl.SelectedItems.Cast<Customer>());
        }

        /// <summary>
        ///     自定义列显示样式
        /// </summary>
        /// <param name="e"></param>
        public virtual void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Balance" )
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
        }


        public CustomerViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
