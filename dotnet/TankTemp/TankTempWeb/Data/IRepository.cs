using System;
using System.Linq;

namespace TankTempWeb.Data
{
    public interface IRepository<T>
    {
        T Get(int id);

        void Save(T obj);

        IQueryable<T> Query();
    }
}