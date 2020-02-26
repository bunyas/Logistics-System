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
using mascis.ScheduledTasks;

namespace mascis.Controllers
{
    public class LMIS_FacilityOISTIController : Controller
    {
        mascisEntities context = new mascisEntities();

        public ActionResult HSIPRoutineOrders()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;
            return View();
        }

        public ActionResult HSIPCustomOrders()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var Products = context.View_ProductClassification.Where(a => a.product_category == 9).OrderBy(a => a.product_description);
            // var Products = context.A_Product.Where(a => a.product_category == 9).ToList();
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
            var Products = context.A_Product.Where(a => a.product_category == 9).ToList();
            ViewBag.ProductDataSource = Products;
            return View();
        }

        public JsonResult GetFacilityData()
        {
            int? x = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
            {
                x = new UserManagement().getCurrentuserFacility();
            }
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_Facilities.AsNoTracking().Where(a => a.FacilityCode == x).OrderBy(a => a.Facility).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult QueryHSIP()
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

        public int getFacilitySector(int facilitycode)
        {
            var facilities = context.A_Facilities.FirstOrDefault(f => f.FacilityCode == facilitycode);
            var zcode = facilities.DeliveryZoneCode;
            return System.Convert.ToInt32(zcode);
            //retu
        }

        public ActionResult DataSourceGetAll(DataManager dm)
        {
            OISTIModels obj = new OISTIModels();
            IEnumerable data = obj.GetAll(null, null, null).ToList();
            int count = obj.GetAll(null, null, null).ToList().Count();

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
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_OI_STI_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId,a.FinalSubmission,a.EditedBy,a.EditedDate,a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.EndDate).ToList();
            int count = context.Order_OI_STI_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId ,a.OrderStatusId, a.FinalSubmission, a.EditedBy, a.EditedDate,a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) &&( b.FinalSubmission == false || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.EndDate).ToList().Count();

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

        public ActionResult DataSourceHSIPCustomeOrders(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;

            IEnumerable data = context.Order_OI_STI_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId,a.FinalSubmission, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_OI_STI_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId ,a.FinalSubmission,a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId,a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 9 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId, a.FinalSubmission,a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 9 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null|| a.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            OISTIModels obj = new OISTIModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrder(param_on);
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
        //GetOrder_Add
        public ActionResult GetOrder_Add(Syncfusion.JavaScript.DataManager dm, int FC, string StartDate, string EndDate)
        {
            DateTime? sDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
            DateTime? eDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);

            OISTIModels obj = new OISTIModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrder_Add(FC, sDate, eDate).ToList();
            int count = obj.GetOrder_Add(FC, sDate, eDate).Count();

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
            OISTIModels obj = new OISTIModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrderCustom(param_on);
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

            IEnumerable data = obj.GetOrder(param_on, 9);
            int count = obj.GetOrder(param_on, 9).Count();

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

        public ActionResult Update(string key, List<spView_WebTemplate_OI_STIGetAll_Result> changed, List<spView_WebTemplate_OI_STIGetAll_Result> added, List<spView_WebTemplate_OI_STIGetAll_Result> deleted)
        {
            Formulas formulas = new Formulas();
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderOISTIModels obj = new OrderOISTIModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.ProductCode = System.Convert.ToInt32(temp.ProductCode);
                    obj.PhysicalCountAtBeginningOfReviewPeriod = temp.PhysicalCountAtBeginningOfReviewPeriod;
                    obj.QuantityReceivedDuringTheTwoMonthsCycle = temp.QuantityReceivedDuringTheTwoMonthsCycle;
                    obj.QuantityUsedDuringTwoMonths = temp.QuantityUsedDuringTwoMonths;
                    obj.NumberOfDaysOutOfStock = temp.NumberOfDaysOutOfStock;
                    obj.ThisMonthPhysicalCount = temp.ThisMonthPhysicalCount;
                    obj.LossesAndAdjustments = temp.LossesAndAdjustments;
                    obj.AMC = ((temp.QuantityUsedDuringTwoMonths*30) / (60- temp.NumberOfDaysOutOfStock));
                    obj.MonthsOfStock = ((temp.ThisMonthPhysicalCount)/ (temp.AMC));
                    obj.QuantityRequired = Convert.ToDouble(formulas.Allocated((4*(temp.AMC) - (temp.ThisMonthPhysicalCount))));
                    obj.MaximumStockQuantity = (2* temp.QuantityUsedDuringTwoMonths);
                    obj.Notes = temp.Notes;
                    obj.Quantity_Allocated = Convert.ToDouble(formulas.Allocated((temp.QuantityRequired)));
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }

            //Performing delete operation
            //if (deleted != null && deleted.Count() > 0)
            //{
            //    foreach (var temp in deleted)
            //    {
            //        db.core_householdmember.Remove(db.core_householdmember.FirstOrDefault(o => o.id == temp.id));
            //    }
            //}

            //db.SaveChanges();
            //var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCustom(string key, List<spView_WebTemplate_OI_GetAll_Custom1_Result> changed, List<spView_WebTemplate_OI_GetAll_Custom1_Result> added, List<spView_WebTemplate_OI_GetAll_Custom1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderOISTIModels obj = new OrderOISTIModels();
            ////Performing update operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.QuantityRequired = temp.QuantityRequired;
                    obj.Notes = temp.Notes;
                    obj.Quantity_Allocated = temp.QuantityRequired;
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
                    obj.QuantityRequired = temp.QuantityRequired;
                    obj.Notes = temp.Notes;
                    obj.Quantity_Allocated = temp.QuantityRequired;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            //var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
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
                    obj.Quantity_Allocated = temp.Quantity_Required;
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
                    obj.Quantity_Allocated = temp.Quantity_Required;
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public class DropdownList
        {
            public int ID { get; set; }
            public string DESC { get; set; }
        }
        public ActionResult DialogUpdateHSIP(Order_OI_STI_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_OI_STI_Header table = db_master.Order_OI_STI_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            value.OrderTypeId = 2;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == false)
            {
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber, "OISTI");
            }
            if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == true)
            {
                email.OrderReSentToScto(value.FacilityCode, value.OrderNumber, "OISTI");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogUpdateHSIP(Order_OI_STI_Header value)
        //{
        //    OrderOISTIHeaderModels obj = new OrderOISTIHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = value.StartDate;
        //    obj.EndDate = value.EndDate;
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult DialogUpdateCustomHSIP(Order_OI_STI_Header value)
        //{
        //    OrderOISTIHeaderModels obj = new OrderOISTIHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.EndDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.OrderTypeId = 1;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
       // EmailJob obx = new EmailJob();
        public ActionResult DialogInsertHSIP(Order_OI_STI_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderOISTIHeaderModels obj = new OrderOISTIHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "09", "02");

            db_hv.Order_OI_STI_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 2;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
            if (value.FinalSubmission == true)
            {
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber,"OISTI");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertHSIP(Order_OI_STI_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "09", "02");
        //    OrderOISTIHeaderModels obj = new OrderOISTIHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = value.StartDate;
        //    obj.EndDate = value.EndDate;
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogInsertHSIPCustom(Order_OI_STI_Header value)
        {
           // EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderOISTIHeaderModels obj = new OrderOISTIHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "09", "02");

            db_hv.Order_OI_STI_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 1;
            value.OrderStatusId = 1;
            value.StartDate = DateTime.Now;
            value.EndDate = DateTime.Now;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //if (value.FinalSubmission == true )
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertHSIPCustom(Order_OI_STI_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "09", "02");
        //    OrderOISTIHeaderModels obj = new OrderOISTIHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.EndDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.OrderTypeId = 1;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogUpdateCustomHSIP(Order_OI_STI_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_OI_STI_Header table = db_master.Order_OI_STI_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            value.OrderTypeId = 1;
            value.OrderStatusId = 1;
            value.StartDate = DateTime.Now;
            value.EndDate = DateTime.Now;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            //if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == false)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        {
            //EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "09", "01");
            db_hv.Order_MAULT_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.ProductCategoryId = 9;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            // //obj.SaveLog(value);
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //if (value.FinalSubmission == true )
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "09", "01");
        //    OrderMAULTHeaderModels obj = new OrderMAULTHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode; 
        //    obj.DateExpected = value.DateExpected;
        //    obj.ProductCategoryId = 9;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
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
            value.OrderStatusId = 1;
            value.ProductCategoryId = 9;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            //if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == false)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogUpdateMAULT(Order_MAULT_Header value)
        //{
        //    OrderMAULTHeaderModels obj = new OrderMAULTHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode; 
        //    obj.DateExpected = value.DateExpected;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

    }
}