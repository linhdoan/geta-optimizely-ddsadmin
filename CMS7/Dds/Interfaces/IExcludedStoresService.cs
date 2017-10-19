using System.Collections.Generic;
using EPiServer.Data;

namespace Geta.DdsAdmin.Dds.Interfaces
{
    public interface IExcludedStoresService
    {
        Identity Add(ExcludedStore excludedStore);
        bool Any();
        bool Delete(string id);
        IEnumerable<ExcludedStore> GetAll();
        void Initialize();
    }
}
