using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class SAP_MonitorOrder
    {
        public int id { get ;  set  ;  }
        public int? status_id { get ; set ; }
        public DateTime? confirmed_date { get; set; }
         public DateTime? processed_date { get; set; }
         public string order_number { get; set; }
         public DateTime? dispatched_date { get; set; }
        public DateTime? delivered_date { get; set; }
        public Boolean? read_by_mascis { get; set; }
    }
}