using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ApplicationDb.Cor;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.XtraRichEdit;
using JxcApplication.Core.Mail;
using JxcApplication.ViewModels.Inherit;
using Utilities;
using RichEditControl = DevExpress.Xpf.RichEdit.RichEditControl;

namespace JxcApplication.ViewModels.Mail
{
    public enum NewMailType
    {
        Mail,
        DayPlan,
        WeekPlan,
        MonthPlan
    }

    [POCOViewModel]
    public class MailPreview : IMailPreview
    {
        private MailOrder _currentMailOrder;
        private IMailDataProvide _mailDataProvide;

 

        public void Loaded(RichEditControl control)
        {
                PutDataPreview(control);
        }

        public static MailPreview Create(MailOrder mailOrder, IMailDataProvide dataProvide)
        {
            if ((dataProvide == null) || (mailOrder == null))
                throw new ArgumentNullException();
            var tem = ViewModelSource.Create(() => new MailPreview());
            tem._mailDataProvide = dataProvide;
            tem._currentMailOrder = mailOrder;
            return tem;
        }

        private void PutDataPreview(RichEditControl control)
        {
            control.BeginInit();
            control.Document.LoadDocument(@"C:\Users\XIAO\Desktop\c++笔记z.doc", DocumentFormat.Doc);
            control.EndInit();
        }
    }

    //[POCOViewModel]
    public class MailNewEdit :BindableBase, IMailPreview
    {
        private IMailDataProvide _dataProvide;
        private MailOrder _mailOrder;
        private NewMailType _newMailType;
        private bool _isReply;


        //public virtual ObservableCollection<SystemUser> Users { get; set; }
        public virtual ICollectionView Users { get;  set; }
        public virtual  List<SystemUser> SelectUsers { get;set; }

        public  MailOrder NewMail { get; set; }


        public DelegateCommand SendMessageCommand { get; set; }
        public DelegateCommand<RichEditControl> LoadedCommand { get; set; }
        public DelegateCommand EmailToUsersEditChangedCommand { get; set; }
        public DelegateCommand buttonCommand { get; set; }
        public void Loaded(RichEditControl control)
        {
            PutDataPreview(control);
            FillPrepareDataAsync();
        }

        private void SendMessage()
        {
        }

        private bool CanSendMessage()
        {
            bool i = SelectUsers != null;
            if (i)
            {
                i = SelectUsers.Count > 0;

            }
            i = NewMail != null;
            if (i)
            {
                i = !string.IsNullOrWhiteSpace(NewMail.Subject);

            }
            return SelectUsers != null && SelectUsers.Count > 0 && NewMail != null &&
                   !string.IsNullOrWhiteSpace(NewMail.Subject);
        }

        private void EmailToUsersEditChanged()
        {
            SendMessageCommand.RaiseCanExecuteChanged();
        }

        private void button()
        {
            MessageBox.Show(SelectUsers.Count.ToString());
        }
        private void PutDataPreview(RichEditControl control)
        {
            control.BeginInit();
            control.Document.LoadDocument(@"C:\Users\XIAO\Desktop\c++笔记z.doc", DocumentFormat.Doc);
            control.EndInit();
        }
        /// <summary>
        /// 填充准备数据
        /// </summary>
        private async void FillPrepareDataAsync()
        {
            var result = (await _dataProvide.GetMailUserList()).ToObservableCollection();
            Users = new CollectionViewSource() { Source = result }.View;
            //填充选择用户
            if (SelectUsers == null)
            {
                if (_isReply)
                {
                    if (_mailOrder.FormUser.HasValue && _mailOrder.FormUser.Value != Guid.Empty)
                    {
                        var findFormUser = result.First(a => a.Id == _mailOrder.FormUser.Value);
                        if (findFormUser != null)
                        {
                            SelectUsers = new List<SystemUser>(new[] { findFormUser });
                        }
                    }
                }
                if (SelectUsers == null)
                {
                    SelectUsers = new List<SystemUser>();
                }
            }
            //创建mailorder
            NewMail = CreateNewMailOrder();
            RaisePropertiesChanged("Users", "SelectUsers", "NewMail");
        }

