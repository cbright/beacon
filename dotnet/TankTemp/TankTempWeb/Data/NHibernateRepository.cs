using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace TankTempWeb.Data
{
    public class NHibernateRepository<T> : IRepository<T>
    {
        private readonly ISession _session;

        public NHibernateRepository(ISession session)
        {
            _session = session;
        }

        public T Get(Guid id)
        {
            return _session.Load<T>(id);
        }

        public void Save(T obj)
        {
            _session.SaveOrUpdate(obj);
        }

        public IQueryable<T> Query()
        {
            return _session.Query<T>();
        }
    }
}