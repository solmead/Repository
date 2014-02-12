using System;
using System.Linq;

namespace Repository
{
    public interface IRepositoryBase
    {
        Type GetHandledType();
    }
}
