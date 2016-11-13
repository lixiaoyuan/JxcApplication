using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using JxcApplication.Control;
using JxcApplication.ViewModels.Inherit;
using Utilities;

namespace JxcApplication.ViewModels
{
    public class RoleManagerViewModel : ViewModelTabItem
    {
        private readonly RoleManager _roleManager = RoleManager.Create();

        public ObservableCollection<AuthRole> Roles { get; set; }



        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitData();
        }

        private void InitData()
        {
            Roles = _roleManager.Que();
            RaisePropertiesChanged("Roles");
        }

        private void EditCor(AuthRole modiftyRole, bool isAdd)
        {
            if (isAdd)
            {
                modiftyRole = new AuthRole {Enable = true};
            }

            var re = ViewModelSource.Create(() => new RoleEditDataContent(modiftyRole, isAdd));
            var roleEdit = new RoleEdit();
            roleEdit.DataContext = re;
            var result = roleEdit.ShowDialog();
            if (result != null && result.Value)
            {
                if (isAdd)
                {
                    modiftyRole.Id = Guid.NewGuid();
                    modiftyRole.CreateUserId = App.GlobalApp.LoginUser.Id;
                    modiftyRole.CreateDate = DateTime.Now;
                    modiftyRole.CreateUserName = App.GlobalApp.LoginUser.Name;

                    var resultadd = _roleManager.Add(modiftyRole);
                    if (resultadd)
                    {
                        ShowNotification("添加成功!");
                    }
                    else
                    {
                        ShowNotification("添加失败!");
                        _roleManager.Undo(modiftyRole);
                    }
                }
                else
                {
                    modiftyRole.ModifyUserId = App.GlobalApp.LoginUser.Id;
                    modiftyRole.ModifyDate = DateTime.Now;
                    modiftyRole.ModifyUserName = App.GlobalApp.LoginUser.Name;
                    var resultUpdate = _roleManager.Update(modiftyRole);
                    if (resultUpdate)
                    {
                        ShowNotification("更新成功!");
                    }
                    else
                    {
                        ShowNotification("更新失败!");
                        _roleManager.Undo(modiftyRole);
                    }
                }
            }
            else if (!isAdd)
            {
                _roleManager.Undo(modiftyRole);
            }
        }

        public void Add(GridControl control)
        {
            try
            {
                EditCor(null, true);
                InitData();
            }
            finally
            {
                control.RefreshData();
            }
        }

        public void Edit(GridControl control)
        {
            try
            {
                var ar = (AuthRole) control.SelectedItem;
                if (ar == null)
                {
                    DXMessageBox.Show("请选择编辑数据!");
                    return;
                }
                EditCor(ar, false);
            }
            finally
            {
                control.RefreshData();
            }
        }

        public void Del(GridControl control)
        {
            try
            {
                var ar = (AuthRole) control.SelectedItem;
                if (ar == null)
                {
                    DXMessageBox.Show("请选择删除数据!");
                    return;
                }
                if (DXMessageBox.Show("是否删除?", "警告:", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }

                var resultDel = _roleManager.Delete(ar);
                if (resultDel)
                {
                    ShowNotification("删除成功!!");
                    InitData();
                }
                else
                {
                    ShowNotification("删除失败!!");
                }
            }
            finally
            {
                control.RefreshData();
            }
        }

        public void Refresh(GridControl control)
        {
            try
            {
                InitData();
            }
            finally
            {
                control.RefreshData();
            }
        }

        public RoleManagerViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }

    public class RoleEditDataContent
    {
        public RoleEditDataContent(AuthRole role, bool isAdd)
        {
            EditObject = role;
            IsEdit = !isAdd;
            IsAdd = isAdd;
        }

        public AuthRole EditObject { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
    }
}