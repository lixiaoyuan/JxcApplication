using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.Business
{
    public class SuppliersManager
    {
        private ApplicationDbContext _entities;
        public static SuppliersManager Create()
        {
            return new SuppliersManager();
        }

        public SuppliersManager()
        {
            _entities=new ApplicationDbContext();
        }

        public ObservableCollection<Suppliers> QueSupplierses()
        {
            _entities.Suppliers.Load();
            return _entities.Suppliers.Local;
        }

        public bool Save()
        {
            return _entities.SaveChanges() > 0;
        }
        public bool UnSave()
        {
            foreach (Suppliers suppliers in _entities.Suppliers)
            {
                var entry = _entities.Entry(suppliers);
                if (entry.State != EntityState.Unchanged && entry.State != EntityState.Detached)
                {
                    return true;
                }
            }
            return false;
        }
        public ObservableCollection<Suppliers> Refresh()
        {
            _entities = new ApplicationDbContext();
            _entities.Suppliers.Load();
            return _entities.Suppliers.Local;
        }

        public void  Remove(Suppliers suppliers)
        {
            _entities.Suppliers.Remove(suppliers);
        }

        public void Remove(IEnumerable<Suppliers> supplierses )
        {
            _entities.Suppliers.RemoveRange(supplierses);
        }

        public string GetMaxCode()
        {
            return _entities.Suppliers.Max(a => a.Code);
        }
    }
}
