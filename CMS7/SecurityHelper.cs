using EPiServer.Security;

namespace Geta.DdsAdmin
{
    public static class SecurityHelper
    {
        public static bool CheckAccess()
        {
            return PrincipalInfo.Current != null && PrincipalInfo.HasAdminAccess;
        }
    }
}
