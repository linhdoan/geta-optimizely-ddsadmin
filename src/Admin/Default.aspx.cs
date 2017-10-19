using System;
using EPiServer.Shell.Navigation;
using EPiServer.UI;

namespace Geta.DdsAdmin.Admin
{
    [MenuSection(MenuPaths.Global + "/geta/dds_admin")]
    public partial class Default : SystemPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!SecurityHelper.CheckAccess())
            {
                AccessDenied();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MasterPageFile = ResolveUrlFromUI("MasterPages/Frameworks/Framework.master");
        }
    }
}
