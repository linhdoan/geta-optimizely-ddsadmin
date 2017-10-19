using System;
using EPiServer;
using EPiServer.Shell.WebForms;
using Geta.DdsAdmin.Dds;
using Geta.DdsAdmin.Dds.Services;

namespace Geta.DdsAdmin.Admin
{
    public partial class Menu : WebFormsBase
    {
        protected StoreMetadata Item
        {
            get { return Page.GetDataItem() as StoreMetadata; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack)
            {
                return;
            }

            LoadData();
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        private void LoadData()
        {
            var storeService = new StoreService(new ExcludedStoresService());
            var stores = storeService.GetAllMetadata(true);

            repStoreTypes.DataSource = stores;
            repStoreTypes.DataBind();
        }
    }
}
