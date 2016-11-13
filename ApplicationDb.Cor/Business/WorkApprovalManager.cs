using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using ApplicationDb.Cor.EntityModels;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class WorkApprovalManager
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private static readonly WorkApprovalManager _instanceManager = new WorkApprovalManager();

        public static WorkApprovalManager Instance
        {
            get { return _instanceManager; }
        }

        public static WorkApprovalManager Create()
        {
            return new WorkApprovalManager();
        }

        public static ObservableCollection<WorkApproval> QueryLookUp()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.WorkApproval.Where(a => a.Enable).ToObservableCollection();
            }
        }

        /// <summary>
        ///     审批用户
        /// </summary>
        /// <param name="workApprovalId"></param>
        /// <returns></returns>
        public static SystemUser[] ApprovalUsers(Guid workApprovalId)
        {
            using (var db = new ApplicationDbContext())
            {
                return
                    db.WorkApprovalUser.Where(a => a.WorkApprovalId == workApprovalId)
                        .OrderBy(a => a.Sort)
                        .Join(db.SystemUser, a => a.UserId, b => b.Id, (appproval, systemUser) => systemUser)
                        .ToArray();
            }
        }

        public static WorkApproval Find(Guid id)
        {
            return Instance.db.WorkApproval.Find(id);
        }

        public ObservableCollection<WorkApprovalCopyUser> CopyUsers(WorkApproval workApproval)
        {
            return
                db.WorkApprovalCopyUser.Where(a => a.WorkApprovalId == workApproval.Id)
                    .OrderBy(a => a.Sort)
                    .ToObservableCollection();
        }

        public ObservableCollection<WorkApprovalUser> ApprovalUsers(WorkApproval workApproval)
        {
            return
                db.WorkApprovalUser.Where(a => a.WorkApprovalId == workApproval.Id)
                    .OrderBy(a => a.Sort)
                    .ToObservableCollection();
        }

        public string AddApprovalUser(WorkApprovalUser workApprovalUser)
        {
            try
            {
                db.WorkApprovalUser.Add(workApprovalUser);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string AddApprovalCopyUser(WorkApprovalCopyUser workApprovalCopyUser)
        {
            try
            {
                db.WorkApprovalCopyUser.Add(workApprovalCopyUser);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Delete<T>(T copyUser) where T : class
        {
            try
            {
                db.Entry(copyUser).State = EntityState.Deleted;
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string Modifty<T>(T copyUser) where T : class
        {
            try
            {
                db.Entry(copyUser).State = EntityState.Modified;
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string SaveChanged()
        {
            try
            {
                db.SaveChanges();
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}