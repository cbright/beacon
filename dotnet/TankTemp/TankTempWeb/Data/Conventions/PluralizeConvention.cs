using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace TankTempWeb.Data.Conventions
{
    public class PluralizeConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(PluralizationService
                .CreateService(new CultureInfo("en-US")).Pluralize(instance.EntityType.Name));
        }
    }
}