using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using System.IO;
using System.Configuration;
using System.Data.Entity.Infrastructure;

namespace mascis.Controllers
{
    public class FilesClientInfoesController : Controller
    {
        private mascisEntities db = new mascisEntities();

        public ActionResult UploadDocuments()
        {
            ((IObjectContextAdapter)this.db).ObjectContext.CommandTimeout = 180;
            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.Filecategories = db.fo_a_FilesClientInfo.ToList();
            var filesClientInfoes = db.FilesClientInfoes;
            if (string.IsNullOrEmpty((string)TempData["status"]))
                ViewBag.status = string.Empty;
            else
                ViewBag.status = (string)TempData["status"];
            return View();
            //return View(filesClientInfoes.ToList());
        }

        public ActionResult SaveSynchronous(IEnumerable<HttpPostedFileBase> fileInput, string details, string Doc_Category)
        {
            FilesClientInfo objFileCollection = new FilesClientInfo();
            if (fileInput != null)
            {
                foreach (var file in fileInput)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploadedFilePath = "~/" + ConfigurationManager.AppSettings["UploadedFilesDirectory"];
                    var destinationPath = Path.Combine(Server.MapPath(uploadedFilePath), fileName);

                    if (System.IO.File.Exists(destinationPath))
                    {
                        // fileExists = true;
                    }
                    file.SaveAs(destinationPath);
                    string filepathtosave = ConfigurationManager.AppSettings["UploadedFilesDirectory"] + "/" + fileName;
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);

                    //Strip the string of non-numeric characters
                    //  var Regcode = GetNumbers(regcode);
                    objFileCollection.Id = Guid.NewGuid().ToString();
                    objFileCollection.Details = details;
                    objFileCollection.FileURL = filepathtosave;
                    objFileCollection.FileType = fileExtention;
                    int number;

                    bool success = Int32.TryParse(Doc_Category.ToString(), out number);
                    if (success)
                    {
                        objFileCollection.Doc_Category = number;
                    }
                    else
                    {
                        Console.WriteLine("Attempted conversion of '{0}' failed.",
                                           Doc_Category.ToString() ?? "<null>");
                    }

                    db.FilesClientInfoes.Add(objFileCollection);
                    /*Save data to database*/
                    db.SaveChanges();
                    TempData["status"] = "Successfully Uploaded";
                }

                return RedirectToAction("UploadDocuments");
            }
            else
            {
                TempData["status"] = "Select a file to upload";
                return RedirectToAction("UploadDocuments");
            }

        }


        public ActionResult DoesFileExist(IEnumerable<HttpPostedFileBase> fileInput)
        {
            var fileExists = false;
            FilesClientInfo objFileCollection = new FilesClientInfo();
            if (fileInput != null)
            {
                foreach (var file in fileInput)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploadedFilePath = "~/" + ConfigurationManager.AppSettings["UploadedFilesDirectory"];
                    var destinationPath = Path.Combine(Server.MapPath(uploadedFilePath), fileName);
                    /// file.SaveAs(destinationPath);
                    string filepathtosave = ConfigurationManager.AppSettings["UploadedFilesDirectory"] + "/" + fileName;
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);
                    var absolutePath = HttpContext.Server.MapPath(destinationPath);
                    if (System.IO.File.Exists(absolutePath))
                    {
                        fileExists = true;
                    }
                }
            }
            else
            {
                fileExists = false;
            }

            return Content(fileExists.ToString());
        }

        // GET: FilesClientInfoes
        public ActionResult Index()
        {
            var filesClientInfoes = db.FilesClientInfoes;
            return View(filesClientInfoes.ToList());
        }

        // GET: FilesClientInfoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilesClientInfo filesClientInfo = db.FilesClientInfoes.Find(id);
            if (filesClientInfo == null)
            {
                return HttpNotFound();
            }
            return View(filesClientInfo);
        }

        // GET: FilesClientInfoes/Create
        public ActionResult Create()
        {
            ViewBag.Doc_Category = new SelectList(db.fo_a_FilesClientInfo, "Id", "ClientInfo_Desc");
            return View();
        }

        // POST: FilesClientInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FileURL,FileType,Details,Doc_Category,Archive")] FilesClientInfo filesClientInfo)
        {
            if (ModelState.IsValid)
            {
                db.FilesClientInfoes.Add(filesClientInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Doc_Category = new SelectList(db.fo_a_FilesClientInfo, "Id", "ClientInfo_Desc", filesClientInfo.Doc_Category);
            return View(filesClientInfo);
        }

        // GET: FilesClientInfoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilesClientInfo filesClientInfo = db.FilesClientInfoes.Find(id);
            if (filesClientInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Doc_Category = new SelectList(db.fo_a_FilesClientInfo, "Id", "ClientInfo_Desc", filesClientInfo.Doc_Category);
            return View(filesClientInfo);
        }

        // POST: FilesClientInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileURL,FileType,Details,Doc_Category,Archive")] FilesClientInfo filesClientInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filesClientInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Doc_Category = new SelectList(db.fo_a_FilesClientInfo, "Id", "ClientInfo_Desc", filesClientInfo.Doc_Category);
            return View(filesClientInfo);
        }

        // GET: FilesClientInfoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilesClientInfo filesClientInfo = db.FilesClientInfoes.Find(id);
            if (filesClientInfo == null)
            {
                return HttpNotFound();
            }
            return View(filesClientInfo);
        }

        // POST: FilesClientInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            FilesClientInfo filesClientInfo = db.FilesClientInfoes.Find(id);
            db.FilesClientInfoes.Remove(filesClientInfo);
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
