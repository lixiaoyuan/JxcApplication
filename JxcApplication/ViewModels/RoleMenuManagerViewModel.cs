using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.LayoutControl;
using JxcApplication;
using JxcApplication.ViewModels;
using JxcApplication.ViewModels.Inherit;
using Utilities;

namespace JxcApplication.ViewModels
{
    public class RoleMenuManagerViewModel : ViewModelTabItem
    {
        private bool _isTab1;
        private bool _isTab2;
        private AuthRole _lastRole;
        private AuthRibbonNode _lastMenu;
        private readonly MenuManager _menuManager = MenuManager.Create();
        private readonly RoleManager _roleManager = RoleManager.Create();

        public virtual ObservableCollection<AuthRole> RoleListData { get; set; }

        public virtual ObservableCollection<SystemUser> RoleUsersListData { get; set; }

        public virtual ObservableCollection<AuthRibbonNode> RoleMenuListData { get; set; }

        public virtual ObservableCollection<AuthRibbonNode> RoleMenuRibbonNodeListData { get; set; }
        public virtual AuthRibbonNode SelectMenuItem { get; set; }
        #region Command

        public DelegateCommand<GridControl> AssignUsersToRoleCommand { get; set; }
        public DelegateCommand<TreeListControl> AssignMenuToRoleCommand { get; set; }
        [Obsolete]
        public DelegateCommand<GridControl> AssignToolButtonToRoleMenuCommand { get; set; }
        #endregion

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            AssignUsersToRoleCommand = new DelegateCommand<GridControl>(AssignUsersToRole, CanAssignUsersToRole);
            AssignMenuToRoleCommand = new DelegateCommand<TreeListControl>(AssignMenuToRole, CanAssignMenuToRole);
            //AssignToolButtonToRoleMenuCommand = new DelegateCommand<GridControl>(AssignToolButtonToRoleMenu, CanAssignToolButtonToRoleMenu);
            InitRoleListData();
            _isTab1 = true;
        }


        /// <summary>
        /// 初始化角色列表数据
        /// </summary>
        private void InitRoleListData()
        {
            RoleListData = _roleManager.Que();
            //RaisePropertyChanged("RoleListData");
        }

        /// <summary>
        /// 初始化角色用户列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        private void InitRoleUser(Guid roleId)
        {
            RoleUsersListData = _roleManager.RoleUsersCheck(roleId);
        }

        private void InitRoleMenuTreeData(Guid roleId)
        {
            //RoleMenuTreeData = _menuManager.GetRoleMenuCheck(roleId, App.GlobalApp.SystemId);
            RoleMenuListData = _menuManager.GetRoleMenuCheck(roleId);
        }

        private void InitRoleMenuToolButton(Guid roleId, Guid menuId)
        {
            //RoleMenuButtons = _menuManager.GetRoleMenuAndCheckToolButton(menuId, roleId, App.GlobalApp.SystemId);
            RoleMenuRibbonNodeListData = _menuManager.GetRoleMenuRibbonNodeCheck(roleId, menuId);
        }

        /// <summary>
        /// 角色选择发生改变
        /// </summary>
        /// <param name="selectChangedRole"></param>
        public void SelectRoleChanged(AuthRole selectChangedRole)
        {
            _lastRole = selectChangedRole;
            InitRoleUser(selectChangedRole.Id);
            InitRoleMenuTreeData(selectChangedRole.Id);
        }

        /// <summary>
        /// 选择菜单发生改变
        /// </summary>
        /// <param name="args"></param>
        public void SelectMenuChanged(DevExpress.Xpf.Grid.SelectedItemChangedEventArgs args)
        {
            if (args.NewItem == null)
            {
                return;
            }
            _lastMenu = (AuthRibbonNode)args.NewItem;
            if (_lastRole == null)
            {
                return;
            }

            InitRoleMenuToolButton(_lastRole.Id, _lastMenu.Id);
        }

        public void RibbonNodeCheckChanged(TreeListNodeEventArgs e)
        {
            var node = e.Row as AuthRibbonNode;
            if (node == null)
            {
                return;
            }
            _menuManager.UpdateRoleMenuNew(_lastRole.Id,node.Id,node.Checked);
        }

