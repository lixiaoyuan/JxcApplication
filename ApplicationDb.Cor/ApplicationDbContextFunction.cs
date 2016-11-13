using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor
{
    public static class ApplicationDbContextFunction
    {
        [Obsolete]
        public static ObjectResult<AuthMenu> GetRoleMenuAndCheckByRoleId(this ApplicationDbContext db,Nullable<System.Guid> roleId, string systemId)
        {
            var roleIdParameter = roleId.HasValue ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(System.Guid));

            var systemIdParameter = systemId != null ?
                new SqlParameter("SystemId", systemId) :
                new SqlParameter("SystemId", typeof(string));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthMenu>("dbo.GetRoleMenuAndCheckByRoleId @RoleId,@SystemId", roleIdParameter, systemIdParameter);
        }
        public static ObjectResult<AuthRibbonNode> GetRoleMenuCheck(this ApplicationDbContext db, Nullable<System.Guid> roleId)
        {
            var roleParameter = roleId.HasValue ?
                new SqlParameter("Role", roleId) :
                new SqlParameter("Role", typeof(System.Guid));
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthRibbonNode>("dbo.GetRoleMenuCheck @Role", roleParameter);
        }
        public static ObjectResult<AuthRibbonNode> GetRoleMenuRibbonNodeCheck(this ApplicationDbContext db, Nullable<System.Guid> roleId,Nullable<Guid> menuId)
        {
            var roleIdParameter = roleId.HasValue ?
                new SqlParameter("Role", roleId) :
                new SqlParameter("Role", typeof(System.Guid));
            var menuIdParameter= menuId.HasValue?
                  new SqlParameter("Menu", menuId) :
                new SqlParameter("Menu", typeof(System.Guid));
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthRibbonNode>("dbo.GetRoleMenuRibbonNodeCheck @Role ,@Menu", roleIdParameter,menuIdParameter);
        }

        public static ObjectResult<SystemUser> GetRoleUserCheck(this ApplicationDbContext db, Nullable<System.Guid> roleId)
        {
            var roleIdParameter = roleId.HasValue ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(System.Guid));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SystemUser>("dbo.GetRoleUserCheck @RoleId", roleIdParameter);
        }

        public static int UpdateRoleUsers(this ApplicationDbContext db, string userIds, Nullable<System.Guid> roleId, Nullable<System.Guid> createUserId, SqlParameter msg)
        {
            var userIdsParameter = userIds != null ?
                new SqlParameter("UserIds", userIds) :
                new SqlParameter("UserIds", typeof(string));

            var roleIdParameter = roleId.HasValue ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(System.Guid));

            var createUserIdParameter = createUserId.HasValue ?
                new SqlParameter("CreateUserId", createUserId) :
                new SqlParameter("CreateUserId", typeof(System.Guid));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.UpdateRoleUsers @UserIds,@RoleId,@CreateUserId,@Msg out", userIdsParameter, roleIdParameter, createUserIdParameter, msg);
        }

        public static int UpdateRoleMenu(this ApplicationDbContext db, Nullable<System.Guid> roleId, string menuIds, string systemId, Nullable<System.Guid> createUserId, SqlParameter msg)
        {
            var roleIdParameter = roleId.HasValue ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(Guid));

            var menuIdsParameter = menuIds != null ?
                new SqlParameter("MenuIds", menuIds) :
                new SqlParameter("MenuIds", typeof(string));

            var systemIdParameter = systemId != null ?
                new SqlParameter("SystemId", systemId) :
                new SqlParameter("SystemId", typeof(string));

            var createUserIdParameter = createUserId.HasValue ?
                new SqlParameter("CreateUserId", createUserId) :
                new SqlParameter("CreateUserId", typeof(Guid));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.UpdateRoleMenu @RoleId,@MenuIds,@SystemId,@CreateUserId,@Msg out", roleIdParameter, menuIdsParameter, systemIdParameter, createUserIdParameter, msg);

        }

        public static ObjectResult<AuthToolButton> GetToolButtonAndCheckByMenuIdRoleId(this ApplicationDbContext db, Nullable<System.Guid> menuId, Nullable<System.Guid> roleId, string systemId)
        {
            var menuIdParameter = menuId.HasValue ?
                new SqlParameter("MenuId", menuId) :
                new SqlParameter("MenuId", typeof(System.Guid));

            var roleIdParameter = roleId.HasValue ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(System.Guid));

            var systemIdParameter = systemId != null ?
                new SqlParameter("SystemId", systemId) :
                new SqlParameter("SystemId", typeof(string));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthToolButton>("dbo.GetToolButtonAndCheckByMenuIdRoleId @MenuId,@RoleId,@SystemId", menuIdParameter, roleIdParameter, systemIdParameter);
        }

        //public static ObjectResult<AuthToolButton> GetToolButtonAndCheckByMenuIdRoleId(this ApplicationDbContext db, Nullable<System.Guid> menuId, Nullable<System.Guid> roleId, string systemId, MergeOption mergeOption)
        //{
        //    var menuIdParameter = menuId.HasValue ?
        //        new SqlParameter("MenuId", menuId) :
        //        new SqlParameter("MenuId", typeof(System.Guid));

        //    var roleIdParameter = roleId.HasValue ?
        //        new SqlParameter("RoleId", roleId) :
        //        new SqlParameter("RoleId", typeof(System.Guid));

        //    var systemIdParameter = systemId != null ?
        //        new SqlParameter("SystemId", systemId) :
        //        new SqlParameter("SystemId", typeof(string));

        //    return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthToolButton>("dbo.GetToolButtonAndCheckByMenuIdRoleId @MenuId,@RoleId,@SystemId", mergeOption, menuIdParameter, roleIdParameter, systemIdParameter);
        //}
        [Obsolete]
        public static int UpdateRoleMenuToolButton(this ApplicationDbContext db, Nullable<System.Guid> roleId, Nullable<System.Guid> menuId, string systemId, string toolButtonIds, Nullable<System.Guid> createUserId, SqlParameter msg)
        {
            var roleIdParameter = roleId.HasValue ?
                new SqlParameter("RoleId", roleId) :
                new SqlParameter("RoleId", typeof(System.Guid));

            var menuIdParameter = menuId.HasValue ?
                new SqlParameter("MenuId", menuId) :
                new SqlParameter("MenuId", typeof(System.Guid));

            var systemIdParameter = systemId != null ?
                new SqlParameter("SystemId", systemId) :
                new SqlParameter("SystemId", typeof(string));

            var toolButtonIdsParameter = toolButtonIds != null ?
                new SqlParameter("ToolButtonIds", toolButtonIds) :
                new SqlParameter("ToolButtonIds", typeof(string));

            var createUserIdParameter = createUserId.HasValue ?
                new SqlParameter("CreateUserId", createUserId) :
                new SqlParameter("CreateUserId", typeof(System.Guid));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.UpdateRoleMenuToolButton @RoleId,@MenuId,@SystemId,@ToolButtonIds,@CreateUserId", roleIdParameter, menuIdParameter, systemIdParameter, toolButtonIdsParameter, createUserIdParameter, msg);
        }

        public static void UpdateRoleMenuNew(this ApplicationDbContext db ,Guid roleId,Guid menuId,bool check)
        {
             ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.UpdateRoleMenuNew @RoleId,@MenuId,@Checked", new SqlParameter("RoleId",roleId), new SqlParameter("MenuId",menuId), new SqlParameter("Checked",check));
        }

        public static ObjectResult<AuthToolButton> GetToolButton(this ApplicationDbContext db, Nullable<System.Guid> userId, Nullable<System.Guid> menuId, string systemId)
        {
            var userIdParameter = userId.HasValue ?
                new SqlParameter("UserId", userId) :
                new SqlParameter("UserId", typeof(System.Guid));

            var menuIdParameter = menuId.HasValue ?
                new SqlParameter("MenuId", menuId) :
                new SqlParameter("MenuId", typeof(System.Guid));

            var systemIdParameter = systemId != null ?
                new SqlParameter("SystemId", systemId) :
                new SqlParameter("SystemId", typeof(string));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthToolButton>("dbo.GetToolButton @UserId,@MenuId,@SystemId", userIdParameter, menuIdParameter, systemIdParameter);
        }

        //public static ObjectResult<AuthToolButton> GetToolButton(this ApplicationDbContext db, Nullable<System.Guid> userId, Nullable<System.Guid> menuId, string systemId)
        //{
        //    var userIdParameter = userId.HasValue ?
        //        new SqlParameter("UserId", userId) :
        //        new SqlParameter("UserId", typeof(System.Guid));

        //    var menuIdParameter = menuId.HasValue ?
        //        new SqlParameter("MenuId", menuId) :
        //        new SqlParameter("MenuId", typeof(System.Guid));

        //    var systemIdParameter = systemId != null ?
        //        new SqlParameter("SystemId", systemId) :
        //        new SqlParameter("SystemId", typeof(string));

        //    return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthToolButton>("dbo.GetToolButton @UserId,@MenuId,@SystemId", mergeOption, userIdParameter, menuIdParameter, systemIdParameter);
        //}

        public static ObjectResult<AuthMenu> GetUserTile(this ApplicationDbContext db, Nullable<System.Guid> userId, string systemId)
        {
            var userIdParameter = userId.HasValue ?
                new SqlParameter("UserId", userId) :
                new SqlParameter("UserId", typeof(System.Guid));

            var systemIdParameter = systemId != null ?
                new SqlParameter("SystemId", systemId) :
                new SqlParameter("SystemId", typeof(string));

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthMenu>("dbo.GetUserTile @UserId,@SystemId", userIdParameter, systemIdParameter);
        }

        //public static ObjectResult<AuthMenu> GetUserTile(this ApplicationDbContext db, Nullable<System.Guid> userId, string systemId)
        //{
        //    var userIdParameter = userId.HasValue ?
        //        new SqlParameter("UserId", userId) :
        //        new SqlParameter("UserId", typeof(System.Guid));

        //    var systemIdParameter = systemId != null ?
        //        new SqlParameter("SystemId", systemId) :
        //        new SqlParameter("SystemId", typeof(string));

        //    return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthMenu>("dbo.GetUserTile @UserId,@SystemId",  userIdParameter, systemIdParameter);
        //}

        public static ObjectResult<SystemUser> GetOrganizationUserCheck(this ApplicationDbContext db, Guid organId)
        {
            var organizationIdParameter = new SqlParameter("OrganizationId", organId) ;

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<SystemUser>("dbo.GetOrganizationUserCheck @OrganizationId", organizationIdParameter);
        }

        public static int UpdateOrganizationUser(this ApplicationDbContext db, Guid organId, string userIds,
            SqlParameter outMsg)
        {
            var organizationIdParameter = new SqlParameter("OrganizationId", organId);
            var userIdsParameter=new SqlParameter("userIds", userIds);
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreCommand("dbo.UpdateOrganizationUser @OrganizationId,@userIds,@Msg out", organizationIdParameter, userIdsParameter, outMsg);
        }

        public static ObjectResult<AuthRibbonNode> GetUserRibbon(this ApplicationDbContext db, Guid userId,
            Guid ribbonRootId,string systemId)
        {
            var userIdParameter = new SqlParameter("UserId", userId);
            var ribbonRootIdParameter = new SqlParameter("RibbonRootId", ribbonRootId);
            var systemIdParameter = new SqlParameter("SystemId", systemId);

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthRibbonNode>("dbo.GetUserRibbon @UserId,@RibbonRootId,@SystemId", userIdParameter, ribbonRootIdParameter, systemIdParameter);
        }

        public static ObjectResult<AuthRibbonNode> GetMenuRibbonNode(this ApplicationDbContext db, Guid menuId)
        {
            var menuIdParameter = new SqlParameter("menuId", menuId);

            return ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<AuthRibbonNode>("dbo.GetMenuRibbonNode @menuId", menuIdParameter);
        }
    }
}
