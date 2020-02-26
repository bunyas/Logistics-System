using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using mascis.Models;

namespace mascis.Controllers
{
    public class SMSController : Controller
    {
        private mascisEntities db = new mascisEntities();
        // GET: SMS
        public ActionResult sendSMS()
        {
            SMS sms = new SMS();
            return View(sms);
        }
        public ActionResult Index()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.View_fo_Contacts.Where(o=> o.cp_category==8 ).OrderBy(o=> o.cp_name).ToList();
            ViewBag.Contacts = contacts;
            return View();
        }
        public ActionResult FacilitySms()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var contacts = db.View_fo_Contacts_Full.Where(o => o.cp_category == 3 || o.cp_category == 5).OrderBy(o => o.cp_name).ToList();
            ViewBag.Contacts = contacts;
            return View();
        }
        [HttpPost/*, ActionName("send-sms")*/]
        public ActionResult sendSMS(SMS sms)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    //string username = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsername"]);
                    //string password = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsernamePassword"]);
                    //string senderid = HttpUtility.UrlEncode(sms.SenderId) ;
                    //string sURL = ConfigurationManager.AppSettings["SmsUrl"]; ;

                    //StreamReader objReader;
                    //sms.SenderId = "MAUL";

                    //string msgText = HttpUtility.UrlEncode(sms.Message);
                    //string myURL = string.Format("http://apidocs.speedamobile.com/api/SendSMS?api_id=API4678406659&api_password=MUa334Md&sms_type=T&encoding=T&sender_id=MediaAccess&phonenumber={0}&textmessage={1}",sms.ReceipientsNumbers, msgText);
                    //string xURL = "";

                    //xURL = sURL + "?username=" + username + "&password=" + password + "&from=MAUL" + "&to=" + HttpUtility.UrlEncode(sms.ReceipientsNumbers) + "&text=" + HttpUtility.UrlEncode(sms.Message); 


                    //WebRequest wrGETURL;
                    //wrGETURL = WebRequest.Create(myURL);
                    //try
                    //{
                    //    Stream objStream;
                    //    objStream = wrGETURL.GetResponse().GetResponseStream();
                    //    objReader = new StreamReader(objStream);
                    //    string responseString = objReader.ReadToEnd();
                    //    objReader.Close(); 
                    //    ViewData["SuccessMsg"] = responseString;
                    //}
                    //catch (Exception ex)
                    //{
                    //    ex.ToString();
                    //}


                    if (string.IsNullOrEmpty(sms.ReceipientsNumbers)  || string.IsNullOrEmpty(sms.Message))
                    {
                        ViewData["SuccessMsg"] = "Please select the receipients or enter the message";
                    }
                    else
                    {
                        string username = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsername"]);
                        string password = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsernamePassword"]);
                        //string senderid = "MAUL";
                        string sURL = ConfigurationManager.AppSettings["SmsUrl"]; ;

                        StreamReader objReader;
                        //sms.SenderId = "MAUL";

                        string msgText = HttpUtility.UrlEncode(sms.Message);

                        if (sms.ReceipientsNumbers.Contains(","))
                        {
                            var x = sms.ReceipientsNumbers.Split(',');
                            foreach (var n in x)
                            {
                                string phone = 256 + n;
                                string myURL = string.Format("http://apidocs.speedamobile.com/api/SendSMS?api_id=API4678406659&api_password=MUa334Md&sms_type=T&encoding=T&sender_id=MediaAccess&phonenumber={0}&textmessage={1}", phone, msgText);
                                string xURL = "";

                                xURL = sURL + "?username=" + username + "&password=" + password + "&from=MAUL" + "&to=" + phone + "&text=" + msgText;


                                WebRequest wrGETURL;
                                wrGETURL = WebRequest.Create(myURL);
                                Stream objStream;
                                objStream = wrGETURL.GetResponse().GetResponseStream();
                                objReader = new StreamReader(objStream);
                                string responseString = objReader.ReadToEnd();
                                objReader.Close();
                                ViewData["SuccessMsg"] = responseString;
                            }
                        }
                        else
                        {
                            string phone = 256 + sms.ReceipientsNumbers;
                            string myURL = string.Format("http://apidocs.speedamobile.com/api/SendSMS?api_id=API4678406659&api_password=MUa334Md&sms_type=T&encoding=T&sender_id=MediaAccess&phonenumber={0}&textmessage={1}", phone, msgText);
                            string xURL = "";

                            xURL = sURL + "?username=" + username + "&password=" + password + "&from=MAUL" + "&to=" + phone + "&text=" + msgText;


                            WebRequest wrGETURL;
                            wrGETURL = WebRequest.Create(myURL);
                            Stream objStream;
                            objStream = wrGETURL.GetResponse().GetResponseStream();
                            objReader = new StreamReader(objStream);
                            string responseString = objReader.ReadToEnd();
                            objReader.Close();
                            ViewData["SuccessMsg"] = responseString;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }
        public ActionResult SendMessage(string ReceipientsNumbers, string Message)
        {
            string result = string.Empty;
            try
            {
                if(string.IsNullOrEmpty(ReceipientsNumbers) || ReceipientsNumbers.Contains("null") || string.IsNullOrEmpty(Message) || Message.Contains("null"))
                {
                    result = "Please select the receipients or enter the message";
                }
                else
                {
                    string username = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsername"]);
                    string password = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["SmsUsernamePassword"]);
                    //string senderid = "MAUL";
                    string sURL = ConfigurationManager.AppSettings["SmsUrl"]; ;

                    StreamReader objReader;
                    //sms.SenderId = "MAUL";

                    string msgText = Message;

                    if (ReceipientsNumbers.Contains(","))
                    {
                        var x = ReceipientsNumbers.Split(',');
                        foreach (var n in x)
                        {
                            string phone = 256 + n;
                            string myURL = string.Format("http://apidocs.speedamobile.com/api/SendSMS?api_id=API4678406659&api_password=MUa334Md&sms_type=T&encoding=T&sender_id=MediaAccess&phonenumber={0}&textmessage={1}", phone, msgText);
                            string xURL = "";

                            xURL = sURL + "?username=" + username + "&password=" + password + "&from=MAUL" + "&to=" + phone + "&text=" + msgText;


                            WebRequest wrGETURL;
                            wrGETURL = WebRequest.Create(myURL);
                            Stream objStream;
                            objStream = wrGETURL.GetResponse().GetResponseStream();
                            objReader = new StreamReader(objStream);
                            string responseString = objReader.ReadToEnd();
                            objReader.Close();
                            result = responseString;
                        }
                    }
                    else
                    {
                        string phone = 256 + ReceipientsNumbers;
                        string myURL = string.Format("http://apidocs.speedamobile.com/api/SendSMS?api_id=API4678406659&api_password=MUa334Md&sms_type=T&encoding=T&sender_id=MediaAccess&phonenumber={0}&textmessage={1}", phone, msgText);
                        string xURL = "";

                        xURL = sURL + "?username=" + username + "&password=" + password + "&from=MAUL" + "&to=" + phone + "&text=" + msgText;


                        WebRequest wrGETURL;
                        wrGETURL = WebRequest.Create(myURL);
                        Stream objStream;
                        objStream = wrGETURL.GetResponse().GetResponseStream();
                        objReader = new StreamReader(objStream);
                        string responseString = objReader.ReadToEnd();
                        objReader.Close();
                        result = responseString;
                    }

                }


            }
            catch(Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}