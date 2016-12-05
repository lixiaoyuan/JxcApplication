using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using DevExpress.Mvvm;
using JxcApplication.Control;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels.Sell
{
    public class ChargeViewModel : ViewModelTabItem
    {
        private bool _isHistoryItemChanged;
        private ProductChargeInsertManager _chargeInsertManager;
        private PorductChargeUpdateManager _chargeUpdateManager;
        private ProductOrderManager _productOrderManager;
        private bool _isNewOrder;
        //protected AccountManager AccountManager;

        protected CustomerManager CustomerManager;

        public ObservableCollection<Customer> CustomersLookUp { get; set; }
        public ObservableCollection<Account> AccountsLookUp { get; set; }
        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }


        public Charge Charge { get; set; }
        public ObservableCollection<ChargeDetails> Details { get; set; }
        public ObservableCollection<OrderBrowser> HistoryOrderList { get; set; }

        public string Title
        {
            get { return "销售收款单"; }
        }

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

        private string OrderType()
        {
            return "XS";
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CustomerManager = CustomerManager.Create();
            //AccountManager = AccountManager.Create();

            CustomersLookUp = CustomerManager.QueCustomers();
            AccountsLookUp = AccountManager.QueryLookUp();
            SystemUserLookUp = SystemAccountManager.QueryLookUp();

            InitNewOrder();
            InitHistoryOrder();
        }

        private void InitNewOrder()
        {
            IsNewOrder = true;
            _chargeInsertManager = ProductChargeInsertManager.Create();
            _productOrderManager = ProductOrderManager.Create();

            var newOrder = _chargeInsertManager.GetInsertProducrChargeOrder();
            Charge = newOrder.MasterStorage;
            Details = newOrder.Details;
            Details.CollectionChanged += (a, b) => RefClearCore(true);
            RaisePropertiesChanged("Charge", "Details");
        }

        /// <summary>
        /// 引用订单
        /// </summary>
        /// <param name="chargeOrders"></param>
        private void CopyDetail(IEnumerable<SelectChargeOrder> chargeOrders)
        {
            foreach (var order in chargeOrders)
            {
                if (Details.Any(a => a.RefId == order.Id))
                {
                    break;
                }
                var detail = new ChargeDetails
                {
                    Id = Guid.NewGuid(),
                    OrderType = order.OrderType,
                    ChargeId = Charge.Id,
                    RefId = order.Id,
                    RefCode = order.Code,
                    RefDate = order.CreateDate,
                    Remark = order.Remark
                };
                if (order.OrderType == "XT")
                {
                    detail.SumPrice = -order.SumPrice;
                    detail.Paid = -order.Paid;
                    detail.LastPrice = -order.UnPay;
                    detail.ThisPrice = -order.UnPay;
                }
                else
                {
                    detail.SumPrice = order.SumPrice;
                    detail.Paid = order.Paid;
                    detail.LastPrice = order.UnPay;
                    detail.ThisPrice = order.UnPay;
                }
                Details.Add(detail);
            }
            RaisePropertyChanged("Details");
        }

        /// <summary>
        /// 校验清零单记录
        /// </summary>
        private void RefClearCore(bool isCollChange = false)
        {
            if (Details == null || Details.Count == 0)
            {
                return;
            }

            var chargeCler = Details.FirstOrDefault(a => a.OrderType == "QL");
            var sumPice = Details.Where(a => a.OrderType != "QL").Sum(a => a.ThisPrice);
            
            //清零
            decimal clearMoney;
            if (sumPice < 0)
            {//退钱，多退零
                clearMoney = (int)sumPice.Value - 1 - sumPice.Value;
            }
            else
            {//收钱，少收零
                clearMoney = - (sumPice.Value - (int)sumPice.Value);
            }
            if (clearMoney == 0)
            {
                return;
            }if (chargeCler == null)
            {

                if (isCollChange == false)
                {
                    //判断是否已经有添加清零记录订单
                    chargeCler = new ChargeDetails
                    {
                        Id = Guid.NewGuid(),
                        ChargeId = Charge.Id,
                        OrderType = "QL",
                        RefCode = "QL-9999-99-99-9999",
                        RefDate = DateTime.Now,
                        Remark = "清零记录",
                        //--
                        SumPrice = clearMoney,
                        Paid = 0,
                        LastPrice = clearMoney,
                        ThisPrice = clearMoney
                    };
                    Details.Add(chargeCler);
                }
                else
                {
                    return;
                }
            }
            else
            {
                chargeCler.SumPrice = clearMoney;
                chargeCler.Paid = 0;
                chargeCler.LastPrice = clearMoney;
                chargeCler.ThisPrice = clearMoney;
            }

            RaisePropertyChanged("Details");
        }

        private bool ValidateCor(ChargeDetails detail)
        {
            if (detail.LastPrice != null && detail.ThisPrice != null)
            {
                if (detail.OrderType == "XT" || detail.OrderType == "QL")
                {
                    if (detail.ThisPrice.Value < detail.LastPrice.Value)
                    {
                        ShowNotification("本次结算金额不能小于未结算金额!");
                        return false;
                    }
                    if (detail.ThisPrice.Value >= 0)
                    {
                        ShowNotification("本次结算金额不能大于等于0");
                        return false;
                    }
                }
                else
                {
                    if (detail.LastPrice.Value < detail.ThisPrice.Value)
                    {
                        ShowNotification("本次结算金额不能大于未结算金额!");
                        return false;
                    }
                    if (detail.ThisPrice.Value <= 0)
                    {
                        ShowNotification("本次结算金额不能小于等于0");
                        return false;
                    }
                }
                if (detail.ThisPrice.Value == 0)
                {
                    ShowNotification("明细金额不能为0");
                    return false;
                }
            }
            return true;
        }

        private bool Validate(ChargeDetails detal = null)
        {
            if (detal != null)
            {
                if (!ValidateCor(detal))
                {
                    return false;
                }
            }
            else
            {
                foreach (var detail in Details)
                {
                    if (!ValidateCor(detail))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 保存之前
        /// </summary>
        /// <returns></returns>
        private bool SaveProductOrderBefor()
        {
            if (!_isNewOrder)
            {
                //订单保存后不能修改。
                ShowNotification("订单保存后不能修改!", "警告:");
                return false;
            }
            if (Details == null || Details.Count == 0)
            {
                ShowNotification("保存失败,明细不能为空!");
                return false;
            }

            //检查数据
            if (Validate() == false)
            {
                return false;
            }

            if (Charge.PaymentAccountId == null || Charge.PaymentAccountId.Value == Guid.Empty)
            {
                ShowNotification("请选择账户!");
                return false;
            }

            if (Charge.CustomerId == null || Charge.CustomerId.Value == Guid.Empty)
            {
                ShowNotification("请选择客户!");
                return false;
            }
            Charge.SumPrice = Details.Sum(a => a.ThisPrice);

            if (IsNewOrder)
            {
                Charge.Code = _productOrderManager.GetNewOrderCode(OrderType());
                Charge.CreateUser = App.GlobalApp.LoginUser.Id;
                Charge.CreateDate = DBUnit.GetDbTime();
                Charge.CreateIp = Utilities.Utilities.GetLocalIpAddress();
            }
            else
            {
                Charge.ModiftyDate = DBUnit.GetDbTime();
                Charge.ModiftyIp = Utilities.Utilities.GetLocalIpAddress();
                Charge.ModiftyUserId = App.GlobalApp.LoginUser.Id;

            }

            return true;
        }

        /// <summary>
        /// 余额不足，收费窗口
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="customer">本次需要金额</param>
        /// <param name="thisSum">客户总金额</param>
        /// <returns></returns>
        private decimal InputMoney(BusinessDb.Cor.ApplicationDbContext dbContext, Customer customer, decimal thisSum)
        {
            InputMoneyDialog inputMoney = new InputMoneyDialog();
            var dataContext= ViewModelSource.Create((() => new InputMoneyDialogViewModel()));
            dataContext.CustomerSum = customer.Balance ?? 0m;
            dataContext.ThisSum = thisSum;
            dataContext.NeedMoney = thisSum - dataContext.CustomerSum;
            inputMoney.DataContext = dataContext;
            bool? result = inputMoney.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return dataContext.ThisMoney;
            }
            else
            {
                return 0;
            }
            //return false;
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
                result = _chargeInsertManager.InsertProducrChargeOrder(Charge, Details, InputMoney);
            }
            else
            {
                //result = _chargeUpdateManager.UpdateProductChargeOrder();
                result = "订单已生成不能修改!";
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
        ///     引用订单
        /// </summary>
        public void RefOrder()
        {
            if (Charge.CustomerId == null || Charge.CustomerId.Value == Guid.Empty)
            {
                ShowNotification("请先选择客户!");
                return;
            }

            var dataContent = ViewModelSource.Create(() => new SelectOrderViewModel());
            dataContent.CustomId = Charge.CustomerId.Value;

            var selectOrder = new SelectOrder();
            selectOrder.DataContext = dataContent;
            selectOrder.ShowDialogWindow();
            if (selectOrder.DialogResult != null && selectOrder.DialogResult.Value && dataContent.SelectItems != null && dataContent.SelectItems.Any())
            {
                CopyDetail(dataContent.SelectItems);
                //GC.Collect();
            }
        }

        /// <summary>
        /// 引用清空订单
        /// </summary>
        public void RefClear()
        {
            RefClearCore();
        }


        /// <summary>
        ///     初始化新订单
        /// </summary>
        public void NewProductOrder()
        {
            InitNewOrder();
        }

        /// <summary>
        ///     删除选中行
        /// </summary>
        public void DeleteSelectRow(GridControl opGridControl)
        {
            //Debug.Write("DeleteSelectRow");
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
        ///     保存
        /// </summary>
        /// <param name="opControl"></param>
        public void SaveProduct(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var result = SaveProductCor();
            if (result)
            {
                InitNewOrder();
                InitHistoryOrder();
            }
        }

        /// <summary>
        /// 显示类型
        /// </summary>
        public void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "SumPrice" || e.Column.FieldName == "Paid" || e.Column.FieldName == "LastPrice" || e.Column.FieldName == "ThisPrice")
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
        }

        /// <summary>
        /// 单元格值发生改变
        /// </summary>
        public void CellValueChanged(CellValueChangedEventArgs e)
        {
            var chargeDetail = e.Row as ChargeDetails;
            if (chargeDetail == null)
            {
                return;
            }
            if (e.Column.FieldName== "ThisPrice")
            {
                if (chargeDetail.OrderType == "QL")
                {
                    chargeDetail.SumPrice = chargeDetail.LastPrice = chargeDetail.ThisPrice;
                }
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
            Report.Report.Print(OrderType(), Charge.Id);
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
            Report.Report.ShowPreviewDialog(OrderType(), Charge.Id);
        }

        #region 历史订单


        /// <summary>
        ///     初始化或刷新历史纪录列表
        /// </summary>
        private void InitHistoryOrder()
        {
            Debug.Write("InitHistoryOrder");
            HistoryOrderList = _productOrderManager.GetHistoryChargeOrder(DBUnit.GetDbTime().AddMonths(-2));
            _isHistoryItemChanged = false;
            RaisePropertyChanged("HistoryOrderList");
        }

        /// <summary>
        ///     加载历史订单
        /// </summary>
        /// <param name="id"></param>
        private void LoadHistory(Guid id)
        {
            IsNewOrder = false;
            _chargeUpdateManager = PorductChargeUpdateManager.Create();
            //ProductReturnOrderUpdateManager = ProductReturnOrderUpdateManager.Create();
            var updateOrder = _chargeUpdateManager.GetUpdateProductChargeOrder(id);
            if (updateOrder == null)
            {
                ShowNotification("没有找到历史订单!");
                InitNewOrder();
                return;
            }
            Details = updateOrder.Details;
            Charge = updateOrder.MasterStorage;
            RaisePropertiesChanged("Details", "Charge");
        }

        /// <summary>
        ///     查看历史订单变更
        /// </summary>
        /// <param name="e"></param>
        public virtual void OrderBrowserSelectedItemChanged(SelectedItemChangedEventArgs e)
        {
            if (_isHistoryItemChanged == false)
            {
                _isHistoryItemChanged = true;
                return;
            }
            var orderBrowser = (OrderBrowser)e.NewItem;
            LoadHistory(orderBrowser.Id);
        }

        /// <summary>
        ///     查看历史订单(双击)
        /// </summary>
        /// <param name="e"></param>
        public virtual void OrderBrowserRowDoubleClick(RowDoubleClickEventArgs e)
        {
            var tableView = (TableView)e.Source;
            if (e.HitInfo.InRow)
            {
                var orderBrowser = (OrderBrowser)tableView.Grid.GetRow(e.HitInfo.RowHandle);
                LoadHistory(orderBrowser.Id);
            }
        }

        #endregion

        public ChargeViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}