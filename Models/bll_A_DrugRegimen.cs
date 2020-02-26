using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class bll_A_DrugRegimen
    {
        mascisEntities context = new mascisEntities();

        public A_DrugRegimen getRecord(string regimen, int regimen_classification, int category, Boolean standard_regimen)
        {
            try
            {
                var regimens = context.A_DrugRegimen.FirstOrDefault(s => s.RegimenDesc.Trim().Equals(regimen.Trim(), StringComparison.CurrentCultureIgnoreCase) && s.RegimenClassification == regimen_classification && s.StandardRegimen == standard_regimen && s.RegimenCategoryCode == category);
                //if(regimens==null)
                //{
                //    regimens = context.A_DrugRegimen.OrderBy(c => c.RegimenCode)
                //        .FirstOrDefault(s => s.RegimenDesc.Trim().Equals(regimen.Trim(), StringComparison.CurrentCultureIgnoreCase));
                //}
                //return context.A_DrugRegimen.FirstOrDefault(s => s.RegimenDesc.Trim().Equals(regimen.Trim(), StringComparison.CurrentCultureIgnoreCase) && s.RegimenClassification == regimen_classification && s.StandardRegimen == standard_regimen && s.RegimenCategoryCode == category);
                return regimens;// context.A_DrugRegimen.FirstOrDefault(s => s.RegimenDesc.Trim().Equals(regimen.Trim(), StringComparison.CurrentCultureIgnoreCase) && s.RegimenClassification == regimen_classification && s.StandardRegimen == standard_regimen && s.RegimenCategoryCode == category);
            }
            catch (Exception x)
            {
                throw (x);
            }
        }
    }
}