using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Geta.DdsAdmin.Dds
{
    [EPiServerDataStore(AutomaticallyRemapStore = true, AutomaticallyCreateStore = true)]
    public class ExcludedStore
    {
        public Identity Id { get; set; }
        public string Filter { get; set; }
    }
}
