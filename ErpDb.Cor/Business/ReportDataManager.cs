using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BusinessDb.Cor.Models.ReportDataModel;

namespace BusinessDb.Cor.Business
{
    public  class ReportDataManager
    {
        private ApplicationDbContext _entities;

        static ReportDataManager()
        {
            Instances = new ReportDataManager();
        }

        public static readonly ReportDataManager Instances;

        private ReportDataManager()
        {
            _entities = new ApplicationDbContext();
        }

        public string GetReportUri(string orderType)
        {
            var result = _entities.OrderType.FirstOrDefault(a => a.Code==orderType);
            if (result!=null)
            {
                return result.ReportUri;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 销售订单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OutProductData> GetOutProductData(Guid id)
        {
            if (id == Guid.Empty)
            {
               yield break;
            }
            var head = _entities.Database.SqlQuery<OutProductData.Head>("SELECT  a.CreateDate ,c.Name as 'CreateUser',a.Code,a.GiveAddress,a.GiveArea,b.Name AS 'CustomerName',a.AcontackTel as 'Tel' ,d.Area as 'GiveArea' ,c2.Name AS 'BusinessUser',a.SumPrice ,a.AcontackName,a.Remark,CASE a.PaymoneyType WHEN 0 THEN '现结' WHEN 1 THEN '转账' END AS 'PaymoneyType',0.0 as 'SumQuant' FROM    dbo.ProductOutStorage a LEFT JOIN dbo.Customer b ON a.CustomerId = b.Id LEFT JOIN ApplicationDb.dbo.SystemUser c ON a.CreateUserId = c.Id LEFT JOIN ApplicationDb.dbo.SystemUser c2 ON c2.Id=a.BusinessUser LEFT JOIN dbo.Acontact d ON a.AcontackId = d.Id WHERE a.Id = @id", new SqlParameter("id", id)).FirstOrDefault();
            if (head == null)
            {
               yield break;
            }
            var details = _entities.Database.SqlQuery<OutProductData.Detail>("SELECT b.Name AS 'Name',a.ProductSpecification,a.ProductUnit,a.OutStock,a.UnitPrice,a.SumPrice  FROM dbo.ProductOutStorageDetail a LEFT JOIN dbo.Product b ON a.ProductId=b.Id WHERE PutOutId=@PutOutId ORDER BY a.SortId", new SqlParameter("PutOutId", id)).ToList();
	        head.SumQuant = details.Sum(a => a.OutStock);
            yield return   new OutProductData() { Title = head, Details = details };
        }

        /// <summary>
        /// 原材料出库单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RawOutStoreData> GetRawOutStoreDatas(Guid id)
        {
            if (id == Guid.Empty)
            {
                yield break;
            }

            var head = _entities.Database.SqlQuery<RawOutStoreData.Head>("select ProductOutStorage.CreateDate,ProductOutStorage.Code,Store.Name as 'Store',ProductOutStorage.Remark,u.Name as 'CreateUser' from ProductOutStorage left join Store on Store.Id =ProductOutStorage.StorageId left join ApplicationDb.dbo.SystemUser u on u.Id =ProductOutStorage.CreateUserId where ProductOutStorage.Id = @id", new SqlParameter("id", id)).FirstOrDefault();
            if (head == null)
            {
                yield break;
            }

            var details = _entities.Database.SqlQuery<RawOutStoreData.Detail>("select p.StoreLocation,p.StoreLocationCode,p.Name,ProductOutStorageDetail.ProductSpecification,ProductOutStorageDetail.ProductUnit,outin.SubtractStock ,inn.ProducingDate from ProductOutStorageDetail left join ProductOutInStorageDetail outin on outin.PutOutStorageId=ProductOutStorageDetail.Id left join ProductInStorageDetail ind on ind.id = outin.PutInStorageId left join ProductInStorage inn on inn.Id=ind.PutInId left join Product p on p.Id= ProductOutStorageDetail.ProductId where ProductOutStorageDetail.PutOutId=@id order by ProductOutStorageDetail.ProductId", new SqlParameter("id", id))
                .ToList();
            yield return new RawOutStoreData()
            {
                Title = head,
                Details = details
            };
        }

        /// <summary>
        /// 入库订单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductInStroeData> GetProductInStroeData(Guid id)
        {
            if (id==Guid.Empty)
            {
                yield break;
            }
            var head = _entities.Database.SqlQuery<ProductInStroeData.Head>("SELECT a.CreteDate , a.Code , b.Name AS 'Stroe', a.ProducingDate, a.Remark, c.Name AS 'CreateUser', d.Name AS 'BusinessUser' FROM dbo.ProductInStorage a LEFT JOIN dbo.Store b ON a.StorageId=b.Id LEFT JOIN ApplicationDb.dbo.SystemUser c ON c.Id=a.CreteUserId LEFT JOIN ApplicationDb.dbo.SystemUser d ON d.Id=a.DeliveredUser WHERE   a.Id = @id", new SqlParameter("id", id)).FirstOrDefault();
            if (head == null)
            {
                yield break;
            }
            var details = _entities.Database.SqlQuery<ProductInStroeData.Detail>("SELECT b.Name ,a.ProductSpecification,a.ProductUnit,a.OriginalStock,a.UnitPrice,a.SumPrice FROM ProductInStorageDetail a  LEFT JOIN dbo.Product b ON a.ProductId = b.Id WHERE   a.PutInId =@putInId ", new SqlParameter("putInId", id)).ToList();
            yield return new ProductInStroeData() { Title = head, Details = details };
        }

        /// <summary>
        /// 销售收款单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ChargeData> GetChargeData(Guid id)
        {
            if (id==Guid.Empty)
            {
                yield break;
            }
            var head=_entities.Database.SqlQuery<ChargeData.Head>("SELECT a.Code , b.Name AS 'Customer', c.Name AS 'PayAccount', a.SumPrice, d.Name AS 'BusinessUser', a.Remark, e.Name AS 'CreateUser' ,a.CreateDate FROM   dbo.Charge a  LEFT JOIN dbo.Customer b ON a.CustomerId = b.Id LEFT JOIN dbo.Account c ON a.PaymentAccountId=c.Id LEFT JOIN ApplicationDb.dbo.SystemUser d ON a.BusinessUser=d.Id LEFT JOIN ApplicationDb.dbo.SystemUser e ON a.BusinessUser=e.Id WHERE a.Id=@id", new SqlParameter("id", id)).FirstOrDefault();
            if (head==null)
            {
                yield break;
            }
            var details =
                _entities.Database.SqlQuery<ChargeData.Detail>(
                    "SELECT b.Name as 'OrderType', a.RefCode , a.RefDate , a.SumPrice , a.Paid , a.ThisPrice , a.Remark FROM dbo.ChargeDetails a LEFT JOIN dbo.OrderType b ON a.OrderType = b.Code  WHERE a.ChargeId=@ChargeId",
                    new SqlParameter("ChargeId", id)).ToList();
            yield return new ChargeData() {Title = head,Details = details};

        }

        /// <summary>
        /// 销售退货单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductReturnData> GetProductReturnData(Guid id)
        {
            if (id==Guid.Empty)
            {
                yield break;
            }

            var head =
                _entities.Database.SqlQuery<ProductReturnData.Head>(
                    "SELECT a.Code , b.Name AS 'Store' , c.Name AS 'Customer' , d.Name AS 'BuinessUser' , e.Name AS 'AcontactUser' , f.Name AS 'PaymentType' , g.Name AS 'PaymentAccount' , a.SumPrice , h.Name AS 'CreateUser' ,a.CreateDate ,a.Remark FROM dbo.ProductReturnInStorage a LEFT JOIN dbo.Store b ON a.StorageId = b.Id LEFT JOIN dbo.Customer c ON a.CustomerId = c.Id LEFT JOIN ApplicationDb.dbo.SystemUser d ON a.BusinessUser = d.Id LEFT JOIN dbo.Acontact e ON a.AcontackId = e.Id LEFT JOIN dbo.RefundType f ON a.PaymentType = f.Code LEFT JOIN dbo.Account g ON a.PaymentAccountId = g.Id LEFT JOIN ApplicationDb.dbo.SystemUser h ON a.CreateUserId = h.Id WHERE a.Id=@id",
                    new SqlParameter("id", id)).FirstOrDefault();
            if (head==null)
            {
                yield break;
            }
            var details = _entities.Database.SqlQuery<ProductReturnData.Detail>("SELECT b.Name ,a.ProductSpecification,a.ProductUnit,a.OriginalStock,a.UnitPrice,a.SumPrice FROM ProductInStorageDetail a  LEFT JOIN dbo.Product b ON a.ProductId = b.Id WHERE   a.ReturnInId =@ReturnInId ", new SqlParameter("ReturnInId", id)).ToList();
            yield return new ProductReturnData() {Title = head,Details = details};

        }

        /// <summary>
        /// 获取工资单打印数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WageData> GetWageDatas(Guid id)
        {
            if (id == Guid.Empty)
            {
                yield break;
            }
            var head = _entities.Database.SqlQuery<WageData.Head>("SELECT  CONVERT(VARCHAR(7), WageDate, 120) + ' 工资单' AS 'Title' , SumPrice , ( SELECT a.Name FROM dbo.Account a WHERE  a.Id = PaymentAccountId  ) AS 'PatymentAccount' FROM  dbo.Wage WHERE   Id = @id", new SqlParameter("id", id)).FirstOrDefault();
            if (head == null)
            {
                yield break;
            }
            var details = _entities.Database.SqlQuery<WageData.Detail>("SELECT  CASE ISNULL(UserId,'00000000-0000-0000-0000-000000000000') WHEN '00000000-0000-0000-0000-000000000000' THEN Name ELSE (SELECT TOP 1 t.Name  FROM ApplicationDb.dbo.SystemUser t WHERE t.id=UserId) END  AS 'Name', WorkDay, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15, C16, X1, X2, X3, PreTaxSum, C17, AfterTaxSum FROM dbo.WageDetail WHERE WageId=@wageid", new SqlParameter("wageid", id)).ToList();
            yield return new WageData() { Title = head, Details = details };
        }
    }
}
