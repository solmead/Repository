using System;
using System.Collections.Generic;
using System.Linq;
using HttpObjectCaching;
using Repository.Properties;

namespace Repository
{
    public class Repositories
    {
        public static IEnumerable<IRepository> GetRepositoryList() 
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from t in assembly.GetTypes()
                            where t.GetInterfaces().Contains(typeof(IRepository)) &&
                                  t.GetConstructor(Type.EmptyTypes) != null
                            select ((IRepository)Activator.CreateInstance(t))).ToList();
        }

        public static BaseRepository<tt> GetRepository<tt>() where tt:class
        {
            var tpe = typeof (tt);

            var list = Cache.GetItem<List<IRepository>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList",
                () =>
                {
                    return GetRepositoryList().ToList();
                });

            if (list == null || list.Count == 0)
            {
                Cache.SetItem<List<IRepository>>(Settings.Default.RepositoriesCachedWhere, "RepositoryList", null);
                list = GetRepositoryList().ToList();
            }

            try
            {
                return (from i in list
                        where i.GetHandledType() == tpe && (i as BaseRepository<tt>) != null
                        select i as BaseRepository<tt>).FirstOrDefault();
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

    }
}
