using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;
using DevExpress.Mvvm.POCO;

namespace JxcApplication.Core.Mail
{
    public enum MailListType
    {
        InBox,
        SenBox,
        DelBox,
        DraftsBox
    }
    public class MailListShowType
    {
        public string Name { get; set; }
        public MailListType Type { get; set; }

        public static ObservableCollection<MailListShowType> GetItems()
        {
            return new ObservableCollection<MailListShowType>(new[]
            {
                new MailListShowType() {Name = "收件箱",Type = MailListType.InBox},
                new MailListShowType() {Name = "发件箱",Type = MailListType.SenBox}, 
                new MailListShowType() {Name = "回收站",Type = MailListType.DelBox}, 
                new MailListShowType() {Name = "草稿箱",Type = MailListType.DraftsBox},
            });
        }
    }

    public static class Common
    {
        public static MailOrder ConvertMailOrderModel( MailOrder mailOrder)
        {
            var t = ViewModelSource.Create(() => new MailOrder());
            if (mailOrder == null)
            {
                return t;
            }
            t.Id = mailOrder.Id;
            t.IsDelete = mailOrder.IsDelete;
            t.IsDaft = mailOrder.IsDaft;
            t.IsReply = mailOrder.IsReply;
            t.IsUnread = mailOrder.IsUnread;
            t.Subject = mailOrder.Subject;
            t.ToAddress = mailOrder.ToAddress;
            t.ToUser = mailOrder.ToUser;
            t.ToUserName = mailOrder.ToUserName;
            t.ConetntFileId = mailOrder.ConetntFileId;
            t.CreateDateTime = mailOrder.CreateDateTime;
            t.FormAddress = mailOrder.FormAddress;
            t.FormUser = mailOrder.FormUser;
            t.FormUserName = mailOrder.FormUserName;
            t.ReplyMailId = mailOrder.ReplyMailId;
            return t;
        }
        public static MailOrder ConvertMailOrder(MailOrder mailOrder)
        {
            var t = new MailOrder();
            if (mailOrder == null)
            {
                return t;
            }
            t.Id = mailOrder.Id;
            t.IsDelete = mailOrder.IsDelete;
            t.IsDaft = mailOrder.IsDaft;
            t.IsReply = mailOrder.IsReply;
            t.IsUnread = mailOrder.IsUnread;
            t.Subject = mailOrder.Subject;
            t.ToAddress = mailOrder.ToAddress;
            t.ToUser = mailOrder.ToUser;
            t.ToUserName = mailOrder.ToUserName;
            t.ConetntFileId = mailOrder.ConetntFileId;
            t.CreateDateTime = mailOrder.CreateDateTime;
            t.FormAddress = mailOrder.FormAddress;
            t.FormUser = mailOrder.FormUser;
            t.FormUserName = mailOrder.FormUserName;
            t.ReplyMailId = mailOrder.ReplyMailId;
            return t;
        }
    }
}
