using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using BusinessDb.Cor.EntityModels;
using Utilities;

namespace BusinessDb.Cor.Business
{
    public class ProductManager
    {
        private ApplicationDbContext _entities;
        private ApplicationDbContext _entitiesCache;
        public static ProductManager Create()
        {
            return new ProductManager();
        }

        public ProductManager()
        {
            _entities = new ApplicationDbContext();
        }


        public static ObservableCollection<Product> QueryByStore(Guid storeId)
        {
            ApplicationDbContext application=new ApplicationDbContext();
            return application.Product.Where(a => a.Enable.Value).Join(application.Store.Where(a => a.Id == storeId), a => a.ProductType, b => b.ProductType, ((product, store) => product)).AsNoTracking().ToObservableCollection();
        }

        #region CDM

        public ObservableCollection<Product> QueryByProductType(string productType, bool? filterEnable = null)
        {
            _entitiesCache = new ApplicationDbContext();
            if (filterEnable == null)
            {//过滤已启用
                _entitiesCache.Product.Where(a => a.ProductType == productType).Load();
            }
            else
            {
                _entitiesCache.Product.Where(a => a.ProductType == productType && a.Enable == filterEnable).Load();
            }
            return _entitiesCache.Product.Local;
        }
        public ObservableCollection<Product> QueryByProductType(bool filterEnable, params string[] productType)
        {
            _entitiesCache = new ApplicationDbContext();
            _entitiesCache.Product.Where(a => a.Enable == filterEnable && productType.Contains(a.ProductType)).OrderBy(a => a.ProductType).Load();
            return _entitiesCache.Product.Local;
        }

        public void Remove(IEnumerable<Product> products)
        {
            if (_entitiesCache != null) _entitiesCache.Product.RemoveRange(products);
        }

        public bool Save()
        {
            return _entitiesCache != null && _entitiesCache.SaveChanges() > 0;
        }

        #endregion


        /// <summary>
        /// 获取最大的产品编码
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode(string productType)
        {
            return _entities.Product.Where(a => a.ProductType == productType).Max(a => a.Code);
        }

        /// <summary>
        /// 获取产品最新信息,用于产品选择填充的数据
        /// </summary>
        public Product GetProductNewInfo(Guid productGuid)
        {
            return _entities.GetProductNewInfo(productGuid, MergeOption.NoTracking).FirstOrDefault();
        }
        /// <summary>
        /// 销售开单，检查锁定库存，锁定数量不能销售
        /// </summary>
        /// <returns></returns>
        public bool CheckLockAmount( Guid storageId, Guid productId, decimal amount,
            out string msg)
        {
            return _entities.CheckLockAmount(storageId, productId, amount,out msg);
        }
    }
}
