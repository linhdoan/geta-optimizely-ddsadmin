using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using EPiServer.Data.Dynamic;
using EPiServer.UI;
using Geta.DdsAdmin.Dds;
using Geta.DdsAdmin.Dds.Services;

namespace Geta.DdsAdmin.Admin
{
    public partial class DdsAdmin : SystemPageBase
    {
        private int[] HiddenColumns { get; set; }
        protected string CurrentStoreName { get; set; }
        protected string CustomHeading { get; set; }
        protected string CustomMessage { get; set; }
        protected PropertyMap Item { get; set; }
        protected StoreMetadata Store { get; set; }

        protected string GetColumnsScript()
        {
            return DdsAdminScriptHelper.GetColumns(Store.Columns.ToList(), HiddenColumns);
        }

        protected string GetInvisibleColumnsScript()
        {
            return DdsAdminScriptHelper.GetInvisibleColumns(HiddenColumns);
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

            GetQueryStringParameters();
            LoadAndDisplayData();
        }

        protected string SetItem(RepeaterItem repeaterItem)
        {
            Item = repeaterItem.DataItem as PropertyMap;
            return string.Empty;
        }

        private void GetQueryStringParameters()
        {
            CurrentStoreName = HttpUtility.HtmlEncode(Request.QueryString[Constants.StoreKey]);
            CustomHeading = HttpUtility.HtmlEncode(Request.QueryString[Constants.HeadingKey]);
            CustomMessage = HttpUtility.HtmlEncode(Request.QueryString[Constants.MessageKey]);

            var hiddenColumns = HttpUtility.HtmlEncode(Request.QueryString[Constants.HiddenColumnsKey]);
            if (string.IsNullOrEmpty(hiddenColumns))
            {
                HiddenColumns = new int[0];
                return;
            }
            HiddenColumns =
                hiddenColumns.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(item => Convert.ToInt32(item)).ToArray();
        }

        private void LoadAndDisplayData()
        {
            if (string.IsNullOrEmpty(CurrentStoreName))
            {
                hdivNoStoreTypeSelected.Visible = true;
                return;
            }

            var storeService = new StoreService(new ExcludedStoresService());
            Store = storeService.GetMetadata(CurrentStoreName);

            if (Store == null)
            {
                hdivStoreTypeDoesntExist.Visible = true;
                return;
            }

            hdivStoreTypeSelected.Visible = true;

            repColumnsHeader.DataSource = Store.Columns;
            repForm.DataSource = Store.Columns;
            repColumnsHeader.DataBind();
            repForm.DataBind();
        }

        protected void FlushStore(object sender, EventArgs e)
        {
            var storeName = Request.Form["CurrentStoreName"];
            var storeService = new StoreService(new ExcludedStoresService());
            storeService.Flush(storeName);

            Response.Redirect(Request.RawUrl);
        }
    }
}
