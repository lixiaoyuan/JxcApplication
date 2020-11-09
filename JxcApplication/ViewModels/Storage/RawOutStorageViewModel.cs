using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using DevExpress.Xpf.Editors;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;
using JxcApplication.Control;
using JxcApplication.Views.Storage;

namespace JxcApplication.ViewModels.Storage
{
    public class RawOutStorageViewModel : ViewModelTabItem
    {
        private bool _isNewOrder;
        private ProductOrderManager _productOrderManager;

        private RawOutStorageSelectOutWindow _rawOutOrderInsertManager;
        private RawOutOrderUpdateManager _rawOutOrderUpdateManager;

        protected ProductManager ProductManager;
        protected StoreManager StoreManager;
        public bool StoreLookUpEnable { get; set; }

        /// <summary>
        /// 是否隐藏金额
        /// </summary>
        public bool IsVisibleMoney { get; set; } = true;

        public ObservableCollection<Product> ProductLookUp { get; set; }
        public ObservableCollection<Store> StoresLookUp { get; set; }
        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }

        public ProductOutStorage OutStorage { get; set; }
        public ObservableCollection<ProductOutStorageDetail> OutStorageDetails { get; set; }

        public ObservableCollection<OrderBrowser> HistoryOrderList { get; set; }
        private ObservableCollection<ProductOutStorageDetail> _deleteDetails = new LockableCollection<ProductOutStorageDetail>();

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

        public string Title
        {
            get { return "消耗品领用单"; }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            ProductManager = ProductManager.Create();
            StoreManager = StoreManager.Create();
            SystemAccountManager.Create();
            _productOrderManager = ProductOrderManager.Create();

            StoresLookUp = StoreManager.QueStores();
            SystemUserLookUp = SystemAccountManager.QueryLookUp();
            //ProductLookUp = ProductManager.QueryByProductType("YL");

            InitNewOrder();
        }

        private void InitNewOrder()
        {
            IsNewOrder = true;
            _rawOutOrderInsertManager = RawOutStorageSelectOutWindow.Create();

            var newOrder = _rawOutOrderInsertManager.GetInsertRawOutOrder();
            OutStorage = newOrder.MasterStorage;
            OutStorageDetails = newOrder.Details;
            OutStorageDetails.CollectionChanged += (a, b) =>
            {
                StoreLookUpEnable = !OutStorageDetails.Any();
                RaisePropertyChanged("StoreLookUpEnable");
            };
            StoreLookUpEnable = true;
            RaisePropertiesChanged("OutStorage", "OutStorageDetails", "StoreLookUpEnable");
        }

        /// <summary>
        ///     加载历史订单
        /// </summary>
        /// <param name="arg"></param>
        public virtual void LoadHistory(ShowOrderEventArgs arg)
        {
            IsNewOrder = false;
            StoreLookUpEnable = false;
            _deleteDetails.Clear();
            _rawOutOrderUpdateManager = RawOutOrderUpdateManager.Create();
            var updateOrder = _rawOutOrderUpdateManager.GetUpdateProductOutOrder(arg.OrderId);
            if (updateOrder == null)
            {
                ShowNotification("没有找到历史订单!");
                InitNewOrder();
                return;
            }
            OutStorageDetails = updateOrder.Details;
            OutStorage = updateOrder.MasterStorage;
            RaisePropertiesChanged("OutStorageDetails", "OutStorage");
        }

        /// <summary>
        ///     获取产品最新信息(Product表)
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        private Product GetProductNewInfo(Guid productGuid)
        {
            if (OutStorage.StorageId == null)
            {
                ShowNotification("请选择仓库");
                return null;
            }
            return ProductManager.GetProductNewInfo(productGuid, OutStorage.StorageId.Value);
        }
        private string OrderType()
        {
            return "YL";
        }


