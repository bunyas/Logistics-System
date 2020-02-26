using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class ViralLoadModels
    {
        mascisEntities context = new mascisEntities();

        public ObjectResult<spView_WebTemplate_ViralLoadReagentsGetAll_Result> GetOrder(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ViralLoadReagentsGetAll(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }

        }
        //  public ObjectResult<spView_WebTemplate_SMC_SLM_WebGetAll_Add_Result> GetOrder_Add(int FC, DateTime? StartDate, DateTime? EndDate)
        //{
        //    try
        //    {
        //        return context.spView_WebTemplate_SMC_SLM_WebGetAll_Add(null, FC, StartDate, EndDate);
        //    }
        //    catch (Exception x)
        //    {
        //        throw (x);
        //    }
        //}
        public ObjectResult<spView_WebTemplate_ViralLoadReagentsGetAll_Custom1_Result> GetOrderCustom(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ViralLoadReagentsGetAll_Custom1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }

        }
        public ObjectResult<spSAP_EXPORT_Order_ViralLoadReagents_HeaderGetAll_Result> SAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spSAP_EXPORT_Order_ViralLoadReagents_HeaderGetAll(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //Emergency
        public ObjectResult<spView_lmis_allocation_e_viral_loadGetAll_Result> GetAllocationEmergency(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_e_viral_loadGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_allocation_n_viral_loadGetAll_Result> GetAllocation(int? order_type,int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_viral_loadGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public List<spView_export_atomic_viral_load_reagentsGetAll_Result> GetAll(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_viral_load_reagentsGetAll_Result>();
                var m = new List<spView_export_atomic_viral_load_reagentsGetAll_Result>();
                var t = context.spView_export_atomic_viral_load_reagentsGetAll(start_date, end_date).ToList();
                if (emergency == false)
                {
                    x = t.Where(e => (e.EmergencyOrder == false || e.EmergencyOrder == null) && e.OrderStatusId != 5).ToList();
                }
                if (emergency == true)
                {
                    x = t.Where(e => e.EmergencyOrder == true && e.OrderStatusId != 5).ToList();
                }
                if (emergency == null)
                {
                    x = t.Where(e=> e.OrderStatusId != 5).ToList();
                }
                //if (x.Count() > 0)
                //{
                //    foreach (var a in x)
                //    {
                //        var b = context.A_Sector_Change.OrderBy(e => e.date_of_change).FirstOrDefault(e => e.date_of_change > a.DatePrepared && e.facility_code == a.FacilityCode);
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
        public List<spView_export_atomic_viral_load_reagents_SummaryGetAll_Result> ViralloadSummary(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_viral_load_reagents_SummaryGetAll_Result>();
                var m = new List<spView_export_atomic_viral_load_reagents_SummaryGetAll_Result>();
                var t = context.spView_export_atomic_viral_load_reagents_SummaryGetAll(start_date, end_date).ToList();
                if (emergency == false)
                {
                    x = t.Where(e => (e.EmergencyOrder == false || e.EmergencyOrder == null) && e.OrderStatusId != 5).ToList();
                }
                if (emergency == true)
                {
                    x = t.Where(e => e.EmergencyOrder == true && e.OrderStatusId != 5).ToList();
                }
                if (emergency == null)
                {
                    x = t.Where(e => e.OrderStatusId != 5).ToList();
                }
                //if (x.Count() > 0)
                //{
                //    foreach (var a in x)
                //    {
                //        var b = context.A_Sector_Change.OrderBy(e => e.date_of_change).FirstOrDefault(e => e.date_of_change > a.DatePrepared && e.facility_code == a.FacilityCode);
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

        public ObjectResult<spView_sap_viral_loadGetAll1_Result> GetSAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_sap_viral_loadGetAll1(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}