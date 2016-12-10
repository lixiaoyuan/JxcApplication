using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using JxcApplication.ViewModels.Mail;

namespace JxcApplication.Core.Mail
{
    public class VirtualDatabaseMailDataProvide : IMailDataProvide
    { 
        public Task<List<MailOrder>> GetItemListAsync(MailListShowType type)
        {
            return Task<List<MailOrder>>.Factory.StartNew(() =>
            {
                if (type.Type == MailListType.InBox)
                {
                    return MailManager.GetInBoxItems(App.GlobalApp.LoginUser.Id).ToList();
                }
                else if (type.Type == MailListType.DelBox)
                {
                    return MailManager.GetDelBoxItems(App.GlobalApp.LoginUser.Id).ToList();
                }
                else if (type.Type == MailListType.SenBox)
                {
                    return MailManager.GetSendBoxItems(App.GlobalApp.LoginUser.Id).ToList();
                }
                return new List<MailOrder>();
            });
          
        }

        public  Task<bool> UpdateItemAsync(MailOrder MailOrder)
        {
            return Task<bool>.Factory.StartNew(() => MailManager.Update(MailOrder));
        }

        public Task<bool> ReomverItemAsync(MailOrder mailOrder)
        {
            return Task<bool>.Factory.StartNew(() => MailManager.Delete(mailOrder));
        }

        public Task<bool> CreateItemAsync(byte[] content, params MailOrder[] mailOrder)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                Guid fileid = FileCabinetsManager.InserFile(content, $"邮件文件-发送人:{mailOrder[0].FormUserName}");
                if (fileid == Guid.Empty)
                {
                    return false;
                }
                foreach (MailOrder order in mailOrder)
                {
                    order.ConetntFileId = fileid;
                }
                return MailManager.Inster(mailOrder);
            });
        }

        public Task<byte[]> GetItemConentAsync(MailOrder mailOrder)
        {
            return Task<byte[]>.Factory.StartNew(() =>
            {
                if (mailOrder == null || mailOrder.ConetntFileId == null || mailOrder.ConetntFileId == Guid.Empty)
                {
                    return null;
                }
                return FileCabinetsManager.GetFile(mailOrder.ConetntFileId.Value).Data;
            });
        }

        public Task<byte[]> GetNewMailConentAsync(NewMailType newMailType)
        {
            return Task<byte[]>.Factory.StartNew(() =>
            {
                Guid fileGuid = Guid.Empty;
                switch (newMailType)
                {
                    case NewMailType.DayPlan: fileGuid = Guid.Parse("C5BBF4D7-DA5F-41EC-9D0B-047529EF46D2"); break;
                    case NewMailType.WeekPlan: fileGuid = Guid.Parse("113109E0-BE03-4CDE-A818-4FF828C5F6CB"); break;
                    case NewMailType.MonthPlan: fileGuid = Guid.Parse("BC8FE0EE-A1E4-4758-A1DF-67F3888747E6"); break;
                    default: fileGuid = Guid.Empty; break;
                }
                if (fileGuid != Guid.Empty)
                {
                    return MailManager.GetNewMailContent(fileGuid);
                }
                return new byte[0];
            });
        }

        public Task<List<SystemUser>> GetMailUserList()
        {
            return Task<List<SystemUser>>.Factory.StartNew(() => { return SystemAccountManager.QueryLookUp().ToList(); });
        }
    }
}
