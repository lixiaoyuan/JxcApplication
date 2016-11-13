using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.Business
{
    public class ZhangtaoManager
    {
        private ApplicationDbContext _entities;
        public static ZhangtaoManager Create()
        {
            return new ZhangtaoManager();
        }

        public ZhangtaoManager()
        {
            _entities = new ApplicationDbContext();
        }

        public string GetConnectString(Guid id)
        {
            var result = _entities.Database.SqlQuery(typeof(string), "SELECT top 1 ConnectionString FROM dbo.Zhangtao WHERE Id=@id", new SqlParameter("id", id)).GetEnumerator();
            while (result.MoveNext())
            {
                return result.Current.ToString();
            }
            return null;
        }
    }
}
