using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mascis.Models
{
   
    public class MembersRepository
    {
        //BantwanaEntities
        public IEnumerable<SelectListItem> GetHH_Members()
        {
            List<SelectListItem> HH_Members = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            return HH_Members;
        }

        public IEnumerable<SelectListItem> GetHH_Members(string hh_id)
        {
            if (!String.IsNullOrWhiteSpace(hh_id.ToString()))
            {
                using (var context = new mascisEntities())
                {
                    IEnumerable<SelectListItem> HH_Members = context.View_WebTemplate_Laboratory.AsNoTracking()
                        .OrderBy(n => n.product_order)
                        .Where(n => n.original_product_code.ToString() == hh_id)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.product_order.ToString(),
                               Text = n.product_description
                           }).Distinct().ToList();
                    return new SelectList(HH_Members, "Value", "Text");
                }
            }
            return null;
        }

       

        //public IList<core_householdmember> GetAllRecords(string memID, int hh_id)
        //{
        //    IList<core_householdmember> core_householdmember = (IList<core_householdmember>)HttpContext.Current.Session["core_householdmember"];

        //    if (core_householdmember == null)
        //    {
        //        HttpContext.Current.Session["core_householdmember"] = core_householdmember = (from core_hhm in new BantwanaEntities().core_householdmember. 
        //                                                                                      Where(n => n.id_number == memID && n.household_id == hh_id) // .Take(50)
        //                                                        select new core_householdmember
        //                                                        {
        //                                                            id=core_hhm.id ,
        //                                                            name=core_hhm.name,
        //                                                            sex=core_hhm.sex,
        //                                                            year_of_birth=core_hhm.year_of_birth, 
        //                                                        }).ToList();

        //    }
        //    return core_householdmember;
        //}
    }



}