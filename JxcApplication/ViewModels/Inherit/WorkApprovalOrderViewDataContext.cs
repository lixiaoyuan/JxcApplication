using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;

namespace JxcApplication.ViewModels.Inherit
{
    public class WorkApprovalOrderViewDataContext
    {
        public string TemplateName { get; set; }
        private WorkApproval _workApproval;
        public WorkApprovalOrder OrderRow { get; set; }

        public object Data { get; set; }
        public WorkApprovalOrderViewDataContext(WorkApprovalOrder order)
        {
            OrderRow = order;
            InitRowTemplate();
        }

        /// <summary>
        /// 初始化Row模板
        /// </summary>
        private void InitRowTemplate()
        {
            if (OrderRow == null || !OrderRow.WorkApprovalId.HasValue)
            {
                TemplateName = "DataTemplateDefault";
                return;
            }
            _workApproval = WorkApprovalManager.Find(OrderRow.WorkApprovalId.Value);
            if (_workApproval == null)
            {
                TemplateName = "DataTemplateDefault";
                return;
            }
            TemplateName = _workApproval.FormDataTemplate;
            using (MemoryStream ms = new MemoryStream(OrderRow.FormData))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Data = bf.Deserialize(ms);
            }
            //var dataContentType = RowData.View.DataContext.GetType();
            //AgreeCommand = dataContentType.GetProperty("AgreeCommand").GetValue(RowData.View.DataContext, null);// as DelegateCommand<WorkApprovalOrder>
            //RefuseCommand = dataContentType.GetProperty("RefuseCommand").GetValue(RowData.View.DataContext, null);
        }

    }
}
