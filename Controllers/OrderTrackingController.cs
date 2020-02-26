using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mascis.Models;
using System.Linq.Expressions;
using System.Data.Entity;

namespace mascis.Controllers
{
    public class OrderTrackingController : Controller
    {
        private mascisEntities context = new mascisEntities();

        // GET: OrderTracking
        public ActionResult OrderTracking(int? ProductCategoryId, string OrderNumber)
        {
            TempData["_ProductCategoryId"] = ProductCategoryId;
            TempData["_OrderNumber"] = OrderNumber;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

            ViewBag.Orders = context.View_Order_TrackingModule_OrderDetails_1.Select(a => new { a.OrderNumber, a.SAP_code, a.ProductCode, a.product_description, a.packsize, a.strength,a.Quantity_Required, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

            return View();
        }

        public ActionResult Current_OrderTracking()
        {
          
            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

            ViewBag.Orders = context.View_Order_TrackingModule_OrderDetails_1.Select(a => new { a.OrderNumber, a.SAP_code, a.ProductCode, a.product_description, a.packsize, a.strength, a.Quantity_Required, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

            return View();
        }

        public ActionResult LMIS_OrderTracking()
        {
           
            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

            ViewBag.Orders = context.View_Order_TrackingModule_OrderDetails_1.Select(a => new { a.OrderNumber, a.SAP_code, a.ProductCode, a.product_description, a.packsize, a.strength, a.Quantity_Required, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

            return View();
        }

        public ActionResult Codinator_OrderTracking()
        {
           
            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

            ViewBag.Orders = context.View_Order_TrackingModule_OrderDetails_1.Select(a => new { a.OrderNumber, a.SAP_code, a.ProductCode, a.product_description, a.packsize, a.strength, a.Quantity_Required, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

            return View();
        }

        public JsonResult GetProductCategoryData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.A_product_category.Where(a => a.category_code == 1 || a.category_code == 2 || a.category_code == 3 || a.category_code == 9 || a.category_code == 10 || a.category_code == 11 || a.category_code == 12 || a.category_code == 13).ToList().OrderBy(x=>x.category_code);
           
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //OrderTrackingDetails.cshtml
        //public ActionResult OrderTrackingDetails()
        //{  
        //    return View();
        //}
        public ActionResult GetData(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var RFSOfacilities = new UserManagement().getUserFacilityList();

            int? productcategory = null ;
            string order_no = null;

            var _ProductCategoryId = TempData.Peek("_ProductCategoryId");
            if (_ProductCategoryId != null)
                productcategory = System.Convert.ToInt32(_ProductCategoryId);

            var _OrderNumber = TempData.Peek("_OrderNumber");
            if (_OrderNumber != null)
                order_no = _OrderNumber.ToString();


            OrderTracking obj = new OrderTracking();
            IEnumerable data = obj.GetTracking(productcategory, order_no).Where(b => b.FacilityCode == facility_id).OrderByDescending(a => a.OrderStatusDateChanged).ToList();
            int count = obj.GetTracking(productcategory, order_no).Where(b => b.FacilityCode == facility_id).OrderByDescending(a => a.OrderStatusDateChanged).ToList().Count;

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
            if (dm.Skip > 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take > 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
            //return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentData(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var RFSOfacilities = new UserManagement().getUserFacilityList();

            int? productcategory = null;
            string order_no = null;

            var _ProductCategoryId = TempData.Peek("_ProductCategoryId");
            if (_ProductCategoryId != null)
                productcategory = System.Convert.ToInt32(_ProductCategoryId);

            var _OrderNumber = TempData.Peek("_OrderNumber");
            if (_OrderNumber != null)
                order_no = _OrderNumber.ToString();


            OrderTracking obj = new OrderTracking();
            IEnumerable data = context.View_Order_TrackingModule_1.Where(b => b.FacilityCode == 99999 && (b.OrderStatusId==6|| b.OrderStatusId==4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderByDescending(a => a.OrderStatusDateChanged).ToList();
            int count = context.View_Order_TrackingModule_1.Where(b => b.FacilityCode == 99999 && (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderByDescending(a => a.OrderStatusDateChanged).ToList().Count;

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
            if (dm.Skip > 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take > 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
            //return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentDataSearch(DataManager dm, int? productCateory, /*string order_no,*/ string startdate, string enddate)
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            DateTime? sDate ;
            DateTime? eDate ;

            //sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            // eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            if ((startdate != null && startdate != "") || (enddate != null && enddate != ""))
            {
                sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            }
            else
            {
                sDate = null;// Convert.ToDateTime(DBNull.Value);
                eDate = null;// Convert.ToDateTime(DBNull.Value);
            }
            OrderTracking obj = new OrderTracking();

            IEnumerable data = context.View_Order_TrackingModule_1.Where(b => b.FacilityCode == facility_id && (b.DatePrepared >= sDate || b.DatePrepared==null) && (b.DatePrepared <=eDate || b.DatePrepared == null) && (b.ProductCategoryId==productCateory || (productCateory == null)) && (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderByDescending(a => a.OrderStatusDateChanged).ToList();
            int count = context.View_Order_TrackingModule_1.Where(b => b.FacilityCode == facility_id && (b.DatePrepared >= sDate || b.DatePrepared == null) && (b.DatePrepared <= eDate || b.DatePrepared == null) && (b.ProductCategoryId == productCateory || (productCateory == null)) && (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderByDescending(a => a.OrderStatusDateChanged).ToList().Count;

            //IEnumerable data = obj.GetCurrentTracking(productCateory, order_no,sDate,eDate).Where(b => b.FacilityCode == facility_id && (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9)).OrderByDescending(a => a.OrderStatusDateChanged).ToList();
            //int count = obj.GetCurrentTracking(productCateory, order_no, sDate, eDate).Where(b => b.FacilityCode == facility_id && (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9)).OrderByDescending(a => a.OrderStatusDateChanged).ToList().Count;

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
            if (dm.Skip > 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take > 0)
                data = operation.PerformTake(data, dm.Take);

            //return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CurrentData(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            //int? facility_id = -1;
            //if (new UserManagement().getCurrentuserFacility() != null)
            //    facility_id = new UserManagement().getCurrentuserFacility();
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var RFSOfacilities = new UserManagement().getUserFacilityList();
 
            OrderTracking obj = new OrderTracking();
            IEnumerable data = context.View_Order_TrackingModule_1.Where(b =>  (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3) && b.FacilityCode==99999).OrderByDescending(a => a.OrderStatusDateChanged).ToList();
            int count = context.View_Order_TrackingModule_1.Where(b =>  (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3) && b.FacilityCode == 99999).OrderByDescending(a => a.OrderStatusDateChanged).ToList().Count;

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
            if (dm.Skip > 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take > 0)
                data = operation.PerformTake(data, dm.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
            //return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CurrentDataSearch(DataManager dm, int? productCateory,/* string order_no,*/ string startdate, string enddate)
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            //int? facility_id = -1;
            //if (new UserManagement().getCurrentuserFacility() != null)
            //    facility_id = new UserManagement().getCurrentuserFacility();
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var RFSOfacilities = new UserManagement().getUserFacilityList();

            DateTime? sDate;
            DateTime? eDate;

            if ((startdate != null && startdate != "") || (enddate != null && enddate != ""))
            {
                sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            }
            else
            {
                sDate = null;// Convert.ToDateTime(DBNull.Value);
                eDate = null;// Convert.ToDateTime(DBNull.Value);
            }
           // sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            //eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            
            OrderTracking obj = new OrderTracking();
            IEnumerable data = context.View_Order_TrackingModule_1.Where(b => (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3) && (b.DatePrepared >= sDate || b.DatePrepared == null) && (b.DatePrepared <= eDate || b.DatePrepared == null) && (b.ProductCategoryId == productCateory || productCateory == null)).OrderByDescending(a => a.OrderStatusDateChanged).ToList();
            int count = context.View_Order_TrackingModule_1.Where(b => (b.OrderStatusId == 6 || b.OrderStatusId == 4 || b.OrderStatusId == 7 || b.OrderStatusId == 8 || b.OrderStatusId == 9 || b.OrderStatusId == 2 || b.OrderStatusId == 3) && (b.DatePrepared >= sDate || b.DatePrepared == null) && (b.DatePrepared <= eDate || b.DatePrepared == null) && (b.ProductCategoryId == productCateory || productCateory == null)).OrderByDescending(a => a.OrderStatusDateChanged).ToList().Count;

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
            if (dm.Skip > 0)
                data = operation.PerformSkip(data, dm.Skip);
            if (dm.Take > 0)
                data = operation.PerformTake(data, dm.Take);

            //return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrder(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.View_Order_TrackingModule_OrderDetails_1.Where(a=>a.OrderNumber==param_on);
            int count = context.View_Order_TrackingModule_OrderDetails_1.Where(a => a.OrderNumber == param_on).Count();

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
            //return Json(data, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
