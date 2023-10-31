using System;
using System.Collections.Generic;
using EPiServer.Shell;
using EPiServer.Shell.Navigation;
using Geta.DdsAdmin;
using Microsoft.AspNetCore.Http;

namespace Geta.DdsAdmin
{
    [MenuProvider]
    public class MenuProvider : IMenuProvider
    {
        private const string GetaTopMenuIsSetKey = "GetaTopMenuIsSet";
        private const string ParentPath = MenuPaths.Global + "/geta";

        private static readonly HttpContextAccessor HttpContextAccessor = new HttpContextAccessor();

        public IEnumerable<MenuItem> GetMenuItems()
        {
            var menuItems = new List<MenuItem>();

            if (!Convert.ToBoolean(HttpContextAccessor.HttpContext?.Items[GetaTopMenuIsSetKey]))
            {
                var mainMenu = new SectionMenuItem("Geta", ParentPath) { IsAvailable = CheckAccess };
                menuItems.Add(mainMenu);
                if (HttpContextAccessor.HttpContext != null)
                {
                    HttpContextAccessor.HttpContext.Items[GetaTopMenuIsSetKey] = true;
                }
            }

            var adminItem = new UrlMenuItem("DDS Admin", ParentPath + "/ddsadmin", Paths.ToResource(typeof(MenuProvider), "/DdsAdmin/Index"))
                                {
                                    IsAvailable = CheckAccess
                                };

            menuItems.Add(adminItem);

            return menuItems;
        }

        private bool CheckAccess(HttpContext context)
        {
            return SecurityHelper.CheckAccess();
        }
    }
}
