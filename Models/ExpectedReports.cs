using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
 using System.Collections;
 using System.Web.Mvc;
using mascis.Models;
using System.Linq.Expressions;
using System.Data.Entity;
using mascis.ScheduledTasks;
using Newtonsoft.Json;
using System.Reflection;
using Syncfusion.JavaScript.Models;
using System.Web.Script.Serialization;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using System.Data.Entity.Validation;
namespace mascis.Models
{
    public class ExpectedReports
    {
        mascisEntities context = new mascisEntities();
        private int mfacility_code;
        private int mproduct_category;
        
        private DateTime mstart_date;
        private DateTime mend_date;

        public int facility_code { get { return mfacility_code; } set { mfacility_code = value; } }
        public int product_category { get { return mproduct_category; } set { mproduct_category = value; } }
        public DateTime start_date { get { return mstart_date; } set { mstart_date = value; } }
        public DateTime end_date { get { return mend_date; } set { mend_date = value; } }
        
        public LMIS_ExpectedReport GetRecordByKey(int facility_code, int product_category, DateTime start_date, DateTime end_date)
        {
            try
            {
                return context.LMIS_ExpectedReport.FirstOrDefault(s => s.facility_code == facility_code && s.product_category== product_category 
                && s.start_date== start_date && s.end_date== end_date);
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
                if (GetRecordByKey(mfacility_code,mproduct_category,mstart_date,mend_date) == null)
                {
                    var x = new LMIS_ExpectedReport
                    {
                        facility_code = mfacility_code,
                        start_date = mstart_date,
                        end_date = mend_date,
                        product_category = mproduct_category
                       
                    };
                    context.LMIS_ExpectedReport.Add(x);
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
                var x = GetRecordByKey(mfacility_code, mproduct_category, mstart_date, mend_date);
                if (x != null)
                {
                    x.facility_code = mfacility_code;
                    x.start_date = mstart_date;
                    x.end_date = mend_date;
                    x.product_category = mproduct_category;
                    context.SaveChanges();
                }
                else
                {
                    Save();
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