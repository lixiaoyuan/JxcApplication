using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JxcApplication.Core.WorkApprovalData
{
    [Serializable]
    public class MealsReimburseData
    {
        public ObservableCollection<MealsReimburseData.Item> Meals { get; set; }

        public MealsReimburseData()
        {
            this.Meals = new ObservableCollection<MealsReimburseData.Item>();
        }

        [Serializable]
        public class Item
        {
            public int Id { get; set; }

            public short Day { get; set; }

            public Decimal DayMoney { get; set; }

            public string Remark { get; set; }
        }
    }
}
