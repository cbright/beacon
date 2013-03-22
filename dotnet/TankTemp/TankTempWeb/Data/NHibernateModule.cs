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
            var sessionFactory =
                Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(
                    x => x.FromConnectionStringWithKey("tanktemp")))
                    .Mappings(m => m.AutoMappings.Add(
                        AutoMap.AssemblyOf<Sensor>(t => t.Namespace == "TankTempWeb.Models")
                            .Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never())))
                    .CurrentSessionContext("managed_web")
                    .ExposeConfiguration(c => new SchemaExport(c)
                                                  .SetOutputFile(@"C:\script.sql")
                                                  .Execute(true, false, false)).BuildSessionFactory();

            Kernel.Bind<ISessionFactory>().ToConstant(sessionFactory).InSingletonScope();
            Kernel.Bind(typeof(IRepository<>)).To(typeof(NHibernateRepository<>));

            //Let NHibernate manage the ISessionScope
            Kernel.Bind<ISession>()
                .ToMethod(c => c.Kernel.Get<ISessionFactory>().GetCurrentSession()).InTransientScope();
        }


    }
}