using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    public class RoleMenuManagerViewModel : ViewModelTabItem
    {
        private readonly MenuManager _menuManager = MenuManager.Create();
        private readonly RoleManager _roleManager = RoleManager.Create();
        private bool _isTab1;
        private bool _isTab2;
        //private AuthRibbonNode _lastMenu;
        private AuthRole _lastRole;
        private List<Guid> _prevs;


        public RoleMenuManagerViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }

        public virtual ObservableCollection<AuthRole> RoleListData { get; set; }

        public virtual ObservableCollection<SystemUser> RoleUsersListData { get; set; }

        public virtual ObservableCollection<AuthRibbonNode> MenuListData { get; set; }

        public virtual ObservableCollection<AuthRibbonNode> RoleMenuRibbonNodeListData { get; set; }
        public virtual AuthRibbonNode SelectMenuItem { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            AssignUsersToRoleCommand = new DelegateCommand<GridControl>(AssignUsersToRole, CanAssignUsersToRole);
            AssignMenuToRoleCommand = new DelegateCommand<TreeListControl>(AssignMenuToRole, CanAssignMenuToRole);
            PrevNavigationCommand = new DelegateCommand(PrevNavigation, CanPrevNavigation);
            TabSelectionChangedCommand = new DelegateCommand<TabControlSelectionChangedEventArgs>(TabSelectionChanged);
            RibbonNodeCheckChangedCommand = new DelegateCommand<TreeListNodeEventArgs>(RibbonNodeCheckChanged);
            RibbonNodeRowDoubleClickCommand = new DelegateCommand<RowDoubleClickEventArgs>(RibbonNodeRowDoubleClick);
            SelectRoleChangedCommand = new DelegateCommand<AuthRole>(SelectRoleChanged);


            InitRoleListData();
            _isTab1 = true;
            MenuListData = _menuManager.GetMenuList();
        }


        /// <summary>
        ///     初始化角色列表数据
        /// </summary>
        private void InitRoleListData()
        {
            RoleListData = _roleManager.Que();
            //RaisePropertyChanged("RoleListData");
        }

        /// <summary>
        ///     更新角色用户列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        private void UpdateRoleUser(Guid roleId)
        {
            RoleUsersListData = _roleManager.RoleUsersCheck(roleId);
        }

        /// <summary>
        ///     更新角色节点列表
        /// </summary>
        private void UpdateRoleRibbonNodeList(Guid roleId, Guid menuId, bool recording = true)
        {
            if (recording)
            {
                if (_prevs == null)
                    _prevs = new List<Guid>();
                if (_prevs.Count == 0 || _prevs.Last() != menuId)
                    _prevs.Add(menuId);
            }
            RoleMenuRibbonNodeListData = _menuManager.GetRoleMenuRibbonNodeCheck(roleId, menuId);
        }

        /// <summary>
        ///     角色选择发生改变
        /// </summary>
        /// <param name="selectChangedRole"></param>
        private void SelectRoleChanged(AuthRole selectChangedRole)
        {
            _lastRole = selectChangedRole;
            if (_isTab1)
                UpdateRoleUser(selectChangedRole.Id);
            if (_isTab2)
                if (MenuListData != null && MenuListData.Count > 0)
                {
                    ClearNavigation();
                    UpdateRoleRibbonNodeList(_lastRole.Id, MenuListData.First().Id);
                }
        }

        /// <summary>
        ///     行节点复选框发生改变
        /// </summary>
        /// <param name="e"></param>
        private void RibbonNodeCheckChanged(TreeListNodeEventArgs e)
        {
            var node = e.Row as AuthRibbonNode;
            if (node == null)
                return;
            _menuManager.UpdateRoleMenuNew(_lastRole.Id, node.Id, node.Checked);
        }

        /// <summary>
        ///     节点双击
        /// </summary>
        private void RibbonNodeRowDoubleClick(RowDoubleClickEventArgs e)
        {
            if (e.HitInfo.InRow)
            {
                var view = e.OriginalSource as TreeListView;
                if (view == null)
                    return;
                var t = view.GetNodeByRowHandle(e.HitInfo.RowHandle).Content as AuthRibbonNode;
                if (t != null && t.NodeType == RibbonNodeType.BarItem && t.RibbonNodeRootId != Guid.Empty &&
                    t.RibbonNodeRootId.HasValue)
                    UpdateRoleRibbonNodeList(_lastRole.Id, t.RibbonNodeRootId.Value);
            }
        }

        /// <summary>
        ///     给角色分配成员
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
            var result = DXMessageBox.Show(string.Format("是否把当前已选择用户分配到【{0}】角色?", _lastRole.Name), "警告:",
                MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
                return;
            var checkedUserIds = string.Join(",",
                RoleUsersListData.Where(a => { return a.Check != null && a.Check.Value; }).Select(a => a.Id));

            var saveResult = _roleManager.UpdateRoleUsers(_lastRole.Id, checkedUserIds, App.GlobalApp.LoginUser.Id);
            if (!string.IsNullOrWhiteSpace(saveResult))
            {
                NotificationService.CreateCustomNotification(CustomNotificationViewModel.Create(saveResult)).ShowAsync();
                UpdateRoleUser(_lastRole.Id);
            }
            else
            {
                NotificationService.CreateCustomNotification(CustomNotificationViewModel.Create("保存成功!")).ShowAsync();
            }
        }

        /// <summary>
        ///     给角色分配菜单
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
        ///     菜单选择向后导航
        /// </summary>
        private void PrevNavigation()
        {
            Guid prevId;
            if (_prevs != null && _prevs.Count > 1)
            {
                prevId = _prevs[_prevs.Count - 2];
                _prevs.RemoveAt(_prevs.Count - 1);
                UpdateRoleRibbonNodeList(_lastRole.Id, prevId, false);
            }
        }

        /// <summary>
        ///     Tab标签页选择发生改变
        /// </summary>
        private void TabSelectionChanged(TabControlSelectionChangedEventArgs args)
        {
            ClearNavigation();
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
                    if (_lastRole != null && MenuListData != null && MenuListData.Count > 0)
                        UpdateRoleRibbonNodeList(_lastRole.Id, MenuListData.First().Id);
                    break;
                }
            }
            UpdateButtonCommand();
        }

        private void ClearNavigation()
        {
            if (_prevs != null)
                _prevs.Clear();
        }

        #region Command

        public DelegateCommand<GridControl> AssignUsersToRoleCommand { get; set; }
        public DelegateCommand<TreeListControl> AssignMenuToRoleCommand { get; set; }

        [Obsolete]
        public DelegateCommand<GridControl> AssignToolButtonToRoleMenuCommand { get; set; }

        public DelegateCommand PrevNavigationCommand { get; set; }
        public DelegateCommand<TabControlSelectionChangedEventArgs> TabSelectionChangedCommand { get; set; }
        public DelegateCommand<TreeListNodeEventArgs> RibbonNodeCheckChangedCommand { get; set; }
        public DelegateCommand<RowDoubleClickEventArgs> RibbonNodeRowDoubleClickCommand { get; set; }
        public DelegateCommand<AuthRole> SelectRoleChangedCommand { get; set; }

        #endregion

        #region CommandState

        private bool CanAssignUsersToRole(GridControl opControl)
        {
            return _isTab1;
        }

        private bool CanAssignMenuToRole(TreeListControl opControl)
        {
            return false;
        }

        private bool CanPrevNavigation()
        {
            return _prevs != null && _prevs.Count > 1;
        }

        private void UpdateButtonCommand()
        {
            AssignUsersToRoleCommand.RaiseCanExecuteChanged();
            AssignMenuToRoleCommand.RaiseCanExecuteChanged();
            //AssignToolButtonToRoleMenuCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}