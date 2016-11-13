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
        public static ObjectResult<SalesDetailModel> Report_SalesDetails(this ApplicationDbContext db,DateTime startDate,DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SalesDetailModel>("dbo.Report_SalesDetails @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
        }
        public static ObjectResult<SalesDetailModelUser> Report_SalesDetails_User(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SalesDetailModelUser>("dbo.Report_SalesDetails_User @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
        }
        public static ObjectResult<SalesDetailModelCustomer> Report_SalesDetails_Customer(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SalesDetailModelCustomer>("dbo.Report_SalesDetails_Customer @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
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
        public static ObjectResult<HasReceivableModel> Report_HasReceivable(this ApplicationDbContext db, DateTime startDate, DateTime endDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<HasReceivableModel>("dbo.Report_HasReceivable @startDate ,@endDate ", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate));
        }
        public static ObjectResult<ReportAccountInfoModel> ReportAccountInfo(this ApplicationDbContext db)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ReportAccountInfoModel>("dbo.ReportAccountInfo");
        }
        public static ObjectResult<ReportAccountInfoModel> ReportSnapshotAccountInfo(this ApplicationDbContext db,DateTime snapshotDate)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ReportAccountInfoModel>("dbo.Report_AccountSnapshot @snapshotDateTime",new SqlParameter("snapshotDateTime", snapshotDate));
        }

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
    }
}
