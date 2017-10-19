using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;
using EPiServer.Data.Dynamic;
using Geta.DdsAdmin.Dds.Interfaces;

namespace Geta.DdsAdmin.Dds.Services
{
    public class ExcludedStoresService : IExcludedStoresService
    {
        private static DynamicDataStore ExcludedStores
        {
            get { return typeof(ExcludedStore).GetStore(); }
        }

        public Identity Add(ExcludedStore excludedStore)
        {
            return ExcludedStores.Save(excludedStore);
        }

        public bool Any()
        {
            // using Count instead of Any cos it is not supported
            return ExcludedStores.Items<ExcludedStore>().Count() > 0;
        }

        public bool Delete(string id)
        {
            Identity identity;
            if (Identity.TryParse(id, out identity))
            {
                ExcludedStores.Delete(identity);
                return true;
            }
            return false;
        }

        public IEnumerable<ExcludedStore> GetAll()
        {
            return ExcludedStores.Items<ExcludedStore>();
        }

        public void Initialize()
        {
            if (Any())
            {
                return;
            }

            // if no records present adding default
            ExcludedStores.Save(new ExcludedStore { Id = Identity.NewIdentity(), Filter = typeof(ExcludedStore).Namespace });
            ExcludedStores.Save(new ExcludedStore { Id = Identity.NewIdentity(), Filter = "EPiServer" });
        }
    }
}
