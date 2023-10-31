using System;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Geta.DdsAdmin
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Shell.UI.InitializationModule))]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class InitializationModule : IConfigurableModule
    {
        protected ServiceConfigurationContext ServiceConfigurationContext;

        public void ConfigureContainer(
            ServiceConfigurationContext serviceConfigurationContext)
        {
            ServiceConfigurationContext = serviceConfigurationContext;
            serviceConfigurationContext.Services.Configure<ProtectedModuleOptions>(
                options =>
                {
                    // Register shell module
                    if (options.Items.Any(
                            x => x.Name.Equals("Geta.DdsAdmin")))
                        return;
                    var moduleDetails = new ModuleDetails
                    {
                        Name = "Geta.DdsAdmin",
                        Assemblies = { "Geta.DdsAdmin" }
                    };
                    options.Items.Add(moduleDetails);
                });
            serviceConfigurationContext.Services.Configure(
                (Action<RazorViewEngineOptions>)(ro =>
                {
                    if (ro.ViewLocationExpanders.Any(
                            v =>
                                v.GetType() == typeof(ModuleLocationExpander)))
                        return;
                    ro.ViewLocationExpanders.Add(
                        new ModuleLocationExpander());
                }));
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
