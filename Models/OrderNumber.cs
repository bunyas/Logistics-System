using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace mascis.Models
{
    public class OrderNumber
    {
        mascisEntities context = new mascisEntities();
        public string GetOrderNumber(DateTime _date,int facility,string product_category,string facility_type)
        {
            try
            {
                string sap_code = GetFacilitySAPCode(facility);
                string year = _date.Year.ToString().Substring(2, 2);

                string month = _date.Month.ToString();
                if (month.Length < 2)
                    month = "0" + month;

                string day = _date.Day.ToString();
                if (day.Length < 2)
                    day = "0" + day;

                if (product_category.Length < 2)
                    product_category = "0" + product_category;

                string partial_no = "";
                //partial_no = facility_type + sap_code + month + day + year + product_category;
                partial_no = facility_type + sap_code + day + month + year + product_category;//Day + Month + Year
                string _number = "";
                if (facility_type == "02")
                {
                    switch (product_category)
                    {
                        case "01":
                            _number = GetLab(partial_no);
                            break;
                        case "02":
                            _number = GetART(partial_no);
                            break;
                        case "03":
                            _number = GetHIVTestKits(partial_no);
                            break;
                        case "09":
                            _number = GetOI(partial_no);
                            break;
                        case "10":
                            _number = GetSMC(partial_no);
                            break;
                        case "11":
                            _number = GetViralLoad(partial_no);
                            break;
                        case "12":
                            _number = GetRUTF(partial_no);
                            break;
                        case "13":
                            _number = GetTB(partial_no);
                            break;
                    }
                }
                if (facility_type == "01")
                {
                    _number = GetMAULT(partial_no);
                }
                    return partial_no.Trim() + _number.Trim();
            }
            catch(Exception e)
            {
                throw (e);
            }
        }

        private string GetFacilitySAPCode(int facility_code)
        {
            try
            {
                var x = context.A_Facilities.FirstOrDefault(a => a.FacilityCode == facility_code);
                if (x != null)
                    return
                        x.SAP_Code;
                else
                    return "";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string GetART(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_ARTGetAll1(order).LastOrDefault();
                if (x!=null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string GetHIVTestKits(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_HIVTestKitsGetAll1(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string GetLab(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_LaboratoryGetAll1(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string GetOI(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_OIGetAll1(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string GetSMC(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_SMCGetAll1(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string GetViralLoad(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_ViralLoadGetAll1(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        private string GetRUTF(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_RutfGetAll(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        private string GetTB(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_TBGetAll(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        private string GetMAULT(string order)
        {
            try
            {
                var x = context.spView_OrderNumber_MAULTGetAll1(order).FirstOrDefault();
                if (x != null)
                {
                    return Increment(x.OrderIndex);
                }
                else
                    return "001";
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private string Increment(string no)
        {
            string b = "";
            int a = System.Convert.ToInt32(no) + 1;
            b = a.ToString();
            do
            {
                b = "0" + b;
            }
            while (b.Length < 3);
            return b;
        }
    }
}