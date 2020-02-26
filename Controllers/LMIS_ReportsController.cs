using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using Microsoft.Reporting.WebForms;
//using ReportViewerForMvc;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data.Entity.Core.Objects;
using System.Configuration;
using mascis.Models;
using System.Web.Script.Serialization;
using System.Data.Entity.Infrastructure;
using Syncfusion.JavaScript;
using static mascis.Controllers.A_ProductController;
using Syncfusion.JavaScript.DataSources;
using Syncfusion.JavaScript.Models;
using System.Reflection;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
using Syncfusion.EJ.Export;

namespace mascis.Controllers
{ 
    public class LMIS_ReportsController : Controller
    {
        public IEnumerable currentData;
        private mascisEntities db = new mascisEntities();
        //ReportingRatesIndex
        public ActionResult ReportingRates(string Year,string Month,string productCategory, string DeliveryZoneCode, string cdc, string IP, string SCTO, string sdate, string edate)
        {
            ReportsModel obj = new ReportsModel();
            if (Year == "")
            {
                Year = null;
            }
            if (Month == "")
            {
                Month = null;
            }
            if (productCategory == "")
            {
                productCategory = null;
            }
            if (DeliveryZoneCode == "")
            {
                DeliveryZoneCode = null;
            }
            if (cdc == "")
            {
                cdc = null;
            }
            if (IP == "")
            {
                IP = null;
            }
            if (SCTO == "")
            {
                SCTO = null;
            }
            obj.Year = Year;
            obj.Month = Month;
            obj.productCategory = productCategory;
            obj.DeliveryZoneCode = DeliveryZoneCode;
            obj.cdc = cdc;
            obj.IP = IP;
            obj.SCTO = SCTO;
            DateTime? StartDate;//= DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate;//= DateTime.ParseExact(edate, "dd/MM/yyyy", null);

            if ((!sdate.Contains("null") && sdate != "") || (!edate.Contains("null") && edate != ""))
            {
                StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
                EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            }
            else
            {
                StartDate = null;// Convert.ToDateTime(DBNull.Value);
                EndDate = null;// Convert.ToDateTime(DBNull.Value);
            }
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;

            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
             
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\LMIS_Reports\ReportingRates.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ReportingRates", obj.GetReportingRates().ToList()));

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public ActionResult ReportingRatesCopy( string productCategory,  string sdate, string edate)
        {
            ReportsModel obj = new ReportsModel();
            
            if (productCategory == "")
            {
                productCategory = null;
            }
           
            obj.productCategory = productCategory;
             
            DateTime? StartDate;//= DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate;//= DateTime.ParseExact(edate, "dd/MM/yyyy", null);

            if ((!sdate.Contains("null") && sdate != "") || (!edate.Contains("null") && edate != ""))
            {
                StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
                EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            }
            else
            {
                StartDate = null;// Convert.ToDateTime(DBNull.Value);
                EndDate = null;// Convert.ToDateTime(DBNull.Value);
            }
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;

            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\LMIS_Reports\Reporting_Rates.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ReportingRates", obj.GetReportingRatesCopy().ToList()));

            ViewBag.ReportViewer = reportViewer;

            return View();
        }
        //ReportingRatesIndex
        public ActionResult ReportingRatesIndex()
        {
            return View();
        }
        public ActionResult StockStatusReport()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var productCat = db.A_product_category.Where(o=> o.category_code != 5 && o.category_code != 6 && o.category_code != 7 && o.category_code != 8 && o.category_code != 14).OrderBy(o=> o.category_desc).AsNoTracking().ToList();
            ViewBag.Category = productCat;
            var products = db.A_Product.OrderBy(o => o.product_description).AsNoTracking().ToList();
            ViewBag.product = products;
            return View();
        }
        public ActionResult GetStockStatus(DataManager dataManager, string Product,string Category,string StartDate, string EndDate)
        {
            DateTime? sdate = null;
            DateTime? edate = null;
            if (Product.Contains("null"))
            {
                Product = null;
            }
            if (Category.Contains("null"))
            {
                Category = null;
            }
            if (!string.IsNullOrEmpty(StartDate))
            {
                sdate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                edate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);
            }
            TempData["Product"] = Product;
            TempData["Category"] = Category;
            TempData["StartDate"] = StartDate;
            TempData["EndDate"] = EndDate;
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable DataSource = db.spView_crystal_lmis_StockStatusGetAll(Category, sdate, edate, Product).ToList();
            int count = db.spView_crystal_lmis_StockStatusGetAll(Category, sdate, edate, Product).ToList().Count;
            