        /// <summary>
        ///     保存之前
        /// </summary>
        /// <returns></returns>
        private bool SaveProductOrderBefor()
        {
            if (!IsNewOrder)
            {
                //订单保存后不能修改。
                ShowNotification("订单保存后不能修改!", "警告:");
                return false;
                //var resultStr = _productOrderManager.CanUpdateProductOutOrder(OutStorage.Id);
                //if (!string.IsNullOrWhiteSpace(resultStr))
                //{
                //    ShowNotification(resultStr, "警告:");
                //    return false;
                //}
            }

            if (OutStorageDetails.Count == 0)
            {
                ShowNotification("订单明细不能为空!");
                return false;
            }

            if (OutStorage.StorageId == null || OutStorage.StorageId.Value == Guid.Empty)
            {
                ShowNotification("请选择仓库!", "失败:");
                return false;
            }

            if (OutStorage.BusinessUser == null || OutStorage.BusinessUser.Value == Guid.Empty)
            {
                ShowNotification("请选择领料人!", "失败:");
                return false;
            }

            decimal sumDecimal = 0;

            foreach (var detail in OutStorageDetails)
            {
                if (detail.SumPrice < 0)
                {
                    ShowNotification("明细金额不能小于0", "失败:");
                    return false;
                }
                sumDecimal += detail.SumPrice;
            }

            OutStorage.SumPrice = sumDecimal;
            OutStorage.StatusFlag = 0;
            if (IsNewOrder)
            {
                OutStorage.Id = Guid.NewGuid();
                OutStorage.Code = _productOrderManager.GetNewOrderCode(OrderType());
                OutStorage.OrderType = OrderType();
                OutStorage.Paid = 0;
                OutStorage.CreateUserId = App.GlobalApp.LoginUser.Id;
                OutStorage.CreateDate = DBUnit.GetDbTime();
                OutStorage.CreateIp = Utilities.Utilities.GetLocalIpAddress();
            }
            else
            {
                OutStorage.ModiftyDate = DBUnit.GetDbTime();
                OutStorage.ModiftyIp = Utilities.Utilities.GetLocalIpAddress();
                OutStorage.ModiftyUserId = App.GlobalApp.LoginUser.Id;
            }

            foreach (var detail in OutStorageDetails)
            {
                detail.OrderType = OrderType();
                if (IsNewOrder || detail.Id == Guid.Empty)
                {
                    detail.Id = Guid.NewGuid();
                }
                detail.PutOutId = OutStorage.Id;
            }

            return true;
        }

        /// <summary>
        ///     保存订单
        /// </summary>
        /// <returns></returns>
        private bool SaveProductCor()
        {
            if (!SaveProductOrderBefor()) return false;

            string result = null;
            if (IsNewOrder)
            {
                result = _rawOutOrderInsertManager.InsertProductInOrder(OutStorage, OutStorageDetails);
            }
            else
            {
                result = _rawOutOrderUpdateManager.UpdateRawOutOrder(OutStorage, OutStorageDetails,_deleteDetails);
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
            Report.Report.Print(OrderType(), OutStorage.Id);
        }


        /// <summary>
        ///     打印预览
        /// </summary>
        public virtual void PrintPreview()
        {
            Debug.Write("PrintPreview");
            if (IsNewOrder)
            {
                ShowNotification("订单未生成，不能打印!");
                return;
            }
            Report.Report.ShowPreviewDialog(OrderType(), OutStorage.Id);
        }

        /// <summary>
        ///     单元格值发生改变后
        /// </summary>
        /// <param name="e"></param>
        public virtual void CellValueChanged(CellValueChangedEventArgs e)
        {
            var editRow = e.Cell.Row as ProductOutStorageDetail;
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
                if (newInfo.StockRemind != null)
                {
                    //引用剩余库存
                    editRow.OutStock = newInfo.StockRemind.Value;
                }
                if (newInfo.UnitPrice != null)
                {
                    //引用最新单价
                    editRow.UnitPrice = newInfo.UnitPrice.Value;
                }
                editRow.SumPrice = editRow.OutStock * editRow.UnitPrice;
                editRow.ProductSpecification = newInfo.Specification;
                editRow.ProductUnit = newInfo.Unit;
                editRow.ProductName = newInfo.Name;

                editRow.ProductId = newInfo.Id;

                #endregion
            }
            else if (e.Cell.Property == "OutStock")
            {
                editRow.SumPrice = editRow.UnitPrice * editRow.OutStock;
            }
            else if (e.Cell.Property == "UnitPrice")
            {
                editRow.SumPrice = editRow.UnitPrice * editRow.OutStock;
            }
            else if (e.Cell.Property == "SumPrice")
            {
                editRow.UnitPrice = editRow.SumPrice / editRow.OutStock;
            }
        }

        /// <summary>
        ///     保存或更新订单
        /// </summary>
        /// <param name="opControl"></param>
        public void SaveProduct(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var result = SaveProductCor();
            if (result)
            {
                InitNewOrder();
            }
        }

        /// <summary>
        /// 删除选中行
        /// </summary>
        public void DeleteSelectRow(GridControl opGridControl)
        {
            Debug.Write("DeleteSelectRow");
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
                if (!IsNewOrder)
                {
                    ProductOutStorageDetail deleteDetail = (ProductOutStorageDetail)opGridControl.GetRow(row);
                    if (deleteDetail.Id != Guid.Empty)
                    {
                        _deleteDetails.Add(deleteDetail);

                    }
                }
                view.DeleteRow(row);
            }
        }

        public void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "SumPrice" || e.Column.FieldName == "UnitPrice")
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
            if (e.NewValue != null && e.NewValue.ToString().ToUpper() == "E1406832-8887-4C83-A73C-3B4DFC49CD79")
            {
                IsVisibleMoney = false;
            }
            else
            {
                IsVisibleMoney = true;
            }
            RaisePropertyChanged("IsVisibleMoney");
        }


        public RawOutStorageViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
