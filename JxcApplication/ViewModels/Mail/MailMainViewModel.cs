using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.XtraRichEdit;
using JxcApplication.Core;
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
    public interface IMailEditPreview
    {
        MailEditPreviewType PreviewType { get; set; }
        bool ReadOnly { get; set; }
        void SetMail(MailOrder mail = null);
        void BuildEditPreview();
        void FreedEditPreview();
        void ControlLoaded(object control);
    }

    [POCOViewModel]
    public class MailPreview : IMailEditPreview
    {
        private MailOrder _currentMailOrder;
        private IMailDataProvide _mailDataProvide;
        public static MailPreview Create(IMailDataProvide dataProvide)
        {
            if (dataProvide == null)
            {
                throw new ArgumentNullException();
            }
            var tem = ViewModelSource.Create(() => new MailPreview());
            tem._mailDataProvide = dataProvide;
            return tem;
        }

        public virtual MailEditPreviewType PreviewType { get; set; }
        public virtual bool ReadOnly { get; set; }

        public void SetMail(MailOrder mail)
        {
            _currentMailOrder = mail;
            //throw new NotImplementedException();
        }

        public void BuildEditPreview()
        {
            PreviewType = _currentMailOrder.EditPreviewType;
            //throw new NotImplementedException();
        }

        public void FreedEditPreview()
        {
            PreviewType = MailEditPreviewType.Null;
            //throw new NotImplementedException();
        }

        public void ControlLoaded(object control)
        {
            if ((control as RichEditControl) != null)
            {
                PutDataPreview(control as RichEditControl);
            }
            else if ((control as SpreadsheetControl) != null)
            {
                PutDataPreview(control as SpreadsheetControl);
            }
        }

        private void PutDataPreview(RichEditControl control)
        {
            control.BeginInit();
            control.Document.LoadDocument(@"C:\Users\XIAO\Desktop\c++笔记z.doc", DocumentFormat.Doc);
            control.EndInit();
        }

        private void PutDataPreview(SpreadsheetControl control)
        {
            control.BeginInit();
            control.Document.LoadDocument(@"C:\Users\XIAO\Desktop\工资表格式.xlsx", DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            control.EndInit();
        }
    }

    [POCOViewModel]
    public class MailMainViewModel: ViewModelTabItem
    {
        private IMailDataProvide _mailDataProvide;
        public virtual ObservableCollection<MailListShowType> MailListShowTypes { get; set; }//Description = "收件箱"
        public virtual MailListShowType CurrentShowStype { get; set; }

        public virtual ObservableCollection<MailOrder> Mails { get; set; }
        public virtual MailOrder CurrentMail { get; set; }

        public virtual DelegateCommand ChangeUnreadStatusCommand { get; set; }
        public virtual DelegateCommand DeleteCommand { get; set; }
        public virtual DelegateCommand<NewMailType> CreateNewMailCommand { get; set; }

        public virtual IMailEditPreview EditPreview { get; set; }

        public MailMainViewModel(Guid menuId, string caption) : base(menuId, caption)
        {

        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitializeCommand();

            _mailDataProvide = new VirtualDatabaseMailDataProvide();

            MailListShowTypes = MailListShowType.GetItems();
            CurrentShowStype = MailListShowTypes[0];
        }

        #region POCO

        public void OnCurrentShowStypeChanged()
        {
            UpdateSource();
        }

        public void OnCurrentMailChanged()
        {
            UpdateToRead(CurrentMail);
            EditPreview = MailPreview.Create(_mailDataProvide);
            if (CurrentMail == null)
            {
                EditPreview.FreedEditPreview();
            }
            else
            {
                EditPreview.SetMail(CurrentMail);
                EditPreview.BuildEditPreview();
            }
            //RaisePropertyChanged("EditPreview");
        }

        #endregion

        #region Access Database

        private async void UpdateSource()
        {
            if (CurrentShowStype == null)
            {
                return;
            }
            var result = (await _mailDataProvide.GetItemAsync(CurrentShowStype))?.Select(Common.ConvertMailOrderModel).ToObservableCollection();
            if (Mails != result)
            {
                Mails = result;
            }
        }
        private async void UpdateToRead(MailOrder mail, bool unread = false)
        {
            if (mail == null)
            {
                return;
            }
            mail.IsUnread = unread;
            await _mailDataProvide.UpdateItemAsync(Common.ConvertMailOrder(mail));
        }
        private async void UpdateToDelete(MailOrder mail, bool dele = true)
        {
            if (mail == null)
            {
                return;
            }
            mail.IsDelete = dele;
            var result = await _mailDataProvide.UpdateItemAsync(Common.ConvertMailOrder(mail));
            if (result && dele)
            {
                Mails.Remove(CurrentMail);
                if (Mails.Count > 0)
                {
                    CurrentMail = Mails[0];
                }
                else
                {
                    CurrentMail = null;
                }
                RaisePropertiesChanged("Mails", "CurrentMail");
            }
        }
        private async void UpdateToRemove(MailOrder mail)
        {
            if (mail == null)
            {
                return;
            }
            var result = await _mailDataProvide.ReomverItemAsync(Common.ConvertMailOrder(mail));
            if (result)
            {
                Mails.Remove(CurrentMail);
                if (Mails.Count > 0)
                {
                    CurrentMail = Mails[0];
                }
                else
                {
                    CurrentMail = null;
                }
                RaisePropertiesChanged("Mails", "CurrentMail");
            }
        }
        

        #endregion

        #region Command method

        private void ChangeUnreadStatus()
        {
            UpdateToRead(CurrentMail, !CurrentMail.IsUnread);
            RaisePropertiesChanged("CurrentMail");
        }
        private bool CanChangeUnreadStatus()
        {
            return CurrentMail != null && CurrentShowStype.Type == MailListType.InBox;
        }

        private void Delete()
        {
            if (CurrentShowStype.Type == MailListType.DelBox)
            {//粉碎回收站邮件
                UpdateToRemove(CurrentMail);
            }
            else
            {//标记删除邮件
                UpdateToDelete(CurrentMail);
            }
            RaisePropertiesChanged("CurrentMail");
        }
        private bool CanDelete()
        {
            return CurrentMail != null && (CurrentShowStype.Type == MailListType.InBox || CurrentShowStype.Type == MailListType.DelBox);
        }

        private void CreateNewMail(NewMailType newMail)
        {
            //this.GetService<>()
        }
        #endregion

        private void InitializeCommand()
        {
            ChangeUnreadStatusCommand = new DelegateCommand(ChangeUnreadStatus, CanChangeUnreadStatus);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            CreateNewMailCommand = new DelegateCommand<NewMailType>(CreateNewMail);
        }
    }
}