            result.result = DataSource;
            result.count = count;
            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AMCIndex()
        {
            return View();
        }
        public ActionResult AMC(string Year, string Month, string productCategory, string DeliveryZoneCode, string cdc, string IP, string SCTO, string sdate, string edate, string fc)
        {
            ReportsModel obj = new ReportsModel();
            if (Year == "")
            {
                Year = null;
            }
            if (Month == "")
            {
                Month = null;
            }
            if (productCategory == "")
            {
                productCategory = null;
            }
            if (DeliveryZoneCode == "")
            {
                DeliveryZoneCode = null;
            }
            if (cdc == "")
            {
                cdc = null;
            }
            if (IP == "")
            {
                IP = null;
            }
            if (SCTO == "")
            {
                SCTO = null;
            }
            if (fc == "")
            {
                fc = null;
            }
            obj.Year = Year;
            obj.Month = Month;
            obj.productCategory = productCategory;
            obj.DeliveryZoneCode = DeliveryZoneCode;
            obj.cdc = cdc;
            obj.IP = IP;
            obj.SCTO = SCTO;
            obj.fc = fc;

            DateTime? StartDate;//= DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate;//= DateTime.ParseExact(edate, "dd/MM/yyyy", null);

            if ((!sdate.Contains("null") && sdate != "") || (!edate.Contains("null") && edate != ""))
            {
                StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
                EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            }
            else
            {
                StartDate = null;// Convert.ToDateTime(DBNull.Value);
                EndDate = null;// Convert.ToDateTime(DBNull.Value);
            }
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;

            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\LMIS_Reports\AMC.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("amc", obj.GetAMC().ToList()));

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public ActionResult ConsumptionIndex()
        {
            return View();
        }
        public ActionResult Consumption(string Year, string Month, string productCategory, string DeliveryZoneCode, string cdc, string IP, string SCTO, string sdate, string edate, string fc)
        {
            ReportsModel obj = new ReportsModel();
            if (Year == "")
            {
                Year = null;
            }
            if (Month == "")
            {
                Month = null;
            }
            if (productCategory == "")
            {
                productCategory = null;
            }
            if (DeliveryZoneCode == "")
            {
                DeliveryZoneCode = null;
            }
            if (cdc == "")
            {
                cdc = null;
            }
            if (IP == "")
            {
                IP = null;
            }
            if (SCTO == "")
            {
                SCTO = null;
            }
            if (fc == "")
            {
                fc = null;
            }
            obj.Year = Year;
            obj.Month = Month;
            obj.productCategory = productCategory;
            obj.DeliveryZoneCode = DeliveryZoneCode;
            obj.cdc = cdc;
            obj.IP = IP;
            obj.SCTO = SCTO;
            obj.fc = fc;

            DateTime? StartDate;//= DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate;//= DateTime.ParseExact(edate, "dd/MM/yyyy", null);

            if ((!sdate.Contains("null") && sdate != "") || (!edate.Contains("null") && edate != ""))
            {
                StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
                EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            }
            else
            {
                StartDate = null;// Convert.ToDateTime(DBNull.Value);
                EndDate = null;// Convert.ToDateTime(DBNull.Value);
            }
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;

            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\LMIS_Reports\Consumption.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("amc", obj.GetConsumption().ToList()));

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public ActionResult ReportingRatesCopyIndex()
        {
            return View();
        }
        public JsonResult GetYearData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.View_lmis_arv_amc.Select(a => a.EndYear).Distinct().OrderByDescending(a=>a.Value).ToList();
            var jsonSerializer = new JavaScriptSerializer();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //month
        public JsonResult GetMonthData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.A_Month.ToList().OrderBy(x => x.month_id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //product category
        public JsonResult GetProductCategoryData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.A_product_category.Where(a => a.category_code == 1 || a.category_code == 2 || a.category_code == 3 || a.category_code == 9 || a.category_code == 10 || a.category_code == 11 || a.category_code == 12 || a.category_code == 13).ToList().OrderBy(x => x.category_code);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Facility 
        public JsonResult GetFacilityData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.A_Facilities.ToList().OrderBy(x => x.Facility);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GetSectorData
        public JsonResult GetSectorData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.A_DeliveryZone.ToList().OrderBy(x => x.ZoneCode);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //GetRegionData
        public JsonResult GetRegionData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.A_CDCRegion.ToList().OrderBy(x => x.CDCRegion);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //IP
        public JsonResult GetIPData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.A_ImplimentingPartners.ToList().OrderBy(x => x.ImplementingPartnerDescription);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //GetSCTOData
        public JsonResult GetSCTOData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.fo_SCTO_ContactPerson.OrderBy(a => a.SCTO).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FrostReport()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Scto = db.fo_SCTO.Select(o => o.SCTO).Distinct().AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Region = db.A_CDCRegion.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.IP = db.A_ImplimentingPartners.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Facility = db.A_Facilities.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.District = db.A_District.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Category = db.A_product_category.Where(o => o.category_code != 14 && o.category_code != 7 && o.category_code != 6 && o.category_code != 5 && o.category_code != 8).AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.LevelOfCare = db.A_Facility_Level_Of_Care.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Sector = db.A_DeliveryZone.AsNoTracking().ToList();

            return View();
        }
        public ActionResult LoadFrostReport(string sdate, string edate, string productCategory, string facility, string Region, string ip, string district, string levelOfCare, string SCTO, string sector)
        {
            DateTime passeddate = DateTime.Now;
           // DateTime? startDate = null, endDate = null;
            if (productCategory.Contains("null") || string.IsNullOrEmpty(productCategory))
            {
                productCategory = null;
            }
            if (facility.Contains("null") || string.IsNullOrEmpty(facility))
            {
                facility = null;
            }
            if (Region.Contains("null") || string.IsNullOrEmpty(Region))
            {
                Region = null;
            }
            if (ip.Contains("null") || string.IsNullOrEmpty(ip))
            {
                ip = null;
            }
            if (district.Contains("null") || string.IsNullOrEmpty(district))
            {
                district = null;
            }
            if (levelOfCare.Contains("null") || string.IsNullOrEmpty(levelOfCare))
            {
                levelOfCare = null;
            }
            if (SCTO.Contains("null") || string.IsNullOrEmpty(SCTO))
            {
                SCTO = null;
            }
            if (sector.Contains("null") || string.IsNullOrEmpty(sector))
            {
                sector = null;
            }
            //if (DateTime.TryParse(sdate, out passeddate))
            //{
            //    startDate = passeddate;
            //}
            //if (DateTime.TryParse(edate, out passeddate))
            //{
            //    endDate = passeddate;
            //}
            DateTime? StartDate;//= DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate;//= DateTime.ParseExact(edate, "dd/MM/yyyy", null);

            if ((!sdate.Contains("null") && sdate != "") || (!edate.Contains("null") && edate != ""))
            {
                StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
                EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            }
            else
            {
                StartDate = null;// Convert.ToDateTime(DBNull.Value);
                EndDate = null;// Convert.ToDateTime(DBNull.Value);
            }
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\LMIS_Reports\Frost_Report.rdlc";
            ((IObjectContextAdapter)this.db).ObjectContext.CommandTimeout = 3000;
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("frost", db.spView_crystal_lmis_FrostGetAll(productCategory, StartDate, EndDate, facility, sector, district, levelOfCare, Region, ip, SCTO).ToList()));

