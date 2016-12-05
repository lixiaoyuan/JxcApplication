using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.Model;

namespace ApplicationDb.Cor.EntityModels
{
    public class MailOrder
    {
        public virtual Guid Id { get; set; }
        public virtual bool IsDelete { get; set; }
        public virtual bool IsDaft { get; set; }
        public virtual bool IsReply { get; set; }
        public virtual bool IsUnread { get; set; }
        public virtual Guid? ReplyMailId { get; set; }
        public virtual Guid? FormUser { get; set; }
        public virtual string FormAddress { get; set; }
        public virtual string FormUserName { get; set; }
        public virtual Guid? ToUser { get; set; }
        public virtual string ToAddress { get; set; }
        public virtual string ToUserName { get; set; }
        public virtual DateTime CreateDateTime { get; set; }
        public virtual string Subject { get; set; }
        public virtual Guid? ConetntFileId { get; set; }
        public virtual MailEditPreviewType EditPreviewType { get; set; }

    }
}
