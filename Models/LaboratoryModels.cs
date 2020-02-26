using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class LaboratoryModels
    {
        mascisEntities context = new mascisEntities();

        public ObjectResult<spView_WebTemplate_LaboratoryGetAll_MA_WFA_Result> GetOrder(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_LaboratoryGetAll_MA_WFA(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_LaboratoryGetAll_Add_Result> GetOrder_Add(int FC, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_WebTemplate_LaboratoryGetAll_Add(null, FC, StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_export_atomic_lab_Physical_CountGetAll_Result> GetPhysicalCount(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_export_atomic_lab_Physical_CountGetAll(StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spSAP_EXPORT_Order_Lab_HeaderGetAll_Result> SAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spSAP_EXPORT_Order_Lab_HeaderGetAll(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_LaboratoryGetAll_Custom1_Result> GetOrderCustom(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_LaboratoryGetAll_Custom1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }


        public ObjectResult<spView_WebTemplate_Lab_CustomGetAll_Result> GetOrderCustom_LAB(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_Lab_CustomGetAll(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //Emergency
        public ObjectResult<spView_lmis_allocation_e_labGetAll_Result> GetAllocationEmergency(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_e_labGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_allocation_n_labGetAll_Result> GetAllocation(int? order_type,int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_labGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_allocation_n_labCustomGetAll_Result> GetAllocationCustom(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_labCustomGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public List<spView_export_atomic_labGetAll_Result> GetAll(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_labGetAll_Result>();
                List<spView_export_atomic_labGetAll_Result> m = new List<spView_export_atomic_labGetAll_Result>();
                var t = context.spView_export_atomic_labGetAll(start_date, end_date).ToList();
                if (emergency == false)
                {
                    x = t.Where(e => (e.EmergencyOrder == false || e.EmergencyOrder == null) && e.StatusId != 5).ToList();
                }
                if (emergency == true)
                {
                    x = t.Where(e => e.EmergencyOrder == true && e.StatusId != 5).ToList();
                }
                if (emergency == null)
                {
                    x = t.Where(e => e.StatusId != 5).ToList();
                }
                //if (x.Count() > 0)
                //{
                //    foreach (var a in x)
                //    {
                //        var b = context.A_Sector_Change.OrderBy(e => e.date_of_change).FirstOrDefault(e => e.date_of_change > a.date_completed && e.facility_code == a.FacilityCode);
                //        if (b != null)
                //        {
                //            a.DeliveryZoneCode = b.Old_sector;
                //            a.ZoneDescription = "Sector " + b.Old_sector;
                //        }
                //        m.Add(a);
                //    }
                //}
                return x;

            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_sap_labGetAll1_Result> GetSAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_sap_labGetAll1(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

    }
}