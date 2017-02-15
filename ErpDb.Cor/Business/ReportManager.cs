using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using BusinessDb.Cor.Models.Report;
using Utilities;

namespace BusinessDb.Cor.Business
{
    public  class ReportManager
    {
        private ApplicationDbContext _applicationDb;
        public static ReportManager Create()
        {
            return new ReportManager();
        }

        public ReportManager()
        {
            _applicationDb = new ApplicationDbContext();
        }
        /// <summary>
        /// 剩余库存查询
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ProductStock> QueStoreProductStocks(Guid storeId)
        {
            return _applicationDb.ReportStoreStock(storeId).ToObservableCollection();
        }

        /// <summary>
        /// 库存报警
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<StockAlarmModel> QueStockAlarm()
        {
            return _applicationDb.GetStockAlarm().ToObservableCollection();
        }
        /// <summary>
        /// 入库库明细报表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ProductInStoreModel> QueProductInStore(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_ProductInStore(startDate, endDate).ToObservableCollection();
        }
        /// <summary>
        /// 出库明细报表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ProductOutStoreModel> QueProductOutStore(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_ProductOutStore(startDate,endDate).ToObservableCollection();
        } 
         
        /// <summary>
        /// 销售报表明细
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SalesDetailModel> QueReportSalesDetail(DateTime startDate,DateTime endDate)
        {
            return _applicationDb.Report_SalesDetails(startDate, endDate).ToObservableCollection();
        }
        public ObservableCollection<SalesDetailModelUser> QueReportSalesDetailUser(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_SalesDetails_User(startDate, endDate).ToObservableCollection();
        }
        public ObservableCollection<SalesDetailModelCustomer> QueReportSalesDetailCustomer(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_SalesDetails_Customer(startDate, endDate).ToObservableCollection();
        }
        /// <summary>
        /// 账户交易明细
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<AccountTransactionRecordModel> QueAccountTransactionRecord(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_AccountTransactionRecord(startDate, endDate).ToObservableCollection();
        }

        /// <summary>
        /// 账户基本信息
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ReportAccountInfoModel> QueReportAccountInfo()
        {
            return _applicationDb.ReportAccountInfo().ToObservableCollection();
        }
        /// <summary>
        /// 账户快照信息
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ReportAccountInfoModel> QueReportSnapshotAccountInfo(DateTime snapshotDate)
        {
            return _applicationDb.ReportSnapshotAccountInfo(snapshotDate).ToObservableCollection();
        }
        /// <summary>
        /// 费用支出明细报表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ExpensesModel> QueExpenses(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_Expenses(startDate, endDate).ToObservableCollection();
        }

        /// <summary>
        /// 客户未付款明细报表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PaymentModel> NoPayment(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_NoPayment(startDate, endDate).ToObservableCollection();
        }
        /// <summary>
        /// 客户已付款明细报表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PaymentModel> Paymented(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_Paymented(startDate, endDate).ToObservableCollection();
        }
        public object PaymentedV2(DateTime startDate, DateTime endDate,string paidWithGroup)
        {
            return _applicationDb.Report_PaymentedV2(startDate, endDate, paidWithGroup);
        }
        /// <summary>
        /// 收款报表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ObservableCollection<HasReceivableModel> HasReceivable(DateTime startDate, DateTime endDate)
        {
            return _applicationDb.Report_HasReceivable(startDate, endDate).ToObservableCollection();
        }
    }
}