            ViewBag.ReportViewer = reportViewer;

            return View();
        }
        public void ExportToExcel(string GridModel)
        {
            string Product = null;
            string Category = null;
            DateTime? sdate = null;
            DateTime? edate = null;
            if (!string.IsNullOrEmpty((string)TempData["Product"]))
            {
                Product = (string)TempData["Product"];
            }
            if (!string.IsNullOrEmpty((string)TempData["Category"]))
            {
                Category = (string)TempData["Category"];
            }
            if (!string.IsNullOrEmpty((string)TempData["StartDate"]))
            {
                sdate = DateTime.ParseExact((string)TempData["StartDate"], "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty((string)TempData["EndDate"]))
            {
               edate = DateTime.ParseExact((string)TempData["EndDate"], "dd/MM/yyyy", null);
            }
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource =  db.spView_crystal_lmis_StockStatusGetAll(Category, sdate, edate, Product).ToList(); 
            ///GridProperties properties = (GridProperties)Syncfusion.JavaScript.Utils.DeserializeToModel(typeof(GridProperties), GridModel);
            var dataSource = new DataOperations().Execute(DataSource, obj);
            //GridProperties obj = (GridProperties)Syncfusion.JavaScript.Utils.DeserializeToModel(typeof(GridProperties), GridModel);
            exp.Export(obj, DataSource, "Stock status Report " + DateTime.Now.ToLongDateString() + ".xlsx", ExcelVersion.Excel2010, false, false, /*"flat-saffron"*/"none");

            //ExcelExport exp = new ExcelExport();
            //ExcelEngine excel = new ExcelEngine();
            //IApplication application = excel.Excel;
            //GridProperties obj = ConvertGridObject(GridModel);
            //IWorkbook workbook = application.Workbooks.Create(2);

            ////List<Orders> reports = BindDataSource();


            //workbook.Worksheets[0].ImportData(currentData, 1, 1, false);

            //workbook.SaveAs(Module + ".xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2010);
        }
        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483644;
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (ds.Key == "MyData")
                {
                    string str = Convert.ToString(ds.Value);
                    currentData = JsonConvert.DeserializeObject<IEnumerable<spView_crystal_lmis_StockStatusGetAll_Result>>(str);
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
    }
}