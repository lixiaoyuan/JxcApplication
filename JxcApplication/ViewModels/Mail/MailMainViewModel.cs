using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ApplicationDb.Cor;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.XtraRichEdit;
using JxcApplication.Core.Mail;
using JxcApplication.ViewModels.Inherit;
using Utilities;
using RichEditControl = DevExpress.Xpf.RichEdit.RichEditControl;

namespace JxcApplication.ViewModels.Mail
{
    public class Lookup
    {
        public string Name { get; set; }
    }
    public enum NewMailType
    {
        Mail,
        DayPlan,
        WeekPlan,
        MonthPlan
    }
     

    //[POCOViewModel]
    public class MailNewEdit :BindableBase, IMailPreview
    {
        private IMailDataProvide _dataProvide;
        private MailOrder _mailOrder;
        private NewMailType _newMailType;
        private bool _isReply;

        //public virtual ICollectionView Users { get;  set; }

        public virtual ICollectionView Users { get; set; }
        public virtual IList SelectUsers { get; set; }
        public bool SelectUserReadOnly { get { return _isReply; } }

        public MailOrder NewMail { get; set; }

        public DelegateCommand SendMessageCommand { get; set; }
        public DelegateCommand<RichEditControl> LoadedCommand { get; set; }
        public DelegateCommand<EditValueChangedEventArgs> EmailToUsersEditChangedCommand { get; set; }
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
            return SelectUsers != null && SelectUsers.Count > 0 && NewMail != null &&
                   !string.IsNullOrWhiteSpace(NewMail.Subject);
        }

        private void EmailToUsersEditChanged(EditValueChangedEventArgs e)
        {
            SelectUsers = (IList)e.NewValue;
            SendMessageCommand.RaiseCanExecuteChanged();
        }
         /// <summary>
         /// 推入邮件内容
         /// </summary>
         /// <param name="control"></param>
        private async void PutDataPreview(RichEditControl control)
        {
            control.BeginInit();
            MemoryStream ms = _isReply ? new MemoryStream(await _dataProvide.GetItemConentAsync(_mailOrder)) : new MemoryStream(await _dataProvide.GetNewMailConentAsync(_newMailType));
            if ( ms.Length > 0)
            {
                control.Document.LoadDocument(ms, DocumentFormat.Doc);
            }
            control.EndInit();
            GC.Collect();
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
            EmailToUsersEditChangedCommand=new DelegateCommand<EditValueChangedEventArgs>(EmailToUsersEditChanged);
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
        private RichEditControl _richEditControl;
        public MailMainViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }

        public virtual bool ShowLoadingMailList { get; set; }
        public virtual ObservableCollection<MailListShowType> MailListShowTypes { get; set; } //Description = "收件箱"
        public virtual MailListShowType CurrentShowStype { get; set; }

        public virtual ObservableCollection<MailOrder> Mails { get; set; }
        public virtual MailOrder CurrentMail { get; set; }

        public DelegateCommand<RichEditControl> RichEditControlLoadedCommand { get; set; }
        public virtual DelegateCommand ChangeUnreadStatusCommand { get; set; }
        public virtual DelegateCommand DeleteCommand { get; set; }
        public virtual DelegateCommand<NewMailType> CreateNewMailCommand { get; set; }
        public virtual DelegateCommand ReplyCommand { get; set; }

        public virtual IMailPreview MailNewEdit { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitializeCommand();

            _mailDataProvide = new VirtualDatabaseMailDataProvide();

            MailListShowTypes = MailListShowType.GetItems();
            CurrentShowStype = MailListShowTypes[0];
        }
        /// <summary>
        /// 初始化绑定命令
        /// </summary>
        private void InitializeCommand()
        {
            ChangeUnreadStatusCommand = new DelegateCommand(ChangeUnreadStatus, CanChangeUnreadStatus);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            CreateNewMailCommand = new DelegateCommand<NewMailType>(CreateNewMail);
            ReplyCommand = new DelegateCommand(Reply, CanReply);
            RichEditControlLoadedCommand = new DelegateCommand<RichEditControl>(RichEditControlLoaded);
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

        /// <summary>
        /// 推入预览数据
        /// </summary>
        private async void PutDataPreview()
        {
            if (_richEditControl == null)
            {
                return;
            }
            _richEditControl.BeginInit();
            MemoryStream ms = new MemoryStream(await _mailDataProvide.GetItemConentAsync(CurrentMail));
            _richEditControl.Document.LoadDocument(ms, DocumentFormat.Doc);
            _richEditControl.EndInit();
            GC.Collect();
        }

        #region POCO

        public void OnCurrentShowStypeChanged()
        {
            UpdateSource();
        }

        public void OnCurrentMailChanged()
        {
            UpdateToRead(CurrentMail);
            PutDataPreview();
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

        private void RichEditControlLoaded(RichEditControl control)
        {
            _richEditControl = control;
        }

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