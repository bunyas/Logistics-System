using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class SAPExportDetails
    {
        mascisEntities context = new mascisEntities();
        // private Boolean existing_records = false;

        private string mOrderNumber;
        private int mItemCode;
        private Boolean? mCurrentRecord;
        private Boolean? mRecordReadBySAP;
        private string mDocNum;
        private int? mQuantityAllocated;
        private int? mQuantityOrdered;
        private Boolean? mCurrentRecordOnSAP;
        private DateTime mAddedDate;
        private string mAddedBy;

        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public int ItemCode { get { return mItemCode; } set { mItemCode = value; } }
        public Boolean? CurrentRecord { get { return mCurrentRecord; } set { mCurrentRecord = value; } }
        public Boolean? RecordReadBySAP { get { return mRecordReadBySAP; } set { mRecordReadBySAP = value; } }
        public string DocNum { get { return mDocNum; } set { mDocNum = value; } }
        public int? QuantityAllocated { get { return mQuantityAllocated; } set { mQuantityAllocated = value; } }
        public int? QuantityOrdered { get { return mQuantityOrdered; } set { mQuantityOrdered = value; } }
        public Boolean? CurrentRecordOnSAP { get { return mCurrentRecordOnSAP; } set { mCurrentRecordOnSAP = value; } }
        public DateTime AddedDate { get { return mAddedDate; } set { mAddedDate = value; } }
        public string AddedBy { get { return mAddedBy; } set { mAddedBy = value; } }

        //art module
        public List<Order_DrugDetails> GetRecordByKey(string order_no)
        {
            try
            {
                return context.Order_DrugDetails.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public List<SAP_ExportDocumentLines> GetRecord(string order_no, int code)
        {
            try
            {
                return context.SAP_ExportDocumentLines.Where(s => s.OrderNumber == order_no && s.ItemCode==code).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean SaveART(string order_no)
        {
           
            try
            {
                if (GetRecordByKey(order_no).Count > 0)
                {
                    List<Order_DrugDetails> m = new List<Order_DrugDetails>();
                    m = GetRecordByKey(order_no);
                    //DateTime eDate = m.;
                    DateTime date = System.Convert.ToDateTime(context.Order_Header.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    foreach (var n in m)
                    {
                        if(n.Quantity_Allocated > 0)
                        {
                            DateTime pDate = date;//System.Convert.ToDateTime(n.Order_Header.DatePrepared);
                            int? facility = n.FacilityCode;
                            int productcode = n.DrugCode;
                            var sapcode = context.A_Product.FirstOrDefault(a => a.product_code == n.DrugCode);
                            if (sapcode != null)
                            {
                                productcode = Convert.ToInt32(sapcode.SAP_code);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.Quantity_Allocated),
                                QuantityOrdered = Convert.ToInt32(n.TotalDrugs_Required),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };

                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                      
                    }
                   
                    return true;
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

        // hiv module
        public List<order_hiv_rapid_test_kit> GetRecordByKeyHIV(string order_no)
        {
            try
            {
                return context.order_hiv_rapid_test_kit.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        
        public Boolean SaveHIV(string order_no)
        {

            try
            {
                if (GetRecordByKeyHIV(order_no).Count > 0)
                {
                    List<order_hiv_rapid_test_kit> m = new List<order_hiv_rapid_test_kit>();
                    m = GetRecordByKeyHIV(order_no);
                    DateTime date = System.Convert.ToDateTime(context.order_hiv_rapid_test_kit_header.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    //DateTime eDate = m.;
                    foreach (var n in m)
                    {
                       if(n.Quantity_Allocated > 0)
                        {
                            DateTime pDate = date;//System.Convert.ToDateTime(n.order_hiv_rapid_test_kit_header.DatePrepared);
                            int? facility = n.Facility_Code;
                            int productcode = n.item_code;
                            var sapcode = context.A_Product.FirstOrDefault(a => a.product_code == n.item_code);
                            if (sapcode != null)
                            {
                                productcode = Convert.ToInt32(sapcode.SAP_code);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.Quantity_Allocated),
                                QuantityOrdered = Convert.ToInt32(n.Quantity_Required),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };
                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                    }

                    return true;
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

        //lab
        public List<View_Order_Lab_SAP_Export_Details> GetRecordByKeyLab(string order_no)
        {
            try
            {
                return context.View_Order_Lab_SAP_Export_Details.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

       
        public Boolean SaveLab(string order_no)
        {

            try
            {
                if (GetRecordByKeyLab(order_no).Count > 0)
                {
                    List<View_Order_Lab_SAP_Export_Details> m = new List<View_Order_Lab_SAP_Export_Details>();
                    m = GetRecordByKeyLab(order_no);
                    //DateTime eDate = m.;
                    DateTime date = System.Convert.ToDateTime(context.View_Order_Lab_SAP_Export_Details.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    foreach (var n in m)
                    {
                      if(n.Quantity_Allocated > 0)
                        {
                            DateTime pDate = date;//System.Convert.ToDateTime(n.DatePrepared);
                            int? facility = n.FacilityCode;
                            int productcode = n.ProductCode;
                            var pro = context.A_Product.FirstOrDefault(e => e.product_code == n.ProductCode);
                            if (!string.IsNullOrEmpty(pro.SAP_code))
                            {
                                string r = pro.SAP_code.Replace('"', ' ').Trim();
                                productcode = Convert.ToInt32(r);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.Quantity_Allocated),
                                QuantityOrdered = Convert.ToInt32(n.Quantity_Required),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };

                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                    }

                    return true;
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

        // OIs
        public List<Order_OI_STI_Detail> GetRecordByKeyOIs(string order_no)
        {
            try
            {
                return context.Order_OI_STI_Detail.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        
        public Boolean SaveOIs(string order_no)
        {

            try
            {
                if (GetRecordByKeyOIs(order_no).Count > 0)
                {
                    List<Order_OI_STI_Detail> m = new List<Order_OI_STI_Detail>();
                    m = GetRecordByKeyOIs(order_no);
                    //DateTime eDate = m.;
                    DateTime date = System.Convert.ToDateTime(context.Order_OI_STI_Header.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    foreach (var n in m)
                    {
                      if(n.Quantity_Allocated > 0)
                        {
                            DateTime pDate = date;// System.Convert.ToDateTime(n.Order_OI_STI_Header.DatePrepared);
                            int? facility = n.FacilityCode;
                            int productcode = n.ProductCode;
                            var sapcode = context.A_Product.FirstOrDefault(a => a.product_code == n.ProductCode);
                            if (sapcode.SAP_code != null)
                            {
                                productcode = Convert.ToInt32(sapcode.SAP_code);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.Quantity_Allocated),
                                QuantityOrdered = Convert.ToInt32(n.QuantityRequired),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };
                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                    }

                    return true;
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

        // SMC
        public List<Order_SMC_SLM> GetRecordByKeyMSC(string order_no)
        {
            try
            {
                return context.Order_SMC_SLM.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        
        public Boolean SaveSMC(string order_no)
        {

            try
            {
                if (GetRecordByKeyMSC(order_no).Count > 0)
                {
                    List<Order_SMC_SLM> m = new List<Order_SMC_SLM>();
                    m = GetRecordByKeyMSC(order_no);
                    //DateTime eDate = m.;
                    DateTime date = System.Convert.ToDateTime(context.Order_SMC_SLM_Header.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    foreach (var n in m)
                    {
                      if(n.QuantityAllocated > 0)
                        {
                            DateTime pDate = date;// System.Convert.ToDateTime(n.Order_SMC_SLM_Header.DatePrepared);
                            int? facility = n.FacilityCode;
                            int productcode = n.ProductCode;
                            var sapcode = context.A_Product.FirstOrDefault(a => a.product_code == n.ProductCode);
                            if (sapcode.SAP_code != null)
                            {
                                productcode = Convert.ToInt32(sapcode.SAP_code);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.QuantityAllocated),
                                QuantityOrdered = Convert.ToInt32(n.QuantityToOrder),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };
                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                    }

                    return true;
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

        // Viral load
        public List<Order_ViralLoadReagents_Detail> GetRecordByKeyViralLoad(string order_no)
        {
            try
            {
                return context.Order_ViralLoadReagents_Detail.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
        
        public Boolean SaveViralLoad(string order_no)
        {

            try
            {
                if (GetRecordByKeyViralLoad(order_no).Count > 0)
                {
                    List<Order_ViralLoadReagents_Detail> m = new List<Order_ViralLoadReagents_Detail>();
                    m = GetRecordByKeyViralLoad(order_no);
                    //DateTime eDate = m.;
                    DateTime date = System.Convert.ToDateTime(context.Order_ViralLoadReagents_Header.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    foreach (var n in m)
                    {
                      if(n.Quantity_Allocated > 0)
                        {
                            DateTime pDate = date;// System.Convert.ToDateTime(n.Order_ViralLoadReagents_Header.DatePrepared);
                            int? facility = n.FacilityCode;
                            int productcode = n.ProductCode;
                            var sapcode = context.A_Product.FirstOrDefault(a => a.product_code == n.ProductCode);
                            if (sapcode.SAP_code != null)
                            {
                                productcode = Convert.ToInt32(sapcode.SAP_code);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.Quantity_Allocated),
                                QuantityOrdered = Convert.ToInt32(n.QuantityOrdered),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };
                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                    }

                    return true;
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
        public List<Order_TB> GetRecordByKeyTB(string order_no)
        {
            try
            {
                return context.Order_TB.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean SaveTB(string order_no)
        {

            try
            {
                if (GetRecordByKeyTB(order_no).Count > 0)
                {
                    List<Order_TB> m = new List<Order_TB>();
                    m = GetRecordByKeyTB(order_no);
                    //DateTime eDate = m.;
                    DateTime date = System.Convert.ToDateTime(context.Order_TB_Header.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    foreach (var n in m)
                    {
                        if(n.Quantity_Allocated > 0)
                        {
                            DateTime pDate = date;// System.Convert.ToDateTime(n.Order_ViralLoadReagents_Header.DatePrepared);
                            int? facility = n.FacilityCode;
                            int productcode = n.ProductCode;
                            var sapcode = context.A_Product.FirstOrDefault(a => a.product_code == n.ProductCode);
                            if (sapcode.SAP_code != null)
                            {
                                productcode = Convert.ToInt32(sapcode.SAP_code);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.Quantity_Allocated),
                                QuantityOrdered = Convert.ToInt32(n.QuantityRequired),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };
                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                    }

                    return true;
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
        public List<Order_RUTF_Details> GetRecordByKeyRUtf(string order_no)
        {
            try
            {
                return context.Order_RUTF_Details.Where(s => s.OrderNumber == order_no).ToList();
            }
            catch (Exception x)
            {
                throw (x);
            }
        }

        public Boolean SaveRutf(string order_no)
        {

            try
            {
                if (GetRecordByKeyRUtf(order_no).Count > 0)
                {
                    List<Order_RUTF_Details> m = new List<Order_RUTF_Details>();
                    m = GetRecordByKeyRUtf(order_no);
                    //DateTime eDate = m.;
                    DateTime date = System.Convert.ToDateTime(context.Order_RUTF_Header.FirstOrDefault(a => a.OrderNumber == order_no).DatePrepared);

                    foreach (var n in m)
                    {
                        if(n.Quantity_Allocated > 0)
                        {
                            DateTime pDate = date;// System.Convert.ToDateTime(n.Order_ViralLoadReagents_Header.DatePrepared);
                            int? facility = n.FacilityCode;
                            int productcode = n.ProductCode;
                            var sapcode = context.A_Product.FirstOrDefault(a => a.product_code == n.ProductCode);
                            if (sapcode.SAP_code != null)
                            {
                                productcode = Convert.ToInt32(sapcode.SAP_code);
                            }
                            var x = new SAP_ExportDocumentLines
                            {
                                OrderNumber = n.OrderNumber,
                                ItemCode = productcode,
                                CurrentRecord = true,
                                RecordReadBySAP = false,
                                DocNum = System.Convert.ToString(System.Convert.ToDateTime(pDate).Day + "" + System.Convert.ToDateTime(pDate).Month + "" + System.Convert.ToDateTime(pDate).Year + "" + facility),
                                QuantityAllocated = Convert.ToInt32(n.Quantity_Allocated),
                                QuantityOrdered = Convert.ToInt32(n.QuantityRequired),
                                CurrentRecordOnSAP = true,
                                AddedDate = System.DateTime.Now,
                                AddedBy = new UserManagement().getCurrentuser(),
                            };
                            if (GetRecord(order_no, x.ItemCode).Count > 0)
                            {

                            }
                            else
                            {
                                context.SAP_ExportDocumentLines.Add(x);
                                context.SaveChanges();
                            }
                        }
                    }

                    return true;
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