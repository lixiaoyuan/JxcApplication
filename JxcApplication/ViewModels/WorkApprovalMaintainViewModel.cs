using System;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;
using DevExpress.Xpf.Grid.TreeList;
using JxcApplication.Control;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    [POCOViewModel]
    public class WorkApprovalMaintainViewModel : ViewModelTabItem
    {
        private bool _addWorkApprovalCopyUser;
        private bool _addWorkApprovalUser;
        private bool _deleteWorkApprovalCopyUser;
        private bool _deleteWorkApprovalUser;
        private bool _lisenItemChanged;
        private WorkApproval _currentWorkApproval;
        private readonly WorkApprovalManager _workApprovalManager;

        public WorkApprovalMaintainViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
            _workApprovalManager = WorkApprovalManager.Create();
            LoadedCommand = new DelegateCommand(Loaded);
            WorkApprovalItemsSourceChangedCommand =
                new DelegateCommand<ItemsSourceChangedEventArgs>(WorkApprovalItemsSourceChanged);
            WorkApprovalItemChangedCommand = new DelegateCommand<GridSelectionChangedEventArgs>(WorkApprovalItemChanged);
            WorkApprovalUserItemChangedCommand =
                new DelegateCommand<GridSelectionChangedEventArgs>(WorkApprovalUserItemChanged);
            WorkApprovalCopyUserItemChangedCommand =
                new DelegateCommand<GridSelectionChangedEventArgs>(WorkApprovalCopyUserItemChanged);

            AddWorkApprovalUserCommand = new DelegateCommand<WorkApproval>(AddWorkApprovalUser,
                a => _addWorkApprovalUser);
            AddWorkApprovalCopyUserCommand = new DelegateCommand<WorkApproval>(AddWorkApprovalCopyUser,
                a => _addWorkApprovalCopyUser);
            DeleteWorkApprovalUserCommand = new DelegateCommand<WorkApprovalUser>(DeleteWorkApprovalUser,
                a => _deleteWorkApprovalUser);
            DeleteWorkApprovalCopyUserCommand = new DelegateCommand<WorkApprovalCopyUser>(DeleteWorkApprovalCopyUser,
                a => _deleteWorkApprovalCopyUser);

            ApprovalUserDragDropCommand = new DelegateCommand<GridDroppedEventArgs>(ApprovalUserDragDrop);
            CopyUserDragDropCommand = new DelegateCommand<GridDroppedEventArgs>(CopyUserDragDrop);

            OrganizationNodeCheckChangedCommand = new DelegateCommand<TreeListNodeEventArgs>(OrganizationNodeCheckChanged);
        }


        public ObservableCollection<SystemUser> SystemUserLookUp { get; set; }

        public ObservableCollection<WorkApproval> WorkApprovals { get; set; }
        public ObservableCollection<WorkApprovalUser> WorkApprovalUsers { get; set; }
        public ObservableCollection<WorkApprovalCopyUser> WorkApprovalCopyUsers { get; set; }
        public ObservableCollection<Organization> WorkApprovaVisibleOrganization { get; set; }
        public DelegateCommand LoadedCommand { get; set; }
        public DelegateCommand<ItemsSourceChangedEventArgs> WorkApprovalItemsSourceChangedCommand { get; set; }
        public DelegateCommand<GridSelectionChangedEventArgs> WorkApprovalItemChangedCommand { get; set; }
        public DelegateCommand<GridSelectionChangedEventArgs> WorkApprovalUserItemChangedCommand { get; set; }
        public DelegateCommand<GridSelectionChangedEventArgs> WorkApprovalCopyUserItemChangedCommand { get; set; }
        public DelegateCommand<TreeListNodeEventArgs> OrganizationNodeCheckChangedCommand { get; set; }
        public DelegateCommand<WorkApproval> AddWorkApprovalUserCommand { get; set; }
        public DelegateCommand<WorkApproval> AddWorkApprovalCopyUserCommand { get; set; }
        public DelegateCommand<WorkApprovalUser> DeleteWorkApprovalUserCommand { get; set; }
        public DelegateCommand<WorkApprovalCopyUser> DeleteWorkApprovalCopyUserCommand { get; set; }
        public DelegateCommand<GridDroppedEventArgs> ApprovalUserDragDropCommand { get; set; }
        public DelegateCommand<GridDroppedEventArgs> CopyUserDragDropCommand { get; set; }

        private void CopyUserDragDrop(GridDroppedEventArgs obj)
        {
            try
            {
                obj.GridControl.BeginDataUpdate();
                int i = 0;
                foreach (WorkApprovalCopyUser user in WorkApprovalCopyUsers)
                {
                    user.Sort = i++;
                    _workApprovalManager.Modifty(user);
                }
                var saveResult = _workApprovalManager.SaveChanged();
                if (string.IsNullOrWhiteSpace(saveResult))
                {
                    ShowNotification("保存成功!");
                    RaisePropertyChanged("WorkApprovalCopyUsers");
                }
                else
                {
                    ShowNotification(saveResult, "保存失败:");
                }
            }
            finally
            {
                obj.GridControl.EndDataUpdate();
            }
        }

        private void ApprovalUserDragDrop(GridDroppedEventArgs obj)
        {
            try
            {
                obj.GridControl.BeginDataUpdate();
                int i = 0;
                foreach (WorkApprovalUser user in WorkApprovalUsers)
                {
                    user.Sort = i++;
                    _workApprovalManager.Modifty(user);
                }
                var saveResult = _workApprovalManager.SaveChanged();
                if (string.IsNullOrWhiteSpace(saveResult))
                {
                    ShowNotification("保存成功!");
                }
                else
                {
                    ShowNotification(saveResult, "保存失败:");
                }
            }
            finally
            {
                obj.GridControl.EndDataUpdate();
            }
           
        }

        private void RaiseCanExecuteChanged()
        {
            AddWorkApprovalUserCommand.RaiseCanExecuteChanged();
            AddWorkApprovalCopyUserCommand.RaiseCanExecuteChanged();
            DeleteWorkApprovalUserCommand.RaiseCanExecuteChanged();
            DeleteWorkApprovalCopyUserCommand.RaiseCanExecuteChanged();
        }

        private void Loaded()
        {
            WorkApprovals = WorkApprovalManager.QueryLookUp();
            SystemUserLookUp = SystemAccountManager.QueryLookUp();
            RaisePropertiesChanged("WorkApprovals", "SystemUserLookUp");
        }

        private void WorkApprovalItemsSourceChanged(ItemsSourceChangedEventArgs e)
        {
            e.Source.SelectedItem = null;
            _lisenItemChanged = true;
        }

        private void WorkApprovalItemChanged(GridSelectionChangedEventArgs e)
        {
            if (!_lisenItemChanged)
            {
                _currentWorkApproval = null;
                _addWorkApprovalUser = false;
                _addWorkApprovalCopyUser = false;
                _deleteWorkApprovalUser = false;
                _deleteWorkApprovalCopyUser = false;
                RaiseCanExecuteChanged();
                return;
            }
            var tableView = e.Source as TableView;
            if ((tableView == null) || (tableView.Grid.SelectedItem == null))
            {
                _currentWorkApproval = null;
                _addWorkApprovalUser = false;
                _addWorkApprovalCopyUser = false;
                _deleteWorkApprovalUser = false;
                _deleteWorkApprovalCopyUser = false;
                RaiseCanExecuteChanged();
                return;
            }
            var selectRow = tableView.Grid.SelectedItem as WorkApproval;
            if (selectRow == null)
            {
                _currentWorkApproval = null;
                _addWorkApprovalUser = false;
                _addWorkApprovalCopyUser = false;
                _deleteWorkApprovalUser = false;
                _deleteWorkApprovalCopyUser = false;
                RaiseCanExecuteChanged();
                return;
            }
            _currentWorkApproval = selectRow;
            WorkApprovalUsers = _workApprovalManager.ApprovalUsers(selectRow);
            WorkApprovalCopyUsers = _workApprovalManager.CopyUsers(selectRow);
            WorkApprovaVisibleOrganization = WorkApprovalManager.WorkApprovalOrganization(selectRow.Id);

            _addWorkApprovalUser = true;
            _addWorkApprovalCopyUser = true;
            _deleteWorkApprovalUser = WorkApprovalUsers.Any();
            _deleteWorkApprovalCopyUser = WorkApprovalCopyUsers.Any();
            RaisePropertiesChanged("WorkApprovalUsers", "WorkApprovalCopyUsers", "WorkApprovaVisibleOrganization");
            RaiseCanExecuteChanged();
        }

        private void WorkApprovalUserItemChanged(GridSelectionChangedEventArgs e)
        {
        }

        private void WorkApprovalCopyUserItemChanged(GridSelectionChangedEventArgs e)
        {
        }

        private void AddWorkApprovalUser(WorkApproval workApproval)
        {
            if (workApproval == null)
                return;
            var selectUser = new SelectUser();
            var datacontent = ViewModelSource.Create(() => new SelectUserViewModel());
            datacontent.SystemUsers = SystemUserLookUp;
            selectUser.DataContext = datacontent;
            var diagResult = selectUser.ShowDialog();
            var selectList = datacontent.SelectItems.ToList();
            if (diagResult.HasValue && diagResult.Value && (selectList.Count > 0))
            {
                var sort = WorkApprovalUsers.Count > 0 ? WorkApprovalUsers[WorkApprovalUsers.Count-1].Sort + 1 : 0;
                foreach (var systemUser in selectList)
                {
                    if (WorkApprovalUsers.Any(a => a.UserId == systemUser.Id))
                        break;
                    var addItem = new WorkApprovalUser
                    {
                        Id = Guid.NewGuid(),
                        Sort = sort++,
                        UserId = systemUser.Id,
                        WorkApprovalId = workApproval.Id
                    };
                    if (string.IsNullOrWhiteSpace(_workApprovalManager.AddApprovalUser(addItem)))
                        WorkApprovalUsers.Add(addItem);
                }
                var saveResult = _workApprovalManager.SaveChanged();
                if (string.IsNullOrWhiteSpace(saveResult))
                    ShowNotification("添加成功!");
                else
                    ShowNotification(saveResult, "添加失败:");
            }
        }

        private void AddWorkApprovalCopyUser(WorkApproval workApproval)
        {
            if (workApproval == null)
                return;
            var selectUser = new SelectUser();
            var datacontent = ViewModelSource.Create(() => new SelectUserViewModel());
            datacontent.SystemUsers = SystemUserLookUp;
            selectUser.DataContext = datacontent;
            var diagResult = selectUser.ShowDialog();
            var selectList = datacontent.SelectItems.ToList();
            if (diagResult.HasValue && diagResult.Value && (selectList.Count > 0))
            {
                var sort = WorkApprovalCopyUsers.Count > 0 ? WorkApprovalCopyUsers[WorkApprovalCopyUsers.Count-1].Sort + 1 : 0;
                foreach (var systemUser in selectList)
                {
                    if (WorkApprovalCopyUsers.Any(a => a.UserId == systemUser.Id))
                        break;
                    var addItem = new WorkApprovalCopyUser
                    {
                        Id = Guid.NewGuid(),
                        Sort = sort++,
                        UserId = systemUser.Id,
                        WorkApprovalId = workApproval.Id
                    };
                    if (string.IsNullOrWhiteSpace(_workApprovalManager.AddApprovalCopyUser(addItem)))
                        WorkApprovalCopyUsers.Add(addItem);
                }
                var saveResult = _workApprovalManager.SaveChanged();
                if (string.IsNullOrWhiteSpace(saveResult))
                    ShowNotification("添加成功!");
                else
                    ShowNotification(saveResult, "添加失败:");
            }
        }

        private void DeleteWorkApprovalCopyUser(WorkApprovalCopyUser workApprovalCopyUser)
        {
            if (workApprovalCopyUser == null)
                return;
            if (string.IsNullOrWhiteSpace(_workApprovalManager.Delete(workApprovalCopyUser)))
            {
                WorkApprovalCopyUsers.Remove(workApprovalCopyUser);
                var saveChangedResult = _workApprovalManager.SaveChanged();
                if (string.IsNullOrWhiteSpace(saveChangedResult))
                {
                    ShowNotification("删除成功!");
                }
                else
                {
                    ShowNotification(saveChangedResult, "删除失败:");
                }
            }

        }

        private void DeleteWorkApprovalUser(WorkApprovalUser workApprovalUser)
        {
            if (workApprovalUser == null)
                return;
            if (string.IsNullOrWhiteSpace(_workApprovalManager.Delete(workApprovalUser)))
            {
                WorkApprovalUsers.Remove(workApprovalUser);
                var saveChangedResult = _workApprovalManager.SaveChanged();
                if (string.IsNullOrWhiteSpace(saveChangedResult))
                {
                    ShowNotification("删除成功!");
                }
                else
                {
                    ShowNotification(saveChangedResult, "删除失败:");
                }
            }
        }

        private void OrganizationNodeCheckChanged(TreeListNodeEventArgs e)
        {
            var row = e.Row as Organization;
            if (row == null || _currentWorkApproval == null)
            {
                return;
            }
            WorkApprovalManager.UpdateOrganizationWorkApproval(row.Id, _currentWorkApproval.Id, row.Checked);
        }
    }
}