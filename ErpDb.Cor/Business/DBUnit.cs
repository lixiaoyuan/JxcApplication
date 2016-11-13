using System;
using System.Linq;

namespace BusinessDb.Cor.Business
{
    public static class DBUnit
    {
        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDbTime()
        {
            using (ApplicationDbContext entities =new ApplicationDbContext())
            {
                return entities.Database.SqlQuery<DateTime>("SELECT GETDATE()").FirstOrDefault();
            }
        }
    }
}
