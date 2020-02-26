using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
namespace mascis.Models
{
    public class QualityIssuesModel
    {
        private mascisEntities dbcontext = new mascisEntities();
        public ObjectResult<spfo_complaint_quality_issueGetAll_Result> Quality_Issue(string e_reg_complaint_code,
            string batch_no, int? product_code)
        {
            try
            {
                return dbcontext.spfo_complaint_quality_issueGetAll(e_reg_complaint_code, batch_no, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public class QualityIssue
        {
            public int e_reg_complaint_code { get; set; }
            public string batch_no { get; set; }
            public int product_code { get; set; }
            public string complainant_name { get; set; }
            public Nullable<int> complainant_phone { get; set; }
            public Nullable<int> complainant_title { get; set; }
            public string complainant_email { get; set; }
            public string product_strength { get; set; }
            public string dosage_form { get; set; }
            public string product_other { get; set; }
            public string manufacturer { get; set; }
            public Nullable<System.DateTime> date_quality_issue_identified { get; set; }
            public string description_of_quality_issue { get; set; }
            public Nullable<int> intervention_taken { get; set; }
            public string intervention_taken_other { get; set; }
            public Nullable<bool> attached_picture { get; set; }
            public Nullable<bool> attached_email { get; set; }
            public Nullable<bool> attached_note_letter { get; set; }
            public string attached_other { get; set; }
            public string recipient_name { get; set; }
            public Nullable<int> recipient_title { get; set; }
            public byte[] attached_image_picture { get; set; }
            public string attached_image_email { get; set; }
            public string attached_image_letter { get; set; }
            // public HttpPostedFileBase UploadedFile { get; set; }
            public IEnumerable<HttpPostedFileBase> UploadedFile { get; set; }
            //public IEnumerable<string> UploadedFile { get; set; }
        }

    }
}