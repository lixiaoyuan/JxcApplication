using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using BusinessDb.Cor.EntityModels;
using BusinessDb.Cor.Models;
using Utilities;

namespace BusinessDb.Cor.Business
{
    public class ProductOrderManager
    {
        private readonly ApplicationDbContext _entities;
        public ProductOrderManager()
        {
            _entities = new ApplicationDbContext();
        }

        public static ProductOrderManager Create()
        {
            return new ProductOrderManager();
        }

        public string GetNewOrderCode(string orderType)
        {
            return _entities.GetOrderCode(orderType).ToArray()[0];
        }

        public int GetExpensesOrder()
        {
            var max= _entities.Expenses.OrderByDescending(a => a.Order).FirstOrDefault();
            if (max == null)
            {
                return 0;
            }
            else
            {
                return max.Order;
            }
        }

        #region 判断订单是否可以更新

        /// <summary>
        /// 判断新增入库订单是否可以修改
        /// </summary>
        /// <returns></returns>
        public string CanUpdateProductInOrder(Guid updateId)
        {
            SqlParameter returnParameter = new SqlParameter("ReurnCanUpdate", SqlDbType.VarChar, 200);
            returnParameter.Direction = ParameterDirection.Output;
            _entities.GetProductInCanUpdate(updateId, returnParameter);
            return returnParameter.Value.ToString();
        }

        /// <summary>
        /// 判断销售入库订单是否可以修改
        /// </summary>
        /// <returns></returns>
        public string CanUpdateProductOutOrder(Guid updateId)
        {
            SqlParameter returnParameter = new SqlParameter("returnCanUpdate", SqlDbType.VarChar, 200);
            returnParameter.Direction = ParameterDirection.Output;
            _entities.GetProductOutCanUpdate(updateId, returnParameter);
            return returnParameter.Value.ToString();
        }

        /// <summary>
        /// 判断销售退货入库单是否可以修改
        /// </summary>
        /// <returns></returns>
        public string CanUpdateProductReturnInOrder(Guid returnInStorageId)
        {
            SqlParameter returnParameter = new SqlParameter("ReurnCanUpdate", SqlDbType.VarChar, 200);
            returnParameter.Direction = ParameterDirection.Output;
            _entities.GetProductReturnInCanUpdate(returnInStorageId, returnParameter);
            return returnParameter.Value.ToString();
        }

        #endregion


        /// <summary>
        /// 获取历史入库订单
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<OrderBrowser> GetHistoryInOrder(string orderType,DateTime starTime)
        {
            return _entities.ProductInStorage.Where(a =>
                a.OrderType == orderType && a.CreteDate > starTime).OrderByDescending(
                    b => b.CreteDate).Select(
                        b => new OrderBrowser() { Code = b.Code, DateTime = b.CreteDate, Id = b.Id }).AsNoTracking().ToObservableCollection();
        }

        /// <summary>
        /// 获取历史销售退货入库单
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<OrderBrowser> GetHistoryReturnInOrder(string orderType,DateTime starTime)
        {
            return _entities.ProductReturnInStorage.Where(a =>
                a.OrderType == orderType && a.CreateDate > starTime).OrderByDescending(
                    b => b.CreateDate).Select(
                        b => new OrderBrowser() { Code = b.Code, DateTime = b.CreateDate, Id = b.Id }).AsNoTracking().ToObservableCollection();

        }

        /// <summary>
        /// 获取历史出库订单
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<OrderBrowser> GetHistoryOutOrder(string orderType, DateTime starTime)
        {
            return _entities.ProductOutStorage.Where(a =>
                a.OrderType == orderType && a.CreateDate > starTime).OrderByDescending(
                    b => b.CreateDate).Select(
                        b => new OrderBrowser() { Code = b.Code, DateTime = b.CreateDate, Id = b.Id }).AsNoTracking().ToObservableCollection();
        }

        /// <summary>
        /// 获取历史收款单
        /// </summary>
        /// <param name="starTime"></param>
        /// <returns></returns>
        public ObservableCollection<OrderBrowser> GetHistoryChargeOrder(DateTime starTime)
        {
            return _entities.Charge.Where(
                    a => a.CreateDate > starTime).OrderByDescending(
                        b => b.CreateDate).Select(
                            c => new OrderBrowser() {Code = c.Code,DateTime = c.CreateDate,Id = c.Id}).ToObservableCollection();
        }

        /// <summary>
        /// 获取历史费用开支单
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<OrderBrowser> GetHistoryExpensesOrder(string orderType,DateTime starTime)
        {
            return
                _entities.Expenses.Where(a => a.OrderType == orderType && a.CreteDate >= starTime)
                    .OrderByDescending(a => a.CreteDate)
                    .Select(a => new OrderBrowser() { Code = a.Code, DateTime = a.CreteDate, Id = a.Id })
                    .ToObservableCollection();
        } 

        /// <summary>
        /// 获取待收款订单
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SelectChargeOrder> GetOrders(Guid customId,DateTime starTime, DateTime endTime)
        {
            return _entities.Database.SqlQuery<SelectChargeOrder>("SELECT Id, OrderType, Code,CreateDate,ISNULL(SumPrice,0) AS 'SumPrice',ISNULL(Paid,0) AS 'Paid',ISNULL(SumPrice,0)-ISNULL(Paid,0) AS 'UnPay',Remark FROM dbo.ProductReturnInStorage WHERE PaymentType in('OffsetTransactions','Rebate')  AND CreateDate>=@StartTime AND CreateDate <=@EndTime AND CustomerId=@customerId AND ISNULL(SumPrice, 0) - ISNULL(Paid, 0) > 0 UNION ALL SELECT Id, OrderType,Code,CreateDate,ISNULL(SumPrice,0) AS 'SumPrice',ISNULL(Paid,0) AS 'Paid',ISNULL(SumPrice,0)-ISNULL(Paid,0) AS 'UnPay',Remark FROM dbo.ProductOutStorage WHERE CreateDate>=@StartTime AND CreateDate <=@EndTime AND CustomerId=@customerId AND ISNULL(SumPrice, 0) - ISNULL(Paid, 0) > 0", new SqlParameter("StartTime", starTime), new SqlParameter("EndTime", endTime),new SqlParameter("customerId", customId)).ToObservableCollection();
        }
        /// <summary>
        /// 获取销售开单明细
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ProductOutStorageDetail> GetSellOutStorage(string outCode)
        {
            var outOrder = _entities.ProductOutStorage.AsNoTracking().FirstOrDefault(a => a.OrderType == "XK" && a.Code == outCode.ToUpper());
            if (outOrder == default(ProductOutStorage))
            {
                return null;
            }
            return _entities.ProductOutStorageDetail.Where(a => a.PutOutId == outOrder.Id).AsNoTracking().ToObservableCollection();
        }
    }


    #region 新增入库单

    /// <summary>
    /// 入库单新增管理
    /// </summary>
    public class ProductOrderInsertManager
    {
        private ApplicationDbContext _entities;
        public static ProductOrderInsertManager Create()
        {
            return new ProductOrderInsertManager();
        }

        private ProductOrderInsertManager()
        {

        }

        public string InsertProductInOrder()
        {

            try
            {
                _entities.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }

            return null;
        }

        public CategoriesOrder<ProductInStorage, ProductInStorageDetail> GetInsertProductInOrder()
        {

            _entities = new ApplicationDbContext();
            ProductInStorage inserInStorage = new ProductInStorage() { Id = Guid.NewGuid() };
            _entities.ProductInStorage.Add(inserInStorage);
            _entities.ProductInStorage.Where(a => a.Id == inserInStorage.Id).Load();
            _entities.ProductInStorageDetail.Where(a => a.PutInId == inserInStorage.Id).Load();
            return new CategoriesOrder<ProductInStorage, ProductInStorageDetail>()
            {
                Details = _entities.ProductInStorageDetail.Local,
                MasterStorage = inserInStorage
            };
        }

    }

    /// <summary>
    /// 入库单更新管理
    /// </summary>
    public class ProductOrderUpdateManager
    {
        private ApplicationDbContext _entities;
        public static ProductOrderUpdateManager Create()
        {
            return new ProductOrderUpdateManager();
        }

        private ProductOrderUpdateManager()
        {

        }

        public string UpdateProductInOrder()
        {
            try
            {
                _entities.SaveChanges();
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }

        public CategoriesOrder<ProductInStorage,ProductInStorageDetail> GetUpdateProductInOrder(Guid productInStoreId)
        {
            try
            {
                _entities = new ApplicationDbContext();
                _entities.ProductInStorage.Where(a => a.Id == productInStoreId).Load();
                _entities.ProductInStorageDetail.Where(a => a.PutInId == productInStoreId).Load();
                return new CategoriesOrder<ProductInStorage, ProductInStorageDetail>() { Details = _entities.ProductInStorageDetail.Local, MasterStorage = _entities.ProductInStorage.Local.First() };
            }
            catch (Exception)
            {
                //if (_updateOrderEntities != null) _updateOrderEntities.Dispose();
                return null;
            }
        }
    }

    #endregion

    #region 原材料领用

    public class RawOutOrderInsertManager
    {
        private ApplicationDbContext _entities;
        public static RawOutOrderInsertManager Create()
        {
            return new RawOutOrderInsertManager();
        }

        private RawOutOrderInsertManager()
        {

        }

