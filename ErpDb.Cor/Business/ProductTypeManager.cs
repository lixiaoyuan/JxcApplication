using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;
using Utilities;

namespace BusinessDb.Cor.Business
{
    public class ProductTypeManager
    {
        private ApplicationDbContext _entities;

        public static ProductTypeManager Create()
        {
            return new ProductTypeManager();
        }

        public ProductTypeManager()
        {
            _entities = new ApplicationDbContext();
        }

        public ObservableCollection<ProductType> QueryProductType()
        {
            _entities.ProductType.Load();
            return _entities.ProductType.Local;
        }

        public static ObservableCollection<ProductAsType> QueryProductAsTypesLookUp()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.ProductAsType.OrderBy(a=>a.Sort).AsNoTracking().ToObservableCollection();
            }
        }
    }
}
