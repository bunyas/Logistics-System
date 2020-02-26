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

namespace mascis.Controllers
{
    public class ReportsModuleController : Controller
    {
        private mascisEntities context = new mascisEntities();

        // GET: Reports
        public ActionResult PCARTIndex()
        {
            return View();
        }
        public ActionResult PC_OIsIndex()
        {
            return View();
        }
        //PC_HIVTEST_Index 
        public ActionResult PC_HIVTEST_Index()
        {
            return View();
        }
        //HIV_Index
        public ActionResult HIV_Index()
        {
            return View();
        }
        //OIs_Index
        public ActionResult OIs_Index()
        {
            return View();
        }
        //ViralLoadReagents_Index
        public ActionResult ViralLoadReagents_Index()
        {
            return View();
        }
        public ActionResult SMC_Index()
        {
            return View();
        }
        public ActionResult ART_Index()
        {
            return View();
        }

        public ActionResult Lab_Index()
        {
            return View();
        }

        public ActionResult PC_SMCIndex()
        {
            return View();
        }
        public ActionResult OrderTrackingIndex()
        {
            return View();
        }
        public JsonResult GetProductCategoryData()
        {
            context.Configuration.ProxyCreationEnabled = false;
            //var RFSOfacilities = new UserManagement().getUserFacilityList();
            IEnumerable data = context.A_product_category.Where(a => a.category_code == 1 || a.category_code == 2 || a.category_code == 3 || a.category_code == 9 || a.category_code == 10 || a.category_code == 11).ToList().OrderBy(x => x.category_code);

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

        //[HttpGet]
        public ActionResult PCART(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_PC_WebTemplate_ARV.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("art_pc", obj.GetPC_ART().ToList()));
            
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public ActionResult PC_OIs(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_PC_WebTemplate_OI_STI.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("art_pc", obj.GetPC_OIs().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult PC_SMC(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_PC_WebTemplate_SMC_SLM.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("art_pc", obj.GetPC_SMC().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public ActionResult PC_HIVTEST(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_PC_WebTemplate_Laboratory.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("test_kit_pc", obj.GetPC_HIV().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("lab_pc", obj.GetPC_Lab().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult OrderTracking( string Order_Number)
        {
            //int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            //if (int.TryParse(ProductCategory, out x))
            //    obj.ProductCategoryId = System.Convert.ToInt32(ProductCategory);
            obj.OrderNumber = Order_Number;

            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\OrderStatus.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("OrderStatus", obj.GetCurrentTracking().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("OrderDetails", obj.GetTrackingDetails().ToList()));
            //reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_regional_activity", obj.GetCoARegionalMeetings().ToList()));
            //reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_activity_distribution", obj.GetCoADistributionOfActivities().ToList()));
            //reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("CoA_AcitivitiesByFundingType", obj.GetCoAActivitiesByFundingType().ToList()));
            //reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("CoA_AcitivitiesByMeetingType", obj.GetCoAActivitiesByMeetingType().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public ActionResult HIV(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_WebTemplate_HIV_Test_Kits.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Header", obj.HIV_Header().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Tests", obj.HIV().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Consumption", obj.HIV_Summary().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        //ART 

        public ActionResult ART(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_WebTemplate_ARV.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_consumption", obj.ART().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_patients", obj.ART_Patients().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_patients_paediatric", obj.ART_Paediatric().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_patients_fluconazole", obj.ARV_Fluconazole().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_header", obj.ARV_ReportHeader().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_extra_regimen", obj.ARV_ReportExtraRegimen().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        //Lab
        public ActionResult Lab(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_WebTemplate_Laboratory.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_consumption", obj.Lab().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_header", obj.Lab_Header().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        //OIs
        public ActionResult OIs(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_WebTemplate_OI_STI.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_consumption", obj.OIs().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_header", obj.OIs_Header().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        //Viral Load Reagents
        public ActionResult ViralLoadReagents(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_WebTemplate_ViralLoadReagents.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_consumption", obj.Viralload().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_header", obj.Viralload_Header().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        //SMC
        public ActionResult SMC(string FacilityCode, string sdate, string edate)
        {
            int x = 0;
            //Boolean y = false;
            ReportsModel obj = new ReportsModel();
            if (int.TryParse(FacilityCode, out x))
                obj.FacilityCode = System.Convert.ToInt32(FacilityCode);

            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            obj.StartDate = StartDate;
            obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);

            //List<spView_Admin_Calender_ScheduleAppointmentsGetAll_Result> titles = obj.GetDetailedReport();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\Report_WebTemplate_SMC_SLM.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_consumption", obj.SMC().ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ds_header", obj.SMC_Header().ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
    }
}
