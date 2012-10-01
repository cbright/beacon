using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace TankTempWeb.Data
{
    public class NHibernateRepository<T> : IRepository<T>
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public T Get(int id)
        {
            return  _sessionFactory.GetCurrentSession().Load<T>(id);
        }

        public void Save(T obj)
        {
            _sessionFactory.GetCurrentSession().SaveOrUpdate(obj);
        }

        public IQueryable<T> Query()
        {
            return _sessionFactory.GetCurrentSession().Query<T>();
        }
    }
}