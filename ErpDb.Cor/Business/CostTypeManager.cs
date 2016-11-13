using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.Business
{
    public class CostTypeManager
    {
        private ApplicationDbContext _applicationDb;
        private CostTypeManager()
        {
            _applicationDb = new ApplicationDbContext();
        }

        public static CostTypeManager Create()
        {
            return new CostTypeManager();
        }

        public ObservableCollection<CostType> QueryCostTypes()
        {
            _applicationDb.CostType.Where(a => a.Enable.HasValue && a.Enable.Value).Load();
            return _applicationDb.CostType.Local;
        }

        public bool Save()
        {
            return _applicationDb.SaveChanges() > 0;
        }
    }
}
