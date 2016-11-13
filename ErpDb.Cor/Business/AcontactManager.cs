using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.Business
{
    /// <summary>
    /// 联系人管理
    /// </summary>
    public class AcontactManager
    {
        private ApplicationDbContext _entities;

        public static AcontactManager Create()
        {
            return new AcontactManager();
        }

        public AcontactManager()
        {
            _entities = new ApplicationDbContext();
        }

        public Acontact Find(Guid id)
        {
            return _entities.Acontact.Find(id);
        }

        public ObservableCollection<Acontact> QueAcontacts()
        {
            _entities.Acontact.Load();
            return _entities.Acontact.Local;
        }

        public bool Save()
        {
            return _entities.SaveChanges() > 0;
        }
        public bool UnSave()
        {
            foreach (Acontact acontact in _entities.Acontact)
            {
                var entry = _entities.Entry(acontact);
                if (entry.State != EntityState.Unchanged && entry.State != EntityState.Detached)
                {
                    return true;
                }
            }
            return false;
        }
        public ObservableCollection<Acontact> Refresh()
        {
            _entities = new ApplicationDbContext();
            _entities.Acontact.Load();
            return _entities.Acontact.Local;
        }

        public void Remove(Acontact acontact)
        {
            _entities.Acontact.Remove(acontact);
        }

        public void Remove(IEnumerable<Acontact> acontacts)
        {
            _entities.Acontact.RemoveRange(acontacts);
        }

        public string GetMaxCode()
        {
            return _entities.Acontact.Max(a => a.Code);
        }
    }
}
