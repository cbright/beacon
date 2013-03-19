using System;
using System.Web.Mvc;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Ninject;
using Ninject.Web.Common;
using TankTempWeb.Models;

namespace TankTempWeb.Data
{
    public class NHibernateModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISessionFactory>().ToMethod(
                s => Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(
                    x => x.FromConnectionStringWithKey("tanktemp")))
                         .Mappings(m => m.AutoMappings.Add(
                             AutoMap.AssemblyOf<TempatureSensor>().IgnoreBase(typeof(Sensor))))
                         .CurrentSessionContext("managed_web")
                         .ExposeConfiguration(c => {
                             new SchemaExport(c)
                                 .SetOutputFile("script.sql")
                                 .Execute(true, false, false);
                             
                         })
                         .BuildSessionFactory()).InSingletonScope();
            Kernel.Bind(typeof(IRepository<>)).To(typeof(NHibernateRepository<>));

            //Let NHibernate manage the ISessionScope
            Kernel.Bind<ISession>()
                .ToMethod(c => c.Kernel.Get<ISessionFactory>().GetCurrentSession()).InTransientScope();
        }


    }
}