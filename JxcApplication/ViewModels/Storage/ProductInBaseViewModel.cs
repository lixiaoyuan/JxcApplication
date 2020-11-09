using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.EntityModels;
using BusinessDb.Cor.Models;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels.Storage
{
    public abstract class ProductInBaseViewModel : ViewModelTabItem
    {
        private bool _isHistoryItemChanged;
        private bool _isNewOrder;
        protected ProductManager ProductManager;
        protected ProductOrderInsertManager ProductOrderInsertManager;
        protected ProductOrderManager ProductOrderManager;
        protected ProductOrderUpdateManager ProductOrderUpdateManager;
        protected StoreManager StoreManager;

        /// <summary>
        ///     ViewModel初始化
        /// </summary>
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            if (StoreManager == null)
            {
                StoreManager = StoreManager.Create();
                StoresLookUp = StoreManager.QueStores();
            }
            if (ProductManager == null)
            {
                ProductManager = ProductManager.Create();
            }
            SystemUserLookUp = SystemAccountManager.QueryLookUp();
            ProductOrderManager = ProductOrderManager.Create();

            InitNewOrder();
        }


        /// <summary>
        ///     初始化新录入产品
        /// </summary>
        protected virtual void InitNewOrder()
        {
            IsNewOrder = true;
            ProductOrderInsertManager = ProductOrderInsertManager.Create();
            var order = ProductOrderInsertManager.GetInsertProductInOrder();
            Details = order.Details;
            ProductInStorage = order.MasterStorage;
            Details.CollectionChanged += (a, b) =>
            {
                StoreLookUpEnable = !Details.Any();
                RaisePropertyChanged("StoreLookUpEnable");
            };
            StoreLookUpEnable = true;
            RaisePropertiesChanged("Details", "ProductInStorage", "StoreLookUpEnable");
        }


        /// <summary>
        ///     获取产品最新信息(Product表)
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        protected Product GetProductNewInfo(Guid productGuid)
        {
            if (ProductInStorage.StorageId == null)
            {
                ShowNotification("请选择仓库");
                return null;
            }
            return ProductManager.GetProductNewInfo(productGuid, ProductInStorage.StorageId.Value);
        }

        /// <summary>
        ///     保存或更新产品订单信息到数据库
        /// </summary>
        /// <returns></returns>
        private bool SaveProductCor()
        {
            if (!IsNewOrder)
            {
                //订单保存后不能修改。
                ShowNotification("订单保存后不能修改!", "警告:");
                return false;
                //var resultStr = ProductOrderManager.CanUpdateProductInOrder(ProductInStorage.Id);
                //if (!string.IsNullOrWhiteSpace(resultStr))
                //{
                //    ShowNotification(resultStr, "警告:");
                //    return false;
                //}
            }
            if (Details.Count == 0)
            {
                ShowNotification("订单明细不能为空!");
                return false;
            }

            foreach (var productInStorageDetail in Details)
            {
                if (productInStorageDetail.SumPrice < 0)
                {
                    ShowNotification("明细金额不能小于0");
                    return false;
                }
            }

            if (!SaveProductOrderBefor()) return false;

            string result = null;
            if (IsNewOrder)
            {
                result = ProductOrderInsertManager.InsertProductInOrder();
            }
            else
            {
                result = ProductOrderUpdateManager.UpdateProductInOrder();
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                ShowNotification("保存成功!");
                return true;
            }
            ShowNotification(result, "失败:");
            return false;
        }

        /// <summary>
        ///     加载历史订单
        /// </summary>
        /// <param name="arg"></param>
        public virtual void LoadHistory(ShowOrderEventArgs arg)
        {
            IsNewOrder = false;
            StoreLookUpEnable = false;
            ProductOrderUpdateManager = ProductOrderUpdateManager.Create();
            var updateOrder = ProductOrderUpdateManager.GetUpdateProductInOrder(arg.OrderId);
            if (updateOrder == null)
            {
                ShowNotification("没有找到历史订单!");
                InitNewOrder();
                return;
            }
            Details = updateOrder.Details;
            ProductInStorage = updateOrder.MasterStorage;
            RaisePropertiesChanged("ProductInStorage", "Details", "StoreLookUpEnable");
        }

        /// <summary>
        ///     产品类型
        /// </summary>
        /// <returns></returns>
        protected abstract string ProductType();

        /// <summary>
        ///     订单类型
        /// </summary>
        /// <returns></returns>
        protected abstract string OrderType();

        /// <summary>
        ///     保存产品订单之前,返回false取消保存
        /// </summary>
        protected abstract bool SaveProductOrderBefor();

        #region 绑定属性

        /// <summary>
        ///     是否为新增订单
        /// </summary>
        public bool IsNewOrder
        {
            get { return _isNewOrder; }
            set
            {
                _isNewOrder = value;
                RaisePropertyChanged("IsHistory");
            }
        }

        public bool IsHistory
        {
            get { return !IsNewOrder; }
        }

        public bool StoreLookUpEnable { get; set; }

        /// <summary>
        ///     仓库下拉列表
        /// </summary>
        public ObservableCollection<Store> StoresLookUp { get; set; }

        /// <summary>
        ///     产品下拉列表
        /// </summary>
        public ObservableCollection<Product> ProductLookUp { get; set; }

        /// <summary>
        ///     用户下拉列表
        /// </summary>
        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }

        /// <summary>
        ///     产品主记录
        /// </summary>
        public ProductInStorage ProductInStorage { get; set; }

        /// <summary>
        ///     记录明细
        /// </summary>
        public ObservableCollection<ProductInStorageDetail> Details { get; set; }

        /// <summary>
        ///     历史纪录明细
        /// </summary>
        public ObservableCollection<OrderBrowser> HistoryOrderList { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        public virtual string Title
        {
            get { return "产品入库单"; }
        }

        #endregion

        #region POCO Command

        /// <summary>
        ///     初始化新订单
        /// </summary>
        public void NewProductOrder()
        {
            InitNewOrder();
        }

        /// <summary>
        ///     打印
        /// </summary>
        public virtual void Print()
        {
            if (IsNewOrder)
            {
                ShowNotification("订单未生成，不能打印!");
                return;
            }
            Report.Report.Print(OrderType(), ProductInStorage.Id);
        }

        /// <summary>
        ///     打印预览
        /// </summary>
        public virtual void PrintPreview()
        {
            if (IsNewOrder)
            {
                ShowNotification("订单未生成，不能打印!");
                return;
            }
            Report.Report.ShowPreviewDialog(OrderType(), ProductInStorage.Id);
        }

        /// <summary>
        ///     保存产品
        /// </summary>
        public virtual void SaveProduct(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var result = SaveProductCor();
            if (result)
            {
                InitNewOrder();
            }
        }

        /// <summary>
        ///     删除选中行
        /// </summary>
        /// <param name="opGridControl"></param>
        public virtual void DeleteSelectRow(GridControl opGridControl)
        {
            if (opGridControl == null)
            {
                return;
            }

            var view = opGridControl.View as TableView;
            if (view == null)
            {
                return;
            }
            var handles = opGridControl.GetSelectedRowHandles();
            foreach (var row in handles)
            {
                view.DeleteRow(row);
            }
        }


        /// <summary>
        ///     单元格值发生改变后
        /// </summary>
        /// <param name="e"></param>
        public virtual void CellValueChanged(CellValueChangedEventArgs e)
        {
            var editRow = e.Cell.Row as ProductInStorageDetail;
            if (editRow == null)
            {
                return;
            }


            if (e.Cell.Property == "ProductId")
            {
                #region 当产品Id选择发生改变时，更新输入行的产品信息

                var newGuid = Guid.Parse(e.Cell.Value.ToString());
                if (newGuid == Guid.Empty)
                {
                    return;
                }

                var newInfo = GetProductNewInfo(newGuid);
                if (newInfo == null)
                {
                    return;
                }
                editRow.ProductCode = newInfo.Code;
                if (newInfo.UnitPrice != null)
                {
                    editRow.UnitPrice = newInfo.UnitPrice.Value;
                }
                editRow.SumPrice = editRow.OriginalStock * editRow.UnitPrice;
                editRow.ProductSpecification = newInfo.Specification;
                editRow.ProductUnit = newInfo.Unit;

                editRow.ProductId = newInfo.Id;

                #endregion
            }
            else if (e.Cell.Property == "OriginalStock")
            {
                editRow.SumPrice = editRow.UnitPrice*editRow.OriginalStock;
            }
            else if (e.Cell.Property == "UnitPrice")
            {
                editRow.SumPrice = editRow.UnitPrice*editRow.OriginalStock;
            }
            else if (e.Cell.Property == "SumPrice")
            {
                if (editRow.SumPrice == 0)
                {
                    editRow.OriginalStock = 0;
                }
                else
                {
                    editRow.UnitPrice = editRow.SumPrice/editRow.OriginalStock;
                }
            }
        }

        /// <summary>
        ///     自定义列显示样式
        /// </summary>
        /// <param name="e"></param>
        public virtual void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "SumPrice" || e.Column.FieldName == "UnitPrice" ||
                e.Column.FieldName == "LastPrice" || e.Column.FieldName == "ThisPrice")
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
        }

        /// <summary>
        ///     仓库选择发生改变
        /// </summary>
        /// <param name="e"></param>
        public void StoreEditValueChanged(EditValueChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ProductLookUp = ProductManager.QueryByStore(Guid.Parse(e.NewValue.ToString()));
                RaisePropertyChanged("ProductLookUp");
            }
        }

        #endregion

        public ProductInBaseViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}