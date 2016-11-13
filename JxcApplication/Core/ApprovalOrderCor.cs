using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.Model;
using BusinessDb.Cor.Business;

namespace JxcApplication.Core
{
    public class ApprovalOrderCor
    {
        public ApprovalOrderCor(string ApprovalUsersXml)
        {
            
        }
        /// <summary>
        /// 创建ApprovalUsers Xml
        /// </summary>
        /// <param name="workApprovalId">审批Id</param>
        /// <param name="nextUserId">下一位审批人Id</param>
        /// <returns></returns>
        public static string CreateApprovalUsers(Guid workApprovalId,out Guid nextUserId)
        {
            DateTime createTime = DBUnit.GetDbTime();
            var approval = WorkApprovalManager.ApprovalUsers(workApprovalId);
            if (approval == null || approval.Length == 0)
            {
                nextUserId = Guid.Empty;
                return null;
            }
            nextUserId = approval[0].Id;
            XDocument document=new XDocument();
            XElement root=new XElement("users");
            foreach (SystemUser user in approval)
            {
                XElement li = new XElement("li");
                li.Add(new XAttribute("id",user.Id),
                    new XAttribute("name",user.Name),
                    new XAttribute("result", WorkApprovalOrderResult.Approvaling),
                    new XAttribute("remark","-"),
                    new XAttribute("modate",createTime));
                root.Add(li);
            }document.Add(root);
            return document.ToString();
        }

        /// <summary>
        /// 创建抄送用户 xml
        /// </summary>
        /// <param name="workApprovalId"></param>
        /// <returns></returns>
        public static string CreateCopyUsers(Guid workApprovalId)
        {
            return null;
        }
    }
}
