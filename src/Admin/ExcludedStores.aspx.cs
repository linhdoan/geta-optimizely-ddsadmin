using System;
using EPiServer.Data;
using EPiServer.UI;
using Geta.DdsAdmin.Dds;
using Geta.DdsAdmin.Dds.Interfaces;
using Geta.DdsAdmin.Dds.Services;

namespace Geta.DdsAdmin.Admin
{
    public partial class ExcludedStores : SystemPageBase
    {
        private readonly IExcludedStoresService excludedStoresService;

        public ExcludedStores()
        {
            this.excludedStoresService = new ExcludedStoresService();
        }

        protected void AddClick(object sender, EventArgs e)
        {
            this.excludedStoresService.Add(new ExcludedStore { Filter = this.item.Text.Trim(), Id = Identity.NewIdentity() });
            this.item.Text = string.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!SecurityHelper.CheckAccess())
            {
                AccessDenied();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            LoadData();
        }

        protected void RemoveClick(object sender, EventArgs e)
        {
            this.excludedStoresService.Delete(this.list.SelectedValue);
        }

        private void LoadData()
        {
            this.list.DataSource = this.excludedStoresService.GetAll();
            this.list.DataBind();
        }
    }
}
