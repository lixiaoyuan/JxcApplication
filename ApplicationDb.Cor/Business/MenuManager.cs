using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class MenuManager
    {
        private readonly ApplicationDbContext _applicationDbEntities = new ApplicationDbContext();

        private MenuManager()
        {
        }

        public static MenuManager Create()
        {
            return new MenuManager();
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="systemId">系统ID</param>
        /// <returns></returns>
        public IEnumerable<AuthMenu> GetUserTile(Guid userId, string systemId)
        {
                return _applicationDbEntities.GetUserTile(userId, systemId);
        }

        /// <summary>
        /// 获取菜单打开窗口按钮
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AuthToolButton> GetLayoutButtons(Guid userId,Guid menuId,string systemId)
        {
            return _applicationDbEntities.GetToolButton(userId, menuId, systemId);
        } 

        /// <summary>
        /// 【维护】获取角色菜单，包含未选中菜单。
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="systemId">系统Id</param>
        /// <returns></returns>
        [Obsolete]
        public ObservableCollection<AuthMenu> GetRoleMenuCheck(Guid roleId, string systemId)
        {
            return _applicationDbEntities.GetRoleMenuAndCheckByRoleId(roleId, systemId).ToObservableCollection();
            //throw new NotImplementedException();
        }
        /// <summary>
        ///  获取菜单列表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<AuthRibbonNode> GetMenuList()
        {
            return _applicationDbEntities.AuthRibbonNode.Where(a => a.NodeType == 0).OrderBy(a=>a.Sort).AsNoTracking().ToObservableCollection();
        }

        public ObservableCollection<AuthRibbonNode> GetRoleMenuCheck(Guid roleId)
        {
            return _applicationDbEntities.GetRoleMenuCheck(roleId).ToObservableCollection();
        }

        public ObservableCollection<AuthRibbonNode> GetRoleMenuRibbonNodeCheck(Guid roleId, Guid menuId)
        {
            return _applicationDbEntities.GetRoleMenuRibbonNodeCheck(roleId, menuId).ToObservableCollection();
        }

        /// <summary>
        /// 【维护】获取角色菜单按钮，包含未选中按钮。
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <param name="roleId">角色Id</param>
        /// <param name="systemId">系统Id</param>
        /// <returns></returns>
        [Obsolete]
        public ObservableCollection<AuthToolButton> GetRoleMenuAndCheckToolButton(Guid menuId,Guid roleId,string systemId)
        {
            return _applicationDbEntities.GetToolButtonAndCheckByMenuIdRoleId(menuId, roleId, systemId).ToObservableCollection();
        }

        /// <summary>
        /// 更新角色菜单按钮
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        /// <param name="checkButtonIds"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        [Obsolete]
        public string UpdateRoleMenuToolButton(Guid roleId, Guid menuId, IEnumerable<Guid> checkButtonIds, Guid createUserId)
        {

            try
            {
                _applicationDbEntities.AuthMenuToolButton.RemoveRange(_applicationDbEntities.AuthMenuToolButton.Where(a => a.RoleId == roleId && a.MenuId == menuId));
                foreach (Guid buttonId in checkButtonIds)
                {
                    _applicationDbEntities.AuthMenuToolButton.Add(new AuthMenuToolButton()
                    {
                        Id = Guid.NewGuid(),
                        RoleId = roleId,
                        MenuId = menuId,
                        ToolButtonId = buttonId,
                        CreateUserId = createUserId
                    });
                }
                _applicationDbEntities.SaveChanges();
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        public void UpdateRoleMenuNew(Guid roleId, Guid menuId, bool check)
        {
            _applicationDbEntities.UpdateRoleMenuNew(roleId,menuId,check);
        }
    }
}