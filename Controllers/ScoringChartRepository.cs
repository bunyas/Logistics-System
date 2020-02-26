using mascis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Controllers
{
    public class ScoringChartRepository
    {
        private List<Scoring_Range_Chart> Scores_Chart;
        public List<Scoring_Range_Chart> Scores_ChartCollection
        {
            get { return Scores_Chart; }
            set { this.Scores_Chart = value; }
        }
        public ScoringChartRepository()
        {
            Scores_Chart = new List<Scoring_Range_Chart>();
        }
        public void LoadChart(List<Chss_Facility_Visits> visits)
        {
            mascisEntities db = new mascisEntities();
            var Facilities = db.Chss_M_Facilities.ToList();
            List<Chss_Facility_Visits> final_Visits = new List<Chss_Facility_Visits>();
            foreach(var n in Facilities)
            {
                var x = visits.Where(e => e.facility_code == n.FacilityCode).OrderBy(e=> e.date_of_visit).ToList();
                if(x.Count > 0)
                {
                    final_Visits.Add(x[0]);
                }
            }

        }
    }
}