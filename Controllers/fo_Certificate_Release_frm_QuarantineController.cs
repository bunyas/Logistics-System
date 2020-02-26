using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using System.Collections;
using Syncfusion.JavaScript.DataSources;

namespace mascis.Controllers
{
    public class fo_Certificate_Release_frm_QuarantineController : Controller
    {
        private mascisEntities db = new mascisEntities();

        // GET: fo_Certificate_Release_frm_Quarantine
        public ActionResult Index()
        {
            var fo_Certificate_Release_frm_Quarantine = db.fo_Certificate_Release_frm_Quarantine.Include(f => f.A_yes_no).Include(f => f.fo_a_authorized_representative).Include(f => f.fo_a_release_instruction);
            return View(fo_Certificate_Release_frm_Quarantine.ToList());
        }

        public ActionResult Certificate_Release_from_Quarantine()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var maul_Staff = db.fo_contact_person.Where(c => c.cp_category == 8);
            ViewBag.MAUL_Staff = maul_Staff;
            //var prodcuts = db.A_Product.Where(p => p.product_category == 2 || p.product_category == 9).AsNoTracking().ToArray();
            //ViewBag.Products = prodcuts; 

            var contacts = db.fo_contact_category.AsNoTracking().ToArray();
            ViewBag.Contacts = contacts;

            ViewBag.fo_a_release_instruction = db.fo_a_release_instruction .ToList();
            

            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }

            ViewBag.YesNo = yesno;  

