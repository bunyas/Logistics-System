using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;
using mascis.Models;
using System.Collections;

 
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using System.Data.Entity.SqlServer;
using Syncfusion.JavaScript.DataSources;
using System.Data.Entity.Validation;
using mascis.ScheduledTasks;

namespace mascis.Controllers
{
    public class ScheduleEventsController : Controller
    {
        private mascisEntities db = new mascisEntities();
        List<Resources> owners = new List<Resources>();
        // GET: ScheduleEvents
        public ActionResult Index()
        {
            return View(db.ScheduleEvents.ToList());
        }


        public ActionResult ScheduleEvent()
        {
            var id = string.Empty;
            Session["Id"] = id;

            db.Configuration.ProxyCreationEnabled = false;
            foreach (var x in db.A_Venue.AsNoTracking().OrderBy(a => a.VenueName))
            {
                owners.Add(new Resources { Text = x.VenueName, Id = x.id.ToString(), GroupId = "1", Color = x.Color });
            }

            ViewBag.Owners = owners;
            db.Configuration.ProxyCreationEnabled = false;
            var maul_Staff = db.fo_contact_person.Where(c => c.cp_category == 8).AsNoTracking().ToList();
            ViewBag.MAUL_Staff = maul_Staff;
            List<String> resources = new List<String>();
            resources.Add("Owners");
            ViewBag.Resources = resources;

            db.Configuration.ProxyCreationEnabled = false;
            var eventcatgories = db.A_EventCategory.AsNoTracking().ToList();
            ViewBag.EventCategories = eventcatgories;
            db.Configuration.ProxyCreationEnabled = false;
            var venues = db.A_Venue.AsNoTracking().ToList();
            ViewBag.Venues = venues;
            db.Configuration.ProxyCreationEnabled = false;
            var venuesList = db.A_Venue_List.AsNoTracking().ToList();
            ViewBag.VenuesList = venuesList;
            return View( );
        }


