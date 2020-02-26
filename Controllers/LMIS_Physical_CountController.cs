using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mascis.Models;
using System.Data;
using System.Data.Entity;

namespace mascis.Controllers
{
    public class LMIS_Physical_CountController : Controller
    {
        private mascisEntities context = new mascisEntities();

        // GET: LMIS_Physical_Count
        public ActionResult PC_ART()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.View_ProductClassification.Where(a => a.product_category == 2).OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;


            //context.Configuration.ProxyCreationEnabled = false;
            //var art = context.A_Product.Where(a => a.product_category == 2).OrderBy(a => a.product_description);
            //ViewBag.ProductDataSource = art;

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            //context.Configuration.ProxyCreationEnabled = false;
            //var sector = context.A_Facilities.Select(a => new {a.DeliveryZoneCode }).ToList();
            //ViewBag.sector = sector;
            return View();
        }
        public ActionResult PC_Lab()
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var product = context.View_ProductClassification.Where(a => a.product_category == 1).OrderBy(a => a.product_description);
            var product = context.View_WebTemplate_Laboratory.Where(a => a.product_category == 1).OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            //context.Configuration.ProxyCreationEnabled = false;
            //var sector = context.A_Facilities.Select(a => new {a.DeliveryZoneCode }).ToList();
            //ViewBag.sector = sector;
            return View();
        }
        public ActionResult PC_HIV()
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var product = context.View_ProductClassification.Where(a => a.product_category == 3).OrderBy(a => a.product_description);
            var product = context.A_Product.Where(a => a.product_category == 3).OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            //context.Configuration.ProxyCreationEnabled = false;
            //var sector = context.A_Facilities.Select(a => new {a.DeliveryZoneCode }).ToList();
            //ViewBag.sector = sector;
            return View();
        }
        public ActionResult PC_OISTI()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.View_ProductClassification.Where(a => a.product_category == 9).OrderBy(a => a.product_description);
            //var product = context.A_Product.Where(a => a.product_category == 9).OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            //context.Configuration.ProxyCreationEnabled = false;
            //var sector = context.A_Facilities.Select(a => new {a.DeliveryZoneCode }).ToList();
            //ViewBag.sector = sector;
            return View();
        }
        public ActionResult PC_SMC()
        {
            context.Configuration.ProxyCreationEnabled = false;
            var product = context.View_ProductClassification.Where(a => a.product_category == 10).OrderBy(a => a.product_description);
            //var product = context.A_Product.Where(a => a.product_category == 10).OrderBy(a => a.product_description);
            ViewBag.ProductDataSource = product;

            context.Configuration.ProxyCreationEnabled = false;
            var facility = context.A_Facilities.AsNoTracking();
            ViewBag.FacilityDataSource = facility;

            //context.Configuration.ProxyCreationEnabled = false;
            //var sector = context.A_Facilities.Select(a => new {a.DeliveryZoneCode }).ToList();
            //ViewBag.sector = sector;
            return View();
        }
        public class DataResult
        {
            public IEnumerable result { get; set; }
            public int count { get; set; }
        }
        //ART
        public ActionResult DataSourcePC_ART(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();

            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_DrugDetails_PhysicalCount.Where(a=>a.FacilityCode==facility_id).OrderBy(a=>a.DateOfPhysicalCount).ToList();
            int count = context.Order_DrugDetails_PhysicalCount.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList().Count();

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
        public ActionResult BatchData(Syncfusion.JavaScript.DataManager dm, int? Code)
        {
            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable Data = context.Order_DrugDetails_PhysicalCount.Where(a=>a.product_code==Code).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            context.Configuration.ProxyCreationEnabled = false;
            result.count = context.Order_DrugDetails_PhysicalCount.Where(a => a.product_code == Code).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchDataEdit(Syncfusion.JavaScript.DataManager dm, int? FacilityCode, string DateOfPhysicalCount)
        {
            DateTime vDate = DateTime.ParseExact(DateOfPhysicalCount, "dd/MM/yyyy", null);

            IEnumerable Data = context.View_WebTemplate_ARV_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.View_WebTemplate_ARV_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchUpdate(string key, List<Order_DrugDetails_PhysicalCount> changed, List<Order_DrugDetails_PhysicalCount> added, List<Order_DrugDetails_PhysicalCount> deleted)
        {
            mascisEntities dbCase = new mascisEntities();
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    dbCase.Order_DrugDetails_PhysicalCount.Add(temp);
                    temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                    temp.DateAdded = DateTime.Now;
                    temp.AddedBy = new UserManagement().getCurrentuser();
                    temp.record_status = true;
                }
            }
            dbCase.SaveChanges();

            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    Order_DrugDetails_PhysicalCount old = dbCase.Order_DrugDetails_PhysicalCount.
                        FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.product_code == temp.product_code && o.BatchNo == temp.BatchNo);
                    if (old != null)
                    {
                        dbCase.Entry(old).CurrentValues.SetValues(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                    else
                    {
                        dbCase.Order_DrugDetails_PhysicalCount.Add(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                }
            }
            dbCase.SaveChanges();
            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    dbCase.Order_DrugDetails_PhysicalCount.Remove(dbCase.Order_DrugDetails_PhysicalCount.FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.product_code == temp.product_code && o.BatchNo == temp.BatchNo));
                }
            }

            dbCase.SaveChanges();
            //return RedirectToAction("BatchData");
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //LABAROTORY
        public ActionResult DataSourcePC_Labaratory(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();

            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_Lab_PhysicalCount.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList();
            int count = context.Order_Lab_PhysicalCount.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList().Count();

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
        public ActionResult BatchDataLAB(Syncfusion.JavaScript.DataManager dm,  int? Code)
        {
            IEnumerable Data = context.Order_Lab_PhysicalCount.Where(a => a.product_code == Code ).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.Order_Lab_PhysicalCount.Where(a => a.product_code == Code ).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchDataEditLAB(Syncfusion.JavaScript.DataManager dm, int? FacilityCode, string DateOfPhysicalCount)
        {
            DateTime vDate = DateTime.ParseExact(DateOfPhysicalCount, "dd/MM/yyyy", null);

            IEnumerable Data = context.View_WebTemplate_Laboratory_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.View_WebTemplate_Laboratory_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchUpdateLAB(string key, List<Order_Lab_PhysicalCount> changed, List<Order_Lab_PhysicalCount> added, List<Order_Lab_PhysicalCount> deleted)
        {
            mascisEntities dbCase = new mascisEntities();
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    dbCase.Order_Lab_PhysicalCount.Add(temp);
                    temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                    temp.DateAdded = DateTime.Now;
                    temp.AddedBy = new UserManagement().getCurrentuser();
                    temp.record_status = true;
                }
            }
            dbCase.SaveChanges();

            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    Order_Lab_PhysicalCount old = dbCase.Order_Lab_PhysicalCount.
                        FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.product_code == temp.product_code && o.BatchNo == temp.BatchNo);
                    if (old != null)
                    {
                        dbCase.Entry(old).CurrentValues.SetValues(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                    else
                    {
                        dbCase.Order_Lab_PhysicalCount.Add(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                }
            }
            dbCase.SaveChanges();
            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    dbCase.Order_Lab_PhysicalCount.Remove(dbCase.Order_Lab_PhysicalCount.FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.product_code == temp.product_code && o.BatchNo == temp.BatchNo));
                }
            }

            dbCase.SaveChanges();
            return RedirectToAction("BatchDataLAB");
        }
        //HIV TEST KITS
        public ActionResult DataSourcePC_HIV(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();

            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.order_hiv_rapid_test_kit_PhysicalCount.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList();
            int count = context.order_hiv_rapid_test_kit_PhysicalCount.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList().Count();

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
        public ActionResult BatchDataHIV(Syncfusion.JavaScript.DataManager dm, int? Code)
        {
            IEnumerable Data = context.order_hiv_rapid_test_kit_PhysicalCount.Where(a => a.product_code == Code).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.order_hiv_rapid_test_kit_PhysicalCount.Where(a => a.product_code == Code).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchDataEditHIV(Syncfusion.JavaScript.DataManager dm, int? FacilityCode, string DateOfPhysicalCount)
        {
            DateTime vDate = DateTime.ParseExact(DateOfPhysicalCount, "dd/MM/yyyy", null);

            IEnumerable Data = context.View_WebTemplate_HIV_Test_Kits_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate ).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.View_WebTemplate_HIV_Test_Kits_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate ).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchUpdateHIV(string key, List<order_hiv_rapid_test_kit_PhysicalCount> changed, List<order_hiv_rapid_test_kit_PhysicalCount> added, List<order_hiv_rapid_test_kit_PhysicalCount> deleted)
        {
            mascisEntities dbCase = new mascisEntities();
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    order_hiv_rapid_test_kit_PhysicalCount old = dbCase.order_hiv_rapid_test_kit_PhysicalCount.
                          FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.product_code == temp.product_code && o.BatchNo == temp.BatchNo);
                    if (old != null)
                    {
                        dbCase.order_hiv_rapid_test_kit_PhysicalCount.Add(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                    else
                    {
                        dbCase.order_hiv_rapid_test_kit_PhysicalCount.Add(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                }
            }
            //dbCase.SaveChanges();

            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    order_hiv_rapid_test_kit_PhysicalCount old = dbCase.order_hiv_rapid_test_kit_PhysicalCount.
                        FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.product_code == temp.product_code && o.BatchNo == temp.BatchNo);
                    if (old != null)
                    {
                        dbCase.Entry(old).CurrentValues.SetValues(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                    else
                    {
                        dbCase.order_hiv_rapid_test_kit_PhysicalCount.Add(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                }
            }
            //dbCase.SaveChanges();
            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    dbCase.order_hiv_rapid_test_kit_PhysicalCount.Remove(dbCase.order_hiv_rapid_test_kit_PhysicalCount.FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.product_code == temp.product_code && o.BatchNo == temp.BatchNo));
                }
            }

            dbCase.SaveChanges();
            return RedirectToAction("BatchDataHIV");
            //var data = 0;
            //return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[HttpGet]
        //public ActionResult BatchUpdateHIV (string key, List<order_hiv_rapid_test_kit_PhysicalCount> changed, List<order_hiv_rapid_test_kit_PhysicalCount> added, List<order_hiv_rapid_test_kit_PhysicalCount> deleted)
        //{
        //    PC_Model obj = new PC_Model();
        //    if (added != null && added.Count() > 0)
        //    {
        //        foreach (var temp in added)
        //        {
        //            obj.FacilityCode = temp.FacilityCode;
        //            string iDate = temp.DateOfPhysicalCount.ToString();
        //            DateTime oDate = DateTime.Parse(iDate);
        //            obj.DateOfPhysicalCount = oDate;
        //           // obj.DateOfPhysicalCount = temp.DateOfPhysicalCount;
        //            obj.ProductCode = System.Convert.ToInt32(temp.product_code);
        //            obj.BatchNo = temp.BatchNo;
        //            obj.Quantity_Dispensary = temp.Quantity_Dispensary;
        //            obj.Quantity_Store = temp.Quantity_Store;
        //            //obj.ExpiryDate = temp.ExpiryDate;
        //            //string Expiry_Date = temp.ExpiryDate.ToString();
        //            //DateTime ExpiryTime = DateTime.Parse(Expiry_Date);
        //            //obj.ExpiryDate = ExpiryTime;

        //            obj.Total = (temp.Quantity_Dispensary + temp.Quantity_Store);
        //            obj.Comment = temp.Comment;
        //            obj.AddedBy = new UserManagement().getCurrentuser();
        //           // obj.DateAdded=  DateTime.Now;
        //            obj.record_status = true;
        //            obj.Update();
        //        }
        //    }
        //    if (changed != null && changed.Count() > 0)
        //    {
        //        foreach (var temp in changed)
        //        {
        //            obj.FacilityCode = temp.FacilityCode;
        //            string iDate = temp.DateOfPhysicalCount.ToString();
        //            DateTime oDate = DateTime.Parse(iDate);
        //            obj.DateOfPhysicalCount = oDate;
        //            //obj.DateOfPhysicalCount = temp.DateOfPhysicalCount;
        //            obj.ProductCode = System.Convert.ToInt32(temp.product_code);
        //            obj.BatchNo = temp.BatchNo;
        //            obj.Quantity_Dispensary = temp.Quantity_Dispensary;
        //            obj.Quantity_Store = temp.Quantity_Store;
        //            //obj.ExpiryDate = temp.ExpiryDate;
        //            string Expiry_Date = temp.ExpiryDate.ToString();
        //            DateTime ExpiryTime = DateTime.Parse(Expiry_Date);
        //            obj.ExpiryDate = ExpiryTime;
        //            obj.Total = (temp.Quantity_Dispensary + temp.Quantity_Store);
        //            obj.Comment = temp.Comment;
        //            obj.AddedBy = new UserManagement().getCurrentuser();
        //            obj.DateAdded = DateTime.Now;
        //            obj.record_status = true;
        //            obj.Update();
        //        }
        //    }
        //    var data = 0;
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //OISTI
        public ActionResult DataSourcePC_OI(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();

            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_OI_STI_Detail_Physical_Count.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList();
            int count = context.Order_OI_STI_Detail_Physical_Count.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList().Count();

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
        public ActionResult BatchDataOI(Syncfusion.JavaScript.DataManager dm, int? Code)
        {
            IEnumerable Data = context.Order_OI_STI_Detail_Physical_Count.Where(a => a.ProductCode == Code).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.Order_OI_STI_Detail_Physical_Count.Where(a => a.ProductCode == Code).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchDataEditOI(Syncfusion.JavaScript.DataManager dm, int? FacilityCode, string DateOfPhysicalCount)
        {
            DateTime vDate = DateTime.ParseExact(DateOfPhysicalCount, "dd/MM/yyyy", null);

            IEnumerable Data = context.View_WebTemplate_OI_STI_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate ).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.View_WebTemplate_OI_STI_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate ).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchUpdateOI(string key, List<Order_OI_STI_Detail_Physical_Count> changed, List<Order_OI_STI_Detail_Physical_Count> added, List<Order_OI_STI_Detail_Physical_Count> deleted)
        {
            mascisEntities dbCase = new mascisEntities();
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    dbCase.Order_OI_STI_Detail_Physical_Count.Add(temp);
                    temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                    temp.DateAdded = DateTime.Now;
                    temp.AddedBy = new UserManagement().getCurrentuser();
                    temp.record_status = true;
                }
            }
            dbCase.SaveChanges();

            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    Order_OI_STI_Detail_Physical_Count old = dbCase.Order_OI_STI_Detail_Physical_Count.
                        FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.ProductCode == temp.ProductCode && o.BatchNo == temp.BatchNo);
                    if (old != null)
                    {
                        dbCase.Entry(old).CurrentValues.SetValues(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                    else
                    {
                        dbCase.Order_OI_STI_Detail_Physical_Count.Add(temp);
                        temp.Total = temp.Quantity_Dispensary + temp.Quantity_Store;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                }
            }
            dbCase.SaveChanges();
            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    dbCase.Order_OI_STI_Detail_Physical_Count.Remove(dbCase.Order_OI_STI_Detail_Physical_Count.FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.ProductCode == temp.ProductCode && o.BatchNo == temp.BatchNo));
                }
            }

            dbCase.SaveChanges();
            //return RedirectToAction("BatchDataOI");
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //SMC
        public ActionResult DataSourcePC_SMC(DataManager dm)
        {
            int? facility_id = -1;
            if (new UserManagement().getCurrentuserFacility() != null)
                facility_id = new UserManagement().getCurrentuserFacility();

            context.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = context.Order_SMC_SLM_Physical_Count.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList();
            int count = context.Order_SMC_SLM_Physical_Count.Where(a => a.FacilityCode == facility_id).OrderBy(a => a.DateOfPhysicalCount).ToList().Count();

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
        public ActionResult BatchDataSMC(Syncfusion.JavaScript.DataManager dm, int? Code)
        {
            IEnumerable Data = context.Order_SMC_SLM_Physical_Count.Where(a => a.ProductCode == Code).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.Order_SMC_SLM_Physical_Count.Where(a => a.ProductCode == Code).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchDataEditSMC(Syncfusion.JavaScript.DataManager dm, int? FacilityCode, string DateOfPhysicalCount)
        {
            DateTime vDate = DateTime.ParseExact(DateOfPhysicalCount, "dd/MM/yyyy", null);

            IEnumerable Data = context.View_WebTemplate_SMC_SLM_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate ).ToList();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            result.count = context.View_WebTemplate_SMC_SLM_PhysicalCount_Edit.Where(h => h.FacilityCode == FacilityCode && h.DateOfPhysicalCount == vDate ).ToList().Count();
            if (dm.Skip > 0)
                Data = operation.PerformSkip(Data, dm.Skip);
            if (dm.Take > 0)
                Data = operation.PerformTake(Data, dm.Take);
            result.result = Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchUpdateSMC(string key, List<Order_SMC_SLM_Physical_Count> changed, List<Order_SMC_SLM_Physical_Count> added, List<Order_SMC_SLM_Physical_Count> deleted)
        {
            mascisEntities dbCase = new mascisEntities();
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    dbCase.Order_SMC_SLM_Physical_Count.Add(temp);
                    temp.Total = temp.Quantity_Dispensary;
                    temp.DateAdded = DateTime.Now;
                    temp.AddedBy = new UserManagement().getCurrentuser();
                    temp.record_status = true;
                }
            }
            dbCase.SaveChanges();

            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    string iDate = temp.DateOfPhysicalCount.ToString();
                    DateTime oDate = DateTime.Parse(iDate);
                    temp.DateOfPhysicalCount = oDate;

                    Order_SMC_SLM_Physical_Count old = dbCase.Order_SMC_SLM_Physical_Count.
                        FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.ProductCode == temp.ProductCode && o.BatchNo == temp.BatchNo);
                    if (old != null)
                    {
                        dbCase.Entry(old).CurrentValues.SetValues(temp);
                        temp.Total = temp.Quantity_Dispensary;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                    else
                    {
                        dbCase.Order_SMC_SLM_Physical_Count.Add(temp);
                        temp.Total = temp.Quantity_Dispensary;
                        temp.DateAdded = DateTime.Now;
                        temp.AddedBy = new UserManagement().getCurrentuser();
                        temp.record_status = true;
                    }
                }
            }
            dbCase.SaveChanges();
            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    dbCase.Order_SMC_SLM_Physical_Count.Remove(dbCase.Order_SMC_SLM_Physical_Count.FirstOrDefault(o => o.FacilityCode == temp.FacilityCode && o.DateOfPhysicalCount == temp.DateOfPhysicalCount && o.ProductCode == temp.ProductCode && o.BatchNo == temp.BatchNo));
                }
            }

            dbCase.SaveChanges();
            //return RedirectToAction("BatchDataSMC");
            var data = 0;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
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
        public ActionResult GetFacility(int? FacilityCode)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_Facilities.AsNoTracking().Where(a => a.FacilityCode == FacilityCode).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult FacilitySecto(int? sector)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_DeliveryZone.Where(a => a.ZoneCode == sector);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
       // [HttpGet]
        public JsonResult GetProduct()
        {
            //context.Configuration.ProxyCreationEnabled = false;
            var data = context.View_ProductClassification.Where(a=>a.product_category==2).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //DistrictData
        [HttpGet]
        public ActionResult DistrictData(int? District)
        {
            context.Configuration.ProxyCreationEnabled = false;
            var data = context.A_District.Where(a => a.District_Code == District);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
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

        //public ActionResult BatchDataKount(DataManager dataManager)
        //{
        //    IEnumerable DataSource = context.WebTemplate_ARV_PhysicalCount.ToList();
        //    DataResult result = new DataResult();
        //    result.result = DataSource;
        //    result.count = context.WebTemplate_ARV_PhysicalCount.Count();

        //    return Json(result.count, JsonRequestBehavior.AllowGet);
        //}
        public int SavePC(int FacilityCode, DateTime DateOfPhysicalCount, int product_code, string BatchNo,
       string Quantity_Dispensary, string Quantity_Store, DateTime? ExpiryDate, string Total, string Comment)
        { 
            order_hiv_rapid_test_kit_PhysicalCount obj = new order_hiv_rapid_test_kit_PhysicalCount();
            int recstat = 0;
             
            obj.FacilityCode = FacilityCode;
            obj.DateOfPhysicalCount = DateOfPhysicalCount;
            obj.product_code = System.Convert.ToInt32(product_code);
            obj.BatchNo =  BatchNo;
            if (Quantity_Dispensary != null && !string.IsNullOrEmpty(Quantity_Dispensary))
            {
                obj.Quantity_Dispensary = System.Convert.ToDouble(Quantity_Dispensary);
            }
            if (Quantity_Store != null && !string.IsNullOrEmpty(Quantity_Store))
            {
                obj.Quantity_Store = System.Convert.ToDouble(Quantity_Store);
            }
            obj.ExpiryDate = ExpiryDate;
            if (Total != null && !string.IsNullOrEmpty(Total))
            {
                obj.Total = System.Convert.ToDouble(Total);
            }
            obj.Comment = Comment;
         
            try
            {
                //Check for the existance of the record
                var ca = context.order_hiv_rapid_test_kit_PhysicalCount.FirstOrDefault(c => c.FacilityCode == FacilityCode && c.DateOfPhysicalCount == DateOfPhysicalCount && c.product_code==product_code && c.BatchNo==BatchNo);

                if (ca == null)
                {
                    context.order_hiv_rapid_test_kit_PhysicalCount.Add(obj);
                    obj.DateAdded = DateTime.Now;
                    obj.AddedBy = new UserManagement().getCurrentuser();
                    recstat = context.SaveChanges();
                    // recstat = obj.household_number;
                }
                else
                {
                    order_hiv_rapid_test_kit_PhysicalCount table = context.order_hiv_rapid_test_kit_PhysicalCount.FirstOrDefault(c => c.FacilityCode == FacilityCode && c.DateOfPhysicalCount == DateOfPhysicalCount && c.product_code == product_code && c.BatchNo == BatchNo);
                    // obj.household_number = ca.household_number ;
                    context.Entry(table).CurrentValues.SetValues(obj);
                    obj.DateAdded = DateTime.Now;
                    obj.AddedBy = new UserManagement().getCurrentuser();
                    context.Entry(table).State = EntityState.Modified;
                    recstat = context.SaveChanges();
                    //recstat = ca.household_number;

                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return recstat;
        }


    }
}
