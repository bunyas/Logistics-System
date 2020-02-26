using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace mascis.Models
{
    public class ReportsModel
    {
        mascisEntities context = new mascisEntities();

        private int? mProductCategoryId;
        private string mOrderNumber;
        private DateTime? mEndDate;
        private int? mFacilityCode;
        private DateTime? mStartDate;
        private string mYear;
        private string mMonth;
        private string mDeliveryZoneCode;
        private string mcdc;
        private string mIP;
        private string mSCTO;
        private string mproductCategory;
        private string mfc;
        private string mClientType;
        private string mIsActive;
        private string mOwner;
        public string fc { get { return mfc; } set { mfc = value; } }
        public string Owner { get { return mOwner; } set { mOwner = value; } }
        public string IsActive { get { return mIsActive; } set { mIsActive = value; } }
        public string ClientType { get { return mClientType; } set { mClientType = value; } }
        public string Year { get { return mYear; } set { mYear = value; } }
        public string Month { get { return mMonth; } set { mMonth = value; } }
        public string DeliveryZoneCode { get { return mDeliveryZoneCode; } set { mDeliveryZoneCode = value; } }
        public string cdc { get { return mcdc; } set { mcdc = value; } }
        public string IP { get { return mIP; } set { mIP = value; } }
        public string SCTO { get { return mSCTO; } set { mSCTO = value; } }
        public string productCategory { get { return mproductCategory; } set { mproductCategory = value; } }
        public int? ProductCategoryId { get { return mProductCategoryId; } set { mProductCategoryId = value; } }
        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public DateTime? EndDate { get { return mEndDate; } set { mEndDate = value; } }
        public DateTime? StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public int? FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }

        public ObjectResult<spOrder_TrackingModule_LogGetAll_Result> GetTracking()
        {
            try
            {
                return context.spOrder_TrackingModule_LogGetAll(mProductCategoryId, mOrderNumber);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public ObjectResult<spOrder_TrackingModuleGetAll_Result> GetCurrentTracking()
        {
            try
            {
                return context.spOrder_TrackingModuleGetAll(mProductCategoryId, mOrderNumber,null,null);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //spView_WebTemplate_ARV_PC_ReportGetAll
        public ObjectResult<spView_WebTemplate_ARV_PC_ReportGetAll_Result> GetPC_ART()
        {
            try
            {
                return context.spView_WebTemplate_ARV_PC_ReportGetAll(null, mStartDate,mEndDate,mFacilityCode);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_OIs_PC_ReportGetAll_Result> GetPC_OIs()
        {
            try
            {
                return context.spView_WebTemplate_OIs_PC_ReportGetAll(mFacilityCode, mStartDate, mEndDate );
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_SMC_PC_ReportGetAll_Result> GetPC_SMC()
        {
            try
            {
                return context.spView_WebTemplate_SMC_PC_ReportGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_HIV_Test_Kits_PC_ReportGetAll_Result> GetPC_HIV()
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_Kits_PC_ReportGetAll(null, mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_Laboratory_PC_ReportGetAll_Result> GetPC_Lab()
        {
            try
            {
                return context.spView_WebTemplate_Laboratory_PC_ReportGetAll( mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spOrder_TrackingDetailsGetAll_Result> GetTrackingDetails()
        {
            try
            {
                return context.spOrder_TrackingDetailsGetAll(mOrderNumber);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //HIV TEST KITS
        public ObjectResult<spView_WebTemplate_HIV_Test_KitsReportGetAll_Result> HIV()
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_KitsReportGetAll(mFacilityCode,mStartDate,mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_HIV_Test_Kits_ReportHeaderGetAll_Result> HIV_Header()
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_Kits_ReportHeaderGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_WebTemplate_HIV_Test_Kits_SummaryReportGetAll_Result> HIV_Summary()
        {
            try
            {
                return context.spView_WebTemplate_HIV_Test_Kits_SummaryReportGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //ART

        public ObjectResult<spView_WebTemplate_ARVReport_Result1> ART()
        {
            try
            {
                return context.spView_WebTemplate_ARVReport(null, mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //ART_Patients
        public ObjectResult<spView_WebTemplate_ARV_PatientsReportGetAll_Result> ART_Patients()
        {
            try
            {
                return context.spView_WebTemplate_ARV_PatientsReportGetAll(null, mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //ART_Paediatric
        public ObjectResult<spView_WebTemplate_ARV_Patients_PaediatricReportGetAll_Result> ART_Paediatric()
        {
            try
            {
                return context.spView_WebTemplate_ARV_Patients_PaediatricReportGetAll(null, mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //ARV_Fluconazole
        public ObjectResult<spView_WebTemplate_ARV_FluconazoleReportGetAll_Result> ARV_Fluconazole()
        {
            try
            {
                return context.spView_WebTemplate_ARV_FluconazoleReportGetAll( mFacilityCode, mStartDate, mEndDate, null);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //ARV_ReportHeader
        public ObjectResult<spView_WebTemplate_ARV_ReportHeaderGetAll_Result> ARV_ReportHeader()
        {
            try
            {
                return context.spView_WebTemplate_ARV_ReportHeaderGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //ARV_ReportExtraRegimen
        public ObjectResult<spView_WebTemplate_ARV_ReportExtraRegimenGetAll_Result> ARV_ReportExtraRegimen()
        {
            try
            {
                return context.spView_WebTemplate_ARV_ReportExtraRegimenGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //LAB
        public ObjectResult<spView_WebTemplate_LaboratoryReportGetAll_Result> Lab()
        {
            try
            {
                return context.spView_WebTemplate_LaboratoryReportGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //LAB Header
        public ObjectResult<spView_WebTemplate_Laboratory_ReportHeaderGetAll_Result> Lab_Header()
        {
            try
            {
                return context.spView_WebTemplate_Laboratory_ReportHeaderGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //OIs
        public ObjectResult<spView_WebTemplate_OI_STIReportGetAll_Result> OIs()
        {
            try
            {
                return context.spView_WebTemplate_OI_STIReportGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //OIs Header
        public ObjectResult<spView_WebTemplate_OIs_ReportHeaderGetAll_Result> OIs_Header()
        {
            try
            {
                return context.spView_WebTemplate_OIs_ReportHeaderGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //SMC Header
        public ObjectResult<spView_WebTemplate_SMC_ReportHeaderGetAll_Result> SMC_Header()
        {
            try
            {
                return context.spView_WebTemplate_SMC_ReportHeaderGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //SMC 
        public ObjectResult<spView_WebTemplate_SMC_SLMReportGetAll_Result> SMC()
        {
            try
            {
                return context.spView_WebTemplate_SMC_SLMReportGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //Viralload 
        public ObjectResult<spView_WebTemplate_ViralLoadReagentsReportGetAll_Result> Viralload()
        {
            try
            {
                return context.spView_WebTemplate_ViralLoadReagentsReportGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //Viralload  Header
        public ObjectResult<spView_WebTemplate_ViralLoad_ReportHeaderGetAll_Result> Viralload_Header()
        {
            try
            {
                return context.spView_WebTemplate_ViralLoad_ReportHeaderGetAll(mFacilityCode, mStartDate, mEndDate);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        //reporting rates
        public ObjectResult<spView_Rpt_ReportingRatesGetAll_Result> GetReportingRates()
        {
            try
            {
                ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
                var mydata = context.spView_Rpt_ReportingRatesGetAll(mMonth,mYear,mproductCategory,mDeliveryZoneCode,mcdc,mIP,mSCTO,mStartDate,mEndDate);
                return mydata;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_Rpt_AMCGetAll_Result> GetAMC()
        {
            try
            {
                ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
                var mydata = context.spView_Rpt_AMCGetAll(mMonth, mYear, mproductCategory, mDeliveryZoneCode, mcdc, mIP, mSCTO,mfc, mStartDate, mEndDate);
                return mydata;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public ObjectResult<spView_Rpt_ConsumptionGetAll_Result> GetConsumption()
        {
            try
            {
                ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
                var mydata = context.spView_Rpt_ConsumptionGetAll(mMonth, mYear, mproductCategory, mDeliveryZoneCode, mcdc, mIP, mSCTO, mfc, mStartDate, mEndDate);
                return mydata;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //reporting rates
        public ObjectResult<spView_Rpt_ReportingRates_1_GetAll_Result> GetReportingRatesCopy()
        {
            try
            {
                ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
                var mydata = context.spView_Rpt_ReportingRates_1_GetAll(mproductCategory, mStartDate, mEndDate);
                return mydata;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        // Expected reports
        public ObjectResult<spView_LMIS_ExpectedReportNewGetAll_Result> GetExpectedReports()
        {
            try
            {
                ((IObjectContextAdapter)this.context).ObjectContext.CommandTimeout = 3000;
                var mydata = context.spView_LMIS_ExpectedReportNewGetAll(mcdc, mClientType,mproductCategory, mIsActive, mOwner,mStartDate,mEndDate, mDeliveryZoneCode);
                return mydata;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}