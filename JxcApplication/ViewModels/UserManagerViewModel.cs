using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using JxcApplication.Control;
using JxcApplication.ViewModels.Inherit;
using Utilities;

namespace JxcApplication.ViewModels
{
    public class UserManagerViewModel : ViewModelTabItem
    {
        private readonly SystemAccountManager _systemAccountManager = SystemAccountManager.Create();

        public ObservableCollection<SystemUser> SystemUsers { get; set; }


        //protected override void OnNavigatedTo()
        //{
        //    try
        //    {
        //        base.OnNavigatedTo();

        //    }
        //    catch (Exception e)
        //    {
        //        if (!IsInDesignMode)
        //        {
        //            DXMessageBox.Show(e.Message);
        //        }
        //    }
        //}

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitData();
        }

        public void InitData()
        {
            SystemUsers = _systemAccountManager.Query();
            RaisePropertiesChanged("SystemUsers");
        }

        public void Save(GridControl opControl)
        {
            opControl.View.CommitEditing();
            var result = _systemAccountManager.Save();
            if (string.IsNullOrWhiteSpace(result))
            {
                ShowNotification("保存成功!");
            }
            else
            {
                ShowNotification($"保存失败,{result}");
            }
        }

        public void ReSetPassword(SystemUser selectUser)
        {
            if (selectUser==null)
            {
                DXMessageBox.Show("请选择需要重置密码的用户!");
                return;
            }
            var modiftyUser = new SystemUser() { Name = selectUser.Name };
            var result = CreateDialog("DataTemplateRestPassword", modiftyUser, "重置用户密码").ShowDialog();

            if (result.HasValue && result.Value)
            {
                //Console.WriteLine(modiftyUser.Password);
                if (string.IsNullOrWhiteSpace(selectUser.Salt) || selectUser.Salt == Guid.Empty.ToString("D"))
                {
                    selectUser.Salt = Guid.NewGuid().ToString("D");
                }
                selectUser.Password = _systemAccountManager.EncryptPassword(selectUser.Salt, modiftyUser.Password);
                selectUser.ChangePasswordDate = DBUnit.GetDbTime();
                var resultstr = _systemAccountManager.Save();
                if (string.IsNullOrWhiteSpace(resultstr))
                {
                    ShowNotification("保存成功!");
                }
                else
                {
                    ShowNotification($"保存失败,{resultstr}");
                }
            }
        }

        public void InitNewRow(InitNewRowEventArgs args)
        {

            TableView tableView = (TableView)args.OriginalSource;
            var newRow = (SystemUser)tableView.Grid.GetRow(args.RowHandle);
            newRow.Id=Guid.NewGuid();
            newRow.Salt = Guid.NewGuid().ToString("D");
            newRow.Enable = true;
            newRow.Gender = true;
            newRow.CreateDate = DBUnit.GetDbTime();
            newRow.CreateUserName = App.GlobalApp.LoginUser.Name;
            newRow.CreateUserId = App.GlobalApp.LoginUser.Id;
        }

        public class UserEditDataContent
        {
            public UserEditDataContent(bool isAdd, SystemUser user, GridControl parentControl)
            {
                IsEdit = !isAdd;
                IsAdd = isAdd;
                editObject = user;
                Parent = parentControl;
            }

            public GridControl Parent { get; set; }
            public bool IsAdd { get; }
            public bool IsEdit { get; set; }
            public SystemUser editObject { get; }
        }

        #region 没用

        public void SetRole(GridControl opControl)
        {
            //SelectRoleDataContent selectRoleDataContent=ViewModelSource.Create(()=> new SelectRoleDataContent());
            //selectRoleDataContent.Roles = _roleManager.Que();

            //SelectRole selectRole = new SelectRole
            //{
            //    DataContext = selectRoleDataContent
            //};
            //selectRole.ShowDialog();
        }

        private void SetRole()
        {
        }

        #endregion

        #region 增删改

        [Obsolete]
        public void Add(GridControl opControl)
        {
            EditCor(true, opControl);
        }

        [Obsolete]
        public void Edit(GridControl opControl)
        {
            EditCor(false, opControl);
        }

        [Obsolete]
        public void Del(GridControl opControl)
        {
            var user = (SystemUser) opControl.SelectedItem;
            var buttonresult = DXMessageBox.Show(opControl, string.Format("是否删除{0}?", user.Name), "警告:", MessageBoxButton.YesNo);
            if (buttonresult == MessageBoxResult.Yes)
            {
                var result = _systemAccountManager.Delete(user);
                if (result)
                {
                    ShowNotification(string.Format("{0}删除成功!", user.Name));
                }
                else
                {
                    ShowNotification(string.Format("{0}删除失败!", user.Name));
                }
                InitData();
            }
        }

        [Obsolete]
        private void EditCor(bool isAdd, GridControl opControl)
        {
            SystemUser user;
            if (isAdd)
            {
                user = new SystemUser();
                user.Enable = true;
                user.Gender = true;
            }
            else
            {
                user = (SystemUser) opControl.SelectedItem;
            }
            var model = ViewModelSource.Create(() => new UserEditDataContent(isAdd, user, opControl));
            var userEdit = new UserEdit
            {
                DataContext = model
            };
            var result = userEdit.ShowDialog();

            Model_Closed(result != null && result.Value, model);
        }

        [Obsolete]
        private void Model_Closed(bool result, UserEditDataContent dataContent)
        {
            try
            {
                if (result)
                {
                    if (dataContent.IsAdd)
                    {
                        #region 新增

                        dataContent.editObject.CreateUserId = App.GlobalApp.LoginUser.Id;
                        dataContent.editObject.CreateUserName = App.GlobalApp.LoginUser.Name;
                        dataContent.editObject.Id = Guid.NewGuid();
                        var salt = Guid.NewGuid().ToString();
                        var password = dataContent.editObject.Password;
                        var encResult = PasswordHelper.Encrypt(ref password, ref salt);
                        if (!encResult)
                        {
                            ShowNotification("新增失败!密码加密错误!");
                            return;
                        }
                        dataContent.editObject.Salt = salt;
                        dataContent.editObject.Password = password;
                        if (_systemAccountManager.Add(dataContent.editObject))
                        {
                            ShowNotification(string.Format("{0}资料新增成功!", dataContent.editObject.Name));
                        }
                        else
                        {
                            ShowNotification(string.Format("{0}资料新增失败!", dataContent.editObject.Name));
                        }
                        InitData();

                        #endregion
                    }
                    else
                    {
                        #region 更新

                        dataContent.editObject.ModifyUserId = App.GlobalApp.LoginUser.Id;
                        dataContent.editObject.ModifyUserName = App.GlobalApp.LoginUser.Name;
                        if (_systemAccountManager.Update(dataContent.editObject))
                        {
                            ShowNotification(string.Format("{0}资料更新成功!", dataContent.editObject.Name));
                        }
                        else
                        {
                            ShowNotification(string.Format("{0}资料更新失败!", dataContent.editObject.Name));
                        }

                        #endregion
                    }
                }
                else if (!dataContent.IsAdd)
                {
                    _systemAccountManager.Undo(dataContent.editObject);
                }
            }
            catch (Exception e)
            {
                ShowNotification(e.Message, "错误:");
            }
            finally
            {
                dataContent.Parent.RefreshData();
            }
        }

        #endregion

        public UserManagerViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}