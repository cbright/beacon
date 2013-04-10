using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TankTempWeb.Models;
using TankTempWeb.Models.Domain;

namespace TankTempWeb.Data
{
    public class NHibernateRepository<T> : IRepository<T>
    {
        protected readonly ISession Session;

        public NHibernateRepository(ISession session)
        {
            Session = session;
        }

        public T Get(int id)
        {
            return Session.Get<T>(id);
        }

        public void Save(T obj)
        {
            Session.SaveOrUpdate(obj);
        }

        public IQueryable<T> Query()
        {
            return Session.Query<T>();
        }
    }

    public interface ISensorRepository : IRepository<Sensor>
    {
        Sensor GetBySerialNumber(string serialNumber);
        IQueryable<TemperatureObservation> Query(int id);
    }

    public class NHibernateSensorRepository : NHibernateRepository<Sensor>, ISensorRepository
    {
        public NHibernateSensorRepository(ISession session)
            : base(session)
        {
            
        }

        public Sensor GetBySerialNumber(string serialNumber)
        {
            return
                Query().FirstOrDefault(
                    s => s.SerialNumber.Equals(serialNumber, StringComparison.CurrentCultureIgnoreCase));
        }

        public IQueryable<TemperatureObservation> Query(int id)
        {
            return Session.Query<TemperatureObservation>()
                .Where(t => t.Sensor.Id == id);
        }
    }
}