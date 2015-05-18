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
            excludedStoresService = new ExcludedStoresService();
        }

        protected void AddClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(item.Text))
            {
                excludedStoresService.Add(new ExcludedStore { Filter = item.Text.Trim(), Id = Identity.NewIdentity() });
            }

            item.Text = string.Empty;
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
            excludedStoresService.Delete(list.SelectedValue);
        }

        private void LoadData()
        {
            list.DataSource = excludedStoresService.GetAll();
            list.DataBind();
        }
    }
}
