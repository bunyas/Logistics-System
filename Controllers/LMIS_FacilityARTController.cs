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

namespace mascis.Controllers
{
    public class LMIS_FacilityARTController : Controller
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
            var facility = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var product = context.View_ProductClassification.Where(a => a.product_category == 2).OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;
           
            return View();
        }

        public ActionResult MAULTOrders()
        {
            TempData["OrderNumber"] = "";
            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            context.Configuration.ProxyCreationEnabled = false;
            var Products = context.A_Product.Where(a => a.product_category == 2).ToList();
            ViewBag.ProductDataSource = Products;
            return View();
        }

        public JsonResult GetFacilityData()
        {
            int? x = -1;
            if(new UserManagement().getCurrentuserFacility()!=null)
            {
                x = new UserManagement().getCurrentuserFacility();
            }
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_Facilities.AsNoTracking().Where(a=>a.FacilityCode==x).OrderBy(a => a.Facility).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DataSourceHSIPOrders(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();
            context.Configuration.ProxyCreationEnabled = false;

           

            IEnumerable data = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId , a.FinalSubmission, a.EditedBy,a.EditedDate,a.RFSO_SentBackTofacility,a.EmergencyOrder}
             

            ).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission== false  || b.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.EndDate).ToList();
            int count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.StartDate, a.EndDate, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.FinalSubmission, a.EditedBy, a.EditedDate,a.RFSO_SentBackTofacility, a.EmergencyOrder }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 2 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.EndDate).ToList().Count();

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
            var RFSOfacilities = new UserManagement().getUserFacilityList();
            context.Configuration.ProxyCreationEnabled = false;

            IEnumerable data = context.Order_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;

            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                data = context.Order_Header.Select(a => new {
                    a.OrderNumber,
                    a.FacilityCode,
                    a.DatePrepared,
                    a.A_Facilities.DistrrictCode,
                    a.A_Facilities.DeliveryZoneCode,
                    a.FinalSubmission,//.Where(e=>e.FinalSubmission==false)
                    a.A_Facilities.FacilityTypeId,
                    a.OrderTypeId,
                    a.OrderStatusId,
                    a.DateExpected
                }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false )).OrderByDescending(a => a.DatePrepared).ToList();

                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.DateExpected, a.FinalSubmission }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1)).OrderByDescending(a => a.DatePrepared).ToList().Count();

            }
            else
            {
                data = context.Order_Header.Select(a => new {
                    a.OrderNumber,
                    a.FacilityCode,
                    a.DatePrepared,
                    a.A_Facilities.DistrrictCode,
                    a.A_Facilities.DeliveryZoneCode,
                    a.FinalSubmission,//.Where(e=>e.FinalSubmission==false)
                    a.A_Facilities.FacilityTypeId,
                    a.OrderTypeId,
                    a.OrderStatusId,
                    a.DateExpected,a.RFSO_SentBackTofacility
                }).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId, a.OrderStatusId, a.DateExpected, a.FinalSubmission , a.RFSO_SentBackTofacility}).Where(b => (b.FacilityTypeId == "02" || b.FacilityTypeId == "03") && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false  || b.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList().Count();
            }


            //IEnumerable data = context.Order_Header.Select(a => new { a.OrderNumber,
            //    a.FacilityCode,
            //    a.DatePrepared,
            //    a.A_Facilities.DistrrictCode,
            //    a.A_Facilities.DeliveryZoneCode,a.FinalSubmission,//.Where(e=>e.FinalSubmission==false)
            //    a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.DateExpected }).Where(b => b.FacilityTypeId == "02" && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1) && (b.FinalSubmission == false )).OrderByDescending(a => a.DatePrepared).ToList();
            //int count = context.Order_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.OrderTypeId,a.OrderStatusId, a.DateExpected ,a.FinalSubmission}).Where(b => b.FacilityTypeId == "02" && b.FacilityCode == facility_id && b.OrderTypeId == 1 && (b.OrderStatusId == 1)).OrderByDescending(a => a.DatePrepared).ToList().Count();

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

            //To take care of RFSO in several facilities --RFSOfacilities
            var RFSOfacilities = new UserManagement().getUserFacilityList();

            context.Configuration.ProxyCreationEnabled = false;

            IEnumerable data = context.Order_MAULT_Header.Where(a => a.FacilityCode == 9999999).ToList();
            int count = 0;

            var muserrole = User.IsInRole("SCTO");
            if (muserrole)
            {
                 data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission }).Where(a => a.ProductCategoryId == 2 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null)).OrderByDescending(a => a.DatePrepared).ToList();
                 count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission }).Where(a => a.ProductCategoryId == 2 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null)).OrderByDescending(a => a.DatePrepared).ToList().Count();

            }
            else
            {
                data = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && a.FacilityCode == facility_id && (a.OrderStatusId == 1 || a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList();//.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 1 && a.FacilityCode == facility_id && (a.OrderStatusId == 1 || a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList();
                count = context.Order_MAULT_Header.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && a.FacilityCode == facility_id && (a.OrderStatusId == 1 || a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList().Count();//.Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 1 && a.FacilityCode == facility_id && (a.OrderStatusId == 1 || a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList().Count();

                //data = context.Order_MAULT_Header.Where(BuildContainsExpression<Order_MAULT_Header, int>(e => (int)e.FacilityCode , RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission, a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList();
                //count = context.Order_MAULT_Header.Where(BuildContainsExpression<Order_MAULT_Header, int>(e => (int)e.FacilityCode, RFSOfacilities)).Select(a => new { a.OrderNumber, a.FacilityCode, a.DateExpected, a.DatePrepared, a.A_Facilities.DistrrictCode, a.A_Facilities.DeliveryZoneCode, a.A_Facilities.FacilityTypeId, a.ProductCategoryId, a.OrderStatusId, a.FinalSubmission,a.RFSO_SentBackTofacility }).Where(a => a.ProductCategoryId == 2 && a.FacilityCode == facility_id && (a.OrderStatusId == 1|| a.OrderStatusId == null) && (a.FinalSubmission == false || a.FinalSubmission == null || a.RFSO_SentBackTofacility == true)).OrderByDescending(a => a.DatePrepared).ToList().Count();
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
            ARTModels obj = new ARTModels();
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
            ARTModels obj = new ARTModels();

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
        public ActionResult GetOrder_Add(Syncfusion.JavaScript.DataManager dm, int FC, string StartDate, string EndDate)
        {
            DateTime? sDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
            DateTime? eDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);

            ARTModels obj = new ARTModels();
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
            ARTModels obj = new ARTModels();

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

            IEnumerable data = obj.GetOrder(param_on, 2);
            int count = obj.GetOrder(param_on, 2).Count();

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

            IEnumerable data = obj.GetOrderPatients(param_on);
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

            IEnumerable data = obj.GetOrderPatientsPaediatric(param_on);
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

            IEnumerable data = obj.GetOrderPatientsOther(param_on);
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

            IEnumerable data = obj.GetOrderPatientsExtraRegimen(1, param_on);
            int count = obj.GetOrderPatientsExtraRegimen(1, param_on).Count();

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

        public ActionResult GetOrderPaediatricExtraRegimen(Syncfusion.JavaScript.DataManager dm, string param_on)
        {
            ARTModels obj = new ARTModels();

            IEnumerable data = obj.GetOrderPatientsExtraRegimen(2, param_on);
            int count = obj.GetOrderPatientsExtraRegimen(2, param_on).Count();

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

        public ActionResult Update(string key, List<spView_WebTemplate_ARVGetAll_MA_WFA_Result> changed, List<spView_WebTemplate_ARVGetAll_MA_WFA_Result> added, List<spView_WebTemplate_ARVGetAll_MA_WFA_Result> deleted)
        {
            Formulas formulas = new Formulas();
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTModels obj = new OrderARTModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.DrugCode = System.Convert.ToInt32(temp.product_code);
                    obj.OpeningBalance = temp.OpeningBalance;
                    obj.QuantityRecieved = temp.QuantityRecieved;
                    obj.Losses_Adjustments = temp.Losses_Adjustments;
                    obj.ClosingBalance = temp.ClosingBalance;
                    //obj.Months_Stock_atHand = temp.Months_Stock_atHand;
                    obj.ART_eMTCT_Consumption = temp.ART_eMTCT_Consumption;
                    //obj.Quantity_Required_Current_Patients = temp.Quantity_Required_Current_Patients;
                    
                    obj.DaysOutOfStockDuring2Months = temp.DaysOutOfStockDuring2Months;
                    obj.AdjustedAMC = (temp.ART_eMTCT_Consumption*30)/(60- temp.DaysOutOfStockDuring2Months);
                    obj.Months_Stock_atHand = ((temp.ClosingBalance) / (temp.AdjustedAMC));
                    //obj.Months_Stock_atHand = ((temp.ClosingBalance)/(temp.ART_eMTCT_Consumption * 30) / (60 - temp.DaysOutOfStockDuring2Months));
                    obj.Quantity_Required_Current_Patients = Convert.ToDouble(formulas.Allocated((4 * (temp.ART_eMTCT_Consumption * 30) / (60 - temp.DaysOutOfStockDuring2Months)) - temp.ClosingBalance));
                    //obj.Quantity_Required_Current_Patients = (4 * (temp.ART_eMTCT_Consumption * 30) / (60 - temp.DaysOutOfStockDuring2Months)) - temp.ClosingBalance;
                    obj.Quantity_Allocated = obj.Quantity_Required_Current_Patients;
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
                    obj.Quantity_Allocated = temp.Quantity_Required_Current_Patients;
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
                    obj.Quantity_Allocated = temp.Quantity_Required_Current_Patients;
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

        public ActionResult UpdatePatients(string key, List<spView_WebTemplate_ARV_PatientsGetAll1_Result> changed, List<spView_WebTemplate_ARV_PatientsGetAll1_Result> added, List<spView_WebTemplate_ARV_PatientsGetAll1_Result> deleted)
        {
            var order_no = TempData.Peek("OrderNumber").ToString();
            OrderARTPatientModels obj = new OrderARTPatientModels();
            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    obj.OrderNumber = order_no;
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
                    obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.RegimenCode = System.Convert.ToInt16(temp.regimen_code);
                    obj.Existing_0_3Yrs = temp.Existing_0_3Yrs;
                    obj.New_0_3Yrs = temp.New_0_3Yrs;
                    obj.Existing_3_10Yrs = temp.Existing_3_10Yrs;
                    obj.New_3_10Yrs = temp.Existing_3_10Yrs;
                    obj.Existing_10_15Yrs_Less35Kg = temp.Existing_10_15Yrs_Less35Kg;
                    obj.New_10_15Yrs_Less35Kg = temp.New_10_15Yrs_Less35Kg;
                    obj.Existing_10_15Yrs_Greater35Kg = temp.Existing_10_15Yrs_Greater35Kg;
                    obj.New_10_15Yrs_Greater35Kg = temp.Existing_10_15Yrs_Greater35Kg;
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
                    obj.OrderNumber = order_no;
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
                    obj.OrderNumber = order_no;
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
                    obj.OrderNumber = order_no;
                    //obj.StartDate = System.Convert.ToDateTime(temp.StartDate);
                    //obj.EndDate = System.Convert.ToDateTime(temp.EndDate);
                    //obj.FacilityCode = System.Convert.ToInt32(temp.FacilityCode);
                    obj.RegimenCode = System.Convert.ToInt16(temp.RegimenCode);
                    obj.Existing_0_3Yrs = temp.Existing_0_3Yrs;
                    obj.New_0_3Yrs = temp.New_0_3Yrs;
                    obj.Existing_3_10Yrs = temp.Existing_3_10Yrs;
                    obj.New_3_10Yrs = temp.Existing_3_10Yrs;
                    obj.Existing_10_15Yrs_Less35Kg = temp.Existing_10_15Yrs_Less35Kg;
                    obj.New_10_15Yrs_Less35Kg = temp.New_10_15Yrs_Less35Kg;
                    obj.Existing_10_15Yrs_Greater35Kg = temp.Existing_10_15Yrs_Greater35Kg;
                    obj.New_10_15Yrs_Greater35Kg = temp.Existing_10_15Yrs_Greater35Kg;
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

        //public ActionResult DialogUpdateHSIP(Order_Header value)
        //{
        //    OrderARTHeaderModels obj = new OrderARTHeaderModels();
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
        public ActionResult DialogUpdateHSIP(Order_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
            TempData["OrderNumber"] = value.OrderNumber;
            value.OrderTypeId = 2;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_master.Entry(table).CurrentValues.SetValues(value);
            db_master.Entry(table).State = EntityState.Modified;
            db_master.SaveChanges();
            if(value.FinalSubmission == true && value.RFSO_SentBackTofacility == false)
            {
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber, "ARVs");
            }
            if (value.FinalSubmission == true && value.RFSO_SentBackTofacility == true)
            {
                email.OrderReSentToScto(value.FacilityCode, value.OrderNumber,"ARVs");
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogInsertHSIP(Order_Header value)
        {
            EmailJob email = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderARTHeaderModels obj = new OrderARTHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "02");
           
            db_hv.Order_Header.Add(value);
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
                email.OrderSentToScto(value.FacilityCode, value.OrderNumber,"ARVs");
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
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DialogInsertHSIPCustom(Order_Header value)
        {
            //EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            OrderARTHeaderModels obj = new OrderARTHeaderModels();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "02");

            db_hv.Order_Header.Add(value);
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
            //if (value.FinalSubmission == true )
            //{
            //    obx.SendEmail(value.FacilityCode, value.OrderNumber);
            //}
            // obx.SendEmail(value.FacilityCode, value.OrderNumber);
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
        //    obj.DateExpected = System.Convert.ToDateTime(value.DateExpected);
        //    obj.OrderTypeId = 1;
        //    obj.OrderStatusId = 1;
        //    obj.FinalSubmission = System.Convert.ToBoolean(value.FinalSubmission);
        //    obj.EditedBy = new UserManagement().getCurrentuser();
        //    obj.EditedDate = DateTime.Now;
        //    obj.Save();
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DialogUpdateCustomHSIP(Order_Header value)
        { 
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            Order_Header table = db_master.Order_Header.FirstOrDefault(o => o.OrderNumber == value.OrderNumber);
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
           // EmailJob obx = new EmailJob();
            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_hv = new mascisEntities();
            string OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(value.DatePrepared), value.FacilityCode, "02", "01");
            db_hv.Order_MAULT_Header.Add(value);
            value.OrderNumber = OrderNumber;
            TempData["OrderNumber"] = OrderNumber;
            value.ProductCategoryId = 2;
            value.OrderStatusId = 1;
            value.EditedDate = DateTime.Now;
            value.EditedBy = new UserManagement().getCurrentuser();
            db_hv.SaveChanges();
            //if (value.FinalSubmission == true )
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
        //    obj.ProductCategoryId = 2;
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
            value.ProductCategoryId = 2;
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

    }
}