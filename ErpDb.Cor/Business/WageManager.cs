using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Business
{
    public  class WageManager
    {
        /// <summary>
        /// 判断这个月是否有记录工资单
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool HasExisWage(DateTime date)
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                return db.Wage.FirstOrDefault(a => DbFunctions.DiffMonths(a.WageDate, date) == 0) != null;
            }
        }
    }
}
