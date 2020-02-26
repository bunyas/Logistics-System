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
    public class QueriesController : Controller
    {
        // art  Queries
        mascisEntities context = new mascisEntities();
        public ActionResult ART_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        // art partial view Queries
        public ActionResult ART( string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if(_emmergency == 1)
            {
                emergency = false;
            }
            else if(_emmergency == 2)
            {
                emergency = true;
            }
            ARTModels obj = new ARTModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
           // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\ART.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("art", obj.GetAll(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public ActionResult FluconazoleQuery()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult Fluconazole(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            ARTModels obj = new ARTModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\Fluconazole.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("fluconazole", obj.GetTreatment_Fluconazole(StartDate,EndDate, emergency)));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public ActionResult PatientQuery()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult Patient(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            ARTModels obj = new ARTModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\ART_Patient.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("patient", obj.GetPatientSummary(StartDate, EndDate, emergency)));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public ActionResult TestKits_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
              new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult TestKits(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            HIVTestKitsModels obj = new HIVTestKitsModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\TestKits.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("testKits", obj.GetAll(StartDate, EndDate, emergency)));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult TestKitsSummary_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult TestKitSummary(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            HIVTestKitsModels obj = new HIVTestKitsModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\TestKitSummary.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("testKitSummary", obj.TestKitSummary(StartDate, EndDate, emergency)));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult Laboratory_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult Laboratory(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            LaboratoryModels obj = new LaboratoryModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\Laboratory.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("laboratory", obj.GetAll(StartDate, EndDate, emergency)));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult SMC_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult SMC(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            SMCModels obj = new SMCModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\SMC.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("sMC", obj.GetAll(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult SMCSummary_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult SMCSummary(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            SMCModels obj = new SMCModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\SMCSummary_Report.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("SMCSumaryDataSet", obj.SMCSummary(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult Viralload_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult Viralload(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            ViralLoadModels obj = new ViralLoadModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\Viralload.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("viralload", obj.GetAll(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult ViralloadSummary_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult ViralloadSummary(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            ViralLoadModels obj = new ViralLoadModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\ViralloadSummary.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("viralloadSummary", obj.ViralloadSummary(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult OISI_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult OISI(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            OISTIModels obj = new OISTIModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\OISI.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("oISI", obj.GetAll(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult TB_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult TB(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            TBModels obj = new TBModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\TB.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tB", obj.GetAll(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult TBSummary_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult TBSummary(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            TBModels obj = new TBModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\TB_Summary.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tBSummary", obj.GetTBSummary(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult Ruft_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult Ruft(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            RuftModels obj = new RuftModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\RUTF.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("rUTF", obj.GetAll(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult RuftSummary_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
                new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult RuftSummary(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            RuftModels obj = new RuftModels();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\RUTF_Summary.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("rUTFSummary", obj.GetRuftSummary(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public ActionResult All_Products_Index()
        {
            List<EmergencyType> emergency = new List<EmergencyType>()
            {
               new EmergencyType(){ID=0, Description ="Please Select the order type"},
                new EmergencyType(){ID=1, Description ="Routine"},
                new EmergencyType(){ID=2, Description ="Emergency"},
                new EmergencyType(){ID=3, Description="All Orders"}
            };
            ViewBag.Emergency = emergency;
            return View();
        }
        public ActionResult All_Products(string sdate, string edate, int? _emmergency)
        {
            bool? emergency = null;
            if (_emmergency == 1)
            {
                emergency = false;
            }
            else if (_emmergency == 2)
            {
                emergency = true;
            }
            ProductModel obj = new ProductModel();
            DateTime? StartDate = DateTime.ParseExact(sdate, "dd/MM/yyyy", null);
            DateTime? EndDate = DateTime.ParseExact(edate, "dd/MM/yyyy", null);
            // obj.GetAll(StartDate, EndDate);
            //obj.EndDate = EndDate;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Views\Queries\ALLProducts.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("aLLProducts", obj.GetOrders(StartDate, EndDate, emergency).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        private class EmergencyType
        {
            public int ID { get; set; }
            public string Description { get; set; }
        }
    }
}