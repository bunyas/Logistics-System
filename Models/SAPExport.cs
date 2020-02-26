using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using mascis.ScheduledTasks;
using System.Data.Entity;

namespace mascis.Models
{
    public class SAPExport
    {
        mascisEntities context = new mascisEntities();
     
        // private Boolean existing_records = false;

        private string mOrderNumber;
        private DateTime mAddedDate;
        private string mAddedBy;
        private Boolean? mCurrentReccord;
        private Boolean? mRecordReadBySAP;
        private string mDocNum;
        private DateTime? mDocDate;
        private DateTime? mDocDueDate;
        private string mCardCode;
        private int? mDocEntry;
        private int? mProductCategory;

        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public DateTime AddedDate { get { return mAddedDate; } set { mAddedDate = value; } }
        public string AddedBy { get { return mAddedBy; } set { mAddedBy = value; } }
        public Boolean? CurrentReccord { get { return mCurrentReccord; } set { mCurrentReccord = value; } }
        public Boolean? RecordReadBySAP { get { return mRecordReadBySAP; } set { mRecordReadBySAP = value; } }
        public string DocNum { get { return mDocNum; } set { mDocNum = value; } }
        public DateTime? DocDate { get { return mDocDate; } set { mDocDate = value; } }
        public DateTime? DocDueDate { get { return mDocDueDate; } set { mDocDueDate = value; } }
        public string CardCode   { get { return mCardCode; } set { mCardCode = value; } }
        public int? DocEntry { get { return mDocEntry; } set { mDocEntry = value; } }
        public int? ProductCategory { get { return mProductCategory; } set { mProductCategory = value; } }

