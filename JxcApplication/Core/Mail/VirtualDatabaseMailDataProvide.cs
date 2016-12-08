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
        public Task<IEnumerable<MailOrder>> GetItemAsync(MailListShowType type)
        {
            return Task<IEnumerable<MailOrder>>.Factory.StartNew(() =>
            {
                if (type.Type == MailListType.InBox)
                {
                    return MailManager.GetInBoxItems(App.GlobalApp.LoginUser.Id);
                }
                else if (type.Type == MailListType.DelBox)
                {
                    return MailManager.GetDelBoxItems(App.GlobalApp.LoginUser.Id);
                }
                return null;
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

        public Task<bool> CreateItemAsync(MailOrder mailOrder)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateItemAsync(MailOrder mailOrder, byte[] content)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetItemConentAsync(MailOrder mailOrder)
        {
            return Task<byte[]>.Factory.StartNew(() =>
            {
                if (mailOrder.ConetntFileId == null || mailOrder.ConetntFileId == Guid.Empty)
                {
                    return null;
                }
                return FileCabinetsManager.GetFile(mailOrder.ConetntFileId.Value).Data;
            });
        }

        public Task<byte[]> GetNewMailConentAsync(NewMailType newMailType)
        {
            return Task<byte[]>.Factory.StartNew(() => MailManager.GetNewMailContent(Guid.Empty));
        }

        public Task<IEnumerable<SystemUser>> GetMailUserList()
        {
            return Task<IEnumerable<SystemUser>>.Factory.StartNew(SystemAccountManager.QueryLookUp);
        }
    }
}
