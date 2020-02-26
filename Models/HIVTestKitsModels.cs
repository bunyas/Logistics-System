﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class HIVTestKitsModels
    {
        mascisEntities context = new mascisEntities();

        public ObjectResult<spView_WebTemplate_HIV_Test_KitsGetAll_MA_WFA_Result> GetOrder(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_KitsGetAll_MA_WFA(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_export_atomic_hiv_tk_Physical_CountGetAll_Result> GetPhysicalCount(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_export_atomic_hiv_tk_Physical_CountGetAll(StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_HIV_Test_KitsGetAll_Add_Result> GetOrder_Add(int FC, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_KitsGetAll_Add(null, FC, StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spSAP_EXPORT_order_hiv_rapid_test_kit_headerGetAll_Result> SAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spSAP_EXPORT_order_hiv_rapid_test_kit_headerGetAll(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_HIV_Test_KitsGetAll_Custom1_Result> GetOrderCustom(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_KitsGetAll_Custom1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_HIV_Test_Kits_SummaryGetAll_MA_WFA_Result> GetOrderSummary(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_Kits_SummaryGetAll_MA_WFA(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //Emergency
        public ObjectResult<spView_lmis_allocation_e_test_kitGetAll_Result> GetAllocationEmergency(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_e_test_kitGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_allocation_n_test_kitGetAll_Result> GetAllocation(int? order_type,int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_test_kitGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public List<spView_export_atomic_hiv_tkGetAll_Result> GetAll(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_hiv_tkGetAll_Result>();
                List<spView_export_atomic_hiv_tkGetAll_Result> m = new List<spView_export_atomic_hiv_tkGetAll_Result>();
                var t = context.spView_export_atomic_hiv_tkGetAll(start_date, end_date).ToList();
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
        public List<spView_export_atomic_hiv_tk_SummaryGetAll_Result> TestKitSummary(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_hiv_tk_SummaryGetAll_Result>();
                List<spView_export_atomic_hiv_tk_SummaryGetAll_Result> m = new List<spView_export_atomic_hiv_tk_SummaryGetAll_Result>();
                var t = context.spView_export_atomic_hiv_tk_SummaryGetAll(start_date, end_date).ToList();
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
                //        var b = context.A_Sector_Change.OrderBy(e => e.date_of_change).FirstOrDefault(e => e.date_of_change > a.DatePrepared && e.facility_code == a.Facility_Code);
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

        public ObjectResult<spView_sap_hiv_test_kitsGetAll1_Result> GetSAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_sap_hiv_test_kitsGetAll1(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

    }
}