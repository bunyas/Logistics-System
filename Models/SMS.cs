using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
using System.ComponentModel.DataAnnotations;
namespace mascis.Models
{
    public class SMS
    {
        [Required]
        [Display(Name = "Send To:")]
        public string ReceipientsNumbers { get; set; }

        public string SenderId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]

        [Display(Name = "Message:")]
        public string Message { get; set; }

        public string Numbers { get; set; }

        
    }
}