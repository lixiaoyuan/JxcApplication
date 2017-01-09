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

        public static ObservableCollection<WorkApproval> QueryLookUp(Guid userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var orguser = db.OrganizationUser.FirstOrDefault(a => a.UserId == userId);
                if (orguser == null)
                {
                    return null;
                }
                return
                    db.WorkApproval.Join(
                        db.OrganizationWorkApproval.Where(a => a.OrganizationId == orguser.OrganizationId), (a) => a.Id,
                        b => b.WorkApprovalId, (a, b) => a).ToObservableCollection();
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

        /// <summary>
        /// 获取审批对应可视的部门组织
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Organization> WorkApprovalOrganization(Guid WorkApprovalId)
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                return db.GetOrganizationWorkApprovalCheck(WorkApprovalId).ToObservableCollection();
            }
        }
        /// <summary>
        /// 更新组织审批可视
        /// </summary>
        public static void UpdateOrganizationWorkApproval(Guid orgId, Guid workAppId, bool isCheck)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.UpdateOrganizationWorkApproval(orgId, workAppId, isCheck);
            }
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