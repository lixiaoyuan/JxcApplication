using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;
using BusinessDb.Cor.Models.Report;

namespace BusinessDb.Cor
{
    public static class ApplicationDbContextFunction
    {
        public static ObjectResult<Product> GetProductNewInfo(this ApplicationDbContext db, Nullable<System.Guid> productId, MergeOption mergeOption)
        {
            var productIdParameter = productId.HasValue ?
                new SqlParameter("ProductId", productId) :
                new SqlParameter("ProductId", typeof(System.Guid));
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<Product>("dbo.GetProductNewInfo @ProductId",productIdParameter);
            //<Product>("GetProductNewInfo",new ExecutionOptions(MergeOption.NoTracking), productIdParameter);
        }


        public static ObjectResult<string> GetOrderCode(this ApplicationDbContext db, string type)
        {
            var typeParameter = type != null ?
                new SqlParameter("Type", type) :
                new SqlParameter("Type", typeof(string));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<string>("dbo.GetOrderCode @Type", typeParameter);
        }

        public static int GetProductInCanUpdate(this ApplicationDbContext db, Nullable<System.Guid> productInOrder, SqlParameter reurnCanUpdate)
        {
            var productInOrderParameter = productInOrder.HasValue ?
                new SqlParameter("ProductInOrder", productInOrder) :
                new SqlParameter("ProductInOrder", typeof(System.Guid));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand(" dbo.GetProductInCanUpdate @ProductInOrder , @ReurnCanUpdate out", productInOrderParameter, reurnCanUpdate);
        }

        public static int GetProductOutCanUpdate(this ApplicationDbContext db, Nullable<System.Guid> outId, SqlParameter returnCanUpdate)
        {
            var outIdParameter = outId.HasValue ?
                new SqlParameter("outId", outId) :
                new SqlParameter("outId", typeof(System.Guid));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.GetProductOutCanUpdate @outId ,@returnCanUpdate out", outIdParameter, returnCanUpdate);
        }

        public static int GetProductReturnInCanUpdate(this ApplicationDbContext db, Nullable<System.Guid> productReturnInOrder, SqlParameter reurnCanUpdate)
        {
            var productReturnInOrderParameter = productReturnInOrder.HasValue ?
                new SqlParameter("ProductReturnInOrder", productReturnInOrder) :
                new SqlParameter("ProductReturnInOrder", typeof(System.Guid));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.GetProductReturnInCanUpdate @ProductReturnInOrder ,@ReurnCanUpdate out", productReturnInOrderParameter, reurnCanUpdate);
        }

        public static ObjectResult<ProductStock> ReportStoreStock(this ApplicationDbContext db, Nullable<System.Guid> storeId)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ProductStock>("dbo.ReportStoreStock @StoreId ", new SqlParameter("StoreId",storeId) );
        }