        #region Static

        public static MailNewEdit Create(IMailDataProvide dataProvide, NewMailType newMailType)
        {
            //var returnTem = ViewModelSource.Create(() => new MailNewEdit(dataProvide, newMailType));
            //return returnTem;
            return new MailNewEdit(dataProvide,newMailType);
        }

        public static MailNewEdit Create(IMailDataProvide dataProvide, MailOrder mailOrder)
        {
            //var returnTem = ViewModelSource.Create(() => new MailNewEdit(dataProvide, mailOrder));
            //return returnTem;
            return new MailNewEdit(dataProvide,mailOrder);
        }

        #endregion

        #region 构造函数

        public MailNewEdit(IMailDataProvide dataProvide, MailOrder mailOrder)
        {
            _dataProvide = dataProvide;
            _mailOrder = mailOrder;
            _newMailType = NewMailType.Mail;
            _isReply = true;
            InitCommand();
        }

        public MailNewEdit(IMailDataProvide dataProvide, NewMailType newMailType)
        {
            _dataProvide = dataProvide;
            _mailOrder = CreateNewMailOrder();
            _newMailType = newMailType;
            InitCommand();
        }

        private void InitCommand()
        {
            SendMessageCommand = new DelegateCommand(SendMessage, CanSendMessage);
            LoadedCommand = new DelegateCommand<RichEditControl>(Loaded);
            EmailToUsersEditChangedCommand=new DelegateCommand(EmailToUsersEditChanged);
            buttonCommand=new DelegateCommand(button);
        }

        #endregion

        private MailOrder CreateNewMailOrder()
        {
            var returnTem= new MailOrder();
            returnTem.Id = Guid.NewGuid();
            returnTem.FormUser = App.GlobalApp.LoginUser.Id;
            returnTem.FormUserName = App.GlobalApp.LoginUser.Name;
            if (_isReply)
            {
                returnTem.IsReply = true;
                returnTem.ReplyMailId = _mailOrder.Id;
                returnTem.Subject = "回复 - " + _mailOrder.Subject;
            }
            return returnTem;
        }

    }

    [POCOViewModel]
    public class MailMainViewModel : ViewModelTabItem
    {
        private IMailDataProvide _mailDataProvide;

        public MailMainViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }

        public virtual bool ShowLoadingMailList { get; set; }
        public virtual ObservableCollection<MailListShowType> MailListShowTypes { get; set; } //Description = "收件箱"
        public virtual MailListShowType CurrentShowStype { get; set; }

        public virtual ObservableCollection<MailOrder> Mails { get; set; }
        public virtual MailOrder CurrentMail { get; set; }

        public virtual DelegateCommand ChangeUnreadStatusCommand { get; set; }
        public virtual DelegateCommand DeleteCommand { get; set; }
        public virtual DelegateCommand<NewMailType> CreateNewMailCommand { get; set; }
        public virtual DelegateCommand ReplyCommand { get; set; }

        public virtual IMailPreview MailPreview { get; set; }
        public virtual IMailPreview MailNewEdit { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitializeCommand();

            _mailDataProvide = new VirtualDatabaseMailDataProvide();

            MailListShowTypes = MailListShowType.GetItems();
            CurrentShowStype = MailListShowTypes[0];
        }

        private void InitializeCommand()
        {
            ChangeUnreadStatusCommand = new DelegateCommand(ChangeUnreadStatus, CanChangeUnreadStatus);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            CreateNewMailCommand = new DelegateCommand<NewMailType>(CreateNewMail);
            ReplyCommand = new DelegateCommand(Reply, CanReply);
        }

