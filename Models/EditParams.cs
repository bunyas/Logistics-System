using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    //public class EditParams
    //{
    //    public string key { get; set; }
    //    public string action { get; set; }
    //    public List<eSchedule> added { get; set; }
    //    public List<eSchedule> changed { get; set; }
    //    public eSchedule value { get; set; }
    //}

    public class EditParams
    {
        public string key { get; set; }
        public string action { get; set; }
        public List<ScheduleEvent> added { get; set; }
        public List<ScheduleEvent> changed { get; set; }
        public ScheduleEvent value { get; set; }
    }
}