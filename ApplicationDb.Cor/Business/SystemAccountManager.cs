using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class SystemAccountManager
    {
        private ApplicationDbContext applicationDbEntities;

        private SystemAccountManager()
        {
            applicationDbEntities = new ApplicationDbContext();
        }
        public static SystemAccountManager Create()
        {
            return new SystemAccountManager();
        }

        public bool Create(SystemUser addUser, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new NotSupportedException("空密码");
            }
            string salt = "";
            if (!PasswordHelper.Encrypt(ref password, ref salt))
            {
                return false;
            }
            addUser.Password = password;
            addUser.Salt = salt;
            applicationDbEntities.SystemUser.Add(addUser);
            applicationDbEntities.SaveChanges();

            return true;
        }

        public bool Login(ref SystemUser loginUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUser.Code)||string.IsNullOrWhiteSpace(loginUser.Password))
                {
                    return false;
                }
                string code = loginUser.Code;
                SystemUser user = applicationDbEntities.SystemUser.FirstOrDefault(a => a.Code == code && a.Enable.HasValue && a.Enable.Value);
                if (user == null||user.Id==Guid.Empty)
                {
                    return false;
                }
                string password = loginUser.Password;
                string salt = user.Salt;

                if (PasswordHelper.Encrypt(ref password,ref salt))
                {
                    if (password==user.Password)
                    {
                        loginUser = user;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;

            }
            catch (Exception )
            {
                return false;
            }
        }

        public bool ModiftyPassword(string userName, string password, string newPassword)
        {
            var user = applicationDbEntities.SystemUser.FirstOrDefault(a => a.Code == userName);
            if (user==null)
            {
                return false;
            }
            string passwordResult = password;
            string salt = user.Salt;
            var resultBool = PasswordHelper.Encrypt(ref passwordResult, ref salt);
            if (resultBool == false)
            {
                return false;
            }
            if (user.Password != passwordResult)
            {
                return false;
            }
            string modiftyNewPassword = newPassword;

             resultBool = PasswordHelper.Encrypt(ref modiftyNewPassword, ref salt);
            if (resultBool == false)
            {
                return false;
            }

            user.Password = modiftyNewPassword;
            applicationDbEntities.Entry(user).State=EntityState.Modified;
            applicationDbEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 生成新密码
        /// </summary>
        /// <returns>加密密码</returns>
        public string EncryptPassword(string salt,string password)
        {
            string passwordResult = password;
            var resultBool = PasswordHelper.Encrypt(ref passwordResult, ref salt);
            if (resultBool == false)
            {
                throw new Exception("生成密码失败!");
            }
            return passwordResult;
        }


        public static ObservableCollection<SystemUser> QueryLookUp()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                return db.SystemUser.Where(a => a.Enable.HasValue && a.Enable.Value).ToObservableCollection();
            }
        } 

        public ObservableCollection<SystemUser> Query()
        {
            applicationDbEntities = new ApplicationDbContext();
            applicationDbEntities.SystemUser.Load();
            return applicationDbEntities.SystemUser.Local;
        }

        public string Save()
        {
            try
            {
                applicationDbEntities.SaveChanges();
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #region 弃用

        public bool Update(SystemUser user)
        {
            applicationDbEntities.Entry(user).State = EntityState.Modified;
            return applicationDbEntities.SaveChanges() > 0;
        }

        public void Undo(SystemUser user)
        {
            var entry = applicationDbEntities.Entry(user);
            entry.CurrentValues.SetValues(applicationDbEntities.Entry(user).OriginalValues);
            entry.State = EntityState.Unchanged;

        }

        public bool Add(SystemUser user)
        {
            applicationDbEntities.Entry(user).State = EntityState.Added;
            return applicationDbEntities.SaveChanges() > 0;
        }

        public bool Delete(SystemUser user)
        {
            applicationDbEntities.Entry(user).State = EntityState.Deleted;
            return applicationDbEntities.SaveChanges() > 0;
        }

        public bool Refresh()
        {
            return false;
        }

        #endregion

    }
}
