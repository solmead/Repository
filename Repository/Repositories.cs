using System;
using System.Collections.Generic;
using System.Linq;
using HttpObjectCaching;
using Repository.Properties;

namespace Repository
{
    public class Repositories
    {
        public static IEnumerable<IRepositoryBase> GetRepositoryList() 
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from t in assembly.GetTypes()
                            where t.GetInterfaces().Contains(typeof(IRepositoryBase)) &&
                                  t.GetConstructor(Type.EmptyTypes) != null
                            select ((IRepositoryBase)Activator.CreateInstance(t))).ToList();
        }

        public static IRepository<tt> GetRepository<tt>() where tt : class
        {
            var tpe = typeof (tt);

            var list = Cache.GetItem<List<IRepositoryBase>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList",
                () =>
                {
                    return GetRepositoryList().ToList();
                });

            if (list == null || list.Count == 0)
            {
                Cache.SetItem<List<IRepositoryBase>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList", null);
                list = GetRepositoryList().ToList();
            }

            try
            {
                return (from i in list
                        where i.GetHandledType() == tpe && (i as IRepository<tt>) != null
                        select i as IRepository<tt>).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            //    () => 
            //var list = Cache.GetItem<List<IGrapher>>(CacheArea.Global, "GSys_Graphers", 
            //    () => (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //    from t in assembly.GetTypes()
            //    where t.GetInterfaces().Contains(typeof(IGrapher)) &&
            //          t.GetConstructor(Type.EmptyTypes) != null
            //    select ((IGrapher)Activator.CreateInstance(t))).ToList());

            //    Cache.SetItem<List<IGrapher>>(CacheArea.Global, "GSys_Graphers",null);
            //return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //        from t in assembly.GetTypes()
            //        where t.GetInterfaces().Contains(typeof(IRepository<tt>)) &&
            //              t.GetConstructor(Type.EmptyTypes) != null
            //        select ((IRepository<tt>)Activator.CreateInstance(t))).FirstOrDefault();

        }
        public static tRepository GetRepositoryOf<tRepository, tItem>()
            where tRepository : IRepository<tItem>
            where tItem : class
        {
            var tpe = typeof(tItem);
            var repotpe = typeof(tRepository);

            var list = Cache.GetItem<List<IRepositoryBase>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList",
                () =>
                {
                    return GetRepositoryList().ToList();
                });

            if (list == null || list.Count == 0)
            {
                Cache.SetItem<List<IRepositoryBase>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList", null);
                list = GetRepositoryList().ToList();
            }

            try
            {
                return (from i in list
                        where i.GetHandledType() == tpe && (i is tRepository)
                        select (tRepository)i).FirstOrDefault();
            }
            catch (Exception)
            {
                return default(tRepository);
            }

            //    () => 
            //var list = Cache.GetItem<List<IGrapher>>(CacheArea.Global, "GSys_Graphers", 
            //    () => (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //    from t in assembly.GetTypes()
            //    where t.GetInterfaces().Contains(typeof(IGrapher)) &&
            //          t.GetConstructor(Type.EmptyTypes) != null
            //    select ((IGrapher)Activator.CreateInstance(t))).ToList());

            //    Cache.SetItem<List<IGrapher>>(CacheArea.Global, "GSys_Graphers",null);
            //return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //        from t in assembly.GetTypes()
            //        where t.GetInterfaces().Contains(typeof(IRepository<tt>)) &&
            //              t.GetConstructor(Type.EmptyTypes) != null
            //        select ((IRepository<tt>)Activator.CreateInstance(t))).FirstOrDefault();

        }
    }
}
