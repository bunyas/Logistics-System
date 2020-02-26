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
using Newtonsoft.Json;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Data.Entity.Validation;

namespace mascis.Controllers
{
    public class LMIS_ExpectedReportsController : Controller
    {
        // GET: LMIS_ExpectedReports
        mascisEntities context = new mascisEntities();
        public ActionResult ExpectedReports()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_product_category.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var fc = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.fc = fc;

            return View();
        }
        public ActionResult ExpectedReports_Edit()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var OrderStatus = context.A_product_category.AsNoTracking();
            ViewBag.OrderStatusDataSource = OrderStatus;

            context.Configuration.ProxyCreationEnabled = false;
            var fc = context.A_Facilities.AsNoTracking().ToList();
            ViewBag.fc = fc;

            return View();
        }
        public JsonResult GetProductCategoryData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.A_product_category.Where(a => a.category_code == 1 || a.category_code == 2 || a.category_code == 3 || a.category_code == 9 || a.category_code == 10 || a.category_code == 11 || a.category_code == 12 || a.category_code == 13).ToList().OrderBy(x => x.category_code);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClientTypeData()
        {
            context.Configuration.ProxyCreationEnabled = false;
             IEnumerable data = context.A_ClientType.ToList().OrderBy(x => x.client_type_desc);
             return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeliveryZoneCodeData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.A_DeliveryZone.ToList().OrderBy(x => x.ZoneCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCDCRegionData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.A_CDCRegion.ToList().OrderBy(x => x.CDCRegion);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOwnerShipData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.A_Ownership.ToList().OrderBy(x => x.OwnershipDescription);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIsActiveData()
        {
            List<YesNoDropdownlist> YesNoDataSource = new List<YesNoDropdownlist>();
            YesNoDataSource.Add(new YesNoDropdownlist
            {
                //Id = 0,
                Description = "No"
            });
            YesNoDataSource.Add(new YesNoDropdownlist
            {
                //Id = 1,
                Description = "Yes"
            });
            return Json(YesNoDataSource, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataSourceExpectedReport(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.View_LMIS_ExpectedReport.OrderBy(o => o.ZoneDescription).ToList();
            int count = context.View_LMIS_ExpectedReport.Count();

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
        public ActionResult DataSourceExpectedReportEdit(DataManager dm)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.View_LMIS_ExpectedReport.Where(a=>a.FacilityCode==99999).OrderBy(o => o.ZoneDescription).ToList();
            int count = context.View_LMIS_ExpectedReport.Where(a => a.FacilityCode == 99999).Count();

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

        public ActionResult SaveExpectedReport(LMIS_ExpectedReport value, string fc, string productcategory,string startdate, string enddate)
        {
            DateTime sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            DateTime eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            int facility = int.Parse(fc);
            int pc = int.Parse(productcategory);

            context.Configuration.ProxyCreationEnabled = false;
            mascisEntities db_master = new mascisEntities();
            LMIS_ExpectedReport table = db_master.LMIS_ExpectedReport.FirstOrDefault(o => o.facility_code == facility && o.product_category== pc && o.start_date== sDate && o.end_date== eDate);
            ExpectedReports obj = new ExpectedReports();
            if (table == null)
            {
                //db.Entry(table).CurrentValues.SetValues(value);
                obj.facility_code = facility;
                obj.product_category = pc;
                obj.start_date = sDate;
                obj.end_date = eDate;
                //db_master.SaveChanges();
            }
           else
           {
                //db.Entry(table).CurrentValues.SetValues(value);
                obj.facility_code = facility;
                obj.product_category = pc;
                obj.start_date = sDate;
                obj.end_date = eDate;
                db_master.Entry(table).State = EntityState.Modified;
                context.Entry(table).CurrentValues.SetValues(table);
           }
            obj.Update();
            return Json(value, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("DataSourceRFSO");
        }


        public ActionResult BatchUpdate(string key, List<LMIS_ExpectedReport> changed, List<LMIS_ExpectedReport> added, List<LMIS_ExpectedReport> deleted)
        {
            mascisEntities dbCase = new mascisEntities();
            ExpectedReports obj = new ExpectedReports();
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    //dbCase.A_Facilities.Add(temp);
                    obj.facility_code = temp.facility_code;
                    obj.start_date = temp.start_date;
                    obj.end_date = temp.end_date;
                    obj.product_category = temp.product_category;
                    context.SaveChanges();
                }
            }
            obj.Update();

            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                { 
                    obj.facility_code = temp.facility_code;
                    obj.start_date = temp.start_date;
                    obj.end_date = temp.end_date;
                    obj.product_category = temp.product_category;
                    var check = context.LMIS_ExpectedReport.FirstOrDefault(e => e.facility_code == temp.facility_code && e.start_date==temp.start_date &&e.end_date==temp.end_date && e.product_category==temp.product_category);
                    context.Entry(check).CurrentValues.SetValues(check);
                    context.Entry(check).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            //dbCase.SaveChanges();
            obj.Update();
            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    dbCase.LMIS_ExpectedReport.Remove(dbCase.LMIS_ExpectedReport.FirstOrDefault(e => e.facility_code == temp.facility_code && e.start_date == temp.start_date && e.end_date == temp.end_date && e.product_category == temp.product_category));
                }
            }

            //dbCase.SaveChanges();
            obj.Update();
            return RedirectToAction("DataSourceRFSO");
        }

        public ActionResult GetEditDataSource(string productcategory, string startdate, string enddate)
        {
            context.Configuration.ProxyCreationEnabled = false;
            // IEnumerable data2; //= context.Order_Lab_Header.where.ToList();
            int count = 0;
            List<LMIS_ExpectedReport> data = new List<LMIS_ExpectedReport>();
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate) || string.IsNullOrEmpty(productcategory)) { }
            else
            {
                DateTime? sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime? eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                int pc = int.Parse(productcategory);
                
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    data = context.LMIS_ExpectedReport.Where(b => b.start_date >= sDate && b.end_date <= eDate  && b.product_category == pc).OrderByDescending(a => a.start_date).ToList();
                    count = context.LMIS_ExpectedReport.Where(b => b.start_date >= sDate && b.end_date <= eDate && b.product_category == pc).OrderByDescending(a => a.start_date).ToList().Count();
                }

            }
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { result = data2, count = count }, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult GetCurrentDataSearch(DataManager dm, int? productcategory, string startdate, string enddate, int? ClientType, int? OwnerShip, int? cdc, string IsActive)
        //{
        //    context.Configuration.ProxyCreationEnabled = false;
        //    DateTime? sDate;
        //    DateTime? eDate;
        //    productcategory = null; 
        //    if ((!startdate.Contains("null") && startdate != "") || (!enddate.Contains("null") && enddate != ""))
        //    {
        //        sDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
        //        eDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
        //    }
        //    else
        //    {
        //        sDate = null;// Convert.ToDateTime(DBNull.Value);
        //        eDate = null;// Convert.ToDateTime(DBNull.Value);
        //    }
 
        //    IEnumerable data = context.View_LMIS_ExpectedReport.Where(b =>  (b.client_type_code== ClientType || ClientType==null) && (b.CDCRegionId == cdc || cdc == null) && (b.OwnershipId == OwnerShip || OwnerShip == null) && (b.IsActive == IsActive || IsActive == null || IsActive == "")).OrderByDescending(a => a.Facility).ToList();
        //    int count = context.View_LMIS_ExpectedReport.Where(b => ( b.client_type_code == ClientType || ClientType == null) && (b.CDCRegionId == cdc || cdc == null) && (b.OwnershipId == OwnerShip || OwnerShip == null) && (b.IsActive == IsActive || IsActive == null || IsActive == "")).OrderByDescending(a => a.Facility).ToList().Count;
 
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
        //    if (dm.Skip > 0)
        //        data = operation.PerformSkip(data, dm.Skip);
        //    if (dm.Take > 0)
        //        data = operation.PerformTake(data, dm.Take);

        //    //return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetCurrentDataSearch(DataManager dm, string productcategory, string startdate, string enddate, string ClientType, string OwnerShip, string cdc, string IsActive, string DeliveryZoneCode)
        {
            ReportsModel obj = new ReportsModel();

            if (productcategory == "")
            {
                productcategory = null;
            }
            if (ClientType == "")
            {
                ClientType = null;
            }
            if (OwnerShip == "")
            {
                OwnerShip = null;
            }
            if (cdc == "")
            {
                cdc = null;
            }
            if (IsActive == "")
            {
                IsActive = null;
            }
            if (DeliveryZoneCode == "")
            {
                DeliveryZoneCode = null;
            }
            //obj.productCategory = productcategory;
            obj.productCategory = null;
            obj.ClientType = ClientType;
            obj.Owner = OwnerShip;
            obj.cdc = cdc;
            obj.IsActive = IsActive;
            obj.DeliveryZoneCode = DeliveryZoneCode;

            DateTime? StartDate;//= DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate;//= DateTime.ParseExact(edate, "dd/MM/yyyy", null);

            if ((!startdate.Contains("null") && startdate != "") || (!enddate.Contains("null") && enddate != ""))
            {
                StartDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                EndDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            }
            else
            {
                StartDate = null;// Convert.ToDateTime(DBNull.Value);
                EndDate = null;// Convert.ToDateTime(DBNull.Value);
            }
            //obj.StartDate = StartDate;
            //obj.EndDate = EndDate;
            obj.StartDate = null;
            obj.EndDate = null;
            IEnumerable data = obj.GetExpectedReports().OrderByDescending(a => a.Facility).ToList();
            int count = obj.GetExpectedReports().OrderByDescending(a => a.Facility).ToList().Count;

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

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}