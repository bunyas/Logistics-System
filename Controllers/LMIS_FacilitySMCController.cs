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
    public class LMIS_FacilitySMCController : Controller
    {
        mascisEntities context = new mascisEntities();

        public ActionResult HSIPRoutineOrders()
        {
            TempData["OrderNumber"] = "";
            TempData["FacilityCode"] = "";
            TempData["StartDate"] = "";
            TempData["EndDate"] = "";
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
            var Products = context.View_ProductClassification.Where(a => a.product_category == 10).OrderBy(a => a.product_description);
            //var Products = context.A_Product.Where(a => a.product_category == 10).ToList();
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
            var Products = context.A_Product.Where(a => a.product_category == 10).ToList();
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

        public ActionResult DataSourceGetAll(DataManager dm)
        {
            SMCModels obj = new SMCModels();
            IEnumerable data = obj.GetAll(null, null,null).ToList();
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
            IEnumerable data = context.Order_SMC_SLM_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId,a.FinalSubmission,a.EditedBy,a.EditedDate,a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.EndDate).ToList();
            int count = context.Order_SMC_SLM_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.FinalSubmission, a.EditedBy, a.EditedDate,a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.EndDate).ToList().Count();

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
        public int getFacilitySector(int facilitycode)
        {
            var facilities = context.A_Facilities.FirstOrDefault(f => f.FacilityCode == facilitycode);
            var zcode = facilities.DeliveryZoneCode;
            return System.Convert.ToInt32(zcode);
            //retu
        }
        public ActionResult DataSourceHSIPCustomOrders(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_SMC_SLM_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId,a.FinalSubmission, a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_SMC_SLM_Header.Select(a => new { a.OrderNumber, a.FacilityCode,a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId,a.FinalSubmission , a.RFSO_SentBackTofacility, a.DateExpected }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            IEnumerable data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId,a.FinalSubmission,a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 10 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId,a.FinalSubmission , a.RFSO_SentBackTofacility}).Where(a => a.ProductCategoryId == 10 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            SMCModels obj = new SMCModels();
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

        public ActionResult GetOrderPatientSummary(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            //string a = start_date.ToString();
            //string b = end_date.ToString();
            ARTModels obj = new ARTModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrderSMCPatientSummary(param_on);
            int count = obj.GetOrderSMCPatientSummary(param_on).Count();

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

        public ActionResult GetOrder_Add(Syncfusion.JavaScript.DataManager dm, int FC, string StartDate, string EndDate)
        {
            DateTime? sDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
            DateTime? eDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);

            SMCModels obj = new SMCModels();
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
            SMCModels obj = new SMCModels();
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

            IEnumerable data = obj.GetOrder(param_on, 10);
            int count = obj.GetOrder(param_on, 10).Count();

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

        public ActionResult Update(string key, List<spView_WebTemplate_SMC_SLMGetAll_Result> changed, List<spView_WebTemplate_SMC_SLMGetAll_Result> added, List<spView_WebTemplate_SMC_SLMGetAll_Result> deleted)
        {
            Formulas formulas = new Formulas();
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderSMCModels obj = new OrderSMCModels();
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
                    obj.OpeningBalance = temp.OpeningBalance;
                    obj.QtyRecieved = temp.QtyRecieved;
                    obj.Consumption = temp.Consumption;
                    obj.LossesAndAdjustments = temp.LossesAndAdjustments;
                    obj.TotalClosingBalance = temp.TotalClosingBalance;
                    obj.QuantityToOrder = Convert.ToDouble(formulas.Allocated((3* temp.Consumption) - temp.TotalClosingBalance));
                    obj.QuantityAllocated = (formulas.Allocated((3 * temp.Consumption) - temp.TotalClosingBalance));
                    obj.Comments = temp.Comments;
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

        public ActionResult UpdateCustom(string key, List<spView_WebTemplate_SMC_SLMGetAll_Custom1_Result> changed, List<spView_WebTemplate_SMC_SLMGetAll_Custom1_Result> added, List<spView_WebTemplate_SMC_SLMGetAll_Custom1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderSMCModels obj = new OrderSMCModels();
            ////Performing update operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.QuantityToOrder = temp.QuantityToOrder;
                    obj.QuantityAllocated = System.Convert.ToInt32(temp.QuantityToOrder);
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
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.QuantityToOrder = temp.QuantityToOrder;
                    obj.QuantityAllocated = System.Convert.ToInt32(temp.QuantityToOrder);
                    obj.Comments = temp.Comments;
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
        public ActionResult DialogUpdateHSIP(Order_SMC_SLM_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_SMC_SLM_Header table = db_master.Order_SMC_SLM_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
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
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber, "SMC");
            }
            if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == true)
            {
                email.OrderReSentToScto(value.FacilityCode, value.OrderNumber, "SMC");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogUpdateHSIP(Order_SMC_SLM_Header value)
        //{
        //    OrderSMCHeaderModels obj = new OrderSMCHeaderModels();
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
        public ActionResult DialogInsertHSIP(Order_SMC_SLM_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderSMCHeaderModels obj = new OrderSMCHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "10", "02");

            db_hv.Order_SMC_SLM_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            TempData["FacilityCode"] = value.FacilityCode;
            TempData["StartDate"] = value.StartDate;
            TempData["EndDate"] = value.EndDate;
            value.OrderTypeId = 2;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
            if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == false)
            {
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber,"SMC");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogUpdateCustomHSIP(Order_SMC_SLM_Header value)
        //{
        //    OrderSMCHeaderModels obj = new OrderSMCHeaderModels();
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
        public ActionResult DialogUpdateCustomHSIP(Order_SMC_SLM_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
         
            Order_SMC_SLM_Header table = db_master.Order_SMC_SLM_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
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
        //public ActionResult DialogInsertHSIPCustom(Order_SMC_SLM_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "10", "02");
        //    OrderSMCHeaderModels obj = new OrderSMCHeaderModels();
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

        public ActionResult DialogInsertHSIPCustom(Order_SMC_SLM_Header value)
        {
            //EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderSMCHeaderModels obj = new OrderSMCHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "10", "02");

            db_hv.Order_SMC_SLM_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 1;
            value.OrderStatusId = 1;
            //value.DateExpected= DateTime.Now;
            value.StartDate = DateTime.Now;
            value.EndDate = DateTime.Now;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //if (value.FinalSubmission == true)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult DialogInsertHSIP(Order_SMC_SLM_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "10", "02");
        //    OrderSMCHeaderModels obj = new OrderSMCHeaderModels();
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

        public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        {
            //EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "10", "01");
            db_hv.Order_MAULT_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.ProductCategoryId = 10;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //if (value.FinalSubmission == true)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "10", "01");
        //    OrderMAULTHeaderModels obj = new OrderMAULTHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode; 
        //    obj.DateExpected = System.Convert.ToDateTime(value.DateExpected);
        //    obj.ProductCategoryId = 10;
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
            value.ProductCategoryId = 10;
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
        //EmailJob obx = new EmailJob();
        //public ActionResult DialogUpdateMAULT(Order_MAULT_Header value)
        //{
        //    OrderMAULTHeaderModels obj = new OrderMAULTHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode; 
        //    obj.DateExpected = System.Convert.ToDateTime(value.DateExpected);
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult UpdatePatientSummary(Order_SMC_SLM_Summary value)
        //{
        //    context.Configuration.ProxyCreationEnabled = false;
        //    mascisEntities db_hv = new mascisEntities();
        //    //OrderSMCHeaderModels obj = new OrderSMCHeaderModels();
        //    //OrderSMCPatientSummaryModel obj = new OrderSMCPatientSummaryModel();
        //    //string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "10", "02");
        //    var OrderNumber = TempData.Peek("OrderNumber").ToString();
        //    db_hv.Order_SMC_SLM_Summary.Add(value);
        //    value.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    db_hv.SaveChanges();

        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult UpdatePatientSummary(string key, List<spView_WebTemplate_SMC_SLM_Patient_SummaryGetAll_Result> changed, List<spView_WebTemplate_SMC_SLM_Patient_SummaryGetAll_Result> added, List<spView_WebTemplate_SMC_SLM_Patient_SummaryGetAll_Result> deleted)
        {
            // var order_no = TempData.Peek("OrderNumber").ToString();
            OrderSMCPatientSummaryModel obj = new OrderSMCPatientSummaryModel();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    var order_no = TempData.Peek("OrderNumber").ToString();
                    var fc = TempData.Peek("FacilityCode");
                    var sdate = TempData.Peek("StartDate");
                    var edate = TempData.Peek("EndDate");
                   
                    obj.OrderNumber = order_no;
                    obj.StartDate = System.Convert.ToDateTime(sdate);
                    obj.EndDate = System.Convert.ToDateTime(edate);
                    obj.FacilityCode = System.Convert.ToInt32(fc);
                    obj.ID = System.Convert.ToInt16(temp.ID);
                    obj.Quantity = temp.Quantity;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}