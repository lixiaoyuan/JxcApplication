using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Utilities;

namespace ApplicationDb.Cor.Helper
{
    public static class ConnectStringHelper
    {
        public static string ConnectString
        {
            get
            {
#if DEBUG
                return "data source=.;initial catalog=ApplicationDb;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework";
#endif
#pragma warning disable 162
                DESCryp desCryp = new DESCryp();
                return desCryp.Decrypt(ConfigurationManager.ConnectionStrings["ApplicationDbContext"].ConnectionString, "1qaz!QAZ");
#pragma warning restore 162
            }
        }

    }
}
