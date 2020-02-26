using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using mascis.Models;
using mascis.ScheduledTasks;
using System.Data;
using mascis.Hubs;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using TableDependency.SqlClient;
using Microsoft.AspNet.SignalR.Hubs;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient.Base.Enums;


namespace mascis.ScheduledTasks
{
    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
            //Schedule<EmailJob>().ToRunEvery(1).Days().At(21, 15);
            // Schedule<EmailJob>().ToRunNow().AndEvery(1).Days();
            //Call sqltabledepency here************
            //var ms = MonitorService.Instance;
            //Schedule<EmailJob>().ToRunNow().AndEvery(1).Days().At(23, 59); 
        }
    }

    public class EmailJob : IJob
    {

        //public DbContext Context { get; } // we need this dependency, right?!
        mascisEntities dbcontext = new mascisEntities();
        public EmailJob() //constructor injection
        {



            //Send sms
            // if (ModelState.IsValid)
            //{
            try
            {
                SMS sms = new SMS();
                string username = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsername"]);
                string password = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsernamePassword"]);
                string senderid = HttpUtility.UrlEncode(sms.SenderId);
                string sURL = ConfigurationManager.AppSettings["SmsUrl"]; ;

                StreamReader objReader;
                sms.SenderId = "MAUL";
                sms.ReceipientsNumbers = "";
                sms.Message = "Kindly be notified that MAUL has received your order. For any inquiries, please contact-Tel: +256759207240 Email: customercare @medicalaccess.co.ug ";

                string xURL = "http://sms.smsone.co.ug:8866/cgi-bin/sendsms?username=mediacc&password=MUa334&from=MAUL&to=256752646726&text=Peter Wakabi";
                //  string xURL = "http://apidocs.speedamobile.com/api/SendSMS?api_id=API4678406659&api_password=MUa334Md&sms_type=T&encoding=T&sender_id=6669&phonenumber=256787386065&textmessage=testsms";
                xURL = sURL + "?username=" + username + "&password=" + password + "&from=MAUL" + "&to=" + HttpUtility.UrlEncode(sms.ReceipientsNumbers) + "&text=" + HttpUtility.UrlEncode(sms.Message);


                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(xURL);
                try
                {
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                    objReader = new StreamReader(objStream);
                    string responseString = objReader.ReadToEnd();
                    objReader.Close();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            catch (Exception ex)
            {
                //  ModelState.AddModelError("", ex.Message);
            }
            // }

        }
        public void SendEmail(int? facilitycode, string OrderNumber)
        {
            try
            {
                UserManagement obj = new UserManagement();

                var email = dbcontext.View_WebTemplate_RecipientEmail.Where(e => e.FacilityCode == facilitycode && (e.DisableReceived == false || e.DisableReceived == null)&&(e.DisableAll == false || e.DisableAll== null) && e.IsActive == true).ToList();
                var count = obj.getCurrentFacilityEmail(facilitycode).Count();
                //if email is null , no facility tagged to email
                //if (count == 0)
                //{
                //    var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                //    email = emailsss;// = "customercare@medicalaccess.co.ug";
                //}
                var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
                foreach (var n in email)
                {
                    using (var message = new MailMessage("donotreply@medicalaccess.co.ug", n.ce_email))
                    {
                        // foreach (staWebTemplate_RecipientEmail objxx in objx)

                        message.Subject = "Order Received, Order Number: " + OrderNumber + " " + " on" + " " + DateTime.Now.ToLongDateString();
                        message.Body = "Dear " + "" + fc + ",<br/><br/>" + "Kindly be notified that Medical Access Uganda Ltd has received your order " + OrderNumber + ". <br/> Please contact us should you have any questions regarding your order. Tel: +256759207240.<br/> Email: customercare@medicalaccess.co.ug <br/> You can also logon to the <a href='http://10.33.0.15/mascis/Account/Login'> Medical Access Website</a> portal to track you order.<br/><br/> Regards <br/> Medical Access Uganda Limited  <br/>" + DateTime.Now + "<br/><br/><hr/> If You don't want to receive these emails from Medical Access Uganda Limited in the future, Please <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=1'> Unsubscribe</a> Or  <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=6'> Unsubscribe all</a>";
                        message.IsBodyHtml = true;

                        // message..Add("simonbunya@gmail.com");
                        //for (int i = 1; i < email.Count(); i++)
                        //{
                        //    message.CC.Add("customercare@medicalaccess.co.ug");
                        //}
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = true,
                            //Host = "smtp.gmail.com",
                            Host = "smtp.office365.com",
                            //Port = 587,
                            Port = 587,

                            Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                        })
                        {
                            //client.EnableSsl = true;
                            client.Send(message);
                        }
                    }
                }


            }
            catch
            {

            }
           
        }

        //LMIS Codinator

        public void SendEmailLMISCodinator(string OrderNumber)
        {
            UserManagement obj = new UserManagement();
            //getCurrentuser
            var loggedinUser = obj.getCurrentuser();
            var email = obj.getCurrentLMISCOdinatorEmail();
            var count = obj.getCurrentLMISCOdinatorEmail().Count();
            //if email is null , no facility tagged to email

            if (count == 0)
            {
                var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                email = emailsss;// = "customercare@medicalaccess.co.ug";
            }
            //var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
            using (var message = new MailMessage("donotreply@medicalaccess.co.ug", email[0]))
            {
                // foreach (staWebTemplate_RecipientEmail objxx in objx)

                message.Subject = "Order Allocated, Pending Approval from LMIS Co-ordinator, Order Number: " + OrderNumber + " " + "on" + " " + DateTime.Now.ToLongDateString();
                message.Body = "Dear LMIS Co-ordinator,<br/><br/>" + loggedinUser + " is kindly reguesting you to approve the order " + OrderNumber + " For SAP export,<br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to approve the order.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                message.IsBodyHtml = true;

                // message..Add("simonbunya@gmail.com");
                for (int i = 1; i < email.Count(); i++)
                {
                    message.CC.Add(email[i]);
                }
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    //Host = "smtp.gmail.com",
                    Host = "smtp.office365.com",
                    //Port = 587,
                    Port = 587,

                    Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                })
                {
                    //client.EnableSsl = true;
                    client.Send(message);
                }
            }

        }
        public void OrderSentToScto(int? facilitycode, string OrderNumber, string Category)
        {
            try
            {
                var Scto = dbcontext.Chss_M_SCTO_Contact.FirstOrDefault(o => o.FacilityCode == facilitycode);
                var facility = dbcontext.A_Facilities.FirstOrDefault(o => o.FacilityCode == facilitycode);
                if (Scto != null)
                {
                    var email = Scto.SCTO_Email;
                    var name = Scto.SCTO;
                    using (var message = new MailMessage("donotreply@medicalaccess.co.ug", email/*"wokwerahenry@gmail.com"*/))
                    {
                        message.Subject = "New Order Sent" + " on" + " " + DateTime.Now.ToLongDateString();
                        message.Body = "Dear " + name + ",<br/> <br/>" + "A new " + Category + " Order with Ordernumber: " + OrderNumber + " has been submitted by " + facility.Facility + "  on " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString() + "  <br/> <br/>" + "Please Log onto the" + "   <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Website</a>  to review order. <br/><br/> Regards, <br/><br/> Medical Access Uganda limited";
                        message.IsBodyHtml = true;
                        //for (int i = 0; i < Email.Count(); i++)
                        //{
                        //    message.Bcc.Add(Email[i]);
                        //}
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = true,
                            //Host = "smtp.gmail.com",
                            Host = "smtp.office365.com",
                            //Port = 587,
                            Port = 587,

                            Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                        })
                        {
                            //client.EnableSsl = true;
                            client.Send(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public void OrderReSentToScto(int? facilitycode, string OrderNumber, string category)
        {
            try
            {
                var Scto = dbcontext.Chss_M_SCTO_Contact.FirstOrDefault(o => o.FacilityCode == facilitycode);
                var facility = dbcontext.A_Facilities.FirstOrDefault(o => o.FacilityCode == facilitycode);
                if (Scto != null)
                {
                    var email = Scto.SCTO_Email;
                    var name = Scto.SCTO;
                    using (var message = new MailMessage("donotreply@medicalaccess.co.ug", email/*"wokwerahenry@gmail.com"*/))
                    {
                        message.Subject = "Order Resubmitted" + " on" + " " + DateTime.Now.ToLongDateString();
                        message.Body = "Dear " + name + ",<br/> <br/>" + "An " + category + " Order with Ordernumber: " + OrderNumber + " was resubmitted by " + facility.Facility + "  on " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString() + "  <br/> <br/>" + "Please Log onto the" + "   <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Website</a> to review order. <br/><br/> Regards, <br/><br/> Medical Access Uganda limited";
                        message.IsBodyHtml = true;
                        //for (int i = 0; i < Email.Count(); i++)
                        //{
                        //    message.Bcc.Add(Email[i]);
                        //}
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = true,
                            //Host = "smtp.gmail.com",
                            Host = "smtp.office365.com",
                            //Port = 587,
                            Port = 587,

                            Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                        })
                        {
                            //client.EnableSsl = true;
                            client.Send(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public void OrderRejected(int? facilitycode, string OrderNumber, string category)
        {
            UserManagement obj = new UserManagement();

            var email = obj.getCurrentFacilityEmail(facilitycode);
            //if email is null , no facility tagged to email
            var count = obj.getCurrentFacilityEmail(facilitycode).Count();
            //if email is null , no facility tagged to email

            if (count == 0)
            {
                var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                email = emailsss;// = "customercare@medicalaccess.co.ug";
            }

            var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
            using (var message = new MailMessage("donotreply@medicalaccess.co.ug", email[0]))
            {
                // foreach (staWebTemplate_RecipientEmail objxx in objx)

                message.Subject = "Order Rejected" + " on" + " " + DateTime.Now.ToLongDateString();
                message.Body = "Dear  " + "" + fc + ",<br/><br/>" + "Kindly be notified that Medical Access Uganda Ltd has Rejected your " + category + " order " + OrderNumber + ".<br/> Please contact us should you have any questions regarding your order on:-<br/> Tel: +256759207240.<br/> Email: customercare@medicalaccess.co.ug <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to address comments raised by your SCTO.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                message.IsBodyHtml = true;

                // message..Add("simonbunya@gmail.com");
                for (int i = 1; i < email.Count(); i++)
                {
                    message.CC.Add(email[i]);
                }
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    //Host = "smtp.gmail.com",
                    Host = "smtp.office365.com",
                    //Port = 587,
                    Port = 587,

                    Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                })
                {
                    //client.EnableSsl = true;
                    client.Send(message);
                }
            }

        }
        public void OrderAccepted(int? facilitycode, string OrderNumber, string category)
        {
            //UserManagement obj = new UserManagement();

            //var email = obj.getCurrentFacilityEmail(facilitycode);
            ////if email is null , no facility tagged to email
            //var count = obj.getCurrentFacilityEmail(facilitycode).Count();
            ////if email is null , no facility tagged to email

            //if (count == 0)
            //{
            //    var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
            //    email = emailsss;// = "customercare@medicalaccess.co.ug";
            //}
            var email = new List<string>() { "suubi.lubega@medicalaccess.co.ug", "josephine.nakyeyune@medicalaccess.co.ug", "betty.basasibwaki@medicalaccess.co.ug", "timothy.kasule@medicalaccess.co.ug", "fortunate.naamara@medicalaccess.co.ug", "george.kigozi@medicalaccess.co.ug", "christopher.ssekandi@medicalaccess.co.ug", "racheal.twikirize@medicalaccess.co.ug" };
            var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
            using (var message = new MailMessage("donotreply@medicalaccess.co.ug", email[0]))
            {
                // foreach (staWebTemplate_RecipientEmail objxx in objx)

                message.Subject = "Order Rejected" + " on" + " " + DateTime.Now.ToLongDateString();
                message.Body = "Dear LMIS Team,< br/><br/>" + "Kindly be notified that a/an " + category + " order " + OrderNumber + " For " + fc + " has successfully been submitted on " + DateTime.Now + ".<br/> Please logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited web portal<a/> to process the order.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                message.IsBodyHtml = true;

                // message..Add("simonbunya@gmail.com");
                for (int i = 1; i < email.Count(); i++)
                {
                    message.CC.Add(email[i]);
                }
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    //Host = "smtp.gmail.com",
                    Host = "smtp.office365.com",
                    //Port = 587,
                    Port = 587,

                    Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                })
                {
                    //client.EnableSsl = true;
                    client.Send(message);
                }
            }

        }
        // SUCCESSFULLY EXPORTING AT SAP
        public void SendEmailSAP(int? facilitycode, string OrderNumber)
        {
            try
            {
                UserManagement obj = new UserManagement();

                var email = dbcontext.View_WebTemplate_RecipientEmail.Where(e => e.FacilityCode == facilitycode && (e.DisableComfirmed == false || e.DisableComfirmed == null) && (e.DisableAll == false || e.DisableAll == null) && e.IsActive == true).ToList();
                var count = obj.getCurrentFacilityEmail(facilitycode).Count();
                //if email is null , no facility tagged to email
                //if (count == 0)
                //{
                //    var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                //    email = emailsss;// = "customercare@medicalaccess.co.ug";
                //}
                var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
                foreach (var n in email)
                {
                    using (var message = new MailMessage("donotreply@medicalaccess.co.ug", n.ce_email))
                    {
                        // foreach (staWebTemplate_RecipientEmail objxx in objx)

                        message.Subject = "Order Confirmed, Order Number: " + OrderNumber + " " + "on" + " " + DateTime.Now.ToLongDateString();
                        message.Body = "Dear  " + "" + fc + ",<br/><br/>" + "Kindly be notified that Medical Access Uganda Ltd has confirmed your order " + OrderNumber + ".<br/> Please contact us should you have any questions regarding your order on:-<br/> Tel: +256759207240.<br/> Email: customercare@medicalaccess.co.ug <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to track you order.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now + "<br/><br/><hr/> If You don't want to receive these emails from Medical Access Uganda Limited in the future, Please <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=2'> Unsubscribe</a> Or  <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=6'> Unsubscribe all</a>";
                        message.IsBodyHtml = true;

                        // message..Add("simonbunya@gmail.com");
                        //for (int i = 1; i < email.Count(); i++)
                        //{
                        //    message.CC.Add("customercare@medicalaccess.co.ug");
                        //}
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = true,
                            //Host = "smtp.gmail.com",
                            Host = "smtp.office365.com",
                            //Port = 587,
                            Port = 587,

                            Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                        })
                        {
                            //client.EnableSsl = true;
                            client.Send(message);
                        }
                    }
                }


            }
            catch
            {

            }
           
        }

        /// <summary>
        /// Order Procced notifications
        /// </summary>
        /// 
        public void SendEmailOrderProcced(int? facilitycode, string OrderNumber)
        {
            try
            {
                UserManagement obj = new UserManagement();

                var email = dbcontext.View_WebTemplate_RecipientEmail.Where(e => e.FacilityCode == facilitycode && (e.DisableProcessed == false || e.DisableProcessed == null) && (e.DisableAll == false || e.DisableAll == null) && e.IsActive == true).ToList();
                var count = obj.getCurrentFacilityEmail(facilitycode).Count();
                //if email is null , no facility tagged to email
                //if (count == 0)
                //{
                //    var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                //    email = emailsss;// = "customercare@medicalaccess.co.ug";
                //}
                var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
                foreach (var n in email)
                {
                    using (var message = new MailMessage("donotreply@medicalaccess.co.ug", n.ce_email))
                    {
                        // foreach (staWebTemplate_RecipientEmail objxx in objx)

                        message.Subject = "Order Processed, Order Number: " + OrderNumber + " " + "on" + " " + DateTime.Now.ToLongDateString();
                        message.Body = "Dear  " + "" + fc + ",<br/><br/>" + "Kindly be notified that Medical Access Uganda Ltd has Processed your order " + OrderNumber + ".<br/> Please contact us should you have any questions regarding your order on:-<br/> Tel: +256759207240.<br/> Email: customercare@medicalaccess.co.ug <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to track you order.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now + "<br/><br/><hr/> If You don't want to receive these emails from Medical Access Uganda Limited in the future, Please <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=3'> Unsubscribe</a> Or  <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=6'> Unsubscribe all</a>";
                        message.IsBodyHtml = true;

                        // message..Add("simonbunya@gmail.com");
                        //for (int i = 1; i < email.Count(); i++)
                        //{
                        //    message.CC.Add("customercare@medicalaccess.co.ug");
                        //}
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = true,
                            //Host = "smtp.gmail.com",
                            Host = "smtp.office365.com",
                            //Port = 587,
                            Port = 587,

                            Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                        })
                        {
                            //client.EnableSsl = true;
                            client.Send(message);
                        }
                    }
                }


            }
            catch
            {

            }

        }

        /// <summary>
        /// order disputed notification
        /// 
        /// </summary>
        /// 
        public void SendEmailOrderDispatched(int? facilitycode, string OrderNumber)
        {
            try
            {
                UserManagement obj = new UserManagement();

                var email = dbcontext.View_WebTemplate_RecipientEmail.Where(e => e.FacilityCode == facilitycode && (e.DisableDispatched == false || e.DisableDispatched == null) && (e.DisableAll == false || e.DisableAll == null) && e.IsActive == true).ToList();
                var count = obj.getCurrentFacilityEmail(facilitycode).Count();
                //if email is null , no facility tagged to email
                //if (count == 0)
                //{
                //    var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                //    email = emailsss;// = "customercare@medicalaccess.co.ug";
                //}
                var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
                foreach (var n in email)
                {
                    using (var message = new MailMessage("donotreply@medicalaccess.co.ug", n.ce_email))
                    {
                        // foreach (staWebTemplate_RecipientEmail objxx in objx)

                        message.Subject = "Order Dispatched, Order Number: " + OrderNumber + " " + "on" + " " + DateTime.Now.ToLongDateString();
                        message.Body = "Dear  " + "" + fc + ",<br/><br/>" + "Kindly be notified that Medical Access Uganda Ltd has Dispatched your order " + OrderNumber + ".<br/> Please contact us should you have any questions regarding your order on:-<br/> Tel: +256759207240.<br/> Email: customercare@medicalaccess.co.ug <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to track you order.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now + "<br/><br/><hr/> If You don't want to receive these emails from Medical Access Uganda Limited in the future, Please <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=4'> Unsubscribe</a> Or  <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=6'> Unsubscribe all</a>";
                        message.IsBodyHtml = true;

                        // message..Add("simonbunya@gmail.com");
                        //for (int i = 1; i < email.Count(); i++)
                        //{
                        //    message.CC.Add("customercare@medicalaccess.co.ug");
                        //}
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = true,
                            //Host = "smtp.gmail.com",
                            Host = "smtp.office365.com",
                            //Port = 587,
                            Port = 587,

                            Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                        })
                        {
                            //client.EnableSsl = true;
                            client.Send(message);
                        }
                    }
                }


            }
            catch
            {

            }
           
        }

        public void SendEmailCCU(int? facilitycode, string comp_no)
        {
            UserManagement obj = new UserManagement();

            //var email = obj.getCurrentFacilityEmail(facilitycode);
            var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
            using (var message = new MailMessage("donotreply@medicalaccess.co.ug", "customercare@medicalaccess.co.ug"))
            {
                // foreach (staWebTemplate_RecipientEmail objxx in objx)

                message.Subject = "New complaint received " + "on" + " " + DateTime.Now.ToLongDateString();
                message.Body = "Dear  Medical Access," + ",<br/><br/>" + "Kindly be notified that  " + fc + "   has sent a new complaint, complaint number: " + comp_no + " <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to view the complaint.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                message.IsBodyHtml = true;
                //message.CC.Add("betty.basasibwaki@medicalaccess.co.ug");
                // message..Add("simonbunya@gmail.com");
                //for (int i = 1; i < email.Count(); i++)
                //{
                //    message.CC.Add(email[i]);
                //}
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    //Host = "smtp.gmail.com",
                    Host = "smtp.office365.com",
                    //Port = 587,
                    Port = 587,

                    Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                })
                {
                    //client.EnableSsl = true;
                    client.Send(message);
                }
            }

        }
        public void SendEmailFacilityCCU(int? facilitycode, string comp_no)
        {
            UserManagement obj = new UserManagement();
            var _email = dbcontext.View_WebTemplate_RecipientEmail.Where(e => e.FacilityCode == facilitycode && (e.DisableAll == false || e.DisableAll == null) && e.IsActive == true).ToList();
            List<string> email = new List<string>();
            var count = _email.Count();
            //if email is null , no facility tagged to email

            if (count == 0)
            {
                var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                email = emailsss;// = "customercare@medicalaccess.co.ug";
            }
            if (count > 0)
            {
                foreach(var n in _email)
                {
                    email.Add(n.ce_email);
                }
            }
            var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
            if (email.Count > 0)
            {
                using (var message = new MailMessage("donotreply@medicalaccess.co.ug", /*"customercare@medicalaccess.co.ug"*/ email[0]))
                {
                    // foreach (staWebTemplate_RecipientEmail objxx in objx)

                    message.Subject = "New complaint received " + "on" + " " + DateTime.Now.ToLongDateString();
                    message.Body = "Dear  " + fc + "," + ",<br/><br/>" + "Kindly be notified that  your complaint, complaint number: " + comp_no + " has been sent and received by Medical Access Uganda Limited <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to track your complaint.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                    message.IsBodyHtml = true;

                    email.Add("customercare@medicalaccess.co.ug");
                    //email.Add("betty.basasibwaki@medicalaccess.co.ug");
                    for (int i = 1; i < email.Count(); i++)
                    {
                        message.CC.Add(email[i]);
                    }
                    using (SmtpClient client = new SmtpClient
                    {
                        EnableSsl = true,
                        //Host = "smtp.gmail.com",
                        Host = "smtp.office365.com",
                        //Port = 587,
                        Port = 587,

                        Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                    })
                    {
                        //client.EnableSsl = true;
                        client.Send(message);
                    }
                }
            }


        }
        public void SendEmailSchedular(string staff, string venue, DateTime start_time, DateTime end_time, int? Event_Category)
        {
            var day = start_time.ToString("dddd");
            var month = start_time.ToString("MMMM");
            var year = start_time.Year;
            var meetingVenue ="";
            var venue2 = Convert.ToInt32(venue);//=
            var EventCategory = "";
            if (meetingVenue != null)
            {
                meetingVenue = dbcontext.A_Venue.FirstOrDefault(a => a.id == venue2).VenueName; 
            }
            else
            {
                meetingVenue = "Nakasero";
            }

            if (Event_Category != null)
            {
                EventCategory = dbcontext.A_EventCategory.FirstOrDefault(a => a.CategoryId == Event_Category).CategoryDesc;
            }
            else
            {
                EventCategory = "Meeting";
            }
            UserManagement obj = new UserManagement();
            List<string> email = new List<string>();
            string[] b;
            if (staff.Contains(","))
            {
                b = staff.Split(',');
            }
            else { b = new string[] { staff }; }
            foreach (var n in b)
            {
                if (!n.Contains(","))
                {
                    int cp_code = Convert.ToInt32(n);
                    var a = dbcontext.View_fo_Contacts.FirstOrDefault(o => o.cp_code == cp_code && (o.DisableAll == false || o.DisableAll == null) && o.IsActive == true);
                    if (!string.IsNullOrEmpty(a.ce_email))
                    {
                        using (var message = new MailMessage("donotreply@medicalaccess.co.ug", a.ce_email))
                        {
                            message.Subject = "New Scheduler assigned " + "on" + " " + DateTime.Now.ToLongDateString();
                            message.Body = "Dear  " + a.cp_name + "," + "<br/><br/>" + "Kindly be notified that a meeting on "+ EventCategory + " will be held in: " + meetingVenue + " on " + day + " of "+ month +" "+year +" Starting from: "+start_time+" up to " + end_time +"."+ " <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal for more information about this meeting.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                            message.IsBodyHtml = true;
                            message.CC.Add("customercare@medicalaccess.co.ug");
                            message.CC.Add("betty.basasibwaki@medicalaccess.co.ug");
                            using (SmtpClient client = new SmtpClient
                            {
                                EnableSsl = true,
                                //Host = "smtp.gmail.com",
                                Host = "smtp.office365.com",
                                Port = 587,

                                Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                            })
                            {
                                client.Send(message);
                            }
                        }
                    }
                }

            }

        }

        public void SendEmailLev1(string level1, string comp_no)
        {
            try
            {
                UserManagement obj = new UserManagement();
                List<string> email = new List<string>();
                string[] b;
                if (level1.Contains(","))
                {
                    b = level1.Split(',');
                }
                else { b = new string[] { level1 }; }
                foreach (var n in b)
                {
                    if (!n.Contains(","))
                    {
                        int cp_code = Convert.ToInt32(n);
                        var a = dbcontext.View_fo_Contacts.FirstOrDefault(o => o.cp_code == cp_code && (o.DisableAll == false || o.DisableAll == null) && o.IsActive==true);
                        if (!string.IsNullOrEmpty(a.ce_email))
                        {
                            using (var message = new MailMessage("donotreply@medicalaccess.co.ug", /*"customercare@medicalaccess.co.ug"*/ a.ce_email))
                            {
                                message.Subject = "New complaint assigned " + "on" + " " + DateTime.Now.ToLongDateString();
                                message.Body = "Dear  " + a.cp_name + "," + ",<br/><br/>" + "Kindly be notified that  a complaint with the complaint number: " + comp_no + " has been assigned to you on " + DateTime.Now.ToLongDateString() + " <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to  to address the complaint.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                                message.IsBodyHtml = true;
                                message.CC.Add("customercare@medicalaccess.co.ug");
                                message.CC.Add("betty.basasibwaki@medicalaccess.co.ug");
                                using (SmtpClient client = new SmtpClient
                                {
                                    EnableSsl = true,
                                    //Host = "smtp.gmail.com",
                                    Host = "smtp.office365.com",
                                    Port = 587,

                                    Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                                })
                                {
                                    client.Send(message);
                                }
                            }
                        }
                    }

                }
            }
            catch
            {
               
            }
            
        }
        public void SendEmailLev2(string level1, string comp_no)
        {
            try
            {
                UserManagement obj = new UserManagement();
                List<string> email = new List<string>();
                string[] b;
                if (level1.Contains(","))
                {
                    b = level1.Split(',');
                }
                else { b = new string[] { level1 }; }
                foreach (var n in b)
                {
                    if (!n.Contains(","))
                    {
                        int cp_code = Convert.ToInt32(n);
                        var a = dbcontext.View_fo_Contacts.FirstOrDefault(o => o.cp_code == cp_code && (o.DisableAll == false || o.DisableAll == null) && o.IsActive == true);
                        if (!string.IsNullOrEmpty(a.ce_email))
                        {
                            using (var message = new MailMessage("donotreply@medicalaccess.co.ug", /*"customercare@medicalaccess.co.ug"*/ a.ce_email))
                            {
                                message.Subject = "New complaint assigned " + "on" + " " + DateTime.Now.ToLongDateString();
                                message.Body = "Dear  " + a.cp_name + "," + ",<br/><br/>" + "Kindly be notified that  a complaint with the complaint number: " + comp_no + " has been assigned to you on " + DateTime.Now.ToLongDateString() + " <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to  to address the complaint.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                                message.IsBodyHtml = true;
                                message.CC.Add("customercare@medicalaccess.co.ug");
                                message.CC.Add("betty.basasibwaki@medicalaccess.co.ug");
                                using (SmtpClient client = new SmtpClient
                                {
                                    EnableSsl = true,
                                    //Host = "smtp.gmail.com",
                                    Host = "smtp.office365.com",
                                    Port = 587,

                                    Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                                })
                                {
                                    client.Send(message);
                                }
                            }
                        }
                    }

                }
            }
            catch
            {

            }
            
        }
        public void SendEmailQA(int? facilitycode, string comp_no)
        {
            UserManagement obj = new UserManagement();

            //var email = obj.getCurrentFacilityEmail(facilitycode);
            var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
            using (var message = new MailMessage("donotreply@medicalaccess.co.ug", "qa@medicalaccess.co.ug"))
            {
                // foreach (staWebTemplate_RecipientEmail objxx in objx)

                message.Subject = "New complaint received " + "on" + " " + DateTime.Now.ToLongDateString();
                message.Body = "Dear  Quality Assurance Team," + ",<br/><br/>" + "Kindly be notified that  " + fc + "   has sent a new Product Quality complaint with a complaint number: " + comp_no + " <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to view the complaint.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                message.IsBodyHtml = true;
                message.CC.Add("betty.basasibwaki@medicalaccess.co.ug");
                message.CC.Add("customercare@medicalaccess.co.ug");
                //for (int i = 1; i < email.Count(); i++)
                //{
                //    message.CC.Add(email[i]);
                //}
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    //Host = "smtp.gmail.com",
                    Host = "smtp.office365.com",
                    //Port = 587,
                    Port = 587,

                    Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                })
                {
                    //client.EnableSsl = true;
                    client.Send(message);
                }
            }

        }
        public void SendEmailComplaintResolved(string level1, string comp_no)
        {
            UserManagement obj = new UserManagement();
            List<string> email = new List<string>();
            string[] b;
            if (level1.Contains(","))
            {
                b = level1.Split(',');
            }
            else { b = new string[] { level1 }; }
            foreach (var n in b)
            {
                if (!n.Contains(","))
                {
                    var a = obj.getCurrentFacilityEmail(Convert.ToInt32(n));
                    var facilitycode = Convert.ToInt32(n);
                    var fc = dbcontext.A_Facilities.FirstOrDefault(o => o.FacilityCode == facilitycode).Facility;
                    //var a = dbcontext.View_fo_Contacts.FirstOrDefault(o => o.cp_code == Convert.ToInt32(n));
                    if (a.Count > 0)
                    {
                        using (var message = new MailMessage("donotreply@medicalaccess.co.ug", /*"customercare@medicalaccess.co.ug"*/ a[0]))
                        {
                            message.Subject = "New complaint assigned " + "on" + " " + DateTime.Now.ToLongDateString();
                            message.Body = "Dear  " + fc + "," + ",<br/><br/>" + "Kindly be notified that your complaint, complaint number: " + comp_no + " was resolved on " + DateTime.Now.ToLongDateString() + " <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to track your complaint.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now;
                            message.IsBodyHtml = true;
                            message.CC.Add("customercare@medicalaccess.co.ug");
                            message.CC.Add("betty.basasibwaki@medicalaccess.co.ug");
                            for (int i = 1; i < email.Count(); i++)
                            {
                                message.CC.Add(email[i]);
                            }
                            using (SmtpClient client = new SmtpClient
                            {
                                EnableSsl = true,
                                //Host = "smtp.gmail.com",
                                Host = "smtp.office365.com",
                                Port = 587,

                                Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                            })
                            {
                                client.Send(message);
                            }
                        }
                    }
                }

            }

        }
        /// <summary>
        /// order Delivered Notification 
        /// </summary>
        /// 
        public void SendEmailOrderDelivered(int? facilitycode, string OrderNumber)
        {

            try
            {
                UserManagement obj = new UserManagement();

                var email = dbcontext.View_WebTemplate_RecipientEmail.Where(e => e.FacilityCode == facilitycode && (e.DisableDelivered == false || e.DisableDelivered == null) && (e.DisableAll == false || e.DisableAll == null) && e.IsActive == true).ToList();
                var count = obj.getCurrentFacilityEmail(facilitycode).Count();
                //if email is null , no facility tagged to email
                //if (count == 0)
                //{
                //    var emailsss = new List<string>() { "customercare@medicalaccess.co.ug" };
                //    email = emailsss;// = "customercare@medicalaccess.co.ug";
                //}
                var fc = dbcontext.A_Facilities.FirstOrDefault(a => a.FacilityCode == facilitycode).Facility;
                foreach (var n in email)
                {
                    using (var message = new MailMessage("donotreply@medicalaccess.co.ug", n.ce_email))
                    {
                        // foreach (staWebTemplate_RecipientEmail objxx in objx)

                        message.Subject = "Order Delivered, Order Number: " + OrderNumber + " " + "on" + " " + DateTime.Now.ToLongDateString();
                        message.Body = "Dear  " + "" + fc + ",<br/><br/>" + "Kindly be notified that Medical Access Uganda Ltd has Delivered your order " + OrderNumber + ".<br/> Please contact us should you have any questions regarding your order on:-<br/> Tel: +256759207240.<br/> Email: customercare@medicalaccess.co.ug <br/><br/> You can also logon to the  <a href='http://10.33.0.15/mascis/Account/Login'>Medical Access Uganda Limited<a/> web portal to track you order.<br/><br/> Regards, <br/> Medical Access Uganda Limited.<br/><br/>" + DateTime.Now + "<br/><br/><hr/> If You don't want to receive these emails from Medical Access Uganda Limited in the future, Please <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=5'> Unsubscribe</a> Or  <a href='http://mobapp.medicalaccess.co.ug:5555/Unsubscribe/Index?cp_Code=" + n.cp_code + "&Category=6'> Unsubscribe all</a>";
                        message.IsBodyHtml = true;

                        // message..Add("simonbunya@gmail.com");
                        //for (int i = 1; i < email.Count(); i++)
                        //{
                        //    message.CC.Add("customercare@medicalaccess.co.ug");
                        //}
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = true,
                            //Host = "smtp.gmail.com",
                            Host = "smtp.office365.com",
                            //Port = 587,
                            Port = 587,

                            Credentials = new NetworkCredential("donotreply@medicalaccess.co.ug", "Kampala2018")
                        })
                        {
                            //client.EnableSsl = true;
                            client.Send(message);
                        }
                    }
                }


            }
            catch
            {

            }
        }
        public void Execute()
        {
            //Job implementation code: send emails to users and update database
        }
    }

    public class MonitorService
    {
        OrderLaboratoryHeaderModels labroutine = new OrderLaboratoryHeaderModels();
        OrderLaboratoryCustomHeaderModels labcustom = new OrderLaboratoryCustomHeaderModels();
        OrderARTHeaderModels art = new OrderARTHeaderModels();
        OrderHIVTestKitHeaderModels hiv = new OrderHIVTestKitHeaderModels();
        OrderOISTIHeaderModels ois = new OrderOISTIHeaderModels();
        OrderRutfHeaderModels ruft = new OrderRutfHeaderModels();
        OrderSMCHeaderModels smc = new OrderSMCHeaderModels();
        OrderTBHeaderModels tb = new OrderTBHeaderModels();
        OrderViralLoadReagentsHeaderModels vl = new OrderViralLoadReagentsHeaderModels();

        //public MonitorService(IHubConnectionContext<dynamic> clients) : base(clients)
        //{
        //}

        // Singleton instance
        private readonly static Lazy<MonitorService> _instance = new Lazy<MonitorService>(
            () => new MonitorService(GlobalHost.ConnectionManager.GetHubContext<MonitorHub>().Clients));

        private static SqlTableDependency<SAP_ExportHeader> _tableDependency;
        private static SqlTableDependency<SAP_MonitorOrder> _SAP;
        private static SqlTableDependency<SAP_MonitorOrder> _SAP_Dispatched;
        private static SqlTableDependency<SAP_MonitorOrder> _SAP_Delivered;

        public void NotificationService()
        {
            //Clients = clients;

            //var mapper = new ModelToTableMapper<SAP_ExportHeader>();
            //mapper.AddMapping(s => s.OrderNumber, "OrderNumber");

            //_tableDependency = new SqlTableDependency<SAP_ExportHeader>(
            //    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
            //    "SAP_ExportHeader"/*, mapper*/);

            //_tableDependency.OnChanged += SqlTableDependency_Changed;
            //_tableDependency.OnError += SqlTableDependency_OnError;
            //  _tableDependency.Start();
        }

        private MonitorService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;

            var mapper = new ModelToTableMapper<SAP_ExportHeader>();
            mapper.AddMapping(s => s.OrderNumber, "OrderNumber");

            _tableDependency = new SqlTableDependency<SAP_ExportHeader>(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                "SAP_ExportHeader"/*, mapper*/);
            //sap connection
            _SAP = new SqlTableDependency<SAP_MonitorOrder>(
              ConfigurationManager.ConnectionStrings["DefaultSAPConnection"].ConnectionString,
              "order_status");
            _SAP_Dispatched = new SqlTableDependency<SAP_MonitorOrder>(
             ConfigurationManager.ConnectionStrings["DefaultSAPConnection"].ConnectionString,
             "order_status");
            _SAP_Delivered = new SqlTableDependency<SAP_MonitorOrder>(
             ConfigurationManager.ConnectionStrings["DefaultSAPConnection"].ConnectionString,
             "order_status");
            //connection string for order confirmed
            _tableDependency.OnChanged += SqlTableDependency_Changed;
            _tableDependency.OnError += SqlTableDependency_OnError;
            _tableDependency.Start();

            //connection string for sap OrderProcessed_Changed
            _SAP.OnChanged += OrderProcessed_Changed;
            _SAP.OnError += SqlTableDependency_OnError;
            _SAP.Start();

            // connection string for sap OrderDispatched_Changed
            _SAP_Dispatched.OnChanged += OrderDispatched_Changed;
            _SAP_Dispatched.OnError += SqlTableDependency_OnError;
            _SAP_Dispatched.Start();

            //connection string for sap OrderDelivered_Changed
            _SAP_Delivered.OnChanged += OrderDelivered_Changed;
            _SAP_Delivered.OnError += SqlTableDependency_OnError;
            _SAP_Delivered.Start();

        }

        public static MonitorService Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public IEnumerable<SAP_ExportHeader> GetAllStocks()
        {
            var stockModel = new List<SAP_ExportHeader>();

            var connectionString = ConfigurationManager.ConnectionStrings
                    ["DefaultConnection"].ConnectionString;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [SAP_ExportHeader]";

                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var OrderNumber = sqlDataReader.GetString(sqlDataReader.GetOrdinal("OrderNumber"));
                            var RecordReadBySAP = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("RecordReadBySAP"));
                            var DocNum = sqlDataReader.GetString(sqlDataReader.GetOrdinal("DocNum"));

                            stockModel.Add(new SAP_ExportHeader { OrderNumber = OrderNumber, RecordReadBySAP = Convert.ToBoolean(RecordReadBySAP), DocNum = DocNum });
                        }
                    }
                }
            }

            return stockModel;
        }

        void SqlTableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            throw e.Error;
        }

        /// <summary>
        /// Broadcast New Stock Price
        /// </summary>
        /// Order confirmed
        void SqlTableDependency_Changed(object sender, RecordChangedEventArgs<SAP_ExportHeader> e)
        {
            //public DbContext Context { get; } // we need this dependency, right?!
            mascisEntities dbcontext = new mascisEntities();

            if (e.ChangeType != ChangeType.None)
            {
                BroadcastStockPrice(e.Entity);

                var changedEntity = e.Entity;
                EmailJob emails = new EmailJob();
                // Get record values from SQL Server notification
                int? facility = dbcontext.A_Facilities.FirstOrDefault(a => a.SAP_Code == changedEntity.CardCode).FacilityCode;

                if (changedEntity.RecordReadBySAP == true)
                {
                   
                    // to update the order status of the record
                    var sapcode = changedEntity.OrderNumber;
                    // Get first three characters.
                    string sub = sapcode.Substring(2, 6);
                    string product_category = sapcode.Substring(14, 2);
                    try
                    {
                        // string _number = "";
                        switch (product_category)
                        {
                            case "01":
                                labroutine.UpdateSAP(sapcode);
                                labcustom.UpdateSAP(sapcode);
                                break;
                            case "02":
                                art.UpdateSAP(sapcode);
                                break;
                            case "03":
                                hiv.UpdateSAP(sapcode);
                                break;
                            case "09":
                                ois.UpdateSAP(sapcode);
                                break;
                            case "10":
                                smc.UpdateSAP(sapcode);
                                break;
                            case "11":
                                vl.UpdateSAP(sapcode);
                                break;
                            case "12":
                                ruft.UpdateSAP(sapcode);
                                break;
                            case "13":
                                tb.UpdateSAP(sapcode);
                                break;
                        }
                        emails.SendEmailSAP(facility, changedEntity.OrderNumber);
                    }
                    catch (Exception x)
                    {
                        throw (x);
                    }
                    
                }

                // emails.SendEmail()

            }
        }

        //Order Processed

        void OrderProcessed_Changed(object sender, RecordChangedEventArgs<SAP_MonitorOrder> e)
        {
            //public DbContext Context { get; } // we need this dependency, right?!
            mascisEntities dbcontext = new mascisEntities();

            if (e.ChangeType != ChangeType.None)
            {
                SAP(e.Entity);

                var changedEntity = e.Entity;
                var sapcode = changedEntity.order_number;
                // Get first three characters.
                string sub = sapcode.Substring(2, 6);
                string product_category = sapcode.Substring(14, 2);

                EmailJob emails = new EmailJob();
                // Get record values from SQL Server notification
                int? facility = dbcontext.A_Facilities.FirstOrDefault(a => a.SAP_Code == sub).FacilityCode;

                if (changedEntity.status_id == 7)
                {
                    
                    //update order tables
                    try
                    {
                        // string _number = "";
                        switch (product_category)
                        {
                            case "01":
                                labroutine.UpdateOrderProcessed(sapcode);
                                labcustom.UpdateOrderProcessed(sapcode);
                                break;
                            case "02":
                                art.UpdateOrderProcessed(sapcode);
                                break;
                            case "03":
                                hiv.UpdateOrderProcessed(sapcode);
                                break;
                            case "09":
                                ois.UpdateOrderProcessed(sapcode);
                                break;
                            case "10":
                                smc.UpdateOrderProcessed(sapcode);
                                break;
                            case "11":
                                vl.UpdateOrderProcessed(sapcode);
                                break;
                            case "12":
                                ruft.UpdateOrderProcessed(sapcode);
                                break;
                            case "13":
                                tb.UpdateOrderProcessed(sapcode);
                                break;
                        }
                        emails.SendEmailOrderProcced(facility, changedEntity.order_number);
                    }
                    catch (Exception x)
                    {
                        throw (x);
                    }
                   
                }

                // emails.SendEmail()

            }
        }

        //Order Dispatched

        void OrderDispatched_Changed(object sender, RecordChangedEventArgs<SAP_MonitorOrder> e)
        {
            //public DbContext Context { get; } // we need this dependency, right?!
            mascisEntities dbcontext = new mascisEntities();

            if (e.ChangeType != ChangeType.None)
            {
                SAP(e.Entity);

                var changedEntity = e.Entity;
                var sapcode = changedEntity.order_number;
                // Get first three characters.
                string sub = sapcode.Substring(2, 6);
                string product_category = sapcode.Substring(14, 2);

                EmailJob emails = new EmailJob();
                // Get record values from SQL Server notification
                int? facility = dbcontext.A_Facilities.FirstOrDefault(a => a.SAP_Code == sub).FacilityCode;

                if (changedEntity.status_id == 8)
                {
                    
                    //update order tables
                    try
                    {
                        // string _number = "";
                        switch (product_category)
                        {
                            case "01":
                                labroutine.UpdateOrderDispatched(sapcode);
                                labcustom.UpdateOrderDispatched(sapcode);
                                break;
                            case "02":
                                art.UpdateOrderDispatched(sapcode);
                                break;
                            case "03":
                                hiv.UpdateOrderDispatched(sapcode);
                                break;
                            case "09":
                                ois.UpdateOrderDispatched(sapcode);
                                break;
                            case "10":
                                smc.UpdateOrderDispatched(sapcode);
                                break;
                            case "11":
                                vl.UpdateOrderDispatched(sapcode);
                                break;
                            case "12":
                                ruft.UpdateOrderDispatched(sapcode);
                                break;
                            case "13":
                                tb.UpdateOrderDispatched(sapcode);
                                break;
                        }
                        emails.SendEmailOrderDispatched(facility, changedEntity.order_number);
                    }
                    catch (Exception x)
                    {
                        throw (x);
                    }
                }

                // emails.SendEmail()

            }
        }

        //Order Delivered

        void OrderDelivered_Changed(object sender, RecordChangedEventArgs<SAP_MonitorOrder> e)
        {
            //public DbContext Context { get; } // we need this dependency, right?!
            mascisEntities dbcontext = new mascisEntities();

            if (e.ChangeType != ChangeType.None)
            {
                SAP(e.Entity);

                var changedEntity = e.Entity;
                var sapcode = changedEntity.order_number;
                // Get first three characters.
                string sub = sapcode.Substring(2, 6);
                string product_category = sapcode.Substring(14, 2);

                EmailJob emails = new EmailJob();
                // Get record values from SQL Server notification
                int? facility = dbcontext.A_Facilities.FirstOrDefault(a => a.SAP_Code == sub).FacilityCode;

                if (changedEntity.status_id == 9)
                {
                   
                    //update order tables
                    try
                    {
                        // string _number = "";
                        switch (product_category)
                        {
                            case "01":
                                labroutine.UpdateOrderDelivered(sapcode);
                                labcustom.UpdateOrderDelivered(sapcode);
                                break;
                            case "02":
                                art.UpdateOrderDelivered(sapcode);
                                break;
                            case "03":
                                hiv.UpdateOrderDelivered(sapcode);
                                break;
                            case "09":
                                ois.UpdateOrderDelivered(sapcode);
                                break;
                            case "10":
                                smc.UpdateOrderDelivered(sapcode);
                                break;
                            case "11":
                                vl.UpdateOrderDelivered(sapcode);
                                break;
                            case "12":
                                ruft.UpdateOrderDelivered(sapcode);
                                break;
                            case "13":
                                tb.UpdateOrderDelivered(sapcode);
                                break;
                        }
                        emails.SendEmailOrderDelivered(facility, changedEntity.order_number);
                    }
                    catch (Exception x)
                    {
                        throw (x);
                    }
                   
                }

                // emails.SendEmail()

            }
        }


        private void BroadcastStockPrice(SAP_ExportHeader stock)
        {
            Clients.All.updateStockPrice(stock);
        }

        private void SAP(SAP_MonitorOrder stock)
        {
            Clients.All.updateStockPrice(stock);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tableDependency.Stop();
                }

                disposedValue = true;
            }
        }

        ~MonitorService()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}