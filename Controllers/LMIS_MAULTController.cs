using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mascis.Models;
using System.Data.Entity;
namespace mascis.Controllers
{
    public class LMIS_MAULTController : Controller
    {
        mascisEntities context = new mascisEntities();
        // GET: MAULT
        public ActionResult AllocationIndex()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.A_Product.OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            var product_category = context.A_product_category;
            ViewBag.ProductCategoryDataSource = product_category;
            return View();
        }

        public ActionResult Allocation(int? product_category,int? product_code)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking().OrderBy(a => a.Facility);
            ViewBag.FacilityDataSource = facility;

            ViewBag.product_category = product_category.ToString();
            ViewBag.pc = product_code.ToString();
            return View();
        }

        public ActionResult SAPExport()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            var product_category = context.A_product_category;
            ViewBag.ProductCategoryDataSource = product_category;

            ViewBag.Orders = context.Order_MAULT.Select(a => new { a.OrderNumber, a.A_Product.SAP_code, a.A_Product.product_description, a.Quantity_Required, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

            return View();
        }

        public ActionResult DataSourceAllocation(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_MAULT.Select(a => new { a.Order_MAULT_Header.A_product_category.category_code, a.ProductCode, a.A_Product.product_description, a.Order_MAULT_Header.OrderStatusId }).Distinct().Where(a => a.OrderStatusId == 6 && (a.OrderStatusId != 3 || a.OrderStatusId != 4)).ToList();
            int count = context.Order_MAULT.Select(a => new { a.Order_MAULT_Header.A_product_category.category_code, a.ProductCode, a.Order_MAULT_Header.OrderStatusId }).Distinct().Where(a => a.OrderStatusId == 6 && (a.OrderStatusId != 3 || a.OrderStatusId != 4)).ToList().Count();

            DataOperations operation = new DataOperations();
            //Performing filtering operation
            if (dm.Where != null)
            {
                data = operation.PerformWhereFilter(data, dm.Where, "and");
                var filtered = (IEnumerable<object>)data;
                count = filtered.Count();
            }
            //Performing search operation
            if (dm.Search != null)
            {
                data = operation.PerformSearching(data, dm.Search);
                var searched = (IEnumerable<object>)data;
                count = searched.Count();
            }
            //Performing sorting operation
            if (dm.Sorted != null)
                data = operation.PerformSorting(data, dm.Sorted);

            //Performing paging operations
            if (dm.Skip != 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take != 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAllocation(Syncfusion.JavaScript.DataManager dm, int? param_category, int? param_pc)
        {
            MAULTModels obj = new MAULTModels();

            IEnumerable data = obj.GetAllocation(param_category, param_pc);
            int count = obj.GetAllocation(param_category, param_pc).Count();

            DataOperations operation = new DataOperations();
            //Performing filtering operation
            if (dm.Where != null)
            {
                data = operation.PerformWhereFilter(data, dm.Where, "and");
                var filtered = (IEnumerable<object>)data;
                count = filtered.Count();
            }
            //Performing search operation
            if (dm.Search != null)
            {
                data = operation.PerformSearching(data, dm.Search);
                var searched = (IEnumerable<object>)data;
                count = searched.Count();
            }
            //Performing sorting operation
            if (dm.Sorted != null)
                data = operation.PerformSorting(data, dm.Sorted);

            //Performing paging operations
            if (dm.Skip != 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take != 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAllocation(string key, List<spView_lmis_allocation_n_MAULTGetAll1_Result> changed, List<spView_lmis_allocation_n_MAULTGetAll1_Result> added, List<spView_lmis_allocation_n_MAULTGetAll1_Result> deleted)
        {
            OrderMAULTModels obj = new OrderMAULTModels();
            OrderMAULTHeaderModels obj2 = new OrderMAULTHeaderModels();
            int i = 0;
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    if (i < 1)
                    {
                        obj2.OrderNumber = temp.OrderNumber;
                        obj2.OrderStatusId = 3;
                        obj2.Update2();
                        i++;
                    }
                    obj.OrderNumber = temp.OrderNumber;
                    obj.ProductCode = System.Convert.ToInt32(temp.ProductCode);
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.UpdateAllocation();
                }
            }
            var data = "";// objx.GetAllocation(System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date), product_code).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataSourceSAPExport(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.A_product_category.category_code, a.FacilityCode, a.DatePrepared, a.DateExpected, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderStatusId }).Where(b => b.OrderStatusId == 3).OrderBy(a => a.DatePrepared).ToList();
            int count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.A_product_category.category_code, a.FacilityCode, a.DatePrepared, a.DateExpected, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderStatusId }).Where(b => b.OrderStatusId == 3).OrderBy(a => a.DatePrepared).ToList().Count();

            DataOperations operation = new DataOperations();
            //Performing filtering operation
            if (dm.Where != null)
            {
                data = operation.PerformWhereFilter(data, dm.Where, "and");
                var filtered = (IEnumerable<object>)data;
                count = filtered.Count();
            }
            //Performing search operation
            if (dm.Search != null)
            {
                data = operation.PerformSearching(data, dm.Search);
                var searched = (IEnumerable<object>)data;
                count = searched.Count();
            }
            //Performing sorting operation
            if (dm.Sorted != null)
                data = operation.PerformSorting(data, dm.Sorted);

            //Performing paging operations
            if (dm.Skip != 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take != 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
        }
    }
}