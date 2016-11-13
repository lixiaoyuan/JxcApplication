using System.Data.Entity;

namespace BusinessDb.Cor.Business
{
    public static class Extend
    {
        /// <summary>
        /// 移除缓存，置未跟踪
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DbSet<T> Detacheds<T>(this DbSet<T> source, DbContext context) where T : class
        {
            while (source.Local.Count > 0)
            {
                context.Entry(source.Local[0]).State = EntityState.Detached;

            }

            return source;
        }
    }
}
