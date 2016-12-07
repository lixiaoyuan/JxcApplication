using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor;
using ApplicationDb.Cor.EntityModels;

namespace JxcApplication.Core.Mail
{
    public interface IMailDataProvide
    {
        Task<IEnumerable<MailOrder>> GetItemAsync(MailListShowType type);
        Task<bool> UpdateItemAsync(MailOrder MailOrder);
        Task<bool> ReomverItemAsync(MailOrder mailOrder);
        Task<bool> CreateItemAsync(MailOrder mailOrder);
        Task<bool> CreateItemAsync(MailOrder mailOrder, byte[] content);
        Task<byte[]> GetItemConentAsync(MailOrder mailOrder);
        Task<IEnumerable<SystemUser>> GetMailUserList();
    }
}
