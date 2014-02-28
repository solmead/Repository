using System;
using System.Collections.Generic;
using System.Linq;
using HttpObjectCaching;
using Repository.Properties;

namespace Repository
{
    public class Repositories
    {
        private static IEnumerable<IRepositoryBase> GetBaseRepositoryList() 
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from t in assembly.GetTypes()
                            where t.GetInterfaces().Contains(typeof(IRepositoryBase)) &&
                                  t.GetConstructor(Type.EmptyTypes) != null
                            select ((IRepositoryBase)Activator.CreateInstance(t))).ToList();
        }

        private static List<IRepositoryBase> GetRepositoryList()
        {
            var list = Cache.GetItem<List<IRepositoryBase>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList",
                () =>
                {
                    return GetBaseRepositoryList().ToList();
                });

            if (list == null || list.Count == 0)
            {
                Cache.SetItem<List<IRepositoryBase>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList", null);
                list = GetBaseRepositoryList().ToList();
            }
            return list;
        }

        public static IRepository<tt> GetRepository<tt>() where tt : class
        {
            var tpe = typeof (tt);

            try
            {
                return (from i in GetRepositoryList()
                        where i.GetHandledType() == tpe && (i as IRepository<tt>) != null
                        select i as IRepository<tt>).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static tRepository GetRepositoryOf<tRepository>()
            where tRepository : IRepositoryBase
        {
            try
            {
                return (from i in GetRepositoryList()
                        where (i is tRepository)
                        select (tRepository)i).FirstOrDefault();
            }
            catch (Exception)
            {
                return default(tRepository);
            }
        }
        public static tRepository GetRepositoryOf<tRepository, tItem>()
            where tRepository : IRepository<tItem>
            where tItem : class
        {
            var tpe = typeof(tItem);
            var repotpe = typeof(tRepository);

            try
            {
                return (from i in GetRepositoryList()
                        where i.GetHandledType() == tpe && (i is tRepository)
                        select (tRepository)i).FirstOrDefault();
            }
            catch (Exception)
            {
                return default(tRepository);
            }

        }
    }
}
