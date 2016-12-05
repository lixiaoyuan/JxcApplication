using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using JxcApplication.ViewModels.Inherit;
using JxcApplication.ViewModels.Mail;
using JxcApplication.ViewModels.Relation;
using JxcApplication.ViewModels.Sell;
using JxcApplication.ViewModels.Storage;

namespace JxcApplication.ViewModels
{
    //[POCOViewModel]
    public class MainWindowViewModel : ViewModelBaseCor
    {
        public MainWindowViewModel()
        {
            TabItems = new ObservableCollection<ViewModelBaseCor>();
        }

        public ObservableCollection<ViewModelBaseCor> TabItems { get; set; }

        public string BarStaticItemTxt
        {
            get { return $"欢迎使用:{App.GlobalApp.LoginUser.Name}"; }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitRibbonUi(Guid.Parse("FA38CF52-CAFC-43DF-BA63-92958B8E3C5E"));
        }

        private Guid GetBarItemId(BarItem clickItem)
        {
            if (clickItem == null || clickItem.Links == null)
            {
                return Guid.Empty;
            }
            var link = clickItem.Links.FirstOrDefault();
            if (link != null && link.Tag != null)
            {
                Guid ribbonRootId;
                if (Guid.TryParse(link.Tag.ToString(), out ribbonRootId))
                {
                    return ribbonRootId;
                }
            }
            return Guid.Empty;
        }

        private void CreateTabItem<T>(T viewmodel) where T : ViewModelTabItem
        {
            viewmodel.Selected = true;
            TabItems.Add(viewmodel);
            //RaisePropertyChanged("TabItems");
        }

        public void RibbonNodeMaintain(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new RibbonNodeMaintainViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }

        public void ProductInStorage(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(()=>new ProductInStorageViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void RawInStorage(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new RawInStorageViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void RawOutStorage(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new RawOutStorageViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void FixedInStorage(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new FixedInStorageViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void StockAlarm(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new StockAlarmViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void SalesBilling(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new SalesBillingViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void Charge(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ChargeViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void ProductReturn(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ProductReturnViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void Expenses(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ExpensesViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void ExpensesIn(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ExpensesInViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }

        public void Wage(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(()=>new WageViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void LastStoreQue(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new LastStoreQueViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void RepotProductOutStore(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new RepotProductOutStoreViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void ReportSalesDetails(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ReportSalesDetailsViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void RepotProductInStore(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new RepotProductInStoreViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void ReportAccountTransactionRecord(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ReportAccountTransactionRecordViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void ReportAccountInfo(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ReportAccountInfoViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void ReportExpenses(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ReportExpensesViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void ReportNoPayment(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ReportNoPaymentViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }

        public void ReportPaymented(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ReportPaymentedViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }

        public void ReportHasReceivable(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ReportHasReceivableViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void Suppliers(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new SuppliersViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void Customer(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new CustomerViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void Product(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new ProductViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void UserManager(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new UserManagerViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void RoleMenuManager(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new RoleMenuManagerViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void RoleManager(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new RoleManagerViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }
        public void OrganizationStructure(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new OrganizationStructureViewModel(GetBarItemId(clickItem), clickItem.Content.ToString())));
        }

        public void NewWorkApprovalOrder(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new NewWorkApprovalOrderViewModel(GetBarItemId(clickItem),clickItem.Content.ToString())));
        }
        public void WaitWorkApprovalOrder(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new WaitWorkApprovalOrderViewModel(GetBarItemId(clickItem),clickItem.Content.ToString())));
        }
        public void MeWorkApprovalOrder(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new MeWorkApprovalOrderViewModel(GetBarItemId(clickItem),clickItem.Content.ToString())));
        }
        public void MeNewWorkApprovalOrder(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new MeNewWorkApprovalOrderViewModel(GetBarItemId(clickItem),clickItem.Content.ToString())));
        }
        public void CopyMeWorkApprovalOrder(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new CopyMeWorkApprovalOrderViewModel(GetBarItemId(clickItem),clickItem.Content.ToString())));
        }

        public void WorkApprovalMaintain(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new WorkApprovalMaintainViewModel(GetBarItemId(clickItem),clickItem.Content.ToString())));
        }

        public void Mail(BarItem clickItem)
        {
            CreateTabItem(ViewModelSource.Create(() => new MailMainViewModel(GetBarItemId(clickItem),clickItem.Content.ToString())));
        }
    }
}