        public string InsertProductInOrder(ProductOutStorage productOutStorage, ObservableCollection<ProductOutStorageDetail> details)
        {
            DbContextTransaction inserTransaction = null;
            try
            {
                inserTransaction = _entities.Database.BeginTransaction();
                _entities.ProductInStorageDetail.Detacheds(_entities);
                _entities.ProductOutInStorageDetail.Detacheds(_entities);

                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                //订单总金额
                decimal sumPrice = 0;
                List<Guid> lockedList = new List<Guid>();

                foreach (ProductOutStorageDetail detail in details)
                {
                    if (detail.ProductId == null || detail.ProductId.Value == Guid.Empty)
                    {
                        inserTransaction.Rollback();
                        return string.Format("开单失败:{0} 产品Id为空!", detail.ProductCode);
                    }
                    var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();

                    #region 判断库存,或去当前仓库当前产品的剩余数量

                    var sumStock = storageDetails.Sum(a => a.LastStock);
                    if (sumStock < detail.OutStock)
                    {

                        inserTransaction.Rollback();
                        return String.Format("开单失败:{0} 库存不足!", detail.ProductCode);

                    }

                    #endregion

                    decimal needStock = detail.OutStock;
                    foreach (ProductInStorageDetail inStorageDetail in storageDetails)
                    {
                        //记录出入库对照明细
                        ProductOutInStorageDetail addOutInStorage = new ProductOutInStorageDetail()
                        {
                            Id = Guid.NewGuid(),
                            PutInStorageId = inStorageDetail.Id,
                            PutOutStorageId = detail.Id,
                            OriginalStock = inStorageDetail.LastStock,
                            CreateUserId = productOutStorage.CreateUserId,
                            CreateDate = nowDateTime,
                            CreateIp = ip
                        };
                        _entities.ProductOutInStorageDetail.Add(addOutInStorage);
                        //记录已扣库存的入库主单或者退货入库主单
                        if (inStorageDetail.PutInId != null)
                        {
                            //入库单
                            if (inStorageDetail.PutInId.Value != Guid.Empty)
                            {
                                if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                {
                                    lockedList.Add(inStorageDetail.PutInId.Value);
                                }
                            }
                        }

                        //库存扣除调整
                        if ((inStorageDetail.LastStock >= needStock))
                        {
                            inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                            addOutInStorage.SubtractStock = needStock;
                            addOutInStorage.LasetStock = inStorageDetail.LastStock;
                            needStock = 0;
                            break;
                        }
                        else
                        {
                            needStock -= inStorageDetail.LastStock;
                            addOutInStorage.SubtractStock = inStorageDetail.LastStock;
                            inStorageDetail.LastStock = 0;
                            addOutInStorage.LasetStock = 0;
                        }
                    }
                    //判断库存是否足够扣完
                    if (needStock > 0)
                    {
                        inserTransaction.Rollback();
                        return String.Format("开单失败:{0} 尝试扣库存失败!", detail.ProductCode);
                    }
                    sumPrice += detail.SumPrice;

                }
                if (lockedList.Any())
                {
                    //更新入库单标记为已扣库存
                    _entities.Database.ExecuteSqlCommand($"UPDATE dbo.[ProductInStorage] SET StatusFlag=1 WHERE Id IN('{string.Join("','", lockedList)}')");
                }
                //更新主记录的订单总金额
                productOutStorage.SumPrice = sumPrice;
                _entities.SaveChanges();
                inserTransaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (inserTransaction != null)
                {
                    inserTransaction.Rollback();
                }
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }

        }

        public CategoriesOrder<ProductOutStorage, ProductOutStorageDetail> GetInsertRawOutOrder()
        {

            _entities = new ApplicationDbContext();
            ProductOutStorage productOutStorage = new ProductOutStorage() { Id = Guid.NewGuid() };
            _entities.ProductOutStorage.Add(productOutStorage);
            _entities.ProductOutStorage.Where(a => a.Id == productOutStorage.Id).Load();
            _entities.ProductOutStorageDetail.Where(a => a.PutOutId == productOutStorage.Id).Load();
            return new CategoriesOrder<ProductOutStorage, ProductOutStorageDetail>()
            {
                MasterStorage = productOutStorage,
                Details = _entities.ProductOutStorageDetail.Local
            };
        }
    }

    public class RawOutOrderUpdateManager
    {
        private ApplicationDbContext _entities;
        public static RawOutOrderUpdateManager Create()
        {
            return new RawOutOrderUpdateManager();
        }

