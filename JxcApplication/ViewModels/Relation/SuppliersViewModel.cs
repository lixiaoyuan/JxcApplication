using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BusinessDb.Cor.Business;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels.Relation
{
    public class SuppliersViewModel : ViewModelTabItem
    {
        private SuppliersManager _suppliersManager;
        public ObservableCollection<Suppliers> Supplierses { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            _suppliersManager = SuppliersManager.Create();
            InitData();
        }

        private void InitData()
        {
            Supplierses = _suppliersManager.QueSupplierses();
        }

        public void InitNewRow(InitNewRowEventArgs args)
        {
            var tableview = args.Source as TableView;
            if (tableview != null)
            {
                var newDataObject = (Suppliers)tableview.Grid.GetRow(args.RowHandle);
                newDataObject.Id = Guid.NewGuid();
                newDataObject.Enable = true;
                newDataObject.Code = SortCodeGenerate.GetCode("GYS",_suppliersManager.GetMaxCode());
            }
        }

        public void Refresh()
        {
            Supplierses = _suppliersManager.Refresh();
            RaisePropertyChanged("Supplierses");
            //InitData();
        }

        public void Save(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var result = _suppliersManager.Save();
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
            _suppliersManager.Remove(opControl.SelectedItems.Cast<Suppliers>());
        }


        public SuppliersViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}