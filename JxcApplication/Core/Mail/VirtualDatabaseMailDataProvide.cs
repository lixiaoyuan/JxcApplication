using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;

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
            throw new NotImplementedException();
        }
    }
}
