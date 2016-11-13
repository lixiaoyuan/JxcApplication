using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BusinessDb.Cor.EntityModels;
using Utilities;

namespace BusinessDb.Cor.Business
{
    public class StoreManager
    {
        private ApplicationDbContext _entities;
        public static StoreManager Create()
        {
            return new StoreManager();
        }

        public StoreManager()
        {
            _entities=new ApplicationDbContext();
        }

        public ObservableCollection<Store> QueStores()
        {
            _entities.Store.Load();
            return _entities.Store.Local;
        }

        /// <summary>
        /// 查询销售产品仓库
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Store> QueSellStores()
        {
            return
                _entities.Store.Join(_entities.ProductType.Where(a => a.IsSell), a => a.ProductType, b => b.Code,
                    ((store, type) => store)).ToObservableCollection();
        } 
    }
}
