using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class RoleManager
    {
        ApplicationDbContext _applicationDbEntities=new ApplicationDbContext();
        public static RoleManager Create()
        {
            return new RoleManager();
        }

        public RoleManager()
        {
            
        }

        public ObservableCollection<AuthRole> Que()
        {
            return _applicationDbEntities.AuthRole.Where(a => a.Enable != null && a.Enable.Value).ToObservableCollection();
        }

        public bool Add(AuthRole role)
        {
            _applicationDbEntities.AuthRole.Add(role);
            return _applicationDbEntities.SaveChanges() > 0;
        }

        public bool Update(AuthRole role)
        {
            _applicationDbEntities.Entry(role).State=EntityState.Modified;
            return _applicationDbEntities.SaveChanges() > 0;
        }


        public void Undo(AuthRole role)
        {
            var entry = _applicationDbEntities.Entry(role);
            entry.CurrentValues.SetValues(_applicationDbEntities.Entry(role).OriginalValues);
            entry.State = EntityState.Unchanged;

        }

        public bool Delete(AuthRole role)
        {
            _applicationDbEntities.Entry(role).State=EntityState.Deleted;
            return _applicationDbEntities.SaveChanges() > 0;
        }


        public ObservableCollection<SystemUser> RoleUsersCheck(Guid roleId)
        {
            return _applicationDbEntities.GetRoleUserCheck(roleId).ToObservableCollection();
            //return _applicationDbEntities.SystemUser.Join(_applicationDbEntities.AuthUserRole.Where(ur => ur.RoleId == roleId), u => u.Id, r => r.UserId, ((user, role) => user)).ToObservableCollection();
        }

        /// <summary>
        /// 更新角色用户
        /// </summary>
        /// <param name="roleGuid">角色Id</param>
        /// <param name="userIds">分配用户Id集合，用逗号分隔。(id1,id2,id3,idd4,)</param>
        /// <param name="cretaUserId">创建用户id</param>
        /// <returns>null或空成功，否则失败</returns>
        public string UpdateRoleUsers(Guid roleGuid, string userIds,Guid cretaUserId)
        {
            SqlParameter opMsg = new SqlParameter("Msg", SqlDbType.VarChar,200);
            opMsg.Direction = ParameterDirection.Output;
            _applicationDbEntities.UpdateRoleUsers(userIds, roleGuid, cretaUserId, opMsg);
            return opMsg.Value.ToString();
        }

        /// <summary>
        /// 更新角色菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuIds">菜单Id集合，用逗号分隔。</param>
        /// <param name="systemid">系统Id</param>
        /// <param name="createUserId">创建用户Id</param>
        /// <returns>null或空成功，否则失败</returns>
        public string UpdateRoleMenu(Guid roleId,string menuIds,string systemid,Guid createUserId)
        {
            SqlParameter opMsg = new SqlParameter("Msg", SqlDbType.VarChar,200);
            opMsg.Direction = ParameterDirection.Output;
            _applicationDbEntities.UpdateRoleMenu(roleId, menuIds, systemid, createUserId, opMsg);
            return opMsg.Value.ToString();
        }


    }
}