        /// <summary>
        /// 节点双击
        /// </summary>
        public void RibbonNodeRowDoubleClick(RowDoubleClickEventArgs e)
        {
            if (e.HitInfo.InRow)
            {
                var view = e.OriginalSource as TreeListView;
                if (view==null)
                {
                    return;
                }
                var t = view.GetNodeByRowHandle(e.HitInfo.RowHandle).Content as AuthRibbonNode;
                if (t!= null && t.NodeType == RibbonNodeType.BarItem&&t.RibbonNodeRootId!=Guid.Empty)
                {
                    SelectMenuItem = RoleMenuListData.FirstOrDefault(a => a.Id == t.RibbonNodeRootId);
                }
            }
        }
        /// <summary>
        /// 给角色分配成员
        /// </summary>
        /// <param name="opControl"></param>
        private void AssignUsersToRole(GridControl opControl)
        {
            opControl.View.CommitEditing();
            if (_lastRole == null)
            {
                DXMessageBox.Show("请选择角色!");
                return;
            }
            var result = DXMessageBox.Show(String.Format("是否把当前已选择用户分配到【{0}】角色?", _lastRole.Name), "警告:", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }
            string checkedUserIds = string.Join(",", RoleUsersListData.Where(a => { return a.Check != null && a.Check.Value; }).Select(a => a.Id));

            var saveResult = _roleManager.UpdateRoleUsers(_lastRole.Id, checkedUserIds, App.GlobalApp.LoginUser.Id);
            if (!string.IsNullOrWhiteSpace(saveResult))
            {
                NotificationService.CreateCustomNotification(CustomNotificationViewModel.Create(saveResult)).ShowAsync();
                InitRoleUser(_lastRole.Id);
            }
            else
            {
                NotificationService.CreateCustomNotification(CustomNotificationViewModel.Create("保存成功!")).ShowAsync();
            }
        }

        /// <summary>
        /// 给角色分配菜单
        /// </summary>
        /// <param name="opControl"></param>
        private void AssignMenuToRole(TreeListControl opControl)
        {
            //opControl.View.CommitEditing();
            //if (_lastRole == null)
            //{
            //    DXMessageBox.Show("请选择角色!");
            //    return;
            //}
            //var result = DXMessageBox.Show(string.Format("是否把当前已选择的菜单分配到【{0}】角色?", _lastRole.Name), "警告:", MessageBoxButton.OKCancel);
            //if (result != MessageBoxResult.OK)
            //{
            //    return;
            //}
            //string roleMenuIds = string.Join(",", RoleMenuTreeData.Where(a => a.Check != null && a.Check.Value).Select(a=>a.Id));

            //var resultUpMenu = _roleManager.UpdateRoleMenu(_lastRole.Id, roleMenuIds, App.GlobalApp.SystemId, App.GlobalApp.LoginUser.Id);
            //if (string.IsNullOrWhiteSpace(resultUpMenu))
            //{
            //    ShowNotification("保存成功!");
            //}
            //else
            //{
            //    ShowNotification(resultUpMenu, "警告");
            //}
        }

        /// <summary>
        /// 给角色菜单分配按钮
        /// </summary>
        /// <param name="opControl"></param>
        [Obsolete]
        private void AssignToolButtonToRoleMenu(GridControl opControl)
        {
            //opControl.View.CommitEditing();
            //if (_lastRole == null)
            //{
            //    DXMessageBox.Show("请选择角色!");
            //    return;
            //}
            //if (_lastMenu == null)
            //{
            //    DXMessageBox.Show("请选择菜单!");
            //    return;
            //}
            //var result = DXMessageBox.Show(string.Format("是否把当前选择的按钮分配到【{0}】角色的【{1}】菜单?", _lastRole.Name, _lastMenu.DisplayName), "警告:", MessageBoxButton.OKCancel);
            //if (result != MessageBoxResult.OK)
            //{
            //    return;
            //}
            //if (RoleMenuButtons == null)
            //{
            //    DXMessageBox.Show("按钮选择为空！");
            //    return;
            //}
            //IEnumerable<Guid> checkButtonIds = RoleMenuButtons.Where(a => { return a.Check != null && a.Check.Value; }).Select(a => a.Id);
            //string resultUpdate = _menuManager.UpdateRoleMenuToolButton(_lastRole.Id, _lastMenu.Id, checkButtonIds, App.GlobalApp.LoginUser.Id);
            //if (string.IsNullOrWhiteSpace(resultUpdate))
            //{
            //    ShowNotification("保存成功!");
            //}
            //else
            //{
            //    ShowNotification(resultUpdate, "警告");
            //}
        }

        #region CommandState

        private bool CanAssignUsersToRole(GridControl opControl)
        {
            return _isTab1;
        }

        private bool CanAssignMenuToRole(TreeListControl opControl)
        {
            return false;
        }

        [Obsolete]
        private bool CanAssignToolButtonToRoleMenu(GridControl opControl)
        {
            return _isTab2;
        }

        public void SelectionChanged(TabControlSelectionChangedEventArgs args)
        {
            switch (args.NewSelectedIndex)
            {
                case 0:
                {
                    _isTab1 = true;
                    _isTab2 = false;
                    break;
                }
                case 1:
                {
                    _isTab1 = false;
                    _isTab2 = true;
                    break;
                }
            }
            UpdateButtonCommand();
        }

        private void UpdateButtonCommand()
        {
            AssignUsersToRoleCommand.RaiseCanExecuteChanged();
            AssignMenuToRoleCommand.RaiseCanExecuteChanged();
            //AssignToolButtonToRoleMenuCommand.RaiseCanExecuteChanged();
        }

        #endregion


        public RoleMenuManagerViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}