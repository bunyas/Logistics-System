using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class ARTModels
    {
        mascisEntities context = new mascisEntities();

        public ObjectResult<spView_WebTemplate_ARVGetAll_MA_WFA_Result> GetOrder(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ARVGetAll_MA_WFA(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_ARV_ExtraRegimenGetAll_Result> GetOrderExtraRegimen(int? classification_code, string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ARV_ExtraRegimenGetAll(classification_code, null, null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_ARVGetAll_Add_Result> GetOrder_Add(int FC, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_WebTemplate_ARVGetAll_Add(null, FC, StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_export_atomic_arv_Physical_CountGetAll_Result> GetPhysicalCount(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                return context.spView_export_atomic_arv_Physical_CountGetAll(StartDate, EndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_ARVGetAll_Custom1_Result> GetOrderCustom(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ARVGetAll_Custom1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spSAP_EXPORT_Order_HeaderGetAll_Result> SAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spSAP_EXPORT_Order_HeaderGetAll( start_date,  end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_ARV_PatientsGetAll1_Result> GetOrderPatients( string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ARV_PatientsGetAll1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_ARV_Patients_PaediatricGetAll1_Result> GetOrderPatientsPaediatric( string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ARV_Patients_PaediatricGetAll1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_ARV_FluconazoleGetAll1_Result> GetOrderPatientsOther(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ARV_FluconazoleGetAll1(null, null, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_SMC_SLM_Patient_SummaryGetAll_Result> GetOrderSMCPatientSummary(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_SMC_SLM_Patient_SummaryGetAll( order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spView_WebTemplate_ViralLoadReagents_SummaryGetAll_Result> GetOrderViralloadTestSummary(string order_no)
        {
            try
            {
                return context.spView_WebTemplate_ViralLoadReagents_SummaryGetAll(order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_art_extra_regimen_Result> GetOrderPatientsExtraRegimen(int? classification_code, string order_no)
        {
            try
            {
                return context.spView_lmis_art_extra_regimen(null, classification_code, order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_allocation_n_arvGetAll_Result> GetAllocation( int? order_type,int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_n_arvGetAll(order_type,product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_lmis_allocation_e_arvGetAll_Result> GetAllocationEmergency(int? order_type, int? product_code)
        {
            try
            {
                return context.spView_lmis_allocation_e_arvGetAll(order_type, product_code);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public List<spView_export_atomic_arvGetAll_Result> GetAll(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_arvGetAll_Result>();
                List<spView_export_atomic_arvGetAll_Result> m = new List<spView_export_atomic_arvGetAll_Result>();
                var t = context.spView_export_atomic_arvGetAll(start_date, end_date).ToList();
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
                //            m.Add(a);
                //        }
                //        else
                //        {
                //            m.Add(a);
                //        }
                        
                //    }
                //}
                return x;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public List<spView_export_atomic_arv_Treatment_Fluconazole_Result> GetTreatment_Fluconazole(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_arv_Treatment_Fluconazole_Result>();
                List<spView_export_atomic_arv_Treatment_Fluconazole_Result> m = new List<spView_export_atomic_arv_Treatment_Fluconazole_Result>();
                var t = context.spView_export_atomic_arv_Treatment_Fluconazole(start_date, end_date).ToList();
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
        public List<spView_export_atomic_arv_Treatment_PatientSummary_Result> GetPatientSummary(DateTime? start_date, DateTime? end_date, bool? emergency)
        {
            try
            {
                var x = new List<spView_export_atomic_arv_Treatment_PatientSummary_Result>();
                List<spView_export_atomic_arv_Treatment_PatientSummary_Result> m = new List<spView_export_atomic_arv_Treatment_PatientSummary_Result>();
                var t = context.spView_export_atomic_arv_Treatment_PatientSummary(start_date, end_date).ToList();

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

        public ObjectResult<spView_export_atomic_arv_pnGetAll1_Result> PatientsGetAll(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_export_atomic_arv_pnGetAll1(null, null, null, null, start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_export_atomic_arv_pn_fluconazoleGetAll1_Result> PatientsOtherGetAll(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_export_atomic_arv_pn_fluconazoleGetAll1(null, null, null, null, start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_sap_artGetAll1_Result> GetSAPExport(DateTime? start_date, DateTime? end_date)
        {
            try
            {
                return context.spView_sap_artGetAll1(start_date, end_date);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

    }
}