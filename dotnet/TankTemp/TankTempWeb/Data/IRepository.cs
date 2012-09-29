using System;
using System.Linq;

namespace TankTempWeb.Data
{
    public interface IRepository<T>
    {
        T Get(Guid id);

        void Save(T obj);

        IQueryable<T> Query();
    }
}