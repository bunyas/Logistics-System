using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using Newtonsoft.Json;
using Syncfusion.JavaScript.DataSources;
using Syncfusion.JavaScript.Models;
using Syncfusion.JavaScript;
using System.Collections;
using Syncfusion.EJ;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using System.Web.Services;
using mascis.Models;
using System.IO;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.ComponentModel;
using System.Web.Mvc.Ajax;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using Syncfusion.Mvc.Pdf;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.EJ.PdfViewer;
using System.Data.Entity.Validation;
using System.Data.Entity.SqlServer;
using System.Configuration;
using mascis.ScheduledTasks;
using Microsoft.Reporting.WebForms;
using System.Linq.Expressions;

namespace mascis.Controllers
{
    public class fo_complaintController : Controller
    {
        private mascisEntities db = new mascisEntities();

        // GET: fo_complaint
        public ActionResult Index()
        {
            var fo_complaint = db.fo_complaint.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            return View(fo_complaint.ToList());
        }
        public ActionResult ADDComplaint()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.OrderBy(o => o.Release_Instruction_Desc).AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
                                                                                                                                 // ViewBag.Communication_mode = this.Getcommunication_modesJson();

            db.Configuration.ProxyCreationEnabled = false;
            var categories = db.fo_a_complaint_category.Where(e => e.complaint_category_code != 3 && e.complaint_category_code != 4 && e.complaint_category_code != 7 ).OrderBy(o => o.complaint_category_desc).AsNoTracking().ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});
            //db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_authorized_representative = db.fo_a_authorized_representative.OrderBy(o => o.Authorized_Representative_Desc).AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Documents_Attached = db.fo_a_Documents_Attached.ToList();
            db.Configuration.ProxyCreationEnabled = false;
            var prod_cat = db.A_product_category.OrderBy(o => o.category_desc).AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.fo_contact_category.OrderBy(o => o.category_desc).AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;
            db.Configuration.ProxyCreationEnabled = false;
            var comAcute = db.fo_a_complaint_accuteness.OrderBy(o => o.accuteness_desc).OrderBy(o => o.accuteness_desc).AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;


            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                db.Configuration.ProxyCreationEnabled = false;
                var login = db.AspNetUsers.FirstOrDefault(o => o.Email == User.Identity.Name);
                db.Configuration.ProxyCreationEnabled = false;
                var facilities = db.A_Facilities.Where(o => o.FacilityCode == login.FacilityId).ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = true;
            }
            else
            {
                db.Configuration.ProxyCreationEnabled = false;
                var facilities = db.A_Facilities.OrderBy(o => o.Facility).OrderBy(o => o.Facility).AsNoTracking().ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = false;
            }

            db.Configuration.ProxyCreationEnabled = false;
            var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.Products = prodcuts;
            db.Configuration.ProxyCreationEnabled = false;
            var prodcutsOP = db.A_Product.Where(p => p.product_category != 2 && p.product_category != 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.ProductsOP = prodcutsOP;
            db.Configuration.ProxyCreationEnabled = false;
            var dosage = db.fo_a_dosage.OrderBy(o => o.dosage_desc).AsNoTracking().ToArray();
            ViewBag.Dosage = dosage;
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_status = db.fo_a_status.ToList();

            db.Configuration.ProxyCreationEnabled = false;
            var maul_Staff = db.fo_contact_person.OrderBy(o => o.cp_name).Where(c => c.cp_category == 8).AsNoTracking().ToList();
            ViewBag.MAUL_Staff = maul_Staff;
            db.Configuration.ProxyCreationEnabled = false;
            var ContactPerson = db.fo_contact_person.OrderBy(o => o.cp_name).AsNoTracking().ToList();
            ViewBag.Contact_Person = ContactPerson;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            //-------------------- Insertion 23-Nov-2018
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();
            ViewBag.fo_a_action_taken = db.fo_a_action_taken.OrderBy(o => o.action_taken_desc).AsNoTracking().ToList();
            ViewBag.fo_contact_title = db.fo_contact_title.OrderBy(o => o.title_desc).AsNoTracking().ToList();
            ViewBag.A_ImplimentingPartners = db.A_ImplimentingPartners.OrderBy(o => o.ImplementingPartnerDescription).AsNoTracking().ToList();
            return View();
        }
        public ActionResult CCU_MasterChild()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();
            var categories = db.fo_a_complaint_category.ToList();

            var prod_cat = db.A_product_category.AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;

            var contacts = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;

            ViewBag.e_reg_complaint_category = categories;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;

            var comAcute = db.fo_a_complaint_accuteness.AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;

            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;

            var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).AsNoTracking().ToArray();
            ViewBag.Products = prodcuts;

            var prodcutsOP = db.A_Product.Where(p => p.product_category != 2 && p.product_category != 9).AsNoTracking().ToArray();
            ViewBag.ProductsOP = prodcutsOP;

            var dosage = db.fo_a_dosage.AsNoTracking().ToArray();
            ViewBag.Dosage = dosage;

            var maul_Staff = db.fo_contact_person.Where(c => c.cp_category == 8).AsNoTracking().ToList();
            ViewBag.MAUL_Staff = maul_Staff;
            ViewBag.e_reg_complaint_category = categories;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.AsNoTracking().ToList();
            ViewBag.fo_a_action_taken = db.fo_a_action_taken.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.AsNoTracking().ToList();
            return View();

        }
        public ActionResult ComplaintTracker()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
                                                                                   // ViewBag.Communication_mode = this.Getcommunication_modesJson();
                                                                                   //db.Configuration.ProxyCreationEnabled = false;
            var categories = db.fo_a_complaint_category.Where(e => e.complaint_category_code != 3 && e.complaint_category_code != 4 && e.complaint_category_code != 7 ).ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});
            //db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_authorized_representative = db.fo_a_authorized_representative.ToList();
            ViewBag.Documents_Attached = db.fo_a_Documents_Attached.ToList();
            var prod_cat = db.A_product_category.AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            //db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;

            var comAcute = db.fo_a_complaint_accuteness.AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;


            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                var login = db.AspNetUsers.FirstOrDefault(o => o.Email == User.Identity.Name);
                var facilities = db.A_Facilities.Where(o => o.FacilityCode == login.FacilityId).ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = true;
            }
            else
            {
                var facilities = db.A_Facilities.AsNoTracking().ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = false;
            }


            var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).AsNoTracking().ToArray();
            ViewBag.Products = prodcuts;

            var prodcutsOP = db.A_Product.Where(p => p.product_category != 2 && p.product_category != 9).AsNoTracking().ToArray();
            ViewBag.ProductsOP = prodcutsOP;

            var dosage = db.fo_a_dosage.AsNoTracking().ToArray();
            ViewBag.Dosage = dosage;
            ViewBag.fo_a_status = db.fo_a_status.ToList();

            var maul_Staff = db.fo_contact_person.OrderBy(o => o.cp_name).Where(c => c.cp_category == 8);
            ViewBag.MAUL_Staff = maul_Staff;
            var ContactPerson = db.fo_contact_person.OrderBy(o => o.cp_name).ToList();
            ViewBag.Contact_Person = ContactPerson;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            //-------------------- Insertion 23-Nov-2018
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();
            ViewBag.fo_a_action_taken = db.fo_a_action_taken.ToList();
            ViewBag.fo_contact_title = db.fo_contact_title.ToList();
            ViewBag.A_ImplimentingPartners = db.A_ImplimentingPartners.ToList();
            return View();
        }
        public ActionResult Complaint()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
            // ViewBag.Communication_mode = this.Getcommunication_modesJson();
            var categories = db.fo_a_complaint_category.AsNoTracking().ToArray();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});

            var prod_cat = db.A_product_category.AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;

            var contacts = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;

            var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
                                                        // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;

            var comAcute = db.fo_a_complaint_accuteness.AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;

            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;

            return View();
        }
        public ActionResult ComplaintReport()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;

            return View();
        }
        public ActionResult Complaint_Report(string FacilityCode, string startDate, string EndDate)
        {
            DateTime outpam = DateTime.Now;
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (DateTime.TryParse(startDate, out outpam))
            {
                sDate = outpam;
            }
            if (DateTime.TryParse(EndDate, out outpam))
            {
                eDate = outpam;
            }
            if (FacilityCode.Contains("null") || string.IsNullOrEmpty(FacilityCode))
            {
                FacilityCode = null;
            }
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\ComplaintReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("fo_Complaint", db.spView_crystal_fo_complaintGetAll(sDate, eDate, FacilityCode).ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Preliminary_Investigation_Report", db.spView_crystal_fo_Preliminary_Investigation_ReportGetAll(sDate, eDate, FacilityCode).ToList()));
            reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Detailed_Investigation_Report", db.spView_crystal_fo_Detailed_Investigation_ReportGetAll(sDate, eDate, FacilityCode).ToList()));

            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult BatchData_Complaint_Tracker(DataManager dataManager, int? e_reg_track_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //List<spReport_FreeholdPropertiesGetAll_Result> titles = atm.FreeholdProperties (TypeDesc,null,  ProjDesc, null);

            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            var list = (from t in db.fo_complaint_tracker
                        where t.e_reg_track_code == e_reg_track_code
                        orderby t.e_reg_track_added_date descending
                        select t).Take(5);

            IEnumerable DataSource;
            DataSource = list.ToList();// db.fo_complaint_tracker/*.Where(t => t.e_reg_track_code == e_reg_track_code)*/.ToList();
            result.result = DataSource;
            result.count = list.ToList().Count;// db.fo_complaint_tracker/*.Where(t => t.e_reg_track_code == e_reg_track_code)*/.ToList().Count();

            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneralComplaint()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();

            ViewBag.fo_a_action_taken = db.fo_a_action_taken.ToList();

            var categories = db.fo_a_complaint_category.ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});
            //db.Configuration.ProxyCreationEnabled = false;
            var prod_cat = db.A_product_category.AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            //db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;

            var maul_Staff = db.fo_contact_person.Where(c => c.cp_category == 8);
            ViewBag.MAUL_Staff = maul_Staff;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;

            var comAcute = db.fo_a_complaint_accuteness.AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;

            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;


            return View();
        }
        // CCU index
        public ActionResult CCU()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.OrderBy(o => o.Release_Instruction_Desc).AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
                                                                                                                                 // ViewBag.Communication_mode = this.Getcommunication_modesJson();

            db.Configuration.ProxyCreationEnabled = false;
            var categories = db.fo_a_complaint_category.Where(e => e.complaint_category_code != 3 && e.complaint_category_code != 4 && e.complaint_category_code != 7 ).OrderBy(o => o.complaint_category_desc).AsNoTracking().ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});
            //db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_authorized_representative = db.fo_a_authorized_representative.OrderBy(o => o.Authorized_Representative_Desc).AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Documents_Attached = db.fo_a_Documents_Attached.ToList();
            db.Configuration.ProxyCreationEnabled = false;
            var prod_cat = db.A_product_category.OrderBy(o => o.category_desc).AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.fo_contact_category.OrderBy(o => o.category_desc).AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;
            db.Configuration.ProxyCreationEnabled = false;
            var comAcute = db.fo_a_complaint_accuteness.OrderBy(o => o.accuteness_desc).OrderBy(o => o.accuteness_desc).AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;

            ViewBag.CurrentUser = User.IsInRole("CCU");
            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                db.Configuration.ProxyCreationEnabled = false;
                var login = db.AspNetUsers.FirstOrDefault(o => o.Email == User.Identity.Name);
                db.Configuration.ProxyCreationEnabled = false;
                var facilities = db.A_Facilities.Where(o => o.FacilityCode == login.FacilityId).ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = true;
            }
            else
            {
                db.Configuration.ProxyCreationEnabled = false;
                var facilities = db.A_Facilities.OrderBy(o => o.Facility).OrderBy(o => o.Facility).AsNoTracking().ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = false;
            }

            db.Configuration.ProxyCreationEnabled = false;
            var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.Products = prodcuts;
            db.Configuration.ProxyCreationEnabled = false;
            var prodcutsOP = db.A_Product.Where(p => p.product_category != 2 && p.product_category != 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.ProductsOP = prodcutsOP;
            db.Configuration.ProxyCreationEnabled = false;
            var dosage = db.fo_a_dosage.OrderBy(o => o.dosage_desc).AsNoTracking().ToArray();
            ViewBag.Dosage = dosage;
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_status = db.fo_a_status.ToList();

            db.Configuration.ProxyCreationEnabled = false;
            var maul_Staff = db.fo_contact_person.OrderBy(o => o.cp_name).Where(c => c.cp_category == 8).AsNoTracking().ToList();
            ViewBag.MAUL_Staff = maul_Staff;
            db.Configuration.ProxyCreationEnabled = false;
            var ContactPerson = db.fo_contact_person.OrderBy(o => o.cp_name).AsNoTracking().ToList();
            ViewBag.Contact_Person = ContactPerson;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            //-------------------- Insertion 23-Nov-2018
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();
            ViewBag.fo_a_action_taken = db.fo_a_action_taken.OrderBy(o => o.action_taken_desc).AsNoTracking().ToList();
            ViewBag.fo_contact_title = db.fo_contact_title.OrderBy(o => o.title_desc).AsNoTracking().ToList();
            ViewBag.A_ImplimentingPartners = db.A_ImplimentingPartners.OrderBy(o => o.ImplementingPartnerDescription).AsNoTracking().ToList();
            return View();
        }
        public ActionResult FacilityCCU()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.OrderBy(o => o.Release_Instruction_Desc).AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
                                                                                                                                 // ViewBag.Communication_mode = this.Getcommunication_modesJson();
                                                                                                                                 //db.Configuration.ProxyCreationEnabled = false;
            var categories = db.fo_a_complaint_category.Where(e => e.complaint_category_code != 3 && e.complaint_category_code != 4 && e.complaint_category_code != 7 ).AsNoTracking().ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});
            //db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_authorized_representative = db.fo_a_authorized_representative.AsNoTracking().ToList();
            ViewBag.Documents_Attached = db.fo_a_Documents_Attached.AsNoTracking().ToList();
            var prod_cat = db.A_product_category.OrderBy(o => o.category_desc).AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            //db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.fo_contact_category.Where(o => o.category_id == 3).OrderBy(o => o.category_desc).AsNoTracking().ToList();
            ViewBag.Contacts = contacts;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;

            var comAcute = db.fo_a_complaint_accuteness.OrderBy(o => o.accuteness_desc).AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;


            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                var login = db.AspNetUsers.FirstOrDefault(o => o.Email == User.Identity.Name);
                var facilities = db.A_Facilities.Where(o => o.FacilityCode == login.FacilityId).AsNoTracking().ToList();
                var _facilities = db.A_Facilities.FirstOrDefault(o => o.FacilityCode == login.FacilityId);
                ViewBag.Facilities = facilities;
                var district = db.A_District.Where(o => o.District_Code == _facilities.DistrrictCode).AsNoTracking().ToList();
                ViewBag.District = district;
                var ip = db.A_ImplimentingPartners.Where(o => o.ImplimentingPartnerCode == _facilities.ComprehensiveImplimentingPartnerCode).AsNoTracking().ToList();
                ViewBag.A_ImplimentingPartners = ip;
                ViewBag.role = true;
            }
            else
            {
                ViewBag.A_ImplimentingPartners = db.A_ImplimentingPartners.AsNoTracking().ToList();
                var facilities = db.A_Facilities.AsNoTracking().ToList();
                ViewBag.Facilities = facilities;
                ViewBag.role = false;
            }


            var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.Products = prodcuts;

            var prodcutsOP = db.A_Product.Where(p => p.product_category != 2 && p.product_category != 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.ProductsOP = prodcutsOP;

            var dosage = db.fo_a_dosage.OrderBy(o => o.dosage_desc).AsNoTracking().ToArray();
            ViewBag.Dosage = dosage;
            ViewBag.fo_a_status = db.fo_a_status.AsNoTracking().ToList();

            var maul_Staff = db.fo_contact_person.OrderBy(o => o.cp_name).Where(c => c.cp_category == 8).AsNoTracking().ToList();
            ViewBag.MAUL_Staff = maul_Staff;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            //-------------------- Insertion 23-Nov-2018
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();
            ViewBag.fo_a_action_taken = db.fo_a_action_taken.Where(o => o.action_taken_code > 5).OrderBy(o => o.action_taken_desc).AsNoTracking().ToList();
            ViewBag.fo_contact_title = db.fo_contact_title.OrderBy(o => o.title_desc).AsNoTracking().ToList();

            return View();
        }
        public ActionResult CCU_QA()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
                                                                                                                                 // ViewBag.Communication_mode = this.Getcommunication_modesJson();
                                                                                                                                 //db.Configuration.ProxyCreationEnabled = false;
            var categories = db.fo_a_complaint_category.Where(e => e.complaint_category_code != 3 && e.complaint_category_code != 4 && e.complaint_category_code != 7 ).OrderBy(o => o.complaint_category_desc).AsNoTracking().ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});
            //db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_authorized_representative = db.fo_a_authorized_representative.OrderBy(o => o.Authorized_Representative_Desc).ToList();
            ViewBag.Documents_Attached = db.fo_a_Documents_Attached.ToList();
            var prod_cat = db.A_product_category.OrderBy(o => o.category_desc).AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            //db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.fo_contact_category.OrderBy(o => o.category_desc).AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;

            var comAcute = db.fo_a_complaint_accuteness.OrderBy(o => o.accuteness_desc).AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;


            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                var login = db.AspNetUsers.FirstOrDefault(o => o.Email == User.Identity.Name);
                var facilities = db.A_Facilities.Where(o => o.FacilityCode == login.FacilityId).ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = true;
            }
            else
            {
                var facilities = db.A_Facilities.AsNoTracking().ToArray();
                ViewBag.Facilities = facilities;
                ViewBag.role = false;
            }


            var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.Products = prodcuts;

            var prodcutsOP = db.A_Product.Where(p => p.product_category != 2 && p.product_category != 9).OrderBy(o => o.product_description).AsNoTracking().ToArray();
            ViewBag.ProductsOP = prodcutsOP;

            var dosage = db.fo_a_dosage.OrderBy(o => o.dosage_desc).AsNoTracking().ToArray();
            ViewBag.Dosage = dosage;
            ViewBag.fo_a_status = db.fo_a_status.AsNoTracking().ToList();

            var maul_Staff = db.fo_contact_person.OrderBy(o => o.cp_name).Where(c => c.cp_category == 8).AsNoTracking().ToList();
            ViewBag.MAUL_Staff = maul_Staff;
            var ContactPerson = db.fo_contact_person.OrderBy(o => o.cp_name).AsNoTracking().ToList();
            ViewBag.Contact_Person = ContactPerson;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            //-------------------- Insertion 23-Nov-2018
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.OrderBy(o => o.comm_mode_desc).AsNoTracking().ToList();
            ViewBag.fo_a_action_taken = db.fo_a_action_taken.AsNoTracking().ToList();
            ViewBag.fo_contact_title = db.fo_contact_title.OrderBy(o => o.title_desc).AsNoTracking().ToList();
            ViewBag.A_ImplimentingPartners = db.A_ImplimentingPartners.OrderBy(o => o.ImplementingPartnerDescription).AsNoTracking().ToList();
            return View();
        }
        public ActionResult GetContactTitle(int? cp_code)
        {

            var data = db.fo_contact_title.ToList();
            var cp = db.fo_contact_person.FirstOrDefault(e => e.cp_code == cp_code);
            if (cp != null)
            {
                if (cp.cp_title == null)
                {
                    data = db.fo_contact_title.ToList();
                }
                else
                {
                    data = db.fo_contact_title.Where(e => e.title_code == cp.cp_title).ToList();
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Complaints()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction.AsNoTracking().ToList();
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
                                                                                   // ViewBag.Communication_mode = this.Getcommunication_modesJson();
                                                                                   //db.Configuration.ProxyCreationEnabled = false;
            var categories = db.fo_a_complaint_category.ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});
            //db.Configuration.ProxyCreationEnabled = false;
            var prod_cat = db.A_product_category.AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            //db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;

            var comAcute = db.fo_a_complaint_accuteness.AsNoTracking().ToArray();
            ViewBag.Accuteness = comAcute;

            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;

            var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).AsNoTracking().ToArray();
            ViewBag.Products = prodcuts;

            var prodcutsOP = db.A_Product.Where(p => p.product_category != 2 && p.product_category != 9).AsNoTracking().ToArray();
            ViewBag.ProductsOP = prodcutsOP;

            var dosage = db.fo_a_dosage.AsNoTracking().ToArray();
            ViewBag.Dosage = dosage;

            var maul_Staff = db.fo_contact_person.Where(c => c.cp_category == 8);
            ViewBag.MAUL_Staff = maul_Staff;
            //db.Configuration.ProxyCreationEnabled = false;
            //var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
            // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            //-------------------- Insertion 23-Nov-2018
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();
            ViewBag.fo_a_action_taken = db.fo_a_action_taken.ToList();
            return View();
        }

        public int complaintCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_complaint = db.fo_complaint.OrderByDescending(o => o.e_reg_complaint_code).Take(1).Select(f => f.e_reg_complaint_code).FirstOrDefault();
            return (fo_complaint);
        }

        public int complaintTrackCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_complaint = db.fo_complaint_tracker.OrderByDescending(o => o.e_reg_track_code).Take(1).Select(f => f.e_reg_track_code).FirstOrDefault();
            return (fo_complaint);
        }

        public int QualityIssueCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_complaint = db.fo_complaint_quality_issue.OrderByDescending(o => o.Id).Take(1).Select(f => f.Id).FirstOrDefault();
            return (fo_complaint);
        }
        public int QualityIssueCloseOutCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_complaint = db.fo_QI_CloseOut_Certificate.OrderByDescending(o => o.Id).Take(1).Select(f => f.Id).FirstOrDefault();
            return (fo_complaint);

        }
        public int QualityIssueReleaseCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_complaint = db.fo_Certificate_Release_frm_Quarantine.OrderByDescending(o => o.Id).Take(1).Select(f => f.Id).FirstOrDefault();
            return (fo_complaint);
        }
        public int ActionTakenCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_complaint = db.fo_complaint_actiontaken_tracker.OrderByDescending(o => o.Action_id).Take(1).Select(f => f.Action_id).FirstOrDefault();
            return (fo_complaint);
        }
        public string _ComplaintCode()
        {
            var tdate = DateTime.Now;
            string year = tdate.Year.ToString().Trim().Substring(2, 2);
            string month = tdate.Month.ToString();
            string searchCode = "-" + month + "-" + year;
            int count = db.fo_complaint.Where(o => o.e_reg_complaint_No.Contains(searchCode)).ToList().Count;
            int code = (count + 1);
            string result = code.ToString("D4") + "-" + month + "-" + year;
            return result;
        }
        public ActionResult ComplaintCodeString(/*string[] values*/)
        {
            try
            {
                //var tdate = DateTime.Now;
                //var tmoon = tdate.Month;
                //var tyear = tdate.Year;
                //var complaintCode = "";
                //db.Configuration.ProxyCreationEnabled = false;
                //var complaints = db.fo_complaint.ToList();
                //var fo_complaint = complaints.Where(t => /*SqlFunctions.DatePart("mm", t.e_reg_date_recieved) == tmoon && SqlFunctions.DatePart("yyyy", t.e_reg_date_recieved) == tyear*/ Convert.ToDateTime(t.e_reg_date_recieved).Month == tmoon && Convert.ToDateTime(t.e_reg_date_recieved).Year == tyear).ToList().Count;
                //if (fo_complaint == 0)
                //{
                //    fo_complaint = 1;
                //}
                //else
                //{
                //    fo_complaint = fo_complaint + 1;
                //}
                //complaintCode = fo_complaint.ToString("D4") + "-" + tmoon.ToString() + "-" + tyear.ToString().TrimEnd().Substring(2, 2);

                var tdate = DateTime.Now;
                string year = tdate.Year.ToString().Trim().Substring(2, 2);
                string month = tdate.Month.ToString();
                string searchCode = "-" + month + "-" + year;
                int count = db.fo_complaint.Where(o => o.e_reg_complaint_No.Contains(searchCode)).ToList().Count;
                int code = (count + 1);
                string result = code.ToString("D4") + "-" + month + "-" + year;

                return Json(new { msg = String.Format("{0}", result) }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult contactTitle(int title_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = db.fo_contact_title.FirstOrDefault(c => c.title_code == title_code);
            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult contactFacility(int fac_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var FacilityDetail = db.A_Facilities.FirstOrDefault(c => c.FacilityCode == fac_code);
            return Json(FacilityDetail, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult contactDistrict(int dist_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var DistrictDetail = db.A_District.FirstOrDefault(c => c.District_Code == dist_code);
            return Json(DistrictDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult contactDetails(int cp_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = db.fo_contact_person.FirstOrDefault(c => c.cp_code == cp_code);
            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult contactPersons(int? cp_category)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = db.fo_contact_person.ToList();
            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                var login = db.AspNetUsers.FirstOrDefault(o => o.Email == User.Identity.Name);
                contactDetail = db.fo_contact_person.Where(c => c.cp_category == cp_category && c.cp_facility == login.FacilityId).ToList();
            }
            else
            {
                contactDetail = db.fo_contact_person.Where(c => c.cp_category == cp_category).ToList();
            }
            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult contact_Details(string affectedsite)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = new List<fo_contact_person>();
            if (affectedsite.Contains(","))
            {
                var x = affectedsite.Split(',');
                foreach (var n in x)
                {
                    int a = Convert.ToInt32(n);
                    var m = db.fo_contact_person.Where(o => o.cp_facility == a).ToList();
                    if (m.Count > 0)
                    {
                        foreach (var b in m)
                        {
                            contactDetail.Add(b);
                        }
                    }

                }
            }
            else
            {
                int a = Convert.ToInt32(affectedsite);
                var m = db.fo_contact_person.Where(o => o.cp_facility == a).ToList();
                if (m.Count > 0)
                {
                    foreach (var b in m)
                    {
                        contactDetail.Add(b);
                    }
                }
            }


            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult contactPerson(int? cp_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = db.View_fo_Contacts.FirstOrDefault(c => c.cp_code == cp_code);
            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        /// <summary>
        /// Find the number of PreInvestigationGrid intems
        /// </summary>
        /// <returns></returns>
        public int PreInvestigationGrid()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ComplaintsModels cm = new ComplaintsModels();
            DataResult result = new DataResult();
            result.count = result.count = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").Count();// cm.GetIinvestigation(null, "6.", null, null).Count();


            return result.count;
        }

        [HttpGet]
        public int DetailInvestigationGrid()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ComplaintsModels cm = new ComplaintsModels();
            DataResult result = new DataResult();
            result.count = result.count = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").Count();// cm.GetIinvestigation(null, "6.", null, null).Count();
            return result.count;
        }
        public ActionResult BatchData(DataManager dataManager, string regCompCode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //List<spReport_FreeholdPropertiesGetAll_Result> titles = atm.FreeholdProperties (TypeDesc,null,  ProjDesc, null);
            ComplaintsModels cm = new ComplaintsModels();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            IEnumerable DataSource;

            if (string.IsNullOrEmpty(regCompCode))
            {
                DataSource = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").Count();
            }
            else
            {
                DataSource = cm.GetIinvestigation(null, "4.", regCompCode, null).ToList();
                result.result = DataSource;
                result.count = cm.GetIinvestigation(null, "4.", regCompCode, null).Count();
            }
            if (result.count <= 0)
            {
                DataSource = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").Count();
            }
            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchDataDetailInvestGrd(DataManager dataManager, string regCompCode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //List<spReport_FreeholdPropertiesGetAll_Result> titles = atm.FreeholdProperties (TypeDesc,null,  ProjDesc, null);
            ComplaintsModels cm = new ComplaintsModels();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            IEnumerable DataSource;

            if (string.IsNullOrEmpty(regCompCode))
            {
                DataSource = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").Count();
            }
            else
            {
                DataSource = cm.GetIinvestigation(null, "6.", regCompCode, null).ToList();
                result.result = DataSource;
                result.count = cm.GetIinvestigation(null, "6.", regCompCode, null).Count();
            }
            if (result.count <= 0)
            {
                DataSource = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").Count();
            }
            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchData_QI(DataManager dataManager, string regCompCode, int? Id, string batchno, int? product_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //List<spReport_FreeholdPropertiesGetAll_Result> titles = atm.FreeholdProperties (TypeDesc,null,  ProjDesc, null);
            ComplaintsModels cm = new ComplaintsModels();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            IEnumerable DataSource;

            if (string.IsNullOrEmpty(regCompCode))
            {
                DataSource = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").Count();
            }
            else
            {
                DataSource = cm.GetIinvestigation_QI(null, "4.", regCompCode, null, batchno, product_code).ToList();
                result.result = DataSource;
                result.count = cm.GetIinvestigation_QI(null, "4.", regCompCode, null, batchno, product_code).Count();
            }
            if (result.count <= 0)
            {
                DataSource = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "4.").Count();
            }
            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchDataDetailInvestGrd_QI(DataManager dataManager, string regCompCode, int? Id, string batchno, int? product_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //List<spReport_FreeholdPropertiesGetAll_Result> titles = atm.FreeholdProperties (TypeDesc,null,  ProjDesc, null);
            ComplaintsModels cm = new ComplaintsModels();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            IEnumerable DataSource;

            if (string.IsNullOrEmpty(regCompCode))
            {
                DataSource = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").Count();
            }
            else
            {
                DataSource = cm.GetIinvestigation_QI(null, "6.", regCompCode, null, batchno, product_code).ToList();
                result.result = DataSource;
                result.count = cm.GetIinvestigation_QI(null, "6.", regCompCode, null, batchno, product_code).Count();
            }
            if (result.count <= 0)
            {
                DataSource = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").ToList();
                result.result = DataSource;
                result.count = db.vw_fo_investigation_QI_Template.Where(t => t.investigation_code.Substring(0, 2) == "6.").Count();
            }
            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchData_Certificate_Release_frm_Quarantine(DataManager dataManager, string regCompCode, int? Id, string batchno, int? product_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            DataResult result = new DataResult();
            try
            {
                //int productcodeNo;
                //bool success = Int32.TryParse(productcode, out productcodeNo);
                ////if (e_reg_complaint_code != null && success && batchno != null)
                {
                    DataOperations operation = new DataOperations();
                    db.Configuration.ProxyCreationEnabled = false;
                    IEnumerable DataSource = db.fo_Certificate_Release_frm_Quarantine.Where(f => f.e_reg_complaint_No == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList();

                    //IEnumerable DataSource = db.FileCollectionSupDocs.ToList();
                    var muserrole = User.IsInRole("Administrator");
                    if (muserrole)
                    {
                    }
                    result.result = db.fo_Certificate_Release_frm_Quarantine.Where(f => f.e_reg_complaint_No == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList();
                    result.count = db.fo_Certificate_Release_frm_Quarantine.Where(f => f.e_reg_complaint_No == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

                    if (dataManager.Skip > 0)
                        result.result = operation.PerformSkip(result.result, dataManager.Skip);
                    if (dataManager.Take > 0)
                        result.result = operation.PerformTake(result.result, dataManager.Take);
                }
            }
            catch (Exception ex)
            {
                TempData["Success"] += string.Format("Property: {0} Error: {1}", ex.Message, ex.InnerException);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchUpdate_Certificate_Release(string key, List<fo_Certificate_Release_frm_Quarantine> changed, List<fo_Certificate_Release_frm_Quarantine> added, List<fo_Certificate_Release_frm_Quarantine> deleted)
        {
            var recordssaved = 0;
            try
            {
                var name = User.Identity.Name;
                var _name = name.Replace('.', ' ');
                var CP = db.fo_contact_person.FirstOrDefault(o => o.cp_name.ToLower() == _name.ToLower());

                db.Configuration.ProxyCreationEnabled = false;
                if (added != null /*&& added.Count() > 0*/)
                {
                    foreach (var temp in added)
                    {
                        if (CP != null)
                        {
                            temp.cp_code = CP.cp_code;
                        }
                        //fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        //o.batch_no == temp.batch_no && o.product_code == temp.product_code);

                        fo_Certificate_Release_frm_Quarantine old = db.fo_Certificate_Release_frm_Quarantine.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code && o.batch_no == temp.batch_no && o.product_code == temp.product_code);
                        if (string.IsNullOrEmpty(temp.Id.ToString()) || temp.Id == 0)
                        {
                            var newCode = QualityIssueCode();
                            temp.Id = ++newCode;
                        }

                        if (old != null)
                        {
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            // db.Entry(old).State = EntityState.Modified; 
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            old.e_reg_complaint_code = temp.e_reg_complaint_code;
                            old.product_code = temp.product_code;
                            old.batch_no = temp.batch_no;
                            old.e_reg_complaint_No = temp.e_reg_complaint_No;
                            old.Summary_of_Investigations = temp.Summary_of_Investigations;
                            old.Release_Instruction = temp.Release_Instruction;
                            old.product_strength = temp.product_strength;
                            old.dosage_form = temp.dosage_form;
                            old.manufacturer = temp.manufacturer;
                            old.Manufacture_date = temp.Manufacture_date;
                            old.Expiry_date = temp.Expiry_date;
                            old.Supplier = temp.Supplier;
                            old.Quantity = temp.Quantity;
                            old.cp_code = temp.cp_code;
                            old.Staff_name = temp.Staff_name;
                            old.Staff_phone = temp.Staff_phone;
                            old.Staff_Designation = temp.Staff_Designation;
                            old.Staff_email = temp.Staff_email;
                            old.Date_request = temp.Date_request;
                            old.product_other = temp.product_other;
                            old.attached_otherDoc = temp.attached_otherDoc;
                            old.attached_other = temp.attached_other;
                            old.Release_Authorizedby = temp.Release_Authorizedby;
                            old.Release_Authorized_Date = temp.Release_Authorized_Date;
                            old.Released_By = temp.Released_By;
                            old.Release_Date = temp.Release_Date;
                            old.Received_by = temp.Received_by;
                            old.Received_Date = temp.Received_Date;
                            old.Authorized_Representative_Category = temp.Authorized_Representative_Category;
                            db.Entry(old).State = EntityState.Modified;

                        }
                        else
                        {
                            db.fo_Certificate_Release_frm_Quarantine.Add(temp);
                        }
                    }
                    recordssaved = db.SaveChanges();
                }

                ////Performing update operation
                if (changed != null /*&& changed.Count() > 0*/)
                {
                    foreach (var temp in changed)
                    {
                        if (CP != null)
                        {
                            temp.cp_code = CP.cp_code;
                        }
                        fo_Certificate_Release_frm_Quarantine old = db.fo_Certificate_Release_frm_Quarantine.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code && o.batch_no == temp.batch_no && o.product_code == temp.product_code);
                        if (string.IsNullOrEmpty(temp.Id.ToString()))
                        {
                            var newCode = QualityIssueCode();
                            temp.Id = ++newCode;
                        }

                        if (old != null)
                        {
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            // db.Entry(old).State = EntityState.Modified; 
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            //old.description_of_quality_issue = temp.description_of_quality_issue;
                            //old.date_quality_issue_identified = temp.date_quality_issue_identified;
                            //old.e_reg_track_date_of_action = temp.e_reg_track_date_of_action;
                            //old.e_reg_track_action_details = temp.e_reg_track_action_details; 
                            //old.attached_picture = temp.attached_picture;

                            old.e_reg_complaint_code = temp.e_reg_complaint_code;
                            old.product_code = temp.product_code;
                            old.batch_no = temp.batch_no;
                            old.e_reg_complaint_No = temp.e_reg_complaint_No;
                            old.Summary_of_Investigations = temp.Summary_of_Investigations;
                            old.Release_Instruction = temp.Release_Instruction;
                            old.product_strength = temp.product_strength;
                            old.dosage_form = temp.dosage_form;
                            old.manufacturer = temp.manufacturer;
                            old.Manufacture_date = temp.Manufacture_date;
                            old.Expiry_date = temp.Expiry_date;
                            old.Supplier = temp.Supplier;
                            old.Quantity = temp.Quantity;
                            old.cp_code = temp.cp_code;
                            old.Staff_name = temp.Staff_name;
                            old.Staff_phone = temp.Staff_phone;
                            old.Staff_Designation = temp.Staff_Designation;
                            old.Staff_email = temp.Staff_email;
                            old.Date_request = temp.Date_request;
                            old.product_other = temp.product_other;
                            old.attached_otherDoc = temp.attached_otherDoc;
                            old.attached_other = temp.attached_other;
                            old.Release_Authorizedby = temp.Release_Authorizedby;
                            old.Release_Authorized_Date = temp.Release_Authorized_Date;
                            old.Released_By = temp.Released_By;
                            old.Release_Date = temp.Release_Date;
                            old.Received_by = temp.Received_by;
                            old.Received_Date = temp.Received_Date;
                            old.Authorized_Representative_Category = temp.Authorized_Representative_Category;
                            db.Entry(old).State = EntityState.Modified;

                        }
                        else
                        {

                            db.fo_Certificate_Release_frm_Quarantine.Add(temp);
                        }
                    }
                    recordssaved += db.SaveChanges();
                }

                //Performing delete operation
                if (deleted != null /*&& deleted.Count() > 0*/)
                {
                    foreach (var temp in deleted)
                    {
                        db.fo_Certificate_Release_frm_Quarantine.Remove(db.fo_Certificate_Release_frm_Quarantine.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        o.batch_no == temp.batch_no && o.product_code == temp.product_code));
                    }
                    recordssaved += db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["SaveError"] = ex.InnerException.Message;
                throw ex;
            }
            //finally
            //{

            //}

            return Content(recordssaved.ToString());

            //            return Json(new { msg = String.Format("{0}", recordssaved.ToString() + " Records Saved") }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BatchData_QICloseOut1(int? regCompCode, int? Id, string batchno, int? product_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.fo_QI_CloseOut_Certificate.Select(e => new { e.Summary_of_Investigations, e.batch_no }).ToList();
            //DataResult result = new DataResult();
            // try
            {
                //int productcodeNo;
                //bool success = Int32.TryParse(productcode, out productcodeNo);
                ////if (e_reg_complaint_code != null && success && batchno != null)
                //{
                //DataOperations operation = new DataOperations();
                //db.Configuration.ProxyCreationEnabled = false;
                //IEnumerable DataSource = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList();

                //IEnumerable DataSource = db.FileCollectionSupDocs.ToList();
                //    var muserrole = User.IsInRole("Administrator");
                //    if (muserrole)
                //    {
                //    }
                //    result.result = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList();
                //    result.count = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

                //    if (dataManager.Skip > 0)
                //        result.result = operation.PerformSkip(result.result, dataManager.Skip);
                //    if (dataManager.Take > 0)
                //        result.result = operation.PerformTake(result.result, dataManager.Take);
                //}
            }
            //catch (Exception ex)
            //{
            //    TempData["Success"] += string.Format("Property: {0} Error: {1}", ex.Message, ex.InnerException);

            //}

            return Json(DataSource, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BatchData_QICloseOut(DataManager dataManager, int? regCompCode, int? Id, string batchno, int? product_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            DataResult result = new DataResult();
            try
            {
                //int productcodeNo;
                //bool success = Int32.TryParse(productcode, out productcodeNo);
                ////if (e_reg_complaint_code != null && success && batchno != null)
                {
                    DataOperations operation = new DataOperations();
                    db.Configuration.ProxyCreationEnabled = false;
                    IEnumerable DataSource = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).Select(e => new { e.Id, e.batch_no, e.product_code, e.e_reg_complaint_code, e.Release_Instruction, e.Summary_of_Investigations, e.Documents_Attached, e.attached_otherDoc, e.Other_attachment, e.Director_Technical, e.Director_Technical_Date, e.Executive_Director, e.Executive_Director_Date, e.Finance_Manager, e.Finance_Date, e.Warehouse_Officer, e.Warehouse_Date }).ToList();

                    //IEnumerable DataSource = db.FileCollectionSupDocs.ToList();
                    var muserrole = User.IsInRole("Administrator");
                    if (muserrole)
                    {
                    }
                    result.result = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).Select(e => new { e.Id, e.batch_no, e.product_code, e.e_reg_complaint_code, e.Release_Instruction, e.Summary_of_Investigations, e.Documents_Attached, e.attached_otherDoc, e.Other_attachment, e.Director_Technical, e.Director_Technical_Date, e.Executive_Director, e.Executive_Director_Date, e.Finance_Manager, e.Finance_Date, e.Warehouse_Officer, e.Warehouse_Date }).ToList();
                    result.count = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).Select(e => new { e.Id, e.batch_no, e.product_code, e.e_reg_complaint_code, e.Release_Instruction, e.Summary_of_Investigations, e.Documents_Attached, e.attached_otherDoc, e.Other_attachment, e.Director_Technical, e.Director_Technical_Date, e.Executive_Director, e.Executive_Director_Date, e.Finance_Manager, e.Finance_Date, e.Warehouse_Officer, e.Warehouse_Date }).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

                    if (dataManager.Skip > 0)
                        result.result = operation.PerformSkip(result.result, dataManager.Skip);
                    if (dataManager.Take > 0)
                        result.result = operation.PerformTake(result.result, dataManager.Take);
                }
            }
            catch (Exception ex)
            {
                TempData["Success"] += string.Format("Property: {0} Error: {1}", ex.Message, ex.InnerException);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult BatchData_QICloseOut(DataManager dataManager, int? regCompCode, int? Id, string batchno, int? product_code)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    DataResult result = new DataResult();
        //    try
        //    {
        //        //int productcodeNo;
        //        //bool success = Int32.TryParse(productcode, out productcodeNo);
        //        ////if (e_reg_complaint_code != null && success && batchno != null)
        //        {
        //            DataOperations operation = new DataOperations();
        //            db.Configuration.ProxyCreationEnabled = false;
        //            IEnumerable DataSource = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList();

        //            //IEnumerable DataSource = db.FileCollectionSupDocs.ToList();
        //            var muserrole = User.IsInRole("Administrator");
        //            if (muserrole)
        //            {
        //            }
        //            result.result = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList();
        //            result.count = db.fo_QI_CloseOut_Certificate.Where(f => f.e_reg_complaint_code == regCompCode && f.product_code == product_code && f.batch_no == batchno).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

        //            if (dataManager.Skip > 0)
        //                result.result = operation.PerformSkip(result.result, dataManager.Skip);
        //            if (dataManager.Take > 0)
        //                result.result = operation.PerformTake(result.result, dataManager.Take);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Success"] += string.Format("Property: {0} Error: {1}", ex.Message, ex.InnerException);

        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult BatchUpdateQICloseOut(string key, List<fo_QI_CloseOut_Certificate> changed, List<fo_QI_CloseOut_Certificate> added, List<fo_QI_CloseOut_Certificate> deleted)
        {
            var recordssaved = 0;
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                if (added != null /*&& added.Count() > 0*/)
                {
                    foreach (var temp in added)
                    {
                        //fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        //o.batch_no == temp.batch_no && o.product_code == temp.product_code);

                        fo_QI_CloseOut_Certificate old = db.fo_QI_CloseOut_Certificate.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code && o.batch_no == temp.batch_no && o.product_code == temp.product_code);
                        if (string.IsNullOrEmpty(temp.Id.ToString()) || temp.Id == 0)
                        {
                            var newCode = QualityIssueCode();
                            temp.Id = ++newCode;
                        }

                        if (old != null)
                        {
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            // db.Entry(old).State = EntityState.Modified; 
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            old.e_reg_complaint_code = temp.e_reg_complaint_code;
                            old.product_code = temp.product_code;
                            old.batch_no = temp.batch_no;
                            old.Award_Reference = temp.Award_Reference;
                            old.Qty_Delivered = temp.Qty_Delivered;
                            old.Qty_Rejected = temp.Qty_Rejected;
                            old.Unit_Price_FOB = temp.Unit_Price_FOB;
                            old.Unit_Price_CIP_EBB = temp.Unit_Price_CIP_EBB;
                            old.Total_Value_FOB = temp.Total_Value_FOB;
                            old.Total_Value_CIP = temp.Total_Value_CIP;
                            old.e_reg_complaint_No = temp.e_reg_complaint_No;
                            old.Summary_of_Investigations = temp.Summary_of_Investigations;
                            old.Release_Instruction = temp.Release_Instruction;
                            old.product_strength = temp.product_strength;
                            old.dosage_form = temp.dosage_form;
                            old.manufacturer = temp.manufacturer;
                            old.Manufacture_date = temp.Manufacture_date;
                            old.Expiry_date = temp.Expiry_date;
                            old.Supplier = temp.Supplier;
                            old.Quantity = temp.Quantity;
                            old.Documents_Attached = temp.Documents_Attached;
                            old.attached_otherDoc = temp.attached_otherDoc;
                            old.Other_attachment = temp.Other_attachment;
                            old.Warehouse_Officer = temp.Warehouse_Officer;
                            old.Warehouse_Date = temp.Warehouse_Date;
                            old.Finance_Manager = temp.Finance_Manager;
                            old.Finance_Date = temp.Finance_Date;
                            old.Director_Technical = temp.Director_Technical;
                            old.Director_Technical_Date = temp.Director_Technical_Date;
                            old.Executive_Director = temp.Executive_Director;
                            old.Executive_Director_Date = temp.Executive_Director_Date;
                            db.Entry(old).State = EntityState.Modified;

                        }
                        else
                        {
                            db.fo_QI_CloseOut_Certificate.Add(temp);
                        }
                    }
                    recordssaved = db.SaveChanges();
                }

                ////Performing update operation
                if (changed != null /*&& changed.Count() > 0*/)
                {
                    foreach (var temp in changed)
                    {
                        fo_QI_CloseOut_Certificate old = db.fo_QI_CloseOut_Certificate.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code && o.batch_no == temp.batch_no && o.product_code == temp.product_code);
                        if (string.IsNullOrEmpty(temp.Id.ToString()))
                        {
                            var newCode = QualityIssueCode();
                            temp.Id = ++newCode;
                        }

                        if (old != null)
                        {
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            // db.Entry(old).State = EntityState.Modified; 
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            //old.description_of_quality_issue = temp.description_of_quality_issue;
                            //old.date_quality_issue_identified = temp.date_quality_issue_identified;
                            //old.e_reg_track_date_of_action = temp.e_reg_track_date_of_action;
                            //old.e_reg_track_action_details = temp.e_reg_track_action_details; 
                            //old.attached_picture = temp.attached_picture;

                            old.e_reg_complaint_code = temp.e_reg_complaint_code;
                            old.product_code = temp.product_code;
                            old.batch_no = temp.batch_no;
                            old.Award_Reference = temp.Award_Reference;
                            old.Qty_Delivered = temp.Qty_Delivered;
                            old.Qty_Rejected = temp.Qty_Rejected;
                            old.Unit_Price_FOB = temp.Unit_Price_FOB;
                            old.Unit_Price_CIP_EBB = temp.Unit_Price_CIP_EBB;
                            old.Total_Value_FOB = temp.Total_Value_FOB;
                            old.Total_Value_CIP = temp.Total_Value_CIP;
                            old.e_reg_complaint_No = temp.e_reg_complaint_No;
                            old.Summary_of_Investigations = temp.Summary_of_Investigations;
                            old.Release_Instruction = temp.Release_Instruction;
                            old.product_strength = temp.product_strength;
                            old.dosage_form = temp.dosage_form;
                            old.manufacturer = temp.manufacturer;
                            old.Manufacture_date = temp.Manufacture_date;
                            old.Expiry_date = temp.Expiry_date;
                            old.Supplier = temp.Supplier;
                            old.Quantity = temp.Quantity;
                            old.Documents_Attached = temp.Documents_Attached;
                            old.attached_otherDoc = temp.attached_otherDoc;
                            old.Other_attachment = temp.Other_attachment;
                            old.Warehouse_Officer = temp.Warehouse_Officer;
                            old.Warehouse_Date = temp.Warehouse_Date;
                            old.Finance_Manager = temp.Finance_Manager;
                            old.Finance_Date = temp.Finance_Date;
                            old.Director_Technical = temp.Director_Technical;
                            old.Director_Technical_Date = temp.Director_Technical_Date;
                            old.Executive_Director = temp.Executive_Director;
                            old.Executive_Director_Date = temp.Executive_Director_Date;
                            db.Entry(old).State = EntityState.Modified;

                        }
                        else
                        {

                            db.fo_QI_CloseOut_Certificate.Add(temp);
                        }
                    }
                    recordssaved += db.SaveChanges();
                }

                //Performing delete operation
                if (deleted != null /*&& deleted.Count() > 0*/)
                {
                    foreach (var temp in deleted)
                    {
                        db.fo_QI_CloseOut_Certificate.Remove(db.fo_QI_CloseOut_Certificate.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        o.batch_no == temp.batch_no && o.product_code == temp.product_code));
                    }
                    recordssaved += db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["SaveError"] = ex.InnerException.Message;
                throw ex;
            }
            //finally
            //{

            //}

            return Content(recordssaved.ToString());

            //            return Json(new { msg = String.Format("{0}", recordssaved.ToString() + " Records Saved") }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BatchUpdateQICloseOut2(int? Id, int e_reg_complaint_code, int product_code, string batchno,
            string e_reg_complaint_No, string Summary_of_Investigations, int Release_Instruction,
            int Complaint_Report, string Complaint_Report_Comment, int Notification_PAC,
            string Notification_PAC_Comment, int Notification_Vendor, string Notification_Vendor_Comment,
            int Product_COAs, string Product_COAs_Comment, int CAPA, string CAPA_Comment,
            int Returned_Note, string Returned_Note_Comment, int Credit_Note, string Credit_Note_Comment,
            int Reconciliation, string Reconciliation_Comment, int NDA_Notification,
            string NDA_Notification_Comment, string Other_attachment, int? Warehouse_Officer, string Warehouse_Date,
            int? Finance_Manager, string Finance_Date, int? Director_Technical, string Director_Technical_Date,
            int? Executive_Director, string Executive_Director_Date
            )
        {
            fo_QI_CloseOut_Certificate value = new fo_QI_CloseOut_Certificate();
            value.CAPA = CAPA;
            value.batch_no = batchno;
            value.CAPA_Comment = CAPA_Comment;
            value.Complaint_Report = Complaint_Report;
            value.Complaint_Report_Comment = Complaint_Report_Comment;
            value.Credit_Note = Credit_Note;
            value.Credit_Note_Comment = Credit_Note_Comment;
            value.Director_Technical = Director_Technical;
            value.Director_Technical_Date = null;
            if (!string.IsNullOrEmpty(Director_Technical_Date))
            {
                DateTime date = DateTime.ParseExact(Director_Technical_Date, "dd/MM/yyyy", null);
                value.Director_Technical_Date = date;
            }
            value.Executive_Director = Executive_Director;
            value.Executive_Director_Date = null;
            if (!string.IsNullOrEmpty(Executive_Director_Date))
            {
                DateTime date = DateTime.ParseExact(Executive_Director_Date, "dd/MM/yyyy", null);
                value.Executive_Director_Date = date;
            }
            value.e_reg_complaint_code = e_reg_complaint_code;
            value.e_reg_complaint_No = e_reg_complaint_No;
            value.Finance_Manager = Finance_Manager;
            value.Finance_Date = null;
            if (!string.IsNullOrEmpty(Finance_Date))
            {
                DateTime date = DateTime.ParseExact(Finance_Date, "dd/MM/yyyy", null);
                value.Finance_Date = date;
            }
            value.Id = Convert.ToInt32(Id);
            value.NDA_Notification = NDA_Notification;
            value.NDA_Notification_Comment = NDA_Notification_Comment;
            value.Notification_PAC = Notification_PAC;
            value.Notification_PAC_Comment = Notification_PAC_Comment;
            value.Notification_Vendor = Notification_Vendor;
            value.Notification_Vendor_Comment = Notification_Vendor_Comment;
            value.Other_attachment = Other_attachment;
            value.Product_COAs = Product_COAs;
            value.Product_COAs_Comment = Product_COAs_Comment;
            value.product_code = product_code;
            value.Reconciliation = Reconciliation;
            value.Reconciliation_Comment = Reconciliation_Comment;
            value.Release_Instruction = Release_Instruction;
            value.Returned_Note = Returned_Note;
            value.Returned_Note_Comment = Returned_Note_Comment;
            value.Summary_of_Investigations = Summary_of_Investigations;
            value.Warehouse_Officer = Warehouse_Officer;
            value.Warehouse_Date = null;
            if (!string.IsNullOrEmpty(Warehouse_Date))
            {
                DateTime date = DateTime.ParseExact(Warehouse_Date, "dd/MM/yyyy", null);
                value.Warehouse_Date = date;
            }

            fo_QI_CloseOut_Certificate old = db.fo_QI_CloseOut_Certificate.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code && o.batch_no == value.batch_no && o.product_code == value.product_code);
            if (string.IsNullOrEmpty(value.Id.ToString()) || value.Id == 0)
            {
                var newCode = QualityIssueCloseOutCode();
                value.Id = ++newCode;
            }
            if (old == null)
            {
                db.fo_QI_CloseOut_Certificate.Add(value);
            }
            else
            {
                value.Id = old.Id;
                db.Entry(old).CurrentValues.SetValues(value);
                db.Entry(old).State = EntityState.Modified;

            }
            db.SaveChanges();
            return Content("");
        }

        public ActionResult BatchUpdateQIRelease2(int? Id, int e_reg_complaint_code, int product_code, string batchno,
         string e_reg_complaint_No, string Summary_of_Investigations, int Release_Instruction,
         int Returned_Note, string Returned_Note_Comment, string Other_attachment, int? Release_Authorizedby, string Release_Authorized_Date,
         int? Released_By, string Release_Date, int? Received_by, string Received_Date
         )
        {
            fo_Certificate_Release_frm_Quarantine value = new fo_Certificate_Release_frm_Quarantine();
            value.batch_no = batchno;
            value.Release_Authorizedby = Release_Authorizedby;
            value.Release_Authorized_Date = null;
            if (!string.IsNullOrEmpty(Release_Authorized_Date))
            {
                DateTime date = DateTime.ParseExact(Release_Authorized_Date, "dd/MM/yyyy", null);
                value.Release_Authorized_Date = date;
            }
            value.Released_By = Released_By;
            value.Release_Date = null;
            if (!string.IsNullOrEmpty(Release_Date))
            {
                DateTime date = DateTime.ParseExact(Release_Date, "dd/MM/yyyy", null);
                value.Release_Date = date;
            }
            value.e_reg_complaint_code = e_reg_complaint_code;
            value.e_reg_complaint_No = e_reg_complaint_No;
            value.Received_by = Received_by;
            value.Received_Date = null;
            if (!string.IsNullOrEmpty(Received_Date))
            {
                DateTime date = DateTime.ParseExact(Received_Date, "dd/MM/yyyy", null);
                value.Received_Date = date;
            }
            value.attached_other = Other_attachment;
            value.product_code = product_code;
            value.Authorized_Representative_Category = null;
            value.Id = Convert.ToInt32(Id);
            value.Release_Instruction = Release_Instruction;
            value.Goods_Returned_Note = Returned_Note;
            value.Goods_Returned_Note_Commet = Returned_Note_Comment;
            value.Summary_of_Investigations = Summary_of_Investigations;
            fo_Certificate_Release_frm_Quarantine old = db.fo_Certificate_Release_frm_Quarantine.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code && o.batch_no == value.batch_no && o.product_code == value.product_code);
            if (string.IsNullOrEmpty(value.Id.ToString()) || value.Id == 0)
            {
                var newCode = QualityIssueReleaseCode();
                value.Id = ++newCode;
            }
            if (old == null)
            {
                db.fo_Certificate_Release_frm_Quarantine.Add(value);
            }
            else
            {
                value.Id = old.Id;
                db.Entry(old).CurrentValues.SetValues(value);
                db.Entry(old).State = EntityState.Modified;

            }
            db.SaveChanges();
            return Content("");
        }
        public ActionResult GetQICloseOut(int e_reg_complaint_code, int product_code, string batchno)
        {
            var data = db.View_fo_QI_CloseOut_Certificate.FirstOrDefault(o => o.e_reg_complaint_code == e_reg_complaint_code && o.product_code == product_code && o.batch_no == batchno);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetQIRelease(int e_reg_complaint_code, int product_code, string batchno)
        {
            var data = db.View_fo_Certificate_Release_frm_Quarantine.FirstOrDefault(o => o.e_reg_complaint_code == e_reg_complaint_code && o.product_code == product_code && o.batch_no == batchno);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Quality_Complaints(DataManager dataManager, string e_reg_complaint_code)
        {
            //    db.Configuration.ProxyCreationEnabled = false;
            QualityIssuesModel obj = new QualityIssuesModel();
            //    var data = obj.Quality_Issue(e_reg_complaint_code, null, null).ToList();

            // int? district = new UserManagement().getCurrentuserDistrict();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable DataSource = obj.Quality_Issue(e_reg_complaint_code, null, null).Where(p => p.product_category == 2 || p.product_category == 9).ToList();// db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList();

            var muserrole = User.IsInRole("Administrator");
            if (muserrole)
            {
            }
            DataSource = obj.Quality_Issue(e_reg_complaint_code, null, null).Where(p => p.product_category == 2 || p.product_category == 9).ToList();// db.fo_complaint_quality_issue.ToList(); 
            result.result = DataSource;
            result.count = obj.Quality_Issue(e_reg_complaint_code, null, null).Where(p => p.product_category == 2 || p.product_category == 9).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Quality_Complaints1(string e_reg_complaint_code)
        {
            //    db.Configuration.ProxyCreationEnabled = false;
            QualityIssuesModel obj = new QualityIssuesModel();
            //    var data = obj.Quality_Issue(e_reg_complaint_code, null, null).ToList();

            // int? district = new UserManagement().getCurrentuserDistrict();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = obj.Quality_Issue(e_reg_complaint_code, null, null).Where(p => p.product_category == 2 || p.product_category == 9).ToList();// db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList();

            return Json(DataSource, JsonRequestBehavior.AllowGet);

        }
        public ActionResult QualityComplaints(DataManager dataManager, string e_reg_complaint_code, string batchno, int productcode)
        {

            //    db.Configuration.ProxyCreationEnabled = false;
            // QualityIssuesModel obj = new QualityIssuesModel();
            //    var data = obj.Quality_Issue(e_reg_complaint_code, null, null).ToList();
            // int? district = new UserManagement().getCurrentuserDistrict();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable DataSource = db.fo_complaint_quality_issue.Where(o => o.e_reg_complaint_No == e_reg_complaint_code && o.batch_no == batchno && o.product_code == productcode).ToList();// db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList();

            var muserrole = User.IsInRole("Administrator");
            if (muserrole)
            {
            }
            DataSource = db.fo_complaint_quality_issue.Where(o => o.e_reg_complaint_No == e_reg_complaint_code && o.batch_no == batchno && o.product_code == productcode).ToList();// db.fo_complaint_quality_issue.ToList(); 
            result.result = DataSource;
            result.count = db.fo_complaint_quality_issue.Where(o => o.e_reg_complaint_No == e_reg_complaint_code && o.batch_no == batchno && o.product_code == productcode).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LoadGridFiles(DataManager dataManager, string e_reg_complaint_code, string batchno, int? productcode)
        {
            DataResult result = new DataResult();
            try
            {
                //int productcodeNo;
                //bool success = Int32.TryParse(productcode, out productcodeNo);
                ////if (e_reg_complaint_code != null && success && batchno != null)
                {
                    DataOperations operation = new DataOperations();
                    db.Configuration.ProxyCreationEnabled = false;
                    IEnumerable DataSource = db.FileCollectionSupDocs.Where(f => f.e_reg_complaint_No == e_reg_complaint_code && f.product_code == productcode && f.batch_no == batchno).ToList();

                    //IEnumerable DataSource = db.FileCollectionSupDocs.ToList();
                    var muserrole = User.IsInRole("Administrator");
                    if (muserrole)
                    {
                    }
                    result.result = db.FileCollectionSupDocs.Where(f => f.e_reg_complaint_No == e_reg_complaint_code && f.product_code == productcode && f.batch_no == batchno).ToList(); ;
                    result.count = db.FileCollectionSupDocs.Where(f => f.e_reg_complaint_No == e_reg_complaint_code && f.product_code == productcode && f.batch_no == batchno).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

                    if (dataManager.Skip > 0)
                        result.result = operation.PerformSkip(result.result, dataManager.Skip);
                    if (dataManager.Take > 0)
                        result.result = operation.PerformTake(result.result, dataManager.Take);
                }
            }
            catch (Exception ex)
            {
                TempData["Success"] += string.Format("Property: {0} Error: {1}", ex.Message, ex.InnerException);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Quality_ComplaintsOP(DataManager dataManager, string e_reg_complaint_code)
        {
            //    db.Configuration.ProxyCreationEnabled = false;
            QualityIssuesModel obj = new QualityIssuesModel();
            //    var data = obj.Quality_Issue(e_reg_complaint_code, null, null).ToList();

            // int? district = new UserManagement().getCurrentuserDistrict();
            DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable DataSource = obj.Quality_Issue(e_reg_complaint_code, null, null).Where(p => p.product_category != 2 && p.product_category != 9).ToList();// db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList();

            var muserrole = User.IsInRole("Administrator");
            if (muserrole)
            {
            }
            DataSource = obj.Quality_Issue(e_reg_complaint_code, null, null).Where(p => p.product_category != 2 && p.product_category != 9).ToList();// db.fo_complaint_quality_issue.ToList(); 
            result.result = DataSource;
            result.count = obj.Quality_Issue(e_reg_complaint_code, null, null).Where(p => p.product_category != 2 && p.product_category != 9).ToList().Count();//  db.fo_complaint_quality_issue.Where(a => a.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();

            if (dataManager.Skip > 0)
                result.result = operation.PerformSkip(result.result, dataManager.Skip);
            if (dataManager.Take > 0)
                result.result = operation.PerformTake(result.result, dataManager.Take);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DialogInsert(fo_complaint value, string testdata, string testdata2, string testdata3)
        {
            db.Configuration.ProxyCreationEnabled = false;
            fo_complaint table = db.fo_complaint.FirstOrDefault(o =>
            o.e_reg_complaint_code == value.e_reg_complaint_code);

            if (table == null)
            {
                db.fo_complaint.Add(value);
                db.SaveChanges();
            }
            else
            {
                this.DialogUpdate(value, testdata, testdata2, testdata3);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComplaintSaved(int e_reg_complaint_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            fo_complaint table = db.fo_complaint.FirstOrDefault(o =>
            o.e_reg_complaint_code == e_reg_complaint_code);
            return Json(table, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DialogUpdate(fo_complaint value, string testdata, string testdata2, string testdata3)
        {
            value.AffectedSites = testdata;
            db.Configuration.ProxyCreationEnabled = false;
            fo_complaint table = db.fo_complaint.FirstOrDefault(o =>
           o.e_reg_complaint_code == value.e_reg_complaint_code);

            if (table != null)
            {
                //value.Edited_Date = DateTime.Now;
                //value.Edited_By = "Wakayima!!!";
                db.Entry(table).CurrentValues.SetValues(value);
                db.Entry(table).State = EntityState.Modified;

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    db.SaveChanges();
                    TempData["Success"] = "Record Saved Successfully!";
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogInsert2(fo_complaint value,
            DateTime? DateReceived, string date_complaint, string e_reg_date_complaint,
            string e_reg_date_resolved, string Communicated_By_Lev1Date,
            string e_reg_expected_date_resolution,
            string Feedback_Date,
            string Communicated_By_Lev2Date, string AffectedSites)
        {
            db.Configuration.ProxyCreationEnabled = false;
            DateTime dateValue;
            if (value.e_reg_date_recieved == null)
            {
                value.e_reg_date_recieved = DateReceived;
                //if (DateTime.TryParse(DateReceived, out dateValue))
                //{
                //    value.e_reg_date_recieved = dateValue;
                //}
                //else
                //{
                //    value.e_reg_date_recieved = null;
                //}
            }

            if (value.e_reg_date_complaint == null)
            {
                if (DateTime.TryParse(date_complaint, out dateValue))
                {
                    value.e_reg_date_complaint = dateValue;
                }
                else
                {
                    value.e_reg_date_complaint = null;
                }
            }

            if (value.e_reg_date_resolved == null)
            {
                if (DateTime.TryParse(e_reg_date_resolved, out dateValue))
                {
                    value.e_reg_date_resolved = dateValue;
                }
                else
                {
                    value.e_reg_date_resolved = null;
                }
            }

            if (value.Communicated_By_Lev1Date == null)
            {
                if (DateTime.TryParse(Communicated_By_Lev1Date, out dateValue))
                {
                    value.Communicated_By_Lev1Date = dateValue;
                }
                else
                {
                    value.Communicated_By_Lev1Date = null;
                }
            }
            if (value.e_reg_expected_date_resolution == null)
            {
                if (DateTime.TryParse(e_reg_expected_date_resolution, out dateValue))
                {
                    value.e_reg_expected_date_resolution = dateValue;
                }
                else
                {
                    value.e_reg_expected_date_resolution = null;
                }
            }
            if (value.Feedback_Date == null)
            {
                if (DateTime.TryParse(Feedback_Date, out dateValue))
                {
                    value.Feedback_Date = dateValue;
                }
                else
                {
                    value.Feedback_Date = null;
                }
            }
            if (value.Communicated_By_Lev2Date == null)
            {
                if (DateTime.TryParse(Communicated_By_Lev2Date, out dateValue))
                {
                    value.Communicated_By_Lev2Date = dateValue;
                }
                else
                {
                    value.Communicated_By_Lev2Date = null;
                }
            }
            var muserrole1 = User.IsInRole("HSIPClient");
            if (muserrole1) { }
            if (!muserrole1) { value.FinalSubmission = 1; }

            fo_complaint table = db.fo_complaint.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);

            if (table == null)
            {
                if (value.e_reg_communication_mode != null || value.e_reg_communication_mode > 0) { }
                else
                {
                    value.e_reg_communication_mode = 4;
                }

                db.fo_complaint.Add(value);
                db.SaveChanges();

            }
            if (table != null)
            {

                db.Entry(table).CurrentValues.SetValues(value);
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();

            }

            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                if (value.FinalSubmission == 1)
                {
                    ///if its the facility
                    int? facility_id = new UserManagement().getCurrentuserFacility();
                    EmailJob email = new EmailJob();
                    email.SendEmailCCU(facility_id, value.e_reg_complaint_No);
                    //email.SendEmailFacilityCCU(facility_id, value.e_reg_complaint_No);
                    if (value.e_reg_complaint_category == 1)
                    {
                        email.SendEmailQA(facility_id, value.e_reg_complaint_No);
                    }
                }

            }
            if (!muserrole)
            {
                value.FinalSubmission = 1;
                /// if ccu admin
                if (!string.IsNullOrEmpty(value.Level1_Assignment))
                {
                    var check = db.fo_Emails_Sent.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
                    if (check == null || check.Level_One == false || check.Level_One == null)
                    {
                        EmailJob email = new EmailJob();
                        email.SendEmailLev1(value.Level1_Assignment, value.e_reg_complaint_No);
                        value.Level1_Email_Sent = true;
                        if (check == null)
                        {
                            int count = db.fo_Emails_Sent.ToList().Count;
                            var mail = new fo_Emails_Sent()
                            {
                                Email_ID = (count + 1),
                                e_reg_complaint_code = value.e_reg_complaint_code,
                                e_reg_complaint_No = value.e_reg_complaint_No,
                                Sent_Date = DateTime.Now,
                                Level_One = true,
                                Level_Three = false,
                                Level_Two = false
                            };
                            db.fo_Emails_Sent.Add(mail);
                            db.SaveChanges();
                        }
                        else
                        {
                            check.Level_One = true;
                            check.Sent_Date = DateTime.Now;
                            db.Entry(check).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
                if (!string.IsNullOrEmpty(value.Level2_Assignment))
                {
                    var check = db.fo_Emails_Sent.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
                    if (check == null || check.Level_Two == false || check.Level_Two == null)
                    {
                        EmailJob email = new EmailJob();
                        email.SendEmailLev2(value.Level2_Assignment, value.e_reg_complaint_No);
                        value.Level2_Email_Sent = true;
                        if (check == null)
                        {
                            int count = db.fo_Emails_Sent.ToList().Count;
                            var mail = new fo_Emails_Sent()
                            {
                                Email_ID = (count + 1),
                                e_reg_complaint_code = value.e_reg_complaint_code,
                                e_reg_complaint_No = value.e_reg_complaint_No,
                                Sent_Date = DateTime.Now,
                                Level_One = false,
                                Level_Three = false,
                                Level_Two = true
                            };
                            db.fo_Emails_Sent.Add(mail);
                            db.SaveChanges();
                        }
                        else
                        {
                            check.Level_Two = true;
                            check.Sent_Date = DateTime.Now;
                            db.Entry(check).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
                if (value.e_reg_complaint_status == 2)
                {
                    /// send a message to the level one responsible person
                    EmailJob email = new EmailJob();
                    email.SendEmailComplaintResolved(value.AffectedSites, value.e_reg_complaint_No);
                }
            }
            ViewData["woks"] = "Success";
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveComplaint(fo_complaint value)
        {
            ComplaintDetails details = new ComplaintDetails();
            if(value.e_reg_complaint_code == 0)
            {
                int code = complaintCode();
                value.e_reg_complaint_code = (code + 1);
            }
            value.e_reg_date_recieved = DateTime.Now.Date;
            var check_Code = db.fo_complaint.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
            if(check_Code == null) {}
            if (check_Code != null)
            {
                int code = complaintCode();
                value.e_reg_complaint_code = (code + 1);
            }
            var check_No = db.fo_complaint.FirstOrDefault(o => o.e_reg_complaint_No == value.e_reg_complaint_No);
            if (check_No == null){}
            if (check_No != null)
            {
                value.e_reg_complaint_No = _ComplaintCode();
            }
            var muserrole1 = User.IsInRole("HSIPClient");
            if (muserrole1) { }
            if (!muserrole1) { value.FinalSubmission = 1; }
            if (value.e_reg_communication_mode == null || value.e_reg_communication_mode == 0) { value.e_reg_communication_mode = 4; }
            if (!string.IsNullOrEmpty(value.Level1_Assignment)){ value.Level1_Assignment_Date = DateTime.Now; }
            if(value.e_reg_date_complaint == null) { value.e_reg_date_complaint = DateTime.Now; }

            db.fo_complaint.Add(value);
            db.SaveChanges();

            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                if (value.FinalSubmission == 1)
                {
                    ///if its the facility
                    int? facility_id = new UserManagement().getCurrentuserFacility();
                    EmailJob email = new EmailJob();
                    email.SendEmailCCU(facility_id, value.e_reg_complaint_No);
                    //email.SendEmailFacilityCCU(facility_id, value.e_reg_complaint_No);
                    if (value.e_reg_complaint_category == 1)
                    {
                        email.SendEmailQA(facility_id, value.e_reg_complaint_No);
                    }
                }

            }
            if (!muserrole)
            {
                /// if ccu admin
                if (!string.IsNullOrEmpty(value.Level1_Assignment))
                {
                    var check = db.fo_Emails_Sent.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
                    if (check == null || check.Level_One == false || check.Level_One == null)
                    {
                        EmailJob email = new EmailJob();
                        email.SendEmailLev1(value.Level1_Assignment, value.e_reg_complaint_No);
                        value.Level1_Email_Sent = true;
                        if (check == null)
                        {
                            int count = db.fo_Emails_Sent.ToList().Count;
                            var mail = new fo_Emails_Sent()
                            {
                                Email_ID = (count + 1),
                                e_reg_complaint_code = value.e_reg_complaint_code,
                                e_reg_complaint_No = value.e_reg_complaint_No,
                                Sent_Date = DateTime.Now,
                                Level_One = true,
                                Level_Three = false,
                                Level_Two = false
                            };
                            db.fo_Emails_Sent.Add(mail);
                            db.SaveChanges();
                        }
                        else
                        {
                            check.Level_One = true;
                            check.Sent_Date = DateTime.Now;
                            db.Entry(check).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
                if (!string.IsNullOrEmpty(value.Level2_Assignment))
                {
                    var check = db.fo_Emails_Sent.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
                    if (check == null || check.Level_Two == false || check.Level_Two == null)
                    {
                        EmailJob email = new EmailJob();
                        email.SendEmailLev2(value.Level2_Assignment, value.e_reg_complaint_No);
                        value.Level2_Email_Sent = true;
                        if (check == null)
                        {
                            int count = db.fo_Emails_Sent.ToList().Count;
                            var mail = new fo_Emails_Sent()
                            {
                                Email_ID = (count + 1),
                                e_reg_complaint_code = value.e_reg_complaint_code,
                                e_reg_complaint_No = value.e_reg_complaint_No,
                                Sent_Date = DateTime.Now,
                                Level_One = false,
                                Level_Three = false,
                                Level_Two = true
                            };
                            db.fo_Emails_Sent.Add(mail);
                            db.SaveChanges();
                        }
                        else
                        {
                            check.Level_Two = true;
                            check.Sent_Date = DateTime.Now;
                            db.Entry(check).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
                if (value.e_reg_complaint_status == 2)
                {
                    /// send a message to the level one responsible person
                    EmailJob email = new EmailJob();
                    email.SendEmailComplaintResolved(value.AffectedSites, value.e_reg_complaint_No);
                }
            }
            details.ComplaintCode = value.e_reg_complaint_code;
            details.ComplaintNo = value.e_reg_complaint_No;

            return Json(details, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogUpdate2(fo_complaint value,
            DateTime? DateReceived, string date_complaint, string e_reg_date_complaint,
            string e_reg_date_resolved, string Communicated_By_Lev1Date,
            string e_reg_expected_date_resolution,
            string Feedback_Date,
            string Communicated_By_Lev2Date, string AffectedSites)
        {
            db.Configuration.ProxyCreationEnabled = false;
            fo_complaint table = db.fo_complaint.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
            DateTime dateValue;
            if (value.e_reg_date_recieved == null)
            {
                value.e_reg_date_recieved = DateReceived;
                if (DateReceived == null)
                {
                    value.e_reg_date_recieved = DateTime.Now;
                }
                //if (DateTime.TryParse(DateReceived, out dateValue))
                //{
                //    value.e_reg_date_recieved = dateValue;
                //}
                //else
                //{
                //    value.e_reg_date_recieved = null;
                //}
            }

            if (value.e_reg_date_complaint == null)
            {
                if (DateTime.TryParse(date_complaint, out dateValue))
                {
                    value.e_reg_date_complaint = dateValue;
                }
                else
                {
                    value.e_reg_date_complaint = null;
                }
            }

            if (value.e_reg_date_resolved == null)
            {
                if (DateTime.TryParse(e_reg_date_resolved, out dateValue))
                {
                    value.e_reg_date_resolved = dateValue;
                }
                else
                {
                    value.e_reg_date_resolved = null;
                }
            }

            if (value.Communicated_By_Lev1Date == null)
            {
                if (DateTime.TryParse(Communicated_By_Lev1Date, out dateValue))
                {
                    value.Communicated_By_Lev1Date = dateValue;
                }
                else
                {
                    value.Communicated_By_Lev1Date = null;
                }
            }
            if (value.e_reg_expected_date_resolution == null)
            {
                if (DateTime.TryParse(e_reg_expected_date_resolution, out dateValue))
                {
                    value.e_reg_expected_date_resolution = dateValue;
                }
                else
                {
                    value.e_reg_expected_date_resolution = null;
                }
            }
            if (value.Feedback_Date == null)
            {
                if (DateTime.TryParse(Feedback_Date, out dateValue))
                {
                    value.Feedback_Date = dateValue;
                }
                else
                {
                    value.Feedback_Date = null;
                }
            }
            if (value.Communicated_By_Lev2Date == null)
            {
                if (DateTime.TryParse(Communicated_By_Lev2Date, out dateValue))
                {
                    value.Communicated_By_Lev2Date = dateValue;
                }
                else
                {
                    value.Communicated_By_Lev2Date = null;
                }
            }

            if (table != null)
            {
                //value.Edited_Date = DateTime.Now;
                //value.Edited_By = "Wakayima!!!";


                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges


                    var muserrole = User.IsInRole("HSIPClient");
                    if (muserrole)
                    {
                        //if (value.FinalSubmission == 1)
                        //{
                        //    ///if its the facility
                        //    int? facility_id = new UserManagement().getCurrentuserFacility();
                        //    EmailJob email = new EmailJob();
                        //    email.SendEmailCCU(facility_id, value.e_reg_complaint_No);
                        //    //email.SendEmailFacilityCCU(facility_id, value.e_reg_complaint_No);
                        //}

                    }
                    if (!muserrole)
                    {
                        value.FinalSubmission = 1;

                    }
                    db.Entry(table).CurrentValues.SetValues(value);
                    db.Entry(table).State = EntityState.Modified;
                    db.SaveChanges();
                    if (muserrole)
                    {
                        if (value.FinalSubmission == 1)
                        {
                            ///if its the facility
                            int? facility_id = new UserManagement().getCurrentuserFacility();
                            EmailJob email = new EmailJob();
                            email.SendEmailCCU(facility_id, value.e_reg_complaint_No);
                            //email.SendEmailFacilityCCU(facility_id, value.e_reg_complaint_No);
                        }

                    }
                    if (!muserrole)
                    {
                        if (!string.IsNullOrEmpty(value.Level1_Assignment))
                        {
                            var check = db.fo_Emails_Sent.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
                            if (check == null || check.Level_One == false || check.Level_One == null)
                            {
                                EmailJob email = new EmailJob();
                                email.SendEmailLev1(value.Level1_Assignment, value.e_reg_complaint_No);
                                value.Level1_Email_Sent = true;
                                if (check == null)
                                {
                                    int count = db.fo_Emails_Sent.ToList().Count;
                                    var mail = new fo_Emails_Sent()
                                    {
                                        Email_ID = (count + 1),
                                        e_reg_complaint_code = value.e_reg_complaint_code,
                                        e_reg_complaint_No = value.e_reg_complaint_No,
                                        Sent_Date = DateTime.Now,
                                        Level_One = true,
                                        Level_Three = false,
                                        Level_Two = false
                                    };
                                    db.fo_Emails_Sent.Add(mail);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    check.Level_One = true;
                                    check.Sent_Date = DateTime.Now;
                                    db.Entry(check).State = EntityState.Modified;
                                    db.SaveChanges();
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(value.Level2_Assignment))
                        {
                            var check = db.fo_Emails_Sent.FirstOrDefault(o => o.e_reg_complaint_code == value.e_reg_complaint_code);
                            if (check == null || check.Level_Two == false || check.Level_Two == null)
                            {
                                EmailJob email = new EmailJob();
                                email.SendEmailLev2(value.Level2_Assignment, value.e_reg_complaint_No);
                                value.Level2_Email_Sent = true;
                                if (check == null)
                                {
                                    int count = db.fo_Emails_Sent.ToList().Count;
                                    var mail = new fo_Emails_Sent()
                                    {
                                        Email_ID = (count + 1),
                                        e_reg_complaint_code = value.e_reg_complaint_code,
                                        e_reg_complaint_No = value.e_reg_complaint_No,
                                        Sent_Date = DateTime.Now,
                                        Level_One = false,
                                        Level_Three = false,
                                        Level_Two = true
                                    };
                                    db.fo_Emails_Sent.Add(mail);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    check.Level_Two = true;
                                    check.Sent_Date = DateTime.Now;
                                    db.Entry(check).State = EntityState.Modified;
                                    db.SaveChanges();
                                }

                            }
                        }
                        if (value.e_reg_complaint_status == 2)
                        {
                            /// send a message to the level one responsible person
                            EmailJob email = new EmailJob();
                            email.SendEmailComplaintResolved(value.AffectedSites, value.e_reg_complaint_No);
                        }

                        /// if ccu admin
                        //if (Convert.ToDateTime(value.Communicated_By_Lev1Date).Date == DateTime.Now.Date)
                        //{
                        //    if (value.Level1_Email_Sent == true) { }
                        //    else
                        //    {
                        //        /// send a message to the level one responsible person
                        //        EmailJob email = new EmailJob();
                        //        email.SendEmailLev1(value.Communicated_By_Lev1, value.e_reg_complaint_No);
                        //        value.Level1_Email_Sent = true;

                        //    }

                        //}
                        //else if (value.Communicated_By_Lev2 != null && Convert.ToDateTime(value.Communicated_By_Lev2Date).Date == DateTime.Now.Date)
                        //{
                        //    if (value.Level2_Email_Sent == true) { }
                        //    else
                        //    {
                        //        /// send a message to the level one responsible person
                        //        EmailJob email = new EmailJob();
                        //        email.SendEmailLev2(Convert.ToInt32(value.Communicated_By_Lev2), value.e_reg_complaint_No);
                        //        value.Level2_Email_Sent = true;
                        //    }

                        //}
                        //else if(value.e_reg_complaint_status == 2)
                        //{
                        //    /// send a message to the level one responsible person
                        //    EmailJob email = new EmailJob();
                        //    email.SendEmailComplaintResolved(value.AffectedSites, value.e_reg_complaint_No);
                        //}

                    }

                    TempData["Success"] = "Record Saved Successfully!";
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchUpdate(string key, List<fo_complaint_investigation> changed, List<fo_complaint_investigation> added, List<fo_complaint_investigation> deleted)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var recordssaved = 0;
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    fo_complaint_investigation old = db.fo_complaint_investigation.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code && o.investigation_code == temp.investigation_code);

                    if (old != null)
                    {
                        db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_investigation.Add(temp);
                    }
                }
                recordssaved = db.SaveChanges();
            }

            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    fo_complaint_investigation old = db.fo_complaint_investigation.
                        FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code && o.investigation_code == temp.investigation_code);


                    if (old != null)
                    {
                        db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_investigation.Add(temp);
                    }
                }
                recordssaved += db.SaveChanges();
            }

            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    db.fo_complaint_investigation.Remove(db.fo_complaint_investigation.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code && o.investigation_code == temp.investigation_code));
                }
                recordssaved += db.SaveChanges();
            }

            return (null);

            // return Json(new {msg = String.Format("{0}", recordssaved.ToString()+" Records Saved")}, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BatchUpdate_QI(string key, List<fo_complaint_investigation_Qual_Issue> changed, List<fo_complaint_investigation_Qual_Issue> added, List<fo_complaint_investigation_Qual_Issue> deleted)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var recordssaved = 0;
            //Performing insert operation
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    fo_complaint_investigation_Qual_Issue old = db.fo_complaint_investigation_Qual_Issue.
                        FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code
                        && o.investigation_code == temp.investigation_code
                        && o.batch_no == temp.batch_no
                        && o.product_code == temp.product_code);

                    if (old != null)
                    {
                        old.yes_no = temp.yes_no;
                        old.Details = temp.Details;
                        old.FileType = temp.FileType;
                        old.FileURL = temp.FileURL;
                        old.Required_Evidence = temp.Required_Evidence;
                        //db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_investigation_Qual_Issue.Add(temp);
                    }
                }
                recordssaved = db.SaveChanges();
            }

            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    fo_complaint_investigation_Qual_Issue old = db.fo_complaint_investigation_Qual_Issue.
                        FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code
                        && o.investigation_code == temp.investigation_code
                        && o.batch_no == temp.batch_no
                        && o.product_code == temp.product_code);

                    if (old != null)
                    {
                        old.yes_no = temp.yes_no;
                        old.Details = temp.Details;
                        old.FileType = temp.FileType;
                        old.FileURL = temp.FileURL;
                        old.Required_Evidence = temp.Required_Evidence;
                        //db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_investigation_Qual_Issue.Add(temp);
                    }
                }
                recordssaved += db.SaveChanges();
            }

            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    db.fo_complaint_investigation_Qual_Issue.Remove(db.fo_complaint_investigation_Qual_Issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code
                        && o.investigation_code == temp.investigation_code
                        && o.batch_no == temp.batch_no
                        && o.product_code == temp.product_code));
                }
                recordssaved += db.SaveChanges();
            }

            return (null);

            // return Json(new {msg = String.Format("{0}", recordssaved.ToString()+" Records Saved")}, JsonRequestBehavior.AllowGet);
        }



        public ActionResult BatchUpdateQualityIssues(string key, List<fo_complaint_quality_issue> changed, List<fo_complaint_quality_issue> added, List<fo_complaint_quality_issue> deleted)
        {
            var recordssaved = 0;
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                if (added != null /*&& added.Count() > 0*/)
                {
                    foreach (var temp in added)
                    {
                        //fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        //o.batch_no == temp.batch_no && o.product_code == temp.product_code);

                        fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_No == temp.e_reg_complaint_No && o.product_code == temp.product_code);
                        if (string.IsNullOrEmpty(temp.Id.ToString()) || temp.Id == 0)
                        {
                            var newCode = QualityIssueCode();
                            temp.Id = ++newCode;
                        }

                        if (old != null)
                        {
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            // db.Entry(old).State = EntityState.Modified; 
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            old.complainant_name = temp.complainant_name;
                            old.complainant_phone = temp.complainant_phone;
                            old.complainant_title = temp.complainant_title;
                            old.complainant_email = temp.complainant_email;
                            old.product_strength = temp.product_strength;
                            old.dosage_form = temp.dosage_form;
                            old.product_other = temp.product_other;
                            old.manufacturer = temp.manufacturer;
                            old.date_quality_issue_identified = temp.date_quality_issue_identified;
                            old.description_of_quality_issue = temp.description_of_quality_issue;
                            old.intervention_taken = temp.intervention_taken;
                            old.intervention_taken_other = temp.intervention_taken_other;
                            old.attached_picture = temp.attached_picture;
                            old.attached_email = temp.attached_email;
                            old.attached_note_letter = temp.attached_note_letter;
                            old.attached_other = temp.attached_other;
                            old.recipient_name = temp.recipient_name;
                            old.recipient_title = temp.recipient_title;
                            old.attached_image_picture = temp.attached_image_picture;
                            old.attached_image_email = temp.attached_image_email;
                            old.attached_image_letter = temp.attached_image_letter;
                            old.Quantity = temp.Quantity;
                            old.Supplier = temp.Supplier;
                            old.e_reg_track_date_of_action = temp.e_reg_track_date_of_action;
                            old.e_reg_track_action_category = temp.e_reg_track_action_category;
                            old.e_reg_track_action_details = temp.e_reg_track_action_details;
                            old.e_reg_track_status = temp.e_reg_track_status;
                            old.e_reg_track_maul_staff = temp.e_reg_track_maul_staff;
                            old.e_reg_track_action_taken = temp.e_reg_track_action_taken;
                            old.e_reg_Facility_action_taken = temp.e_reg_Facility_action_taken;
                            db.Entry(old).State = EntityState.Modified;

                        }
                        else
                        {
                            db.fo_complaint_quality_issue.Add(temp);
                        }
                    }
                    recordssaved = db.SaveChanges();
                }

                ////Performing update operation
                if (changed != null /*&& changed.Count() > 0*/)
                {
                    foreach (var temp in changed)
                    {
                        fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.Id == temp.Id);
                        if (string.IsNullOrEmpty(temp.Id.ToString()))
                        {
                            var newCode = QualityIssueCode();
                            temp.Id = ++newCode;
                        }

                        if (old != null)
                        {
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            // db.Entry(old).State = EntityState.Modified; 
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            //old.description_of_quality_issue = temp.description_of_quality_issue;
                            //old.date_quality_issue_identified = temp.date_quality_issue_identified;
                            //old.e_reg_track_date_of_action = temp.e_reg_track_date_of_action;
                            //old.e_reg_track_action_details = temp.e_reg_track_action_details; 
                            //old.attached_picture = temp.attached_picture;

                            old.complainant_name = temp.complainant_name;
                            old.complainant_phone = temp.complainant_phone;
                            old.complainant_title = temp.complainant_title;
                            old.complainant_email = temp.complainant_email;
                            old.product_strength = temp.product_strength;
                            old.dosage_form = temp.dosage_form;
                            old.product_other = temp.product_other;
                            old.manufacturer = temp.manufacturer;
                            old.date_quality_issue_identified = temp.date_quality_issue_identified;
                            old.description_of_quality_issue = temp.description_of_quality_issue;
                            old.intervention_taken = temp.intervention_taken;
                            old.intervention_taken_other = temp.intervention_taken_other;
                            old.attached_picture = temp.attached_picture;
                            old.attached_email = temp.attached_email;
                            old.attached_note_letter = temp.attached_note_letter;
                            old.attached_other = temp.attached_other;
                            old.recipient_name = temp.recipient_name;
                            old.recipient_title = temp.recipient_title;
                            old.attached_image_picture = temp.attached_image_picture;
                            old.attached_image_email = temp.attached_image_email;
                            old.attached_image_letter = temp.attached_image_letter;
                            old.Quantity = temp.Quantity;
                            old.Supplier = temp.Supplier;
                            old.e_reg_track_date_of_action = temp.e_reg_track_date_of_action;
                            old.e_reg_track_action_category = temp.e_reg_track_action_category;
                            old.e_reg_track_action_details = temp.e_reg_track_action_details;
                            old.e_reg_track_status = temp.e_reg_track_status;
                            old.e_reg_track_maul_staff = temp.e_reg_track_maul_staff;
                            old.e_reg_track_action_taken = temp.e_reg_track_action_taken;
                            old.e_reg_Facility_action_taken = temp.e_reg_Facility_action_taken;
                            db.Entry(old).State = EntityState.Modified;

                        }
                        else
                        {


                            db.fo_complaint_quality_issue.Add(temp);
                        }
                    }
                    recordssaved += db.SaveChanges();
                }

                //Performing delete operation
                if (deleted != null /*&& deleted.Count() > 0*/)
                {
                    foreach (var temp in deleted)
                    {
                        db.fo_complaint_quality_issue.Remove(db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        o.batch_no == temp.batch_no && o.product_code == temp.product_code));
                    }
                    recordssaved += db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["SaveError"] = ex.InnerException.Message;
                throw ex;
            }
            //finally
            //{

            //}

            return Content(recordssaved.ToString());

            //            return Json(new { msg = String.Format("{0}", recordssaved.ToString() + " Records Saved") }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BatchUpdateActionTaken(string key, List<fo_complaint_actiontaken_tracker> changed, List<fo_complaint_actiontaken_tracker> added, List<fo_complaint_actiontaken_tracker> deleted)
        {
            var recordssaved = 0;
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                if (added != null /*&& added.Count() > 0*/)
                {
                    foreach (var temp in added)
                    {
                        var userName = User.Identity.Name;
                        var name = db.AspNetUsers.FirstOrDefault(o=> o.UserName == userName).Name;
                        temp.AddedBy = name;
                        //fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        //o.batch_no == temp.batch_no && o.product_code == temp.product_code);

                        fo_complaint_actiontaken_tracker old = db.fo_complaint_actiontaken_tracker.FirstOrDefault(o => o.e_reg_complaint_No == temp.e_reg_complaint_No && o.Action_description == temp.Action_description);
                        if (string.IsNullOrEmpty(temp.Action_id.ToString()) || temp.Action_id == 0)
                        {
                            var newCode = ActionTakenCode();
                            temp.Action_id = ++newCode;
                        }

                        if (old != null)
                        {
                            db.Entry(old).CurrentValues.SetValues(temp);
                            db.Entry(old).State = EntityState.Modified;
                            // db.Entry(old).CurrentValues.SetValues(temp);

                            //db.Entry(old).State = EntityState.Modified;
                            //recordssaved = db.SaveChanges();

                        }
                        else
                        {
                            db.fo_complaint_actiontaken_tracker.Add(temp);

                        }
                        recordssaved = db.SaveChanges();
                    }

                }

                ////Performing update operation
                if (changed != null /*&& changed.Count() > 0*/)
                {
                    foreach (var temp in changed)
                    {
                        fo_complaint_actiontaken_tracker old = db.fo_complaint_actiontaken_tracker.FirstOrDefault(o => o.Action_id == temp.Action_id);
                        //if (string.IsNullOrEmpty(temp.Action_id.ToString()))
                        //{
                        //    var newCode = ActionTakenCode();
                        //    temp.Action_id = ++newCode;
                        //}

                        if (old != null)
                        {
                            db.Entry(old).CurrentValues.SetValues(temp);
                            db.Entry(old).State = EntityState.Modified;
                            // db.Entry(old).CurrentValues.SetValues(temp);
                            //db.Entry(old).State = EntityState.Modified;

                        }
                        else
                        {


                            //db.fo_complaint_actiontaken_tracker.Add(temp);
                        }
                        recordssaved += db.SaveChanges();
                    }

                }

                //Performing delete operation
                if (deleted != null /*&& deleted.Count() > 0*/)
                {
                    foreach (var temp in deleted)
                    {
                        db.fo_complaint_actiontaken_tracker.Remove(db.fo_complaint_actiontaken_tracker.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                        o.Action_id == temp.Action_id));
                    }
                    recordssaved += db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["SaveError"] = ex.InnerException.Message;
                throw ex;
            }
            //finally
            //{

            //}

            return Content(recordssaved.ToString());

            //            return Json(new { msg = String.Format("{0}", recordssaved.ToString() + " Records Saved") }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BatchUpdateComplaint_Tracker(string key, List<fo_complaint_tracker> changed, List<fo_complaint_tracker> added, List<fo_complaint_tracker> deleted)
        {
            var recordssaved = 0;
            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                if (added != null && added.Count() > 0)
                {
                    foreach (var temp in added)
                    {
                        fo_complaint_tracker old = db.fo_complaint_tracker.FirstOrDefault(o => o.e_reg_track_code == temp.e_reg_track_code);

                        if (old != null)
                        {
                            db.Entry(old).CurrentValues.SetValues(temp);
                            db.Entry(old).CurrentValues.SetValues(temp);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(temp.e_reg_track_code.ToString()))
                            {
                                var newCode = complaintTrackCode();
                                temp.e_reg_track_code = newCode++;
                            }
                            db.fo_complaint_tracker.Add(temp);
                        }
                    }
                    recordssaved = db.SaveChanges();
                }

                ////Performing update operation
                if (changed != null && changed.Count() > 0)
                {
                    foreach (var temp in changed)
                    {
                        fo_complaint_tracker old = db.fo_complaint_tracker.FirstOrDefault(o => o.e_reg_track_code == temp.e_reg_track_code);
                        if (old != null)
                        {
                            db.Entry(old).CurrentValues.SetValues(temp);
                            // db.Entry(old).State = EntityState.Modified;

                            db.Entry(old).CurrentValues.SetValues(temp);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(temp.e_reg_track_code.ToString()))
                            {
                                var newCode = complaintTrackCode();
                                temp.e_reg_track_code = newCode++;
                            }
                            db.fo_complaint_tracker.Add(temp);
                        }
                    }
                    recordssaved += db.SaveChanges();
                }

                //Performing delete operation
                if (deleted != null && deleted.Count() > 0)
                {
                    foreach (var temp in deleted)
                    {
                        db.fo_complaint_tracker.Remove(db.fo_complaint_tracker.FirstOrDefault(o => o.e_reg_track_code == temp.e_reg_track_code));
                    }
                    recordssaved += db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{

            //}
            return (null);

            //            return Json(new { msg = String.Format("{0}", recordssaved.ToString() + " Records Saved") }, JsonRequestBehavior.AllowGet);

        }



        public ActionResult BatchUpdateActionsTaken(string key, List<fo_complaint_tracker> changed, List<fo_complaint_tracker> added, List<fo_complaint_tracker> deleted)
        {
            var recordssaved = 0;
            //try
            //{

            db.Configuration.ProxyCreationEnabled = false;
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    fo_complaint_tracker old = db.fo_complaint_tracker.FirstOrDefault(o => o.e_reg_track_code == temp.e_reg_track_code);

                    if (old != null)
                    {
                        db.Entry(old).CurrentValues.SetValues(temp);
                        // db.Entry(old).State = EntityState.Modified;

                        db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_tracker.Add(temp);
                    }
                }
                recordssaved = db.SaveChanges();
            }

            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    fo_complaint_tracker old = db.fo_complaint_tracker.FirstOrDefault(o => o.e_reg_track_code == temp.e_reg_track_code);


                    if (old != null)
                    {
                        db.Entry(old).CurrentValues.SetValues(temp);
                        // db.Entry(old).State = EntityState.Modified;

                        db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_tracker.Add(temp);
                    }
                }
                recordssaved += db.SaveChanges();
            }

            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    db.fo_complaint_tracker.Remove(db.fo_complaint_tracker.FirstOrDefault(o => o.e_reg_track_code == temp.e_reg_track_code));
                }
                recordssaved += db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            ////finally
            ////{

            ////}


            return (null);

            //return Json(new { msg = String.Format("{0}", recordssaved.ToString() + " Records Saved") }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BatchUpdateQualityOtherProducts(string key, List<fo_complaint_quality_issue> changed, List<fo_complaint_quality_issue> added, List<fo_complaint_quality_issue> deleted)
        {
            var recordssaved = 0;
            //try
            //{
            db.Configuration.ProxyCreationEnabled = false;
            if (added != null && added.Count() > 0)
            {
                foreach (var temp in added)
                {
                    fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code
                    //&&
                    //o.batch_no == temp.batch_no && o.product_code == temp.product_code
                    );

                    if (old != null)
                    {
                        db.Entry(old).CurrentValues.SetValues(temp);
                        db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_quality_issue.Add(temp);
                    }
                }
                recordssaved += db.SaveChanges();
            }

            ////Performing update operation
            if (changed != null && changed.Count() > 0)
            {
                foreach (var temp in changed)
                {
                    fo_complaint_quality_issue old = db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                    o.batch_no == temp.batch_no && o.product_code == temp.product_code);

                    if (old != null)
                    {
                        db.Entry(old).CurrentValues.SetValues(temp);
                        // db.Entry(old).State = EntityState.Modified;

                        db.Entry(old).CurrentValues.SetValues(temp);
                    }
                    else
                    {
                        db.fo_complaint_quality_issue.Add(temp);
                    }
                }
                recordssaved += db.SaveChanges();
            }

            //Performing delete operation
            if (deleted != null && deleted.Count() > 0)
            {
                foreach (var temp in deleted)
                {
                    db.fo_complaint_quality_issue.Remove(db.fo_complaint_quality_issue.FirstOrDefault(o => o.e_reg_complaint_code == temp.e_reg_complaint_code &&
                    o.batch_no == temp.batch_no && o.product_code == temp.product_code));
                }
                recordssaved += db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            ////finally
            ////{

            ////}


            return (null);

            //return Json(new { msg = String.Format("{0}", recordssaved.ToString() + " Records Saved") }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveSupportDoc(IEnumerable<HttpPostedFileBase> UploadInput, string UploadInput_data)
        {
            try
            {
                foreach (var file in UploadInput)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var uploadedFilePath = "~/" + ConfigurationManager.AppSettings["UploadedFilesDirectory"];
                    //var destinationPath = Path.Combine(Server.MapPath("~/Upload_Files"), fileName);
                    var destinationPath = Path.Combine(Server.MapPath(uploadedFilePath), fileName);
                    file.SaveAs(destinationPath);
                    string filepathtosave = ConfigurationManager.AppSettings["UploadedFilesDirectory"] + "/" + fileName;
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);

                    //Strip the string of non-numeric characters
                    var regcode = GetNumbers(UploadInput_data);

                    //Save infor into the DB
                    FileCollection objFileCollection = new FileCollection();
                    objFileCollection.Id = Guid.NewGuid().ToString();
                    objFileCollection.Details = file.FileName;
                    objFileCollection.FileURL = filepathtosave;
                    objFileCollection.FileType = fileExtention;
                    objFileCollection.e_reg_complaint_code = System.Convert.ToInt32(regcode);

                    db.FileCollections.Add(objFileCollection);
                    /*Save data to database*/
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Json(UploadInput_data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _SaveDefault(IEnumerable<HttpPostedFileBase> UploadDefault)
        {
           // string AdditionParam = HttpContext.Request.Form["UploadDefault_data"];// data is received here 
            //foreach (var file in UploadDefault)
            //{
            //    var fileName = Path.GetFileName(file.FileName);
            //    var destinationPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
            //    file.SaveAs(destinationPath);
            //}
            try
            {
                foreach (var file in UploadDefault)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var uploadedFilePath = "~/" + ConfigurationManager.AppSettings["UploadedFilesDirectory"];
                    //var destinationPath = Path.Combine(Server.MapPath("~/Upload_Files"), fileName);
                    var destinationPath = Path.Combine(Server.MapPath(uploadedFilePath), fileName);
                    file.SaveAs(destinationPath);
                    string filepathtosave = ConfigurationManager.AppSettings["UploadedFilesDirectory"] + "/" + fileName;
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);
                    string AdditionParam = HttpContext.Request.Form["UploadDefault_data"];// data is received here
                    var details = AdditionParam.Split(',');
                    var _regcode = GetNumbers(details[0]);
                    var regcode = 0;
                    int output = 0;
                    if(Int32.TryParse(_regcode, out output))
                    {
                        regcode = output;
                    }
                    var compNo = details[1].Replace("\"", "").Trim();
                    //Strip the string of non-numeric characters
                    //var regcode = GetNumbers(UploadInput_data);

                    //Save infor into the DB
                    FileCollection objFileCollection = new FileCollection();
                    objFileCollection.Id = Guid.NewGuid().ToString();
                    objFileCollection.Details = file.FileName;
                    objFileCollection.FileURL = filepathtosave;
                    objFileCollection.FileType = fileExtention;
                    objFileCollection.e_reg_complaint_code = System.Convert.ToInt32(regcode);
                    objFileCollection.e_reg_complaint_No = compNo;
                    if(regcode > 0)
                    {
                        db.FileCollections.Add(objFileCollection);
                        /*Save data to database*/
                        db.SaveChanges();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Content("");
        }

        public ActionResult SaveSupportingDoc(IEnumerable<HttpPostedFileBase> UploadSupFile, string complaintNo, string regcode, string batch_no, string product_code, int product_comp_code, string Details)
        {
            //Save infor into the DB
            FileCollectionSupDoc objFileCollection = new FileCollectionSupDoc();

            try
            {
                foreach (var file in UploadSupFile)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploadedFilePath = "~/" + ConfigurationManager.AppSettings["UploadedFilesDirectory"];
                    //var destinationPath = Path.Combine(Server.MapPath("~/Upload_Files"), fileName);
                    var destinationPath = Path.Combine(Server.MapPath(uploadedFilePath), fileName);
                    file.SaveAs(destinationPath);
                    string filepathtosave = ConfigurationManager.AppSettings["UploadedFilesDirectory"] + "/" + fileName;
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);

                    //Strip the string of non-numeric characters
                    var Regcode = GetNumbers(regcode);
                    objFileCollection.Id = Guid.NewGuid().ToString();
                    objFileCollection.Details = fileName;
                    objFileCollection.FileURL = filepathtosave;
                    objFileCollection.FileType = fileExtention;
                    objFileCollection.e_reg_complaint_code = System.Convert.ToInt32(Regcode);
                    objFileCollection.product_code = System.Convert.ToInt32(product_code);
                    objFileCollection.batch_no = batch_no;
                    objFileCollection.e_reg_complaint_No = complaintNo;
                    objFileCollection.QualityIssue_code = product_comp_code;// Regcode + "-" + batch_no + "-" + product_code;

                    db.FileCollectionSupDocs.Add(objFileCollection);
                    /*Save data to database*/
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return Json(objFileCollection, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DialogInsertSupDoc(FileCollectionSupDoc value, string compcode, string testStr)
        {
            int number;
            bool success = Int32.TryParse(compcode, out number);
            if (success)
            {
                db.Configuration.ProxyCreationEnabled = false;
                FileCollectionSupDoc table = db.FileCollectionSupDocs.FirstOrDefault(o => o.Id == value.Id);
                value.e_reg_complaint_code = number;
                if (table == null)
                {
                    //try
                    //{
                    //    db.FileCollectionSupDocs.Add(value);
                    //    db.SaveChanges();
                    //}
                    //catch (DbEntityValidationException dbEx)
                    //{
                    //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //    {
                    //        foreach (var validationError in validationErrors.ValidationErrors)
                    //        {
                    //            TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    //            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    //        }
                    //    }
                    //}
                }
                else
                {
                    try
                    {
                        db.Entry(table).CurrentValues.SetValues(value);
                        db.Entry(table).State = EntityState.Modified;


                        db.SaveChanges();
                        TempData["Success"] = "Record Saved Successfully!";
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                                System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }

                }
                //Console.WriteLine("Converted '{0}' to {1}.", compcode, number);
            }
            else
            {
                return Json(value, JsonRequestBehavior.AllowGet);
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ADDComplaintDialogDelete(int? key)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var value = db.fo_complaint.FirstOrDefault(f => f.e_reg_complaint_code == key);
            if(value != null)
            {
                value.DeletedRecord = true;
                db.Entry(value).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ADDComplaint");
        }
        public ActionResult ComplaintDialogDelete(int? key)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var value = db.fo_complaint.FirstOrDefault(f => f.e_reg_complaint_code == key);
            if (value != null)
            {
                value.DeletedRecord = true;
                db.Entry(value).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("CCU");
        }
        public ActionResult DialogDelete(int? key)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var value = db.fo_complaint_quality_issue.FirstOrDefault(f => f.Id == key);
            if (key == null)
            {
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            var recordssaved = 0;
            try
            {

                //Delete the record from the database
                db.fo_complaint_quality_issue.Remove(db.fo_complaint_quality_issue.FirstOrDefault(f => f.Id == key));
                recordssaved += db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogDeleteSupDoc(string key, FileCollectionSupDoc value/*, string compcode*/)
        {
            var recordssaved = 0;
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var filedetails = db.FileCollectionSupDocs.FirstOrDefault(f => f.Id == key);

                //Delete the record from the database
                db.FileCollectionSupDocs.Remove(db.FileCollectionSupDocs.FirstOrDefault(o => o.Id == key));
                recordssaved += db.SaveChanges();

                //Delete the physical file  from the folder
                var physicalPath = string.Empty;
                if (filedetails != null)
                {
                    value = filedetails;

                    physicalPath = Path.Combine(Server.MapPath("~/"), filedetails.FileURL);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        TempData["Success"] += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SaveSupportingDoc(IEnumerable<HttpPostedFileBase> UploadSupFile, string UploadInput_data, string FileDesc)
        //{
        //    //Save infor into the DB
        //    FileCollection objFileCollection = new FileCollection();
        //    try
        //    {
        //        foreach (var file in UploadSupFile)
        //        {
        //            var fileName = Path.GetFileName(file.FileName);

        //            var uploadedFilePath = "~/" + ConfigurationManager.AppSettings["UploadedFilesDirectory"];
        //            //var destinationPath = Path.Combine(Server.MapPath("~/Upload_Files"), fileName);
        //            var destinationPath = Path.Combine(Server.MapPath(uploadedFilePath), fileName);
        //            file.SaveAs(destinationPath);
        //            string filepathtosave = ConfigurationManager.AppSettings["UploadedFilesDirectory"] + "/" + fileName;
        //            string fileExtention = System.IO.Path.GetExtension(file.FileName);

        //            //Strip the string of non-numeric characters
        //            var regcode = GetNumbers(UploadInput_data);


        //            objFileCollection.Id = Guid.NewGuid().ToString();
        //            objFileCollection.Details = file.FileName + "Dora Testing in the wind!";
        //            objFileCollection.FileURL = filepathtosave;
        //            objFileCollection.FileType = fileExtention;
        //            objFileCollection.e_reg_complaint_code = System.Convert.ToInt32(regcode);

        //            db.FileCollections.Add(objFileCollection);
        //            /*Save data to database*/
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return Json(objFileCollection, JsonRequestBehavior.AllowGet);
        //}

        public class DataResult
        {
            public IEnumerable result { get; set; }
            public int count { get; set; }
        }



        #region Save Edvidence

        public void SaveDefault(IEnumerable<HttpPostedFileBase> EmployeeGridPhoto)
        {
            foreach (var file in EmployeeGridPhoto)
            {
                var fileName = Path.GetFileName(file.FileName);
                var destinationPath = Path.Combine(Server.MapPath("~/Content/ejThemes/images/Employees"), fileName);
                file.SaveAs(destinationPath);
            }
        }
        public void RemoveDefault(string[] fileNames)
        {
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/Content/ejThemes/images/Employees"), fileName);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
        }
        #endregion Save Edvidence

        private static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        #region File Uploads

        #region doc to PDF
        public ActionResult DOCtoPDF(string button, string renderingMode, HttpPostedFileBase file)
        {
            if (button == null)
                return View();
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension == ".doc" || extension == ".docx" || extension == ".dot" || extension == ".dotx" || extension == ".dotm" || extension == ".docm"
                   || extension == ".xml" || extension == ".rtf")
                {
                    WordDocument document = new WordDocument(file.InputStream);

                    //Initialize chart to image converter for converting charts in Word to PDF conversion
                    document.ChartToImageConverter = new ChartToImageConverter();
                    document.ChartToImageConverter.ScalingMode = Syncfusion.OfficeChart.ScalingMode.Normal;

                    DocToPDFConverter converter = new DocToPDFConverter();
                    if (renderingMode == "DirectPDF")
                        converter.Settings.EnableFastRendering = true;
                    //Convert word document into PDF document
                    PdfDocument pdfDoc = converter.ConvertToPDF(document);
                    try
                    {
                        return pdfDoc.ExportAsActionResult("sample.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
                    }
                    catch (Exception)
                    { }
                    finally
                    {

                    }
                }
                else
                {
                    ViewBag.Message = string.Format("Please choose Word format document to convert to PDF");
                }
            }
            else
            {
                ViewBag.Message = string.Format("Browse a Word document and then click the button to convert as a PDF document");
            }

            return View();
        }

        #endregion doc to PDF
        // GET: FileUpload
        public ActionResult FileUploads(int? e_reg_complaint_code)
        {
            //if(e_reg_complaint_code== null)
            //{

            //}
            FileListModel objfilelistmodel = new FileListModel();
            objfilelistmodel.FileListCollction = new List<FileList>();
            objfilelistmodel.FileListCollction = GetUploadedFileList(e_reg_complaint_code);
            return View(objfilelistmodel);
            //return View();
        }


        public ActionResult fileUploadedEvidence(int? e_reg_complaint_code)
        {

            FileListModel objfilelistmodel = new FileListModel();
            objfilelistmodel.FileListCollction = new List<FileList>();
            objfilelistmodel.FileListCollction = GetUploadedFileList(e_reg_complaint_code);
            return View(objfilelistmodel);
            // return View();
        }

        public ActionResult ImageGallery()
        {
            var imagesModel = new ImageGallery();
            var imageFiles = Directory.GetFiles(Server.MapPath("~/Upload_Files/"));
            foreach (var item in imageFiles)
            {
                imagesModel.ImageList.Add(Path.GetFileName(item));
            }
            return View(imagesModel);
        }


        public ActionResult ImageGalleryX(int? e_reg_complaint_code)
        {
            FileListModel objfilelistmodel = new FileListModel();
            objfilelistmodel.FileListCollction = new List<FileList>();
            objfilelistmodel.FileListCollction = GetUploadedFileList(e_reg_complaint_code);
            return View(objfilelistmodel);
        }

        public ActionResult loadfiles(int? doc_code)
        {
            FileListModel objfilelistmodel = new FileListModel();
            objfilelistmodel.FileListCollction = new List<FileList>();
            objfilelistmodel.FileListCollction = GetUploadedFileList(doc_code);
            //return View(objfilelistmodel);
            return Json(objfilelistmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadEvidence(int? e_reg_complaint_code)
        {
            FileListModel objfilelistmodel = new FileListModel();
            objfilelistmodel.FileListCollction = new List<FileList>();
            objfilelistmodel.FileListCollction = GetUploadedFileList(e_reg_complaint_code);
            return View(objfilelistmodel);
        }

        public ActionResult DownloadFile(string fileNamePath)
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "Images/";
            string path = AppDomain.CurrentDomain.BaseDirectory + @"/";
            //byte[] fileBytes = System.IO.File.ReadAllBytes(path + "filename.extension");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + fileNamePath);
            //string fileName = "filename.extension";
            string fileName = fileNamePath;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult fancyBox()
        {
            var imagesModel = new ImageGallery();
            var imageFiles = Directory.GetFiles(Server.MapPath("~/Upload_Files/"));
            foreach (var item in imageFiles)
            {
                imagesModel.ImageList.Add(Path.GetFileName(item));
            }
            return View(imagesModel);
        }
        /// <summary>
        /// Post method for uploading files
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FileUploads(HttpPostedFileBase file, FileListModel objfilelistmodel)
        {
            try
            {
                /*Geting the file name*/
                string filename = System.IO.Path.GetFileName(file.FileName);
                string fileExtention = System.IO.Path.GetExtension(file.FileName);
                /*Saving the file in server folder*/
                file.SaveAs(Server.MapPath("~/Images/" + filename));
                string filepathtosave = "Images/" + filename;
                /*HERE WILL BE YOUR CODE TO SAVE THE FILE DETAIL IN DATA BASE */
                FileList _objfilelist = new FileList();
                _objfilelist.Id = Guid.NewGuid().ToString();
                _objfilelist.FileURL = filepathtosave;
                _objfilelist.Detail = objfilelistmodel.FileListDetail.Detail;
                _objfilelist.FileType = fileExtention;
                Save(_objfilelist);
                /*Retrived Data*/
                List<FileList> objfilelist = new List<FileList>();
                objfilelist = GetUploadedFileList(null);
                objfilelistmodel.FileListCollction = objfilelist;

                ViewBag.Message = "File Uploaded successfully.";
            }
            catch
            {
                ViewBag.Message = "Error while uploading the files.";
            }
            return View(objfilelistmodel);
        }
        public int Save(FileList _objfilelist)
        {


            mascisEntities objnorthwind = new mascisEntities();
            FileCollection objFileCollection = new FileCollection();
            objFileCollection.Id = _objfilelist.Id;
            objFileCollection.Details = _objfilelist.Detail;
            objFileCollection.FileURL = _objfilelist.FileURL;
            objFileCollection.FileType = _objfilelist.FileType;
            objFileCollection.e_reg_complaint_code = _objfilelist.e_reg_complaint_code;

            objnorthwind.FileCollections.Add(objFileCollection);
            /*Save data to database*/
            objnorthwind.SaveChanges();
            return 1;
        }

        public ActionResult SaveDefault(IEnumerable<HttpPostedFileBase> uploadbox1, string uploadbox1_data)
        {

            foreach (var file in uploadbox1)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(file.FileName);

                var destinationPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                file.SaveAs(destinationPath);
            }
            ViewData["data"] = uploadbox1_data;
            return Content(uploadbox1_data);
        }

        public int saveEvidence(HttpPostedFileBase file, string fileDetails, int e_reg_complaint_code)
        {
            string filename = System.IO.Path.GetFileName(file.FileName);
            string fileExtention = System.IO.Path.GetExtension(file.FileName);
            /*Saving the file in server folder*/
            file.SaveAs(Server.MapPath("~/Images/" + filename));
            string filepathtosave = "Images/" + filename;
            /*HERE WILL BE YOUR CODE TO SAVE THE FILE DETAIL IN DATA BASE */
            FileList _objfilelist = new FileList();
            _objfilelist.Id = Guid.NewGuid().ToString();
            _objfilelist.FileURL = filepathtosave;
            _objfilelist.Detail = fileDetails;//  objfilelistmodel.FileListDetail.Detail;
            _objfilelist.FileType = fileExtention;
            Save(_objfilelist);

            mascisEntities objnorthwind = new mascisEntities();
            FileCollection objFileCollection = new FileCollection();
            objFileCollection.Id = _objfilelist.Id;
            objFileCollection.Details = _objfilelist.Detail;
            objFileCollection.FileURL = _objfilelist.FileURL;
            objFileCollection.FileType = _objfilelist.FileType;
            objFileCollection.e_reg_complaint_code = _objfilelist.e_reg_complaint_code;

            objnorthwind.FileCollections.Add(objFileCollection);
            /*Save data to database*/
            objnorthwind.SaveChanges();
            return 1;
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Import(ImportRequest importRequest)
        {
            if (importRequest.File == null)
            {
                //importRequest.Url = Request.Url.ToString().Replace(Request.Url.LocalPath, "") + "/Images/" + importRequest.Url;
                importRequest.Url = Request.Url.ToString().Replace(Request.Url.LocalPath, "") + "/" + importRequest.Url;
            }

            return importRequest.SpreadsheetActions();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public void ExportToExcel(string sheetModel, string sheetData, string password)
        {
            if (String.IsNullOrEmpty(password))
                Spreadsheet.Save(sheetModel, sheetData, "sample", ExportFormat.XLSX, ExcelVersion.Excel2013);
            else
                Spreadsheet.Save(sheetModel, sheetData, "sample", ExportFormat.XLSX, ExcelVersion.Excel2013, password);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public void ExportToCsv(string sheetModel, string sheetData)
        {
            Spreadsheet.Save(sheetModel, sheetData, "sample", ExportFormat.CSV);
        }


        public ActionResult GetFile(string filename = null)
        {
            if (filename != null)
            {
                FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                ViewBag.Type = Session["Type"];
                return File(fileStream, Session["Type"]);
            }
            else
            {
                filename = @"C:\\Software Development\\InfoTronics\\MASCIS\\mascis_10-07-2018\\mascis\\mascis\\Images\\Subdivision Wakabi - Annotated.pdf";
                var myfile = System.Web.HttpContext.Current.Server.MapPath(@"/Images/MemoAnnualLeave2018.docx");
                FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                ViewBag.Type = Session["Type"];
                return File(fileStream, "application / octet - stream");
            }
            //  return View("NotFound");
        }

        private ActionResult File(FileStream fileStream, object v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function will return the Data from DB 
        /// </summary>
        /// <param name="UploadFilePath"></param>
        /// <param name="Fileextention"></param>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public List<FileList> GetUploadedFileList(int? e_reg_complaint_code)
        {
            //string filename=item.FileURL

            List<FileList> objfilelist = new List<FileList>();
            mascisEntities objmascis = new mascisEntities();

            //byte[] content/* = your_byte[]*/;
            //var fullPathToFile = @"Some\Path\To\file.pdf";
            //var mimeType = "application/pdf"; 

            /*FileStream xfileStream = new FileStream(Session["Path"].ToString(), FileMode.Open, FileAccess.Read)*/
            ;
            var datacollection = objmascis.FileCollections.ToList();           // var datacollection = from data in objmascis.FileCollections/*. Where(f=>f.e_reg_complaint_code==e_reg_complaint_code)*/

            // select data;
            foreach (var item in datacollection)
            {
                if (item.e_reg_complaint_code == e_reg_complaint_code)
                {
                    objfilelist.Add(new FileList { FileURL = item.FileURL, FileType = item.FileType, Detail = item.Details, e_reg_complaint_code = item.e_reg_complaint_code });
                    ///objfilelist.Add(new FileList { FileURL = @"C:\Software Development\InfoTronics\MASCIS\mascis_15_10_2018\mascis\mascis\"+item.FileURL, FileType = item.FileType, Detail = item.Details, e_reg_complaint_code = item.e_reg_complaint_code });
                }

            }
            return objfilelist;
        }

        public FileContentResult File(string myfile)
        {
            // var fullPathToFile = @"Some\Path\To\file.pdf";
            //  var mimeType = "application/pdf";
            var fileContents = System.IO.File.ReadAllBytes(myfile);

            return new FileContentResult(fileContents, "application/octet-stream");
        }
        #endregion File Uploads
        public ActionResult ListComplaints()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.e_reg_communication_mode = db.fo_a_communication_mode.ToList();// new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
            // ViewBag.Communication_mode = this.Getcommunication_modesJson();
            var categories = db.fo_a_complaint_category.ToList();
            //var list = JsonConvert.SerializeObject(categories, Formatting.None, new JsonSerializerSettings()
            //{ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore});

            var prod_cat = db.A_product_category.AsNoTracking().ToArray();
            ViewBag.Product_Category = prod_cat;
            var fo_complaint = db.fo_complaint.ToList();//.Include(f => f.fo_a_communication_mode).Include(f => f.fo_a_complaint_accuteness).Include(f => f.fo_a_complaint_category);
                                                        // return View(fo_complaint.ToList());
            ViewBag.e_reg_complaint_category = categories;// new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");

            return View();
        }
        public ActionResult LoadMedicEvidence()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var medfiles = db.FileCollectionSupDocs.ToList();
            ViewBag.MedFiles = medfiles;
            return View();
        }

        public ActionResult Get_Medicine_Files(DataManager dataManager, int? e_reg_complaint_code, string batchno, int? productcode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var medfiles = db.FileCollectionSupDocs.ToList();
            return Json(medfiles, JsonRequestBehavior.AllowGet);

        }

        // GET: /Controller/MyAction/{id}

        //public ActionResult MyAction(int id)
        //{
        //    var partialViewModel = new PartialViewModel();
        //    // TODO: Populate the model (viewmodel) here using the id

        //    return PartialView("_MyPartial", partialViewModel);
        //}

        public ActionResult GetComplaint_Issues(Syncfusion.JavaScript.DataManager dm/*, string compCode*/)
        {
            QualityIssuesModel obj = new QualityIssuesModel();
            IEnumerable data = obj.Quality_Issue(null, null, null);
            int count = obj.Quality_Issue(null, null, null).Count();

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
        public ActionResult GetComplaints(Syncfusion.JavaScript.DataManager dm/*, string compCode*/)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ComplaintsModels obj = new ComplaintsModels();
            IEnumerable data; /*= db.fo_complaint.ToList();*/// obj.GetComplaints(null);
            int count = 0; /*= db.fo_complaint.ToList().Count();*///obj.GetComplaints(null).Count();

            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                int? facility_id = new UserManagement().getCurrentuserFacility();
                List<fo_complaint> m = new List<fo_complaint>();
                var _data = db.View_fo_complaint_AffectedSites.Where(e => e.FacilityCode == facility_id.ToString() && e.FinalSubmission != 1 && e.DeletedRecord != true).OrderByDescending(o => o.e_reg_date_recieved).ToList();
                count = db.View_fo_complaint_AffectedSites.Where(e => e.FacilityCode == facility_id.ToString() && e.FinalSubmission != 1 && e.DeletedRecord != true).ToList().Count();
                foreach (var n in _data)
                {
                    fo_complaint x = new fo_complaint();
                    x.AffectedSites = n.AffectedSites;
                    x.Brief_Feedback_Desc = n.Brief_Feedback_Desc;
                    x.Communicated_By_Lev1 = n.Communicated_By_Lev1;
                    x.Communicated_By_Lev1Date = n.Communicated_By_Lev1Date;
                    x.Communicated_By_Lev1Title = n.Communicated_By_Lev1Title;
                    x.Communicated_By_Lev2 = n.Communicated_By_Lev2;
                    x.Communicated_By_Lev2Date = n.Communicated_By_Lev2Date;
                    x.Communicated_By_Lev2Email = n.Communicated_By_Lev2Email;
                    x.Communicated_By_Lev2Title = n.Communicated_By_Lev2Title;
                    x.ComplainantDistrict = n.ComplainantDistrict;
                    x.ComplainantEmail = n.ComplainantEmail;
                    x.ComplainantFacilityCode = n.ComplainantFacilityCode;
                    x.ComplainantMobile = n.ComplainantMobile;
                    x.ComplainantName = n.ComplainantName;
                    x.ComplainantPhone = n.ComplainantPhone;
                    x.ComplainantTitle = n.ComplainantTitle;
                    x.DeletedRecord = n.DeletedRecord;
                    x.Email_letter_attached = n.Email_letter_attached;
                    x.e_reg_affected_sites = n.e_reg_affected_sites;
                    x.e_reg_communication_mode = n.e_reg_communication_mode;
                    x.e_reg_complaint_accuteness = n.e_reg_complaint_accuteness;
                    x.e_reg_complaint_category = n.e_reg_complaint_category;
                    x.e_reg_complaint_code = n.e_reg_complaint_code;
                    x.e_reg_complaint_details = n.e_reg_complaint_details;
                    x.e_reg_complaint_No = n.e_reg_complaint_No;
                    x.e_reg_complaint_status = n.e_reg_complaint_status;
                    x.e_reg_complaint_Title = n.e_reg_complaint_Title;
                    x.e_reg_contact_person_id = n.e_reg_contact_person_id;
                    x.e_reg_date_complaint = n.e_reg_date_complaint;
                    x.e_reg_date_recieved = n.e_reg_date_recieved;
                    x.e_reg_date_resolved = n.e_reg_date_resolved;
                    x.e_reg_expected_date_resolution = n.e_reg_expected_date_resolution;
                    x.e_reg_MAUL_Staff = n.e_reg_MAUL_Staff;
                    x.e_reg_product_category = n.e_reg_product_category;
                    x.Feedback = n.Feedback;
                    x.Feedback_Communicated = n.Feedback_Communicated;
                    x.Feedback_Date = n.Feedback_Date;
                    x.IP = n.IP;
                    x.is_quality_issue = n.is_quality_issue;
                    x.No_Feedback_Reason = n.No_Feedback_Reason;
                    x.Prod_Samples_Provided = n.Prod_Samples_Provided;
                    x.Sup_Doc_Evidence_Rec = n.Sup_Doc_Evidence_Rec;
                    x.FinalSubmission = n.FinalSubmission;
                    x.ComplainantCategory = n.ComplainantCategory;
                    m.Add(x);
                }
                data = m;
            }
            else if (User.IsInRole("SCTO"))
            {
                var RFSOfacilities = new UserManagement().getUserFacilityList();
                var _data = db.View_fo_complaint_AffectedSites.Where(e => (RFSOfacilities.Any(p => e.FacilityCode.Trim() == p.ToString().Trim())) && e.FinalSubmission == 1 && e.DeletedRecord != true).OrderByDescending(o => o.e_reg_date_recieved).ToList();
                count = db.View_fo_complaint_AffectedSites.Where(e => (RFSOfacilities.Any(p => e.FacilityCode.Trim() == p.ToString().Trim())) && e.FinalSubmission == 1 && e.DeletedRecord != true).ToList().Count();
                List<fo_complaint> m = new List<fo_complaint>();
                foreach (var n in _data)
                {
                    fo_complaint x = new fo_complaint();
                    x.AffectedSites = n.AffectedSites;
                    x.Brief_Feedback_Desc = n.Brief_Feedback_Desc;
                    x.Communicated_By_Lev1 = n.Communicated_By_Lev1;
                    x.Communicated_By_Lev1Date = n.Communicated_By_Lev1Date;
                    x.Communicated_By_Lev1Title = n.Communicated_By_Lev1Title;
                    x.Communicated_By_Lev2 = n.Communicated_By_Lev2;
                    x.Communicated_By_Lev2Date = n.Communicated_By_Lev2Date;
                    x.Communicated_By_Lev2Email = n.Communicated_By_Lev2Email;
                    x.Communicated_By_Lev2Title = n.Communicated_By_Lev2Title;
                    x.ComplainantDistrict = n.ComplainantDistrict;
                    x.ComplainantEmail = n.ComplainantEmail;
                    x.ComplainantFacilityCode = n.ComplainantFacilityCode;
                    x.ComplainantMobile = n.ComplainantMobile;
                    x.ComplainantName = n.ComplainantName;
                    x.ComplainantPhone = n.ComplainantPhone;
                    x.ComplainantTitle = n.ComplainantTitle;
                    x.DeletedRecord = n.DeletedRecord;
                    x.Email_letter_attached = n.Email_letter_attached;
                    x.e_reg_affected_sites = n.e_reg_affected_sites;
                    x.e_reg_communication_mode = n.e_reg_communication_mode;
                    x.e_reg_complaint_accuteness = n.e_reg_complaint_accuteness;
                    x.e_reg_complaint_category = n.e_reg_complaint_category;
                    x.e_reg_complaint_code = n.e_reg_complaint_code;
                    x.e_reg_complaint_details = n.e_reg_complaint_details;
                    x.e_reg_complaint_No = n.e_reg_complaint_No;
                    x.e_reg_complaint_status = n.e_reg_complaint_status;
                    x.e_reg_complaint_Title = n.e_reg_complaint_Title;
                    x.e_reg_contact_person_id = n.e_reg_contact_person_id;
                    x.e_reg_date_complaint = n.e_reg_date_complaint;
                    x.e_reg_date_recieved = n.e_reg_date_recieved;
                    x.e_reg_date_resolved = n.e_reg_date_resolved;
                    x.e_reg_expected_date_resolution = n.e_reg_expected_date_resolution;
                    x.e_reg_MAUL_Staff = n.e_reg_MAUL_Staff;
                    x.e_reg_product_category = n.e_reg_product_category;
                    x.Feedback = n.Feedback;
                    x.Feedback_Communicated = n.Feedback_Communicated;
                    x.Feedback_Date = n.Feedback_Date;
                    x.IP = n.IP;
                    x.is_quality_issue = n.is_quality_issue;
                    x.No_Feedback_Reason = n.No_Feedback_Reason;
                    x.Prod_Samples_Provided = n.Prod_Samples_Provided;
                    x.Sup_Doc_Evidence_Rec = n.Sup_Doc_Evidence_Rec;
                    x.FinalSubmission = n.FinalSubmission;
                    x.ComplainantCategory = n.ComplainantCategory;
                    m.Add(x);
                }
                data = m;
            }
            else if (User.IsInRole("ComplaintHandling"))
            {
                var Username = User.Identity.Name;
                var person = db.AspNetUsers.FirstOrDefault(o => o.UserName == Username);
                var contact = db.fo_contact_person.FirstOrDefault(o => o.cp_name.Trim() == person.Name.Trim());
                data = db.fo_complaint.Where(o => o.FinalSubmission == 1 && o.DeletedRecord != true && (o.Level1_Assignment.Contains(contact.cp_code.ToString()) || o.Level2_Assignment.Contains(contact.cp_code.ToString()))).OrderByDescending(o => o.e_reg_date_recieved).ToList();
                count = db.fo_complaint.Where(o => o.FinalSubmission == 1 && o.DeletedRecord != true && (o.Level1_Assignment.Contains(contact.cp_code.ToString()) || o.Level2_Assignment.Contains(contact.cp_code.ToString()))).ToList().Count();
            }
            else
            {
                data = db.fo_complaint.Where(o => o.FinalSubmission == 1 && o.DeletedRecord != true).OrderByDescending(o => o.e_reg_date_recieved).ToList();
                count = db.fo_complaint.Where(o => o.FinalSubmission == 1 && o.DeletedRecord != true).ToList().Count();
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
        public ActionResult TrackComplaints(Syncfusion.JavaScript.DataManager dm/*, string compCode*/)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ComplaintsModels obj = new ComplaintsModels();
            IEnumerable data; /*= db.fo_complaint.ToList();*/// obj.GetComplaints(null);
            int count = 0; /*= db.fo_complaint.ToList().Count();*///obj.GetComplaints(null).Count();

            var muserrole = User.IsInRole("HSIPClient");
            if (muserrole)
            {
                int? facility_id = new UserManagement().getCurrentuserFacility();
                List<fo_complaint> m = new List<fo_complaint>();
                var _data = db.View_fo_complaint_AffectedSites.Where(e => e.FacilityCode == facility_id.ToString() && e.FinalSubmission == 1 && e.DeletedRecord != true).OrderByDescending(o => o.e_reg_date_recieved).ToList();
                count = db.View_fo_complaint_AffectedSites.Where(e => e.FacilityCode == facility_id.ToString() && e.FinalSubmission == 1 && e.DeletedRecord != true).ToList().Count();
                foreach (var n in _data)
                {
                    fo_complaint x = new fo_complaint();
                    x.AffectedSites = n.AffectedSites;
                    x.Brief_Feedback_Desc = n.Brief_Feedback_Desc;
                    x.Communicated_By_Lev1 = n.Communicated_By_Lev1;
                    x.Communicated_By_Lev1Date = n.Communicated_By_Lev1Date;
                    x.Communicated_By_Lev1Title = n.Communicated_By_Lev1Title;
                    x.Communicated_By_Lev2 = n.Communicated_By_Lev2;
                    x.Communicated_By_Lev2Date = n.Communicated_By_Lev2Date;
                    x.Communicated_By_Lev2Email = n.Communicated_By_Lev2Email;
                    x.Communicated_By_Lev2Title = n.Communicated_By_Lev2Title;
                    x.ComplainantDistrict = n.ComplainantDistrict;
                    x.ComplainantEmail = n.ComplainantEmail;
                    x.ComplainantFacilityCode = n.ComplainantFacilityCode;
                    x.ComplainantMobile = n.ComplainantMobile;
                    x.ComplainantName = n.ComplainantName;
                    x.ComplainantPhone = n.ComplainantPhone;
                    x.ComplainantTitle = n.ComplainantTitle;
                    x.DeletedRecord = n.DeletedRecord;
                    x.Email_letter_attached = n.Email_letter_attached;
                    x.e_reg_affected_sites = n.e_reg_affected_sites;
                    x.e_reg_communication_mode = n.e_reg_communication_mode;
                    x.e_reg_complaint_accuteness = n.e_reg_complaint_accuteness;
                    x.e_reg_complaint_category = n.e_reg_complaint_category;
                    x.e_reg_complaint_code = n.e_reg_complaint_code;
                    x.e_reg_complaint_details = n.e_reg_complaint_details;
                    x.e_reg_complaint_No = n.e_reg_complaint_No;
                    x.e_reg_complaint_status = n.e_reg_complaint_status;
                    x.e_reg_complaint_Title = n.e_reg_complaint_Title;
                    x.e_reg_contact_person_id = n.e_reg_contact_person_id;
                    x.e_reg_date_complaint = n.e_reg_date_complaint;
                    x.e_reg_date_recieved = n.e_reg_date_recieved;
                    x.e_reg_date_resolved = n.e_reg_date_resolved;
                    x.e_reg_expected_date_resolution = n.e_reg_expected_date_resolution;
                    x.e_reg_MAUL_Staff = n.e_reg_MAUL_Staff;
                    x.e_reg_product_category = n.e_reg_product_category;
                    x.Feedback = n.Feedback;
                    x.Feedback_Communicated = n.Feedback_Communicated;
                    x.Feedback_Date = n.Feedback_Date;
                    x.IP = n.IP;
                    x.is_quality_issue = n.is_quality_issue;
                    x.No_Feedback_Reason = n.No_Feedback_Reason;
                    x.Prod_Samples_Provided = n.Prod_Samples_Provided;
                    x.Sup_Doc_Evidence_Rec = n.Sup_Doc_Evidence_Rec;
                    x.FinalSubmission = n.FinalSubmission;
                    x.ComplainantCategory = n.ComplainantCategory;
                    m.Add(x);
                }
                data = m;
            }
            else if (User.IsInRole("SCTO"))
            {
                var RFSOfacilities = new UserManagement().getUserFacilityList();
                var _data = db.View_fo_complaint_AffectedSites.Where(e => (RFSOfacilities.Any(p => e.FacilityCode.Trim() == p.ToString().Trim())) && e.FinalSubmission == 1 && e.DeletedRecord != true).OrderByDescending(o => o.e_reg_date_recieved).ToList();
                count = db.View_fo_complaint_AffectedSites.Where(e => (RFSOfacilities.Any(p => e.FacilityCode.Trim() == p.ToString().Trim())) && e.FinalSubmission == 1 && e.DeletedRecord != true).ToList().Count();
                List<fo_complaint> m = new List<fo_complaint>();
                foreach (var n in _data)
                {
                    fo_complaint x = new fo_complaint();
                    x.AffectedSites = n.AffectedSites;
                    x.Brief_Feedback_Desc = n.Brief_Feedback_Desc;
                    x.Communicated_By_Lev1 = n.Communicated_By_Lev1;
                    x.Communicated_By_Lev1Date = n.Communicated_By_Lev1Date;
                    x.Communicated_By_Lev1Title = n.Communicated_By_Lev1Title;
                    x.Communicated_By_Lev2 = n.Communicated_By_Lev2;
                    x.Communicated_By_Lev2Date = n.Communicated_By_Lev2Date;
                    x.Communicated_By_Lev2Email = n.Communicated_By_Lev2Email;
                    x.Communicated_By_Lev2Title = n.Communicated_By_Lev2Title;
                    x.ComplainantDistrict = n.ComplainantDistrict;
                    x.ComplainantEmail = n.ComplainantEmail;
                    x.ComplainantFacilityCode = n.ComplainantFacilityCode;
                    x.ComplainantMobile = n.ComplainantMobile;
                    x.ComplainantName = n.ComplainantName;
                    x.ComplainantPhone = n.ComplainantPhone;
                    x.ComplainantTitle = n.ComplainantTitle;
                    x.DeletedRecord = n.DeletedRecord;
                    x.Email_letter_attached = n.Email_letter_attached;
                    x.e_reg_affected_sites = n.e_reg_affected_sites;
                    x.e_reg_communication_mode = n.e_reg_communication_mode;
                    x.e_reg_complaint_accuteness = n.e_reg_complaint_accuteness;
                    x.e_reg_complaint_category = n.e_reg_complaint_category;
                    x.e_reg_complaint_code = n.e_reg_complaint_code;
                    x.e_reg_complaint_details = n.e_reg_complaint_details;
                    x.e_reg_complaint_No = n.e_reg_complaint_No;
                    x.e_reg_complaint_status = n.e_reg_complaint_status;
                    x.e_reg_complaint_Title = n.e_reg_complaint_Title;
                    x.e_reg_contact_person_id = n.e_reg_contact_person_id;
                    x.e_reg_date_complaint = n.e_reg_date_complaint;
                    x.e_reg_date_recieved = n.e_reg_date_recieved;
                    x.e_reg_date_resolved = n.e_reg_date_resolved;
                    x.e_reg_expected_date_resolution = n.e_reg_expected_date_resolution;
                    x.e_reg_MAUL_Staff = n.e_reg_MAUL_Staff;
                    x.e_reg_product_category = n.e_reg_product_category;
                    x.Feedback = n.Feedback;
                    x.Feedback_Communicated = n.Feedback_Communicated;
                    x.Feedback_Date = n.Feedback_Date;
                    x.IP = n.IP;
                    x.is_quality_issue = n.is_quality_issue;
                    x.No_Feedback_Reason = n.No_Feedback_Reason;
                    x.Prod_Samples_Provided = n.Prod_Samples_Provided;
                    x.Sup_Doc_Evidence_Rec = n.Sup_Doc_Evidence_Rec;
                    x.FinalSubmission = n.FinalSubmission;
                    x.ComplainantCategory = n.ComplainantCategory;
                    m.Add(x);
                }
                data = m;
            }
            else
            {
                data = db.fo_complaint.Where(o => o.FinalSubmission == 1 && o.DeletedRecord != true).OrderByDescending(o => o.e_reg_date_recieved).ToList();
                count = db.fo_complaint.Where(o => o.FinalSubmission == 1 && o.DeletedRecord != true).ToList().Count();
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
        public ActionResult GetComplaints_QA(Syncfusion.JavaScript.DataManager dm/*, string compCode*/)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ComplaintsModels obj = new ComplaintsModels();

            IEnumerable data = db.fo_complaint.Where(o => o.FinalSubmission == 1 && (o.e_reg_complaint_status == 1 || o.e_reg_complaint_status == null) && o.e_reg_complaint_category == 1).OrderByDescending(o => o.e_reg_date_recieved).ToList();
            int count = db.fo_complaint.Where(o => o.FinalSubmission == 1 && (o.e_reg_complaint_status == 1 || o.e_reg_complaint_status == null) && o.e_reg_complaint_category == 1).ToList().Count();

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
        public ActionResult GetActionTracker(Syncfusion.JavaScript.DataManager dm, int? e_reg_complaint_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ComplaintsModels obj = new ComplaintsModels();
            IEnumerable data = db.fo_complaint_actiontaken_tracker.Where(o => o.e_reg_complaint_code == e_reg_complaint_code).ToList();// obj.GetComplaints(null);
            int count = db.fo_complaint_actiontaken_tracker.Where(o => o.e_reg_complaint_code == e_reg_complaint_code).ToList().Count();///obj.GetComplaints(null).Count();


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


        // GET: fo_complaint/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fo_complaint fo_complaint = db.fo_complaint.Find(id);
            if (fo_complaint == null)
            {
                return HttpNotFound();
            }
            return View(fo_complaint);
        }

        // GET: fo_complaint/Create
        public ActionResult Create()
        {
            ViewBag.e_reg_communication_mode = new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc");
            ViewBag.e_reg_complaint_accuteness = new SelectList(db.fo_a_complaint_accuteness, "accuteness_code", "accuteness_desc");
            ViewBag.e_reg_complaint_category = new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc");
            return View();
        }

        // POST: fo_complaint/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "e_reg_complaint_code,e_reg_complaint_No,e_reg_contact_person_id,e_reg_date_recieved,e_reg_date_complaint,e_reg_complaint_category,e_reg_complaint_details,e_reg_affected_sites,e_reg_product_category,e_reg_communication_mode,e_reg_date_resolved,e_reg_expected_date_resolution,e_reg_complaint_accuteness,DeletedRecord,is_quality_issue,AffectedSites,Sup_Doc_Evidence_Rec,Prod_Samples_Provided,Communicated_By_Lev1,Email_letter_attached,Communicated_By_Lev2,Feedback,Feedback_Communicated,Feedback_Date,No_Feedback_Reason")] fo_complaint fo_complaint)
        {
            if (ModelState.IsValid)
            {
                db.fo_complaint.Add(fo_complaint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.e_reg_communication_mode = new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc", fo_complaint.e_reg_communication_mode);
            ViewBag.e_reg_complaint_accuteness = new SelectList(db.fo_a_complaint_accuteness, "accuteness_code", "accuteness_desc", fo_complaint.e_reg_complaint_accuteness);
            ViewBag.e_reg_complaint_category = new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc", fo_complaint.e_reg_complaint_category);
            return View(fo_complaint);
        }

        // GET: fo_complaint/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fo_complaint fo_complaint = db.fo_complaint.Find(id);
            if (fo_complaint == null)
            {
                return HttpNotFound();
            }
            ViewBag.e_reg_communication_mode = new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc", fo_complaint.e_reg_communication_mode);
            ViewBag.e_reg_complaint_accuteness = new SelectList(db.fo_a_complaint_accuteness, "accuteness_code", "accuteness_desc", fo_complaint.e_reg_complaint_accuteness);
            ViewBag.e_reg_complaint_category = new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc", fo_complaint.e_reg_complaint_category);
            return View(fo_complaint);
        }

        // POST: fo_complaint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "e_reg_complaint_code,e_reg_complaint_No,e_reg_contact_person_id,e_reg_date_recieved,e_reg_date_complaint,e_reg_complaint_category,e_reg_complaint_details,e_reg_affected_sites,e_reg_product_category,e_reg_communication_mode,e_reg_date_resolved,e_reg_expected_date_resolution,e_reg_complaint_accuteness,DeletedRecord,is_quality_issue,AffectedSites,Sup_Doc_Evidence_Rec,Prod_Samples_Provided,Communicated_By_Lev1,Email_letter_attached,Communicated_By_Lev2,Feedback,Feedback_Communicated,Feedback_Date,No_Feedback_Reason")] fo_complaint fo_complaint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fo_complaint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.e_reg_communication_mode = new SelectList(db.fo_a_communication_mode, "comm_mode_code", "comm_mode_desc", fo_complaint.e_reg_communication_mode);
            ViewBag.e_reg_complaint_accuteness = new SelectList(db.fo_a_complaint_accuteness, "accuteness_code", "accuteness_desc", fo_complaint.e_reg_complaint_accuteness);
            ViewBag.e_reg_complaint_category = new SelectList(db.fo_a_complaint_category, "complaint_category_code", "complaint_category_desc", fo_complaint.e_reg_complaint_category);
            return View(fo_complaint);
        }

        // GET: fo_complaint/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fo_complaint fo_complaint = db.fo_complaint.Find(id);
            if (fo_complaint == null)
            {
                return HttpNotFound();
            }
            return View(fo_complaint);
        }

        // POST: fo_complaint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            fo_complaint fo_complaint = db.fo_complaint.Find(id);
            db.fo_complaint.Remove(fo_complaint);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private class ComplaintDetails{
            public int ComplaintCode { get; set; }
            public string ComplaintNo { get; set; }
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
    }
}
