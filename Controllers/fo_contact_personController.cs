using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using System.Data.Entity.SqlServer;
using mascis.Models;
using Syncfusion.JavaScript;
using System.Collections;
using Syncfusion.JavaScript.DataSources;

namespace mascis.Controllers
{
    public class fo_contact_personController : Controller
    {
        private mascisEntities db = new mascisEntities();

        // GET: fo_contact_person
        public ActionResult Index()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var fo_contact_person = db.fo_contact_person.Include(f => f.A_Facilities).Include(f => f.A_ImplimentingPartners).Include(f => f.fo_contact_category).Include(f => f.fo_contact_title);
            return View(fo_contact_person.ToList());
        }

        public ActionResult ContactPerson()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                db.Configuration.ProxyCreationEnabled = false;
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }
            ViewBag.YesNo = yesno;

            db.Configuration.ProxyCreationEnabled = false;
            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;

            db.Configuration.ProxyCreationEnabled = false;
            var designations = db.fo_contact_title.AsNoTracking().ToArray();
            ViewBag.Designations = designations;

            db.Configuration.ProxyCreationEnabled = false;
            var impPartners = db.A_ImplimentingPartners .AsNoTracking().ToArray();
            ViewBag.Partners = impPartners;

            db.Configuration.ProxyCreationEnabled = false;
            var suppliers = db.A_Supplier.AsNoTracking().ToArray();
            ViewBag.Suppliers = suppliers;

            db.Configuration.ProxyCreationEnabled = false;
            var contactCategories = db.fo_contact_category .AsNoTracking().ToArray();
            ViewBag.ContactCategories = contactCategories;

            db.Configuration.ProxyCreationEnabled = false;
            var titles = db.fo_contact_title .AsNoTracking().ToArray();
            ViewBag.Titles = titles;

            db.Configuration.ProxyCreationEnabled = false;
            var users = db.AspNetUsers.ToList();
            ViewBag.Users = users;
            //users[0].Id;
            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.View_fo_Contacts.ToList();
            ViewBag.datasource = DataSource;

            db.Configuration.ProxyCreationEnabled = false;
            var districts = db.A_District.AsNoTracking().ToArray();
            ViewBag.Districts = districts;
            //var fo_contact_person = db.fo_contact_person.Include(f => f.A_Facilities).Include(f => f.A_ImplimentingPartners).Include(f => f.fo_contact_category).Include(f => f.fo_contact_title);
            return View(/*fo_contact_person.ToList()*/);
        }
        public ActionResult MaulStaff()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                db.Configuration.ProxyCreationEnabled = false;
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }
            ViewBag.YesNo = yesno;

            db.Configuration.ProxyCreationEnabled = false;
            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;

            db.Configuration.ProxyCreationEnabled = false;
            var designations = db.fo_contact_title.AsNoTracking().ToArray();
            ViewBag.Designations = designations;

            db.Configuration.ProxyCreationEnabled = false;
            var impPartners = db.A_ImplimentingPartners.AsNoTracking().ToArray();
            ViewBag.Partners = impPartners;

            db.Configuration.ProxyCreationEnabled = false;
            var suppliers = db.A_Supplier.AsNoTracking().ToArray();
            ViewBag.Suppliers = suppliers;

            db.Configuration.ProxyCreationEnabled = false;
            var contactCategories = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.ContactCategories = contactCategories;

            db.Configuration.ProxyCreationEnabled = false;
            var titles = db.fo_contact_title.AsNoTracking().ToArray();
            ViewBag.Titles = titles;

            db.Configuration.ProxyCreationEnabled = false;
            var users = db.AspNetUsers.ToList();
            ViewBag.Users = users;
            //users[0].Id;
            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.View_fo_Contacts.ToList();
            ViewBag.datasource = DataSource;

            db.Configuration.ProxyCreationEnabled = false;
            var districts = db.A_District.AsNoTracking().ToArray();
            ViewBag.Districts = districts;
            //var fo_contact_person = db.fo_contact_person.Include(f => f.A_Facilities).Include(f => f.A_ImplimentingPartners).Include(f => f.fo_contact_category).Include(f => f.fo_contact_title);
            return View(/*fo_contact_person.ToList()*/);
        }
        public ActionResult FacilityContacts()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                db.Configuration.ProxyCreationEnabled = false;
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }
            ViewBag.YesNo = yesno;

            db.Configuration.ProxyCreationEnabled = false;
            var facilities = db.A_Facilities.AsNoTracking().ToArray();
            ViewBag.Facilities = facilities;

            db.Configuration.ProxyCreationEnabled = false;
            var designations = db.fo_contact_title.AsNoTracking().ToArray();
            ViewBag.Designations = designations;

            db.Configuration.ProxyCreationEnabled = false;
            var impPartners = db.A_ImplimentingPartners.AsNoTracking().ToArray();
            ViewBag.Partners = impPartners;

            db.Configuration.ProxyCreationEnabled = false;
            var suppliers = db.A_Supplier.AsNoTracking().ToArray();
            ViewBag.Suppliers = suppliers;

            db.Configuration.ProxyCreationEnabled = false;
            var contactCategories = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.ContactCategories = contactCategories;

            db.Configuration.ProxyCreationEnabled = false;
            var titles = db.fo_contact_title.AsNoTracking().ToArray();
            ViewBag.Titles = titles;

            db.Configuration.ProxyCreationEnabled = false;
            var users = db.AspNetUsers.ToList();
            ViewBag.Users = users;
            //users[0].Id;
            db.Configuration.ProxyCreationEnabled = false;
            var DataSource = db.View_fo_Contacts.ToList();
            ViewBag.datasource = DataSource;

            db.Configuration.ProxyCreationEnabled = false;
            var districts = db.A_District.AsNoTracking().ToArray();
            ViewBag.Districts = districts;
            //var fo_contact_person = db.fo_contact_person.Include(f => f.A_Facilities).Include(f => f.A_ImplimentingPartners).Include(f => f.fo_contact_category).Include(f => f.fo_contact_title);
            return View(/*fo_contact_person.ToList()*/);
        }
        public ActionResult DataSourceFacilityContacts(DataManager dm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.View_fo_Contacts.Where(e => e.cp_category == 3 || e.cp_category == 5).OrderBy(o => o.cp_name).ToList();
            int count = db.View_fo_Contacts.Where(e => e.cp_category == 3 || e.cp_category == 5).ToList().Count();
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
        public ActionResult DataSourceMaulContacts(DataManager dm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.View_fo_Contacts.Where(e=> e.cp_category == 8).OrderBy(o => o.cp_name).ToList();
            int count = db.View_fo_Contacts.Where(e => e.cp_category == 8).ToList().Count();
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
        public ActionResult DataSourceContacts(DataManager dm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.View_fo_Contacts.Where(e=> e.cp_category != 8 && e.cp_category != 3 && e.cp_category != 5).OrderBy(o => o.cp_name).ToList();
            int count = db.View_fo_Contacts.Where(e => e.cp_category != 8 && e.cp_category != 3 && e.cp_category != 5).ToList().Count();
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
        public ActionResult DialogInsert(View_fo_Contacts value/*, string testdata, string testdata2, string testdata3*/)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var _feedback = db.fo_contact_person.FirstOrDefault(f=>f.cp_code==value.cp_code);
            
            db.Configuration.ProxyCreationEnabled = false;
            fo_contact_person person = new fo_contact_person();
            //person.cp_code = value.cp_code;
            person.cp_category = value.cp_category;
            person.cp_district = value.cp_district;
            person.cp_facility = value.cp_facility;
            person.cp_ip = value.cp_ip;
            person.cp_name = value.cp_name;
            person.cp_supplier_code = value.cp_supplier_code;
            person.cp_title = value.cp_title;
            person.EditedBy = value.EditedBy;
            person.EditedDate = value.EditedDate;
            person.User_ID = value.User_ID;
            person.AddedBy = value.AddedBy;
            person.AddedDate =value.AddedDate;
            person.IsActive = value.IsActive;

            fo_contact_email email = new fo_contact_email();
            email.cp_code = value.cp_code;
            email.ce_email = value.ce_email;
            email.Personal_email = value.Personal_email;
            email.DisableComfirmed = value.DisableComfirmed;
            email.DisableDelivered = value.DisableDelivered;
            email.DisableDispatched = value.DisableDispatched;
            email.DisableProcessed = value.DisableProcessed;
            email.DisableReceived = value.DisableReceived;
            email.DisableAll = value.DisableAll;
            fo_contact_telephone tel = new fo_contact_telephone();
            tel.cp_code = value.cp_code;
            tel.ct_telephone = Convert.ToInt32(value.ct_telephon);
            tel.ct_telephone_2 = value.ct_telephone_2;

            if (_feedback == null)
            {
                _feedback = db.fo_contact_person.FirstOrDefault(f => f.cp_name == value.cp_name);
                if(_feedback == null)
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    if (person.cp_category == 0) { person.cp_category = null; }
                    if (person.cp_district == 0) { person.cp_district = null; }
                    if (person.cp_facility == 0) { person.cp_facility = null; }
                    if (person.cp_ip == 0) { person.cp_ip = null; }
                    if (person.cp_title == 0) { person.cp_title = null; }
                    var username = new UserManagement().getCurrentuser();
                    var pcode = complaintCode();
                    person.cp_code = ++pcode;

                    person.AddedBy = username;
                    person.AddedDate = DateTime.Now;
                    db.fo_contact_person.Add(person);
                    db.SaveChanges();
                    if (email.ce_email != null)
                    {
                        email.cp_code = person.cp_code;
                        db.fo_contact_email.Add(email);
                        db.SaveChanges();
                    }
                    if (tel.ct_telephone > 0)
                    {
                        tel.cp_code = person.cp_code;
                        db.fo_contact_telephone.Add(tel);
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var username = new UserManagement().getCurrentuser();
                    person.EditedBy = username;
                    person.EditedDate = DateTime.Now;
                    _feedback.AddedBy = person.AddedBy;
                    _feedback.AddedDate = person.AddedDate;
                    _feedback.cp_category = person.cp_category;
                    _feedback.cp_district = person.cp_district;
                    _feedback.cp_facility = person.cp_facility;
                    _feedback.cp_category = person.cp_category;
                    _feedback.cp_district = person.cp_district;
                    _feedback.cp_facility = person.cp_facility;
                    _feedback.cp_ip = person.cp_ip;
                    _feedback.cp_name = person.cp_name;
                    _feedback.cp_supplier_code = person.cp_supplier_code;
                    _feedback.cp_title = person.cp_title;
                    _feedback.EditedBy = person.EditedBy;
                    _feedback.EditedDate = person.EditedDate;
                    _feedback.User_ID = person.User_ID;
                    _feedback.AddedBy = person.AddedBy;
                    _feedback.AddedDate = person.AddedDate;
                    _feedback.IsActive = person.IsActive;
                   // db.Entry(_feedback).CurrentValues.SetValues(person);
                    db.Entry(_feedback).State = EntityState.Modified;
                    db.SaveChanges();
                    var _email = db.fo_contact_email.FirstOrDefault(e => e.cp_code == _feedback.cp_code);
                    if (_email == null)
                    {
                        if (email.ce_email != null)
                        {
                            //email.cp_code = person.cp_code;
                            db.fo_contact_email.Add(email);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.Entry(_email).CurrentValues.SetValues(email);
                        db.Entry(_email).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    var _Tel = db.fo_contact_telephone.FirstOrDefault(e => e.cp_code == _feedback.cp_code);
                    if (_Tel == null)
                    {
                        if (tel.ct_telephone > 0)
                        {
                            //tel.cp_code = person.cp_code;
                            db.fo_contact_telephone.Add(tel);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.Entry(_Tel).CurrentValues.SetValues(tel);
                        db.Entry(_Tel).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }


            }
            else
            {
                db.Configuration.ProxyCreationEnabled = false;
                var username = new UserManagement().getCurrentuser();
                //person.cp_code = _feedback.cp_code;
                person.AddedBy = _feedback.AddedBy;
                person.AddedDate = _feedback.AddedDate;
                person.EditedBy = username;
                person.EditedDate = DateTime.Now;
                _feedback.AddedBy = person.AddedBy;
                _feedback.AddedDate = person.AddedDate;
                _feedback.cp_category = person.cp_category;
                _feedback.cp_district = person.cp_district;
                _feedback.cp_facility = person.cp_facility;
                _feedback.cp_category = person.cp_category;
                _feedback.cp_district = person.cp_district;
                _feedback.cp_facility = person.cp_facility;
                _feedback.cp_ip = person.cp_ip;
                _feedback.cp_name = person.cp_name;
                _feedback.cp_supplier_code = person.cp_supplier_code;
                _feedback.cp_title = person.cp_title;
                _feedback.EditedBy = person.EditedBy;
                _feedback.EditedDate = person.EditedDate;
                _feedback.User_ID = person.User_ID;
                _feedback.AddedBy = person.AddedBy;
                _feedback.AddedDate = person.AddedDate;
                _feedback.IsActive = person.IsActive;
                // db.Entry(_feedback).CurrentValues.SetValues(person);
                db.Entry(_feedback).State = EntityState.Modified;
                db.SaveChanges();
                var _email = db.fo_contact_email.FirstOrDefault(e => e.cp_code == _feedback.cp_code);
                if(_email == null)
                {
                    if (email.ce_email != null)
                    {
                        email.cp_code = person.cp_code;
                        db.fo_contact_email.Add(email);
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.Entry(_email).CurrentValues.SetValues(email);
                    db.Entry(_email).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var _Tel = db.fo_contact_telephone.FirstOrDefault(e => e.cp_code == _feedback.cp_code);
                if (_Tel == null)
                {
                    if (tel.ct_telephone > 0)
                    {
                        tel.cp_code = person.cp_code;
                        db.fo_contact_telephone.Add(tel);
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.Entry(_Tel).CurrentValues.SetValues(tel);
                    db.Entry(_Tel).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            db.SaveChanges(); 
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public int complaintCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var personcode = db.fo_contact_person.OrderByDescending(o => o.cp_code).Take(1).Select(f => f.cp_code).FirstOrDefault();
            return (personcode);
        }
        // GET: fo_contact_person/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            fo_contact_person fo_contact_person = db.fo_contact_person.Find(id);
            if (fo_contact_person == null)
            {
                return HttpNotFound();
            }
            return View(fo_contact_person);
        }

        // GET: fo_contact_person/Create
        public ActionResult Create()
        {
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.cp_facility = new SelectList(db.A_Facilities, "FacilityCode", "Facility");
            ViewBag.cp_ip = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription");
            ViewBag.cp_category = new SelectList(db.fo_contact_category, "category_id", "category_desc");
            ViewBag.cp_title = new SelectList(db.fo_contact_title, "title_code", "title_desc");
            return View();
        }

        // POST: fo_contact_person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cp_code,UserName,UserId,cp_name,cp_title,cp_facility,cp_district,cp_category,cp_ip,cp_supplier_code,AddedBy,AddedDate,EditedBy,EditedDate")] fo_contact_person fo_contact_person)
        {
            if (ModelState.IsValid)
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.fo_contact_person.Add(fo_contact_person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.cp_facility = new SelectList(db.A_Facilities, "FacilityCode", "Facility", fo_contact_person.cp_facility);
            ViewBag.cp_ip = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription", fo_contact_person.cp_ip);
            ViewBag.cp_category = new SelectList(db.fo_contact_category, "category_id", "category_desc", fo_contact_person.cp_category);
            ViewBag.cp_title = new SelectList(db.fo_contact_title, "title_code", "title_desc", fo_contact_person.cp_title);
            return View(fo_contact_person);
        }

        // GET: fo_contact_person/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            fo_contact_person fo_contact_person = db.fo_contact_person.Find(id);
            if (fo_contact_person == null)
            {
                return HttpNotFound();
            }
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.cp_facility = new SelectList(db.A_Facilities, "FacilityCode", "Facility", fo_contact_person.cp_facility);
            ViewBag.cp_ip = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription", fo_contact_person.cp_ip);
            ViewBag.cp_category = new SelectList(db.fo_contact_category, "category_id", "category_desc", fo_contact_person.cp_category);
            ViewBag.cp_title = new SelectList(db.fo_contact_title, "title_code", "title_desc", fo_contact_person.cp_title);
            return View(fo_contact_person);
        }

        // POST: fo_contact_person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cp_code,UserName,UserId,cp_name,cp_title,cp_facility,cp_district,cp_category,cp_ip,cp_supplier_code,AddedBy,AddedDate,EditedBy,EditedDate")] fo_contact_person fo_contact_person)
        {
            if (ModelState.IsValid)
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Entry(fo_contact_person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.cp_facility = new SelectList(db.A_Facilities, "FacilityCode", "Facility", fo_contact_person.cp_facility);
            ViewBag.cp_ip = new SelectList(db.A_ImplimentingPartners, "ImplimentingPartnerCode", "ImplementingPartnerDescription", fo_contact_person.cp_ip);
            ViewBag.cp_category = new SelectList(db.fo_contact_category, "category_id", "category_desc", fo_contact_person.cp_category);
            ViewBag.cp_title = new SelectList(db.fo_contact_title, "title_code", "title_desc", fo_contact_person.cp_title);
            return View(fo_contact_person);
        }

        // GET: fo_contact_person/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            fo_contact_person fo_contact_person = db.fo_contact_person.Find(id);
            if (fo_contact_person == null)
            {
                return HttpNotFound();
            }
            return View(fo_contact_person);
        }

        // POST: fo_contact_person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            fo_contact_person fo_contact_person = db.fo_contact_person.Find(id);
            db.fo_contact_person.Remove(fo_contact_person);
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
