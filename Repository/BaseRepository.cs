using System;
using System.Linq;

namespace Repository
{
    [Obsolete("Use IRepository instead")]
    public abstract class BaseRepository<tt> : IRepository<tt>  where tt: class
    {
        public Type GetHandledType()
        {
            return typeof(tt);
        }


        protected abstract tt FindItem(int id);

        protected abstract IQueryable<tt> GetItemList();

        protected abstract void SaveItem(tt item);

        protected abstract void DeleteItem(tt item);


        public tt Find(int id) 
        {
            return FindItem(id);
        }

        public IQueryable<tt> GetList()
        {
            return GetItemList();
        }

        public void Save(tt item)
        {
            SaveItem(item);
        }

        public void Delete(tt item)
        {
            DeleteItem(item);
        }

    }
}
