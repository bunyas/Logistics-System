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
    public class LMIS_FacilityLaboratoryController : Controller
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
            var Products = context.View_ProductClassification.Where(a => a.product_category == 1).OrderBy(a => a.product_description);
            //var Products = context.A_Product.Where(a => a.product_category == 1).ToList();
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
            var Products = context.A_Product.Where(a => a.product_category == 1).ToList();
            ViewBag.ProductDataSource = Products;
            return View();
        }
        public int getFacilitySector(int facilitycode)
        {
            var facilities = context.A_Facilities.FirstOrDefault(f => f.FacilityCode == facilitycode);
            var zcode = facilities.DeliveryZoneCode;
            return System.Convert.ToInt32(zcode);
            //retu
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

        public ActionResult DataSourceHSIPOrders(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_Lab_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.start_date, a.end_date, a.date_completed, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId,a.FinalSubmission,a.EditedBy,a.EditedDate,a.RFSO_SentBackTofacility,a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.end_date).ToList();
            int count = context.Order_Lab_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.start_date, a.end_date, a.date_completed, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.FinalSubmission, a.EditedBy, a.EditedDate, a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission == false || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.end_date).ToList().Count();

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

        //public ActionResult DataSourceHSIPCustomOrders(DataManager dm)
        //{
        //    int? facility_id = -1;
        //    if (new UserManagement().getCurrentuserFacility() != null)
        //        facility_id = new UserManagement().getCurrentuserFacility();
        //    context.Configuration.ProxyCreationEnabled = false;
        //    IEnumerable data = context.View_Order_Lab_HSIP_Custom_Orders.Select(a => new { a.OrderNumber, a.FacilityCode, a.start_date, a.end_date, a.date_completed, a.DistrrictCode, a.DeliveryZoneCode, a.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.FinalSubmission,a.EditedBy,a.EditedDate, a.DateExpected}).Where(b => b.FacilityTypeId == "02" && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false )).OrderByDescending(a => a.date_completed).ToList();
        //    int count = context.View_Order_Lab_HSIP_Custom_Orders.Select(a => new { a.OrderNumber, a.FacilityCode, a.start_date, a.end_date, a.date_completed, a.DistrrictCode, a.DeliveryZoneCode, a.FacilityTypeId, a.OrderTypeId,a.OrderStatusId , a.FinalSubmission, a.EditedBy, a.EditedDate, a.DateExpected }).Where(b => b.FacilityTypeId == "02" && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false )).OrderByDescending(a => a.date_completed).ToList().Count();

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

        public ActionResult DataSourceHSIPCustomOrders(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.View_Order_Lab_Custom_Orders.Select(a => new { a.OrderNumber, a.FacilityCode,  a.DatePrepared, a.DistrrictCode, a.DeliveryZoneCode, a.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.FinalSubmission, a.EditedBy, a.EditedDate, a.DateExpected , a.RFSO_SentBackTofacility}).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false || b.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.View_Order_Lab_Custom_Orders.Select(a => new { a.OrderNumber, a.FacilityCode,  a.DatePrepared, a.DistrrictCode, a.DeliveryZoneCode, a.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.FinalSubmission, a.EditedBy, a.EditedDate, a.DateExpected, a.RFSO_SentBackTofacility }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false || b.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            IEnumerable data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId, a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 1 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList();
            int count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId,a.OrderStatusId,a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 1 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null|| a.RFSO_SentBackTofacility==true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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
            LaboratoryModels obj = new LaboratoryModels();
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
            LaboratoryModels obj = new LaboratoryModels();
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

       
        public ActionResult GetOrder_Lab(Syncfusion.JavaScript.DataManager dm, int FC, string StartDate, string EndDate)
        {
            DateTime? sDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
            DateTime? eDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);

            LaboratoryModels obj = new LaboratoryModels();
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
            LaboratoryModels obj = new LaboratoryModels();
            //IEnumerable DataSource = obj.getOrders(18, DateTime.Parse("2017-04-01 00:00:00.000"), DateTime.Parse("2017-05-31 00:00:00.000"));
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = obj.GetOrderCustom_LAB(param_on);
            int count = obj.GetOrderCustom_LAB(param_on).Count();

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

            IEnumerable data = obj.GetOrder(param_on, 1);
            int count = obj.GetOrder(param_on, 1).Count();

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
        public ActionResult UpdateCustom(string key, List<spView_WebTemplate_Lab_CustomGetAll_Result> changed, List<spView_WebTemplate_Lab_CustomGetAll_Result> added, List<spView_WebTemplate_Lab_CustomGetAll_Result> deleted)
        {
            Formulas formulas = new Formulas();
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderLaboratoryCustomModels obj = new OrderLaboratoryCustomModels();
            ////Performing Additon operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.ProductCode = System.Convert.ToInt32(temp.product_code);
                    obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                    obj.Quantity_Allocated = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                    if (temp.product_code == 130018)
                    {
                        obj = new OrderLaboratoryCustomModels();
                        obj.OrderNumber = order_no;
                      // var lab_cat = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == 140029);
                        obj.ProductCode = 140029/*System.Convert.ToInt32(lab_cat.product_category_code)*/;
                        obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                        obj.Quantity_Allocated = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                        obj.Notes = temp.Notes;
                        obj.RFSONotes = temp.RFSONotes;
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
                    obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                    obj.Quantity_Allocated = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                    obj.Notes = temp.Notes;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                    if (temp.product_code == 20)
                    {
                        obj = new OrderLaboratoryCustomModels();
                        obj.OrderNumber = order_no;
                        var lab_cat = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == 140029);
                        obj.ProductCode = System.Convert.ToInt32(lab_cat.product_category_code);
                        obj.Quantity_Required = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                        obj.Quantity_Allocated = Convert.ToDouble(formulas.Allocated(temp.Quantity_Required));
                        obj.Notes = temp.Notes;
                        obj.RFSONotes = temp.RFSONotes;
                        obj.Update();
                    }
                }
            }
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
            // var data = 0; return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, List<spView_WebTemplate_LaboratoryGetAll_MA_WFA_Result> changed, List<spView_WebTemplate_LaboratoryGetAll_MA_WFA_Result> added, List<spView_WebTemplate_LaboratoryGetAll_MA_WFA_Result> deleted)
        {
            Formulas formulas = new Formulas();
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderLaboratoryModels obj = new OrderLaboratoryModels();
            ////Performing Addation operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    obj.OrderNumber = order_no;
                    obj.ItemCode = System.Convert.ToInt32(temp.product_code);
                    obj.opening_balance = temp.opening_balance;
                    obj.quantity_recieved = temp.quantity_recieved;
                    obj.consumption = temp.consumption;
                    obj.losses_adjustment = temp.losses_adjustment;
                    obj.closing_balance = temp.closing_balance;
                    obj.quantity_to_order = Convert.ToDouble(formulas.Allocated((2* temp.consumption)- (temp.closing_balance)));
                    obj.quantity_allocated = (formulas.Allocated(temp.quantity_to_order));
                    //obj.quantity_allocated = (formulas.Allocated((2 * temp.consumption) - temp.closing_balance));
                    obj.total_cost = temp.total_cost;
                    obj.comments = temp.comments;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                    if (temp.product_code == 20)
                    {
                        obj = new OrderLaboratoryModels();
                        obj.OrderNumber = order_no;
                        var lab_cat = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == 140029);
                        obj.ItemCode = System.Convert.ToInt32(lab_cat.product_category_code);
                        obj.opening_balance = temp.opening_balance;
                        obj.quantity_recieved = temp.quantity_recieved;
                        obj.consumption = temp.consumption;
                        obj.losses_adjustment = temp.losses_adjustment;
                        obj.closing_balance = temp.closing_balance;
                        obj.quantity_to_order = Convert.ToDouble(formulas.Allocated((2 * temp.consumption) - (temp.closing_balance)));
                        obj.quantity_allocated = (formulas.Allocated(temp.quantity_to_order));
                        obj.total_cost = temp.total_cost;
                        obj.comments = temp.comments;
                        obj.RFSONotes = temp.RFSONotes;
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
                    obj.ItemCode = System.Convert.ToInt32(temp.product_code);
                    obj.opening_balance = temp.opening_balance;
                    obj.quantity_recieved = temp.quantity_recieved;
                    obj.consumption = temp.consumption;
                    obj.losses_adjustment = temp.losses_adjustment;
                    obj.closing_balance = temp.closing_balance;
                    obj.quantity_to_order = Convert.ToDouble(formulas.Allocated((2 * temp.consumption) - (temp.closing_balance)));
                    obj.quantity_allocated = (formulas.Allocated(temp.quantity_to_order));
                    obj.total_cost = temp.total_cost;
                    obj.comments = temp.comments;
                    obj.RFSONotes = temp.RFSONotes;
                    obj.Update();
                    if (temp.product_code == 20)
                    {
                        obj = new OrderLaboratoryModels();
                        obj.OrderNumber = order_no;
                        var lab_cat = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == 140029);
                        obj.ItemCode = System.Convert.ToInt32(lab_cat.product_category_code);
                        obj.opening_balance = temp.opening_balance;
                        obj.quantity_recieved = temp.quantity_recieved;
                        obj.consumption = temp.consumption;
                        obj.losses_adjustment = temp.losses_adjustment;
                        obj.closing_balance = temp.closing_balance;
                        obj.quantity_to_order = Convert.ToDouble(formulas.Allocated((2 * temp.consumption) - (temp.closing_balance)));
                        obj.quantity_allocated = (formulas.Allocated(temp.quantity_to_order));
                        obj.total_cost = temp.total_cost;
                        obj.comments = temp.comments;
                        obj.RFSONotes = temp.RFSONotes;
                        obj.Update();
                    }
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

        //public ActionResult DialogUpdateHSIP(Order_Lab_Header value)
        //{
        //    OrderLaboratoryHeaderModels obj = new OrderLaboratoryHeaderModels();
        //    obj.OrderNumber = value.OrderNumber;
        //    TempData["OrderNumber"] = value.OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.date_completed);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = value.start_date;
        //    obj.EndDate = value.end_date;
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DialogUpdateHSIP(Order_Lab_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_Lab_Header table = db_master.Order_Lab_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
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
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber, "Laboratory");
            }
            if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == true)
            {
                email.OrderReSentToScto(value.FacilityCode, value.OrderNumber, "Laboratory");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertHSIP(Order_Lab_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.date_completed), value.FacilityCode, "01", "02");
        //    OrderLaboratoryHeaderModels obj = new OrderLaboratoryHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.date_completed);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = value.start_date;
        //    obj.EndDate = value.end_date;
        //    obj.OrderTypeId = 2;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}  
       // EmailJob obx = new EmailJob();
        public ActionResult DialogInsertHSIP(Order_Lab_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderLaboratoryHeaderModels obj = new OrderLaboratoryHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.date_completed), value.FacilityCode, "01", "02");

            db_hv.Order_Lab_Header.Add(value);
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
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber,"Laboratory");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DialogInsertHSIPCustom(Order_Lab_Header value)
        //{
        //    string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.date_completed), value.FacilityCode, "01", "02");
        //    OrderLaboratoryHeaderModels obj = new OrderLaboratoryHeaderModels();
        //    obj.OrderNumber = OrderNumber;
        //    TempData["OrderNumber"] = OrderNumber;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.date_completed);
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.StartDate = value.start_date;
        //    obj.EndDate = value.end_date;
        //    obj.OrderStatusId = 1;
        //    obj.OrderTypeId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogInsertHSIPCustom(Order_Lab_Custom_Header value)
        {
            //EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderLaboratoryCustomHeaderModels obj = new OrderLaboratoryCustomHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "01", "02");

            db_hv.Order_Lab_Custom_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.OrderTypeId = 1;
            value.OrderStatusId = 1;
            value.ProductCategoryId = 1;
            //value.start_date = DateTime.Now;
            //value.end_date = DateTime.Now;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //obj.SaveLog(value);
            //if (value.FinalSubmission == true)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogUpdateCustomHSIP(Order_Lab_Custom_Header value)
        {
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_Lab_Custom_Header table = db_master.Order_Lab_Custom_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            value.OrderTypeId = 1;
            value.OrderStatusId = 1;
            value.ProductCategoryId = 1;
            //value.start_date = DateTime.Now;
            //value.end_date = DateTime.Now;
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
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "01");
            db_hv.Order_MAULT_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.ProductCategoryId = 1;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //if (value.FinalSubmission == true)
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            //obx.SendEmail(value.FacilityCode, value.OrderNumber);
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
        //    obj.ProductCategoryId = 1;
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
            value.ProductCategoryId = 1;
            value.OrderStatusId = 1;
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
        //    obj.FacilityCode = value.FacilityCode;
        //    obj.DatePrepared = System.Convert.ToDateTime(value.DatePrepared);
        //    obj.DateExpected = value.DateExpected;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Update();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult GetDistrictData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_District.AsNoTracking().OrderBy(a => a.District_Name).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSectorData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_DeliveryZone.AsNoTracking().OrderBy(a => a.ZoneDescription).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HH_members(string product)
        {
            MembersRepository mm = new MembersRepository();
            //var household_id = this.GetHousehold_Id(household_code);

            var members = mm.GetHH_Members(product).ToList();
            var Members = new List<object>();
            foreach (var id in members)
            {
                Members.Add(new { value = id.Value, text = id.Text });
            }
            return Json(Members, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMember(string memId, string household_code)
        {
            //string hh_id = this.GetHousehold_Id(household_code);
            var members = context.View_WebTemplate_Laboratory.AsNoTracking().Where(h => h.product_order.ToString() == memId && h.original_product_code.ToString() == household_code);
            return Json(members, JsonRequestBehavior.AllowGet);
        }
    }
}