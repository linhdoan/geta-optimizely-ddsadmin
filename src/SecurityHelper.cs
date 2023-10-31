using EPiServer.Authorization;
using EPiServer.Security;

namespace Geta.DdsAdmin
{
    public static class SecurityHelper
    {
        public static bool CheckAccess()
        {
            return PrincipalInfo.CurrentPrincipal != null && (PrincipalInfo.CurrentPrincipal.IsInRole(Roles.CmsAdmins)
                || PrincipalInfo.CurrentPrincipal.IsInRole(Roles.WebAdmins) || PrincipalInfo.CurrentPrincipal.IsInRole(Roles.Administrators));
        }
    }
}
