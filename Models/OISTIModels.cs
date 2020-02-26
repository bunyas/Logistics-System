using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class OISTIModels
    {
        mascisEntities context = new mascisEntities();

        public ObjectResult<spView_WebTemplate_OI_STIGetAll_Result> GetOrder(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_OI_STIGetAll(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_OI_STI_WebGetAll_Add_Result> GetOrder_Add(int FC, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_WebTemplate_OI_STI_WebGetAll_Add(null, FC, StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_export_atomic_oi_sti_Physical_CountGetAll_Result> GetPhysicalCount(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_export_atomic_oi_sti_Physical_CountGetAll(StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_OI_GetAll_Custom1_Result> GetOrderCustom(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_OI_GetAll_Custom1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }


        public ObjectResult<spView_lmis_allocation_n_oi_stiGetAll_Result> GetAllocation(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_oi_stiGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //Emergency
        public ObjectResult<spView_lmis_allocation_e_oi_stiGetAll_Result> GetAllocationEmergency(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_e_oi_stiGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public List<spView_export_atomic_oi_stiGetAll_Result> GetAll(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_oi_stiGetAll_Result>();
                var m = new List<spView_export_atomic_oi_stiGetAll_Result>();
                var t = context.spView_export_atomic_oi_stiGetAll(start_date, end_date).ToList();
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
                    x = t.Where(e=> e.OrderStatusId !=5).ToList();
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
        public ObjectResult<spSAP_EXPORT_Order_OI_STI_HeaderGetAll_Result> SAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spSAP_EXPORT_Order_OI_STI_HeaderGetAll(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_sap_oiGetAll1_Result> GetSAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_sap_oiGetAll1(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

    }
}