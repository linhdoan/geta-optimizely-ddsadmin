using EPiServer.Security;

namespace Geta.DdsAdmin
{
    public class SecurityHelper
    {
        public static bool CheckAccess()
        {
            if (PrincipalInfo.Current != null)
            {
                return PrincipalInfo.Current.HasPathAccess(MenuProvider.RootMenuUri) || PrincipalInfo.HasAdminAccess;
            }

            return false;
        }
    }
}
