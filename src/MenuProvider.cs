using EPiServer.Shell.Navigation;
using System.Collections.Generic;

namespace Geta.DdsAdmin
{
    [MenuProvider]
    public class MenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var listMenuItem = new UrlMenuItem("DDS Admin", "/global/cms/ddsadmin", "/DdsAdmin")
            {
                IsAvailable = context => true,
                SortIndex = SortIndex.Last + 1,
                AuthorizationPolicy = Constants.AuthorizationPolicy
            };

            return new List<MenuItem> { listMenuItem };
        }
    }
}
