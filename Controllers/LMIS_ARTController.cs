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
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using System.Data.Entity.Validation;

namespace mascis.Controllers
{
    public class LMIS_ARTController : Controller
    {
        string _Module;
        int _id = 0;
        public IEnumerable currentData;
        mascisEntities context = new mascisEntities();
        EmailJob obx = new EmailJob();
        // GET: LMIS_ART
        public ActionResult AllocationHSIPIndex()
        {
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

        public ActionResult QueryHSIPPhysicalCount()
        {
            return View();
        }
        public ActionResult GetPhysicalCount(DataManager dm, string startdate, string enddate)
        {
            if(string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate))
            {
                return View();
            }
            else
            {
                DateTime SD = DateTime.ParseExact(startdate, "dd/MM/yyyy", null); 
                DateTime ED = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                ARTModels obj = new ARTModels();
                var data = obj.GetPhysicalCount(SD, ED).ToList();
                int count = obj.GetPhysicalCount(SD, ED).Count();
                //DataOperations operation = new DataOperations();
                ////Performing filtering operation
                //if (dm.Where != null)
                //{
                //    data = operation.PerformWhereFilter(data, dm.Where, "and");
                //    var filtered = (IEnumerable<object>)data;
                //    count = filtered.Count();
                //}
                ////Performing search operation
                //if (dm.Search != null)
                //{
                //    data = operation.PerformSearching(data, dm.Search);
                //    var searched = (IEnumerable<object>)data;
                //    count = searched.Count();
                //}
                ////Performing sorting operation
                //if (dm.Sorted != null)
                //    data = operation.PerformSorting(data, dm.Sorted);

                ////Performing paging operations
                //if (dm.Skip != 0)
                //    data = operation.PerformSkip(data, dm.Skip);
                //if (dm.Take != 0)
                //    data = operation.PerformTake(data, dm.Take);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
           
        }
        public ActionResult AllocationHSIPIndexCustom()
        {
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
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.A_Product.OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;
            return View();
        }

        //public JsonResult GetFacilityData()
        //{
        //    context.Configuration.ProxyCreationEnabled = false;
        //    var data = context.A_Facilities.AsNoTracking().OrderBy(a => a.Facility).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetFacilityData()
        {
            //TempData["OrderNumber"] = "";
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

        public ActionResult HSIPRoutineOrders()
        {
            TempData["OrderNumber"] = "";
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

            ViewBag.Orders = context.Order_DrugDetails.Select(a => new { a.OrderNumber, a.A_Product.SAP_code, a.A_Product.product_description, a.ART_eMTCT_Consumption, a.ART_Consumption, a.Quantity_Required_Current_Patients, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

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
            ViewBag.products = context.A_Product.ToList();
            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.Where(a => a.StatusId == 2 || a.StatusId == 3).OrderBy(a=>a.StatusId).ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

            //ViewBag.Orders = context.Order_DrugDetails.Select(a => new { a.OrderNumber, a.A_Product.SAP_code, a.A_Product.product_description, a.ART_eMTCT_Consumption, a.ART_Consumption, a.Quantity_Required_Current_Patients, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

            return View();
        }

        public ActionResult LoadGridFiles(DataManager dataManager, string OrderNumber)
        {
            DataResult result = new DataResult();
            try
            {
               
                {
                    DataOperations operation = new DataOperations();
                    context.Configuration.ProxyCreationEnabled = false;
                    IEnumerable DataSource = context.Order_DrugDetails.Where(f => f.OrderNumber == OrderNumber).ToList();

                    //IEnumerable DataSource = db.FileCollectionSupDocs.ToList();
                    
                    result.result = context.Order_DrugDetails.Where(f => f.OrderNumber == OrderNumber).ToList(); ;
                    result.count = context.Order_DrugDetails.Where(f => f.OrderNumber == OrderNumber).ToList().Count(); 

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

        public ActionResult SAPExportHSIP_CustomCordinator()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderType = context.A_OrderType.AsNoTracking();
            ViewBag.OrderType = OrderType;

            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_OrderStatus.Where(a => a.StatusId == 2 || a.StatusId == 3).OrderBy(a => a.StatusId).ToList();
            ViewBag.OrderStatusDataSource = OrderStatus;

           // ViewBag.Orders = context.Order_DrugDetails.Select(a => new { a.OrderNumber, a.A_Product.SAP_code, a.A_Product.product_description, a.ART_eMTCT_Consumption, a.ART_Consumption, a.Quantity_Required_Current_Patients, a.Quantity_Allocated, a.RFSONotes, a.Notes }).ToList();

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

        public ActionResult AllocationHSIP(int?order_type, int? product_code)
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
        //Emergency orders
        public ActionResult DataSourceHSIPOrdersEmergencyorders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Where(BuildContainsExpression<Order_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 &&   b.EmergencyOrder==true && (b.OrderStatusId != 3 || b.OrderStatusId != 4) && b.OrderStatusId != 6 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_Header.Where(BuildContainsExpression<Order_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && b.EmergencyOrder == true && (b.OrderStatusId != 3 || b.OrderStatusId != 4) && b.OrderStatusId != 6 && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.EmergencyOrder == true && b.OrderTypeId == 2 && (b.OrderStatusId != 4) && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.EmergencyOrder == true && b.OrderTypeId == 2 && (b.OrderStatusId != 4) && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }

            //IEnumerable data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && (b.OrderStatusId != 3|| b.OrderStatusId != 4)).OrderByDescending(a => a.EndDate).ToList();
            //int count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && (b.OrderStatusId != 3 || b.OrderStatusId != 4)).OrderByDescending(a => a.EndDate).ToList().Count();

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
            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == 9999999 && b.EmergencyOrder == true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))   && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == 9999999 && b.EmergencyOrder == true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))  && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
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
            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Where(BuildContainsExpression<Order_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_Header.Where(BuildContainsExpression<Order_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && (b.OrderStatusId != 4) && (b.OrderStatusId == 6 || b.OrderStatusId == 2 ) && b.EmergencyOrder!=true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 2 && ( b.OrderStatusId != 4) && (b.OrderStatusId == 6|| b.OrderStatusId == 2) && b.EmergencyOrder != true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
            }

            //IEnumerable data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && (b.OrderStatusId != 3|| b.OrderStatusId != 4)).OrderByDescending(a => a.EndDate).ToList();
            //int count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 2 && (b.OrderStatusId != 3 || b.OrderStatusId != 4)).OrderByDescending(a => a.EndDate).ToList().Count();

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
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03")&& b.FacilityCode == 9999999 && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))  && b.EmergencyOrder != true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == 9999999 && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))  && b.EmergencyOrder != true && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.EndDate).ToList().Count();
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
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Where(BuildContainsExpression<Order_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId,a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && b.FacilityCode == 9999999 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_Header.Where(BuildContainsExpression<Order_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && b.FacilityCode == 9999999 && ( b.OrderStatusId == 1 ) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
                //data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 1 && (b.OrderStatusId != 3 || b.OrderStatusId != 4)).OrderByDescending(a => a.DatePrepared).ToList();
                //count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 1 && (b.OrderStatusId != 3 || b.OrderStatusId != 4)).OrderByDescending(a => a.DatePrepared).ToList().Count();

            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && ( b.OrderStatusId != 4) && (b.OrderStatusId == 6|| b.OrderStatusId == 2) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId,a.RFSO_SentBackTofacility,a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && ( b.OrderStatusId != 4) && (b.OrderStatusId == 6 || b.OrderStatusId == 2) && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }
             
            //context.Configuration.ProxyCreationEnabled = false;
            //IEnumerable data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 1 && (b.OrderStatusId != 3 || b.OrderStatusId != 4)).OrderByDescending(a => a.DatePrepared).ToList();
            //int count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode,  a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId }).Where(b => b.FacilityTypeId == "02" && b.OrderTypeId == 1 && (b.OrderStatusId != 3 || b.OrderStatusId != 4)).OrderByDescending(a => a.DatePrepared).ToList().Count();
 

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
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;

            context.Configuration.ProxyCreationEnabled = false;
            data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))  && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
            count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))  && (b.RFSO_SentBackTofacility == false || b.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            
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

        public ActionResult DataSourceMAULTOrders(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;
            context.Configuration.ProxyCreationEnabled = false;
            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_MAULT_Header.Where(BuildContainsExpression<Order_MAULT_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && (a.OrderStatusId != 3 || a.OrderStatusId != 4 ) && a.OrderStatusId != 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_MAULT_Header.Where(BuildContainsExpression<Order_MAULT_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && (a.OrderStatusId != 3 || a.OrderStatusId != 4 ) && a.OrderStatusId != 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && (a.OrderStatusId != 3 || a.OrderStatusId != 4) && a.OrderStatusId == 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && (a.OrderStatusId != 3 || a.OrderStatusId != 4) && a.OrderStatusId == 6 && (a.RFSO_SentBackTofacility == false || a.RFSO_SentBackTofacility == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }

            //IEnumerable data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId,a.ProductCategoryId,a.OrderStatusId}).Where(a=>a.ProductCategoryId==2 && (a.OrderStatusId != 3 || a.OrderStatusId != 4)).OrderByDescending(a => a.DatePrepared).ToList();
            //int count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId}).Where(a => a.ProductCategoryId == 2 && (a.OrderStatusId != 3 || a.OrderStatusId != 4)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            var _data = context.Order_DrugDetails.Select(a => new { a.Order_Header.A_OrderType.OrderType, a.Order_Header.A_OrderType.OrderTypeId, a.DrugCode, a.A_Product.product_category, a.A_Product.product_description, a.Order_Header.OrderStatusId,a.Order_Header.EmergencyOrder }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2)&& a.EmergencyOrder==true && a.OrderTypeId == 2 && a.product_category == 2 && (a.OrderStatusId != 4)).ToList();
            IEnumerable data = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.DrugCode, a.product_description,a.EmergencyOrder }).Distinct().ToList();
            int count = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.DrugCode, a.product_description }).Distinct().ToList().Count;

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
            var _data = context.Order_DrugDetails.Select(a => new { a.Order_Header.A_OrderType.OrderType, a.Order_Header.A_OrderType.OrderTypeId, a.DrugCode, a.A_Product.product_category, a.A_Product.product_description, a.Order_Header.OrderStatusId, a.Order_Header.EmergencyOrder }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.EmergencyOrder != true && a.OrderTypeId == 2 && a.product_category == 2 && (a.OrderStatusId != 4)).ToList();
            //int count = context.Order_DrugDetails.Select(a => new { a.Order_Header.A_OrderType.OrderType, a.Order_Header.A_OrderType.OrderTypeId, a.A_Product.product_category, a.DrugCode, a.Order_Header.OrderStatusId, a.Order_Header.EmergencyOrder }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.EmergencyOrder != true && a.OrderTypeId == 2 && a.product_category == 2 && (a.OrderStatusId != 4)).ToList().Count();
            IEnumerable data = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.DrugCode, a.product_description }).Distinct().ToList();
            int count = _data.Select(a => new { a.OrderType, a.OrderTypeId, a.DrugCode, a.product_description }).Distinct().ToList().Count;
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
        public ActionResult DataSourceFacility(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data =context.A_Facilities.AsNoTracking().ToList();
            int count = context.A_Facilities.AsNoTracking().ToList().Count;
            //DataOperations operation = new DataOperations();
            ////Performing filtering operation
            //if (dm.Where != null)
            //{
            //    data = operation.PerformWhereFilter(data, dm.Where, "and");
            //    var filtered = (IEnumerable<object>)data;
            //    count = filtered.Count();
            //}
            ////Performing search operation
            //if (dm.Search != null)
            //{
            //    data = operation.PerformSearching(data, dm.Search);
            //    var searched = (IEnumerable<object>)data;
            //    count = searched.Count();
            //}
            ////Performing sorting operation
            //if (dm.Sorted != null)
            //    data = operation.PerformSorting(data, dm.Sorted);

            ////Performing paging operations
            //if (dm.Skip != 0)
            //    data = operation.PerformSkip(data, dm.Skip);
            //if (dm.Take != 0)
            //    data = operation.PerformTake(data, dm.Take);

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DataSourceAllocationHSIPCustom(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_DrugDetails.Select(a => new { a.Order_Header.A_OrderType.OrderType, a.Order_Header.A_OrderType.OrderTypeId, a.DrugCode, a.A_Product.product_category, a.A_Product.product_description, a.Order_Header.OrderStatusId }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.OrderTypeId == 1 && a.product_category == 2 && (a.OrderStatusId != 4)).ToList();//&& (b.OrderStatusId != 3 || b.OrderStatusId != 4) && b.OrderStatusId == 6
            int count = context.Order_DrugDetails.Select(a => new { a.Order_Header.A_OrderType.OrderType, a.Order_Header.A_OrderType.OrderTypeId, a.A_Product.product_category, a.DrugCode, a.Order_Header.OrderStatusId }).Distinct().Where(a => (a.OrderStatusId == 6 || a.OrderStatusId == 2) && a.OrderTypeId == 1 && a.product_category == 2 && (a.OrderStatusId != 4)).ToList().Count();

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
            IEnumerable data = context.Order_MAULT.Select(a => new {a.Order_MAULT_Header.DatePrepared, a.ProductCode, a.A_Product.product_description,a.Order_MAULT_Header.OrderStatusId, a.Order_MAULT_Header.ProductCategoryId }).Distinct().Where(a => a.OrderStatusId == 6 && (a.OrderStatusId != 3 || a.OrderStatusId != 4) && a.ProductCategoryId == 2).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_MAULT.Select(a => new { a.Order_MAULT_Header.DatePrepared, a.ProductCode,  a.Order_MAULT_Header.OrderStatusId, a.Order_MAULT_Header.ProductCategoryId }).Distinct().Where(a => a.OrderStatusId == 6 && (a.OrderStatusId != 3 || a.OrderStatusId != 4) && a.ProductCategoryId == 2).ToList().Count();

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
        //[AcceptVerbs(HttpVerbs.Post)]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DataSourceGetAll(string startdate, string enddate)
        {
            List<spView_export_atomic_arvGetAll_Result> data = new List<spView_export_atomic_arvGetAll_Result>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                ARTModels obj = new ARTModels();
                data = obj.GetAll(sDate, eDate, null).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataSourceSAPExportHSIP(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_Header.ToList();
            int count = 0;
            var muserrole = User.IsInRole("LMISCordinator");
            if (muserrole)
            {
                context.Configuration.ProxyCreationEnabled = false;
                 data = context.Order_Header.Where(b => (b.OrderStatusId == 2 || b.OrderStatusId == 3) && b.OrderTypeId == 2).OrderBy(a => a.DatePrepared).ToList();
                 count = context.Order_Header.Where(b => (b.OrderStatusId == 2 || b.OrderStatusId == 3) && b.OrderTypeId == 2).OrderBy(a => a.DatePrepared).ToList().Count();

            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderStatusId == 3).OrderBy(a => a.DatePrepared).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId }).Where(b => b.OrderStatusId == 3).OrderBy(a => a.DatePrepared).ToList().Count();

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

        public ActionResult DataSourceHSIPCustomCodinator(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_Header.ToList();
            int count = 0;
                context.Configuration.ProxyCreationEnabled = false;
                data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_ApproveOrder }).Where(b => (b.OrderStatusId == 2 || b.OrderStatusId == 3) && b.OrderTypeId==1).OrderBy(a => a.DatePrepared).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.RFSO_ApproveOrder }).Where(b => (b.OrderStatusId == 2 || b.OrderStatusId == 3) && b.OrderTypeId == 1).OrderBy(a => a.DatePrepared).ToList().Count();
            
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
            ARTModels obj = new ARTModels();

