using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using mascis.ScheduledTasks;
using System.Data;
using System.Configuration;
using mascis.Hubs;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using TableDependency.SqlClient;
using Microsoft.AspNet.SignalR.Hubs;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace mascis.Controllers
{
    public class MonitorStatusController : Controller
    {
        // GET: MonitorStatus
        //public ActionResult Index()
        //{
        //   // GetStatus();
        //    return View();
        //}


        //SAP_Status

        public ActionResult SAP_Status()
        {
            // GetStatus();
            return View();
        }

        public JsonResult GetStatus()
        {
            using (var sqlConnection = new SqlConnection((ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT  [OrderNumber],[CurrentReccord],[RecordReadBySAP] ,[DocNum] FROM [SAP_ExportHeader]", sqlConnection))
                {
                    //make sure the command object doesnot already have 
                    // a notification obejct associated with it
                    command.Notification = null;

                    SqlDependency dep = new SqlDependency(command);
                    dep.OnChange += new OnChangeEventHandler(dep_OnChange);
                    // dep.OnChange += Dep_OnChange;

                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    var listEmp = reader.Cast<IDataRecord>()
                        .Select(a => new
                        {
                            OrderNumber = (string)a["OrderNumber"],
                            CurrentReccord = (bool)a["CurrentReccord"],
                            RecordReadBySAP = (bool)a["RecordReadBySAP"],
                            DocNum = (string)a["DocNum"],
                        }).ToList();
                    return Json(listEmp , JsonRequestBehavior.AllowGet);
                }


            }
        }

        private void dep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            MonitorHub.Show();
            //this.SqlTableDependency.Stop();
            //throw new NotImplementedException();
        }
         
    }

  
}

  