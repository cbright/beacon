using System;
using FluentNHibernate.Automapping;

namespace TankTempWeb.Data.Conventions
{
    public class TankTempConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "TankTempWeb.Models.Domain";
        }

        public override bool IsDiscriminated(Type type)
        {
            return true;
        }
    }
}