            IEnumerable data = obj.GetOrder(param_on).ToList();
            int count = obj.GetOrder(param_on).Count();

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
            ARTModels obj = new ARTModels();

            IEnumerable data = obj.GetOrderCustom(param_on).ToList();
            int count = obj.GetOrderCustom(param_on).Count();

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

            IEnumerable data = obj.GetOrder(param_on,2).ToList();
            int count = obj.GetOrder(param_on,2).Count();

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

        public ActionResult GetOrderPatients(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            ARTModels obj = new ARTModels();

            IEnumerable data = obj.GetOrderPatients(param_on).ToList();
            int count = obj.GetOrderPatients(param_on).Count();

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

        public ActionResult GetOrderPatientsPaediatric(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            ARTModels obj = new ARTModels();

            IEnumerable data = obj.GetOrderPatientsPaediatric(param_on).ToList();
            int count = obj.GetOrderPatientsPaediatric(param_on).Count();

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

        public ActionResult GetOrderPatientsOther(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            ARTModels obj = new ARTModels();

            IEnumerable data = obj.GetOrderPatientsOther(param_on).ToList();
            int count = obj.GetOrderPatientsOther(param_on).Count();

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

        public ActionResult GetOrderPatientsExtraRegimen(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            ARTModels obj = new ARTModels();
            // IEnumerable data = context.View_lmis_regimen_extra_One.Where(e => e.RegimenClassification == 1 && e.OrderNumber == param_on);
            // int count = context.View_lmis_regimen_extra_One.Where(e => e.RegimenClassification == 1 && e.OrderNumber == param_on).Count();
            var Data = obj.GetOrderExtraRegimen(1, param_on).ToList();
            int count = obj.GetOrderExtraRegimen(1, param_on).Count();
            List<spView_WebTemplate_ARV_ExtraRegimenGetAll_Result> final = new List<spView_WebTemplate_ARV_ExtraRegimenGetAll_Result>();
            var data = obj.GetOrderPatients(param_on).ToList();
            foreach (var k in Data)
            {
                spView_WebTemplate_ARV_ExtraRegimenGetAll_Result l = new spView_WebTemplate_ARV_ExtraRegimenGetAll_Result();
                l.OrderNumber = param_on;
                l.RegimenDesc = k.RegimenDesc;
                l.RegimenCode = k.RegimenCode;
                l.regimen_classification_code = k.regimen_classification_code;
                l.regimen_classification_desc = k.regimen_classification_desc;
                l.RegimenCategoryCode = k.RegimenCategoryCode;
                l.RegimenCategoryDesc = k.RegimenCategoryDesc;
                l.ExistingPatients = k.ExistingPatients;
                l.NewPatients = k.NewPatients;
                l.Patients_Existing = k.Patients_Existing;
                l.Patients_New = k.Patients_New;
                l.Pregnant_Existing = k.Pregnant_Existing;
                l.Pregnant_New = k.Pregnant_New;
                l.StandardRegimen = k.StandardRegimen;
                l.StartDate = data[0].StartDate;
                l.EndDate = data[0].EndDate;
                final.Add(l);
            }

            //DataOperations operation = new DataOperations();
            ////Performing filtering operation
            //if (dm.Where != null)
            //{
            //    data = operation.PerformWhereFilter(data, dm.Where, "and");
            //    var filtered = (IEnumerable<object>)data;
            //    count = filtered.Count();
            //}
            ////Performing search operation
            //if (dm.Search != null)
            //{
            //    data = operation.PerformSearching(data, dm.Search);
            //    var searched = (IEnumerable<object>)data;
            //    count = searched.Count();
            //}
            ////Performing sorting operation
            //if (dm.Sorted != null)
            //    data = operation.PerformSorting(data, dm.Sorted);

            ////Performing paging operations
            //if (dm.Skip != 0)
            //    data = operation.PerformSkip(data, dm.Skip);
            //if (dm.Take != 0)
            //    data = operation.PerformTake(data, dm.Take);

            return Json(new { result = final, count = count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrderPaediatricExtraRegimen(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            ARTModels obj = new ARTModels();
            // IEnumerable data = context.View_lmis_regimen_extra_One.Where(e => e.RegimenClassification == 1 && e.OrderNumber == param_on);
            // int count = context.View_lmis_regimen_extra_One.Where(e => e.RegimenClassification == 1 && e.OrderNumber == param_on).Count();
            var Data = obj.GetOrderExtraRegimen(2, param_on).ToList();
            int count = obj.GetOrderExtraRegimen(2, param_on).Count();
            //List<spView_WebTemplate_ARV_ExtraRegimenGetAll_Result> final = new List<spView_WebTemplate_ARV_ExtraRegimenGetAll_Result>();
            //var data = obj.GetOrderPatients(param_on).ToList();
            //foreach (var k in Data)
            //{
            //    spView_WebTemplate_ARV_ExtraRegimenGetAll_Result l = new spView_WebTemplate_ARV_ExtraRegimenGetAll_Result();
            //    l.OrderNumber = param_on;
            //    l.RegimenDesc = k.RegimenDesc;
            //    l.RegimenCode = k.RegimenCode;
            //    l.regimen_classification_code = k.regimen_classification_code;
            //    l.regimen_classification_desc = k.regimen_classification_desc;
            //    l.RegimenCategoryCode = k.RegimenCategoryCode;
            //    l.RegimenCategoryDesc = k.RegimenCategoryDesc;
            //    l.ExistingPatients = k.ExistingPatients;
            //    l.NewPatients = k.NewPatients;
            //    l.Patients_Existing = k.Patients_Existing;
            //    l.Patients_New = k.Patients_New;
            //    l.Pregnant_Existing = k.Pregnant_Existing;
            //    l.Pregnant_New = k.Pregnant_New;
            //    l.StandardRegimen = k.StandardRegimen;
            //    l.StartDate = data[0].StartDate;
            //    l.EndDate = data[0].EndDate;
            //    final.Add(l);
            //}
            //DataOperations operation = new DataOperations();
            ////Performing filtering operation
            //if (dm.Where != null)
            //{
            //    data = operation.PerformWhereFilter(data, dm.Where, "and");
            //    var filtered = (IEnumerable<object>)data;
            //    count = filtered.Count();
            //}
            ////Performing search operation
            //if (dm.Search != null)
            //{
            //    data = operation.PerformSearching(data, dm.Search);
            //    var searched = (IEnumerable<object>)data;
            //    count = searched.Count();
            //}
            ////Performing sorting operation
            //if (dm.Sorted != null)
            //    data = operation.PerformSorting(data, dm.Sorted);

            ////Performing paging operations
            //if (dm.Skip != 0)
            //    data = operation.PerformSkip(data, dm.Skip);
            //if (dm.Take != 0)
            //    data = operation.PerformTake(data, dm.Take);

            return Json(new { result = Data, count = count }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetOrderPatientsExtraRegimen(Syncfusion.JavaScript.DataManager dm, string param_on)
        //{
        //    ARTModels obj = new ARTModels();
        //   // IEnumerable data = context.View_lmis_regimen_extra_One.Where(e => e.RegimenClassification == 1 && e.OrderNumber == param_on);
        //   // int count = context.View_lmis_regimen_extra_One.Where(e => e.RegimenClassification == 1 && e.OrderNumber == param_on).Count();
        //     IEnumerable data = obj.GetOrderPatientsExtraRegimen(1, param_on);
        //     int count = obj.GetOrderPatientsExtraRegimen(1, param_on).Count();

        //    DataOperations operation = new DataOperations();
        //    //Performing filtering operation
        //    if (dm.Where != null)
        //    {
        //        data = operation.PerformWhereFilter(data, dm.Where, "and");
        //        var filtered = (IEnumerable<object>)data;
        //        count = filtered.Count();
        //    }
        //    //Performing search operation
        //    if (dm.Search != null)
        //    {
        //        data = operation.PerformSearching(data, dm.Search);
        //        var searched = (IEnumerable<object>)data;
        //        count = searched.Count();
        //    }
        //    //Performing sorting operation
        //    if (dm.Sorted != null)
        //        data = operation.PerformSorting(data, dm.Sorted);

        //    //Performing paging operations
        //    if (dm.Skip != 0)
        //        data = operation.PerformSkip(data, dm.Skip);
        //    if (dm.Take != 0)
        //        data = operation.PerformTake(data, dm.Take);

        //    return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetOrderPaediatricExtraRegimen(Syncfusion.JavaScript.DataManager dm, string param_on)
        //{
        //    ARTModels obj = new ARTModels();
        //    //IEnumerable data = context.View_lmis_regimen_extra_One.Where(e => e.RegimenClassification == 2 && e.OrderNumber == param_on);
        //    // var n = obj.GetOrderPatientsExtraRegimen(2, param_on);
        //     IEnumerable data = obj.GetOrderPatientsExtraRegimen(2, param_on);
        //    int count = obj.GetOrderPatientsExtraRegimen(2, param_on).Count();

        //    DataOperations operation = new DataOperations();
        //    //Performing filtering operation
        //    if (dm.Where != null)
        //    {
        //        data = operation.PerformWhereFilter(data, dm.Where, "and");
        //        var filtered = (IEnumerable<object>)data;
        //        count = filtered.Count();
        //    }
        //    //Performing search operation
        //    if (dm.Search != null)
        //    {
        //        data = operation.PerformSearching(data, dm.Search);
        //        var searched = (IEnumerable<object>)data;
        //        count = searched.Count();
        //    }
        //    //Performing sorting operation
        //    if (dm.Sorted != null)
        //        data = operation.PerformSorting(data, dm.Sorted);

        //    //Performing paging operations
        //    if (dm.Skip != 0)
        //        data = operation.PerformSkip(data, dm.Skip);
        //    if (dm.Take != 0)
        //        data = operation.PerformTake(data, dm.Take);

        //    return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
        //}

        //  //GetAllocationEmergency
        public ActionResult GetAllocationEmergency(Syncfusion.JavaScript.DataManager dm, int? param_ot, int? param_pc)
        {
            //DateTime param_sd1 = DateTime.Parse(param_sd.Replace("\"", ""));
            //DateTime param_ed1 = DateTime.Parse(param_ed.Replace("\"", ""));
            ARTModels obj = new ARTModels();
            IEnumerable data = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).ToList();
            int count = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).Count();

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
            ARTModels obj = new ARTModels();
            IEnumerable data = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).OrderBy(o=> o.OrderNumber).ToList();
            int count = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).Count();

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

        //GetAllocationEmergency
        public ActionResult GetAllocationsEmergency(Syncfusion.JavaScript.DataManager dm, int? param_ot, int? param_pc)
        {
            //DateTime param_sd1 = DateTime.Parse(param_sd.Replace("\"", ""));
            //DateTime param_ed1 = DateTime.Parse(param_ed.Replace("\"", ""));
            ARTModels obj = new ARTModels();
            //var data_source = obj.GetAllocation(DateTime.Parse(param_sd.ToString()), DateTime.Parse(param_ed.ToString()), param_pc);
            //IEnumerable data = obj.GetAllocation(DateTime.Parse(param_sd.ToString()), DateTime.Parse(param_ed.ToString()), param_pc);
            //int count = obj.GetAllocation(DateTime.Parse(param_sd.ToString()), DateTime.Parse(param_ed.ToString()), param_pc).Count();
            var data1 = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).OrderBy(o => o.OrderNumber).ToList();
            IEnumerable data = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).OrderBy(o => o.OrderNumber);
            int count = obj.GetAllocationEmergency(param_ot, param_pc).OrderBy(o => o.OrderNumber).Count();

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

            return Json(data1/*new { result = data, count = count }*/, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllocations(Syncfusion.JavaScript.DataManager dm, int? param_ot, int? param_pc)
        {
            //DateTime param_sd1 = DateTime.Parse(param_sd.Replace("\"", ""));
            //DateTime param_ed1 = DateTime.Parse(param_ed.Replace("\"", ""));
            ARTModels obj = new ARTModels();
            //var data_source = obj.GetAllocation(DateTime.Parse(param_sd.ToString()), DateTime.Parse(param_ed.ToString()), param_pc);
            //IEnumerable data = obj.GetAllocation(DateTime.Parse(param_sd.ToString()), DateTime.Parse(param_ed.ToString()), param_pc);
            //int count = obj.GetAllocation(DateTime.Parse(param_sd.ToString()), DateTime.Parse(param_ed.ToString()), param_pc).Count();
            var data1 = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).OrderBy(o => o.OrderNumber).ToList();
            IEnumerable data = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).OrderBy(o => o.OrderNumber);
            int count = obj.GetAllocation(param_ot, param_pc).OrderBy(o=> o.OrderNumber).Count();

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

            return Json(data1/*new { result = data, count = count }*/, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, List<spView_WebTemplate_ARVGetAll_MA_WFA_Result> changed, List<spView_WebTemplate_ARVGetAll_MA_WFA_Result> added, List<spView_WebTemplate_ARVGetAll_MA_WFA_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTModels obj = new OrderARTModels();
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    //obj.OrderNumber = order_no;
                    obj.OrderNumber = temp.OrderNumber;
                    obj.DrugCode = System.Convert.ToInt32(temp.product_code);
                    obj.OpeningBalance = temp.OpeningBalance;
                    obj.QuantityRecieved = temp.QuantityRecieved;
                    obj.Losses_Adjustments = temp.Losses_Adjustments;
                    obj.ClosingBalance = temp.ClosingBalance;
                    obj.Months_Stock_atHand = temp.Months_Stock_atHand;
                    obj.TotalDrugs_Required = temp.TotalDrugs_Required;
                    obj.Quantity_Required_Current_Patients = temp.Quantity_Required_Current_Patients;
                    obj.Quantity_Allocated =  temp.Quantity_Allocated;
                    obj.DaysOutOfStockDuring2Months = temp.DaysOutOfStockDuring2Months;
                    obj.AdjustedAMC = temp.AdjustedAMC;
                    obj.ART_eMTCT_Consumption = temp.ART_eMTCT_Consumption;
                    obj.ART_Consumption = temp.ART_Consumption;
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCustom(string key, List<spView_WebTemplate_ARVGetAll_Custom1_Result> changed, List<spView_WebTemplate_ARVGetAll_Custom1_Result> added, List<spView_WebTemplate_ARVGetAll_Custom1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTModels obj = new OrderARTModels();
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.DrugCode = System.Convert.ToInt32(temp.product_code);
                    obj.Quantity_Required_Current_Patients = temp.Quantity_Required_Current_Patients;
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
                    obj.DrugCode = System.Convert.ToInt32(temp.product_code);
                    obj.Quantity_Required_Current_Patients = temp.Quantity_Required_Current_Patients;
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
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

        public ActionResult UpdatePatients(string key, List<spView_WebTemplate_ARV_PatientsGetAll1_Result> changed, List<spView_WebTemplate_ARV_PatientsGetAll1_Result> added, List<spView_WebTemplate_ARV_PatientsGetAll1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTPatientModels obj = new OrderARTPatientModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    //obj.OrderNumber = order_no;
                    obj.OrderNumber = temp.OrderNumber;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.RegimenCode = System.Convert.ToInt16(temp.regimen_code);
                    obj.Patients_Existing = temp.No_Existing;
                    obj.Patients_New = temp.No_New;
                    obj.Pregnant_Existing = temp.Pregnant_Existing;
                    obj.Pregnant_New = temp.Pregnant_New;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePatientsPaediatric(string key, List<spView_WebTemplate_ARV_Patients_PaediatricGetAll1_Result> changed, List<spView_WebTemplate_ARV_Patients_PaediatricGetAll1_Result> added, List<spView_WebTemplate_ARV_Patients_PaediatricGetAll1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTPatientModels obj = new OrderARTPatientModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = temp.OrderNumber;
                    //obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.RegimenCode = System.Convert.ToInt16(temp.regimen_code);
                    obj.Existing_0_3Yrs = temp.Existing_0_3Yrs;
                    obj.New_0_3Yrs = temp.New_0_3Yrs;
                    obj.Existing_3_10Yrs = temp.Existing_3_10Yrs;
                    obj.New_3_10Yrs = temp.New_3_10Yrs;
                    obj.Existing_10_15Yrs_Less35Kg = temp.Existing_10_15Yrs_Less35Kg;
                    obj.New_10_15Yrs_Less35Kg = temp.New_10_15Yrs_Less35Kg;
                    obj.Existing_10_15Yrs_Greater35Kg = temp.Existing_10_15Yrs_Greater35Kg;
                    obj.New_10_15Yrs_Greater35Kg = temp.New_10_15Yrs_Greater35Kg;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePatientsOther(string key, List<spView_WebTemplate_ARV_FluconazoleGetAll1_Result> changed, List<spView_WebTemplate_ARV_FluconazoleGetAll1_Result> added, List<spView_WebTemplate_ARV_FluconazoleGetAll1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTPatientsFluconazoleModels obj = new OrderARTPatientsFluconazoleModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = temp.OrderNumber;
                    //obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.ConditionCode = System.Convert.ToInt16(temp.regimen_code);
                    obj.Adults = temp.Adult;
                    obj.Children = temp.Child;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePatientsExtraRegimen(string key, List<spView_lmis_art_extra_regimen_Result> changed, List<spView_lmis_art_extra_regimen_Result> added, List<spView_lmis_art_extra_regimen_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTPatientModels obj = new OrderARTPatientModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = temp.OrderNumber;
                    //obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.RegimenCode = System.Convert.ToInt16(temp.RegimenCode);
                    obj.Patients_Existing = temp.Patients_Existing;
                    obj.Patients_New = temp.Patients_New;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePaediatricExtraRegimen(string key, List<spView_lmis_art_extra_regimen_Result> changed, List<spView_lmis_art_extra_regimen_Result> added, List<spView_lmis_art_extra_regimen_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTPatientModels obj = new OrderARTPatientModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = temp.OrderNumber;
                    // obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.RegimenCode = System.Convert.ToInt16(temp.RegimenCode);
                    obj.Existing_0_3Yrs = temp.Existing_0_3Yrs;
                    obj.New_0_3Yrs = temp.New_0_3Yrs;
                    obj.Existing_3_10Yrs = temp.Existing_3_10Yrs;
                    obj.New_3_10Yrs = temp.New_3_10Yrs;
                    obj.Existing_10_15Yrs_Less35Kg = temp.Existing_10_15Yrs_Less35Kg;
                    obj.New_10_15Yrs_Less35Kg = temp.New_10_15Yrs_Less35Kg;
                    obj.Existing_10_15Yrs_Greater35Kg = temp.Existing_10_15Yrs_Greater35Kg;
                    obj.New_10_15Yrs_Greater35Kg = temp.New_10_15Yrs_Greater35Kg;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult UpdateAllocationHSIP(string key, List<spView_lmis_allocation_n_arvGetAll1_Result> changed, List<spView_lmis_allocation_n_arvGetAll1_Result> added, List<spView_lmis_allocation_n_arvGetAll1_Result> deleted)
        //{
        //    ARTModels objx = new ARTModels();
        //    OrderARTModels obj = new OrderARTModels();
        //    OrderARTHeaderModels obj2 = new OrderARTHeaderModels();
        //    int i = 0;
        //    ////Performing update operation
        //    if (changed != null && changed.Count() > 0)
        //    {
        //        foreach (var temp in changed)
        //        {
        //            if (i < 1)
        //            {
        //                obj2.OrderNumber = temp.OrderNumber;
        //                obj2.OrderStatusId = 2;
        //                obj2.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
        //                obj2.StartDate = System.Convert.ToDateTime(temp.StartDate);
        //                obj2.EndDate = System.Convert.ToDateTime(temp.EndDate);
        //                obj2.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
        //                obj2.OrderTypeId = temp.OrderTypeId;
        //                obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
        //                obj2.OrderStatusIdDateChanged = DateTime.Now;
        //                obj2.Update2();
        //                i++;
        //            }
        //            obj.OrderNumber = temp.OrderNumber;
        //            obj.DrugCode = System.Convert.ToInt32(temp.DrugCode);
        //            obj.Quantity_Allocated = temp.Quantity_Allocated;
        //            obj.RFSONotes = temp.RFSONotes;
        //            obj.UpdateAllocation();
        //        }
        //    }
        //    var data = "";// objx.GetAllocation(System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date), product_code).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SaveOrders(string dataSource)
        {
            // Code here for deserializing the dataSource
            List<spView_lmis_allocation_n_arvGetAll_Result> changed = new List<spView_lmis_allocation_n_arvGetAll_Result>();
            // var changed = (List<spView_lmis_allocation_n_arvGetAll1_Result>)Newtonsoft.Json.JsonConvert.DeserializeObject(dataSource, typeof(List<spView_lmis_allocation_n_arvGetAll1_Result>));
            IEnumerable div = (IEnumerable)Newtonsoft.Json.JsonConvert.DeserializeObject(dataSource, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (ds.Key == "Data")
                {
                    string str = Convert.ToString(ds.Value);
                    changed = JsonConvert.DeserializeObject<List<spView_lmis_allocation_n_arvGetAll_Result>>(str);

                    continue;
                }
                //if (property != null)
                //{
                //    Type type = property.PropertyType;
                //    string serialize = serializer.Serialize(ds.Value);
                //    object value = serializer.Deserialize(serialize, type);
                //    property.SetValue(gridProp, value, null);
                //}
            }
            ARTModels objx = new ARTModels();
            OrderARTModels obj = new OrderARTModels();
            OrderARTHeaderModels obj2 = new OrderARTHeaderModels();
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
                        obj2.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                        obj2.StartDate = System.Convert.ToDateTime(temp.StartDate);
                        obj2.EndDate = System.Convert.ToDateTime(temp.EndDate);
                        obj2.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
                        obj2.OrderTypeId = temp.OrderTypeId;
                        obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                        obj2.OrderStatusIdDateChanged = DateTime.Now;
                        obj2.Update2();
                        i++;
                    }
                    obj.OrderNumber = temp.OrderNumber;
                    obj.DrugCode = System.Convert.ToInt32(temp.DrugCode);
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.UpdateAllocation();
                }
            }
            var data = "";// objx.GetAllocation(System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date), product_code).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);

            //return RedirectToAction("GridFeatures");

        }
        public ActionResult UpdateAllocationHSIP1(string _orderNo,int? _ProductCode, int? _Allocation,string _sctoNotes,int? _orderType)
        {
            if(!string.IsNullOrEmpty(_orderNo) && _ProductCode != null && _orderType != null)
            {
                ARTModels objx = new ARTModels();
                OrderARTModels obj = new OrderARTModels();
                OrderARTHeaderModels obj2 = new OrderARTHeaderModels();
               var product = context.Order_DrugDetails.FirstOrDefault(e => e.DrugCode == _ProductCode && e.OrderNumber == _orderNo && e.Order_Header.OrderTypeId == _orderType);
                var orderHeader = context.Order_Header.FirstOrDefault(e => e.OrderNumber == _orderNo && e.OrderTypeId == _orderType);
                if (product != null)
                {
                    obj2.OrderNumber = _orderNo;
                    obj2.OrderStatusId = 2;
                    obj2.FacilityCode = System.Convert.ToInt32(product.FacilityCode);
                    obj2.StartDate = System.Convert.ToDateTime(product.StartDate);
                    obj2.EndDate = System.Convert.ToDateTime(product.EndDate);
                    obj2.DatePrepared = System.Convert.ToDateTime(orderHeader.DatePrepared);
                    obj2.OrderTypeId = _orderType;
                    obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                    obj2.OrderStatusIdDateChanged = DateTime.Now;
                    obj2.Update2();


                    obj.OrderNumber = _orderNo;
                    obj.DrugCode = System.Convert.ToInt32(_ProductCode);
                    obj.Quantity_Allocated = Convert.ToDouble(_Allocation);
                    obj.RFSONotes = _sctoNotes;
                    obj.UpdateAllocation();
                }
            }
                 //var changed = new JavaScriptSerializer().Deserialize<List<spView_lmis_allocation_n_arvGetAll1_Result>>((string)key);
            

            ////Performing update operation
            //if (changed != null && changed.Count() > 0)
            //{
            //    foreach (var temp in changed)
            //    {
            //        if (i < 1)
            //        {
            //            obj2.OrderNumber = temp.OrderNumber;
            //            obj2.OrderStatusId = 2;
            //            obj2.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
            //            obj2.StartDate = System.Convert.ToDateTime(temp.StartDate);
            //            obj2.EndDate = System.Convert.ToDateTime(temp.EndDate);
            //            obj2.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
            //            obj2.OrderTypeId = temp.OrderTypeId;
            //            obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
            //            obj2.OrderStatusIdDateChanged = DateTime.Now;
            //            obj2.Update2();
            //            i++;
            //        }
            //        obj.OrderNumber = temp.OrderNumber;
            //        obj.DrugCode = System.Convert.ToInt32(temp.DrugCode);
            //        obj.Quantity_Allocated = temp.Quantity_Allocated;
            //        obj.RFSONotes = temp.RFSONotes;
            //        obj.UpdateAllocation();
            //    }
            //}
            var data = "";// objx.GetAllocation(System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date), product_code).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAllocationHSIPEmergency(string key, List<spView_lmis_allocation_e_arvGetAll_Result> changed, List<spView_lmis_allocation_e_arvGetAll_Result> added, List<spView_lmis_allocation_e_arvGetAll_Result> deleted)
        {
            ARTModels objx = new ARTModels();
            OrderARTModels obj = new OrderARTModels();
            OrderARTHeaderModels obj2 = new OrderARTHeaderModels();
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
                        obj2.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                        obj2.StartDate = System.Convert.ToDateTime(temp.StartDate);
                        obj2.EndDate = System.Convert.ToDateTime(temp.EndDate);
                        obj2.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
                        obj2.OrderTypeId = temp.OrderTypeId;
                        obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                        obj2.OrderStatusIdDateChanged = DateTime.Now;
                        obj2.Update2();
                        i++;
                    }
                    obj.OrderNumber = temp.OrderNumber;
                    obj.DrugCode = System.Convert.ToInt32(temp.DrugCode);
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.UpdateAllocation();
                }
            }
            var data = "";// objx.GetAllocation(System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date), product_code).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAllocationHSIP(string key, List<spView_lmis_allocation_n_arvGetAll_Result> changed, List<spView_lmis_allocation_n_arvGetAll_Result> added, List<spView_lmis_allocation_n_arvGetAll_Result> deleted)
        {
            ARTModels objx = new ARTModels();
            OrderARTModels obj = new OrderARTModels();
            OrderARTHeaderModels obj2 = new OrderARTHeaderModels();
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
                        obj2.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                        obj2.StartDate = System.Convert.ToDateTime(temp.StartDate);
                        obj2.EndDate = System.Convert.ToDateTime(temp.EndDate);
                        obj2.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
                        obj2.OrderTypeId = temp.OrderTypeId;
                        obj2.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
                        obj2.OrderStatusIdDateChanged = DateTime.Now;
                        obj2.Update2();
                        i++;
                    }
                    obj.OrderNumber = temp.OrderNumber;
                    obj.DrugCode = System.Convert.ToInt32(temp.DrugCode);
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.UpdateAllocation();
                }
            }
            var data = "";// objx.GetAllocation(System.Convert.ToDateTime(start_date), System.Convert.ToDateTime(end_date), product_code).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public class DropdownList
        {
            public int ID { get; set; }
            public string DESC { get; set; }
        }

        //public ActionResult DialogUpdate(Order_Header value)
        //{
        //    OrderARTHeaderModels obj = new OrderARTHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = value.StartDate;
        //    obj.EndDate = value.EndDate;
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = value.OrderStatusId;
        //    obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(value.RFSO_SentBackTofacility);
        //    obj.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
        //    obj.OrderStatusIdDateChanged = DateTime.Now;
        //    obj.Update2();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogUpdate(Order_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            OrderARTHeaderModels obj = new OrderARTHeaderModels();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            value.OrderTypeId = 2;
            //value.OrderStatusId = 1;
            value.OrderStatusIdDateChanged = DateTime.Now;
            value.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            //obj.SaveLog(value);
            if(value.RFSO_ApproveOrder == true)
            {
                EmailJob email = new EmailJob();
                email.OrderAccepted(value.FacilityCode, value.OrderNumber, "ARVs");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCodinator( string Order_Number)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);
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

        //public ActionResult SaveCodinator(Order_Header value)
        //{
        //    OrderARTHeaderModels obj = new OrderARTHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.StartDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.EndDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.OrderTypeId = 1;
        //    obj.OrderStatusId = 3;
        //    obj.OrderStatusId = value.OrderStatusId;
        //    obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(value.RFSO_SentBackTofacility);
        //    obj.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
        //    obj.OrderStatusIdDateChanged = DateTime.Now;
        //    obj.Update2();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogUpdateCustom(Order_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            OrderARTHeaderModels obj = new OrderARTHeaderModels();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
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
            //obj.SaveLog(value);
            if (value.OrderStatusId == 6 )
            {
                obx.SendEmail(value.FacilityCode, value.OrderNumber);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertHSIP(Order_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "02");
        //    OrderARTHeaderModels obj = new OrderARTHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = value.StartDate;
        //    obj.EndDate = value.EndDate;
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = value.OrderStatusId;
        //    obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(value.RFSO_SentBackTofacility);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogInsertHSIP(Order_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderARTHeaderModels obj = new OrderARTHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "02");

            db_hv.Order_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 2;
            //value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
            if (value.OrderStatusId == 6)
            {
                obx.SendEmail(value.FacilityCode, value.OrderNumber);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertHSIPCustom(Order_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "02");
        //    OrderARTHeaderModels obj = new OrderARTHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.StartDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.EndDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.OrderTypeId = 1;
        //    obj.OrderStatusId = value.OrderStatusId;
        //    obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(value.RFSO_SentBackTofacility);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogInsertHSIPCustom(Order_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderARTHeaderModels obj = new OrderARTHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "02");

            db_hv.Order_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 1;
            value.StartDate = DateTime.Now;
            value.EndDate = DateTime.Now;
            //value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
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
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "01");

            db_hv.Order_MAULT_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.ProductCategoryId = 2;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();

            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "01");
        //    OrderMAULTHeaderModels obj = new OrderMAULTHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.DateExpected = value.DateExpected;
        //    obj.ProductCategoryId = 2;
        //    obj.OrderStatusId = value.OrderStatusId;
        //    obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(value.RFSO_SentBackTofacility);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
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
            if (value.OrderStatusId == 6)
            {
                obx.SendEmail(value.FacilityCode, value.OrderNumber);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogUpdateMAULT(Order_MAULT_Header value)
        //{
        //    OrderMAULTHeaderModels obj = new OrderMAULTHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.DateExpected = value.DateExpected;
        //    obj.OrderStatusId = value.OrderStatusId;
        //    obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(value.RFSO_SentBackTofacility);
        //    obj.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
        //    obj.OrderStatusIdDateChanged = DateTime.Now;
        //    obj.Update2();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult SaveCodinator(Order_Header value, string Order_Number)
        //{
        //    mascisEntities dbCase = new mascisEntities();
        //    OrderARTHeaderModels obj = new OrderARTHeaderModels();
        //    //Performing insert operation

        //            obj.OrderNumber = value.OrderNumber;
        //            obj.FacilityCode = value.FacilityCode;
        //            obj.StartDate = value.StartDate;
        //            obj.EndDate = value.EndDate;
        //            obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //            //obj.Adults = temp.Adults;
        //            //obj.RFSO_SentBackTofacility = temp.Children;
        //            obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //            obj.OrderTypeId = value.OrderTypeId;
        //            obj.OrderStatusId = 3;
        //            obj.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
        //            obj.OrderStatusIdDateChanged = DateTime.Now;
        //            obj.DateExpected = value.DateExpected;
        //            obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(value.RFSO_SentBackTofacility);

        //    obj.SaveCodinator(Order_Number);
        //    var data = 0;
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //    //return RedirectToAction("DataSourceRFSO");
        //}


        //public ActionResult SaveCodinator(string Order_Number, List<Order_Header> changed)
        //{
        //    mascisEntities dbCase = new mascisEntities();
        //    OrderARTHeaderModels obj = new OrderARTHeaderModels();
        //    //Performing insert operation
        //    if (changed != null && changed.Count() > 0)
        //    {
        //        foreach (var temp in changed)
        //        {
        //            obj.OrderNumber = temp.OrderNumber;
        //            obj.FacilityCode = temp.FacilityCode;
        //            obj.StartDate = temp.StartDate;
        //            obj.EndDate = temp.EndDate;
        //            obj.DatePrepared = System.Convert.ToDateTime(temp.DatePrepared);
        //            //obj.Adults = temp.Adults;
        //            //obj.RFSO_SentBackTofacility = temp.Children;
        //            obj.FinalSubmission = System.Convert.ToBoolean(temp.FinalSubmission);
        //            obj.OrderTypeId = temp.OrderTypeId;
        //            obj.OrderStatusId = 3;
        //            obj.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
        //            obj.OrderStatusIdDateChanged = DateTime.Now;
        //            obj.DateExpected = temp.DateExpected;
        //            obj.RFSO_SentBackTofacility = System.Convert.ToBoolean(temp.RFSO_SentBackTofacility);
        //        }
        //    }
        //    obj.SaveCodinator(Order_Number);
        //    var data = 0;
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //    //return RedirectToAction("DataSourceRFSO");
        //}
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

            ARTModels obj = new ARTModels();
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
            ARTModels obj = new ARTModels();
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
                m.SaveART(Order_Number);
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
            exp.Export(obj, currentData, _Module + ".xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
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
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_export_atomic_arvGetAll_Result>>(str);
                    _Module = "ART Query " + DateTime.Now.ToLongDateString();
                    _id = 1;
                    continue;
                }
                if (ds.Key == "PhysicalCountData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_export_atomic_arv_Physical_CountGetAll_Result>>(str);
                    _Module = "ART Physical Count " + DateTime.Now.ToLongDateString();
                    _id = 0;
                    continue;
                }
                if(ds.Key == "FluconazoleData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_export_atomic_arv_Treatment_Fluconazole_Result>>(str);
                    _Module = "ART Fluconazole" + DateTime.Now.ToLongDateString();
                    _id = 0;
                    continue;
                }
                if (ds.Key == "PatientData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_export_atomic_arv_Treatment_PatientSummary_Result>>(str);
                    _Module = "ART Patient numbers" + DateTime.Now.ToLongDateString();
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

        public ActionResult FluconazoleQuery()
        {
            return View();
        }
        public ActionResult PatientQuery()
        {
            return View();
        }
        public ActionResult GetFluconazoleQuery(string startdate, string enddate)
        {
            List<spView_export_atomic_arv_Treatment_Fluconazole_Result> data = new List<spView_export_atomic_arv_Treatment_Fluconazole_Result>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                ///ARTModels obj = new ARTModels();
                data = context.spView_export_atomic_arv_Treatment_Fluconazole(sDate, eDate).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPatientQuery(string startdate, string enddate)
        {
            List<spView_export_atomic_arv_Treatment_PatientSummary_Result> data = new List<spView_export_atomic_arv_Treatment_PatientSummary_Result>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                ///ARTModels obj = new ARTModels();
                data = context.spView_export_atomic_arv_Treatment_PatientSummary(sDate, eDate).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAllocated(Order_Header value, string Order_Number)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            //OrderARTHeaderModels obj = new OrderARTHeaderModels();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);
            //TempData["OrderNumber"] = value.OrderNumber;
            //value.OrderTypeId = 2;
            //value.OrderStatusId = 2;
            if (table != null)
            {  //db.Entry(table).CurrentValues.SetValues(value);
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
            //value.OrderStatusIdDateChanged = DateTime.Now;
            //value.OrderStatusIdChangedBy = new UserManagement().getCurrentuser();
            //db_master.Entry(table).CurrentValues.SetValues(value);
            //db_master.Entry(table).State = EntityState.Modified;
            //db_master.SaveChanges();
            ////obj.SaveLog(value);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRecord(Order_Header value, string Order_Number)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);

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
            List<Order_Header> data = new List<Order_Header>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                //LaboratoryModels obj = new LaboratoryModels();
                //data = obj.GetAll(sDate, eDate).ToList();
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    data = context.Order_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder != true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList();
                    count = context.Order_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder != true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList().Count();
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
            List<Order_Header> data = new List<Order_Header>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                //LaboratoryModels obj = new LaboratoryModels();
                //data = obj.GetAll(sDate, eDate).ToList();
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    data = context.Order_Header.Where(b => b.DatePrepared >= sDate && b.DatePrepared <= eDate  && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList();
                    count = context.Order_Header.Where(b => b.DatePrepared >= sDate && b.DatePrepared <= eDate && b.OrderTypeId == 1 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList().Count();
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
            List<Order_Header> data = new List<Order_Header>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

                //LaboratoryModels obj = new LaboratoryModels();
                //data = obj.GetAll(sDate, eDate).ToList();
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    data = context.Order_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder == true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList();
                    count = context.Order_Header.Where(b => b.StartDate >= sDate && b.EndDate <= eDate && b.EmergencyOrder == true && b.OrderTypeId == 2 && ((b.OrderStatusId != 1) && (b.OrderStatusId != 5))).OrderBy(a => a.DatePrepared).ToList().Count();
                }

            }
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { result = data2, count = count }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveOrderStatus(Order_Header value, string Order_Number, int? OrderStatus)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == Order_Number);

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