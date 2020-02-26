using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class DropdownList
    {
    }
    public class YesNoDropdownlist
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    public class BooleanDropdownList
    {
        public Boolean Id { get; set; }
        public string Description { get; set; }
    }
}