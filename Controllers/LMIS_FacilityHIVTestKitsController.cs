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

using System.Globalization;
using mascis.ScheduledTasks;

namespace mascis.Controllers
{
    public class LMIS_FacilityHIVTestKitsController : Controller
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
        //
        public ActionResult HSIPCustomOrders()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            //var Products = context.View_ProductClassification.Where(a => a.product_category == 3).OrderBy(a => a.product_description);
            var Products = context.A_Product.Where(a => a.product_category == 3).ToList();
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
            var Products = context.A_Product.Where(a => a.product_category == 3).ToList();
            ViewBag.ProductDataSource = Products;
            return View();
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
        
        public ActionResult DataSourceHSIPOrders(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.order_hiv_rapid_test_kit_header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.FinalSubmission,a.EditedBy,a.EditedDate, a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.EndDate).ToList();
            int count = context.order_hiv_rapid_test_kit_header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.FinalSubmission, a.EditedBy, a.EditedDate, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1 ) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.EndDate).ToList().Count();

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
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.order_hiv_rapid_test_kit_header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.FinalSubmission , a.RFSO_SentBackTofacility}).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.order_hiv_rapid_test_kit_header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId,a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            IEnumerable data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId , a.FinalSubmission, a.RFSO_SentBackTofacility}).Where(a => a.ProductCategoryId == 3 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null||a.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId,a.FinalSubmission , a.RFSO_SentBackTofacility}).Where(a => a.ProductCategoryId == 3 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null|| a.RFSO_SentBackTofacility== true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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

        public ActionResult DataSourceGetAll(DataManager dm)
        {
            HIVTestKitsModels obj = new HIVTestKitsModels();
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

        public ActionResult GetOrder(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            //string a = start_date.ToString();
            //string b = end_date.ToString();
            HIVTestKitsModels obj = new HIVTestKitsModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
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
        //New HIV Test Kits record
       
        public ActionResult GetOrder_HTK(Syncfusion.JavaScript.DataManager dm, int FC, string StartDate, string EndDate)
        {

            //  string format = "MM/dd/yyyy";
            DateTime? sDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
            DateTime? eDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);
             
            //string a = start_date.ToString();
            //string b = end_date.ToString();
            HIVTestKitsModels obj = new HIVTestKitsModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrder_Add( FC, sDate, eDate).ToList();
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
            HIVTestKitsModels obj = new HIVTestKitsModels();
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

            IEnumerable data = obj.GetOrder(param_on, 3);
            int count = obj.GetOrder(param_on, 3).Count();

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

        public ActionResult GetOrderSummary(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            //string a = start_date.ToString();
            //string b = end_date.ToString();
            HIVTestKitsModels obj = new HIVTestKitsModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrderSummary(param_on);
            int count = obj.GetOrderSummary(param_on).Count();

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

        public ActionResult Update(string key, List<spView_WebTemplate_HIV_Test_KitsGetAll_MA_WFA_Result> changed, List<spView_WebTemplate_HIV_Test_KitsGetAll_MA_WFA_Result> added, List<spView_WebTemplate_HIV_Test_KitsGetAll_MA_WFA_Result> deleted)
        {
            Formulas formulas = new Formulas();
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderHIVTestKitModels obj = new OrderHIVTestKitModels();
            //Perfrom Addtion
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.No_Test_Start_2Months = temp.No_Test_Start_2Months;
                    obj.Test_Recieved_2Months = temp.Test_Recieved_2Months;
                    obj.Test_Used_2Months = temp.Test_Used_2Months;
                    obj.Loss_Adjustment = temp.Loss_Adjustment;
                    obj.Test_Remaining = temp.Test_Remaining;
                    obj.DaysOutOfStockDuring2Months = temp.DaysOutOfStockDuring2Months;
                    obj.AdjustedAMC = ((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                    obj.Maximum_Stock = ((temp.Test_Remaining) / (temp.AdjustedAMC));//((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                    obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(((temp.AdjustedAMC) * 4) - (temp.Test_Remaining)));
                    obj.Quantity_Allocated = formulas.Allocated(temp.Quantity_Required);
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Notes = temp.Notes;
                    obj.Update();

                    if (temp.product_code == 130033)
                    {
                        obj = new OrderHIVTestKitModels();
                        obj.OrderNumber = order_no;
                        obj.ProductCode = 130001;
                        obj.No_Test_Start_2Months = temp.No_Test_Start_2Months;
                        obj.Test_Recieved_2Months = temp.Test_Recieved_2Months;
                        obj.Test_Used_2Months = temp.Test_Used_2Months;
                        obj.Loss_Adjustment = temp.Loss_Adjustment;
                        obj.Test_Remaining = temp.Test_Remaining;
                        obj.DaysOutOfStockDuring2Months = temp.DaysOutOfStockDuring2Months;
                        obj.AdjustedAMC = ((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                        obj.Maximum_Stock = ((temp.Test_Remaining) / (temp.AdjustedAMC));//((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                        obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(((temp.AdjustedAMC) * 4) - (temp.Test_Remaining)));
                        obj.Quantity_Allocated = formulas.Allocated(temp.Quantity_Required);
                        //obj.AdjustedAMC = (temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months);
                        //obj.Maximum_Stock = (temp.Test_Remaining) / ((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                        //obj.Quantity_Required = Convert.ToDouble(formulas.Allocated((((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months)) * 4) - (temp.Test_Remaining)));
                        //obj.Quantity_Allocated = formulas.Allocated((((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months)) * 4) - (temp.Test_Remaining));
                        obj.RFSONotes = temp.RFSONotes;
                        obj.Notes = temp.Notes;
                        obj.Update();
                    }
                }
            }
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.No_Test_Start_2Months = temp.No_Test_Start_2Months;
                    obj.Test_Recieved_2Months = temp.Test_Recieved_2Months;
                    obj.Test_Used_2Months = temp.Test_Used_2Months;
                    obj.Loss_Adjustment = temp.Loss_Adjustment;
                    obj.Test_Remaining = temp.Test_Remaining;
                    obj.DaysOutOfStockDuring2Months = temp.DaysOutOfStockDuring2Months;
                    obj.AdjustedAMC = ((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                    obj.Maximum_Stock = ((temp.Test_Remaining) / (temp.AdjustedAMC));//((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                    obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(((temp.AdjustedAMC) * 4) - (temp.Test_Remaining)));
                    obj.Quantity_Allocated = formulas.Allocated(temp.Quantity_Required);
                    //obj.AdjustedAMC = (temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months);
                    //obj.Maximum_Stock = (temp.Test_Remaining) / ((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                    //obj.Quantity_Required = Convert.ToDouble(formulas.Allocated((((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months)) * 4) - (temp.Test_Remaining)));
                    //obj.Quantity_Allocated = formulas.Allocated((((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months)) * 4) - (temp.Test_Remaining));
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Notes = temp.Notes;
                    obj.Update();
                    if (temp.product_code == 130033)
                    {
                        obj = new OrderHIVTestKitModels();
                        obj.OrderNumber = order_no;
                        obj.ProductCode = 130001;
                        obj.No_Test_Start_2Months = temp.No_Test_Start_2Months;
                        obj.Test_Recieved_2Months = temp.Test_Recieved_2Months;
                        obj.Test_Used_2Months = temp.Test_Used_2Months;
                        obj.Loss_Adjustment = temp.Loss_Adjustment;
                        obj.Test_Remaining = temp.Test_Remaining;
                        obj.DaysOutOfStockDuring2Months = temp.DaysOutOfStockDuring2Months;
                        obj.AdjustedAMC = ((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                        obj.Maximum_Stock = ((temp.Test_Remaining) / (temp.AdjustedAMC));//((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                        obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(((temp.AdjustedAMC) * 4) - (temp.Test_Remaining)));
                        obj.Quantity_Allocated = formulas.Allocated(temp.Quantity_Required);
                        //obj.AdjustedAMC = (temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months);
                        //obj.Maximum_Stock = (temp.Test_Remaining) / ((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                        //obj.Quantity_Required = Convert.ToDouble(formulas.Allocated((((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months)) * 4) - (temp.Test_Remaining)));
                        //obj.Quantity_Allocated =(formulas.Allocated((((temp.Test_Used_2Months * 30) / (60 - temp.DaysOutOfStockDuring2Months)) * 4) - (temp.Test_Remaining)));
                        obj.RFSONotes = temp.RFSONotes;
                        obj.Notes = temp.Notes;
                        obj.Update();
                    }
                }
            }
            //var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public int getFacilitySector( int facilitycode)
        {
            var facilities = context.A_Facilities.FirstOrDefault(f => f.FacilityCode == facilitycode);
            var zcode = facilities.DeliveryZoneCode;
            return System.Convert.ToInt32(zcode);
            //retu
        }
        public ActionResult UpdateCustom(string key, List<spView_WebTemplate_HIV_Test_KitsGetAll_Custom1_Result> changed, List<spView_WebTemplate_HIV_Test_KitsGetAll_Custom1_Result> added, List<spView_WebTemplate_HIV_Test_KitsGetAll_Custom1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderHIVTestKitModels obj = new OrderHIVTestKitModels();
            ////Performing Additon operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.Quantity_Required = temp.Quantity_Required;
                    obj.Quantity_Allocated = System.Convert.ToInt32(temp.Quantity_Required);
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Notes = temp.Notes;
                    obj.Update();
                    if (temp.product_code == 130033)
                    {
                        obj = new OrderHIVTestKitModels();
                        obj.OrderNumber = order_no;
                        obj.ProductCode = 130001;
                        obj.Quantity_Required = temp.Quantity_Required;
                        obj.Quantity_Allocated = System.Convert.ToInt32(temp.Quantity_Required);
                        obj.RFSONotes = temp.RFSONotes;
                        obj.Notes = temp.Notes;
                        obj.Update();
                    }
                }
            }
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.Quantity_Required = temp.Quantity_Required;
                    obj.Quantity_Allocated = System.Convert.ToInt32(temp.Quantity_Required);
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Notes = temp.Notes;
                    obj.Update();
                    if (temp.product_code == 130033)
                    {
                        obj = new OrderHIVTestKitModels();
                        obj.OrderNumber = order_no;
                        obj.ProductCode = 130001;
                        obj.Quantity_Required = temp.Quantity_Required;
                        obj.Quantity_Allocated = System.Convert.ToInt32(temp.Quantity_Required);
                        obj.RFSONotes = temp.RFSONotes;
                        obj.Notes = temp.Notes;
                        obj.Update();
                    }
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
            // var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
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
                    obj.Quantity_Allocated = temp.Quantity_Allocated;
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSummary(string key, List<spView_WebTemplate_HIV_Test_Kits_SummaryGetAll_MA_WFA_Result> changed, List<spView_WebTemplate_HIV_Test_Kits_SummaryGetAll_MA_WFA_Result> added, List<spView_WebTemplate_HIV_Test_Kits_SummaryGetAll_MA_WFA_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderHIVTestKitSummaryModels obj = new OrderHIVTestKitSummaryModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.HCT = temp.HCT;
                    obj.PMTCT = temp.PMTCT;
                    obj.Clinical_Diagnosis = temp.Clinical_Diagnosis;
                    obj.SMC = temp.SMC;
                    obj.Quality_Control = temp.Quality_Control;
                    obj.Total = (temp.HCT+ temp.PMTCT+ temp.Clinical_Diagnosis+ temp.SMC+ temp.Quality_Control);
                    obj.Update();
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
            ///var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
        }

        public class DropdownList
        {
            public int ID { get; set; }
            public string DESC { get; set; }
        }

        //public ActionResult DialogUpdateHSIP(order_hiv_rapid_test_kit_header value)
        //{
        //    OrderHIVTestKitHeaderModels obj = new OrderHIVTestKitHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = System.Convert.ToDateTime(value.StartDate);
        //    obj.EndDate = System.Convert.ToDateTime(value.EndDate);
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = 1;
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.FinalSubmission = System.Convert.ToBoolean( value.FinalSubmission);
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DialogUpdateHSIP(order_hiv_rapid_test_kit_header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            order_hiv_rapid_test_kit_header table = db_master.order_hiv_rapid_test_kit_header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
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
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber,"HIV Test Kits");
            }
            if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == true)
            {
                email.OrderReSentToScto(value.FacilityCode, value.OrderNumber, "HIV Test Kits");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DialogInsertHSIP(order_hiv_rapid_test_kit_header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderHIVTestKitHeaderModels obj = new OrderHIVTestKitHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "03", "02");

            db_hv.order_hiv_rapid_test_kit_header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 2;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
            if (value.FinalSubmission == true)
            {
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber, "HIV Test Kits");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
             
        }

        //public ActionResult DialogInsertHSIP(order_hiv_rapid_test_kit_header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "03", "02");
        //    OrderHIVTestKitHeaderModels obj = new OrderHIVTestKitHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = System.Convert.ToDateTime(value.StartDate);
        //    obj.EndDate = System.Convert.ToDateTime(value.EndDate);
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = 1;
        //    //obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogInsertHSIPCustom(order_hiv_rapid_test_kit_header value)
        {
            //EmailJob obx = new EmailJob();
           
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderHIVTestKitHeaderModels obj = new OrderHIVTestKitHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "03", "02");

            db_hv.order_hiv_rapid_test_kit_header.Add(value);
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
            //if (value.FinalSubmission == true)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //obx.Schedule();
            //return Json(value, JsonRequestBehavior.AllowGet);
            return Json(new
             {
                 msg = "Successfully added " + value.OrderNumber,
                 JsonRequestBehavior.AllowGet
             });

        }
        //public ActionResult DialogInsertHSIPCustom(order_hiv_rapid_test_kit_header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "03", "02");
        //    OrderHIVTestKitHeaderModels obj = new OrderHIVTestKitHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.EndDate = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.DateExpected = System.Convert.ToDateTime(value.DateExpected);
        //    obj.OrderTypeId = 1;
        //    obj.OrderStatusId = 1;
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    //obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult DialogUpdateCustomHSIP(order_hiv_rapid_test_kit_header value)
        {
          //  EmailJob obx = new EmailJob();
            mascisEntities db_master = new mascisEntities();
            order_hiv_rapid_test_kit_header table = db_master.order_hiv_rapid_test_kit_header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
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
            //return Json(value, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                msg = "Successfully added " + value.OrderNumber,
                JsonRequestBehavior.AllowGet
            });
        }
        public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        {
           // EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "03", "01");
            db_hv.Order_MAULT_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.ProductCategoryId = 3;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //if (value.FinalSubmission==true && value.RFSO_SentBackTofacility == false)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertMAULT(Order_MAULT_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "03", "01");
        //    OrderMAULTHeaderModels obj = new OrderMAULTHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.FacilityCode = value.FacilityCode; 
        //    obj.DateExpected = System.Convert.ToDateTime(value.DateExpected);
        //    obj.ProductCategoryId =3;
        //    obj.OrderStatusId = 1;
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DialogUpdateMAULT(Order_MAULT_Header value)
        {
            //EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_MAULT_Header table = db_master.Order_MAULT_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            value.OrderStatusId = 1;
            value.ProductCategoryId = 3;
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
        //    obj.DateExpected = System.Convert.ToDateTime(value.DateExpected);
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        
    }
}