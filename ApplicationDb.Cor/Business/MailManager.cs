using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.EntityModels;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class MailManager
    {
        public static IEnumerable<MailOrder> GetInBoxItems(Guid userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.MailOrders.Where(a => a.ToUser == userId && a.IsDaft == false && a.IsDelete == false).ToList();
            }
        }
        public static IEnumerable<MailOrder> GetDelBoxItems(Guid userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.MailOrders.Where(a => a.ToUser == userId && a.IsDaft == false && a.IsDelete).ToList();
            }
        }
        public static bool Update(MailOrder mailOder)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(mailOder).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public static bool Delete(MailOrder mailOrder)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(mailOrder).State = EntityState.Deleted;
                return db.SaveChanges() > 0;
            }
        }
    }
}
