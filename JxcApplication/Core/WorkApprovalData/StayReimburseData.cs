using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JxcApplication.Core.WorkApprovalData
{
    [Serializable]
    public class StayReimburseData
    {
        public ObservableCollection<StayReimburseData.Item> Stays { get; set; }

        public StayReimburseData()
        {
            this.Stays = new ObservableCollection<StayReimburseData.Item>();
        }

        [Serializable]
        public class Item
        {
            public int Id { get; set; }

            public string HotelName { get; set; }

            public short DayTimes { get; set; }

            public Decimal DayMoney { get; set; }
        }
    }
}
