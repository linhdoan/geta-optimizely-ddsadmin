using System.Collections.Generic;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Geta.DdsAdmin.Dds.Interfaces
{
    public interface IStoreService
    {
        PropertyBag Create(string storeName, Dictionary<string, string> values);
        bool Delete(string storeName, Identity id);
        IEnumerable<StoreMetadata> GetAllMetadata(bool filterEnabled);
        StoreMetadata GetMetadata(string storeName);
        bool UpdateCell(string storeName, Identity id, int columnId, string columnName, object value);
    }
}