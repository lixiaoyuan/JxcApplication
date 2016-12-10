using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor;
using ApplicationDb.Cor.EntityModels;
using JxcApplication.ViewModels.Mail;

namespace JxcApplication.Core.Mail
{
    public interface IMailDataProvide
    {
        /// <summary>
        /// 获取邮件列表
        /// </summary>
        /// <param name="type">邮件列表类型</param>
        /// <returns></returns>
        Task<List<MailOrder>> GetItemListAsync(MailListShowType type);
        /// <summary>
        /// 更新邮件
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateItemAsync(MailOrder MailOrder);
        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <returns></returns>
        Task<bool> ReomverItemAsync(MailOrder mailOrder);
        /// <summary>
        /// 创建邮件
        /// </summary>
        /// <returns></returns>
        Task<bool> CreateItemAsync(byte[] content,params MailOrder[] mailOrder);
        /// <summary>
        /// 获取邮件内容
        /// </summary>
        /// <returns></returns>
        Task<byte[]> GetItemConentAsync(MailOrder mailOrder);
        /// <summary>
        /// 获取新建邮件内容
        /// </summary>
        /// <returns></returns>
        Task<byte[]> GetNewMailConentAsync(NewMailType newMailType);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        Task<List<SystemUser>> GetMailUserList();
    }
}