        private RawOutOrderUpdateManager()
        {

        }
        public CategoriesOrder<ProductOutStorage, ProductOutStorageDetail> GetUpdateProductOutOrder(Guid rawProductOutId)
        {
            try
            {
                _entities = new ApplicationDbContext();
                _entities.ProductOutStorage.Where(a => a.Id == rawProductOutId).Load();
                _entities.ProductOutStorageDetail.Where(a => a.PutOutId == rawProductOutId).Load();

                return new CategoriesOrder<ProductOutStorage, ProductOutStorageDetail>() { Details = _entities.ProductOutStorageDetail.Local, MasterStorage = _entities.ProductOutStorage.Local.First() };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string UpdateRawOutOrder(ProductOutStorage productOutStorage, ObservableCollection<ProductOutStorageDetail> details, ObservableCollection<ProductOutStorageDetail> deleteDetails)
        {
            DbContextTransaction updateTransaction = null;
            try
            {
                updateTransaction = _entities.Database.BeginTransaction();
                _entities.ProductInStorageDetail.Detacheds(_entities);
                _entities.ProductOutInStorageDetail.Detacheds(_entities);


                //初始化基本信息
                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();

                //订单总金额
                decimal sumPrice = 0;
                //记录已扣库存的主订单，用于更新状态标记为已扣库存
                List<Guid> lockedList = new List<Guid>();

                //清空历史出库记录，并退回历史出库数量
                foreach (ProductOutStorageDetail detail in deleteDetails)
                {
                    _entities.ProductOutInStorageDetail.Where(a => a.PutOutStorageId == detail.Id).OrderBy(a => a.LasetStock).Load();
                    while (_entities.ProductOutInStorageDetail.Local.Count > 0)
                    {
                        var subtractStock = _entities.ProductOutInStorageDetail.Local[0].SubtractStock;
                        if (subtractStock != null)
                            _entities.Database.ExecuteSqlCommand("UPDATE dbo.ProductInStorageDetail SET LastStock=LastStock+@LastStock WHERE Id=@id", new SqlParameter("LastStock", subtractStock.Value), new SqlParameter("id", _entities.ProductOutInStorageDetail.Local[0].PutInStorageId));
                        _entities.Entry(_entities.ProductOutInStorageDetail.Local[0]).State = EntityState.Deleted;
                    }
                }

                //处理订单
                foreach (ProductOutStorageDetail detail in details)
                {
                    if (detail.ProductId == null || detail.ProductId.Value == Guid.Empty)
                    {
                        updateTransaction.Rollback();
                        return $"开单失败:{detail.ProductCode} 产品Id为空!";
                    }
                    var objectEntity = _entities.Entry(detail);
                    var state = objectEntity.State;
                    if (state == EntityState.Added)
                    {
                        #region Added

                        var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();
                        var sumStock = storageDetails.Sum(a => a.LastStock);
                        if (sumStock < detail.OutStock)
                        {
                            updateTransaction.Rollback();
                            return $"开单失败:{detail.ProductCode} 库存不足!";
                        }
                        decimal needStock = detail.OutStock;
                        foreach (var inStorageDetail in storageDetails)
                        {
                            //记录出入库对照明细
                            ProductOutInStorageDetail temAddOutInStorageDetail = new ProductOutInStorageDetail()
                            {
                                Id = Guid.NewGuid(),
                                PutInStorageId = inStorageDetail.Id,
                                PutOutStorageId = detail.Id,
                                OriginalStock = inStorageDetail.LastStock,
                                CreateUserId = productOutStorage.CreateUserId,
                                CreateDate = nowDateTime,
                                CreateIp = ip
                            };
                            _entities.ProductOutInStorageDetail.Add(temAddOutInStorageDetail);

                            //记录已扣库存的入库主单或者退货入库主单
                            if (inStorageDetail.PutInId != null)
                            {
                                //入库单
                                if (inStorageDetail.PutInId.Value != Guid.Empty)
                                {
                                    if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                    {
                                        lockedList.Add(inStorageDetail.PutInId.Value);
                                    }
                                }
                            }


                            //库存扣除调整
                            if ((inStorageDetail.LastStock >= needStock))
                            {
                                inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                                temAddOutInStorageDetail.SubtractStock = needStock;
                                temAddOutInStorageDetail.LasetStock = inStorageDetail.LastStock;
                                needStock = 0;
                                break;
                            }
                            else
                            {
                                needStock -= inStorageDetail.LastStock;
                                temAddOutInStorageDetail.SubtractStock = inStorageDetail.LastStock;
                                inStorageDetail.LastStock = 0;
                                temAddOutInStorageDetail.LasetStock = 0;
                            }
                        }
                        //判断库存是否足够扣完
                        if (needStock > 0)
                        {
                            updateTransaction.Rollback();
                            return $"开单失败:{detail.ProductCode} 尝试扣库存失败!";
                        }
                        sumPrice += detail.SumPrice;

                        #endregion
                    }
                    else if (state == EntityState.Modified)
                    {
                        #region Modified

                        //判断
                        decimal needStock = detail.OutStock;
                        ProductOutStorageDetail originalOutDetail = _entities.Database.SqlQuery<ProductOutStorageDetail>("SELECT * FROM dbo.ProductOutStorageDetail WHERE Id=@id", new SqlParameter("id", detail.Id)).FirstOrDefault();
                        if (originalOutDetail == null)
                        {
                            updateTransaction.Rollback();
                            return $"{detail.Id}历史明细不存在，更新失败!";
                        }
                        if (objectEntity.CurrentValues.GetValue<decimal>("OutStock") - originalOutDetail.OutStock > 0)
                        {
                            #region 出库数量调大,继续扣除加大数量库存

                            needStock -= originalOutDetail.OutStock;
                            var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();
                            var sumStock = storageDetails.Sum(a => a.LastStock);
                            if (sumStock < needStock)
                            {
                                updateTransaction.Rollback();
                                return $"开单失败:{detail.ProductCode} 库存不足!";
                            }
                            foreach (var inStorageDetail in storageDetails)
                            {
                                //记录出入库对照明细
                                ProductOutInStorageDetail temAddOutInStorageDetail = new ProductOutInStorageDetail()
                                {
                                    Id = Guid.NewGuid(),
                                    PutInStorageId = inStorageDetail.Id,
                                    PutOutStorageId = detail.Id,
                                    OriginalStock = inStorageDetail.LastStock,
                                    CreateUserId = productOutStorage.CreateUserId,
                                    CreateDate = nowDateTime,
                                    CreateIp = ip
                                };
                                _entities.ProductOutInStorageDetail.Add(temAddOutInStorageDetail);

                                //记录已扣库存的入库主单或者退货入库主单
                                if (inStorageDetail.PutInId != null)
                                {
                                    //入库单
                                    if (inStorageDetail.PutInId.Value != Guid.Empty)
                                    {
                                        if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                        {
                                            lockedList.Add(inStorageDetail.PutInId.Value);
                                        }
                                    }
                                }


                                //库存扣除调整
                                if ((inStorageDetail.LastStock >= needStock))
                                {
                                    inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                                    temAddOutInStorageDetail.SubtractStock = needStock;
                                    temAddOutInStorageDetail.LasetStock = inStorageDetail.LastStock;
                                    needStock = 0;
                                    break;
                                }
                                else
                                {
                                    needStock -= inStorageDetail.LastStock;
                                    temAddOutInStorageDetail.SubtractStock = inStorageDetail.LastStock;
                                    inStorageDetail.LastStock = 0;
                                    temAddOutInStorageDetail.LasetStock = 0;
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region 出库数量调小，还原已扣库存

                            //清空出入库明细记录，并还原入库单数量
                            var listOutInDetail = _entities.ProductOutInStorageDetail.Where(a => a.PutOutStorageId == detail.Id).OrderBy(a => a.LasetStock).ToList();
                            foreach (var outInStorageDetail in listOutInDetail)
                            {
                                if (outInStorageDetail.SubtractStock != null)
                                    _entities.Database.ExecuteSqlCommand("UPDATE dbo.ProductInStorageDetail SET LastStock=LastStock+@LastStock WHERE Id=@id", new SqlParameter("LastStock", outInStorageDetail.SubtractStock.Value), new SqlParameter("id", outInStorageDetail.PutInStorageId));
                            }
                            _entities.Database.ExecuteSqlCommand($"DELETE dbo.ProductOutInStorageDetail WHERE PutOutStorageId IN ('{string.Join("','", listOutInDetail.Select(a => a.PutOutStorageId))}')");
                            //继续出库存
                            var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();
                            var sumStock = storageDetails.Sum(a => a.LastStock);
                            if (sumStock < detail.OutStock)
                            {
                                updateTransaction.Rollback();
                                return $"开单失败:{detail.ProductCode} 库存不足!";
                            }
                            foreach (var inStorageDetail in storageDetails)
                            {
                                //记录出入库对照明细
                                ProductOutInStorageDetail temAddOutInStorageDetail = new ProductOutInStorageDetail()
                                {
                                    Id = Guid.NewGuid(),
                                    PutInStorageId = inStorageDetail.Id,
                                    PutOutStorageId = detail.Id,
                                    OriginalStock = inStorageDetail.LastStock,
                                    CreateUserId = productOutStorage.CreateUserId,
                                    CreateDate = nowDateTime,
                                    CreateIp = ip
                                };
                                _entities.ProductOutInStorageDetail.Add(temAddOutInStorageDetail);

                                //记录已扣库存的入库主单或者退货入库主单
                                if (inStorageDetail.PutInId != null)
                                {
                                    //入库单
                                    if (inStorageDetail.PutInId.Value != Guid.Empty)
                                    {
                                        if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                        {
                                            lockedList.Add(inStorageDetail.PutInId.Value);
                                        }
                                    }
                                }


                                //库存扣除调整
                                if ((inStorageDetail.LastStock >= needStock))
                                {
                                    inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                                    temAddOutInStorageDetail.SubtractStock = needStock;
                                    temAddOutInStorageDetail.LasetStock = inStorageDetail.LastStock;
                                    needStock = 0;
                                    break;
                                }
                                else
                                {
                                    needStock -= inStorageDetail.LastStock;
                                    temAddOutInStorageDetail.SubtractStock = inStorageDetail.LastStock;
                                    inStorageDetail.LastStock = 0;
                                    temAddOutInStorageDetail.LasetStock = 0;
                                }
                            }

                            #endregion
                        }
                        if (needStock > 0)
                        {
                            updateTransaction.Rollback();
                            return $"开单失败:{detail.ProductCode} 尝试扣库存失败!";
                        }

                        sumPrice += detail.SumPrice;

                        #endregion
                    }
                    else if (state == EntityState.Deleted)
                    {
                        #region Deleted


                        #endregion
                    }
                    else if (state == EntityState.Unchanged)
                    {
                        sumPrice += detail.SumPrice;
                    }

                }
                //if ((productOutStorage.SumPrice - sumPrice) != 0)
                //{
                //    if (!query($"调整差价为:{productOutStorage.SumPrice - sumPrice},是否继续保持?"))
                //    {
                //        updateTransaction.Rollback();
                //        return "取消保存!";
                //    }
                //}

                //更新入库单标记为已扣库存
                if (lockedList.Any())
                {
                    _entities.Database.ExecuteSqlCommand($"UPDATE dbo.[ProductInStorage] SET StatusFlag=1 WHERE Id IN('{string.Join("','", lockedList)}')");
                }

                //更新主记录的订单总金额
                productOutStorage.SumPrice = sumPrice;

                _entities.SaveChanges();
                updateTransaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (updateTransaction != null)
                {
                    updateTransaction.Rollback();
                }

                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
    #endregion

    #region 销售出库单

    /// <summary>
    /// 出库单新增管理
    /// </summary>
    public class ProductOrderOutInsertManager
    {
        private ApplicationDbContext _entities;

        public static ProductOrderOutInsertManager Create()
        {
            return new ProductOrderOutInsertManager();
        }

        private ProductOrderOutInsertManager()
        {
        }

        public CategoriesOrder<ProductOutStorage,ProductOutStorageDetail> GetInsertProductOutOrder()
        {
            _entities = new ApplicationDbContext();
            ProductOutStorage productOutStorage = new ProductOutStorage() { Id = Guid.NewGuid() };
            _entities.ProductOutStorage.Add(productOutStorage);
            _entities.ProductOutStorage.Where(a => a.Id == productOutStorage.Id).Load();
            _entities.ProductOutStorageDetail.Where(a => a.PutOutId == productOutStorage.Id).Load();
            return new CategoriesOrder<ProductOutStorage, ProductOutStorageDetail>()
            {
                MasterStorage = productOutStorage,
                Details = _entities.ProductOutStorageDetail.Local
            };
        }

        /// <summary>
        /// 保存新增销售订单
        /// </summary>
        /// <param name="productOutStorage">订单主记录</param>
        /// <param name="details">订单明细</param>
        /// <param name="lackingId">返回参数，库存不足产品Id</param>
        /// <returns>空，成功，否，错误消息</returns>
        public string InsertProductOutOrder(ProductOutStorage productOutStorage, ObservableCollection<ProductOutStorageDetail> details, ref Guid lackingId)
        {
            DbContextTransaction inserTransaction = null;
            try
            {
                inserTransaction = _entities.Database.BeginTransaction();
                _entities.ProductInStorageDetail.Detacheds(_entities);
                _entities.ProductOutInStorageDetail.Detacheds(_entities);
                _entities.Customer.Detacheds(_entities);
                //_entities.CustomerAccountRecord.Detacheds(_entities);

                var custom = _entities.Customer.Find(productOutStorage.CustomerId);
                if (custom == null)
                {
                    inserTransaction.Rollback();
                    return "请选择客户!";
                }

                //while (_entities.ProductOutInStorageDetail.Local.Count > 0)
                //{
                //    _entities.Entry(_entities.ProductOutInStorageDetail.Local[0]).State = EntityState.Detached;
                //}
                //while (_entities.CustomerAccountRecord.Local.Count > 0)
                //{
                //    _entities.Entry(_entities.CustomerAccountRecord.Local[0]).State = EntityState.Detached;
                //}

                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                lackingId = Guid.Empty;
                //订单总金额
                decimal sumPrice = 0;
                //记录已扣库存的主订单，用于更新状态标记为已扣库存
                List<Guid> lockedList = new List<Guid>();
                //记录已扣库存的退货入库单单,用于更新退货入库单状态标记为已扣库存
                List<Guid> lockedReturnList = new List<Guid>();

                //处理订单
                foreach (ProductOutStorageDetail detail in details)
                {
                    if (detail.ProductId == null || detail.ProductId.Value == Guid.Empty)
                    {
                        inserTransaction.Rollback();
                        return string.Format("开单失败:{0} 产品Id为空!", detail.ProductCode);
                    }

                    var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();

                    #region 判断库存,或去当前仓库当前产品的剩余数量

                    var sumStock = storageDetails.Sum(a => a.LastStock);
                    if (sumStock < detail.OutStock)
                    {

                        lackingId = detail.ProductId.Value;
                        inserTransaction.Rollback();
                        return String.Format("开单失败:{0} 库存不足!", detail.ProductCode);

                    }

                    #endregion

                    #region 扣库存,并添加扣库存记录

                    decimal needStock = detail.OutStock;
                    foreach (ProductInStorageDetail inStorageDetail in storageDetails)
                    {
                        //记录出入库对照明细
                        ProductOutInStorageDetail addOutInStorage = new ProductOutInStorageDetail()
                        {
                            Id = Guid.NewGuid(),
                            PutInStorageId = inStorageDetail.Id,
                            PutOutStorageId = detail.Id,
                            OriginalStock = inStorageDetail.LastStock,
                            CreateUserId = productOutStorage.CreateUserId,
                            CreateDate = nowDateTime,
                            CreateIp = ip
                        };
                        _entities.ProductOutInStorageDetail.Add(addOutInStorage);

                        //记录已扣库存的入库主单或者退货入库主单
                        if (inStorageDetail.PutInId != null)
                        {
//入库单
                            if (inStorageDetail.PutInId.Value != Guid.Empty)
                            {
                                if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                {
                                    lockedList.Add(inStorageDetail.PutInId.Value);
                                }
                            }
                        }
                        else if (inStorageDetail.ReturnInId != null)
                        {
//退货入库单
                            if (inStorageDetail.ReturnInId.Value != Guid.Empty)
                            {
                                if (!lockedReturnList.Contains(inStorageDetail.ReturnInId.Value))
                                {
                                    lockedReturnList.Add(inStorageDetail.ReturnInId.Value);
                                }
                            }
                        }


                        //库存扣除调整
                        if ((inStorageDetail.LastStock >= needStock))
                        {
                            inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                            addOutInStorage.SubtractStock = needStock;
                            addOutInStorage.LasetStock = inStorageDetail.LastStock;
                            needStock = 0;
                            break;
                        }
                        else
                        {
                            needStock -= inStorageDetail.LastStock;
                            addOutInStorage.SubtractStock = inStorageDetail.LastStock;
                            inStorageDetail.LastStock = 0;
                            addOutInStorage.LasetStock = 0;
                        }



                    }
                    //判断库存是否足够扣完
                    if (needStock > 0)
                    {
                        lackingId = detail.ProductId.Value;
                        inserTransaction.Rollback();
                        return String.Format("开单失败:{0} 尝试扣库存失败!", detail.ProductCode);
                    }

                    #endregion

                    sumPrice += detail.SumPrice;
                }

                if (lockedList.Any())
                {
                    //更新入库单标记为已扣库存
                    _entities.Database.ExecuteSqlCommand($"UPDATE dbo.[ProductInStorage] SET StatusFlag=1 WHERE Id IN('{string.Join("','", lockedList)}')");
                }
                if (lockedReturnList.Any())
                {
                    //更新退货入库单标记为已扣库存
                    _entities.Database.ExecuteSqlCommand($"UPDATE dbo.[ProductReturnInStorage] SET StatusFlag=1 WHERE Id IN('{string.Join("','", lockedReturnList)}')");
                }

                //更新主记录的订单总金额
                productOutStorage.SumPrice = sumPrice;

                //如果客户金额为空则更新为0
                if (!custom.Balance.HasValue)
                {
                    custom.Balance = 0m;
                }
                if (!custom.Credibility.HasValue)
                {
                    custom.Credibility = 0m;
                }
                //
                //if (custom.Balance.Value - sumPrice < -custom.Credibility.Value)
                //{
                //    if (!query($"客户({custom.Name})信誉度不足{Math.Abs(custom.Balance.Value - sumPrice)}，是否继续保存?"))
                //    {
                //        inserTransaction.Rollback();
                //        return "客户信誉度不足，取消保存!";
                //    }
                //}
                ////写客户扣款记录
                //var customerAc = new CustomerAccountRecord()
                //{
                //    Id = Guid.NewGuid(),
                //    CustomerId = productOutStorage.CustomerId,
                //    TransactionTypeEnum =TransactionTypeEnum.AddOut,
                //    TransactionBefore = custom.Balance,
                //    TransactionBalance = -sumPrice,
                //    CreateUserId = productOutStorage.CreateUserId,
                //    CreateDate = nowDateTime,
                //    CreateIp = ip
                //};
                //_entities.CustomerAccountRecord.Add(customerAc);

                ////更新客户余额
                //custom.Balance -= sumPrice;
                //customerAc.TransactionAfter = custom.Balance;

                _entities.SaveChanges();
                inserTransaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (inserTransaction != null)
                {
                    inserTransaction.Rollback();
                }
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }
    }

    /// <summary>
    /// 出库单更新
    /// </summary>
    public class ProductOrderOutUpdateManager
    {

        private ApplicationDbContext _entities;

        public static ProductOrderOutUpdateManager Create()
        {
            return new ProductOrderOutUpdateManager();
        }

        private ProductOrderOutUpdateManager()
        {

        }

        public string UpdateProductOutOrder(ProductOutStorage productOutStorage, ObservableCollection<ProductOutStorageDetail> details, ObservableCollection<ProductOutStorageDetail> deleteDetails, ref Guid lackingId)
        {

            DbContextTransaction updateTransaction = null;
            try
            {
                updateTransaction = _entities.Database.BeginTransaction();
                _entities.ProductInStorageDetail.Detacheds(_entities);
                _entities.ProductOutInStorageDetail.Detacheds(_entities);
                //_entities.Customer.Detacheds(_entities);
                //_entities.CustomerAccountRecord.Detacheds(_entities);

                var custom = _entities.Customer.Find(productOutStorage.CustomerId);
                if (custom == null)
                {
                    updateTransaction.Rollback();
                    return "请选择客户!";
                }

                //初始化基本信息
                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                lackingId = Guid.Empty;
                //订单总金额
                decimal sumPrice = 0;
                //记录已扣库存的主订单，用于更新状态标记为已扣库存
                List<Guid> lockedList = new List<Guid>();
                //记录已扣库存的退货入库单单,用于更新退货入库单状态标记为已扣库存
                List<Guid> lockedReturnList = new List<Guid>();

                //清空历史出库记录，并退回历史出库数量
                foreach (ProductOutStorageDetail detail in deleteDetails)
                {
                    _entities.ProductOutInStorageDetail.Where(a => a.PutOutStorageId == detail.Id).OrderBy(a => a.LasetStock).Load();
                    while (_entities.ProductOutInStorageDetail.Local.Count > 0)
                    {
                        var subtractStock = _entities.ProductOutInStorageDetail.Local[0].SubtractStock;
                        if (subtractStock != null)
                            _entities.Database.ExecuteSqlCommand("UPDATE dbo.ProductInStorageDetail SET LastStock=LastStock+@LastStock WHERE Id=@id", new SqlParameter("LastStock", subtractStock.Value), new SqlParameter("id", _entities.ProductOutInStorageDetail.Local[0].PutInStorageId));
                        _entities.Entry(_entities.ProductOutInStorageDetail.Local[0]).State = EntityState.Deleted;
                    }
                }

                //处理订单
                foreach (ProductOutStorageDetail detail in details)
                {
                    if (detail.ProductId == null || detail.ProductId.Value == Guid.Empty)
                    {
                        updateTransaction.Rollback();
                        return $"开单失败:{detail.ProductCode} 产品Id为空!";
                    }
                    var objectEntity = _entities.Entry(detail);
                    var state = objectEntity.State;
                    if (state == EntityState.Added)
                    {
                        #region Added

                        var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();
                        var sumStock = storageDetails.Sum(a => a.LastStock);
                        if (sumStock < detail.OutStock)
                        {
                            lackingId = detail.ProductId.Value;
                            updateTransaction.Rollback();
                            return $"开单失败:{detail.ProductCode} 库存不足!";
                        }
                        decimal needStock = detail.OutStock;
                        foreach (var inStorageDetail in storageDetails)
                        {
                            //记录出入库对照明细
                            ProductOutInStorageDetail temAddOutInStorageDetail = new ProductOutInStorageDetail()
                            {
                                Id = Guid.NewGuid(),
                                PutInStorageId = inStorageDetail.Id,
                                PutOutStorageId = detail.Id,
                                OriginalStock = inStorageDetail.LastStock,
                                CreateUserId = productOutStorage.CreateUserId,
                                CreateDate = nowDateTime,
                                CreateIp = ip
                            };
                            _entities.ProductOutInStorageDetail.Add(temAddOutInStorageDetail);

                            //记录已扣库存的入库主单或者退货入库主单
                            if (inStorageDetail.PutInId != null)
                            {
//入库单
                                if (inStorageDetail.PutInId.Value != Guid.Empty)
                                {
                                    if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                    {
                                        lockedList.Add(inStorageDetail.PutInId.Value);
                                    }
                                }
                            }
                            else if (inStorageDetail.ReturnInId != null)
                            {
//退货入库单
                                if (inStorageDetail.ReturnInId.Value != Guid.Empty)
                                {
                                    if (!lockedReturnList.Contains(inStorageDetail.ReturnInId.Value))
                                    {
                                        lockedReturnList.Add(inStorageDetail.ReturnInId.Value);
                                    }
                                }
                            }

                            //库存扣除调整
                            if ((inStorageDetail.LastStock >= needStock))
                            {
                                inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                                temAddOutInStorageDetail.SubtractStock = needStock;
                                temAddOutInStorageDetail.LasetStock = inStorageDetail.LastStock;
                                needStock = 0;
                                break;
                            }
                            else
                            {
                                needStock -= inStorageDetail.LastStock;
                                temAddOutInStorageDetail.SubtractStock = inStorageDetail.LastStock;
                                inStorageDetail.LastStock = 0;
                                temAddOutInStorageDetail.LasetStock = 0;
                            }
                        }
                        //判断库存是否足够扣完
                        if (needStock > 0)
                        {
                            lackingId = detail.ProductId.Value;
                            updateTransaction.Rollback();
                            return $"开单失败:{detail.ProductCode} 尝试扣库存失败!";
                        }
                        sumPrice += detail.SumPrice;

                        #endregion
                    }
                    else if (state == EntityState.Modified)
                    {
                        #region Modified

                        //判断
                        decimal needStock = detail.OutStock;
                        ProductOutStorageDetail originalOutDetail = _entities.Database.SqlQuery<ProductOutStorageDetail>("SELECT * FROM dbo.ProductOutStorageDetail WHERE Id=@id", new SqlParameter("id", detail.Id)).FirstOrDefault();
                        if (originalOutDetail == null)
                        {
                            updateTransaction.Rollback();
                            return $"{detail.Id}历史明细不存在，更新失败!";
                        }
                        if (objectEntity.CurrentValues.GetValue<decimal>("OutStock") - originalOutDetail.OutStock > 0)
                        {
                            #region 出库数量调大,继续扣除加大数量库存

                            needStock -= originalOutDetail.OutStock;
                            var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();
                            var sumStock = storageDetails.Sum(a => a.LastStock);
                            if (sumStock < needStock)
                            {
                                lackingId = detail.ProductId.Value;
                                updateTransaction.Rollback();
                                return $"开单失败:{detail.ProductCode} 库存不足!";
                            }
                            foreach (var inStorageDetail in storageDetails)
                            {
                                //记录出入库对照明细
                                ProductOutInStorageDetail temAddOutInStorageDetail = new ProductOutInStorageDetail()
                                {
                                    Id = Guid.NewGuid(),
                                    PutInStorageId = inStorageDetail.Id,
                                    PutOutStorageId = detail.Id,
                                    OriginalStock = inStorageDetail.LastStock,
                                    CreateUserId = productOutStorage.CreateUserId,
                                    CreateDate = nowDateTime,
                                    CreateIp = ip
                                };
                                _entities.ProductOutInStorageDetail.Add(temAddOutInStorageDetail);

                                //记录已扣库存的入库主单或者退货入库主单
                                if (inStorageDetail.PutInId != null)
                                {
//入库单
                                    if (inStorageDetail.PutInId.Value != Guid.Empty)
                                    {
                                        if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                        {
                                            lockedList.Add(inStorageDetail.PutInId.Value);
                                        }
                                    }
                                }
                                else if (inStorageDetail.ReturnInId != null)
                                {
//退货入库单
                                    if (inStorageDetail.ReturnInId.Value != Guid.Empty)
                                    {
                                        if (!lockedReturnList.Contains(inStorageDetail.ReturnInId.Value))
                                        {
                                            lockedReturnList.Add(inStorageDetail.ReturnInId.Value);
                                        }
                                    }
                                }

                                //库存扣除调整
                                if ((inStorageDetail.LastStock >= needStock))
                                {
                                    inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                                    temAddOutInStorageDetail.SubtractStock = needStock;
                                    temAddOutInStorageDetail.LasetStock = inStorageDetail.LastStock;
                                    needStock = 0;
                                    break;
                                }
                                else
                                {
                                    needStock -= inStorageDetail.LastStock;
                                    temAddOutInStorageDetail.SubtractStock = inStorageDetail.LastStock;
                                    inStorageDetail.LastStock = 0;
                                    temAddOutInStorageDetail.LasetStock = 0;
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region 出库数量调小，还原已扣库存

                            //清空出入库明细记录，并还原入库单数量
                            var listOutInDetail = _entities.ProductOutInStorageDetail.Where(a => a.PutOutStorageId == detail.Id).OrderBy(a => a.LasetStock).ToList();
                            foreach (var outInStorageDetail in listOutInDetail)
                            {
                                if (outInStorageDetail.SubtractStock != null)
                                    _entities.Database.ExecuteSqlCommand("UPDATE dbo.ProductInStorageDetail SET LastStock=LastStock+@LastStock WHERE Id=@id", new SqlParameter("LastStock", outInStorageDetail.SubtractStock.Value), new SqlParameter("id", outInStorageDetail.PutInStorageId));
                            }
                            _entities.Database.ExecuteSqlCommand($"DELETE dbo.ProductOutInStorageDetail WHERE PutOutStorageId IN ('{string.Join("','", listOutInDetail.Select(a => a.PutOutStorageId))}')");
                            //继续出库存
                            var storageDetails = _entities.ProductInStorageDetail.Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0).OrderBy(a => a.LastStock).ToList();
                            var sumStock = storageDetails.Sum(a => a.LastStock);
                            if (sumStock < detail.OutStock)
                            {
                                lackingId = detail.ProductId.Value;
                                updateTransaction.Rollback();
                                return $"开单失败:{detail.ProductCode} 库存不足!";
                            }
                            foreach (var inStorageDetail in storageDetails)
                            {
                                //记录出入库对照明细
                                ProductOutInStorageDetail temAddOutInStorageDetail = new ProductOutInStorageDetail()
                                {
                                    Id = Guid.NewGuid(),
                                    PutInStorageId = inStorageDetail.Id,
                                    PutOutStorageId = detail.Id,
                                    OriginalStock = inStorageDetail.LastStock,
                                    CreateUserId = productOutStorage.CreateUserId,
                                    CreateDate = nowDateTime,
                                    CreateIp = ip
                                };
                                _entities.ProductOutInStorageDetail.Add(temAddOutInStorageDetail);

                                //记录已扣库存的入库主单或者退货入库主单
                                if (inStorageDetail.PutInId != null)
                                {
//入库单
                                    if (inStorageDetail.PutInId.Value != Guid.Empty)
                                    {
                                        if (!lockedList.Contains(inStorageDetail.PutInId.Value))
                                        {
                                            lockedList.Add(inStorageDetail.PutInId.Value);
                                        }
                                    }
                                }
                                else if (inStorageDetail.ReturnInId != null)
                                {
//退货入库单
                                    if (inStorageDetail.ReturnInId.Value != Guid.Empty)
                                    {
                                        if (!lockedReturnList.Contains(inStorageDetail.ReturnInId.Value))
                                        {
                                            lockedReturnList.Add(inStorageDetail.ReturnInId.Value);
                                        }
                                    }
                                }

                                //库存扣除调整
                                if ((inStorageDetail.LastStock >= needStock))
                                {
                                    inStorageDetail.LastStock = inStorageDetail.LastStock - needStock;
                                    temAddOutInStorageDetail.SubtractStock = needStock;
                                    temAddOutInStorageDetail.LasetStock = inStorageDetail.LastStock;
                                    needStock = 0;
                                    break;
                                }
                                else
                                {
                                    needStock -= inStorageDetail.LastStock;
                                    temAddOutInStorageDetail.SubtractStock = inStorageDetail.LastStock;
                                    inStorageDetail.LastStock = 0;
                                    temAddOutInStorageDetail.LasetStock = 0;
                                }
                            }

                            #endregion
                        }
                        if (needStock > 0)
                        {
                            lackingId = detail.ProductId.Value;
                            updateTransaction.Rollback();
                            return $"开单失败:{detail.ProductCode} 尝试扣库存失败!";
                        }

                        sumPrice += detail.SumPrice;

                        #endregion
                    }
                    else if (state == EntityState.Deleted)
                    {
                        #region Deleted


                        #endregion
                    }
                    else if (state == EntityState.Unchanged)
                    {
                        sumPrice += detail.SumPrice;
                    }

                }
                //if ((productOutStorage.SumPrice - sumPrice) != 0)
                //{
                //    if (!query($"调整差价为:{productOutStorage.SumPrice - sumPrice},是否继续保持?"))
                //    {
                //        updateTransaction.Rollback();
                //        return "取消保存!";
                //    }
                //}

                //更新入库单标记为已扣库存
                if (lockedList.Any())
                {
                    _entities.Database.ExecuteSqlCommand($"UPDATE dbo.[ProductInStorage] SET StatusFlag=1 WHERE Id IN('{string.Join("','", lockedList)}')");
                }
                if (lockedReturnList.Any())
                {
                    //更新退货入库单标记为已扣库存
                    _entities.Database.ExecuteSqlCommand($"UPDATE dbo.[ProductReturnInStorage] SET StatusFlag=1 WHERE Id IN('{string.Join("','", lockedReturnList)}')");
                }

                //var originalOutStorage = _entities.Database.SqlQuery<ProductOutStorage>("SELECT * FROM dbo.ProductOutStorage WHERE Id=@id", new SqlParameter("id", productOutStorage.Id)).FirstOrDefault();
                //if (originalOutStorage==null)
                //{
                //    updateTransaction.Rollback();
                //    return $"记录不存在,更新失败!";
                //}
                //if ((originalOutStorage.SumPrice - sumPrice) != 0)
                //{
                //    //是否增大
                //    bool increased = sumPrice > originalOutStorage.SumPrice;
                //    //写客户扣款记录
                //    var customerAc = new CustomerAccountRecord()
                //    {
                //        Id = Guid.NewGuid(),
                //        CustomerId = productOutStorage.CustomerId,
                //        TransactionTypeEnum = increased ? TransactionTypeEnum.UpdateOut : TransactionTypeEnum.UpdateIn,//更新支出  1.2 更新收入  2.2 // (short) ((productOutStorage.SumPrice - sumPrice) > 0 ? 2 : 3),
                //        TransactionBefore = custom.Balance,
                //        TransactionBalance = originalOutStorage.SumPrice - sumPrice,
                //        CreateUserId = productOutStorage.CreateUserId,
                //        CreateDate = nowDateTime,
                //        CreateIp = ip
                //    };
                //    _entities.CustomerAccountRecord.Add(customerAc);

                //    //更新客户余额
                //    custom.Balance += (originalOutStorage.SumPrice - sumPrice);
                //    customerAc.TransactionAfter = custom.Balance;


                //    //更新主记录的订单总金额
                //    productOutStorage.SumPrice = sumPrice;
                //}

                //    //更新主记录的订单总金额
                productOutStorage.SumPrice = sumPrice;

                _entities.SaveChanges();
                updateTransaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (updateTransaction != null)
                {
                    updateTransaction.Rollback();
                }

                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }

        public CategoriesOrder<ProductOutStorage,ProductOutStorageDetail> GetUpdateProductOutOrder(Guid productOutId)
        {
            try
            {
                _entities = new ApplicationDbContext();
                _entities.ProductOutStorage.Where(a => a.Id == productOutId).Load();
                _entities.ProductOutStorageDetail.Where(a => a.PutOutId == productOutId).Load();

                return new CategoriesOrder<ProductOutStorage, ProductOutStorageDetail>() {Details = _entities.ProductOutStorageDetail.Local,MasterStorage = _entities.ProductOutStorage.Local.First()};
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    #endregion

    #region 退货入库单

    public class ProductReturnOrderInsertManager
    {
        private ApplicationDbContext _entities;

        public static ProductReturnOrderInsertManager Create()
        {
            return new ProductReturnOrderInsertManager();
        }

        private ProductReturnOrderInsertManager()
        {

        }

        public string InsertProductReturnOrder(ProductReturnInStorage returnInStorage,Action<string> notifyAction)
        {
            if (_entities == null)
            {
                return "保存失败!数据库上下文不可用!";
            }
            DbContextTransaction transaction = null;
            try
            {
                transaction = _entities.Database.BeginTransaction();
                _entities.Account.Detacheds(_entities);
                _entities.AccountRecord.Detacheds(_entities);
                //_entities.Customer.Detacheds(_entities);
                //_entities.CustomerAccountRecord.Detacheds(_entities);


                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                Models.RefundType refundType = (Models.RefundType) Enum.Parse(typeof (Models.RefundType), returnInStorage.PaymentType);

                if (refundType == Models.RefundType.RefundCash)
                {
                    #region 退还现款，本账户金额支出

                    var account = _entities.Account.Find(returnInStorage.PaymentAccountId);
                    if (account == null)
                    {
                        transaction.Rollback();
                        return "选择账户不存在!";
                    }
                    AccountRecord accountRecord = new AccountRecord
                    {
                        Id = Guid.NewGuid(),
                        AccountId = account.Id,
                        TransactionType = Models.TransactionTypeEnum.AddOut,
                        TransactionBefore = account.Balance??0m,
                        TransactionBalance = -(returnInStorage.SumPrice ?? 0m)
                    };
                    account.Balance -= returnInStorage.SumPrice??0m;
                    accountRecord.TransactionAfter = account.Balance??0m;
                    accountRecord.CreateDate = nowDateTime;
                    accountRecord.CreateIp = ip;
                    accountRecord.CreateUserId = returnInStorage.CreateUserId;
                    _entities.AccountRecord.Add(accountRecord);
                    notifyAction($"退还现金 【{returnInStorage.SumPrice ?? 0m}】 元");
                    //修改订单已付
                    returnInStorage.Paid = returnInStorage.SumPrice??0m;

                    #endregion
                }
                //else if (refundType == RefundType.OffsetTransactions)
                //{
                //    #region 冲抵往来，客户账户余额增加

                //    var custom = _entities.Customer.Find(returnInStorage.CustomerId);
                //    if (custom == null)
                //    {
                //        transaction.Rollback();
                //        return "选择客户不存在!";
                //    }
                //    CustomerAccountRecord customerAccountRecord = new CustomerAccountRecord
                //    {
                //        Id = Guid.NewGuid(),
                //        CustomerId = returnInStorage.CustomerId,
                //        TransactionTypeEnum = TransactionTypeEnum.AddIn,
                //        TransactionBefore = custom.Balance,
                //        TransactionBalance = returnInStorage.SumPrice
                //    };
                //    custom.Balance += returnInStorage.SumPrice;
                //    customerAccountRecord.TransactionAfter = custom.Balance;
                //    customerAccountRecord.CreateDate = nowDateTime;
                //    customerAccountRecord.CreateIp = ip;
                //    customerAccountRecord.CreateUserId = returnInStorage.CreateUserId;
                //    _entities.CustomerAccountRecord.Add(customerAccountRecord);

                //    #endregion

                //}else if (refundType == RefundType.Rebate)
                //{
                //    #region 返利，客户账户余额增加，库存都是0

                //    var custom = _entities.Customer.Find(returnInStorage.CustomerId);
                //    if (custom == null)
                //    {
                //        transaction.Rollback();
                //        return "选择客户不存在!";
                //    }
                //    CustomerAccountRecord customerAccountRecord = new CustomerAccountRecord
                //    {
                //        Id = Guid.NewGuid(),
                //        CustomerId = returnInStorage.CustomerId,
                //        TransactionTypeEnum = TransactionTypeEnum.RebateIn,
                //        TransactionBefore = custom.Balance,
                //        TransactionBalance = returnInStorage.SumPrice
                //    };
                //    custom.Balance += returnInStorage.SumPrice;
                //    customerAccountRecord.TransactionAfter = custom.Balance;
                //    customerAccountRecord.CreateDate = nowDateTime;
                //    customerAccountRecord.CreateIp = ip;
                //    customerAccountRecord.CreateUserId = returnInStorage.CreateUserId;
                //    _entities.CustomerAccountRecord.Add(customerAccountRecord);
                //    #endregion
                //}
                //else
                //{
                //    transaction.Rollback();
                //    return "请选择付款类型!";
                //}


                _entities.SaveChanges();
                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }


            return null;
        }

        public CategoriesOrder<ProductReturnInStorage, ProductInStorageDetail> GetInsertProductReturnOrder()
        {

            _entities = new ApplicationDbContext();
            ProductReturnInStorage inserInStorage = new ProductReturnInStorage() {Id = Guid.NewGuid()};
            _entities.ProductReturnInStorage.Add(inserInStorage);
            _entities.ProductReturnInStorage.Where(a => a.Id == inserInStorage.Id).Load();
            _entities.ProductInStorageDetail.Where(a => a.PutInId == inserInStorage.Id).Load();
            return new CategoriesOrder<ProductReturnInStorage, ProductInStorageDetail>()
            {
                Details = _entities.ProductInStorageDetail.Local,
                MasterStorage = inserInStorage
            };
        }
    }

    public class ProductReturnOrderUpdateManager
    {
        private ApplicationDbContext _entities;

        public static ProductReturnOrderUpdateManager Create()
        {
            return new ProductReturnOrderUpdateManager();
        }

        private ProductReturnOrderUpdateManager()
        {
        }

        public CategoriesOrder<ProductReturnInStorage, ProductInStorageDetail> GetUpdateProductReturnOrder(Guid productReturnId)
        {
            try
            {
                _entities = new ApplicationDbContext();
                _entities.ProductReturnInStorage.Where(a => a.Id == productReturnId).Load();
                _entities.ProductInStorageDetail.Where(a => a.ReturnInId == productReturnId).Load();

                return new CategoriesOrder<ProductReturnInStorage, ProductInStorageDetail>() { Details = _entities.ProductInStorageDetail.Local, MasterStorage = _entities.ProductReturnInStorage.Local.First() };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string UpdateProductReturnOrder(ProductReturnInStorage returnInStorage)
        {
            DbContextTransaction transaction = null;
            try
            {
                transaction = _entities.Database.BeginTransaction();
                _entities.Account.Detacheds(_entities);
                _entities.AccountRecord.Detacheds(_entities);
                _entities.Customer.Detacheds(_entities);
                _entities.CustomerAccountRecord.Detacheds(_entities);

                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                Models.RefundType refundType = (Models.RefundType) Enum.Parse(typeof (Models.RefundType), returnInStorage.PaymentType);

                ProductReturnInStorage originaInStorage = _entities.Database.SqlQuery<ProductReturnInStorage>("SELECT * FROM dbo.ProductReturnInStorage WHERE Id=@id", new SqlParameter("id", returnInStorage.Id)).First();

                //退货单金额减少
                var sub = returnInStorage.SumPrice - originaInStorage.SumPrice;
                if (sub.HasValue)
                {
                    decimal subPrice = sub.Value;

                    if (subPrice > 0)
                    {
                        #region 退货现金加大

                        if (refundType == Models.RefundType.RefundCash)
                        {
                            var account = _entities.Account.Find(returnInStorage.PaymentAccountId);
                            if (account == null)
                            {
                                transaction.Rollback();
                                return "选择账户不存在!";
                            }
                            AccountRecord accountRecord = new AccountRecord
                            {
                                Id = Guid.NewGuid(),
                                AccountId = account.Id,
                                TransactionType = BusinessDb.Cor.Models.TransactionTypeEnum.UpdateOut,
                                TransactionBefore = account.Balance,
                                TransactionBalance = -subPrice
                            };
                            account.Balance -= subPrice;
                            accountRecord.TransactionAfter = account.Balance;
                            accountRecord.CreateDate = nowDateTime;
                            accountRecord.CreateIp = ip;
                            accountRecord.CreateUserId = returnInStorage.CreateUserId;
                            _entities.AccountRecord.Add(accountRecord);
                            //notifyAction($"支出差额现金 【{Math.Abs(subPrice)}】 元");
                            returnInStorage.Paid = returnInStorage.SumPrice;
                        }
                        //else if (refundType == RefundType.OffsetTransactions)
                        //{
                        //    var custom = _entities.Customer.Find(returnInStorage.CustomerId);
                        //    if (custom == null)
                        //    {
                        //        transaction.Rollback();
                        //        return "选择客户不存在!";
                        //    }
                        //    CustomerAccountRecord customerAccountRecord = new CustomerAccountRecord
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        CustomerId = returnInStorage.CustomerId,
                        //        TransactionTypeEnum = TransactionTypeEnum.UpdateIn,
                        //        TransactionBefore = custom.Balance,
                        //        TransactionBalance = Math.Abs(subPrice)
                        //    };
                        //    custom.Balance -= subPrice;
                        //    customerAccountRecord.TransactionAfter = custom.Balance;
                        //    customerAccountRecord.CreateDate = nowDateTime;
                        //    customerAccountRecord.CreateIp = ip;
                        //    customerAccountRecord.CreateUserId = returnInStorage.CreateUserId;
                        //    _entities.CustomerAccountRecord.Add(customerAccountRecord);

                        //}else if (refundType == RefundType.Rebate)
                        //{
                        //    var custom = _entities.Customer.Find(returnInStorage.CustomerId);
                        //    if (custom == null)
                        //    {
                        //        transaction.Rollback();
                        //        return "选择客户不存在!";
                        //    }
                        //    CustomerAccountRecord customerAccountRecord = new CustomerAccountRecord
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        CustomerId = returnInStorage.CustomerId,
                        //        TransactionTypeEnum = TransactionTypeEnum.UpdateIn,
                        //        TransactionBefore = custom.Balance,
                        //        TransactionBalance = Math.Abs(subPrice)
                        //    };
                        //    custom.Balance -= subPrice;
                        //    customerAccountRecord.TransactionAfter = custom.Balance;
                        //    customerAccountRecord.CreateDate = nowDateTime;
                        //    customerAccountRecord.CreateIp = ip;
                        //    customerAccountRecord.CreateUserId = returnInStorage.CreateUserId;
                        //    _entities.CustomerAccountRecord.Add(customerAccountRecord);
                        //}
                        //else
                        //{
                        //    throw new NotImplementedException();
                        //}

                        #endregion
                    }
                    else if (subPrice < 0)
                    {
                        #region 退货金额减少

                        if (refundType == Models.RefundType.RefundCash)
                        {
                            //退还现款
                            var account = _entities.Account.Find(returnInStorage.PaymentAccountId);
                            if (account == null)
                            {
                                transaction.Rollback();
                                return "选择账户不存在!";
                            }
                            AccountRecord accountRecord = new AccountRecord
                            {
                                Id = Guid.NewGuid(),
                                AccountId = account.Id,
                                TransactionType = BusinessDb.Cor.Models.TransactionTypeEnum.UpdateIn,
                                TransactionBefore = account.Balance,
                                TransactionBalance = Math.Abs(subPrice)
                            };
                            account.Balance += subPrice;
                            accountRecord.TransactionAfter = account.Balance;
                            accountRecord.CreateDate = nowDateTime;
                            accountRecord.CreateIp = ip;
                            accountRecord.CreateUserId = returnInStorage.CreateUserId;
                            _entities.AccountRecord.Add(accountRecord);
                            //notifyAction($"收回差价金额 【{subPrice}】 元");
                            returnInStorage.Paid = returnInStorage.SumPrice;
                        }
                        //else if (refundType == RefundType.OffsetTransactions)
                        //{//冲抵往来
                        //    var custom = _entities.Customer.Find(returnInStorage.CustomerId);
                        //    if (custom == null)
                        //    {
                        //        transaction.Rollback();
                        //        return "选择客户不存在!";
                        //    }
                        //    CustomerAccountRecord customerAccountRecord = new CustomerAccountRecord
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        CustomerId = returnInStorage.CustomerId,
                        //        TransactionTypeEnum = TransactionTypeEnum.UpdateOut,
                        //        TransactionBefore = custom.Balance,
                        //        TransactionBalance = -subPrice
                        //    };
                        //    custom.Balance -= subPrice;
                        //    customerAccountRecord.TransactionAfter = custom.Balance;
                        //    customerAccountRecord.CreateDate = nowDateTime;
                        //    customerAccountRecord.CreateIp = ip;
                        //    customerAccountRecord.CreateUserId = returnInStorage.CreateUserId;
                        //    _entities.CustomerAccountRecord.Add(customerAccountRecord);
                        //}else if (refundType==RefundType.Rebate)
                        //{//返利
                        //    var custom = _entities.Customer.Find(returnInStorage.CustomerId);
                        //    if (custom == null)
                        //    {
                        //        transaction.Rollback();
                        //        return "选择客户不存在!";
                        //    }
                        //    CustomerAccountRecord customerAccountRecord = new CustomerAccountRecord
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        CustomerId = returnInStorage.CustomerId,
                        //        TransactionTypeEnum = TransactionTypeEnum.UpdateOut,
                        //        TransactionBefore = custom.Balance,
                        //        TransactionBalance = -subPrice
                        //    };
                        //    custom.Balance -= subPrice;
                        //    customerAccountRecord.TransactionAfter = custom.Balance;
                        //    customerAccountRecord.CreateDate = nowDateTime;
                        //    customerAccountRecord.CreateIp = ip;
                        //    customerAccountRecord.CreateUserId = returnInStorage.CreateUserId;
                        //    _entities.CustomerAccountRecord.Add(customerAccountRecord);
                        //}
                        //else
                        //{
                        //    throw new NotImplementedException();
                        //}

                        #endregion
                    }
                }
                else
                {
                    transaction.Rollback();
                    return $"更新差价计数失败!原始总金额:{originaInStorage.SumPrice},新总金额:{returnInStorage.SumPrice}";
                }
                _entities.SaveChanges();
                transaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }


    }
    #endregion

    #region 销售结算单

    public class ProductChargeInsertManager
    {
        private ApplicationDbContext _entities;

        public static ProductChargeInsertManager Create()
        {
            return new ProductChargeInsertManager();
        }
        private ProductChargeInsertManager()
        {
            
        }

        public CategoriesOrder<Charge, ChargeDetails> GetInsertProducrChargeOrder()
        {
            _entities = new ApplicationDbContext();
            Charge charge = new Charge() { Id = Guid.NewGuid() };
            _entities.Charge.Add(charge);
            _entities.Charge.Where(a => a.Id == charge.Id).Load();
            _entities.ChargeDetails.Where(a => a.ChargeId == charge.Id).Load();
            return new CategoriesOrder<Charge, ChargeDetails>()
            {
                Details = _entities.ChargeDetails.Local,
                MasterStorage = charge
            };
        }

        public string InsertProducrChargeOrder(Charge charge, ObservableCollection<ChargeDetails> details,Func<ApplicationDbContext,Customer,decimal,decimal> inputMoney)
        {
            DbContextTransaction transaction = null;
            try
            {
                _entities.ProductReturnInStorage.Detacheds(_entities);
                _entities.ProductOutStorage.Detacheds(_entities);
                _entities.Customer.Detacheds(_entities);
                _entities.CustomerAccountRecord.Detacheds(_entities);
                _entities.Account.Detacheds(_entities);
                _entities.AccountRecord.Detacheds(_entities);

                transaction = _entities.Database.BeginTransaction();

                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                decimal sumMoney = 0;

                //扣除原记录订单金额
                foreach (ChargeDetails chargeDetailse in details)
                {
                    if (chargeDetailse.ThisPrice == null)
                        break;
                    if (chargeDetailse.OrderType == "XT")
                    {
                        //销售退货
                        var returnInStorage = _entities.ProductReturnInStorage.Find(chargeDetailse.RefId);
                        if (returnInStorage == null)
                        {
                            break;
                        }
                        returnInStorage.Paid = returnInStorage.Paid ?? 0m;
                        returnInStorage.Paid += Math.Abs(chargeDetailse.ThisPrice.Value);
                        if (returnInStorage.Paid > (returnInStorage.SumPrice ?? 0m))
                        {
                            transaction.Rollback();
                            return "提交扣除金额后,已付超出订单总金额,请重生成新订单!";
                        }
                        returnInStorage.StatusFlag = returnInStorage.Paid == returnInStorage.SumPrice ? 3 : 2;
                    }
                    else if (chargeDetailse.OrderType == "XK")
                    {
                        //销售开单
                        var outInStorage = _entities.ProductOutStorage.Find(chargeDetailse.RefId);
                        if (outInStorage == null)
                        {
                            break;
                        }
                        outInStorage.Paid = outInStorage.Paid ?? 0m;
                        outInStorage.Paid += Math.Abs(chargeDetailse.ThisPrice ?? 0m);
                        if (outInStorage.Paid > (outInStorage.SumPrice ?? 0m))
                        {
                            transaction.Rollback();
                            return "提交扣除金额后,已付超出订单总金额,请重生成新订单!";
                        }
                        outInStorage.StatusFlag = outInStorage.Paid == outInStorage.SumPrice ? 2 : 1;
                    }
                    sumMoney += chargeDetailse.ThisPrice.Value;
                }
                //扣除账户金额

                //Customer
                var customer = _entities.Customer.Find(charge.CustomerId);
                if (customer == null)
                {
                    transaction.Rollback();
                    return "找不到该客户!";
                }

                //Account
                var account = _entities.Account.Find(charge.PaymentAccountId);
                if (account == null)
                {
                    transaction.Rollback();
                    return "找不到该账户!";
                }

                if (customer.Balance.HasValue && customer.Balance.Value < sumMoney && sumMoney > 0 & inputMoney != null)
                {

                    var rechargeMoney = inputMoney(_entities, customer, sumMoney);
                    if (rechargeMoney <= 0)
                    {
                        transaction.Rollback();
                        return "取消结算!";
                    }
                    //为客户充值
                    CustomerAccountRecord rechargeRecord = new CustomerAccountRecord()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customer.Id,
                        TransactionType = BusinessDb.Cor.Models.TransactionTypeEnum.ChargeIn,
                        TransactionBalance = rechargeMoney,
                        TransactionBefore = customer.Balance ?? 0m,
                        CreateUserId = charge.CreateUser,
                        CreateDate = nowDateTime,
                        CreateIp = ip
                    };
                    customer.Balance = customer.Balance ?? 0m;
                    customer.Balance += rechargeMoney;
                    rechargeRecord.TransactionAfter = customer.Balance ?? 0m;
                    _entities.CustomerAccountRecord.Add(rechargeRecord);
                }

                CustomerAccountRecord customerAccountRecord = new CustomerAccountRecord()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    TransactionType = sumMoney > 0 ? BusinessDb.Cor.Models.TransactionTypeEnum.ChargeOut : BusinessDb.Cor.Models.TransactionTypeEnum.ChargeIn,
                    TransactionBalance = sumMoney,
                    TransactionBefore = customer.Balance ?? 0m,
                    CreateUserId = charge.CreateUser,
                    CreateDate = nowDateTime,
                    CreateIp = ip
                };
                customer.Balance = customer.Balance.HasValue ? customer.Balance : 0m;
                customer.Balance -= sumMoney;
                customerAccountRecord.TransactionAfter = customer.Balance ?? 0m;
                _entities.CustomerAccountRecord.Add(customerAccountRecord);


                AccountRecord accountRecord = new AccountRecord()
                {
                    Id = Guid.NewGuid(),
                    AccountId = charge.PaymentAccountId,
                    TransactionType = sumMoney > 0 ? BusinessDb.Cor.Models.TransactionTypeEnum.ChargeIn : BusinessDb.Cor.Models.TransactionTypeEnum.ChargeOut,
                    TransactionBalance = sumMoney,
                    TransactionBefore = account.Balance ?? 0m,
                    CreateUserId = charge.CreateUser,
                    CreateDate = nowDateTime,
                    CreateIp = ip
                };
                account.Balance = account.Balance.HasValue ? account.Balance : 0m;
                account.Balance += sumMoney;
                accountRecord.TransactionAfter = account.Balance ?? 0m;
                _entities.AccountRecord.Add(accountRecord);

                _entities.SaveChanges();
                transaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }
    }

    public class PorductChargeUpdateManager
    {
        private ApplicationDbContext _entities;
        private PorductChargeUpdateManager()
        {
            
        }

        public static PorductChargeUpdateManager Create()
        {
            return new PorductChargeUpdateManager();
        }

        public CategoriesOrder<Charge, ChargeDetails> GetUpdateProductChargeOrder(Guid chargeId)
        {
            try
            {
                _entities = new ApplicationDbContext();
                _entities.Charge.Where(a => a.Id == chargeId).Load();
                _entities.ChargeDetails.Where(a => a.ChargeId == chargeId).Load();

                return new CategoriesOrder<Charge, ChargeDetails>() { Details = _entities.ChargeDetails.Local, MasterStorage = _entities.Charge.Local.First() };
            }
            catch (Exception)
            {
                return null;
            }

        }

        public string UpdateProductChargeOrder()
        {
            throw new NotImplementedException();
        }

    }
    #endregion

    #region 费用支出
    public class ExpensesInsertManager
    {
        private ApplicationDbContext _entities;

        public static ExpensesInsertManager Create()
        {
            return new ExpensesInsertManager();
        }
        private ExpensesInsertManager()
        {

        }

        public CategoriesOrder<Expenses, ExpensesDetail> GetInsertExpensesOrder()
        {
            _entities = new ApplicationDbContext();
            Expenses expenses = new Expenses() { Id = Guid.NewGuid() };
            _entities.Expenses.Add(expenses);
            _entities.Expenses.Where(a => a.Id == expenses.Id).Load();
            _entities.ExpensesDetail.Where(a => a.ExpensesId == expenses.Id).Load();
            return new CategoriesOrder<Expenses, ExpensesDetail>()
            {
                Details = _entities.ExpensesDetail.Local,
                MasterStorage = expenses
            };
        }

        public string InsertExpensesOrder(Expenses charge, ObservableCollection<ExpensesDetail> details)
        {
            DbContextTransaction transaction = null;
            try
            {
                _entities.Account.Detacheds(_entities);
                _entities.AccountRecord.Detacheds(_entities);

                transaction = _entities.Database.BeginTransaction();

                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                var account = _entities.Account.FirstOrDefault(a => a.Id == charge.PaymentAccountId);
                if (account==null)
                {
                    return "请选择付款账户!";
                }
                AccountRecord accountRecord = new AccountRecord()
                {
                    Id = Guid.NewGuid(),
                    AccountId = charge.PaymentAccountId,
                    TransactionType = BusinessDb.Cor.Models.TransactionTypeEnum.ExpensesOut,
                    TransactionBalance = -charge.SumMoney ?? -0m,
                    TransactionBefore = account.Balance ?? 0m,
                    CreateUserId = charge.CreteUserId,
                    CreateDate = nowDateTime,
                    CreateIp = ip
                };
                account.Balance = account.Balance ?? 0m;
                account.Balance -= charge.SumMoney ?? 0m;
                accountRecord.TransactionAfter = account.Balance ?? 0m;
                _entities.AccountRecord.Add(accountRecord);

                _entities.SaveChanges();
                transaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }
    }

    public class ExpensesUpdateManager
    {
        private ApplicationDbContext _entities;
        public static ExpensesUpdateManager Create()
        {
            return new ExpensesUpdateManager();
        }

        private ExpensesUpdateManager()
        {
            
        }

        public CategoriesOrder<Expenses, ExpensesDetail> GetUpdateExpensesOrder(Guid Id)
        {
            try
            {
                _entities = new ApplicationDbContext();
                _entities.Expenses.Where(a => a.Id == Id).Load();
                _entities.ExpensesDetail.Where(a => a.ExpensesId == Id).Load();

                return new CategoriesOrder<Expenses, ExpensesDetail>() { Details = _entities.ExpensesDetail.Local, MasterStorage = _entities.Expenses.Local.First() };
            }
            catch (Exception)
            {
                return null;
            }

        }

        public string UpdateProductChargeOrder()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region 费用收入
    public class ExpensesInInsertManager
    {
        private ApplicationDbContext _entities;

        public static ExpensesInInsertManager Create()
        {
            return new ExpensesInInsertManager();
        }
        private ExpensesInInsertManager()
        {

        }

        public CategoriesOrder<Expenses, ExpensesDetail> GetInsertExpensesOrder()
        {
            _entities = new ApplicationDbContext();
            Expenses expenses = new Expenses() { Id = Guid.NewGuid() };
            _entities.Expenses.Add(expenses);
            _entities.Expenses.Where(a => a.Id == expenses.Id).Load();
            _entities.ExpensesDetail.Where(a => a.ExpensesId == expenses.Id).Load();
            return new CategoriesOrder<Expenses, ExpensesDetail>()
            {
                Details = _entities.ExpensesDetail.Local,
                MasterStorage = expenses
            };
        }

        public string InsertExpensesInOrder(Expenses charge, ObservableCollection<ExpensesDetail> details)
        {
            DbContextTransaction transaction = null;
            try
            {
                _entities.Account.Detacheds(_entities);
                _entities.AccountRecord.Detacheds(_entities);

                transaction = _entities.Database.BeginTransaction();

                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();
                var account = _entities.Account.FirstOrDefault(a => a.Id == charge.PaymentAccountId);
                if (account == null)
                {
                    return "请选择付款账户!";
                }
                AccountRecord accountRecord = new AccountRecord()
                {
                    Id = Guid.NewGuid(),
                    AccountId = charge.PaymentAccountId,
                    TransactionType = BusinessDb.Cor.Models.TransactionTypeEnum.ExpensesIn,
                    TransactionBalance = charge.SumMoney ?? -0m,
                    TransactionBefore = account.Balance ?? 0m,
                    CreateUserId = charge.CreteUserId,
                    CreateDate = nowDateTime,
                    CreateIp = ip
                };
                account.Balance = account.Balance ?? 0m;
                account.Balance += charge.SumMoney ?? 0m;
                accountRecord.TransactionAfter = account.Balance ?? 0m;
                _entities.AccountRecord.Add(accountRecord);

                _entities.SaveChanges();
                transaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }
    }

    public class ExpensesInUpdateManager
    {
        private ApplicationDbContext _entities;
        public static ExpensesInUpdateManager Create()
        {
            return new ExpensesInUpdateManager();
        }

        private ExpensesInUpdateManager()
        {

        }

        public CategoriesOrder<Expenses, ExpensesDetail> GetUpdateExpensesOrder(Guid Id)
        {
            try
            {
                _entities = new ApplicationDbContext();
                _entities.Expenses.Where(a => a.Id == Id).Load();
                _entities.ExpensesDetail.Where(a => a.ExpensesId == Id).Load();

                return new CategoriesOrder<Expenses, ExpensesDetail>() { Details = _entities.ExpensesDetail.Local, MasterStorage = _entities.Expenses.Local.First() };
            }
            catch (Exception)
            {
                return null;
            }

        }

        public string UpdateProductChargeOrder()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region 工资
    public class WageInsertManager
    {

        public static WageInsertManager Create()
        {
            return new WageInsertManager();
        }
        private WageInsertManager()
        {

        }

        public CategoriesOrder<Wage, WageDetail> GetInsertWageOrder()
        {
            return new CategoriesOrder<Wage, WageDetail>()
            {
                Details = new ObservableCollection<WageDetail>(),
                MasterStorage = new Wage() { Id = Guid.NewGuid() }
            };
        }

        public string InsertWageInOrder(Wage wage, ObservableCollection<WageDetail> details)
        {
            using (var _entities=new ApplicationDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    _entities.Wage.Add(wage);
                    _entities.WageDetail.AddRange(details);
                    transaction = _entities.Database.BeginTransaction();

                    DateTime nowDateTime = DBUnit.GetDbTime();
                    string ip = global::Utilities.Utilities.GetLocalIpAddress();
                    wage.CreateIp = ip;
                    wage.CreateDate = nowDateTime;
                    var account = _entities.Account.FirstOrDefault(a => a.Id == wage.PaymentAccountId);
                    if (account == null)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        return "请选择付款账户!";
                    }
                    AccountRecord accountRecord = new AccountRecord()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = wage.PaymentAccountId,
                        TransactionType = BusinessDb.Cor.Models.TransactionTypeEnum.WageOut,
                        TransactionBalance = wage.SumPrice,
                        TransactionBefore = account.Balance ?? 0m,
                        CreateUserId = wage.CreateUserId,
                        CreateDate = nowDateTime,
                        CreateIp = ip
                    };
                    account.Balance = account.Balance ?? 0m;
                    account.Balance -= wage.SumPrice;
                    accountRecord.TransactionAfter = account.Balance ?? 0m;
                    _entities.AccountRecord.Add(accountRecord);

                    _entities.SaveChanges();
                    transaction.Commit();
                    return null;
                }
                catch (Exception e)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    return e.Message;
                }
                finally
                {
                    _entities.Dispose();
                    GC.Collect();
                }
            }
            
        }
     
    }

    public class WageUpdateManager
    {
        public static WageUpdateManager Create()
        {
            return new WageUpdateManager();
        }

        private WageUpdateManager()
        {

        }

        public CategoriesOrder<Wage, WageDetail> GetUpdateWageOrder(Guid wageId)
        {
            using (var entities = new ApplicationDbContext())
            {
                try
                {
                    var wage = entities.Wage.Find(wageId);
                    if (wage == null)
                    {
                        return new CategoriesOrder<Wage, WageDetail>();
                    }
                    return new CategoriesOrder<Wage, WageDetail>()
                    {
                        Details = entities.WageDetail.Where(a => a.WageId == wage.Id).ToObservableCollection(),
                        MasterStorage = wage
                    };
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    entities.Dispose();
                    GC.Collect();
                }
            }
               

        }
        public string InsertWageAsInOrder(Wage wage, ObservableCollection<WageDetail> details)
        {
            using (var entities=new ApplicationDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    entities.Entry(wage).State = EntityState.Added;
                    foreach (WageDetail detail in details)
                    {
                        entities.Entry(detail).State = EntityState.Added;
                    }

                    transaction = entities.Database.BeginTransaction();

                    DateTime nowDateTime = DBUnit.GetDbTime();
                    string ip = global::Utilities.Utilities.GetLocalIpAddress();
                    wage.CreateIp = ip;
                    wage.CreateDate = nowDateTime;
                    var account = entities.Account.FirstOrDefault(a => a.Id == wage.PaymentAccountId);
                    if (account == null)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        return "请选择付款账户!";
                    }
                    AccountRecord accountRecord = new AccountRecord()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = wage.PaymentAccountId,
                        TransactionType = BusinessDb.Cor.Models.TransactionTypeEnum.WageOut,
                        TransactionBalance = -wage.SumPrice,
                        TransactionBefore = account.Balance ?? 0m,
                        CreateUserId = wage.CreateUserId,
                        CreateDate = nowDateTime,
                        CreateIp = ip
                    };
                    account.Balance = account.Balance ?? 0m;
                    account.Balance -= wage.SumPrice;
                    accountRecord.TransactionAfter = account.Balance ?? 0m;
                    entities.AccountRecord.Add(accountRecord);

                    entities.SaveChanges();
                    transaction.Commit();
                    return null;
                }
                catch (Exception e)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    return e.Message;
                }
                finally
                {
                    GC.Collect();
                }
            }
            
        }
    }
    #endregion
}
