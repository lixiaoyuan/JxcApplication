using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using DevExpress.Mvvm;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels.Sell
{
    public class SalesBillingViewModel : ViewModelTabItem
    {
        private bool _isNewOrder;
        private ProductOrderManager _productOrderManager;

        private ProductOrderOutInsertManager _productOrderOutInsertManager;
        private ProductOrderOutUpdateManager _productOrderOutUpdateManager;
        protected AcontactManager AcontactManager;
        protected CustomerManager CustomerManager;
        protected ProductManager ProductManager;
        protected StoreManager StoreManager;

        public ObservableCollection<Product> ProductLookUp { get; set; }
        public ObservableCollection<Store> StoresLookUp { get; set; }
        public ObservableCollection<Customer> CustomersLookUp { get; set; }
        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }
        public ObservableCollection<Acontact> AcontactsLookUp { get; set; }

        public ProductOutStorage OutStorage { get; set; }
        public ObservableCollection<ProductOutStorageDetail> OutStorageDetails { get; set; }

        /// <summary>
        ///     历史纪录明细
        /// </summary>
        public ObservableCollection<OrderBrowser> HistoryOrderList { get; set; }

        private ObservableCollection<ProductOutStorageDetail> _deleteDetails = new LockableCollection<ProductOutStorageDetail>();
        /// <summary>
        /// 客户账户余额
        /// </summary>
        public decimal Balance { get; set; }
        public bool BalanceIsEnable { get; set; }
        /// <summary>
        /// 信誉度
        /// </summary>
        public decimal Credibility { get; set; }
        public bool CredibilityIsEnable { get; set; }
        /// <summary>
        /// 所以订单未结算金额
        /// </summary>
        public decimal AllOrderUnCharge { get; set; }
        public bool AllOrderUnChargeIsEnable { get; set; }
        /// <summary>
        /// 剩余信誉度
        /// </summary>
        public decimal RemainingCredibility { get; set; }
        public bool RemainingCredibilityIsEnable { get; set; }
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
            get { return "销售开单"; }
        }

        /// <summary>
        /// 引用开单单号 输入框绑定
        /// </summary>
        public virtual string InputOrderCode { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            ProductManager = ProductManager.Create();
            StoreManager = StoreManager.Create();
            CustomerManager = CustomerManager.Create();
            //AcontactManager = AcontactManager.Create();
            _productOrderManager = ProductOrderManager.Create();

            StoresLookUp = StoreManager.QueSellStores();
            CustomersLookUp = CustomerManager.QueCustomers();
            ProductLookUp = ProductManager.QueryByProductType("CP",true);
            SystemUserLookUp = SystemAccountManager.QueryLookUp();
            //AcontactsLookUp = AcontactManager.QueAcontacts();

            InitNewOrder();
        }

        private void InitNewOrder()
        {
            IsNewOrder = true;
            _productOrderOutInsertManager = ProductOrderOutInsertManager.Create();

            var newOrder = _productOrderOutInsertManager.GetInsertProductOutOrder();
            OutStorage = newOrder.MasterStorage;
            OutStorageDetails = newOrder.Details;
            RaisePropertyChanged("OutStorage");
            RaisePropertyChanged("OutStorageDetails");
        }

        /// <summary>
        ///     初始化客户信息
        /// </summary>
        private void InitCustomerInfo(Guid id)
        {
            var customer = CustomerManager.Find(id);
            if (customer == null)
            {
                return;
            }
            AllOrderUnCharge = CustomerManager.GetCustomerAllOrderPrice(id);
            if (customer.Balance != null)
                Balance = customer.Balance.Value;
            else
                Balance = 0;
            if (customer.Credibility != null)
                Credibility = customer.Credibility.Value;
            else
                Credibility = 0;

            OutStorage.GiveAddress = customer.Address;
            OutStorage.AcontackTel = customer.Tel;
            if (customer.ResponsibleSalesman.HasValue)
            {
                OutStorage.BusinessUser = customer.ResponsibleSalesman.Value;
            }
            if (!string.IsNullOrWhiteSpace(customer.AcontactName) )
            {
                OutStorage.AcontackName = customer.AcontactName;
            }
            if (!string.IsNullOrWhiteSpace(customer.AcontactTel) )
            {
                OutStorage.AcontackTel = customer.AcontactTel;
            }
            OutStorage.PaymoneyType = (int)customer.PaymentType;
            OutStorage.GiveArea = customer.Area;
            RemainingCredibility = (Credibility + Balance) - AllOrderUnCharge;
            BalanceIsEnable = true;
            CredibilityIsEnable = true;
            AllOrderUnChargeIsEnable = true;
            RemainingCredibilityIsEnable = true;
            RaisePropertiesChanged("OutStorage", "Balance", "BalanceIsEnable", "Credibility", "CredibilityIsEnable", "AllOrderUnCharge", "AllOrderUnChargeIsEnable", "RemainingCredibility", "RemainingCredibilityIsEnable");

            //decimal balance,credibility;
            //string giveAddress,giveArea, acontackTel, customAcontactName, customAcontactTel;
            //Guid acontackId, responsibleSalesman;
            //acontackId = Guid.Empty;
            //responsibleSalesman = Guid.Empty;
            //balance= credibility = 0;
            //giveAddress = acontackTel = giveArea = customAcontactName = customAcontactTel = "";
            // CustomerManager.GetCustomerInfo(id, ref balance, ref credibility, ref giveAddress,ref giveArea, ref acontackTel, ref acontackId,ref customAcontactName,ref customAcontactTel,ref responsibleSalesman);
            //AllOrderUnCharge = CustomerManager.GetCustomerAllOrderPrice(id);
            //Balance = balance;
            //Credibility = credibility;
            //OutStorage.GiveAddress = giveAddress;
            //OutStorage.AcontackTel = acontackTel;
            //OutStorage.BusinessUser = responsibleSalesman;
            //if (!string.IsNullOrWhiteSpace(customAcontactName) && string.IsNullOrWhiteSpace(OutStorage.AcontackName))
            //{
            //    OutStorage.AcontackName = customAcontactName;
            //}
            //if (!string.IsNullOrWhiteSpace(customAcontactTel) && string.IsNullOrWhiteSpace(customAcontactTel))
            //{
            //    OutStorage.AcontackTel = customAcontactTel;
            //}
            //OutStorage.GiveArea = giveArea;
            //计算剩余信誉度
            //RemainingCredibility = (Credibility + Balance) - AllOrderUnCharge;
            //BalanceIsEnable = true;
            //CredibilityIsEnable = true;
            //AllOrderUnChargeIsEnable = true;
            //RemainingCredibilityIsEnable = true;
            //RaisePropertiesChanged("OutStorage", "Balance", "BalanceIsEnable", "Credibility", "CredibilityIsEnable", "AllOrderUnCharge", "AllOrderUnChargeIsEnable", "RemainingCredibility", "RemainingCredibilityIsEnable");
        }

        /// <summary>
        ///     初始化联系人信息
        /// </summary>
        [Obsolete]
        private void InitAcontactInfo(Guid id)
        {
            throw new NotSupportedException("该方法已弃用!");
#pragma warning disable CS0162
            var acontact = AcontactManager.Find(id);
            if (acontact == null)
            {
                return;
            }
            OutStorage.GiveAddress = acontact.Address;
            OutStorage.GiveArea =acontact.Area ;
            OutStorage.AcontackTel = string.IsNullOrWhiteSpace(acontact.Tel) ? acontact.Phone : acontact.Tel;
            if (!string.IsNullOrWhiteSpace(acontact.Name)&&string.IsNullOrWhiteSpace(OutStorage.AcontackName))
            {
                OutStorage.AcontackName = acontact.Name;
            }
            RaisePropertyChanged("OutStorage");
#pragma warning restore
        }


        /// <summary>
        ///     加载历史订单
        /// </summary>
        /// <param name="arg"></param>
        public virtual void LoadHistory(ShowOrderEventArgs arg)
        {
            IsNewOrder = false;
            _deleteDetails.Clear();
            _productOrderOutUpdateManager = ProductOrderOutUpdateManager.Create();
            var updateOrder = _productOrderOutUpdateManager.GetUpdateProductOutOrder(arg.OrderId);
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
            return ProductManager.GetProductNewInfo(productGuid);
        }

        private string OrderType()
        {
            return "XK";
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
            if (OutStorage.CustomerId == null || OutStorage.CustomerId.Value == Guid.Empty)
            {
                ShowNotification("请选择客户!", "失败:");
                return false;
            }
            if (OutStorage.BusinessUser == null || OutStorage.BusinessUser.Value == Guid.Empty)
            {
                ShowNotification("请选择业务员!", "失败:");
                return false;
            }


            //foreach (var detail in OutStorageDetails)
            //{
            //    if (detail.SumPrice < 0)
            //    {
            //        ShowNotification("明细金额不能小于0", "失败:");
            //        return false;
            //    }
            //    sumDecimal += detail.SumPrice;
            //}
            var result = OutStorageDetails.Where(a => a.SumPrice < 0).ToList();
            if (result.Any())
            {
               string codes= string.Join(",", result.Select(a => a.ProductCode));
                ShowNotification(codes+ " 明细金额不能小于0", "失败:");
                return false;
            }
            decimal sumDecimal = OutStorageDetails.Sum(a => a.SumPrice);
            if (sumDecimal > RemainingCredibility)
            {
                var messageResult = DXMessageBox.Show("订单总金额已超出该客户的剩余信誉度,\n是否继续开单?", "警告:", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageResult == MessageBoxResult.No)
                {
                    return false;
                }
            }
            //检查是否超出锁定数量
            //foreach (ProductOutStorageDetail detail in OutStorageDetails)
            //{
            //    if(!detail.ProductId.HasValue)continue;
                
            //        ProductManager.CheckLockAmount(OutStorage.StorageId,detail.ProductId.Value,)
            //}
           var groupOutStock= OutStorageDetails.GroupBy(a => a.ProductId).Select(a => new {productId = a.Key, outStock = a.Sum(b => b.OutStock)});
            string msgCheckStock = "";
            foreach (var groupValue in groupOutStock)
            {
                if (!groupValue.productId.HasValue) continue;
                if (!ProductManager.CheckLockAmount(OutStorage.StorageId.Value, groupValue.productId.Value,
                    groupValue.outStock, out msgCheckStock))
                {
                    DXMessageBox.Show(msgCheckStock.Replace(@"\n","\n").Replace(@"\t","\t"), "警告:", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            //更新项目的基本信息
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
            int sort = 0;
            foreach (var detail in OutStorageDetails)
            {
                detail.OrderType = OrderType();
                if (IsNewOrder || detail.Id == Guid.Empty)
                {
                    detail.Id = Guid.NewGuid();
                }
                detail.PutOutId = OutStorage.Id;
                detail.SumPrice = detail.OutStock * detail.UnitPrice;
                detail.SortId = sort++;
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
            var lackingId = Guid.Empty;
            if (IsNewOrder)
            {
                result = _productOrderOutInsertManager.InsertProductOutOrder(OutStorage, OutStorageDetails, ref lackingId);
            }
            else
            {
                result = _productOrderOutUpdateManager.UpdateProductOutOrder(OutStorage, OutStorageDetails, _deleteDetails, ref lackingId);
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
        ///     客户选择发生改变
        /// </summary>
        public void CustomerChanged(EditValueChangedEventArgs e)
        {
            if (e.NewValue == null || !IsNewOrder)
            {
                Balance = 0;
                BalanceIsEnable = false;
                Credibility = 0;
                CredibilityIsEnable = false;
                AllOrderUnCharge = 0;
                AllOrderUnChargeIsEnable = false;
                RemainingCredibility = 0;
                RemainingCredibilityIsEnable = false;
                RaisePropertiesChanged("Balance", "BalanceIsEnable", "Credibility", "CredibilityIsEnable", "AllOrderUnCharge", "AllOrderUnChargeIsEnable", "RemainingCredibility", "RemainingCredibilityIsEnable");
                return;
            }
            //检查客户是否允许继续开单
            string checkMessage = CustomerManager.CheckAllowNewSale((Guid) e.NewValue);
            if (!string.IsNullOrWhiteSpace(checkMessage))
            {
                ShowNotification(checkMessage);
                NewProductOrder();
                return;
            }

            InitCustomerInfo((Guid) e.NewValue);
        }

        /// <summary>
        ///     联系人选择发生改变
        /// </summary>
        /// <param name="e"></param>
        [Obsolete]
        public void AcontactsChanged(EditValueChangedEventArgs e)
        {
            return;
#pragma warning disable CS0162
            if (e.NewValue == null || (Guid)e.NewValue == Guid.Empty)
                return;
            InitAcontactInfo((Guid) e.NewValue);
#pragma warning restore
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
                if (newInfo.StockRemind!=null)
                {
                    //引用剩余库存
                    editRow.OutStock = newInfo.StockRemind.Value;
                }
                editRow.ProductCode = newInfo.Code;
                if (newInfo.UnitPrice != null)
                {
                    editRow.UnitPrice = newInfo.UnitPrice.Value;
                }
                editRow.SumPrice = editRow.OutStock * editRow.UnitPrice;
                editRow.ProductSpecification = newInfo.Specification;
                editRow.ProductUnit = newInfo.Unit;

                editRow.ProductId = newInfo.Id;

                #endregion
            }
            else if (e.Cell.Property == "OutStock")
            {
                editRow.SumPrice = editRow.UnitPrice*editRow.OutStock;
            }
            else if (e.Cell.Property == "UnitPrice")
            {
                editRow.SumPrice = editRow.UnitPrice*editRow.OutStock;
            }
            else if (e.Cell.Property == "SumPrice")
            {
                editRow.UnitPrice = editRow.SumPrice/editRow.OutStock;
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
                Guid orid = OutStorage.Id;
                InitNewOrder();
                LoadHistory(new ShowOrderEventArgs(orid,null));
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

        /// <summary>
        /// 列文字显示样式
        /// </summary>
        public void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "SumPrice" || e.Column.FieldName == "UnitPrice")
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
            else if (e.Column.FieldName == "OutStock")
            {
                e.DisplayText = string.Format("{0:n}", e.Value);
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
        /// 引用销售订单明细
        /// </summary>
        public void RefOrder()
        {
            if (!IsNewOrder)
            {
                ShowNotification("只能在新订单中引入订单,请新建订单!");
                return;
            }
            MessageResult result = GetService<IDialogService>().ShowDialog(MessageButton.OKCancel, "请输入开单单号", this);
            if (result != MessageResult.OK || OutStorage == null)
            {
                return;
            }
            if (OutStorageDetails == null)
            {
                OutStorageDetails = new ObservableCollection<ProductOutStorageDetail>();
            }
            //获取销售单明细
            var detail = _productOrderManager.GetSellOutStorage(InputOrderCode.ToUpper()).OrderBy(x => x.SortId);
            if (detail == null)
            {
                return;
            }
            foreach (var outStorageDetail in detail)
            {
                OutStorageDetails.Add(new ProductOutStorageDetail()
                {
                    Id = Guid.NewGuid(),
                    ProductCode = outStorageDetail.ProductCode,
                    ProductId = outStorageDetail.ProductId,
                    ProductSpecification = outStorageDetail.ProductSpecification,
                    ProductUnit = outStorageDetail.ProductUnit,
                    UnitPrice = outStorageDetail.UnitPrice,
                    SumPrice = outStorageDetail.SumPrice,
                    OrderType = outStorageDetail.OrderType,
                    OutStock = outStorageDetail.OutStock
                });
            }
        }

        public SalesBillingViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}