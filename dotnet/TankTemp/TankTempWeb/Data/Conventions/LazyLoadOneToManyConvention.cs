using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace TankTempWeb.Data.Conventions
{
    public class LazyLoadOneToManyConvention : IHasOneConvention, IHasManyConvention, IReferenceConvention
    {

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.LazyLoad();
            instance.Cascade.AllDeleteOrphan();
        }

        public void Apply(IOneToOneInstance instance)
        {
            instance.Cascade.Delete();
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.Delete();
        }
    }
}