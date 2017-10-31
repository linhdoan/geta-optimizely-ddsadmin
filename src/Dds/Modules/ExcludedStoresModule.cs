using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Geta.DdsAdmin.Dds.Interfaces;
using Geta.DdsAdmin.Dds.Services;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Geta.DdsAdmin.Dds.Modules
{
    [InitializableModule]
    [ModuleDependency((typeof(InitializationModule)))]
    public class ExcludedStoresModule : IInitializableModule
    {
        private readonly IExcludedStoresService excludedStoresService = new ExcludedStoresService();

        public void Initialize(InitializationEngine context)
        {
            this.excludedStoresService.Initialize();
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
