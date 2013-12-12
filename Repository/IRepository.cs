using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace DiversityPortal.Data.Repository
{
    public interface IRepository
    {
        Type GetHandledType();
    }
    //public interface IRepository<tt>
    //{
    //    tt Find(int id);
    //    IQueryable<tt> GetList();
    //    void Save(tt item);
    //    void Delete(tt item);
    //}
}