            return View();
        }
        public ActionResult GetComplaints(Syncfusion.JavaScript.DataManager dm/*, string compCode*/)
        {
            db.Configuration.ProxyCreationEnabled = false;
            
            IEnumerable data = db.fo_Certificate_Release_frm_Quarantine.ToList(); 
            int count = db.fo_Certificate_Release_frm_Quarantine.ToList().Count(); 

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




        // GET: fo_Certificate_Release_frm_Quarantine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fo_Certificate_Release_frm_Quarantine fo_Certificate_Release_frm_Quarantine = db.fo_Certificate_Release_frm_Quarantine.Find(id);
            if (fo_Certificate_Release_frm_Quarantine == null)
            {
                return HttpNotFound();
            }
            return View(fo_Certificate_Release_frm_Quarantine);
        }

        // GET: fo_Certificate_Release_frm_Quarantine/Create
        public ActionResult Create()
        {
            ViewBag.Copy_Investigation_Report = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc");
            ViewBag.Authorized_Representative_Category = new SelectList(db.fo_a_authorized_representative, "AR_code", "Authorized_Representative_Desc");
            ViewBag.Release_Instruction = new SelectList(db.fo_a_release_instruction, "RI_code", "Release_Instruction_Desc");
            ViewBag.e_reg_complaint_code = new SelectList(db.fo_complaint_quality_issue, "Id", "e_reg_complaint_No");
            ViewBag.cp_code = new SelectList(db.fo_contact_person, "cp_code", "cp_name");
            ViewBag.Release_Authorizedby = new SelectList(db.fo_contact_person, "cp_code", "cp_name");
            return View();
        }

        // POST: fo_Certificate_Release_frm_Quarantine/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,e_reg_complaint_code,batch_no,product_code,cp_code,Staff_name,Staff_phone,Staff_Designation,Staff_email,Date_request,product_strength,dosage_form,product_other,manufacturer,Manufacture_date,Expiry_date,Supplier,Quantity,e_reg_complaint_No,Summary_of_Investigations,Release_Instruction,Copy_Investigation_Report,Goods_Returned_Note,attached_otherDoc,attached_other,Release_Authorizedby,Release_Authorized_Date,Released_By,Release_Date,Received_by,Received_Date,Authorized_Representative_Category")] fo_Certificate_Release_frm_Quarantine fo_Certificate_Release_frm_Quarantine)
        {
            if (ModelState.IsValid)
            {
                db.fo_Certificate_Release_frm_Quarantine.Add(fo_Certificate_Release_frm_Quarantine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Copy_Investigation_Report = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_Certificate_Release_frm_Quarantine.Copy_Investigation_Report);
            ViewBag.Authorized_Representative_Category = new SelectList(db.fo_a_authorized_representative, "AR_code", "Authorized_Representative_Desc", fo_Certificate_Release_frm_Quarantine.Authorized_Representative_Category);
            ViewBag.Release_Instruction = new SelectList(db.fo_a_release_instruction, "RI_code", "Release_Instruction_Desc", fo_Certificate_Release_frm_Quarantine.Release_Instruction);
            ViewBag.e_reg_complaint_code = new SelectList(db.fo_complaint_quality_issue, "Id", "e_reg_complaint_No", fo_Certificate_Release_frm_Quarantine.e_reg_complaint_code);
            ViewBag.cp_code = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_Certificate_Release_frm_Quarantine.cp_code);
            ViewBag.Release_Authorizedby = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_Certificate_Release_frm_Quarantine.Release_Authorizedby);
            return View(fo_Certificate_Release_frm_Quarantine);
        }

        // GET: fo_Certificate_Release_frm_Quarantine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fo_Certificate_Release_frm_Quarantine fo_Certificate_Release_frm_Quarantine = db.fo_Certificate_Release_frm_Quarantine.Find(id);
            if (fo_Certificate_Release_frm_Quarantine == null)
            {
                return HttpNotFound();
            }
            ViewBag.Copy_Investigation_Report = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_Certificate_Release_frm_Quarantine.Copy_Investigation_Report);
            ViewBag.Authorized_Representative_Category = new SelectList(db.fo_a_authorized_representative, "AR_code", "Authorized_Representative_Desc", fo_Certificate_Release_frm_Quarantine.Authorized_Representative_Category);
            ViewBag.Release_Instruction = new SelectList(db.fo_a_release_instruction, "RI_code", "Release_Instruction_Desc", fo_Certificate_Release_frm_Quarantine.Release_Instruction);
            ViewBag.e_reg_complaint_code = new SelectList(db.fo_complaint_quality_issue, "Id", "e_reg_complaint_No", fo_Certificate_Release_frm_Quarantine.e_reg_complaint_code);
            ViewBag.cp_code = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_Certificate_Release_frm_Quarantine.cp_code);
            ViewBag.Release_Authorizedby = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_Certificate_Release_frm_Quarantine.Release_Authorizedby);
            return View(fo_Certificate_Release_frm_Quarantine);
        }

        // POST: fo_Certificate_Release_frm_Quarantine/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,e_reg_complaint_code,batch_no,product_code,cp_code,Staff_name,Staff_phone,Staff_Designation,Staff_email,Date_request,product_strength,dosage_form,product_other,manufacturer,Manufacture_date,Expiry_date,Supplier,Quantity,e_reg_complaint_No,Summary_of_Investigations,Release_Instruction,Copy_Investigation_Report,Goods_Returned_Note,attached_otherDoc,attached_other,Release_Authorizedby,Release_Authorized_Date,Released_By,Release_Date,Received_by,Received_Date,Authorized_Representative_Category")] fo_Certificate_Release_frm_Quarantine fo_Certificate_Release_frm_Quarantine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fo_Certificate_Release_frm_Quarantine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Copy_Investigation_Report = new SelectList(db.A_yes_no, "yes_no_id", "yes_no_desc", fo_Certificate_Release_frm_Quarantine.Copy_Investigation_Report);
            ViewBag.Authorized_Representative_Category = new SelectList(db.fo_a_authorized_representative, "AR_code", "Authorized_Representative_Desc", fo_Certificate_Release_frm_Quarantine.Authorized_Representative_Category);
            ViewBag.Release_Instruction = new SelectList(db.fo_a_release_instruction, "RI_code", "Release_Instruction_Desc", fo_Certificate_Release_frm_Quarantine.Release_Instruction);
            ViewBag.e_reg_complaint_code = new SelectList(db.fo_complaint_quality_issue, "Id", "e_reg_complaint_No", fo_Certificate_Release_frm_Quarantine.e_reg_complaint_code);
            ViewBag.cp_code = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_Certificate_Release_frm_Quarantine.cp_code);
            ViewBag.Release_Authorizedby = new SelectList(db.fo_contact_person, "cp_code", "cp_name", fo_Certificate_Release_frm_Quarantine.Release_Authorizedby);
            return View(fo_Certificate_Release_frm_Quarantine);
        }

        // GET: fo_Certificate_Release_frm_Quarantine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fo_Certificate_Release_frm_Quarantine fo_Certificate_Release_frm_Quarantine = db.fo_Certificate_Release_frm_Quarantine.Find(id);
            if (fo_Certificate_Release_frm_Quarantine == null)
            {
                return HttpNotFound();
            }
            return View(fo_Certificate_Release_frm_Quarantine);
        }

        // POST: fo_Certificate_Release_frm_Quarantine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            fo_Certificate_Release_frm_Quarantine fo_Certificate_Release_frm_Quarantine = db.fo_Certificate_Release_frm_Quarantine.Find(id);
            db.fo_Certificate_Release_frm_Quarantine.Remove(fo_Certificate_Release_frm_Quarantine);
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
