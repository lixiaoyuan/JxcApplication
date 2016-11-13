using System;
using System.Linq;
using ApplicationDb.Cor;

namespace ApplicationDb.Cor.Helper
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
