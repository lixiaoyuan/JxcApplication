using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using ApplicationDb.Cor.Model;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class RibbonItemManager
    {
        ApplicationDbEntities _entities = new ApplicationDbEntities();
        private RibbonItemManager()
        {
        }

        public static RibbonItemManager Create()
        {
            return new RibbonItemManager();
        }

        /// <summary>
        /// 获取Ribbon菜单结构
        /// </summary>
        /// <param name="menuId">导航菜单Id</param>
        /// <returns></returns>
        public ObservableCollection<Page> GetRibbonStruct(Guid menuId)
        {
            var que = _entities.AuthMenuToolButton.Where(a => a.MenuId == menuId).Join(_entities.AuthToolButton,a => a.ToolButtonId,b => b.Id,(button, toolButton) => toolButton).ToList();
            return que.Where(p => p.Root != null && p.Root.Value).Select(p =>
                new Page(p.Name, p.SortCode, que.Where(g => g.ParentId == p.Id).Select(g =>
                    new Group(g.Name, g.SortCode, que.Where(i => i.ParentId == g.Id).Select(i =>
                        new Item(i.Name, i.LinkType, i.LinkName, i.SortCode)).OrderBy(oi => oi.Sort).ToObservableCollection())).OrderBy(og => og.Sort).ToObservableCollection())).OrderBy(op => op.Sort).ToObservableCollection();
        }

        /// <summary>
        ///【维护】 获取角色菜单按钮，包括选选择和未选择的
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public ObservableCollection<AuthToolButton> GetRoleMenuAndCheckToolButton(Guid roleId,Guid menuId,string systemId)
        {
            return _entities.GetToolButtonAndCheckByMenuIdRoleId(menuId, roleId, systemId, MergeOption.NoTracking).ToObservableCollection();
           
        }

        /// <summary>
        /// 【维护】更新角色菜单按钮。
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <param name="systemId">系统Id</param>
        /// <param name="toolButtonIds">选择按钮集合</param>
        /// <param name="createUserId">创建用户Id</param>
        /// <returns></returns>
        public string UpdateRoleMenuToolButton(Guid roleId, Guid menuId,string systemId,string toolButtonIds,Guid createUserId)
        {
            ObjectParameter opMsg=new ObjectParameter("Msg",typeof(string));

            _entities.UpdateRoleMenuToolButton(roleId, menuId, systemId, toolButtonIds, createUserId, opMsg);
            return opMsg.Value.ToString();
        }
    }
}