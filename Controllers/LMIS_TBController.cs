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
using mascis.ScheduledTasks;
using Newtonsoft.Json;
using System.Reflection;
using Syncfusion.JavaScript.Models;
using System.Web.Script.Serialization;
using Syncfusion.XlsIO;
using Syncfusion.EJ.Export;
using System.Data.Entity.Validation;

namespace mascis.Controllers
{ 
    public class LMIS_TBController : Controller
    {
        string _Module;
        int _id = 0;
        public IEnumerable currentData;
        mascisEntities context = new mascisEntities();
        EmailJob obx = new EmailJob();
        // GET: LMIS_OISTI
        public ActionResult AllocationHSIPIndex()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.A_Product.OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var fc = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.fc = fc;

            return View();
        }
        public ActionResult AllocationHSIPIndex_Emergency()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.A_Product.OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var fc = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.fc = fc;

            return View();
        }
        public ActionResult AllocationHSIPIndexCustom()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.A_Product.OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var fc = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.fc = fc;

            return View();
        }

        public ActionResult AllocationMAULTIndex()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.A_Product.OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;
            return View();
        }
        public JsonResult GetFacilityData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getCurrentuser();
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_Facilities.Where(a => a.FacilityCode == 999999).AsNoTracking().ToList();
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.A_Facilities.Where(a => a.RFSO_UserName == RFSOfacilities).OrderBy(a => a.Facility).ToList();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.A_Facilities.OrderBy(a => a.Facility).AsNoTracking().ToList();
            }
            // data = context.A_Facilities.OrderBy(a => a.Facility).AsNoTracking().ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetFacilityData()
        //{
        //    context.Configuration.ProxyCreationEnabled = false;
        //    var data = context.A_Facilities.AsNoTracking().OrderBy(a => a.Facility).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetOrderStatusData()
        {
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.A_OrderStatus.Where(a => a.StatusId == 99).ToList();
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            //var userrole = User.IsInRole("MonitoringAndEvaluationOfficer");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.A_OrderStatus.Where(a => a.StatusId == 1 || a.StatusId == 6).ToList();
            }
            else
            //if (userrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.A_OrderStatus.Where(a => a.StatusId == 2 || a.StatusId == 6 || a.StatusId == 5 || a.StatusId == 10).ToList();
                //}
                //{
                //    context.Configuration.ProxyCreationEnabled = false;
                //    data = context.A_OrderStatus.AsNoTracking();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetOrderStatusData()
        //{
        //    context.Configuration.ProxyCreationEnabled = false;
        //    var data = context.A_OrderStatus.AsNoTracking();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DataSourceHSIPCustomCodinator(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_TB_Header.ToList();
            int count = 0;

            context.Configuration.ProxyCreationEnabled = false;
            data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderTypeId == 1 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList();
            count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderTypeId == 1 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList().Count();

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
        public ActionResult DataSourceHSIPRoutine_Codinator(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_TB_Header.ToList();
            int count = 0;

            context.Configuration.ProxyCreationEnabled = false;
            data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId,a.EmergencyOrder }).Where(b => b.OrderTypeId == 2 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList();
            count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId,a.EmergencyOrder }).Where(b => b.OrderTypeId == 2 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList().Count();

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
        public ActionResult LoadGridFiles(DataManager dataManager, string OrderNumber)
        {
            DataResult result = new DataResult();
            try
            {

                {
                    DataOperations operation = new DataOperations();
                    context.Configuration.ProxyCreationEnabled = false;
                    IEnumerable DataSource = context.Order_TB.Where(f => f.OrderNumber == OrderNumber).ToList();

                    //IEnumerable DataSource = db.FileCollectionSupDocs.ToList();

                    result.result = context.Order_TB.Where(f => f.OrderNumber == OrderNumber).ToList(); ;
                    result.count = context.Order_TB.Where(f => f.OrderNumber == OrderNumber).ToList().Count();

                    if (dataManager.Skip > 0)
                        result.result = operation.PerformSkip(result.result, dataManager.Skip);
                    if (dataManager.Take > 0)
                        result.result = operation.PerformTake(result.result, dataManager.Take);
                }
            }
            catch (Exception ex)
            {
                TempData["Success"] += string.Format("Property: {0} Error: {1}", ex.Message, ex.InnerException);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public class DataResult
        {
            public IEnumerable result { get; set; }
            public int count { get; set; }
        }
        public ActionResult HSIPRoutineOrders()
        {
            TempData["OrderNumber"] = "";
            TempData["FacilityCode"] = "";
            TempData["StartDate"] = "";
            TempData["EndDate"] = "";

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;
            return View();
        }

        public ActionResult Edit_HSIPRoutineOrders()
        {
            TempData["OrderNumber"] = "";
            TempData["FacilityCode"] = "";
            TempData["StartDate"] = "";
            TempData["EndDate"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;
            return View();
        }

        public ActionResult HSIPRoutineOrders_Emergency()
        {
            TempData["OrderNumber"] = "";
            TempData["FacilityCode"] = "";
            TempData["StartDate"] = "";
            TempData["EndDate"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;
            return View();
        }
        public ActionResult Edit_HSIPRoutineOrders_Emergency()
        {
            TempData["OrderNumber"] = "";
            TempData["FacilityCode"] = "";
            TempData["StartDate"] = "";
            TempData["EndDate"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;
            return View();
        }
        public ActionResult HSIPRoutineOrders_SCTO()
        {
            TempData["OrderNumber"] = "";
            TempData["FacilityCode"] = "";
            TempData["StartDate"] = "";
            TempData["EndDate"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;
            return View();
        }

        public ActionResult HSIPCustomOrders()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var Products = context.A_Product.AsNoTracking();
            ViewBag.ProductDataSource = Products;
            return View();
        }

        public ActionResult Edit_HSIPCustomOrders()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var Products = context.A_Product.AsNoTracking();
            ViewBag.ProductDataSource = Products;
            return View();
        }
        public ActionResult HSIPCustomOrders_SCTO()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var Products = context.A_Product.AsNoTracking();
            ViewBag.ProductDataSource = Products;
            return View();
        }

        public ActionResult MAULTOrders()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var Products = context.A_Product.AsNoTracking();
            ViewBag.ProductDataSource = Products;
            return View();
        }

        public ActionResult MAULTOrders_SCTO()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var Products = context.A_Product.AsNoTracking();
            ViewBag.ProductDataSource = Products;
            return View();
        }

        public ActionResult QueryHSIP()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var ip_data = context.A_ImplimentingPartners.ToList();
            ViewBag.IPDataSource = ip_data;

            context.Configuration.ProxyCreationEnabled = false;
            var DV = context.A_DeliveryZone.ToList();
            ViewBag.DVDataSource = DV;

            context.Configuration.ProxyCreationEnabled = false;
            var LoC_data = context.A_Facility_Level_Of_Care.ToList();
            ViewBag.LoCDataSource = LoC_data;

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var district = context.A_District.ToList();
            ViewBag.DistrictDataSource = district;

            return View();
        }
        public ActionResult QueryMAULT()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var ip_data = context.A_ImplimentingPartners.ToList();
            ViewBag.IPDataSource = ip_data;

            var district_data = context.A_District.Select(x => new DropdownList
            {
                ID = x.District_Code,
                DESC = x.District_Name
            }).ToList();
            ViewBag.DistrictDataSource = district_data;

            var facility_data = context.A_Facilities.Select(x => new DropdownList
            {
                ID = x.FacilityCode,
                DESC = x.Facility
            }).ToList();
            ViewBag.FacilityDataSource = facility_data;

            context.Configuration.ProxyCreationEnabled = false;
            var LoC_data = context.A_Facility_Level_Of_Care.ToList();
            ViewBag.LoCDataSource = LoC_data;

            return View();
        }

        public ActionResult SAPExportHSIP(DateTime? StartDate, DateTime? EndDate)
        {
            TempData["_start_date"] = StartDate;
            TempData["_end_date"] = EndDate;

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var ordertype = context.A_OrderType.AsNoTracking();
            ViewBag.OrderType = ordertype;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

            ViewBag.Orders = context.Order_TB.Select(a => new { a.OrderNumber, a.A_Product.SAP_code, a.A_Product.product_description, a.Dispensed2MonthsReview, a.QuantityRequired, a.Quantity_Allocated, a.RFSONotes, a.Comments }).ToList();

            return View();
        }
        public ActionResult SAPExportHSIP_Cordinator()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderType = context.A_OrderType.AsNoTracking();
            ViewBag.OrderType = OrderType;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.Where(a => a.StatusId == 3 || a.StatusId == 2).OrderBy(a => a.StatusId).ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;
            ViewBag.Products = context.A_Product.ToList();

            //ViewBag.Orders = context.Order_ViralLoadReagents_Detail.Select(a => new { a.OrderNumber, a.A_Product.SAP_code, a.A_Product.product_description, a.Consumption, a.QuantityOrdered, a.Quantity_Allocated, a.RFSONotes, a.Comment }).ToList();

            return View();
        }

        public ActionResult SAPExportHSIP_CustomCordinator()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderType = context.A_OrderType.AsNoTracking();
            ViewBag.OrderType = OrderType;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.Where(a => a.StatusId == 3 || a.StatusId == 2).OrderBy(a => a.StatusId).ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

            // ViewBag.Orders = context.Order_ViralLoadReagents_Detail.Select(a => new { a.OrderNumber, a.A_Product.SAP_code, a.A_Product.product_description, a.Consumption, a.QuantityOrdered, a.Quantity_Allocated, a.RFSONotes, a.Comment }).ToList();

            return View();
        }
        public ActionResult SAPExportMAULT()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility_data = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.FacilityDataSource = facility_data;

            context.Configuration.ProxyCreationEnabled = false;
            var product_data = context.A_Product.ToList();
            ViewBag.ProductDataSource = product_data;

            context.Configuration.ProxyCreationEnabled = false;
            var ordertype_data = context.A_OrderType.ToList();
            ViewBag.OrderTypeDataSource = ordertype_data;

            return View();
        }

        public ActionResult AllocationHSIP(int? order_type, int? product_code)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking().OrderBy(a => a.Facility);
            ViewBag.FacilityDataSource = facility;

            ViewBag.ot = order_type;
            ViewBag.pc = product_code.ToString();
            return View();
        }

        public ActionResult AllocationMAULT(string start_date, string end_date, int? product_code)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking().OrderBy(a => a.Facility);
            ViewBag.FacilityDataSource = facility;

            ViewBag.sd = start_date;
            ViewBag.ed = end_date;
            ViewBag.pc = product_code.ToString();
            return View();
        }
        //DataSourceHSIPOrdersEmergencyorders
        public ActionResult DataSourceHSIPOrdersEmergencyorders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_TB_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Where(BuildContainsExpression<Order_TB_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.EmergencyOrder == true && b.OrderTypeId == 2 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_TB_Header.Where(BuildContainsExpression<Order_TB_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.EmergencyOrder == true && b.OrderTypeId == 2 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.OrderStatusId != 4 && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && b.EmergencyOrder == true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.OrderStatusId != 4 && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && b.EmergencyOrder == true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }
            //IEnumerable data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId , a.RFSO_SentBackTofacility}).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && b.OrderStatusId !=3 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
            //int count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId , a.RFSO_SentBackTofacility}).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && b.OrderStatusId !=3 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();

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

        public ActionResult Edit_DataSourceHSIPOrdersEmergencyorders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_TB_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;

            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.FacilityCode == 9999999 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5)) && b.EmergencyOrder == true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.FacilityCode == 9999999 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5)) && b.EmergencyOrder == true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }

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

        public ActionResult DataSourceHSIPOrders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_TB_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Where(BuildContainsExpression<Order_TB_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_TB_Header.Where(BuildContainsExpression<Order_TB_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.OrderStatusId != 4 && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && b.EmergencyOrder != true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.OrderStatusId != 4 && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && b.EmergencyOrder != true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }
            //IEnumerable data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId , a.RFSO_SentBackTofacility}).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && b.OrderStatusId !=3 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
            //int count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId , a.RFSO_SentBackTofacility}).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && b.OrderStatusId !=3 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();

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

        public ActionResult Edit_DataSourceHSIPOrders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_TB_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;

            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.FacilityCode == 9999999 &&((b.OrderStatusId != 1) && (b.OrderStatusId != 5)) && b.EmergencyOrder != true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.FacilityCode == 9999999 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5)) && b.EmergencyOrder != true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }

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

        public ActionResult DataSourceHSIPCustomOrders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_TB_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Where(BuildContainsExpression<Order_TB_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected, a.FinalSubmission }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && ( b.OrderStatusId == 1 ) && b.FinalSubmission == true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_TB_Header.Where(BuildContainsExpression<Order_TB_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected, a.FinalSubmission }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && ( b.OrderStatusId == 1 ) && b.FinalSubmission == true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && b.OrderStatusId != 4 && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && b.OrderStatusId != 4 && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }
            //IEnumerable data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 1 && b.OrderStatusId != 3 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
            //int count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId , a.RFSO_SentBackTofacility}).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 1 && b.OrderStatusId != 3 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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

        public ActionResult Edit_DataSourceHSIPCustomOrders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_TB_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;

            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == 9999999 && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5)) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == 9999999 && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5)) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }

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

        public ActionResult DataSourceMAULTOrders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_MAULT_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_MAULT_Header.Where(BuildContainsExpression<Order_MAULT_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 13 && a.OrderStatusId != 3 && a.OrderStatusId != 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_MAULT_Header.Where(BuildContainsExpression<Order_MAULT_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 13 && a.OrderStatusId != 3 && a.OrderStatusId != 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 13 && a.OrderStatusId != 3 && a.OrderStatusId == 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 13 && a.OrderStatusId != 3 && a.OrderStatusId == 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }
            //IEnumerable data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId , a.RFSO_SentBackTofacility}).Where(a => a.ProductCategoryId == 13 && a.OrderStatusId != 3  && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
            //int count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId , a.RFSO_SentBackTofacility}).Where(a => a.ProductCategoryId == 13 && a.OrderStatusId != 3  && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
        //DataSourceAllocationHSIPCustom
        public ActionResult DataSourceAllocationHSIPCustom(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var _data = context.Order_TB.Select(a => new { a.Order_TB_Header.A_OrderType.OrderType, a.Order_TB_Header.A_OrderType.OrderTypeId, a.A_Product.product_category, a.ProductCode, a.A_Product.product_description, a.Order_TB_Header.OrderStatusId/*, a.Order_TB_Header.DatePrepared*/ }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.OrderTypeId == 1 && a.product_category == 13 && (a.OrderStatusId != 4)).ToList();
            //int count = context.Order_ViralLoadReagents_Detail.Select(a => new { a.Order_TB_Header.A_OrderType.OrderType, a.Order_TB_Header.A_OrderType.OrderTypeId, a.ProductCode, a.A_Product.product_category, a.A_Product.product_description, a.Order_TB_Header.OrderStatusId/*, a.Order_TB_Header.DatePrepared*/ }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.OrderTypeId == 1 && a.product_category == 13 && (a.OrderStatusId != 4)).ToList().Count();
            IEnumerable data = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.ProductCode, a.product_description }).Distinct().ToList();
            int count = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.ProductCode, a.product_description }).Distinct().ToList().Count;

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
        public ActionResult DataSourceAllocationHSIP(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var _data = context.Order_TB.Select(a => new { a.Order_TB_Header.A_OrderType.OrderType, a.Order_TB_Header.A_OrderType.OrderTypeId, a.A_Product.product_category, a.ProductCode, a.A_Product.product_description, a.Order_TB_Header.OrderStatusId, a.Order_TB_Header.EmergencyOrder }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.OrderTypeId == 2 && a.product_category == 13 && a.EmergencyOrder != true && (a.OrderStatusId != 4)).ToList();
            //int count = context.Order_ViralLoadReagents_Detail.Select(a => new { a.Order_TB_Header.A_OrderType.OrderType, a.Order_TB_Header.A_OrderType.OrderTypeId, a.ProductCode, a.A_Product.product_category, a.A_Product.product_description, a.Order_TB_Header.OrderStatusId, a.Order_TB_Header.EmergencyOrder }).Distinct().Where(a=> (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.OrderTypeId == 2 && a.product_category == 13 && a.EmergencyOrder != true && ( a.OrderStatusId != 4)).ToList().Count();
            IEnumerable data = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.ProductCode, a.product_description }).Distinct().ToList();
            int count = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.ProductCode, a.product_description }).Distinct().ToList().Count;

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

        public ActionResult DataSourceAllocationHSIP_Emergemcy(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var _data = context.Order_TB.Select(a => new { a.Order_TB_Header.A_OrderType.OrderType, a.Order_TB_Header.A_OrderType.OrderTypeId, a.A_Product.product_category, a.ProductCode, a.A_Product.product_description, a.Order_TB_Header.OrderStatusId, a.Order_TB_Header.EmergencyOrder }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.EmergencyOrder == true && a.OrderTypeId == 2 && a.product_category == 13 && (a.OrderStatusId != 4)).ToList();
            //int count = context.Order_ViralLoadReagents_Detail.Select(a => new { a.Order_TB_Header.A_OrderType.OrderType, a.Order_TB_Header.A_OrderType.OrderTypeId, a.ProductCode, a.A_Product.product_category, a.A_Product.product_description, a.Order_TB_Header.OrderStatusId, a.Order_TB_Header.EmergencyOrder }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.EmergencyOrder == true && a.OrderTypeId == 2 && a.product_category == 13 && (a.OrderStatusId != 4)).ToList().Count();
            IEnumerable data = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.ProductCode, a.product_description,a.EmergencyOrder }).Distinct().ToList();
            int count = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.ProductCode, a.product_description, a.EmergencyOrder }).Distinct().ToList().Count;

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
        public ActionResult DataSourceAllocationMAULT(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_MAULT.Select(a => new { a.Order_MAULT_Header.DatePrepared, a.ProductCode, a.A_Product.product_description, a.Order_MAULT_Header.OrderStatusId, a.Order_MAULT_Header.ProductCategoryId }).Distinct().Where(a => a.OrderStatusId == 6 && (a.OrderStatusId != 3 || a.OrderStatusId != 4) && a.ProductCategoryId == 13).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_MAULT.Select(a => new { a.Order_MAULT_Header.DatePrepared, a.ProductCode, a.Order_MAULT_Header.OrderStatusId, a.Order_MAULT_Header.ProductCategoryId }).Distinct().Where(a => a.OrderStatusId == 6 && (a.OrderStatusId != 3 || a.OrderStatusId != 4) && a.ProductCategoryId == 13).ToList().Count();

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

        public ActionResult DataSourceGetAll(string startdate, string enddate)
        {
            List<spView_export_atomic_viral_load_reagentsGetAll_Result> data = new List<spView_export_atomic_viral_load_reagentsGetAll_Result>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                ViralLoadModels obj = new ViralLoadModels();
                data = obj.GetAll(sDate, eDate, null).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DataSourceSAPExportHSIP(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_TB_Header.ToList();
            int count = 0;
            var muserrole = User.IsInRole("LMISCordinator");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderTypeId == 2 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderTypeId == 2 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderStatusId == 3).OrderBy(a => a.DatePrepared).ToList();
                count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderStatusId == 3).OrderBy(a => a.DatePrepared).ToList().Count();
            }
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

        public ActionResult ExportHSIP_CustomCordinator(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_TB_Header.ToList();
            int count = 0;

            context.Configuration.ProxyCreationEnabled = false;
            data = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderTypeId == 1 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList();
            count = context.Order_TB_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderTypeId == 1 && (b.OrderStatusId == 2 || b.OrderStatusId == 3)).OrderBy(a => a.DatePrepared).ToList().Count();

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

        public ActionResult GetOrder(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            //string a = start_date.ToString();
            //string b = end_date.ToString();
            TBModels obj = new TBModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrder(param_on).Where(a => a.ProductCode != null || a.PackSize == null).ToList();
            int count = obj.GetOrder(param_on).Where(a => a.ProductCode != null || a.PackSize == null).ToList().Count();

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

        public ActionResult GetOrderTestSummary(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            TBModels obj = new TBModels();

            IEnumerable data = obj.GetOrderTBSummary(param_on).ToList();
            int count = obj.GetOrderTBSummary(param_on).ToList().Count();

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

     
        public ActionResult GetOrderCustom(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            //string a = start_date.ToString();
            //string b = end_date.ToString();
            TBModels obj = new TBModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrderCustom(param_on).ToList();
            int count = obj.GetOrderCustom(param_on).ToList().Count();

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

        public ActionResult GetOrderMAULT(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            MAULTModels obj = new MAULTModels();

            IEnumerable data = obj.GetOrder(param_on, 13).ToList();
            int count = obj.GetOrder(param_on, 13).ToList().Count();

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

        public ActionResult GetAllocation(Syncfusion.JavaScript.DataManager dm, int? param_ot, int? param_pc)
        {
            //DateTime param_sd1 = DateTime.Parse(param_sd.Replace("\"", ""));
            //DateTime param_ed1 = DateTime.Parse(param_ed.Replace("\"", ""));
            TBModels obj = new TBModels();
            
            IEnumerable data = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).ToList();
            int count = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).ToList().Count();

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
        public ActionResult GetAllocations(Syncfusion.JavaScript.DataManager dm, int? param_ot, int? param_pc)
        {
            TBModels obj = new TBModels();
            var data = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllocationEmergency(Syncfusion.JavaScript.DataManager dm, int? param_ot, int? param_pc)
        {
            //DateTime param_sd1 = DateTime.Parse(param_sd.Replace("\"", ""));
            //DateTime param_ed1 = DateTime.Parse(param_ed.Replace("\"", ""));
            TBModels obj = new TBModels();

            IEnumerable data = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).ToList();
            int count = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).ToList().Count();

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
        public ActionResult GetAllocationsEmergency(Syncfusion.JavaScript.DataManager dm, int? param_ot, int? param_pc)
        {
            TBModels obj = new TBModels();
            var data = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateTestSummary(string key, List<spView_WebTemplate_TB_SummaryGetAll_Result> changed, List<spView_WebTemplate_TB_SummaryGetAll_Result> added, List<spView_WebTemplate_TB_SummaryGetAll_Result> deleted)
        {
            //var order_no = TempData.Peek("OrderNumber").ToString();
            OrderViralloadTestSummaryModel obj = new OrderViralloadTestSummaryModel();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = temp.OrderNumber;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.ID = System.Convert.ToInt16(temp.ID);
                    obj.Quantity = temp.Quantity;
                    obj.UpdateTB();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, List<spView_WebTemplate_TBGetAll_Result> changed, List<spView_WebTemplate_TBGetAll_Result> added, List<spView_WebTemplate_TBGetAll_Result> deleted)
        {
            OrderTBModels obj = new OrderTBModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = temp.OrderNumber;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.ProductCode = System.Convert.ToInt32(temp.ProductCode);
                    obj.OpeningBalance = temp.OpeningBalance;
                    obj.QtyRecieved = temp.QtyRecieved;
                    obj.Dispensed2MonthsReview = temp.Dispensed2MonthsReview;
                    obj.LossesAndAdjustments = temp.LossesAndAdjustments;
                    obj.ClosingBalance = temp.ClosingBalance;
                    obj.QuantityRequired = temp.QuantityRequired;
                    obj.Quantity_Allocated = System.Convert.ToInt32(temp.Quantity_Allocated);
                    obj.Comments = temp.Comments;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCustom(string key, List<spView_WebTemplate_TB_GetAll_Custom_Result> changed, List<spView_WebTemplate_TB_GetAll_Custom_Result> added, List<spView_WebTemplate_TB_GetAll_Custom_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderTBModels obj = new OrderTBModels();
            ////Performing update operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = temp.product_code;
                    obj.QuantityRequired = temp.QuantityRequired;
                    obj.Quantity_Allocated = System.Convert.ToInt32(temp.QuantityRequired);
                    obj.Comments = temp.Comments;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = temp.product_code;
                    obj.QuantityRequired = temp.QuantityRequired;
                    obj.Quantity_Allocated = System.Convert.ToInt32(temp.QuantityRequired);
                    obj.Comments = temp.Comments;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateMAULT(string key, List<spView_WebTemplate_MAULTGetAll_Result> changed, List<spView_WebTemplate_MAULTGetAll_Result> added, List<spView_WebTemplate_MAULTGetAll_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderMAULTModels obj = new OrderMAULTModels();
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.Quantity_Required = temp.Quantity_Required;
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.Quantity_Required = temp.Quantity_Required;
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAllocation(string key, List<spView_lmis_allocation_n_TBGetAll_Result> changed, List<spView_lmis_allocation_n_TBGetAll_Result> added, List<spView_lmis_allocation_n_TBGetAll_Result> deleted)
        {
            TBModels objx = new TBModels();
            OrderTBModels obj = new OrderTBModels();
            OrderTBHeaderModels obj2 = new OrderTBHeaderModels();
            int i = 0;
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    if (i < 1)
                    {
                        obj2.OrderNumber = temp.OrderNumber;
                        obj2.OrderStatusId = 2;
                        obj2.FacilityCode = temp.FacilityCode;
                        obj2.StartDate = temp.StartDate;
                        obj2.EndDate = temp.EndDate; //obj2.EmergencyOrder = temp.EmergencyOrder;
                        obj2.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
                        obj2.OrderTypeId = temp.OrderTypeId;
                        obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                        obj2.OrderStatusIdDateChanged = DateTime.Now;
                        obj2.Update2();
                        i++;
                    }
                    obj.OrderNumber = temp.OrderNumber;
                    obj.ProductCode = System.Convert.ToInt32(temp.ProductCode);
                    obj.Quantity_Allocated = System.Convert.ToInt32(temp.Quantity_Allocated);
                    obj.RFSONotes = temp.RFSONotes;
                    obj.UpdateAllocation();
                }
            }
            var data = "";// objx.GetAllocation(product_code,System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAllocationEmergency(string key, List<spView_lmis_allocation_e_TBGetAll_Result> changed, List<spView_lmis_allocation_e_TBGetAll_Result> added, List<spView_lmis_allocation_e_TBGetAll_Result> deleted)
        {
            TBModels objx = new TBModels();
            OrderTBModels obj = new OrderTBModels();
            OrderTBHeaderModels obj2 = new OrderTBHeaderModels();
            int i = 0;
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    if (i < 1)
                    {
                        obj2.OrderNumber = temp.OrderNumber;
                        obj2.OrderStatusId = 2;
                        obj2.FacilityCode = temp.FacilityCode;
                        obj2.StartDate = temp.StartDate;
                        obj2.EndDate = temp.EndDate; //obj2.EmergencyOrder = temp.EmergencyOrder;
                        obj2.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
                        obj2.OrderTypeId = temp.OrderTypeId;
                        obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                        obj2.OrderStatusIdDateChanged = DateTime.Now;
                        obj2.Update2();
                        i++;
                    }
                    obj.OrderNumber = temp.OrderNumber;
                    obj.ProductCode = System.Convert.ToInt32(temp.ProductCode);
                    obj.Quantity_Allocated = System.Convert.ToInt32(temp.Quantity_Allocated);
                    obj.RFSONotes = temp.RFSONotes;
                    obj.UpdateAllocation();
                }
            }
            var data = "";// objx.GetAllocation(product_code,System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public class DropdownList
        {
            public int ID { get; set; }
            public string DESC { get; set; }
        }

       
        public ActionResult DialogUpdate(Order_TB_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            OrderTBHeaderModels obj = new OrderTBHeaderModels();
            Order_TB_Header table = db_master.Order_TB_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            TempData["FacilityCode"] = value.FacilityCode;
            TempData["StartDate"] = value.StartDate;
            TempData["EndDate"] = value.EndDate;
            value.OrderTypeId = 2;
            //value.OrderStatusId = 1;
            value.OrderStatusIdDateChanged = DateTime.Now;
            value.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            ////obj.SaveLog(value);
            if (value.OrderStatusId == 6)
            {
                obx.SendEmail(value.FacilityCode, value.OrderNumber);
            }
            if (value.RFSO_ApproveOrder == true)
            {
                EmailJob email = new EmailJob();
                email.OrderAccepted(value.FacilityCode, value.OrderNumber, "TB");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult DialogUpdateCustom(Order_TB_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            OrderTBHeaderModels obj = new OrderTBHeaderModels();
            Order_TB_Header table = db_master.Order_TB_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            value.OrderTypeId = 1;
            //value.OrderStatusId = 1;
            value.StartDate = DateTime.Now;
            value.EndDate = DateTime.Now;
            value.OrderStatusIdDateChanged = DateTime.Now;
            value.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
           // //obj.SaveLog(value);
            if (value.OrderStatusId == 6)
            {
                obx.SendEmail(value.FacilityCode, value.OrderNumber);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DialogInsertHSIP(Order_TB_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderTBHeaderModels obj = new OrderTBHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "13", "02");

            db_hv.Order_TB_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            //TempData["OrderNumber"] = OrderNumber;
            TempData["FacilityCode"] = value.FacilityCode;
            TempData["StartDate"] = value.StartDate;
            TempData["EndDate"] = value.EndDate;
            value.OrderTypeId = 2;
            //value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
           // //obj.SaveLog(value);
            if (value.OrderStatusId == 6)
            {
                obx.SendEmail(value.FacilityCode, value.OrderNumber);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult DialogInsertHSIPCustom(Order_TB_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderTBHeaderModels obj = new OrderTBHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "13", "02");

            db_hv.Order_TB_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 1;
            //value.OrderStatusId = 1;
            value.StartDate = DateTime.Now;
            value.EndDate = DateTime.Now;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            ////obj.SaveLog(value);
            if (value.OrderStatusId == 6)
            {
                obx.SendEmail(value.FacilityCode, value.OrderNumber);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
         
        public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "13", "01");

            db_hv.Order_MAULT_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.ProductCategoryId = 13;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();

            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogUpdateMAULT(Order_MAULT_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_MAULT_Header table = db_master.Order_MAULT_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            // value.OrderStatusId = 1;
            value.OrderStatusIdDateChanged = DateTime.Now;
            value.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        
        public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(
Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        public ActionResult SaveCodinator(string Order_Number)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_TB_Header table = db_master.Order_TB_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);
            //TempData["OrderNumber"] = value.OrderNumber;
            //value.OrderTypeId = 2;
            table.OrderStatusId = 3;
            table.OrderStatusIdDateChanged = DateTime.Now;
            table.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(table);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            return Json(table, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult GetSAPExport(DataManager dm)
        {
            DateTime? StartDate = null;
            DateTime? EndDate = null;

            var _start_date = TempData.Peek("_start_date");
            if (_start_date != null)
                StartDate = System.Convert.ToDateTime(_start_date);

            var _end_date = TempData.Peek("_end_date");
            if (_end_date != null)
                EndDate = System.Convert.ToDateTime(_end_date);

            TBModels obj = new TBModels();
            IEnumerable data = obj.SAPExport(StartDate, EndDate).Where(b => b.OrderStatusId == 3 && b.OrderStatusId != 4).OrderBy(a => a.DatePrepared).ToList();
            int count = obj.SAPExport(StartDate, EndDate).Where(b => b.OrderStatusId == 3 && b.OrderStatusId != 4).OrderBy(a => a.DatePrepared).ToList().Count;

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
        }
        public ActionResult GetSAPOrderCount()
        {
            TBModels obj = new TBModels();
            int count = obj.SAPExport(null, null).Where(b => b.OrderStatusId == 3 && b.OrderStatusId != 4).OrderBy(a => a.DatePrepared).ToList().Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSAPExport(string Order_Number)
        {
            SAPExport m = new SAPExport();
            var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == Order_Number);
            var msg = "Record Saved";
            if (order != null) { msg = Order_Number; }
            else
            {
                m.SaveTB(Order_Number);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public void ExportToExcel(string GridModel)
        {
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            if (_id == 1)
            {
                obj.Columns[1].DataSource = context.A_Facilities.AsNoTracking().ToList();
                obj.Columns[2].DataSource = context.A_DeliveryZone.ToList();
                obj.Columns[3].DataSource = context.A_ImplimentingPartners.ToList();
                obj.Columns[4].DataSource = context.A_Facility_Level_Of_Care.ToList();
                obj.Columns[5].DataSource = context.A_District.ToList();
            }
            //GridProperties obj = (GridProperties)Syncfusion.JavaScript.Utils.DeserializeToModel(typeof(GridProperties), GridModel);
            exp.Export(obj, currentData, _Module + ".xlsx", ExcelVersion.Excel2013, false, false, "flat-saffron");
        }
        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (ds.Key == "QueryData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_export_atomic_viral_load_reagentsGetAll_Result>>(str);
                    _Module = "Medical Access Query Viralload " + DateTime.Now.ToLongDateString();
                    _id = 1;
                    continue;
                }
                if (ds.Key == "PhysicalCountData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_export_atomic_viral_load_reagentsGetAll_Result>>(str);
                    _Module = "Medical Access Physical Count Viralload " + DateTime.Now.ToLongDateString();
                    _id = 0;
                    continue;
                }
                if (ds.Key == "SummaryData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_export_atomic_viral_load_reagents_SummaryGetAll_Result>>(str);
                    _Module = "Viralload Tests done " + DateTime.Now.ToLongDateString();
                    _id = 0;
                    continue;
                }
                if (property != null)
                {
                    Type type = property.PropertyType;
                    string serialize = serializer.Serialize(ds.Value);
                    object value = serializer.Deserialize(serialize, type);
                    property.SetValue(gridProp, value, null);
                }
            }
            return gridProp;
        }
        public ActionResult GetStockOnHandData(string Productcode)
        {
            var pc = context.View_SAP_Stock_Status.FirstOrDefault(a => a.ItemCode.ToString() == Productcode);
            return Json(pc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult QueryTestDone()
        {
            return View();
        }
        public ActionResult GetQueryTestDone(string startdate, string enddate)
        {
            List<spView_export_atomic_viral_load_reagents_SummaryGetAll_Result> data = new List<spView_export_atomic_viral_load_reagents_SummaryGetAll_Result>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                //SMCModels obj = new SMCModels();
                data = context.spView_export_atomic_viral_load_reagents_SummaryGetAll(sDate, eDate).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAllocated(Order_TB_Header value, string Order_Number)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_TB_Header table = db_master.Order_TB_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);

            if (table != null)
            {
                //db.Entry(table).CurrentValues.SetValues(value);
                table.OrderStatusId = 2;
                db_master.Entry(table).State = EntityState.Modified;
                try
                {
                    db_master.SaveChanges();
                    if (table.OrderStatusId == 2)
                    {
                        obx.SendEmailLMISCodinator(Order_Number);
                    }
                    else
                    {

                    }
                    // TempData["Success"] = "Record Saved Successfully!";
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRecord(Order_TB_Header value, string Order_Number)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_TB_Header table = db_master.Order_TB_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);

            if (table != null)
            {
                //db.Entry(table).CurrentValues.SetValues(value);
                table.OrderStatusId = 5;
                db_master.Entry(table).State = EntityState.Modified;
                try
                {
                    db_master.SaveChanges();
                    // TempData["Success"] = "Record Saved Successfully!";
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEditDataSource(string startdate, string enddate)
        {
            context.Configuration.ProxyCreationEnabled = false;
            // IEnumerable data2; //= context.Order_Lab_Header.where.ToList();
            int count = 0;
            List<Order_TB_Header> data = new List<Order_TB_Header>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                //LaboratoryModels obj = new LaboratoryModels();
                //data = obj.GetAll(sDate, eDate).ToList();
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    data = context.Order_TB_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder != true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList();
                    count = context.Order_TB_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder != true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList().Count();
                }

            }
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { result = data2, count = count }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetEditCustomDataSource(string startdate, string enddate)
        {
            context.Configuration.ProxyCreationEnabled = false;
            // IEnumerable data2; //= context.Order_Lab_Header.where.ToList();
            int count = 0;
            List<Order_TB_Header> data = new List<Order_TB_Header>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                //LaboratoryModels obj = new LaboratoryModels();
                //data = obj.GetAll(sDate, eDate).ToList();
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    data = context.Order_TB_Header.Where(b => b.DatePrepared >= sDate && b.DatePrepared <= eDate  && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList();
                    count = context.Order_TB_Header.Where(b => b.DatePrepared >= sDate && b.DatePrepared <= eDate  && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList().Count();
                }

            }
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { result = data2, count = count }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetEditEmergencyDataSource(string startdate, string enddate)
        {
            context.Configuration.ProxyCreationEnabled = false;
            // IEnumerable data2; //= context.Order_Lab_Header.where.ToList();
            int count = 0;
            List<Order_TB_Header> data = new List<Order_TB_Header>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                //LaboratoryModels obj = new LaboratoryModels();
                //data = obj.GetAll(sDate, eDate).ToList();
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    data = context.Order_TB_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder == true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList();
                    count = context.Order_TB_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder == true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList().Count();
                }

            }
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { result = data2, count = count }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveOrderStatus(Order_TB_Header value, string Order_Number, int? OrderStatus)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_TB_Header table = db_master.Order_TB_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);

            if (table != null)
            {
                //db.Entry(table).CurrentValues.SetValues(value);
                table.OrderStatusId = OrderStatus;
                db_master.Entry(table).State = EntityState.Modified;
                try
                {
                    db_master.SaveChanges();
                    if (OrderStatus == 2)
                    {
                        obx.SendEmailLMISCodinator(Order_Number);
                    }
                    else
                    {

                    }
                    // TempData["Success"] = "Record Saved Successfully!";
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }

    }
}