        public static ObjectResult<StockAlarmModel> GetStockAlarm(this ApplicationDbContext db)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<StockAlarmModel>("dbo.GetStockAlarm  ");
        }
        public static ObjectResult<SalesDetailModel> Report_SalesDetails(this ApplicationDbContext db,DateTime startDate,DateTime endDate,Guid busId)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SalesDetailModel>("dbo.Report_SalesDetails @startDate ,@endDate ,@busId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate),new SqlParameter("busId",busId));
        }
        public static ObjectResult<SalesDetailModelUser> Report_SalesDetails_User(this ApplicationDbContext db, DateTime startDate, DateTime endDate, Guid busId)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SalesDetailModelUser>("dbo.Report_SalesDetails_User @startDate ,@endDate ,@busId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("busId", busId));
        }
        public static ObjectResult<SalesDetailModelCustomer> Report_SalesDetails_Customer(this ApplicationDbContext db, DateTime startDate, DateTime endDate, Guid busId)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SalesDetailModelCustomer>("dbo.Report_SalesDetails_Customer @startDate ,@endDate ,@busId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("busId", busId));
        }
        public static ObjectResult<ProductOutStoreModel> Report_ProductOutStore(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ProductOutStoreModel>("dbo.Report_ProductOutStore @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));

        }
        public static ObjectResult<ProductInStoreModel> Report_ProductInStore(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ProductInStoreModel>("dbo.Report_ProductInStore @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));

        }
        public static ObjectResult<AccountTransactionRecordModel> Report_AccountTransactionRecord(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AccountTransactionRecordModel>("dbo.Report_AccountTransactionRecord @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));

        }
        public static ObjectResult<ExpensesModel> Report_Expenses(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ExpensesModel>("dbo.Report_Expenses @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));

        }
        public static ObjectResult<PaymentModel>Report_NoPayment(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentModel>("dbo.Report_NoPayment @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
        }
        public static ObjectResult<PaymentModel> Report_Paymented(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentModel>("dbo.Report_Paymented @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
        }
        public static ObjectResult Report_PaymentedV2(this ApplicationDbContext db, DateTime startDate, DateTime endDate,string paidWithGroup,Guid userId)
        {
            if (paidWithGroup == "PaymentModel")
            {
                return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentModel>("dbo.Report_PaymentedV2 @startDate ,@endDate ,@paidWithGroup ,@userId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("paidWithGroup", paidWithGroup),new SqlParameter("userId",userId.ToString("D")));
            }
            else if (paidWithGroup == "NotPaymentModel")
            {
                return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentModel>("dbo.Report_PaymentedV2 @startDate ,@endDate ,@paidWithGroup ,@userId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("paidWithGroup", paidWithGroup), new SqlParameter("userId", userId.ToString("D")));
            }
            else if (paidWithGroup == "PaymentNotModelUser")
            {
                return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentNotModelUser>("dbo.Report_PaymentedV2 @startDate ,@endDate ,@paidWithGroup ,@userId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("paidWithGroup", paidWithGroup), new SqlParameter("userId", userId.ToString("D")));
            }
            else if (paidWithGroup == "PaymentModelUser")
            {
                return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentModelUser>("dbo.Report_PaymentedV2 @startDate ,@endDate ,@paidWithGroup ,@userId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("paidWithGroup", paidWithGroup), new SqlParameter("userId", userId.ToString("D")));
            }
            else if (paidWithGroup == "PaymentNotModelCustomer")
            {
                return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentNotModelCustomer>("dbo.Report_PaymentedV2 @startDate ,@endDate ,@paidWithGroup ,@userId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("paidWithGroup", paidWithGroup), new SqlParameter("userId", userId.ToString("D")));
            }
            else if (paidWithGroup == "PaymentModelCustomer")
            {
                return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<PaymentModelCustomer>("dbo.Report_PaymentedV2 @startDate ,@endDate ,@paidWithGroup ,@userId", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate), new SqlParameter("paidWithGroup", paidWithGroup), new SqlParameter("userId", userId.ToString("D")));
            }
            return default(ObjectResult<object>);
        }
        public static ObjectResult<HasReceivableModel> Report_HasReceivable(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<HasReceivableModel>("dbo.Report_HasReceivable @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
        }
        public static ObjectResult<ReportAccountInfoModel> ReportAccountInfo(this ApplicationDbContext db, Guid userId)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ReportAccountInfoModel>("dbo.ReportAccountInfo @userId", new SqlParameter("userId", userId.ToString("D")));
        }
        public static ObjectResult<ReportAccountInfoModel> ReportSnapshotAccountInfo(this ApplicationDbContext db,DateTime snapshotDate,Guid userId)
        {
			return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ReportAccountInfoModel>("dbo.Report_AccountSnapshot @snapshotDateTime,@userId", new SqlParameter("snapshotDateTime", snapshotDate), new SqlParameter("userId", userId.ToString("D")));
		}
        public static ObjectResult<ReportSalesSummaryMonthModel> ReportSalesSummaryMonth(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ReportSalesSummaryMonthModel>("dbo.Report_SalesSummary_Month @startDate,@endDate", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
        }
        /// <summary>
        /// 获取客户所有订单未结算金额，正数客户欠款，负数公司欠款
        /// </summary>
        /// <returns></returns>
        public static decimal GetCustomerAllOrderPrice(this ApplicationDbContext db, Guid customerId)
        {
            var outParameter = new SqlParameter("OutSumPrice", SqlDbType.Decimal);
            outParameter.Direction = ParameterDirection.Output;
            ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand(
                "dbo.GetCustomerAllOrderPrice @CustomerId ,@OutSumPrice out", new SqlParameter("CustomerId", customerId),
                outParameter);
            decimal retuvalue = 0;
            decimal.TryParse(outParameter.Value.ToString(), out retuvalue);
            return retuvalue;
        }
        /// <summary>
        /// 销售开单，检查锁定库存，锁定数量不能销售
        /// </summary>
        /// <returns></returns>
        public static bool CheckLockAmount(this ApplicationDbContext db, Guid storageId, Guid productId, decimal amount,
            out string msg)
        {
            var storageIdParameter = new SqlParameter("storageId", storageId);
            var productIdParameter = new SqlParameter("productId", productId);
            var amountParameter = new SqlParameter("amount", amount);
            var allowParameter = new SqlParameter("allow", SqlDbType.Bit, 1) {Direction = ParameterDirection.Output};
            var msgParameter = new SqlParameter("msg", SqlDbType.VarChar, 300) {Direction = ParameterDirection.Output};
            ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.CheckLockAmount @storageId,@productId,@amount,@allow out,@msg out", storageIdParameter,
                productIdParameter,
                amountParameter,
                allowParameter, msgParameter);
            msg = msgParameter.Value.ToString();
            return bool.Parse(allowParameter.Value.ToString());
        }

        public static string CheckCustomerAllowNewSale(this ApplicationDbContext db, Guid customerId)
        {
            var customerIdParameter = new SqlParameter("customerId", customerId);
            var errMsgParameter = new SqlParameter("errMsg", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand(
                "dbo.CheckCustomerAllowNewSale @customerId,@errMsg out", customerIdParameter, errMsgParameter);
            if (errMsgParameter.Value == null || errMsgParameter.Value == DBNull.Value) return string.Empty;
            return errMsgParameter.Value.ToString();
        }
    }
}
