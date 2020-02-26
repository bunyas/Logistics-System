using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using mascis.Models;
using Microsoft.AspNet.SignalR.Hubs;


namespace mascis.Hubs
{
    [HubName("monitor")]

    public class MonitorHub: Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MonitorHub>();
            context.Clients.All.displayStatus();
        }
        

        private readonly MonitorService _Monitor;
         
        public MonitorHub() : this(MonitorService.Instance)
        {

        }

        public MonitorHub(MonitorService monitor)
        {
            _Monitor = monitor;
        }

        public IEnumerable<SAP_ExportHeader> GetAllStocks()
        {
            return _Monitor.GetAllStocks();
        }

    }

}