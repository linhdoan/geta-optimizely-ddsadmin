using System;
using EPiServer.UI;
using Geta.DdsAdmin.Dds;
using Geta.DdsAdmin.Dds.Services;

namespace Geta.DdsAdmin.Admin
{
    public partial class Menu : SystemPageBase
    {
        protected StoreMetadata Item
        {
            get { return Page.GetDataItem() as StoreMetadata; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!SecurityHelper.CheckAccess())
            {
                AccessDenied();
            }

            if (IsPostBack)
            {
                return;
            }

            LoadData();
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MasterPageFile = ResolveUrlFromUI("MasterPages/EPiServerUI.master");
        }

        private void LoadData()
        {
            var storeService = new StoreService(new ExcludedStoresService());
            var stores = storeService.GetAllMetadata(true);

            this.repStoreTypes.DataSource = stores;
            this.repStoreTypes.DataBind();
        }
    }
}
