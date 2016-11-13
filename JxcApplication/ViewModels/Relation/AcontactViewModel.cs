using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using BusinessDb.Cor.Business;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using JxcApplication.Core;

namespace JxcApplication.ViewModels.Relation
{
    public class AcontactViewModel:ViewModelBaseCor
    {
        private AcontactManager _acontactManager;
        public ObservableCollection<Acontact> Acontacts { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            _acontactManager = AcontactManager.Create();
            InitData();
        }

        private void InitData()
        {
            Acontacts = _acontactManager.QueAcontacts();
        }

        public void InitNewRow(InitNewRowEventArgs args)
        {
            var tableview = args.Source as TableView;
            if (tableview != null)
            {
                var newDataObject = (Acontact)tableview.Grid.GetRow(args.RowHandle);
                newDataObject.Id = Guid.NewGuid();
                newDataObject.Enable = true;
                //获取最新编码
                var maxCode = _acontactManager.GetMaxCode();
                newDataObject.Code = SortCodeGenerate.GetCode("LXR", maxCode);
            }
        }

        public void Refresh(GridControl opControl)
        {
            Acontacts = _acontactManager.Refresh();
            RaisePropertyChanged("Acontacts");
            opControl.RefreshData();
        }

        public void Save(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var result = _acontactManager.Save();
            ShowNotification(result ? "保存成功!" : "保存失败!");
        }

        public void Add(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var tableView = opControl.View as TableView;
            if (tableView != null)
            {
                tableView.AddNewRow();
                tableView.MoveLastRow();
            }
        }

        public void Del(GridControl opControl)
        {
            opControl.View.CommitEditing();
            if (opControl.SelectedItems.Count == 0)
            {
                ShowNotification("没有选择删除数据!");
                return;
            }
            _acontactManager.Remove(opControl.SelectedItems.Cast<Acontact>());
        }

    }
}
