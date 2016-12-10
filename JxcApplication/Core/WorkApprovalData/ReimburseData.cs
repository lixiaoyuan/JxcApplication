using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JxcApplication.Core.WorkApprovalData
{
    /// <summary>
    /// 报销
    /// </summary>
    [Serializable]
    public class ReimburseData
    {
        public ObservableCollection<ReimburseData.Item> Tickets { get; set; }

        public ReimburseData()
        {
            this.Tickets = new ObservableCollection<ReimburseData.Item>();
        }

        [Serializable]
        public class Item
        {
            public int Id { get; set; }

            public string TicketName { get; set; }

            public string TicketRoute { get; set; }

            public Decimal TicketMoney { get; set; }

            public short TicketQuan { get; set; }
        }
    }
}
