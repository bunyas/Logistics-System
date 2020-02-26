using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class ComplaintsModels
    {
        private mascisEntities dbcontext = new mascisEntities();
        public ObjectResult<spfo_complaint_trackerGetAll_Result> Complaint_Tracker(int? e_reg_track_complaint_code,
            int? e_reg_track_code, int? e_reg_track_action_category, int? e_reg_track_status, string e_reg_track_maul_staff, bool? e_reg_track_recordStatus)
        {
            try
            {
                return dbcontext.spfo_complaint_trackerGetAll(e_reg_track_complaint_code, e_reg_track_code, e_reg_track_action_category, e_reg_track_status, e_reg_track_maul_staff, e_reg_track_recordStatus);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spfo_complaintGetAll_Result> GetComplaints(int? e_reg_track_complaint_code)
        {
            try
            {
                return dbcontext.spfo_complaintGetAll(e_reg_track_complaint_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spfo_complaint_quality_issueGetAll_Result> GetQualityComplaints(string e_reg_track_complaint_code, string BatchNo)
        {
            try
            {
                return dbcontext.spfo_complaint_quality_issueGetAll(e_reg_track_complaint_code, BatchNo, null);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        /*@investigation_code nchar(10)
,@Code nvarchar(10)
,@e_reg_complaint_code int
,@yes_no int*/
        public ObjectResult<spvw_fo_investigationGetAll_Result> GetIinvestigation(
            string investigation_code, string Code, string e_reg_track_complaint_code, int? yesno)
        {
            try
            {
                return dbcontext.spvw_fo_investigationGetAll(investigation_code, Code, e_reg_track_complaint_code, yesno);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spvw_fo_investigation_QIGetAll_Result> GetIinvestigation_QI(
            string investigation_code, string Code, string e_reg_track_complaint_code, int? Id, string batch_no, int? product_code)
        {
            try
            {
                return dbcontext.spvw_fo_investigation_QIGetAll(investigation_code, Code, e_reg_track_complaint_code, Id, batch_no, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}