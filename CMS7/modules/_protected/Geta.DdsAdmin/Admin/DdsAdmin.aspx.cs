using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Data.Dynamic;
using EPiServer.Shell;
using EPiServer.UI;
using Geta.DdsAdmin.Dds;
using Geta.DdsAdmin.Dds.Services;

namespace Geta.DdsAdmin.Admin
{
    public partial class DdsAdmin : SystemPageBase
    {
        private readonly StoreService _storeService;
        private readonly CrudService _crudService;

        public DdsAdmin()
        {
            _storeService = new StoreService(new ExcludedStoresService());
            _crudService = new CrudService(_storeService);
        }

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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.Header.Controls.Add(new Literal
            {
                Text = "<link type=\"text/css\" rel=\"stylesheet\" href=\"" + Paths.ToClientResource(typeof (MenuProvider), "content/themes/DDSAdmin/custom/minified.css") + "\" />"
            });

            Page.Header.Controls.Add(new Literal
            {
                Text = "<script src=\"" + Paths.ToClientResource(typeof (MenuProvider), "scripts/datatables-1.9.4/media/js/jquery.dataTables.min.js") + "\"></script>"
            });

            Page.Header.Controls.Add(new Literal
            {
                Text = "<script src=\"" + Paths.ToClientResource(typeof (MenuProvider), "scripts/dataTables.jeditable.min.js") + "\"></script>"
            });
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
            Store = _storeService.GetMetadata(CurrentStoreName);

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

            HiddenColumns = hiddenColumns.Split(new[]
            {
                ","
            },
                                                StringSplitOptions.RemoveEmptyEntries).Select(item => Convert.ToInt32(item)).ToArray();
        }

        private void LoadAndDisplayData()
        {
            if (string.IsNullOrEmpty(CurrentStoreName))
            {
                hdivNoStoreTypeSelected.Visible = true;
                return;
            }

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
            _storeService.Flush(storeName);

            Response.Redirect(Request.RawUrl);
        }

        protected void ExportStore(object sender, EventArgs e)
        {
            var storeName = Request.Form["CurrentStoreName"];

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.Unicode;
            Response.AddHeader("content-disposition", $"fileattachment;filename={storeName}.xls");
            Response.BinaryWrite(new[]
            {
                byte.MaxValue, (byte) 254
            });
            DataSet ddsDataSet = GetDdsStoreAsDataSet(storeName);

            using (var stringWriter = new StringWriter())
            {
                using (var writer = new HtmlTextWriter(stringWriter))
                {
                    var dataGrid = new DataGrid
                    {
                        DataSource = ddsDataSet.Tables[0],
                        GridLines = GridLines.Both
                    };

                    dataGrid.HeaderStyle.Font.Bold = true;
                    dataGrid.AllowPaging = false;
                    dataGrid.DataBind();
                    dataGrid.RenderControl(writer);
                    Response.Write(stringWriter.ToString());
                    Response.End();
                }
            }
        }

        private DataSet GetDdsStoreAsDataSet(string storeName)
        {
            var dataTable = new DataTable("record");

            var columns = _storeService.GetMetadata(storeName);

            foreach (var column in columns.Columns)
            {
                dataTable.Columns.Add(column.PropertyName, typeof(string));
            }

            var allRecords = _crudService.Read(storeName, 0, int.MaxValue, null, 0, null);

            if (allRecords == null || !allRecords.Success || allRecords.TotalCount == 0)
            {
                return null;
            }

            foreach (var record in allRecords.Data)
            {
                var row = dataTable.NewRow();

                var columMap = columns.Columns.ToArray();

                for (var i = 0; i < columMap.Length; i++)
                {
                    var column = columMap[i];
                    row[column.PropertyName] = record[i + 1];

                }

                dataTable.Rows.Add(row);
            }

            return new DataSet
            {
                Tables =
                {
                    dataTable
                }
            };
        }
    }
}
