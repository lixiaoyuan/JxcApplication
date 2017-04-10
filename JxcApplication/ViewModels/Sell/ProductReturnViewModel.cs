using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using DevExpress.Mvvm;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels.Sell
{
    public class ProductReturnViewModel : ViewModelTabItem
    {
        private bool _isNewOrder;
        private ProductOrderManager _productOrderManager;
        //protected AccountManager AccountManager;
        protected AcontactManager AcontactManager;
        protected CustomerManager CustomerManager;
        protected ProductManager ProductManager;

        protected ProductReturnOrderInsertManager ProductReturnOrderInsertManager;
        protected ProductReturnOrderUpdateManager ProductReturnOrderUpdateManager;
        protected StoreManager StoreManager;

        public string Title
        {
            get { return "产品退货单"; }
        }

        public virtual string InputOrderCode { get; set; }

        public ObservableCollection<Product> ProductLookUp { get; set; }
        public ObservableCollection<Customer> CustomersLookUp { get; set; }
        public ObservableCollection<Store> StoresLookUp { get; set; }
        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }
        public ObservableCollection<Acontact> AcontactsLookUp { get; set; }
        public ObservableCollection<Account> AccountsLookUp { get; set; }

        public ProductReturnInStorage ReturnStorage { get; set; }
        public ObservableCollection<ProductInStorageDetail> Details { get; set; }
        public ObservableCollection<OrderBrowser> HistoryOrderList { get; set; }

        public bool IsNewOrder
        {
            get { return _isNewOrder; }
            set
            {
                _isNewOrder = value;
                RaisePropertyChanged("IsNewOrder");
                RaisePropertyChanged("IsHistory");
            }
        }

        public bool IsHistory
        {
            get { return !IsNewOrder; }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CustomerManager = CustomerManager.Create();
            StoreManager = StoreManager.Create();
            ProductManager = ProductManager.Create();
            AcontactManager = AcontactManager.Create();
            //AccountManager = AccountManager.Create();
            _productOrderManager = ProductOrderManager.Create();

            CustomersLookUp = CustomerManager.QueCustomers();
            StoresLookUp = StoreManager.QueSellStores();
            SystemUserLookUp = SystemAccountManager.QueryLookUp();
            AcontactsLookUp = AcontactManager.QueAcontacts();
            ProductLookUp = ProductManager.QueryByProductType("CP",true);
            AccountsLookUp = AccountManager.QueryLookUp();

            InitNewOrder();
        }

        private void InitNewOrder()
        {
            IsNewOrder = true;
            ProductReturnOrderInsertManager = ProductReturnOrderInsertManager.Create();

            var newOrder = ProductReturnOrderInsertManager.GetInsertProductReturnOrder();
            ReturnStorage = newOrder.MasterStorage;
            Details = newOrder.Details;
            RaisePropertiesChanged("Details", "ReturnStorage");
        }

        private string OrderType()
        {
            return "XT";
        }

        /// <summary>
        ///     获取产品最新信息(Product表)
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        private Product GetProductNewInfo(Guid productGuid)
        {
            return ProductManager.GetProductNewInfo(productGuid);
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
                //var resultStr = _productOrderManager.CanUpdateProductReturnInOrder(ReturnStorage.Id);
                //if (!string.IsNullOrWhiteSpace(resultStr))
                //{
                //    ShowNotification(resultStr,"警告:");
                //    return false;
                //}
            }

            if (Details.Count == 0)
            {
                ShowNotification("订单明细不能为空!");
                return false;
            }

            if ((ReturnStorage.StorageId == null || ReturnStorage.StorageId.Value == Guid.Empty) && ReturnStorage.PaymentType != "Rebate")
            {
                ShowNotification("请选择仓库!", "失败:");
                return false;
            }
            if (ReturnStorage.CustomerId == null || ReturnStorage.CustomerId.Value == Guid.Empty)
            {
                ShowNotification("请选择客户!", "失败:");
                return false;
            }
            if (ReturnStorage.PaymentType == "RefundCash" && (ReturnStorage.PaymentAccountId == null || ReturnStorage.PaymentAccountId.Value == Guid.Empty))
            {
                ShowNotification("请选择收款账户!", "失败:");
                return false;
            }
            if (ReturnStorage.BusinessUser == null || ReturnStorage.BusinessUser.Value == Guid.Empty)
            {
                ShowNotification("请选择业务员!", "失败:");
                return false;
            }

            decimal sumDecimal = 0;

            foreach (var detail in Details)
            {
                if (detail.SumPrice < 0)
                {
                    ShowNotification("明细金额不能小于0", "失败:");
                    return false;
                }
                sumDecimal += detail.SumPrice;
            }

            ReturnStorage.SumPrice = sumDecimal;
            ReturnStorage.StatusFlag = 0;
            if (IsNewOrder)
            {
                ReturnStorage.Id = Guid.NewGuid();
                ReturnStorage.Code = _productOrderManager.GetNewOrderCode(OrderType());
                ReturnStorage.OrderType = OrderType();
                ReturnStorage.CreateUserId = App.GlobalApp.LoginUser.Id;
                ReturnStorage.CreateDate = DBUnit.GetDbTime();
                ReturnStorage.CreateIp = Utilities.Utilities.GetLocalIpAddress();
            }
            else
            {
                ReturnStorage.ModiftyDate = DBUnit.GetDbTime();
                ReturnStorage.ModiftyIp = Utilities.Utilities.GetLocalIpAddress();
                ReturnStorage.ModiftyUserId = App.GlobalApp.LoginUser.Id;
            }

            foreach (var detail in Details)
            {
                detail.OrderType = OrderType();
                if (IsNewOrder || detail.Id == Guid.Empty)
                {
                    detail.Id = Guid.NewGuid();
                }
                detail.ReturnInId = ReturnStorage.Id;
                detail.StorageId = ReturnStorage.StorageId;
                detail.LastStock = detail.OriginalStock;
                if (ReturnStorage.PaymentType == "Rebate")
                {
                    //返利单，修改库存不增加，改为0
                    detail.OriginalStock = 0;
                    detail.LastStock = 0;
                }
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
                result = ProductReturnOrderInsertManager.InsertProductReturnOrder(ReturnStorage,msg=>ShowNotification(msg));
            }
            else
            {
                result = ProductReturnOrderUpdateManager.UpdateProductReturnOrder(ReturnStorage);
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
            ProductReturnOrderUpdateManager = ProductReturnOrderUpdateManager.Create();
            var updateOrder = ProductReturnOrderUpdateManager.GetUpdateProductReturnOrder(arg.OrderId);
            if (updateOrder == null)
            {
                ShowNotification("没有找到历史订单!");
                InitNewOrder();
                return;
            }
            Details = updateOrder.Details;
            ReturnStorage = updateOrder.MasterStorage;
            RaisePropertiesChanged("Details", "ReturnStorage");
        }

        /// <summary>
        ///     初始化新订单
        /// </summary>
        public void NewProductOrder()
        {
            InitNewOrder();
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
                editRow.UnitPrice = editRow.SumPrice/editRow.OriginalStock;
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

        public bool CanSaveProduct(GridControl opControl)
        {
            return IsNewOrder;
        }

        ///// <summary>
        /////     删除选中行
        ///// </summary>
        //public void DeleteSelectRow(GridControl opGridControl)
        //{
        //    Debug.Write("DeleteSelectRow");
        //    if (opGridControl == null)
        //    {
        //        return;
        //    }

        //    var view = opGridControl.View as TableView;
        //    if (view == null)
        //    {
        //        return;
        //    }
        //    var handles = opGridControl.GetSelectedRowHandles();
        //    foreach (var row in handles)
        //    {
        //        view.DeleteRow(row);
        //    }
        //}

        public void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "UnitPrice" || e.Column.FieldName == "SumPrice")
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
        }

        /// <summary>
        ///     打印
        /// </summary>
        public virtual void Print()
        {
            Debug.Write("Print");
            if (IsNewOrder)
            {
                ShowNotification("订单未生成，不能打印!");
                return;
            }
            Report.Report.Print(OrderType(), ReturnStorage.Id);
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
            Report.Report.ShowPreviewDialog(OrderType(), ReturnStorage.Id);
        }
        /// <summary>
        /// 引用销售订单明细
        /// </summary>
        public void RefOrder()
        {
            MessageResult result = GetService<IDialogService>().ShowDialog(MessageButton.OKCancel, "请输入开单单号", this);
            if (result != MessageResult.OK || ReturnStorage == null)
            {
                return;
            }
            if (Details == null)
            {
                Details = new ObservableCollection<ProductInStorageDetail>();
            }
            //获取销售单明细
            var detail= _productOrderManager.GetSellOutStorage(InputOrderCode.ToUpper());
            if (detail == null)
            {
                return;
            }
            foreach (var outStorageDetail in detail)
            {
                Details.Add(new ProductInStorageDetail()
                {
                    Id = Guid.NewGuid(),
                    ProductCode = outStorageDetail.ProductCode,
                    ProductId = outStorageDetail.ProductId,
                    ProductSpecification = outStorageDetail.ProductSpecification,
                    ProductUnit = outStorageDetail.ProductUnit,
                    OriginalStock = outStorageDetail.OutStock,
                    UnitPrice = outStorageDetail.UnitPrice,
                    SumPrice = outStorageDetail.SumPrice,
                    Remark = "ref "+InputOrderCode
                });
            }
        }

        public bool CanRefOrder()
        {
            return IsNewOrder;
        }
        public ProductReturnViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}