using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using System.Data.Entity.Validation;
using System.Data.Entity.SqlServer;

namespace mascis.Controllers
{
    public class fo_ClientFeedbackController : Controller
    {
        private mascisEntities db = new mascisEntities();

        public ActionResult Example()
        {

            return View();
        }
        public ActionResult Test()
        {
            var login = db.AspNetUsers.FirstOrDefault(o => o.Email == User.Identity.Name);
            var facilities = db.A_Facilities.Where(o => o.FacilityCode == login.FacilityId).ToList();

            ViewData["Facilities"] = new SelectList(facilities, "FacilityCode", "Facility");
            IEnumerable<SelectListItem> items = facilities.Select(c => new SelectListItem
            {
                Value = c.FacilityCode.ToString(),
                Text = c.Facility
            });

            ViewData["Facilities"] = items;
            var contacts = db.fo_contact_person/*.Where(o => o.cp_facility == login.FacilityId)*/.AsNoTracking().ToList();
            ViewBag.Contacts = contacts;

            ViewData["Contact"] = new SelectList(contacts, "cp_code", "cp_name");
            IEnumerable<SelectListItem> _contact = contacts.Select(c => new SelectListItem
            {
                Value = c.cp_code.ToString(),
                Text = c.cp_name
            });

            ViewData["Contact"] = _contact;
            db.Configuration.ProxyCreationEnabled = false;
            var designations = db.fo_contact_title.AsNoTracking().ToList();
            ViewBag.Designations = designations;
            db.Configuration.ProxyCreationEnabled = false;
            var MAUL_service = db.fo_a_MAUL_Service.AsNoTracking().ToList();
            ViewBag.MAUL_Service = MAUL_service;
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                db.Configuration.ProxyCreationEnabled = false;
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }
            ViewBag.YesNo = yesno;
            var scto = db.fo_SCTO_ContactPerson.AsNoTracking().ToList();
            ViewBag.Scto = scto;
            fo_ClientFeedback feedback = new fo_ClientFeedback();
            return View(feedback);
        }
        // GET: fo_ClientFeedback
        public ActionResult Index()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_ClientFeedback = db.fo_ClientFeedback.Include(f => f.A_Facilities).Include(f => f.A_yes_no).Include(f => f.A_yes_no1).Include(f => f.fo_contact_title);
            return View(fo_ClientFeedback.ToList());
        }
        public ActionResult ClientFeedback()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                db.Configuration.ProxyCreationEnabled = false;
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }
            ViewBag.YesNo = yesno;

            //db.Configuration.ProxyCreationEnabled = false;
            //var facilities = db.A_Facilities.AsNoTracking().ToArray();
            //ViewBag.Facilities = facilities;
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
                db.Configuration.ProxyCreationEnabled = false;
                var contacts = db.fo_contact_person.Where(o=> o.cp_facility == login.FacilityId).AsNoTracking().ToList();
                ViewBag.Contacts = contacts;
                db.Configuration.ProxyCreationEnabled = false;
                var DataSource = db.fo_ClientFeedback.Where(o=> o.FacilityCode == login.FacilityId).AsNoTracking().ToList();
                ViewBag.datasource = DataSource;
            }
            else
            {
                ViewBag.A_ImplimentingPartners = db.A_ImplimentingPartners.AsNoTracking().ToList();
                var facilities = db.A_Facilities.AsNoTracking().ToList();
                ViewBag.Facilities = facilities;
                ViewBag.role = false;
                db.Configuration.ProxyCreationEnabled = false;
                var contacts = db.fo_contact_person.AsNoTracking().ToList();
                ViewBag.Contacts = contacts;
                db.Configuration.ProxyCreationEnabled = false;
                var DataSource = db.fo_ClientFeedback.AsNoTracking().ToList();
                ViewBag.datasource = DataSource;
            }

           

            db.Configuration.ProxyCreationEnabled = false;
            var designations = db.fo_contact_title .AsNoTracking().ToList();
            ViewBag.Designations = designations;

            db.Configuration.ProxyCreationEnabled = false;
            var MAUL_service = db.fo_a_MAUL_Service.AsNoTracking().ToList();
            ViewBag.MAUL_Service = MAUL_service;
            
            //var fo_ClientFeedback = db.fo_ClientFeedback.Include(f => f.A_Facilities).Include(f => f.A_yes_no).Include(f => f.A_yes_no1).Include(f => f.fo_contact_person).Include(f => f.fo_contact_title);
            return View(/*fo_ClientFeedback.ToList()*/);
        }
        public ActionResult ClientFeedbackHSIP()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                db.Configuration.ProxyCreationEnabled = false;
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }
            ViewBag.YesNo = yesno;

            //db.Configuration.ProxyCreationEnabled = false;
            //var facilities = db.A_Facilities.AsNoTracking().ToArray();
            //ViewBag.Facilities = facilities;
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
                db.Configuration.ProxyCreationEnabled = false;
                var contacts = db.fo_contact_person.Where(o => o.cp_facility == login.FacilityId).AsNoTracking().ToList();
                ViewBag.Contacts = contacts;
            }
            else
            {
                ViewBag.A_ImplimentingPartners = db.A_ImplimentingPartners.AsNoTracking().ToList();
                var facilities = db.A_Facilities.AsNoTracking().ToList();
                ViewBag.Facilities = facilities;
                ViewBag.role = false;
                db.Configuration.ProxyCreationEnabled = false;
                var contacts = db.fo_contact_person.AsNoTracking().ToList();
                ViewBag.Contacts = contacts;
            }



            db.Configuration.ProxyCreationEnabled = false;
            var designations = db.fo_contact_title.AsNoTracking().ToList();
            ViewBag.Designations = designations;

            db.Configuration.ProxyCreationEnabled = false;
            var MAUL_service = db.fo_a_MAUL_Service.AsNoTracking().ToList();
            ViewBag.MAUL_Service = MAUL_service;

            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.fo_ClientFeedback.AsNoTracking().ToList();
            ViewBag.datasource = DataSource;

            //var fo_ClientFeedback = db.fo_ClientFeedback.Include(f => f.A_Facilities).Include(f => f.A_yes_no).Include(f => f.A_yes_no1).Include(f => f.fo_contact_person).Include(f => f.fo_contact_title);
            return View(/*fo_ClientFeedback.ToList()*/);
        }
        public ActionResult GetContactTitle(int? cp_code)
        {

            int? data = 0; //db.fo_contact_title.ToList();
            var cp = db.fo_contact_person.FirstOrDefault(e => e.cp_code == cp_code);
            if (cp != null)
            {
                if(cp.cp_title != null)
                {
                    data = cp.cp_title;
                }
                //if (cp.cp_title == null)
                //{
                //    data = db.fo_contact_title.AsNoTracking().ToList();
                //}
                //else
                //{
                //    data = db.fo_contact_title.Where(e => e.title_code == cp.cp_title).AsNoTracking().ToList();
                //}
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTelephone(int? cp_code)
        {

            var data = new fo_contact_telephone() ;
            var cp = db.fo_contact_person.FirstOrDefault(e => e.cp_code == cp_code);
            if (cp != null)
            {
                data = db.fo_contact_telephone.FirstOrDefault(e => e.cp_code == cp.cp_code);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DialogInsert(fo_ClientFeedback value/*, string testdata, string testdata2, string testdata3*/)
        {
            var tdate = System.Convert.ToDateTime(value.FeedbackDate);

            var tday = tdate.Day;
            var tmoon = tdate.Month;
            var tyear = tdate.Year;

            var thour = tdate.Hour;
            var tmin = tdate.Minute;
            var tsec = tdate.Second;

            //db.Configuration.ProxyCreationEnabled = false;
            //var _feedback = db.fo_ClientFeedback.FirstOrDefault
            //  (t => SqlFunctions.DatePart("ss", t.FeedbackDate) == tsec &&
            //    SqlFunctions.DatePart("mi", t.FeedbackDate) == tmin &&
            //    SqlFunctions.DatePart("hh", t.FeedbackDate) == thour &&
            //    SqlFunctions.DatePart("dd", t.FeedbackDate) == tday &&
            //    SqlFunctions.DatePart("mm", t.FeedbackDate) == tmoon &&
            //    SqlFunctions.DatePart("yyyy", t.FeedbackDate) == tyear) ;
            var _feedback = db.fo_ClientFeedback.FirstOrDefault(o => o.ID == value.ID);

            db.Configuration.ProxyCreationEnabled = false;
            if (_feedback == null)
            {
                value.ID = (GetFeedbackCount() + 1);
                db.Configuration.ProxyCreationEnabled = false;
                db.fo_ClientFeedback.Add(value);
               // db.SaveChanges();
            }
            else
            {
                //db.Configuration.ProxyCreationEnabled = false;
                //_feedback.FacilityCode = value.FacilityCode;
                //_feedback.FeedbackDate = value.FeedbackDate;
                //_feedback.ContactPersonCode = value.ContactPersonCode;
                //_feedback.ContactPersonDesg = value.ContactPersonDesg;
                //_feedback.ContactPerson = value.ContactPerson;
                //_feedback.ContactPersonDesignation = value.ContactPersonDesignation;
                //_feedback.ContactPersonTele = value.ContactPersonTele;
                //_feedback.SCTO_visited = value.SCTO_visited;
                //_feedback.SCTOs_visited = value.SCTOs_visited;
                //_feedback.SCTO_LastVist = value.SCTO_LastVist;
                //_feedback.MAUL_Services = value.MAUL_Services;
                //_feedback.AnyAreas_of_Concern = value.AnyAreas_of_Concern;
                //_feedback.Areas_of_Concern = value.Areas_of_Concern;
                //_feedback.Better_Service = value.Better_Service;

                db.Entry(_feedback).CurrentValues.SetValues(value);
                db.Entry(_feedback).State = EntityState.Modified;
                
            }
            db.SaveChanges();
            var timestring = tsec.ToString()+"-"+ tmin.ToString()+ "-" +
                thour.ToString() + "-" + tday.ToString() + "-" +
                tmoon.ToString() + "-" + tyear.ToString();
            return Json(timestring, JsonRequestBehavior.AllowGet);
        }
        public int GetFeedbackCount()
        {
            int count = db.fo_ClientFeedback.ToList().Count;
            return count;
        }
        [HttpGet]
        public ActionResult contactTitle(int title_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = db.fo_contact_title.FirstOrDefault(c => c.title_code == title_code);
            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult contactPersons(int? cp_facility)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = db.fo_contact_person.Where(c => c.cp_facility  == cp_facility);
            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult contactPerson(int? cp_code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contactDetail = db.fo_contact_person.FirstOrDefault(c => c.cp_code == cp_code);
            return Json(contactDetail, JsonRequestBehavior.AllowGet);
        }

        // GET: fo_ClientFeedback/Details/5
        public ActionResult Details(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            fo_ClientFeedback fo_ClientFeedback = db.fo_ClientFeedback.Find(id);
            if (fo_ClientFeedback == null)
            {
                return HttpNotFound();
            }
            return View(fo_ClientFeedback);
        }

        // GET: fo_ClientFeedback/Create
        public ActionResult Create()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility");
            ViewBag.AnyAreas_of_Concern = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc");
            ViewBag.MAUL_Services = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc");
            ViewBag.ContactPersonCode = new SelectList(db.fo_contact_person, "cp_code", "cp_name");
            ViewBag.ContactPersonDesg = new SelectList(db.fo_contact_title, "title_code", "title_desc");
            return View();
        }

        // POST: fo_ClientFeedback/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FacilityCode,FeedbackDate,ContactPersonCode,ContactPersonDesg,ContactPersonTele,SCTO_visited,SCTOs_visited,SCTO_LastVist,MAUL_Services,AnyAreas_of_Concern,Areas_of_Concern,Better_Service")] fo_ClientFeedback fo_ClientFeedback)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (ModelState.IsValid)
            {
                db.fo_ClientFeedback.Add(fo_ClientFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", fo_ClientFeedback.FacilityCode);
            ViewBag.AnyAreas_of_Concern = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_ClientFeedback.AnyAreas_of_Concern);
            ViewBag.MAUL_Services = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_ClientFeedback.MAUL_Services);
            ViewBag.ContactPersonCode = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_ClientFeedback.ContactPersonCode);
            ViewBag.ContactPersonDesg = new SelectList(db.fo_contact_title, "title_code", "title_desc", fo_ClientFeedback.ContactPersonDesg);
            return View(fo_ClientFeedback);
        }

        // GET: fo_ClientFeedback/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            fo_ClientFeedback fo_ClientFeedback = db.fo_ClientFeedback.Find(id);
            if (fo_ClientFeedback == null)
            {
                return HttpNotFound();
            }
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", fo_ClientFeedback.FacilityCode);
            ViewBag.AnyAreas_of_Concern = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_ClientFeedback.AnyAreas_of_Concern);
            ViewBag.MAUL_Services = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_ClientFeedback.MAUL_Services);
            ViewBag.ContactPersonCode = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_ClientFeedback.ContactPersonCode);
            ViewBag.ContactPersonDesg = new SelectList(db.fo_contact_title, "title_code", "title_desc", fo_ClientFeedback.ContactPersonDesg);
            return View(fo_ClientFeedback);
        }

        // POST: fo_ClientFeedback/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FacilityCode,FeedbackDate,ContactPersonCode,ContactPersonDesg,ContactPersonTele,SCTO_visited,SCTOs_visited,SCTO_LastVist,MAUL_Services,AnyAreas_of_Concern,Areas_of_Concern,Better_Service")] fo_ClientFeedback fo_ClientFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Entry(fo_ClientFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.FacilityCode = new SelectList(db.A_Facilities, "FacilityCode", "Facility", fo_ClientFeedback.FacilityCode);
            ViewBag.AnyAreas_of_Concern = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_ClientFeedback.AnyAreas_of_Concern);
            ViewBag.MAUL_Services = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_ClientFeedback.MAUL_Services);
            ViewBag.ContactPersonCode = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_ClientFeedback.ContactPersonCode);
            ViewBag.ContactPersonDesg = new SelectList(db.fo_contact_title, "title_code", "title_desc", fo_ClientFeedback.ContactPersonDesg);
            return View(fo_ClientFeedback);
        }

        // GET: fo_ClientFeedback/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            fo_ClientFeedback fo_ClientFeedback = db.fo_ClientFeedback.Find(id);
            if (fo_ClientFeedback == null)
            {
                return HttpNotFound();
            }
            return View(fo_ClientFeedback);
        }

        // POST: fo_ClientFeedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            fo_ClientFeedback fo_ClientFeedback = db.fo_ClientFeedback.Find(id);
            db.fo_ClientFeedback.Remove(fo_ClientFeedback);
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
    }
}
