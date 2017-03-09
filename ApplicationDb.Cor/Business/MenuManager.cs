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

        public void UpdateRoleMenuNew(Guid roleId, Guid menuId, bool check)
        {
            _applicationDbEntities.UpdateRoleMenuNew(roleId,menuId,check);
        }
    }
}