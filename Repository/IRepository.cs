using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<tt> : IRepositoryBase where tt : class
    {
        tt Find(int id);

        IQueryable<tt> GetList();

        void Save(tt item);

        void Delete(tt item);
    }
}
