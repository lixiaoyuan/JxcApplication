using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor.EntityModels;

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
        /// <summary>
        /// 获取指定日期月份的工资记录集合
        /// </summary>
        /// <returns></returns>
        public Task<IList<Wage>> GetWages(DateTime date)
        {
            return Task<IList<Wage>>.Factory.StartNew(() =>
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return db.Wage.Where(a => DbFunctions.DiffMonths(a.WageDate, date) == 0).OrderByDescending(a=>a.CreateDate).ToList() ;
                }
            });
        }
    }
}
