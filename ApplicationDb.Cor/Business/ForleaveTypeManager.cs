using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class ForleaveTypeManager
    {
        public static ObservableCollection<ForleaveType> QueryLookup()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.ForleaveType.Where(a => a.Enable).OrderBy(a=>a.Sort).AsNoTracking().ToObservableCollection();
            }
        }
    }
}
