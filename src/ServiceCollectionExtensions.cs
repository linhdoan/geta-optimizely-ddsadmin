using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using Geta.DdsAdmin.Dds.Interfaces;
using Geta.DdsAdmin.Dds.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Geta.DdsAdmin
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOptimizelyDdsAdmin(this IServiceCollection services, Action<AuthorizationOptions> authorizationOptions = null)
        {
            services.AddSingleton<IExcludedStoresService, ExcludedStoresService>();
            services.AddSingleton<ICrudService, CrudService>();
            services.AddSingleton<IStoreService, StoreService>();

            services.Configure<ProtectedModuleOptions>(
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
            services.Configure(
                (Action<RazorViewEngineOptions>)(ro =>
                {
                    if (ro.ViewLocationExpanders.Any(
                            v =>
                                v.GetType() == typeof(ModuleLocationExpander)))
                        return;
                    ro.ViewLocationExpanders.Add(
                        new ModuleLocationExpander());
                }));

            // Authorization
            if (authorizationOptions != null)
            {
                services.AddAuthorization(authorizationOptions);
            }
            else
            {
                var allowedRoles = new List<string> { "CmsAdmins", "Administrator", "WebAdmins" };
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(Constants.AuthorizationPolicy, policy =>
                    {
                        policy.RequireRole(allowedRoles);
                    });
                });
            }
        }
    }
}
