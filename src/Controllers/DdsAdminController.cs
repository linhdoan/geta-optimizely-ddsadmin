using ClosedXML.Excel;
using EPiServer.Data;
using Geta.DdsAdmin.Dds;
using Geta.DdsAdmin.Dds.Interfaces;
using Geta.DdsAdmin.Dds.Services;
using Geta.DdsAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Geta.DdsAdmin.Controllers
{
    [Authorize(Policy = Constants.AuthorizationPolicy)]
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

        [Route("/DdsAdmin/")]
        public IActionResult Index()
        {
            var stores = _storeService.GetAllMetadata(true);
            return View(stores);
        }


        [Route("/DdsAdmin/[action]")]
        public IActionResult Store(string store, string heading = "", string message = "")
        {
            var hiddenColumns = WebUtility.HtmlEncode(Request.Query[Constants.HiddenColumnsKey]);
            var ddsStore = _storeService.GetMetadata(store);
            var model = new StoreViewModel()
            {
                CustomHeading = heading,
                CustomMessage = message,
                StoreMetadata = ddsStore,
                StoreName = store,
                HiddenColumns = hiddenColumns.Split(new[] { "," },
                    StringSplitOptions.RemoveEmptyEntries).Select(item => Convert.ToInt32(item)).ToArray()
            };
            return View(model);
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult FlushStore(string storeName)
        {
            _storeService.Flush(storeName);
            return RedirectToAction("Store", new {store = storeName, hideColumns = 0});
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult ExportStore(string storeName)
        {
            var ddsDataSet = GetDdsStoreAsDataSet(storeName);

            using var wb = new XLWorkbook();
            wb.Worksheets.Add(ddsDataSet.Tables[0], "Sheet1");

            var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{storeName}.xlsx");
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult Create(string store)
        {
            var values = HttpContext.Request.Form.Keys.ToDictionary(item => item.Substring(5), item => HttpContext.Request.Form[item].ToString());
            var createResponse = _crudService.Create(store, values);
            if (createResponse.Success)
            {
                return Ok(createResponse.Response);
            }
            else
            {
                return BadRequest(createResponse.Response);
            }
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult Delete(string store)
        {
            var id = HttpContext.Request.Form["id"].ToString();
            var deleteResponse = _crudService.Delete(store, id);
            return Ok(deleteResponse.Success ? "ok" : deleteResponse.Response);
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult Read(string store)
        {
            int start = Convert.ToInt32(HttpContext.Request.Query["iDisplayStart"]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Query["iDisplayLength"]);
            int echo = Convert.ToInt32(HttpContext.Request.Query["sEcho"]);
            string search = HttpContext.Request.Query["sSearch"];
            int sortByColumn = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            string sortDirection = HttpContext.Request.Query["sSortDir_0"];

            var readResponse = _crudService.Read(store, start, pageSize, search, sortByColumn, sortDirection);

            var result = new
            {
                sEcho = echo,
                iTotalRecords = readResponse.TotalCount,
                iTotalDisplayRecords = readResponse.TotalCount,
                aaData = readResponse.Data
            };

            return Json(result);
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult Update(string store)
        {
            int columnId = Convert.ToInt32(HttpContext.Request.Form["columnId"]);
            string columnName = HttpContext.Request.Form["columnName"];
            string id = HttpContext.Request.Form["id"];
            string value = HttpContext.Request.Form["value"];

            var updateResponse = _crudService.Update(store, columnId, value, id, columnName);
            return Ok(updateResponse.Success ? value : updateResponse.Response);
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult ExcludedStores()
        {
            return View(_excludedStoresService.GetAll());
        }

        [Route("/DdsAdmin/[action]")]
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
            return RedirectToAction("ExcludedStores");
        }

        [Route("/DdsAdmin/[action]")]
        public IActionResult DeleteFilter(string selected)
        {
            _excludedStoresService.Delete(selected);
            return RedirectToAction("ExcludedStores");
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