        public JsonResult getParticipationData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.fo_contact_person.Where(c => c.cp_category == 8).OrderBy(c => c.cp_name);
            var jsonSerializer = new JavaScriptSerializer();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getVenueData()
        {
            List<DropdownList> obj = new List<DropdownList>();
            obj.Add(new DropdownList { ID = "", DESC = "--Please Select--" });
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.A_Venue.AsNoTracking().ToList();

            foreach (var x in data)
            {
                obj.Add(new DropdownList { ID = x.id.ToString(), DESC = x.VenueName });
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public class DropdownList
        {
            public string ID { get; set; }
            public string FK_ID { get; set; }
            public string DESC { get; set; }
        }

        public class Resources
        {
            public string Text { set; get; }
            public string Id { set; get; }
            public string GroupId { set; get; }
            public string Color { set; get; }
        }
        public ActionResult LoadOnDemand()
        {
            System.Web.HttpContext.Current.Session["Appointments"] = null;
            return View();
        }

        public ActionResult GetRecords(string CurrentView, DateTime? CurrentDate, string CurrentAction)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = EventsRepository.FilterAppointment(System.Convert.ToDateTime(CurrentDate), CurrentAction, CurrentView);
            BatchDataResult result = new BatchDataResult();
            result.result = data;
            result.count = EventsRepository.GetAllRecords().ToList().Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(ScheduleEvent value)
        {
            if ((value.IsBlockAppointment == true && value.BlockForAll == true) /*|| value.RoundTripActivity == true*/)
            {
                if (value.IsBlockAppointment == true)
                {
                    value.AllDay = true;
                }
            }
            Save(value);
            // send email
            EmailJob email = new EmailJob();
            email.SendEmailSchedular(value.ParticipationInvitee, value.VenueCode,Convert.ToDateTime(value.StartTime), Convert.ToDateTime(value.EndTime), value.EventCategory);

            IEnumerable data = new mascisEntities().ScheduleEvents;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public void Save(ScheduleEvent value)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //int intMax = db.ScheduleEvents.ToList().Count > 0 ? db.ScheduleEvents.ToList().Max(p => p.Id) : 1;
            DateTime startTime = Convert.ToDateTime(value.StartTime);
            DateTime endTime = Convert.ToDateTime(value.EndTime);
            var currentTimeZone = TimeZone.CurrentTimeZone;
            ScheduleEvent appoint = new ScheduleEvent()
            {
                SheduleID = value.SheduleID,
                //Id = intMax + 1,
                StartTime = startTime.ToUniversalTime(),
                EndTime = endTime.ToUniversalTime(),
                Subject = value.Subject,
                Location = value.Location,
                Description = value.Description,
                Owner = value.Owner,
                Priority = value.Priority,
                Recurrence = value.Recurrence,
                RecurrenceType = value.RecurrenceType,
                Reminder = value.Reminder,
                Categorize = value.Categorize,
                AllDay = value.AllDay,
                RecurrenceEndDate = value.RecurrenceEndDate != null ? Convert.ToDateTime(value.RecurrenceEndDate).ToUniversalTime() : endTime.ToUniversalTime(),
                RecurrenceStartDate = value.RecurrenceStartDate != null ? Convert.ToDateTime(value.RecurrenceStartDate).ToUniversalTime() : startTime.ToUniversalTime(),
                RecurrenceRule = value.RecurrenceRule,
                RecurrenceTypeCount = value.RecurrenceTypeCount,
                CustomStyle = value.CustomStyle,
                RemiderType = value.RemiderType,
                RemiderTypeCount = value.RemiderTypeCount,
                RemiderStartDate = value.RemiderStartDate,
                RemiderEndDate = value.RemiderEndDate,
                RemiderRule = value.RemiderRule,
                VenueCode = value.VenueCode,
                ParticipationInvitee = value.ParticipationInvitee,
                ParticipationAttendee = value.ParticipationAttendee,
                EventCategory = value.EventCategory,
                Organizer = value.Organizer,
                MeetingType = value.MeetingType,
                PublicHoliday = value.PublicHoliday,
                Added_By = new UserManagement().getCurrentuser(), //value.Added_By,
                Date_Added = DateTime.Now, //value.Date_Added,
                EventOccured = value.EventOccured,
                NoEventOccuredComment = value.NoEventOccuredComment,
                IsBlockAppointment = value.IsBlockAppointment,
                VenueList = value.VenueList,
                BlockForAll = value.BlockForAll,
            };
            db.Configuration.ProxyCreationEnabled = false;
            db.ScheduleEvents.Add(appoint);
            //db.SaveChanges();
            try
            {
                db.SaveChanges();
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
         
        public JsonResult Delete(ScheduleEvent value)
        {
            //if (value.ApprovedActivity != true)
            {
                var activity = new ScheduleEvent { SheduleID = value.SheduleID };
                db.ScheduleEvents.Attach(activity);
                db.ScheduleEvents.Remove(activity);
                //db.SaveChanges();
            }
            try
            {
                db.SaveChanges();
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            IEnumerable data = new mascisEntities().ScheduleEvents;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(ScheduleEvent value)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var filterData = db.ScheduleEvents.Where(c => c.SheduleID == value.SheduleID);

            if (filterData.Count() > 0)
            {
                DateTime startTime = Convert.ToDateTime(value.StartTime);
                DateTime endTime = Convert.ToDateTime(value.EndTime);
                ScheduleEvent appoint = db.ScheduleEvents.Single(A => A.SheduleID == value.SheduleID);
                appoint.StartTime = startTime.ToUniversalTime();
                appoint.EndTime = endTime.ToUniversalTime();
                appoint.Subject = value.Subject;
                appoint.Location = value.Location;
                appoint.Description = value.Description;
                appoint.Owner = value.Owner;
                appoint.Priority = Convert.ToByte(value.Priority);
                appoint.Recurrence = Convert.ToBoolean(value.Recurrence);
                appoint.RecurrenceType = value.RecurrenceType;
                appoint.RecurrenceTypeCount = Convert.ToInt16(value.RecurrenceTypeCount);
                appoint.Reminder = value.Reminder;
                appoint.Categorize = value.Categorize;
                appoint.AllDay = value.AllDay;
                appoint.RecurrenceEndDate = value.RecurrenceEndDate != null ? Convert.ToDateTime(value.RecurrenceEndDate).ToUniversalTime() : endTime.ToUniversalTime();
                appoint.RecurrenceStartDate = value.RecurrenceStartDate != null ? Convert.ToDateTime(value.RecurrenceStartDate).ToUniversalTime() : startTime.ToUniversalTime();
                appoint.RecurrenceRule = value.RecurrenceRule;
                appoint.RecurrenceTypeCount = value.RecurrenceTypeCount;
                appoint.CustomStyle = value.CustomStyle;
                appoint.RemiderType = value.RemiderType;
                appoint.RemiderTypeCount = value.RemiderTypeCount;
                appoint.RemiderStartDate = value.RemiderStartDate;
                appoint.RemiderEndDate = value.RemiderEndDate;
                appoint.RemiderRule = value.RemiderRule;
                appoint.VenueCode = value.VenueCode;
                appoint.ParticipationInvitee = value.ParticipationInvitee;
                appoint.ParticipationAttendee = value.ParticipationAttendee;
                appoint.EventCategory = value.EventCategory;
                appoint.Organizer = value.Organizer;
                appoint.MeetingType = value.MeetingType;
                appoint.PublicHoliday = value.PublicHoliday;
                appoint.Added_By = new UserManagement().getCurrentuser();
                appoint.Date_Added = DateTime.Now;
                appoint.EventOccured = value.EventOccured;
                appoint.NoEventOccuredComment = value.NoEventOccuredComment;
                appoint.IsBlockAppointment = value.IsBlockAppointment;
                appoint.VenueCode = value.VenueList;
                appoint.BlockForAll = value.BlockForAll;
            }
            else
            {
                Save(value);
            }
            // db.SaveChanges();
            try
            {
                db.SaveChanges();
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            IEnumerable data = new mascisEntities().ScheduleEvents;
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Batch(EditParams param)
        {
            try
            {
                if (param.action == "insert" || (param.action == "batch" && param.added != null))          // this block of code will execute while inserting the appointments
                {
                    var value = param.action == "insert" ? param.value : param.added[0];
                    db.Configuration.ProxyCreationEnabled = false;
                    //int intMax = db.ScheduleEvents.ToList().Count > 0 ? db.ScheduleEvents.ToList().Max(p => p.Id) : 1;
                    DateTime startTime = Convert.ToDateTime(value.StartTime);
                    DateTime endTime = Convert.ToDateTime(value.EndTime);
                    var currentTimeZone = TimeZone.CurrentTimeZone;
                    ScheduleEvent appoint = new ScheduleEvent()
                    {
                        SheduleID=value.SheduleID,
                        //Id = intMax + 1,
                        StartTime = startTime.ToUniversalTime(),
                        EndTime = endTime.ToUniversalTime(),
                        Subject = value.Subject,
                        Location = value.Location,
                        Description = value.Description,
                        Owner = value.Owner,
                        Priority = value.Priority,
                        Recurrence = value.Recurrence,
                        RecurrenceType = value.RecurrenceType,
                        Reminder = value.Reminder,
                        Categorize = value.Categorize,
                        AllDay = value.AllDay,
                        RecurrenceEndDate = value.RecurrenceEndDate != null ? Convert.ToDateTime(value.RecurrenceEndDate).ToUniversalTime() : endTime.ToUniversalTime(),
                        RecurrenceStartDate = value.RecurrenceStartDate != null ? Convert.ToDateTime(value.RecurrenceStartDate).ToUniversalTime() : startTime.ToUniversalTime(),
                        RecurrenceRule = value.RecurrenceRule,
                        RecurrenceTypeCount = value.RecurrenceTypeCount,
                        CustomStyle = value.CustomStyle,
                        RemiderType = value.RemiderType,
                        RemiderTypeCount = value.RemiderTypeCount,
                        RemiderStartDate = value.RemiderStartDate,
                        RemiderEndDate = value.RemiderEndDate,
                        RemiderRule = value.RemiderRule,
                        VenueCode = value.VenueCode,
                        ParticipationInvitee = value.ParticipationInvitee,
                        ParticipationAttendee = value.ParticipationAttendee,
                        EventCategory = value.EventCategory,
                        Organizer = value.Organizer,
                        MeetingType = value.MeetingType,
                        PublicHoliday = value.PublicHoliday,
                        Added_By = new UserManagement().getCurrentuser(), //value.Added_By,
                        Date_Added = DateTime.Now, //value.Date_Added,
                        EventOccured = value.EventOccured,
                        NoEventOccuredComment = value.NoEventOccuredComment,
                        IsBlockAppointment = value.IsBlockAppointment,
                        VenueList=value.VenueList,
                        BlockForAll = value.BlockForAll, 
                    };
                    db.Configuration.ProxyCreationEnabled = false;
                    db.ScheduleEvents.Add(appoint);
                    db.SaveChanges();
                }
                else if (param.action == "remove")                                        // this block of code will execute while removing the appointment
                {
                    ScheduleEvent app = db.ScheduleEvents.Where(c => c.SheduleID.ToString() == param.key).FirstOrDefault();
                    var myevent = app;
                    if (app != null) db.ScheduleEvents.Remove(app);
                    db.SaveChanges();
                }
                else if ((param.action == "batch" && param.changed != null) || param.action == "update")   // this block of code will execute while updating the appointment
                {
                    var value = param.action == "update" ? param.value : param.changed[0];
                    //var filterData = db.ScheduleEvents.Where(c => c.Id == Convert.ToInt32(value.Id));
                    db.Configuration.ProxyCreationEnabled = false;
                    var filterData = db.ScheduleEvents.Where(c => c.SheduleID == value.SheduleID);

                    if (filterData.Count() > 0)
                    {
                        DateTime startTime = Convert.ToDateTime(value.StartTime);
                        DateTime endTime = Convert.ToDateTime(value.EndTime);
                        ScheduleEvent appoint = db.ScheduleEvents.Single(A => A.SheduleID == value.SheduleID);
                        appoint.StartTime = startTime.ToUniversalTime();
                        appoint.EndTime = endTime.ToUniversalTime();
                        appoint.Subject = value.Subject;
                        appoint.Location = value.Location;
                        appoint.Description = value.Description;
                        appoint.Owner = value.Owner;
                        appoint.Priority = Convert.ToByte(value.Priority);
                        appoint.Recurrence = Convert.ToBoolean(value.Recurrence);
                        appoint.RecurrenceType = value.RecurrenceType;
                        appoint.RecurrenceTypeCount = Convert.ToInt16(value.RecurrenceTypeCount);
                        appoint.Reminder = value.Reminder;
                        appoint.Categorize = value.Categorize;
                        appoint.AllDay = value.AllDay;
                        appoint.RecurrenceEndDate = value.RecurrenceEndDate != null ? Convert.ToDateTime(value.RecurrenceEndDate).ToUniversalTime() : endTime.ToUniversalTime();
                        appoint.RecurrenceStartDate = value.RecurrenceStartDate != null ? Convert.ToDateTime(value.RecurrenceStartDate).ToUniversalTime() : startTime.ToUniversalTime();
                        appoint.RecurrenceRule = value.RecurrenceRule;
                        appoint.RecurrenceTypeCount = value.RecurrenceTypeCount;
                        appoint.CustomStyle = value.CustomStyle;
                        appoint.RemiderType = value.RemiderType;
                        appoint.RemiderTypeCount = value.RemiderTypeCount;
                        appoint.RemiderStartDate = value.RemiderStartDate;
                        appoint.RemiderEndDate = value.RemiderEndDate;
                        appoint.RemiderRule = value.RemiderRule;
                        appoint.VenueCode = value.VenueCode;
                        appoint.ParticipationInvitee = value.ParticipationInvitee;
                        appoint.ParticipationAttendee = value.ParticipationAttendee;
                        appoint.EventCategory = value.EventCategory;
                        appoint.Organizer = value.Organizer;
                        appoint.MeetingType = value.MeetingType;
                        appoint.PublicHoliday = value.PublicHoliday;
                        appoint.Added_By = new UserManagement().getCurrentuser();
                        appoint.Date_Added = DateTime.Now;
                        appoint.EventOccured = value.EventOccured;
                        appoint.NoEventOccuredComment = value.NoEventOccuredComment;
                        appoint.IsBlockAppointment = value.IsBlockAppointment;
                        appoint.VenueCode = value.VenueList;
                        appoint.BlockForAll = value.BlockForAll; 
                    }
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw ex;//new Exception ex.Message;
            }
            
            IEnumerable data = new mascisEntities().ScheduleEvents.ToList(); // nw.Appointment.Take(5);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveDefault(string[] fileNames)
        {
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            return Content("");
        }

        public ActionResult loadfiles(int? doc_code)
        {
            FileListModel objfilelistmodel = new FileListModel();
            objfilelistmodel.FileListCollction = new List<FileList>();
            objfilelistmodel.FileListCollction = GetUploadedFileList(doc_code);
            //return View(objfilelistmodel);
            return Json(objfilelistmodel, JsonRequestBehavior.AllowGet);
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
            List<FileList> objfilelist = new List<FileList>();
            mascisEntities objmascis = new mascisEntities(); 
            var datacollection = objmascis.FileCollections.ToList();      
            foreach (var item in datacollection)
            {
                if (item.e_reg_complaint_code == e_reg_complaint_code)
                {
                    objfilelist.Add(new FileList { FileURL = item.FileURL, FileType = item.FileType, Detail = item.Details, e_reg_complaint_code = item.e_reg_complaint_code });
                 } 
            }
            return objfilelist;
        }

        public ActionResult SaveSupportDoc(IEnumerable<HttpPostedFileBase> UploadInput, string UploadInput_data)
        {
            Session["Id"] = UploadInput_data;
            try
            {
                foreach (var file in UploadInput)
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = Path.GetFileName(file.FileName);
                    var uploadedFilePath = "~/" + ConfigurationManager.AppSettings["UploadedFilesDirectory"];
                    //var destinationPath = Path.Combine(Server.MapPath("~/Upload_Files"), fileName);
                    var destinationPath = Path.Combine(Server.MapPath(uploadedFilePath), fileName);
                    file.SaveAs(destinationPath);
                    string filepathtosave = ConfigurationManager.AppSettings["UploadedFilesDirectory"] + "/" + fileName;
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);

                    //Strip the string of non-numeric characters
                    var regcode = GetNumbers(UploadInput_data);

                    //Save infor into the DB
                    ScheduleEvent_FileCollection objFileCollection = new ScheduleEvent_FileCollection();
                    objFileCollection.Id = Guid.NewGuid().ToString();
                   // objFileCollection.Details = file.FileName + " <---- Testing in the wind!";
                    objFileCollection.Details = Path.GetFileNameWithoutExtension(fileName); ;
                    objFileCollection.FileURL = filepathtosave;
                    objFileCollection.FileType = fileExtention;
                    objFileCollection.SheduleAppID = regcode.ToString();

                    db.ScheduleEvent_FileCollection.Add(objFileCollection);
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

        private object GetNumbers(string uploadInput_data)
        {
            try
            {
                return new string(uploadInput_data.Where(c => c != '"').ToArray());
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
            
        }

        //private static string GetNumbers(string input)
        //{
            
        //}


        public class BatchDataResult
        {
            public IEnumerable result { get; set; }
            public int count { get; set; }
        }

        // GET: ScheduleEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleEvent scheduleEvent = db.ScheduleEvents.Find(id);
            if (scheduleEvent == null)
            {
                return HttpNotFound();
            }
            return View(scheduleEvent);
        }

        // GET: ScheduleEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduleEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subject,Description,StartTime,EndTime,AllDay,Recurrence,RecurrenceRule,VenueCode,ParticipationInvitee,ParticipationAttendee,ExpectedOutput,Objective,ActivityClassificationId,OrganId,DirectorateId,SectorId,SourceOfFundingId,FundingTypeId,PublicHoliday,CostEstimates,MeetingType,Approved_By,Date_Approved,Added_By,Date_Added,City,ActivityImplemented,ActivityImplementedComment,ApprovedActivity,IsBlockAppointment,BlockForAll,ActivityForwarded,Organizer")] ScheduleEvent scheduleEvent)
        {
            if (ModelState.IsValid)
            {
                db.ScheduleEvents.Add(scheduleEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scheduleEvent);
        }

        // GET: ScheduleEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleEvent scheduleEvent = db.ScheduleEvents.Find(id);
            if (scheduleEvent == null)
            {
                return HttpNotFound();
            }
            return View(scheduleEvent);
        }

        // POST: ScheduleEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,Description,StartTime,EndTime,AllDay,Recurrence,RecurrenceRule,VenueCode,ParticipationInvitee,ParticipationAttendee,ExpectedOutput,Objective,ActivityClassificationId,OrganId,DirectorateId,SectorId,SourceOfFundingId,FundingTypeId,PublicHoliday,CostEstimates,MeetingType,Approved_By,Date_Approved,Added_By,Date_Added,City,ActivityImplemented,ActivityImplementedComment,ApprovedActivity,IsBlockAppointment,BlockForAll,ActivityForwarded,Organizer")] ScheduleEvent scheduleEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scheduleEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scheduleEvent);
        }

        // GET: ScheduleEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleEvent scheduleEvent = db.ScheduleEvents.Find(id);
            if (scheduleEvent == null)
            {
                return HttpNotFound();
            }
            return View(scheduleEvent);
        }

        // POST: ScheduleEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScheduleEvent scheduleEvent = db.ScheduleEvents.Find(id);
            db.ScheduleEvents.Remove(scheduleEvent);
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

        public ActionResult CodeString()
        {
            try
            {
                var tdate = DateTime.Now;
                var tmoon = tdate.Month;
                var tyear = tdate.Year;
                var complaintCode = "";
                db.Configuration.ProxyCreationEnabled = false;
                var fo_complaint = db.ScheduleEvents.Where(t => SqlFunctions.DatePart("mm", t.Date_Added) == tmoon && SqlFunctions.DatePart("yyyy", t.Date_Added) == tyear).ToList().Count;
                if (fo_complaint < 1)
                {
                    fo_complaint = 1;
                }
                complaintCode = fo_complaint.ToString("D4") + "-" + tmoon.ToString() + "-" + tyear.ToString().TrimEnd().Substring(2, 2);

                return Json(new { msg = String.Format("{0}", complaintCode) }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CompaintCount()
        {
            db.Configuration.ProxyCreationEnabled = false;
            // DataResult result = new DataResult();
            DataOperations operation = new DataOperations();
            int result = 0;
            db.Configuration.ProxyCreationEnabled = false;
            result = db.ScheduleEvents.ToList().Count();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
