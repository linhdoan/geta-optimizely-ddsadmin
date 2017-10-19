using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;

namespace Geta.DdsAdmin.Dds
{
    public class StoreMetadata
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public IEnumerable<PropertyMap> Columns { get; set; }

        public PropertyMap GetColumnMetadata(string columnName)
        {
            return Columns.First(x => x.PropertyName == columnName);
        }
    }
    
    public static class StoreMetadataExtensions
    {
        public static StoreMetadata Metadata(this DynamicDataStore store)
        {
            return new StoreMetadata
                       {
                           Name = store.Name,
                           Columns = store.StoreDefinition.ActiveMappings
                       };
        }
    }
}
