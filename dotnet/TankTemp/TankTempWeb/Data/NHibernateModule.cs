using System.Web.Mvc;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Ninject;
using Ninject.Web.Common;
using TankTempWeb.Data.Conventions;
using TankTempWeb.Models;
using TankTempWeb.Models.Domain;
using System.Data.Entity.Design;

namespace TankTempWeb.Data
{
    public class NHibernateModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            var tankTempMapping = new TankTempConfiguration();
            var sessionFactory =
                Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(
                    x => x.FromConnectionStringWithKey("tanktemp")))
                    .Mappings(m => m.AutoMappings.Add(
                        AutoMap.AssemblyOf<Sensor>(tankTempMapping)
                            .IncludeBase(typeof (Sensor))
                            .Conventions.AddFromAssemblyOf<PluralizeConvention>()
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