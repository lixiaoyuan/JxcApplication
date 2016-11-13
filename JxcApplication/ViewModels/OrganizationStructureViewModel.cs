using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using JxcApplication.Core;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    [POCOViewModel]
    public class OrganizationStructureViewModel : ViewModelTabItem
    {
        private OrganizationStructureManager _manager;
        private SystemAccountManager _accountManager;
        private Organization _lastOrganization;

        public ObservableCollection<Organization> Organs { get; set; }
        public ObservableCollection<SystemUser> Users { get; set; }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            _manager = OrganizationStructureManager.Create();
            _accountManager = SystemAccountManager.Create();

            Organs = _manager.QueOrganizations();
        }

        public void Add(Organization selectItem)
        {
            Organization newOrganization = new Organization();
            newOrganization.Id = Guid.NewGuid();
            newOrganization.ParentId = selectItem?.ParentId;
            newOrganization.Enable = true;
            newOrganization.Code = SortCodeGenerate.GetCode("BM",_manager.MaxCode());
            var result = CreateDialog("DataTemplateOrganEdit", newOrganization,"添加部门").ShowDialog();
            if (result.HasValue && result.Value)
            {
                _manager.Add(newOrganization);
                RaisePropertyChanged("Organs");
            }
        }
        public void AddChildren(Organization selectItem)
        {
            if (selectItem == null)
            {
                return;
            }
            Organization newOrganization=new Organization();
            newOrganization.Id = Guid.NewGuid();
            newOrganization.ParentId = selectItem.Id;
            newOrganization.Enable = true;
            newOrganization.Code = SortCodeGenerate.GetCode("BM",_manager.MaxCode());
            var result = CreateDialog("DataTemplateOrganEdit", newOrganization, "添加子部门").ShowDialog();
            if (result.HasValue && result.Value)
            {
                _manager.Add(newOrganization);
                RaisePropertyChanged("Organs");
            }
        }

        public void SelectItemChanged(SelectedItemChangedEventArgs args)
        {
            if (args == null || args.NewItem == null)
            {
                _lastOrganization = null;
                return;
            }
            _lastOrganization = (Organization) args.NewItem;
            Users = _manager.GetOrganizationUserCheck(_lastOrganization.Id);
            RaisePropertyChanged("Users");
        }

        public void Save(GridControl opControl)
        {
            opControl.View.CommitEditing();
            if (_lastOrganization == null)
            {
                DXMessageBox.Show("请选择部门!");
                return;
            }
            var result = DXMessageBox.Show(String.Format("是否把当前已选择用户分配到【{0}】部门?", _lastOrganization.Name), "警告:", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }

            string checkedUserIds = string.Join(",", Users.Where(a => { return a.Check != null && a.Check.Value; }).Select(a => a.Id));
            var saveResult = _manager.UpdateOrganizationUser(_lastOrganization.Id, checkedUserIds);
            if (!string.IsNullOrWhiteSpace(saveResult))
            {
                NotificationService.CreateCustomNotification(CustomNotificationViewModel.Create(saveResult)).ShowAsync();
                Users = null;
                RaisePropertyChanged("Users");
            }
            else
            {
                NotificationService.CreateCustomNotification(CustomNotificationViewModel.Create("保存成功!")).ShowAsync();
            }
        }

        public OrganizationStructureViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}