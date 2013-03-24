using System;
using System.Linq;
using TankTempWeb.Models;

namespace TankTempWeb.Data
{
    public interface IRepository<T>
    {
        T Get(int id);

        void Save(T obj);

        IQueryable<T> Query();
    }
}