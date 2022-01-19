using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BusinessDb.Cor.Business;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    public class ProductViewModel : ViewModelTabItem
    {
        private ProductManager _productManager;
        private ProductTypeManager _productTypeManager;

        public ObservableCollection<Product> Products { get; set; }

        public ObservableCollection<ProductType> ProductTypeLookUp { get; set; }
        public ObservableCollection<ProductAsType> ProductAsTypeLookUp { get; set; }
        public string ProductTypeCode { get; set; }

        /// <summary>
        /// 上次最大的编码
        /// </summary>
        private string preMaxCode = null;

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();

            InitData();
        }

        public void InitData()
        {
            _productTypeManager = ProductTypeManager.Create();
            _productManager = ProductManager.Create();

            ProductAsTypeLookUp = ProductTypeManager.QueryProductAsTypesLookUp();
            ProductTypeLookUp = _productTypeManager.QueryProductType();
            RaisePropertiesChanged("ProductTypeLookUp", "ProductAsTypeLookUp");
            EditValueChanged();
        }

        public void EditValueChanged()
        {
            if (string.IsNullOrWhiteSpace(ProductTypeCode))
            {
                Products = null;
            }
            else
            {
                Products = _productManager.QueryByProductType(ProductTypeCode);
                preMaxCode = null;
            }
            RaisePropertyChanged("Products");
        }

        public void Add(GridControl control)
        {
            control.View.CommitEditing();
            var tableView = control.View as TableView;
            if (tableView == null)
            {
                ShowNotification("操作失败!该窗口不是TableView！");
                return;
            }
            tableView.AddNewRow();
        }

        public void OnInitNewRow(InitNewRowEventArgs e)
        {
            try
            {
                //添加新行
                TableView tableView = (TableView)e.OriginalSource;
                var newRow = (Product)tableView.Grid.GetRow(e.RowHandle);
                tableView.FocusedRowHandle = e.RowHandle;

                newRow.Id = Guid.NewGuid();
                newRow.Enable = true;

                string maxCode;
                if (preMaxCode == null)
                {
                    maxCode = _productManager.GetMaxCode(ProductTypeCode);
                }
                else
                {
                    maxCode = preMaxCode;
                }
                newRow.Code = SortCodeGenerate.GetCode(ProductTypeCode,maxCode);
                preMaxCode = newRow.Code;
                newRow.ProductType = ProductTypeCode;
            }
            catch (Exception exception)
            {
                ShowNotification(exception.Message);
            }
            
            //GridControl gc;
            //gc.GetRow(e.RowHandle)
        }

        public void Del(GridControl control)
        {
            control.View.CommitEditing();
            if (control.SelectedItems.Count == 0)
            {
                ShowNotification("没有选择删除数据!");
                return;
            }
            _productManager.Remove(control.SelectedItems.Cast<Product>());
        }

        public void Save(GridControl control)
        {
            control.View.CommitEditing();
            var result = _productManager.Save();
            ShowNotification(result ? "保存成功!" : "保存失败!");
        }

        public virtual void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if ( e.Column.FieldName == "UnitPrice")
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
        }

        public ProductViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
