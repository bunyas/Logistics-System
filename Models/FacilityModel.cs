using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class FacilityModel
    {
        mascisEntities context = new mascisEntities();
        private int mFacilityCode;
        private int? mDeliveryZoneCode;
        private int? mImplimentingPartnerCode;
        private int? mDistrrictCode;
        private string mFacility;
        private string mSAP_Code;
        private Boolean? mSupportedByMAUL;
        private Boolean? mIsAccredited;
        private int? mlevel_of_care;
        private int? mclient_type_code;
        private int? mregion_code;
        private string mRFSO_UserName;
        private int? mOwnershipId;
        private int? mCDCRegionId;
        private string mFacilityTypeId;
        private string mLongtitude;
        private string mLatititude;
        private string mVillage_Id;
        private int? mComprehensiveImplimentingPartnerCode;
        private int? mPatientLoadCode;
        private Boolean? mIsActive;
        private int? mNearest_Public_HF_Distance;
        private string mDSDM;

        public int FacilityCode { get { return mFacilityCode; } set { mFacilityCode = value; } }
        public int? DeliveryZoneCode { get { return mDeliveryZoneCode; } set { mDeliveryZoneCode = value; } }
        public int? ImplimentingPartnerCode { get { return mImplimentingPartnerCode; } set { mImplimentingPartnerCode = value; } }
        public int? DistrrictCode { get { return mDistrrictCode; } set { mDistrrictCode = value; } }
        public string Facility { get { return mFacility; } set { mFacility = value; } }
        public string SAP_Code { get { return mSAP_Code; } set { mSAP_Code = value; } }
        public Boolean? SupportedByMAUL { get { return mSupportedByMAUL; } set { mSupportedByMAUL = value; } }
        public Boolean? IsAccredited { get { return mIsAccredited; } set { mIsAccredited = value; } }
        public int? level_of_care { get { return mlevel_of_care; } set { mlevel_of_care = value; } }
        public int? client_type_code { get { return mclient_type_code; } set { mclient_type_code = value; } }
        public int? region_code { get { return mregion_code; } set { mregion_code = value; } }
        public string RFSO_UserName { get { return mRFSO_UserName; } set { mRFSO_UserName = value; } }
        public int? OwnershipId { get { return mOwnershipId; } set { mOwnershipId = value; } }
        public int? CDCRegionId { get { return mCDCRegionId; } set { mCDCRegionId = value; } }
        public string FacilityTypeId { get { return mFacilityTypeId; } set { mFacilityTypeId = value; } }
        public string Longtitude { get { return mLongtitude; } set { mLongtitude = value; } }
        public string Latititude { get { return mLatititude; } set { mLatititude = value; } }
        public string Village_Id { get { return mVillage_Id; } set { mVillage_Id = value; } }
        public int? ComprehensiveImplimentingPartnerCode { get { return mComprehensiveImplimentingPartnerCode; } set { mComprehensiveImplimentingPartnerCode = value; } }
        public int? PatientLoadCode { get { return mPatientLoadCode; } set { mPatientLoadCode = value; } }
        public Boolean? IsActive { get { return mIsActive; } set { mIsActive = value; } }
        public int? Nearest_Public_HF_Distance { get { return mNearest_Public_HF_Distance; } set { mNearest_Public_HF_Distance = value; } }
        public string DSDM { get { return mDSDM; } set { mDSDM = value; } }

        public A_Facilities GetRecordByKey(int order_no)
        {
            try
            {
                return context.A_Facilities.FirstOrDefault(s => s.FacilityCode == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean Save()
        {
            try
            {
                if (GetRecordByKey(mFacilityCode) == null)
                {
                    var x = new A_Facilities
                    {
                        FacilityCode = mFacilityCode,
                        DeliveryZoneCode = mDeliveryZoneCode,
                        ImplimentingPartnerCode = mImplimentingPartnerCode,
                        DistrrictCode = mDistrrictCode,
                        Facility = mFacility,
                        SAP_Code = mSAP_Code,
                        SupportedByMAUL = mSupportedByMAUL,
                        IsAccredited = mIsAccredited,
                        level_of_care = mlevel_of_care,
                        client_type_code = mclient_type_code,
                        region_code = mregion_code,
                        RFSO_UserName = mRFSO_UserName,
                        OwnershipId = mOwnershipId,
                        CDCRegionId = mCDCRegionId,
                        FacilityTypeId = mFacilityTypeId,
                        Longtitude = mLongtitude,
                        Latititude = mLatititude,
                        Village_Id = mVillage_Id,
                        ComprehensiveImplimentingPartnerCode = mComprehensiveImplimentingPartnerCode,
                        PatientLoadCode = mPatientLoadCode,
                        IsActive = mIsActive,
                        Nearest_Public_HF_Distance = mNearest_Public_HF_Distance,
                        DSDM = mDSDM

                    };
                    context.A_Facilities.Add(x);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean Update()
        {
            try
            {
                var x = GetRecordByKey(mFacilityCode);
                if (x != null)
                {
                    x.FacilityCode = mFacilityCode;
                    x.DeliveryZoneCode = mDeliveryZoneCode;
                    x.ImplimentingPartnerCode = mImplimentingPartnerCode;
                    x.DistrrictCode = mDistrrictCode;
                    x.Facility = mFacility;
                    x.SAP_Code = mSAP_Code;
                    x.SupportedByMAUL = mSupportedByMAUL;
                    x.IsAccredited = mIsAccredited;
                    x.level_of_care = mlevel_of_care;
                    x.client_type_code = mclient_type_code;
                    x.region_code = mregion_code;
                    x.RFSO_UserName = mRFSO_UserName;
                    x.OwnershipId = mOwnershipId;
                    x.CDCRegionId = mCDCRegionId;
                    x.FacilityTypeId = mFacilityTypeId;
                    x.Longtitude = mLongtitude;
                    x.Latititude = mLatititude;
                    x.Village_Id = mVillage_Id;
                    x.ComprehensiveImplimentingPartnerCode = mComprehensiveImplimentingPartnerCode;
                    x.PatientLoadCode = mPatientLoadCode;
                    x.IsActive = mIsActive;
                    x.Nearest_Public_HF_Distance = mNearest_Public_HF_Distance;
                    x.DSDM = mDSDM;
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}