        // ART MODULE
        public Order_Header GetRecordByKey(string order_no)
        {
            try
            {
                return context.Order_Header.FirstOrDefault(s => s.OrderNumber == order_no );
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean SaveART(string OrderNo)
        {
            try
            {
                var allocated = context.Order_DrugDetails.Where(o => o.OrderNumber == OrderNo && o.Quantity_Allocated > 0).ToList().Count;
                if (allocated > 0)
                {
                    if (GetRecordByKey(OrderNo) != null)
                    {
                        Order_Header t = new Order_Header();
                        t = GetRecordByKey(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            CardCode = sapcode,// t.A_Facilities.SAP_Code,
                            DocEntry = mDocEntry,
                            ProductCategory = 2,
                        };
                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();

                        }
                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveART(OrderNo);
                        //var update = t;
                        //update.OrderStatusId = 4;
                        //context.Entry(t).CurrentValues.SetValues(update);
                        //context.Entry(t).State = EntityState.Modified;
                        //context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
                }
                else
                {
                    // existing_records = true;
                    return false;
                }
               
            }
            catch (Exception x)
            {
                throw (x);
            }
        }   

        //hiv test kits
        public order_hiv_rapid_test_kit_header GetRecordByKey_HIV(string order_no)
        {
            try
            {
                return context.order_hiv_rapid_test_kit_header.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveHIV(string OrderNo)
        {
            try
            {
                var allocated = context.order_hiv_rapid_test_kit.Where(o => o.OrderNumber == OrderNo && o.Quantity_Allocated > 0).ToList().Count;
                if (allocated > 0)
                {
                    if (GetRecordByKey_HIV(OrderNo) != null)
                    {
                        order_hiv_rapid_test_kit_header t = new order_hiv_rapid_test_kit_header();
                        t = GetRecordByKey_HIV(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            //CardCode = t.A_Facilities.SAP_Code,
                            CardCode = sapcode,
                            DocEntry = mDocEntry,
                            ProductCategory = 3,
                        };

                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();
                        }
                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveHIV(OrderNo);
                        //var update = t;
                        //update.OrderStatusId = 4;
                        //context.Entry(t).CurrentValues.SetValues(update);
                        //context.Entry(t).State = EntityState.Modified;
                        //context.SaveChanges();


                        //// to update the order status of the record
                        //OrderHIVTestKitHeaderModels sap = new OrderHIVTestKitHeaderModels();
                        //sap.UpdateSAP(OrderNo);

                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
                }
                else
                {
                    // existing_records = true;
                    return false;
                }
               
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public View_Order_Lab_SAP_Export_Final GetRecordByKey_Lab(string order_no)
        {
            try
            {
                return context.View_Order_Lab_SAP_Export_Final.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveLab(string OrderNo)
        {
            try
            {
                var allocated = context.Order_Lab.Where(o => o.OrderNumber == OrderNo && o.quantity_allocated > 0).ToList().Count;
                if (allocated > 0)
                {
                    if (GetRecordByKey_Lab(OrderNo) != null)
                    {
                        View_Order_Lab_SAP_Export_Final t = new View_Order_Lab_SAP_Export_Final();
                        t = GetRecordByKey_Lab(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        //var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            CardCode = t.SAP_Code,
                            DocEntry = mDocEntry,
                            ProductCategory = 1,
                        };
                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();

                        }
                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveLab(OrderNo);
                        //var lab = context.Order_Lab_Header.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        //var update = lab;
                        //update.OrderStatusId = 4;
                        //context.Entry(lab).CurrentValues.SetValues(update);
                        //context.Entry(lab).State = EntityState.Modified;
                        //context.SaveChanges();

                        //// to update the order status of the record
                        //OrderLaboratoryHeaderModels sap = new OrderLaboratoryHeaderModels();
                        //sap.UpdateSAP(OrderNo);

                        //// to update the order status of the record in lab custom
                        //OrderLaboratoryCustomHeaderModels sapCustom = new OrderLaboratoryCustomHeaderModels();
                        //sapCustom.UpdateSAP(OrderNo);

                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
                }
                else
                {
                    // existing_records = true;
                    return false;
                }
                
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //OIs
        public Order_OI_STI_Header GetRecordByKeyOIs(string order_no)
        {
            try
            {
                return context.Order_OI_STI_Header.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveOIs(string OrderNo)
        {
            try
            {
                var allocated = context.Order_OI_STI_Detail.Where(o => o.OrderNumber == OrderNo && o.Quantity_Allocated > 0).ToList().Count;
                if (allocated > 0)
                {
                    if (GetRecordByKeyOIs(OrderNo) != null)
                    {
                        Order_OI_STI_Header t = new Order_OI_STI_Header();
                        t = GetRecordByKeyOIs(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            CardCode = sapcode,// t.A_Facilities.SAP_Code,
                            DocEntry = mDocEntry,
                            ProductCategory = 9,
                        };
                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();

                        }

                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveOIs(OrderNo);
                        //var update = t;
                        //update.OrderStatusId = 4;
                        //context.Entry(t).CurrentValues.SetValues(update);
                        //context.Entry(t).State = EntityState.Modified;
                        //context.SaveChanges();
                        //context.SAP_ExportHeader.Add(x);
                        //context.SaveChanges();

                        //SAPExportDetails details = new SAPExportDetails();
                        //details.SaveOIs(OrderNo);

                        // to update the order status of the record
                        //OrderOISTIHeaderModels sap = new OrderOISTIHeaderModels();
                        //sap.UpdateSAP(OrderNo);

                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
                }
                else
                {
                    // existing_records = true;
                    return false;
                }
               
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //SMC
        public Order_SMC_SLM_Header GetRecordByKeySMC(string order_no)
        {
            try
            {
                return context.Order_SMC_SLM_Header.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveSMC(string OrderNo)
        {
            try
            {
                var allocated = context.Order_SMC_SLM.Where(o => o.OrderNumber == OrderNo && o.QuantityAllocated > 0).ToList().Count;
                if(allocated > 0)
                {
                    if (GetRecordByKeySMC(OrderNo) != null)
                    {
                        Order_SMC_SLM_Header t = new Order_SMC_SLM_Header();
                        t = GetRecordByKeySMC(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            CardCode = sapcode,// t.A_Facilities.SAP_Code,
                            DocEntry = mDocEntry,
                            ProductCategory = 10,
                        };
                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();

                        }

                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveSMC(OrderNo);
                        //var update = t;
                        //update.OrderStatusId = 4;
                        //context.Entry(t).CurrentValues.SetValues(update);
                        //context.Entry(t).State = EntityState.Modified;
                        //context.SaveChanges();
                        //context.SAP_ExportHeader.Add(x);
                        //context.SaveChanges();

                        //SAPExportDetails details = new SAPExportDetails();
                        //details.SaveSMC(OrderNo);

                        // to update the order status of the record
                        //OrderSMCHeaderModels sap = new OrderSMCHeaderModels();
                        //sap.UpdateSAP(OrderNo);

                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
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

        //Viral Load
        public Order_ViralLoadReagents_Header GetRecordByKeyViralLoad(string order_no)
        {
            try
            {
                return context.Order_ViralLoadReagents_Header.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveViralLoad(string OrderNo)
        {
            try
            {
                var allocated = context.Order_ViralLoadReagents_Detail.Where(o => o.OrderNumber == OrderNo && o.Quantity_Allocated > 0).ToList().Count;
                if (allocated > 0)
                {
                    if (GetRecordByKeyViralLoad(OrderNo) != null)
                    {
                        Order_ViralLoadReagents_Header t = new Order_ViralLoadReagents_Header();
                        t = GetRecordByKeyViralLoad(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            CardCode = sapcode,// t.A_Facilities.SAP_Code,
                            DocEntry = mDocEntry,
                            ProductCategory = 11,
                        };
                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();

                        }

                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveViralLoad(OrderNo);
                        //var update = t;
                        //update.OrderStatusId = 4;
                        //context.Entry(t).CurrentValues.SetValues(update);
                        //context.Entry(t).State = EntityState.Modified;
                        //context.SaveChanges();
                        //context.SAP_ExportHeader.Add(x);
                        //context.SaveChanges();

                        //SAPExportDetails details = new SAPExportDetails();
                        //details.SaveViralLoad(OrderNo);

                        // to update the order status of the record
                        //OrderViralLoadReagentsHeaderModels sap = new OrderViralLoadReagentsHeaderModels();
                        //sap.UpdateSAP(OrderNo);

                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
                }
                else
                {
                    // existing_records = true;
                    return false;
                }
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //TB
        public Order_TB_Header GetRecordByKeyTB(string order_no)
        {
            try
            {
                return context.Order_TB_Header.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveTB(string OrderNo)
        {
            try
            {
                var allocated = context.Order_TB.Where(o => o.OrderNumber == OrderNo && o.Quantity_Allocated > 0).ToList().Count;
                if (allocated > 0)
                {
                    if (GetRecordByKeyTB(OrderNo) != null)
                    {
                        Order_TB_Header t = new Order_TB_Header();
                        t = GetRecordByKeyTB(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            CardCode = sapcode,// t.A_Facilities.SAP_Code,
                            DocEntry = mDocEntry,
                            ProductCategory = 13,
                        };
                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();

                        }
                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveTB(OrderNo);
                        //var update = t;
                        //update.OrderStatusId = 4;
                        //context.Entry(t).CurrentValues.SetValues(update);
                        //context.Entry(t).State = EntityState.Modified;
                        //context.SaveChanges();
                        //context.SAP_ExportHeader.Add(x);
                        //context.SaveChanges();

                        //SAPExportDetails details = new SAPExportDetails();
                        //details.SaveTB(OrderNo);

                        // to update the order status of the record
                        //OrderTBHeaderModels sap = new OrderTBHeaderModels();
                        //sap.UpdateSAP(OrderNo);

                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
                }
                 else
                {
                    // existing_records = true;
                    return false;
                }
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        //Rutf

        public Order_RUTF_Header GetRecordByKeyRutf(string order_no)
        {
            try
            {
                return context.Order_RUTF_Header.FirstOrDefault(s => s.OrderNumber == order_no);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        public Boolean SaveRutf(string OrderNo)
        {
            try
            {
                var allocated = context.Order_RUTF_Details.Where(o => o.OrderNumber == OrderNo && o.Quantity_Allocated > 0).ToList().Count;
                if (allocated > 0)
                {
                    if (GetRecordByKeyRutf(OrderNo) != null)
                    {
                        Order_RUTF_Header t = new Order_RUTF_Header();
                        t = GetRecordByKeyRutf(OrderNo);
                        //DateTime eDate  = t.EndDate;
                        DateTime pDate = System.Convert.ToDateTime(t.DatePrepared);
                        int facility = t.FacilityCode;
                        var sapcode = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility).SAP_Code;
                        var x = new SAP_ExportHeader
                        {
                            OrderNumber = t.OrderNumber,
                            AddedDate = System.DateTime.Now,
                            AddedBy = new UserManagement().getCurrentuser(),
                            CurrentReccord = true,
                            RecordReadBySAP = false,
                            DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                            DocDate = System.Convert.ToDateTime(pDate),
                            DocDueDate = System.Convert.ToDateTime(pDate),
                            CardCode = sapcode,// t.A_Facilities.SAP_Code,
                            DocEntry = mDocEntry,
                            ProductCategory = 12,
                        };
                        var order = context.SAP_ExportHeader.FirstOrDefault(o => o.OrderNumber == OrderNo);
                        if (order == null)
                        {
                            context.SAP_ExportHeader.Add(x);
                            context.SaveChanges();

                        }
                        SAPExportDetails details = new SAPExportDetails();
                        details.SaveRutf(OrderNo);
                        //var update = t;
                        //update.OrderStatusId = 4;
                        //context.Entry(t).CurrentValues.SetValues(update);
                        //context.Entry(t).State = EntityState.Modified;
                        //context.SaveChanges();
                        //context.SAP_ExportHeader.Add(x);
                        //context.SaveChanges();

                        //SAPExportDetails details = new SAPExportDetails();
                        //details.SaveRutf(OrderNo);

                        // to update the order status of the record
                        //OrderRutfHeaderModels sap = new OrderRutfHeaderModels();
                        //sap.UpdateSAP(OrderNo);

                        return true;
                    }
                    else
                    {
                        // existing_records = true;
                        return false;
                    }
                }
                else
                {
                    // existing_records = true;
                    return false;
                }
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}