﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class SMCModels
    {
        mascisEntities context = new mascisEntities();

        public ObjectResult<spView_WebTemplate_SMC_SLMGetAll_Result> GetOrder( string order_no)
        {
            try
            {
                return context.spView_WebTemplate_SMC_SLMGetAll(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }

        }

        public ObjectResult<spView_WebTemplate_SMC_SLM_WebGetAll_Add_Result> GetOrder_Add(int FC, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_WebTemplate_SMC_SLM_WebGetAll_Add(null, FC, StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_export_atomic_smc_slm_Physical_CountAll_Result> GetPhysicalCount(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_export_atomic_smc_slm_Physical_CountAll(StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_SMC_SLMGetAll_Custom1_Result> GetOrderCustom(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_SMC_SLMGetAll_Custom1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }

        }
        //Emergency
        public ObjectResult<spView_lmis_allocation_e_smcGetAll_Result> GetAllocationEmergency(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_e_smcGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_lmis_allocation_n_smcGetAll_Result> GetAllocation(int? order_type,int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_smcGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spSAP_EXPORT_Order_SMC_SLM_HeaderGetAll_Result> SAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spSAP_EXPORT_Order_SMC_SLM_HeaderGetAll(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public List<spView_export_atomic_smc_slmGetAll_Result> GetAll(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_smc_slmGetAll_Result>();
                List<spView_export_atomic_smc_slmGetAll_Result> m = new List<spView_export_atomic_smc_slmGetAll_Result>();
                var t = context.spView_export_atomic_smc_slmGetAll(start_date, end_date).ToList();
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
        public List<spView_export_atomic_smc_slm_SummaryGetAll_Result> SMCSummary(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_smc_slm_SummaryGetAll_Result>();
                List<spView_export_atomic_smc_slm_SummaryGetAll_Result> m = new List<spView_export_atomic_smc_slm_SummaryGetAll_Result>();
                var t = context.spView_export_atomic_smc_slm_SummaryGetAll(start_date, end_date).ToList();
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

        public ObjectResult<spView_sap_smcGetAll1_Result> GetSAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_sap_smcGetAll1(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

    }
}