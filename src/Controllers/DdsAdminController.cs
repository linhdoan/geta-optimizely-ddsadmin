using ClosedXML.Excel;
using EPiServer.Data;
using Geta.DdsAdmin.Dds;
using Geta.DdsAdmin.Dds.Interfaces;
using Geta.DdsAdmin.Dds.Services;
using Geta.DdsAdmin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;

namespace Geta.DdsAdmin.Controllers
{
    public class DdsAdminController : Controller
    {
        private readonly CrudService _crudService;
        private readonly StoreService _storeService;
        private readonly IExcludedStoresService _excludedStoresService;

        public DdsAdminController(IExcludedStoresService excludedStoresService)
        {
            _storeService = new StoreService(new ExcludedStoresService());
            _crudService = new CrudService(_storeService);
            _excludedStoresService = new ExcludedStoresService();
            _excludedStoresService = excludedStoresService;
        }

        public IActionResult Index()
        {
            var stores = _storeService.GetAllMetadata(true);
            return View(stores);
        }



        public IActionResult Store(string storeName, string heading = "", string message = "")
        {
            var hiddenColumns = WebUtility.HtmlEncode(Request.Query[Constants.HiddenColumnsKey]);
            var store = _storeService.GetMetadata(storeName);
            var model = new StoreViewModel()
            {
                CustomHeading = heading,
                CustomMessage = message,
                StoreMetadata = store,
                StoreName = storeName,
                HiddenColumns = hiddenColumns.Split(new[] { "," },
                    StringSplitOptions.RemoveEmptyEntries).Select(item => Convert.ToInt32(item)).ToArray()
            };
            return View(model);
        }

        public IActionResult FlushStore(string storeName)
        {
            _storeService.Flush(storeName);
            return Store(storeName);
        }

        public IActionResult ExportStore(string storeName)
        {
            var ddsDataSet = GetDdsStoreAsDataSet(storeName);

            using var wb = new XLWorkbook();
            wb.Worksheets.Add(ddsDataSet.Tables[0], "Sheet1");

            using var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{storeName}.xlsx");
        }

        public IActionResult Create(string storeName)
        {
            var values = HttpContext.Request.Form.Keys.ToDictionary(item => item.Substring(5), item => HttpContext.Request.Form[item].ToString());
            var createResponse = _crudService.Create(storeName, values);
            return Json(createResponse);
        }

        public IActionResult Delete(string storeName)
        {
            var id = HttpContext.Request.Form["id"].ToString();
            var deleteResponse = _crudService.Delete(storeName, id);
            return Json(deleteResponse);
        }

        public IActionResult Read(string storeName)
        {
            int start = Convert.ToInt32(HttpContext.Request.Query["iDisplayStart"]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Query["iDisplayLength"]);
            int echo = Convert.ToInt32(HttpContext.Request.Query["sEcho"]);
            string search = HttpContext.Request.Query["sSearch"];
            int sortByColumn = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            string sortDirection = HttpContext.Request.Query["sSortDir_0"];

            var readResponse = _crudService.Read(storeName, start, pageSize, search, sortByColumn, sortDirection);

            var result = new
            {
                sEcho = echo,
                iTotalRecords = readResponse.TotalCount,
                iTotalDisplayRecords = readResponse.TotalCount,
                aaData = readResponse.Data
            };

            return Json(result);
        }

        public IActionResult Update(HttpContext context, string storeName)
        {
            int columnId = Convert.ToInt32(context.Request.Form["columnId"]);
            string columnName = context.Request.Form["columnName"];
            string id = context.Request.Form["id"];
            string value = context.Request.Form["value"];

            var updateResponse = _crudService.Update(storeName, columnId, value, id, columnName);
            return Json(updateResponse);
        }

        public IActionResult ExcludedStores()
        {
            return View(_excludedStoresService.GetAll());
        }

        public IActionResult AddFilter(string filter)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                _excludedStoresService.Add(new ExcludedStore
                {
                    Filter = filter,
                    Id = Identity.NewIdentity()
                });
            }

            return ExcludedStores();
        }

        public IActionResult DeleteFilter(string selected)
        {
            _excludedStoresService.Delete(selected);
            return ExcludedStores();
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
