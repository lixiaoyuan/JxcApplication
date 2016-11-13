using System;
using System.Collections.ObjectModel;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.EntityModels;
using BusinessDb.Cor.Models;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    /// <summary>
    /// 费用支出
    /// </summary>
    [POCOViewModel]
    public class ExpensesViewModel : ViewModelTabItem
    {
        protected AcontactManager AcontactManager;
        private CostTypeManager costTypeManager;
        private CustomerManager customerManager;
        //private AccountManager accountManager;
        private ProductOrderManager productOrderManager;
        private ExpensesInsertManager expensesInsertManager;
        private ExpensesUpdateManager expensesUpdateManager;
/*
        protected SystemAccountManager SystemAccountManager;
*/
        private bool _isHistoryItemChanged;
        private bool _isNewOrder;

        public ObservableCollection<CostType> CostTypeLookUp { get; set; }
        public ObservableCollection<Acontact> AcontactsLookUp { get; set; }
        public ObservableCollection<Customer> CustomersLookUp { get; set; }
        public ObservableCollection<Account> AccountsLookUp { get; set; }
        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }
        public ObservableCollection<OrderBrowser> HistoryOrderList { get; set; }
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
        /// <summary>
        ///     标题
        /// </summary>
        public virtual string Title
        {
            get { return "费用开支"; }
        }
        public Expenses Expenses { get; set; }
        public ObservableCollection<ExpensesDetail> ExpensesDetail { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            if (productOrderManager == null)
            {
                productOrderManager = ProductOrderManager.Create();
            }
            if (costTypeManager==null)
            {
                costTypeManager=CostTypeManager.Create();
                CostTypeLookUp = costTypeManager.QueryCostTypes();
            }

            if (AcontactManager==null)
            {
                AcontactManager=AcontactManager.Create();
                AcontactsLookUp = AcontactManager.QueAcontacts();
            }

            if (customerManager == null)
            {
                customerManager=CustomerManager.Create();
                CustomersLookUp = customerManager.QueCustomers();
            }
            //if (accountManager==null)
            //{
            //    accountManager=AccountManager.Create();
            //    AccountsLookUp = accountManager.QueAccounts();
            //}
            AccountsLookUp = AccountManager.QueryLookUp();
            //if (SystemAccountManager==null)
            //{
            //    SystemAccountManager=SystemAccountManager.Create();
            //}
            SystemUserLookUp = SystemAccountManager.QueryLookUp();

            InitNewOrder();
            InitHistoryOrder();
        }

        /// <summary>
        ///     初始化新录入费用
        /// </summary>
        protected virtual void InitNewOrder()
        {
            IsNewOrder = true;
            expensesInsertManager = ExpensesInsertManager.Create();
            var order = expensesInsertManager.GetInsertExpensesOrder();
            ExpensesDetail = order.Details;
            Expenses = order.MasterStorage;
            RaisePropertiesChanged("ExpensesDetail", "Expenses");
        }
        /// <summary>
        ///     初始化或刷新历史纪录列表
        /// </summary>
        private void InitHistoryOrder()
        {
            HistoryOrderList = productOrderManager.GetHistoryExpensesOrder( OrderType(),DBUnit.GetDbTime().AddMonths(-2));
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
            expensesUpdateManager = ExpensesUpdateManager.Create();
            var updateOrder = expensesUpdateManager.GetUpdateExpensesOrder(id);
            if (updateOrder == null)
            {
                ShowNotification("没有找到历史订单!");
                InitNewOrder();
                return;
            }
            ExpensesDetail = updateOrder.Details;
            Expenses = updateOrder.MasterStorage;
            RaisePropertiesChanged("Expenses", "ExpensesDetail");
        }
        protected  string OrderType()
        {
            return "EO";
        }

        private bool SaveProductCor()
        {
            if (!_isNewOrder)
            {
                //订单保存后不能修改。
                ShowNotification("订单保存后不能修改!", "警告:");
                return false;
            }
            if (ExpensesDetail.Count == 0)
            {
                ShowNotification("订单明细不能为空!");
                return false;
            }
            decimal? sumMoney = 0m;
            foreach (ExpensesDetail detail in ExpensesDetail)
            {
                sumMoney += detail.Cost;
                detail.ExpensesId = Expenses.Id;
                detail.Id = Guid.NewGuid();
                if (!detail.Cost.HasValue || detail.Cost < 0)
                {
                    ShowNotification("金额输入错误!");
                    return false;
                }
            }
            Expenses.SumMoney = sumMoney;
            

            string result = null;
            if (IsNewOrder)
            {
                Expenses.Code = productOrderManager.GetNewOrderCode(OrderType());
                Expenses.OrderType = OrderType();
                Expenses.Order = productOrderManager.GetExpensesOrder() + 1;
                Expenses.CreteUserId = App.GlobalApp.LoginUser.Id;
                Expenses.CreteDate = DBUnit.GetDbTime();
                Expenses.CreteIp = Utilities.Utilities.GetLocalIpAddress();
                result = expensesInsertManager.InsertExpensesOrder(Expenses,ExpensesDetail);
            }
            else
            {
                return false;
                //result = ProductOrderUpdateManager.UpdateProductInOrder();
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                ShowNotification("保存成功!");
                return true;
            }
            ShowNotification(result, "失败:");
            return false;
        }
        #region POCO Command

        /// <summary>
        ///     初始化新订单
        /// </summary>
        public void NewProductOrder()
        {
            InitNewOrder();
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
            Report.Report.Print(OrderType(), Expenses.Id);
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
            Report.Report.ShowPreviewDialog(OrderType(), Expenses.Id);
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
                InitHistoryOrder();
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
        ///     自定义列显示样式
        /// </summary>
        /// <param name="e"></param>
        public virtual void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Cost" )
            {
                e.DisplayText = string.Format("{0:c2}", e.Value);
            }
        }

        /// <summary>
        /// 添加费用类别
        /// </summary>
        public void AddCostType()
        {
            var newCostType = new CostType()
            {
                Id = Guid.NewGuid(),
                Enable = true,
                Name = ""
            };

            var diagResult = CreateDialog("DataTemplateCostTypeEdit", newCostType, "请输入费用类别名称").ShowDialog();
            if (diagResult.HasValue && diagResult.Value)
            {
                if (string.IsNullOrWhiteSpace(newCostType.Name))
                {
                    ShowNotification("添加失败，输入为空!");
                    return;
                }
                CostTypeLookUp.Add(newCostType);
                if (costTypeManager.Save())
                {
                    ShowNotification("保存成功!");
                }
                else
                {
                    ShowNotification("保存失败!");
                }
            }
            //DXDialogWindow window = new DXDialogWindow("请输入费用类别名称", MessageBoxButton.OKCancel);
            //var textEdit = new TextEdit();
            //var layoutitem = new LayoutItem()
            //{
            //    Label = "费用类别名称:",
            //    Content = textEdit
            //};

            //window.Content = layoutitem;
            //window.Width = 300;
            //window.Height = 120;
            //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //if (window.ShowDialogWindow(MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //{
                
            //    CostTypeLookUp.Add(new CostType()
            //    {
            //        Id = Guid.NewGuid(),
            //        Enable = true,
            //        Name = textEdit.EditValue.ToString()
            //    });
                
            //}
        }
        #endregion

        public ExpensesViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}