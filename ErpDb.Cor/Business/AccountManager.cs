using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BusinessDb.Cor.EntityModels;
using Utilities;

namespace BusinessDb.Cor.Business
{
    public class AccountManager
    {
        private ApplicationDbContext _entities;
        public static AccountManager Create()
        {
            return new AccountManager();
        }

        private AccountManager()
        {
            _entities = new ApplicationDbContext();
        }

        public ObservableCollection<Account> QueAccounts()
        {
            _entities.Account.Load();
            return _entities.Account.Local;
        }

        public static ObservableCollection<Account> QueryLookUp()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Account.ToObservableCollection();
            }
        }

        public Account Find(Guid id)
        {
            return _entities.Account.Find(id);
        }
    }
}