        private void CreateNewMailCore(NewMailType newMailType)
        {
            MailNewEdit = JxcApplication.ViewModels.Mail.MailNewEdit.Create(_mailDataProvide, newMailType);
            GetService<IWindowService>().Show(this);
        }

        private void CreateNewMailCore(MailOrder replyMail)
        {
            MailNewEdit = JxcApplication.ViewModels.Mail.MailNewEdit.Create(_mailDataProvide, replyMail);
            GetService<IWindowService>().Show(this);
        }

        #region POCO

        public void OnCurrentShowStypeChanged()
        {
            UpdateSource();
        }

        public void OnCurrentMailChanged()
        {
            UpdateToRead(CurrentMail);
            if (CurrentMail == null)
                MailPreview = null;
            else
                MailPreview = Mail.MailPreview.Create(CurrentMail, _mailDataProvide);
        }

        #endregion

        #region Access Database

        private async void UpdateSource()
        {
            if (CurrentShowStype == null)
                return;
            ShowLoadingMailList = true;
            var result =
                (await _mailDataProvide.GetItemAsync(CurrentShowStype))?.Select(Common.ConvertMailOrderModel)
                    .ToObservableCollection();
            ShowLoadingMailList = false;
            if (Mails != result)
                Mails = result;
            GC.Collect();
        }

        private async void UpdateToRead(MailOrder mail, bool unread = false)
        {
            if (mail == null)
                return;
            mail.IsUnread = unread;
            await _mailDataProvide.UpdateItemAsync(Common.ConvertMailOrder(mail));
        }

        private async void UpdateToDelete(MailOrder mail, bool dele = true)
        {
            if (mail == null)
                return;
            mail.IsDelete = dele;
            var result = await _mailDataProvide.UpdateItemAsync(Common.ConvertMailOrder(mail));
            if (result && dele)
            {
                Mails.Remove(CurrentMail);
                if (Mails.Count > 0)
                    CurrentMail = Mails[0];
                else
                    CurrentMail = null;
                RaisePropertiesChanged("Mails", "CurrentMail");
            }
        }

        private async void UpdateToRemove(MailOrder mail)
        {
            if (mail == null)
                return;
            var result = await _mailDataProvide.ReomverItemAsync(Common.ConvertMailOrder(mail));
            if (result)
            {
                Mails.Remove(CurrentMail);
                if (Mails.Count > 0)
                    CurrentMail = Mails[0];
                else
                    CurrentMail = null;
                RaisePropertiesChanged("Mails", "CurrentMail");
            }
        }

        #endregion

        #region Command method

        /// <summary>
        ///     修改未读状态
        /// </summary>
        private void ChangeUnreadStatus()
        {
            UpdateToRead(CurrentMail, !CurrentMail.IsUnread);
            RaisePropertiesChanged("CurrentMail");
        }

        private bool CanChangeUnreadStatus()
        {
            return (CurrentMail != null) && (CurrentShowStype.Type == MailListType.InBox);
        }

        /// <summary>
        ///     删除邮件
        /// </summary>
        private void Delete()
        {
            if (CurrentShowStype.Type == MailListType.DelBox)
                UpdateToRemove(CurrentMail);
            else
                UpdateToDelete(CurrentMail);
            RaisePropertiesChanged("CurrentMail");
        }

        private bool CanDelete()
        {
            return (CurrentMail != null) &&
                   ((CurrentShowStype.Type == MailListType.InBox) || (CurrentShowStype.Type == MailListType.DelBox));
        }

        /// <summary>
        ///     创建新邮件
        /// </summary>
        /// <param name="newMail"></param>
        private void CreateNewMail(NewMailType newMail)
        {
            CreateNewMailCore(newMail);
        }

        /// <summary>
        ///     回复
        /// </summary>
        private void Reply()
        {
            CreateNewMailCore(CurrentMail);
        }

        private bool CanReply()
        {
            return (CurrentMail != null) && (CurrentShowStype.Type == MailListType.InBox);
        }

        #endregion
    }
}