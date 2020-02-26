using mascis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Controllers
{
    public class ScoringRepository
    {
        int count = 0;
        int Missing_ART, Missing_Stock, Missing_Traceability, Missing_Ordering, Missing_Store, Missing_Expiry;
        //double ART_Score = 0.0, Stock_Score = 0.0, Traceability_Score = 0.0, Ordering_Score = 0.0, Store_Score = 0.0, Expiry_Score = 0.0;
        double ART_Score = 0.0, Stock_Score = 0.0, Traceability_Score = 0.0, Store_Score = 0.0, Ordering_Score = 0.0, Expiry_Score = 0.0;
        Scoring_Summary summary;
        Scoring_Range_Chart ChartScore;
        Scoring_Range_Chart ChartScore1;
        private List<Scoring_Range_Chart> Scores_Chart;
        public List<Scoring_Range_Chart> Scores_ChartCollection
        {
            get { return Scores_Chart; }
            set { this.Scores_Chart = value; }
        }
        private List<Scoring_Summary> Scores;
        public List<Scoring_Summary> ScoresCollection
        {
            get { return Scores; }
            set { this.Scores = value; }
        }
        public ScoringRepository()
        {
            Scores = new List<Scoring_Summary>();
            Scores_Chart = new List<Scoring_Range_Chart>();
        }
        public void LoadScores(List<Chss_Facility_Visits> visits)
        {
            ChartScore = new Scoring_Range_Chart();
            ChartScore1 = new Scoring_Range_Chart();
            int Above_20 = 0; int Below_20 = 0;

            foreach (var n in visits)
            {
                summary = new Scoring_Summary();
                count = 0;
                Missing_ART = 0; Missing_Stock = 0; Missing_Traceability = 0; Missing_Ordering = 0; Missing_Store = 0; Missing_Expiry = 0; ;
                ART_Score = 0.0; Stock_Score = 0.0; Traceability_Score = 0.0;
                Ordering_Score = 0.0; Store_Score = 0.0; Expiry_Score = 0.0;
                LoadFacilityDetails(n.facility_code, n.date_of_visit);
                Load_ART_Patient_Care_Tools_ScoresAsync(n.facility_code, n.date_of_visit);
                Load_ART_Patient_Care_Register_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_ART_Patient_Care_DispensingLog_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_ART_Patient_Care_Treatment_GuideAsync(n.facility_code, n.date_of_visit);
                Load_Stock_ManagementAsync(n.facility_code, n.date_of_visit);
                Load_Requisitioning_System_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_From_Warehouse_To_Facility_Store_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_From_Facility_To_DispensingUnit_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_Issued_To_Patients_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_Order_Means_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_Order_Accuracy_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_Order_Patient_Report_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_Order_Test_Kits_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_StoreCleanliness_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_StorageSystem_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_StorageCondition_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_StoragePractice_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_FEFOUse_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_ExpiredCommodityPresence_ScoreAsync(n.facility_code, n.date_of_visit);
                Load_TrackingTools_ScoreAsync(n.facility_code, n.date_of_visit);
                summary.Final_Total = Math.Round((summary.ART_Total + summary.Order_Total + summary.Stock_Total + summary.Traceability_Total), 3);
                summary.Final_Spars_Total = Math.Round((summary.ART_Total + summary.Expiry_Total + summary.Order_Total + summary.Stock_Total + summary.Store_Total + summary.Traceability_Total), 3);
                double spars = 0.0;
                if (summary.Final_Spars_Total > 0) { spars = (summary.Final_Spars_Total / 25) * 100; }
                summary.Spars_Score = Math.Round(spars, 3) + "%";
                double score = 0.0;
                if (summary.Final_Total > 0) { score = (summary.Final_Total / 18) * 100; }
                summary.Score = Math.Round(score, 3) + "%";
                if (summary.Final_Spars_Total >= 20)
                {
                    Above_20 += 1;
                }
                if (summary.Final_Spars_Total < 20)
                {
                    Below_20 += 1;
                }
                if (count == 1)
                {
                    Scores.Add(summary);
                }

            }
            ChartScore.Range_name = "below 20";
            ChartScore.Percentage = Below_20;
            Scores_Chart.Add(ChartScore);
            ChartScore1.Range_name = "Above 20";
            ChartScore1.Percentage = Above_20;
            Scores_Chart.Add(ChartScore1);
        }
        private void LoadFacilityDetails(int facilityid, DateTime visitdate)
        {
            mascisEntities db = new mascisEntities();
            var a = db.Chss_M_Facilities.ToList();
            var x = a.Find(e => e.FacilityCode == facilityid);
            if (x != null)
            {
                count = 1;
                summary.CDCRegion = x.CDCRegion;
                summary.CDCRegionId = x.CDCRegionId;
                summary.ComprehensiveImplimentingPartnerCode = x.ComprehensiveImplimentingPartnerCode;
                summary.ComprehensiveImplimentingPartnerDescription = x.ComprehensiveImplimentingPartnerDescription;
                summary.date_of_visit = visitdate;
                summary.District_Name = x.District_Name;
                summary.DistrrictCode = x.DistrrictCode;
                summary.Facility = x.Facility;
                summary.facility_code = x.FacilityCode;
                summary.ImplimentingPartnerCode = x.ImplimentingPartnerCode;
                summary.ImplimentingPartnerDescription = x.ImplimentingPartnerDescription;
                summary.Level_desc = x.Level_Desc;
                summary.level_of_care = x.level_of_care;
                summary.SAP_Code = x.SAP_Code;
                summary.SCTO = x.SCTO;
            }
        }
        private void Load_ART_Patient_Care_Tools_ScoresAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_ART_Patient_Care_Availability.ToList();

                List<Chss_M_ART_Patient_Care_Availability> Tools = new List<Chss_M_ART_Patient_Care_Availability>();
                Tools = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
                
                if (Tools.Count() > 0)
                {
                    double Score = 0.0;
                    int ToolsCount = 0;
                    foreach (var t in Tools)
                    {
                        //int result = 0;
                        //if (t.Tool_Availability == 1)
                        //{
                        //    result = 1;
                        //}
                        //else
                        //{
                        //    result = 0;
                        //}
                        //ToolsCount += result;
                    }
                    Score = (float)(ToolsCount) / Tools.Count;
                    ART_Score += Score;
                    summary.ART_Availability = Score.ToString();
                }
                else
                {
                    Missing_ART += 1;
                    double Score = 0.0;
                    ART_Score += Score;
                    summary.ART_Availability = "NA";
                }
            }
            catch(Exception ex) { throw(ex); }
        }
        private void Load_ART_Patient_Care_Register_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_ART_Patient_Care_Register.ToList();

                List<Chss_M_ART_Patient_Care_Register> Data = new List<Chss_M_ART_Patient_Care_Register>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
                if (Data.Count > 0)
                {
                    double finalScore = 0.00;
                    int ToolsCount = 0;
                    int DataCount = Data.Count;
                    foreach (var t in Data)
                    {
                        int date = 0, Pno = 0, Pname = 0, Pweight = 0, Page = 0, Sdate = 0, Original = 0, current = 0;
                        if (t.Current_Regimen == "1") { current = 1; }
                        if (t.Dapsone == "1") { Sdate = 1; }
                        if (t.Original_Regimen == "1") { Original = 1; }
                        if (t.Patient_Age == "1") { Page = 1; }
                        if (t.Patient_Name == "1") { Pname = 1; }
                        if (t.Patient_No == "1") { Pno = 1; }
                        if (t.Patient_Weight == "1") { Pweight = 1; }
                        if (t.Start_Date == "1") { date = 1; }
                        ToolsCount += (date + Pno + Pname + Pweight + Page + Sdate + Original + current);
                    }
                    double Score = ((float)(ToolsCount) / ((float)(DataCount) * 8)) * 100;

                    if (Score == 100) { finalScore = 1.0; }
                    else if (Score < 75) { finalScore = 0.0; }
                    else if (Score >= 75 && Score < 100) { finalScore = 0.5; }
                    ART_Score += finalScore;
                    summary.ART_Register = finalScore.ToString();
                }
                else
                {
                    Missing_ART += 1;
                    double finalScore = 0.00;
                    ART_Score += finalScore;
                    summary.ART_Register = "NA";
                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_ART_Patient_Care_DispensingLog_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_ART_Patient_Care_DispensingLog.ToList();
                List<Chss_M_ART_Patient_Care_DispensingLog> Data = new List<Chss_M_ART_Patient_Care_DispensingLog>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
                
                if (Data.Count > 0)
                {
                    double finalScore = 0.00;
                    int ToolsCount = 0;
                    int DataCount = Data.Count;
                    foreach (var t in Data)
                    {
                        int date = 0, Pno = 0, Pname = 0, amount = 0;
                        if (t.Dispensing_Date == "1") { date = 1; }
                        if (t.Amount_Dispensed == "1") { amount = 1; }
                        if (t.Patient_No == "1") { Pno = 1; }
                        if (t.Medicine_Name == "1") { Pname = 1; }

                        ToolsCount += (date + Pno + Pname + amount);
                    }
                    double Score = ((float)(ToolsCount) / ((float)(DataCount) * 4)) * 100;

                    if (Score == 100) { finalScore = 1.0; }
                    else if (Score < 75) { finalScore = 0.0; }
                    else if (Score >= 75 && Score < 100) { finalScore = 0.5; }
                    ART_Score += finalScore;
                    summary.ART_DispensingLog = finalScore.ToString();
                }
                else
                {
                    Missing_ART += 1;
                    double finalScore = 0.00;
                    ART_Score += finalScore;
                    summary.ART_DispensingLog = "NA";
                }
            }
            catch { throw; }
        }
        private class RegCodes
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
                List<RegCodes> codes = new List<RegCodes>()
                {
                    new RegCodes(){ ID=2,Name="TDF/3TC/EFV"},
                    new RegCodes(){ ID=117,Name="TDF/3TC+DTG"},
                    new RegCodes(){ ID=5,Name="ABC/3TC/EFV"},
                    new RegCodes(){ ID=14,Name="ABC/3TC/EFV"},
                    new RegCodes(){ ID=13,Name="ABC/3TC/NVP"},
                    new RegCodes(){ ID=18,Name="ABC+3TC+LPV/r"},
                    new RegCodes(){ ID=26,Name="ABC+3TC+LPV/r"},
                    new RegCodes(){ ID=119,Name="ABC+3TC+LPV/r"},
                    //new RegCodes(){ ID=17,Name="AZT/3TC/LPV/r"},
                };
        private void Load_ART_Patient_Care_Treatment_GuideAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                TreatmentAvailable = 0;
                 mascisEntities db = new mascisEntities();
                var a = db.Chss_M_ART_Patient_Care_Treatment.ToList();

                List<Chss_M_ART_Patient_Care_Treatment> Data = new List<Chss_M_ART_Patient_Care_Treatment>();
                Data = a.Where(e => e.FacilityCode == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
                
                if (Data.Count > 0)
                {
                    var p = Data.Where(e => e.regimen_classification_code == 2).ToList();
                    var f = Data.Where(e => e.regimen_classification_code == 1).ToList();
                    double FinalScore = 0.0;
                    int sum = 0;
                    sum += Children(p);
                    sum += Adolescent(f);
                    if (TreatmentAvailable == 1)
                    {
                        FinalScore = (float)(sum);
                    }
                    else if (TreatmentAvailable == 0)
                    {
                        FinalScore = (float)(sum) / 2;
                    }
                    else
                    {
                        FinalScore = 0.0;
                    }
                    ART_Score += FinalScore;
                    summary.ART_Treatment = FinalScore.ToString();
                    summary.ART_Total = Math.Round(ART_Score, 2);
                    int z = 4 - Missing_ART;
                    double x = (float)((float)(ART_Score) / z);
                    double Spars_Score = x * 5;
                    summary.ART_Spars_Total = Math.Round(Spars_Score, 2);
                }
                else
                {
                    Missing_ART += 1;
                    double FinalScore = 0.0;
                    ART_Score += FinalScore;
                    summary.ART_Treatment = "NA";
                    summary.ART_Total = Math.Round(ART_Score, 2);
                    double Spars_Score = 0.0;
                    if (ART_Score == 0)
                    {
                        Spars_Score = 0.0;
                        summary.ART_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {
                        int z = 4 - Missing_ART;
                        if (z == 0)
                        {
                            Spars_Score = 0.0;
                        }
                        else { Spars_Score = (ART_Score / z) * 5; }
                        summary.ART_Spars_Total = Math.Round(Spars_Score, 2);
                    }

                }
            }
            catch { throw; }

        }
        int TreatmentAvailable;
        private int Children(List<Chss_M_ART_Patient_Care_Treatment> Data)
        {
            int FinalScore = 0;
            int sum = 0;
            try
            {
                if (Data.Count > 0)
                {
                    foreach (var n in Data)
                    {
                       // var a = codes.FirstOrDefault(e => e.ID == n.RegimenCode);
                        //if (a != null)
                        //{
                        //    sum += 1;
                        //}
                    }
                    double Score = ((float)(sum) / Data.Count) * 100;
                    if (Score >= 100) { FinalScore = 1; }
                }
                else
                {
                    TreatmentAvailable += 1;
                }
                return FinalScore;

            }
            catch { return FinalScore; }
        }
        private int Adolescent(List<Chss_M_ART_Patient_Care_Treatment> Data)
        {
            int FinalScore = 0;
            int sum = 0;
            try
            {
                if (Data.Count > 0)
                {
                    foreach (var n in Data)
                    {
                       // var a = codes.FirstOrDefault(e => e.ID == n.RegimenCode);
                        //if (a != null)
                        //{
                        //    sum += 1;
                        //}
                    }
                    double Score = ((float)(sum) / Data.Count) * 100;
                    if (Score >= 100) { FinalScore = 1; }
                }
                else
                {
                    TreatmentAvailable += 1;
                }
                return FinalScore;

            }
            catch { return FinalScore; }
        }
        private void Load_Stock_ManagementAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Stock_Management.ToList();

                List<Chss_M_Stock_Management> Data = new List<Chss_M_Stock_Management>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate).ToList();
                double FinalphysicalScore = 0.0;
                double FinalcardavailabilityScore = 0.0;
                double FinalStockCardComparisonScore = 0.0;
                double FinalCorrectstockBookUseScore = 0.0;
                double FinalAmcComparisonScore = 0.0;
                double FinalCorrectCardFillScore = 0.0;
                if (Data.Count > 0)
                {
                    int Missing_Physical = 0, Missing_Card = 0, Missing_Card_Fil = 0, Missing_Stock_Book = 0, Missing_Balance_Comp = 0;
                    int physicalScore = 0, cardavailabilityScore = 0, CorrectCardFillScore = 0, StockCardComparisonScore = 0, CorrectstockBookUseScore = 0, AmcComparisonScore = 0;
                    foreach (var n in Data)
                    {
                        if (!n.AMC.Contains("NA") && !n.AMC.Contains("NR")) { if (Math.Round(Convert.ToDouble(n.AMC), 3) == Math.Round(Convert.ToDouble(n.Calculated_AMC), 3)) { AmcComparisonScore += 1; } }
                        if (n.Monthly_Physical_Count == "1") { physicalScore += 1; }
                        if (n.Monthly_Physical_Count == "NA") { Missing_Physical += 1; }
                        if (n.Card_Availability == "1") { cardavailabilityScore += 1; }
                        if (n.Card_Availability == "NA") { Missing_Card += 1; }
                        if (n.Correct_Card_Fil == "1") { CorrectCardFillScore += 1; }
                        if (n.Correct_Card_Fil == "NA") { Missing_Card_Fil += 1; }
                        if (n.Correct_Stock_Book_Use == "1") { CorrectstockBookUseScore += 1; }
                        if (n.Correct_Stock_Book_Use == "NA") { Missing_Stock_Book += 1; }
                        if (n.Stock_Card_Balance == "NA") { Missing_Balance_Comp += 1; }
                        if (n.Stock_Card_Balance == n.Store_Quantity.ToString()) { StockCardComparisonScore += 1; }

                    }
                    int StockCount = Data.Count;
                    if (Missing_Physical == StockCount) { Missing_Stock += 1; }
                    if (Missing_Card == StockCount) { Missing_Stock += 1; }
                    if (Missing_Card_Fil == StockCount) { Missing_Stock += 1; }
                    //if (Missing_Stock_Book == StockCount) { Missing_Stock += 1; }
                    if (Missing_Balance_Comp == StockCount) { Missing_Stock += 1; }

                    if (Missing_Physical != StockCount) { FinalphysicalScore = (float)(physicalScore) / (float)(StockCount - Missing_Physical); }
                    if (Missing_Card != StockCount) { FinalcardavailabilityScore = (float)(cardavailabilityScore) / (float)(StockCount - Missing_Card); }
                    if (Missing_Card_Fil != StockCount) { FinalCorrectCardFillScore = (float)(CorrectCardFillScore) / (float)(StockCount - Missing_Card_Fil); }
                    if (Missing_Stock_Book != StockCount) { FinalCorrectstockBookUseScore = (float)(CorrectstockBookUseScore) / (float)(StockCount - Missing_Stock_Book); }
                    if (Missing_Balance_Comp != StockCount) { FinalStockCardComparisonScore = (float)(StockCardComparisonScore) / (float)(StockCount - Missing_Balance_Comp); }
                    FinalAmcComparisonScore = (float)(AmcComparisonScore) / (float)(StockCount);
                    Stock_Score = FinalphysicalScore + FinalcardavailabilityScore + FinalStockCardComparisonScore + FinalCorrectstockBookUseScore + FinalAmcComparisonScore + FinalCorrectCardFillScore;

                    FinalAmcComparisonScore = (float)(AmcComparisonScore) / (float)(StockCount);
                    summary.Physical_Count = Math.Round(FinalphysicalScore, 3).ToString();
                    summary.Stock_Card = Math.Round(FinalcardavailabilityScore, 3).ToString();
                    summary.Stock_Card_Fill = Math.Round(FinalCorrectCardFillScore, 3).ToString();
                    summary.Updating_Stock_Card = Math.Round(FinalStockCardComparisonScore, 3).ToString();
                    summary.Stock_Book_Use = Math.Round(FinalCorrectstockBookUseScore, 3).ToString();
                    summary.AMC = Math.Round(FinalAmcComparisonScore, 3).ToString();
                    Stock_Score = FinalphysicalScore + FinalcardavailabilityScore + FinalCorrectCardFillScore + FinalStockCardComparisonScore + FinalCorrectstockBookUseScore + FinalAmcComparisonScore;

                    if (Stock_Score == 0 || double.IsNaN(Stock_Score))
                    {
                        summary.Stock_Total = 0.0;
                        summary.Stock_Spars_Total = 0.0;
                    }
                    else
                    {
                        summary.Stock_Total = Math.Round(Stock_Score, 2);
                        int m = 6 - Missing_Stock;
                        double d = (float)((float)Stock_Score / m);
                        double Score = d * 5;
                        summary.Stock_Spars_Total = Math.Round(Score, 2);
                    }

                }
                else
                {
                    Stock_Score = FinalphysicalScore + FinalcardavailabilityScore + FinalCorrectCardFillScore + FinalStockCardComparisonScore + FinalCorrectstockBookUseScore + FinalAmcComparisonScore;

                    summary.Physical_Count = "NA";
                    summary.Stock_Card = "NA";
                    summary.Stock_Card_Fill = "NA";
                    summary.Updating_Stock_Card = "NA";
                    summary.Stock_Book_Use = "NA";
                    summary.AMC = "NA";
                    Stock_Score = 0.0;
                    summary.Stock_Total = 0.0;
                    summary.Stock_Spars_Total = 0.0;
                }


            }
            catch { throw; }
        }
        private void Load_Requisitioning_System_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {

                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Traceability_Requisition_System.ToList();
                List<Chss_M_Traceability_Requisition_System> Data = new List<Chss_M_Traceability_Requisition_System>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
                double Finalscore = 0.0;
                if (Data.Count > 0)
                {
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Result == 1)
                        {
                            count += 1;
                        }
                    }
                    Finalscore = (float)(count) / (float)(Data.Count);
                    Traceability_Score += Finalscore;
                    summary.Requisition_System = Math.Round(Finalscore, 2).ToString();
                }
                else
                {
                    Missing_Traceability += 1;
                    Traceability_Score += Finalscore;
                    summary.Requisition_System = "NA";
                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_From_Warehouse_To_Facility_Store_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {

                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Traceability_Supplier_To_Facility_Store.ToList();

                List<Chss_M_Traceability_Supplier_To_Facility_Store> Data = new List<Chss_M_Traceability_Supplier_To_Facility_Store>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
                double Finalscore = 0.0;
                if (Data.Count > 0)
                {
                    int Batch = 0, Quantity = 0;
                    foreach (var n in Data)
                    {
                        if (string.IsNullOrEmpty(n.Received_Stock_Card_Note_Batch) || string.IsNullOrEmpty(n.Recent_Delivery_Note_Batch) || string.IsNullOrEmpty(n.Responding_Goods_Received_Batch))
                        {
                            Batch += 0;
                            Quantity += 0;
                        }
                        else
                        {
                            if (n.Received_Stock_Card_Note_Batch == n.Recent_Delivery_Note_Batch && n.Recent_Delivery_Note_Batch == n.Responding_Goods_Received_Batch)
                            {
                                Batch += 1;
                            }
                            if (n.Received_Stock_Card_Quantity == n.Recent_Delivery_Note_Quantity && n.Recent_Delivery_Note_Quantity == n.Responding_Goods_Received_Note_Quantity)
                            {
                                Quantity += 1;
                            }
                        }
                    }
                    double finalBatch = (float)(Batch) / (float)(Data.Count);
                    double finalQuantity = (float)(Quantity) / (float)(Data.Count);

                    Finalscore = (finalBatch + finalQuantity) / 2;
                    Traceability_Score += Finalscore;
                    summary.Supplier = Math.Round(Finalscore, 2).ToString();
                }
                else
                {
                    Missing_Traceability += 1;
                    Traceability_Score += Finalscore;
                    summary.Supplier = "NA";
                }

            }
            catch
            {
                throw;
            }
        }
        private void Load_From_Facility_To_DispensingUnit_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {

                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Traceability_Facility_Store_To_DispensingUnit.ToList();

                List<Chss_M_Traceability_Facility_Store_To_DispensingUnit> Data = new List<Chss_M_Traceability_Facility_Store_To_DispensingUnit>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.00;
               
                if (Data.Any())
                {
                    int Date = 0, Quantity = 0;
                    foreach (var n in Data)
                    {
                        if (n.Issue_Date == n.Stock_Card_Issue_date & n.Issue_Date != null & n.Stock_Card_Issue_date != null)
                        {
                            Date += 1;
                        }
                        if (n.Quantity_Issued == n.Stock_Card_Quantity_Issued & n.Quantity_Issued != null & n.Stock_Card_Quantity_Issued != null)
                        {
                            Quantity += 1;
                        }
                    }
                    double finalDate = (float)(Date) / (float)(Data.Count);
                    double finalQuantity = (float)(Quantity) / (float)(Data.Count);
                    double score = ((finalDate + finalQuantity) / 2) * 100;

                    if (score == 100) { Finalscore = 1.0; }
                    else if (score >= 75 && score <= 99) { Finalscore = 0.5; }
                    else { Finalscore = 0.0; }
                    Traceability_Score += Finalscore;
                    summary.Facility_Store = Math.Round(Finalscore, 2).ToString();
                }
                else
                {
                    Missing_Traceability += 1;
                    Traceability_Score += Finalscore;
                    summary.Facility_Store = "NA";
                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_Issued_To_Patients_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Traceability_Issued_To_Patients.ToList();

                List<Chss_M_Traceability_Issued_To_Patients> Data = new List<Chss_M_Traceability_Issued_To_Patients>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                if (Data.Any())
                {
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Guid_Id == 19 && n.Results == 1)
                        {
                            count += 1;
                        }
                    }
                    Finalscore = (float)(count);
                    Traceability_Score += Finalscore;
                    summary.Issued_to_Patients = Math.Round(Finalscore, 2).ToString();
                    summary.Traceability_Total = Math.Round(Traceability_Score, 2);
                    int z = 4 - Missing_Traceability;
                    double x = (float)((float)(Traceability_Score) / z);
                    double Spars_Score = x * 5;
                    summary.Traceability_Spars_Total = Math.Round(Spars_Score, 2);
                }
                else
                {
                    Missing_Traceability += 1;
                    Traceability_Score += Finalscore;
                    if (Traceability_Score == 0)
                    {
                        summary.Issued_to_Patients = "NA";
                        summary.Traceability_Total = Math.Round(Traceability_Score, 2);
                        double Spars_Score = 0.0;
                        summary.Traceability_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {
                        double Spars_Score = 0.0;
                        int z = 4 - Missing_Traceability;
                        summary.Issued_to_Patients = "NA";
                        summary.Traceability_Total = Math.Round(Traceability_Score, 2);
                        if (z > 0)
                        {
                            double x = (float)((float)(Traceability_Score) / z);
                            Spars_Score = x * 5;
                        }
                        summary.Traceability_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_Order_Means_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Order_Report.ToList();

                List<Chss_M_Order_Report> Data = new List<Chss_M_Order_Report>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                if (Data.Count > 0)
                {
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Guid_Id == 25 && n.Result == 1)
                        {
                            count += 1;
                        }
                    }
                    Finalscore = (float)(count);
                    Ordering_Score += Finalscore;
                    summary.Report_Availability = Math.Round(Finalscore, 2).ToString();
                }
                else
                {
                    Missing_Ordering += 1;
                    Ordering_Score += Finalscore;
                    summary.Report_Availability = "NA";
                }

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// comming back
        /// </summary>
        /// <param name="facilityid"></param>
        /// <param name="visitdate"></param>
        private void Load_Order_Accuracy_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                Comb = 0;
                double score = 0.0;
                score = Order_Accuracy_Score(facilityid, visitdate);
                score += Order_Balance_Comaprison(facilityid, visitdate);
                
                if (score > 0)
                {
                    score = (float)(score / 2);
                    Ordering_Score += score;
                    summary.Order_Accuracy = Math.Round(score, 2).ToString();
                }
                else
                {
                    if (Comb == 2) { Missing_Ordering += 1; }
                    score = 0.0;
                    Ordering_Score += score;
                    summary.Order_Accuracy = "NA";
                }

            }
            catch
            {
                throw;
            }
        }
        int Comb;
        private double Order_Accuracy_Score(int facilityid, DateTime visitdate)
        {
            double Finalscore = 0.0;
            mascisEntities db = new mascisEntities();
            var a = db.Chss_M_Order_Accuracy.ToList();

            List<Chss_M_Order_Accuracy> Data = new List<Chss_M_Order_Accuracy>();
            Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
            if (Data.Count > 0)
            {
                int itemcount = Data.Count;
                int count = 0;
                foreach (var n in Data)
                {
                    if (n.Availability == 1)
                    {
                        if (n.Dispensing_Quantity_Consumed == n.Quantity_Comsumed)
                        {
                            count += 1;
                        }
                    }
                    else
                    {
                        if (itemcount > 0)
                            itemcount = itemcount - 1;

                        else
                            itemcount = 0;
                    }
                }
                if (itemcount == 0)
                {
                    Finalscore = 0.0;
                    //score += Finalscore;
                }
                else
                {
                    Finalscore = (float)(count) / (float)(itemcount);
                    //score += Finalscore;
                }

            }
            else
            {
                Comb += 1;
                Finalscore = 0.0;
            }
            return Finalscore;
        }
        private double Order_Balance_Comaprison(int facilityid, DateTime visitdate)
        {
            double Finalscore = 0.0;
            mascisEntities db = new mascisEntities();
            var b = db.Chss_M_Order_Balance.ToList();

            List<Chss_M_Order_Balance> Data1 = new List<Chss_M_Order_Balance>();
            Data1 = b.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();
            if (Data1.Count > 0)
            {
                int itemcount = Data1.Count;
                int count = 0;
                foreach (var n in Data1)
                {
                    if (n.Availability == 1)
                    {
                        if (n.Opening_Balance == n.Closing_Balance)
                        {
                            count += 1;
                        }
                    }
                    else
                    {
                        if (itemcount > 0)
                            itemcount = itemcount - 1;

                        else
                            itemcount = 0;
                    }
                }
                if (itemcount == 0)
                {
                    Finalscore = 0.0;
                    //score += 0.0;
                }
                else
                {
                    Finalscore = (float)(count) / (float)(itemcount);
                    // score += Finalscore;
                }

            }
            else
            {
                Comb += 1;
                Finalscore = 0.0;
            }
            return Finalscore;
        }
        private void Load_Order_Patient_Report_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Order_Patients_Report.ToList();

                List<Chss_M_Order_Patients_Report> Data = new List<Chss_M_Order_Patients_Report>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                
                if (Data.Any())
                {
                    int itemcount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Availability == 1)
                        {
                            if (n.ArtRegister == n.OrderForm)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            if (itemcount == 0)
                            {
                                itemcount = 0;
                            }
                            else
                            {

                                itemcount = itemcount - 1;
                            }
                        }
                    }
                    if (itemcount == 0)
                    {
                        Missing_Ordering += 1;
                        Finalscore = 0.0;
                        Ordering_Score += Finalscore;
                        summary.Patient_report = Math.Round(Finalscore, 2).ToString();
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemcount);
                        Ordering_Score += Finalscore;
                        summary.Patient_report = Math.Round(Finalscore, 2).ToString();
                    }
                }
                else
                {
                    Missing_Ordering += 1;
                    Ordering_Score += Finalscore;
                    summary.Patient_report = "NA";

                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_Order_Test_Kits_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Order_TestKits.ToList();

                List<Chss_M_Order_TestKits> Data = new List<Chss_M_Order_TestKits>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                if (Data.Count > 0)
                {
                    int itemcount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Availability == 1)
                        {
                            if (n.Order_Quantity == n.Register_Quantity)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            if (itemcount == 0)
                            {
                                itemcount = 0;
                            }
                            else
                            {
                                itemcount = itemcount - 1;
                            }
                        }
                    }
                    if (itemcount == 0)
                    {
                        double Spars_Score = 0.0;
                        Missing_Ordering += 1;
                        Finalscore = 0.0;
                        Ordering_Score += Finalscore;
                        summary.Test_Kits = Math.Round(Finalscore, 2).ToString();
                        summary.Order_Total = Math.Round(Ordering_Score, 2);
                        int z = 4 - Missing_Ordering;
                        if (z > 0)
                        {
                            double x = (float)((float)(Ordering_Score) / z);
                            Spars_Score = x * 5;
                        }
                        summary.Order_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {

                        Finalscore = (float)(count) / (float)(itemcount);
                        Ordering_Score += Finalscore;
                        summary.Test_Kits = Math.Round(Finalscore, 2).ToString();
                        summary.Order_Total = Math.Round(Ordering_Score, 2);
                        int z = 4 - Missing_Ordering;
                        double x = (float)((float)(Ordering_Score) / z);
                        double Spars_Score = x * 5;
                        summary.Order_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                }
                else
                {
                    Ordering_Score += Finalscore;
                    if (Ordering_Score == 0)
                    {
                        summary.Test_Kits = "NA";
                        summary.Order_Total = Math.Round(Ordering_Score, 2);
                        double Spars_Score = 0.0;
                        summary.Order_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {
                        double Spars_Score = 0.0;
                        Missing_Ordering += 1;
                        summary.Test_Kits = "NA";
                        summary.Order_Total = Math.Round(Ordering_Score, 2);
                        int z = 4 - Missing_Ordering;
                        if (z > 0)
                        {
                            double x = (float)((float)(Ordering_Score) / z);
                            Spars_Score = x * 5;
                        }

                        summary.Order_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_StoreCleanliness_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Storage_Cleanliness.ToList();

                List<Chss_M_Storage_Cleanliness> Data = new List<Chss_M_Storage_Cleanliness>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                if (Data.Any())
                {
                    int itemCount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Score == "1")
                        {
                            count += 1;
                        }
                    }
                    if (itemCount == 0)
                    {
                        Missing_Store += 1;
                        Finalscore = 0.0;
                        Store_Score += Finalscore;
                        summary.Cleanliness = Math.Round(Finalscore, 2).ToString();
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemCount);
                        Store_Score += Finalscore;
                        summary.Cleanliness = Math.Round(Finalscore, 2).ToString();
                    }
                }
                else
                {
                    Missing_Store += 1;
                    Finalscore = 0.0;
                    Store_Score += Finalscore;
                    summary.Cleanliness = "NA";
                }

            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }
        private void Load_StorageSystem_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Storage_System.ToList();

                List<Chss_M_Storage_System> Data = new List<Chss_M_Storage_System>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                
                if (Data.Any())
                {
                    int itemCount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Score == "1")
                        {
                            count += 1;
                        }
                    }
                    if (itemCount == 0)
                    {
                        Missing_Store += 1;
                        Finalscore = 0.0;
                        Store_Score += Finalscore;
                        summary.Storage_System = Math.Round(Finalscore, 2).ToString();
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemCount);
                        Store_Score += Finalscore;
                        summary.Storage_System = Math.Round(Finalscore, 2).ToString();
                    }
                }
                else
                {
                    Missing_Store += 1;
                    Finalscore = 0.0;
                    Store_Score += Finalscore;
                    summary.Storage_System = "NA";

                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_StorageCondition_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Store_Condition.ToList();

                List<Chss_M_Store_Condition> Data = new List<Chss_M_Store_Condition>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
               
                if (Data.Any())
                {
                    int itemCount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Score == "NA")
                        {
                            if (itemCount == 0)
                            {
                                itemCount = 0;
                            }
                            else
                            {
                                itemCount = itemCount - 1;
                            }

                        }
                        else if (n.Score == "1")
                        {
                            count += 1;
                        }
                    }
                    if (itemCount == 0)
                    {
                        Missing_Store += 1;
                        Finalscore = 0.0;
                        Store_Score += Finalscore;
                        summary.Storage_Condition = Math.Round(Finalscore, 2).ToString();
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemCount);
                        Store_Score += Finalscore;
                        summary.Storage_Condition = Math.Round(Finalscore, 2).ToString();
                    }
                }
                else
                {
                    Missing_Store += 1;
                    Finalscore = 0.0;
                    Store_Score += Finalscore;
                    summary.Storage_Condition = "NA";

                }
            }
            catch
            {
                throw;
            }
        }
        private void Load_StoragePractice_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Storage_Practice.ToList();

                List<Chss_M_Storage_Practice> Data = new List<Chss_M_Storage_Practice>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                
                if (Data.Any())
                {

                    int itemCount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Score == "NA")
                        {
                            if (itemCount == 0)
                            {
                                itemCount = 0;
                            }
                            else
                            {
                                itemCount = itemCount - 1;
                            }

                        }
                        else if (n.Score == "1")
                        {
                            count += 1;
                        }
                    }
                    if (itemCount == 0)
                    {
                        double Spars_Score = 0.0;
                        int z = 4 - Missing_Store;
                        Missing_Store += 1;
                        Finalscore = 0.0;
                        Store_Score += Finalscore;
                        summary.Storage_Practice = Math.Round(Finalscore, 2).ToString();
                        summary.Store_Total = Math.Round(Store_Score, 2);
                        if (z > 0)
                        {
                            double x = (float)((float)(Store_Score) / z);
                            Spars_Score = x * 5;
                        }
                        summary.Store_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemCount);
                        Store_Score += Finalscore;
                        summary.Storage_Practice = Math.Round(Finalscore, 2).ToString();
                        summary.Store_Total = Math.Round(Store_Score, 2);
                        int z = 4 - Missing_Store;
                        double x = (float)((float)(Store_Score) / z);
                        double Spars_Score = x * 5;
                        summary.Store_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                }
                else
                {
                    Missing_Store += 1;
                    Finalscore = 0.0;
                    Store_Score += Finalscore;
                    if (Store_Score == 0)
                    {
                        summary.Storage_Practice = "NA";
                        summary.Store_Total = Math.Round(Store_Score, 2);
                        double Spars_Score = 0.0;
                        summary.Store_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {
                        double Spars_Score = 0.0;
                        int z = 4 - Missing_Store;
                        summary.Storage_Practice = "NA";
                        summary.Store_Total = Math.Round(Store_Score, 2);
                        if (z > 0)
                        {
                            double x = (float)((float)(Store_Score) / z);
                            Spars_Score = x * 5;
                        }

                        summary.Store_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                }

            }
            catch
            {
                throw;
            }
        }
        private void Load_FEFOUse_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Expiry_Fefo_Use.ToList();

                List<Chss_M_Expiry_Fefo_Use> Data = new List<Chss_M_Expiry_Fefo_Use>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                if (Data.Any())
                {
                    int itemCount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Result == "NA")
                        {
                            if (itemCount == 0)
                            {
                                itemCount = 0;
                            }
                            else
                            {
                                itemCount = itemCount - 1;
                            }
                        }
                        else if (n.Result == "1")
                        {
                            count += 1;
                        }
                    }
                    if (itemCount == 0)
                    {
                        Missing_Expiry += 1;
                        Finalscore = 0.0;
                        Expiry_Score += Finalscore;
                        summary.FEFO_Use = Math.Round(Finalscore, 2).ToString();
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemCount);
                        Expiry_Score += Finalscore;
                        summary.FEFO_Use = Math.Round(Finalscore, 2).ToString();
                    }
                }
                else
                {
                    Missing_Expiry += 1;
                    Finalscore = 0.0;
                    Expiry_Score += Finalscore;
                    summary.FEFO_Use = "NA";
                }

            }
            catch
            {
                throw;
            }
        }
        private void Load_ExpiredCommodityPresence_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Expiry_Presence.ToList();

                List<Chss_M_Expiry_Presence> Data = new List<Chss_M_Expiry_Presence>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                
                if (Data.Any())
                {
                    int itemCount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Result == "NA")
                        {
                            if (itemCount == 0)
                            {
                                itemCount = 0;
                            }
                            else
                            {
                                itemCount = itemCount - 1;
                            }

                        }
                        else if (n.Result == "1")
                        {
                            if (n.Guid_Id == 55)
                            {
                                count += 0;
                            }
                            else
                            {
                                count += 1;
                            }
                        }
                        else if (n.Result == "0")
                        {
                            if (n.Guid_Id == 55)
                            {
                                count += 1;
                            }
                            else
                            {
                                count += 0;
                            }
                        }
                    }
                    if (itemCount == 0)
                    {
                        Missing_Expiry += 1;
                        Finalscore = 0.0;
                        Expiry_Score += Finalscore;
                        summary.Expiry_Presence = Math.Round(Finalscore, 2).ToString();
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemCount);
                        Expiry_Score += Finalscore;
                        summary.Expiry_Presence = Math.Round(Finalscore, 2).ToString();
                    }
                }
                else
                {
                    Missing_Expiry += 1;
                    Finalscore = 0.0;
                    Expiry_Score += Finalscore;
                    summary.Expiry_Presence = "NA";
                }

            }
            catch
            {
                throw;
            }
        }
        private void Load_TrackingTools_ScoreAsync(int facilityid, DateTime visitdate)
        {
            try
            {
                mascisEntities db = new mascisEntities();
                var a = db.Chss_M_Expiry_Tracking_Tools.ToList();

                List<Chss_M_Expiry_Tracking_Tools> Data = new List<Chss_M_Expiry_Tracking_Tools>();
                Data = a.Where(e => e.facility_code == facilityid & e.date_of_visit.Date == visitdate.Date).ToList();

                double Finalscore = 0.0;
                if (Data.Any())
                {
                    int itemCount = Data.Count;
                    int count = 0;
                    foreach (var n in Data)
                    {
                        if (n.Result == "NA")
                        {
                            if (itemCount == 0)
                            {
                                itemCount = 0;
                            }
                            else
                            {
                                itemCount = itemCount - 1;
                            }
                        }
                        else if (n.Result == "1")
                        {
                            count += 1;
                        }
                    }
                    if (itemCount == 0)
                    {
                        double Spars_Score = 0.0;
                        Missing_Expiry += 1;
                        Finalscore = 0.0;
                        Expiry_Score += Finalscore;
                        summary.Tacking_tools = Math.Round(Finalscore, 2).ToString();
                        summary.Expiry_Total = Math.Round(Expiry_Score, 2);
                        int z = 3 - Missing_Expiry;
                        if (z > 0)
                        {
                            double x = (float)((float)(Expiry_Score) / z);
                            Spars_Score = x * 5;
                        }
                        summary.Expiry_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {
                        Finalscore = (float)(count) / (float)(itemCount);
                        Expiry_Score += Finalscore;
                        summary.Tacking_tools = Math.Round(Finalscore, 2).ToString();
                        summary.Expiry_Total = Math.Round(Expiry_Score, 2);
                        int z = 3 - Missing_Expiry;
                        double x = (float)((float)(Expiry_Score) / z);
                        double Spars_Score = x * 5;
                        summary.Expiry_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                }
                else
                {
                    Missing_Expiry += 1;
                    Finalscore = 0.0;
                    Expiry_Score += Finalscore;
                    if (Expiry_Score == 0)
                    {
                        summary.Tacking_tools = "NA";
                        summary.Expiry_Total = Math.Round(Expiry_Score, 2);
                        double Spars_Score = 0.0;
                        summary.Expiry_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                    else
                    {
                        double Spars_Score = 0.0;
                        summary.Tacking_tools = "NA";
                        summary.Expiry_Total = Math.Round(Expiry_Score, 2);
                        int z = 3 - Missing_Expiry;
                        if (z > 0)
                        {
                            double x = (float)((float)(Expiry_Score) / z);
                            Spars_Score = x * 5;
                        }
                        summary.Expiry_Spars_Total = Math.Round(Spars_Score, 2);
                    }
                }

            }
            catch
            {
                throw;
            }
        }
    }
}