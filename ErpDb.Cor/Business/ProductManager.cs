using BusinessDb.Cor.EntityModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text.RegularExpressions;
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
            ApplicationDbContext application = new ApplicationDbContext();
            // 成品仓库2也需要跟成品显示出来
            if (storeId.Equals(Guid.Parse("688D2340-1413-4467-93AA-BCDA21FC64BC")))
            {
                storeId = Guid.Parse("D66C26BD-3BEB-4242-8346-0A4980BA6FC7");
            }

            var query = from p in application.Product.Where(a => a.Enable.Value)
                        from s in application.Store.Where(a => a.Id == storeId)
                        where s.ProductType.StartsWith(p.ProductType)
                        select p;
            return query.AsNoTracking().ToObservableCollection();
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

            int maxCode = 0;
            string code = null;
            foreach (var p in _entities.Product.Where(a => a.ProductType == productType).ToList())
            {
                if (int.TryParse(Regex.Replace(p.Code, @"^[A-Za-z]+", string.Empty), out int f))
                {
                    if (f > maxCode)
                    {
                        maxCode = f;
                        code = p.Code;
                    }
                }
            }

            return code;
        }

        /// <summary>
        /// 获取产品最新信息,用于产品选择填充的数据
        /// </summary>
        public Product GetProductNewInfo(Guid productGuid, Guid storeId)
        {
            return _entities.GetProductNewInfo(productGuid, storeId, MergeOption.NoTracking).FirstOrDefault();
        }
        /// <summary>
        /// 销售开单，检查锁定库存，锁定数量不能销售
        /// </summary>
        /// <returns></returns>
        public bool CheckLockAmount(Guid storageId, Guid productId, decimal amount,
            out string msg)
        {
            return _entities.CheckLockAmount(storageId, productId, amount, out msg);
        }
    }
}
