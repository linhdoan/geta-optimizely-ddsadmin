using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using EPiServer.Shell.Navigation;

namespace Geta.DdsAdmin
{
    [MenuProvider]
    public class MenuProvider : IMenuProvider
    {
        public const string RootMenuUri = "/Modules/Geta.DdsAdmin";
        private const string GetaTopMenuIsSetKey = "GetaTopMenuIsSet";
        private const string ParentPath = MenuPaths.Global + "/geta";

        public IEnumerable<MenuItem> GetMenuItems()
        {
            var menuItems = new List<MenuItem>();

            if (!Convert.ToBoolean(HttpContext.Current.Items[GetaTopMenuIsSetKey]))
            {
                var mainMenu = new SectionMenuItem("Geta", ParentPath) { IsAvailable = CheckAccess };
                menuItems.Add(mainMenu);
                HttpContext.Current.Items[GetaTopMenuIsSetKey] = true;
            }

            var adminItem = new UrlMenuItem("DDS Admin", ParentPath + "/dds_admin", RootMenuUri + "/Admin/Default.aspx")
                                {
                                    IsAvailable = CheckAccess
                                };
            menuItems.Add(adminItem);

            return menuItems;
        }

        protected bool CheckAccess(RequestContext requestContext)
        {
            return SecurityHelper.CheckAccess();
        }
    }
}
