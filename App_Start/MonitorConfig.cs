using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
//using Ionic.Zip;

using System.Web.Mvc;
using Syncfusion.XlsIO;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Web.Script.Serialization;

using System.Security.Cryptography;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.JavaScript.Shared.Serializer;
using Syncfusion.JavaScript.Models;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO.Implementation.Collections;

using mascis.Models;
using System.Globalization;
using System.Data.Entity;

namespace mascis.App_Start
{
    public class MonitorConfig
    {

        public static string filename = "";
        public static void RegisterWatchers()
        {
            //var fsw = new FileSystemWatcher
            //{
            //    // Filter = "*.zip",
            //    Filter = "*.xl*",
            //    Path = ConfigurationManager.AppSettings["watchDirectoryPath"],
            //    EnableRaisingEvents = true,
            //    IncludeSubdirectories = false
            //};
           // fsw.Created += new FileSystemEventHandler(OnCreated);
           
          //var test = FileIsLocked(filename);
            //We need to rename the file after its importation
           // renametheFile(filename);
        }

        static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string excelToImport = e.FullPath;
            string unpackDirectory = ConfigurationManager.AppSettings["unpackDirectory"];
            string importFilesDirectory = ConfigurationManager.AppSettings["importFilesDirectory"];
            var test = FileIsLocked(excelToImport);
            ImportSpreadsheetData(excelToImport);
            filename = excelToImport;
            renametheFile(excelToImport);
        }
         

        //private static List<SMCData> ConvertData(string file)
        private static void ImportSpreadsheetData(string file)
        {
            //try
            {
                string filePath = file;//= Server.MapPath("~/Files/" + fileName);
                                       // filePath = @"C:\Software Development\InfoTronics\MASCIS\mascis_tools_14-05-2018\" + file;

                ExcelEngine excelEngine = new ExcelEngine();
                IWorkbook workbook =  excelEngine.Excel.Workbooks.Open(file);
                IWorksheet worksheet = workbook.Worksheets[0];


                //Import SMC
                #region "Import SMC"

                if (worksheet.Range["F6"].Value.TrimEnd().Length >= 15 && worksheet.Range["G6"].Value.ToUpper().TrimEnd().Length >= 11 &&
               worksheet.Range["H6"].Value.ToUpper().TrimEnd().Length >= 7)
                {
                    if (
                   (worksheet.Range["A6"].Value.ToLower() == "#") &&
                   (worksheet.Range["B6"].Value.ToUpper() == "PRODUCT CODE") &&
                   (worksheet.Range["C6"].Value.ToUpper() == "DESCRIPTION") &&
                   (worksheet.Range["D6"].Value.ToUpper().TrimEnd() == "UOM") &&
                   (worksheet.Range["E6"].Value.ToUpper().TrimEnd().Substring(0, 15) == "OPENING BALANCE") &&
                   (worksheet.Range["F6"].Value.ToUpper().Substring(0, 12) == "QTY RECEIVED") &&
                   (worksheet.Range["G6"].Value.ToUpper().Substring(0, 11) == "CONSUMPTION") &&
                   (worksheet.Range["H6"].Value.ToUpper().Substring(0, 7) == "LOSSES/") &&
                   (worksheet.Range["I6"].Value.ToUpper() == "TOTAL CLOSING BALANCE") &&
                   (worksheet.Range["J6"].Value.ToUpper() == "QUANTITY TO ORDER") &&
                   (worksheet.Range["K6"].Value.ToUpper() == "COMMENTS"))
                    {
                        mascisEntities context = new mascisEntities();
                        //context.Configuration.ProxyCreationEnabled = false;
                        Order_SMC_SLM_Header smcHeader = new Order_SMC_SLM_Header();
                        Order_SMC_SLM smc = new Order_SMC_SLM();
                        //Read the FACILITY/IP NAME: 
                        string facilityName = worksheet.Range["C2"].FormulaStringValue.ToString();
                        // var facility = context.A_Facilities.FirstOrDefault(f => f.SAP_Code == facilityName);
                        var facility = context.A_Facilities.FirstOrDefault(f => f.SAP_Code == facilityName);
                        int facilitycode = 0;
                        if (facility != null)
                        {
                            smcHeader.FacilityCode = facility.FacilityCode;
                            facilitycode = facility.FacilityCode;
                        }

                        string districtName = worksheet.Range["C4"].Value.ToString().TrimEnd();
                        string startDate = worksheet.Range["I3"].Value.ToString();
                        string endDate = worksheet.Range["I4"].Value.ToString();
                        string datePrepared = worksheet.Range["I5"].DateTime.ToString();
                        var district = context.A_District.FirstOrDefault(f => f.District_Name == districtName);
                        string NoCircumcised = worksheet.Range["E95"].Value.ToString();
                        string NoTargetedCircumcision = worksheet.Range["E96"].Value.ToString();
                        if (int.TryParse(NoCircumcised, out int parsenumCircumcised))
                        {
                            smcHeader.No_Circumcised = parsenumCircumcised;
                        }
                        if (int.TryParse(NoTargetedCircumcision, out int parsenumTargetedCircumcision))
                        {
                            smcHeader.No_Targeted = parsenumTargetedCircumcision;
                        }


                        DateTime dateTime = new DateTime();
                        if (DateTime.TryParse(startDate, out dateTime))
                        {
                            smcHeader.StartDate = dateTime;
                        }

                        if (DateTime.TryParse(endDate, out dateTime))
                        {
                            smcHeader.EndDate = dateTime;
                        }

                        if (DateTime.TryParse(datePrepared, out dateTime))
                        {
                            smcHeader.DatePrepared = dateTime;
                        }

                        string preparedbyName = worksheet.Range["C100"].Value.ToString();
                        //smc.RFSO_ApprovedBy = preparedbyName;

                        string preparedbyDesignation = worksheet.Range["C102"].Value.ToString();
                        string preparedbyPhoneNo = worksheet.Range["C103"].Value.ToString();
                        string approvedbyName = worksheet.Range["F100"].Value.ToString();
                        string approvedbyDesignation = worksheet.Range["F102"].Value.ToString();

                        string approvedbyPhoneNo = worksheet.Range["F103"].Value.ToString();
                        //Save if valid facility, start and end dates
                        if (smcHeader.FacilityCode > 0 &&
                            (DateTime.TryParse(startDate, out dateTime)) &&
                            (DateTime.TryParse(endDate, out dateTime))
                            )
                        {
                            //var objsmcHeader = context.Order_SMC_SLM_Header.FirstOrDefault(o => o.FacilityCode == smcHeader.FacilityCode &&
                            //o.StartDate == smcHeader.StartDate && o.EndDate == smcHeader.EndDate);
                            string _OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(smcHeader.DatePrepared), facilitycode, "10", "02");
                            var objsmcHeader = context.Order_SMC_SLM_Header.FirstOrDefault(o => o.OrderNumber == _OrderNumber);
                            smcHeader.OrderNumber = _OrderNumber;
                            smcHeader.OrderTypeId = 2;
                            smcHeader.FinalSubmission = true;
                            smcHeader.OrderStatusId = 6;
                            smcHeader.EditedDate = DateTime.Now;

                            if (objsmcHeader != null)
                            {
                                context.Entry(objsmcHeader).CurrentValues.SetValues(smcHeader);
                                context.Entry(objsmcHeader).State = EntityState.Modified;
                            }
                            else
                            {
                                context.Order_SMC_SLM_Header.Add(smcHeader);
                            }
                            context.SaveChanges();
                            //(int row = 14; row <= 159; row++)
                            for (int row = 9; row <= 92; row++)
                            {
                                if (worksheet.IsRowVisible(row))
                                {
                                    if (row == 13 || row == 29 || row == 34 || row == 61 || row == 84)
                                    {
                                        continue;
                                    }
                                    smc = new Order_SMC_SLM();
                                    smc.OrderNumber = _OrderNumber;
                                    smc.FacilityCode = facilitycode;
                                    smc.StartDate = smcHeader.StartDate;
                                    smc.EndDate = smcHeader.EndDate;

                                    for (int column = 1; column <= worksheet.UsedRange.LastColumn; column++)
                                    {
                                        switch (column)
                                        {
                                            case 1: // Serial No
                                                    //if (worksheet.Range[1, column].Value.ToLower() == "#")
                                                if (/*worksheet.Range["A6"].Value.ToLower() == "#" &&*/ row >= 9 && row <= 92)
                                                {
                                                    string serial = worksheet.Range[row, column].Value.ToString();

                                                    if (int.TryParse(serial, out int serialnum))
                                                    {
                                                        //smc.
                                                    }
                                                }

                                                break;
                                            case 2: // ProductCode
                                                if (/*worksheet.Range["B6"].Value.ToUpper() == "PRODUCT CODE" && */row >= 9 && row <= 92)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (int.TryParse(coltext, out int parsenum))
                                                    {
                                                        smc.ProductCode = parsenum;
                                                    }
                                                }

                                                break;
                                            case 3:
                                                //if (worksheet.Range["C6"].Value.ToUpper() == "DESCRIPTION" && row >= 9 && row <= 92)
                                                //{
                                                //    smcobj.Desc = worksheet.Range[row, column].Value.ToString();
                                                //}

                                                break;
                                            case 4:
                                                if (/*worksheet.Range["D6"].Value.ToUpper().TrimEnd() == "UOM" &&*/ row >= 9 && row <= 92)
                                                {
                                                    //  smc.UoM = worksheet.Range[row, column].Value.ToString();
                                                }
                                                break;
                                            case 5:
                                                if (/*worksheet.Range["E6"].Value.ToUpper().TrimEnd() == "OPENING BALANCE                                           At start of period" && */
                                                    row >= 9 && row <= 92)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        smc.OpeningBalance = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(smc.OpeningBalance)))
                                                    {
                                                        smc.OpeningBalance = null;
                                                    }
                                                }

                                                break;

                                            case 6:
                                                if (/*worksheet.Range["F6"].Value.ToUpper() == "QTY RECEIVED" &&*/ row >= 9 && row <= 92)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        smc.QtyRecieved = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(smc.QtyRecieved)))
                                                    {
                                                        smc.QtyRecieved = null;
                                                    }
                                                }

                                                break;

                                            case 7:
                                                if (/*worksheet.Range["G6"].Value.ToUpper() == "CONSUMPTION" &&*/ row >= 9 && row <= 92)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        smc.Consumption = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(smc.Consumption)))
                                                    {
                                                        smc.Consumption = null;
                                                    }
                                                }

                                                break;
                                            case 8:
                                                if (/*worksheet.Range["H6"].Value.ToUpper() == "LOSSES/" && */row >= 9 && row <= 92)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        smc.LossesAndAdjustments = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(smc.LossesAndAdjustments)))
                                                    {
                                                        smc.LossesAndAdjustments = null;
                                                    }
                                                }

                                                break;
                                            case 9:
                                                if (/*worksheet.Range["I6"].Value.ToUpper() == "TOTAL CLOSING BALANCE" &&*/ row >= 9 && row <= 92)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        smc.TotalClosingBalance = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(smc.TotalClosingBalance)))
                                                    {
                                                        smc.TotalClosingBalance = null;
                                                    }
                                                }

                                                break;
                                            case 10:
                                                if (/*worksheet.Range["J6"].Value.ToUpper() == "QUANTITY TO ORDER" && */row >= 9 && row <= 92)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        smc.QuantityToOrder = Math.Round(parsenum, 0);
                                                        if (parsenum < 0 || double.IsNaN(parsenum))
                                                        {
                                                            smc.QuantityAllocated = 0;
                                                        }
                                                        else
                                                        {
                                                            smc.QuantityAllocated = Convert.ToInt32(parsenum);
                                                        }
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(smc.QuantityToOrder)))
                                                    {
                                                        smc.QuantityToOrder = null;
                                                        smc.QuantityAllocated = 0;
                                                    }
                                                }

                                                break;

                                            case 11:
                                                if (/*worksheet.Range["K6"].Value.ToUpper() == "COMMENTS" && */row >= 9 && row <= 92)
                                                {
                                                    smc.Comments = worksheet.Range[row, column].Value.ToString();
                                                }
                                                break;
                                        }
                                    }
                                    //var objsmc = context.Order_SMC_SLM.FirstOrDefault(o => o.FacilityCode == smcHeader.FacilityCode && o.StartDate == smc.StartDate &&
                                    //o.EndDate == smc.EndDate && o.ProductCode == smc.ProductCode);
                                    var objsmc = context.Order_SMC_SLM.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.ProductCode == smc.ProductCode);
                                    if (objsmc != null)
                                    {
                                        //  context.Entry(objsmc).CurrentValues.SetValues(smc);
                                        objsmc.OpeningBalance = smc.OpeningBalance;
                                        objsmc.QtyRecieved = smc.QtyRecieved;
                                        objsmc.Consumption = smc.Consumption;
                                        objsmc.LossesAndAdjustments = smc.LossesAndAdjustments;
                                        objsmc.TotalClosingBalance = smc.TotalClosingBalance;
                                        objsmc.QuantityToOrder = smc.QuantityToOrder;
                                        objsmc.QuantityAllocated = smc.QuantityAllocated;
                                        objsmc.Comments = smc.Comments;
                                        objsmc.RFSONotes = smc.RFSONotes;
                                        context.Entry(objsmc).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        //objsmc.OrderNumber = _OrderNumber;
                                        context.Order_SMC_SLM.Add(smc);
                                    }
                                    context.SaveChanges();

                                }

                            }
                        }

                    }
                }


                #endregion "Import SMC" 

                //Import   "Essential Laboratory & Specialised Lab Supplies Order Form"
                #region "Import Essential Laboratory & Specialised Lab Supplies Order Form"
                if (// worksheet.Range["A2"].Value.TrimEnd().Length >= 47 && 
                    worksheet.Range["F10"].Value.ToUpper().TrimEnd().Length >= 15 &&
                    worksheet.Range["G10"].Value.ToUpper().TrimEnd().Length >= 12 &&
                    worksheet.Range["H10"].Value.ToUpper().TrimEnd().Length >= 11 &&
                    worksheet.Range["I10"].Value.ToUpper().TrimEnd().Length >= 6 &&
                    worksheet.Range["K10"].Value.ToUpper().TrimEnd().Length >= 17 &&
                    worksheet.Range["L10"].Value.ToUpper().TrimEnd().Length >= 10)
                {
                    if (
                    //(worksheet.Range["A2"].Value.TrimEnd().Substring(1, 47) == "Essential Laboratory & Specialised Lab Supplies") &&
                    (worksheet.Range["A10"].Value.ToUpper().TrimEnd() == "CODE") &&
                    (worksheet.Range["C10"].Value.ToUpper().TrimEnd() == "DESCRIPTION") &&
                    (worksheet.Range["D10"].Value.ToUpper().TrimEnd() == "UOM") &&
                    (worksheet.Range["E10"].Value.ToUpper().TrimEnd() == "UNIT PRICE") &&
                    (worksheet.Range["F10"].Value.Contains("OPENING BALANCE")) &&
                    (worksheet.Range["G10"].Value.ToUpper().Contains("QTY RECEIVED")) &&
                    (worksheet.Range["H10"].Value.ToUpper().Contains("CONSUMPTION")) &&
                    (worksheet.Range["I10"].Value.ToUpper().Contains("LOSSES")) &&
                    (worksheet.Range["J10"].Value.ToUpper().Contains("TOTAL CLOSING BALANCE")) &&
                    (worksheet.Range["K10"].Value.ToUpper().Contains("QUANTITY TO ORDER")) &&
                    (worksheet.Range["L10"].Value.ToUpper().Contains("TOTAL COST")) &&
                    (worksheet.Range["M10"].Value.ToUpper().Contains("COMMENTS"))
                    )
                    {
                        mascisEntities context = new mascisEntities();
                        //context.Configuration.ProxyCreationEnabled = false;
                        Order_Lab_Header Header = new Order_Lab_Header();
                        Order_Lab obj = new Order_Lab();

                        string facilityName = worksheet.Range["C5"].FormulaStringValue.ToString();
                        string HSD = worksheet.Range["C6"].Value.ToString();

                        string startDate = worksheet.Range["J6"].FormulaDateTime.ToString();
                        string endDate = worksheet.Range["J7"].FormulaDateTime.ToString();
                        string datePrepared = worksheet.Range["J8"].DateTime.ToString();

                        string orderTotal = worksheet.Range["A8"].Value.ToString();
                        string districtName = worksheet.Range["F5"].Value.ToString();
                        string completedBy = worksheet.Range["F6"].Value.ToString();
                        string dateCompleted = worksheet.Range["F7"].Value.ToString();

                        string orderedbyName = worksheet.Range["C617"].Value.ToString();
                        string orderedbyDesign = worksheet.Range["I617"].Value.ToString();
                        string orderedbyDate = worksheet.Range["M617"].Value.ToString();

                        string approvedbyName = worksheet.Range["C620"].Value.ToString();
                        string approvedbyDesign = worksheet.Range["I620"].Value.ToString();
                        string approvedbyDate = worksheet.Range["M620"].Value.ToString();
                        string confirmedByName = worksheet.Range["C623"].Value.ToString();
                        string confirmedByDesign = worksheet.Range["I623"].Value.ToString();
                        string confirmedByDate = worksheet.Range["M623"].Value.ToString();


                        int facilitycode = 0;
                        var facility = context.A_Facilities.FirstOrDefault(f => f.SAP_Code == facilityName);

                        if (facility != null)
                        {
                            Header.FacilityCode = facility.FacilityCode;
                            facilitycode = facility.FacilityCode;
                        }
                        var district = context.A_District.FirstOrDefault(f => f.District_Name == districtName);

                        if (district != null)
                        {
                            //smcHeader.d = district.District_Code;
                        }

                        string format = "MM/dd/yyyy";
                        DateTime dateTime = new DateTime();//
                        CultureInfo current = new CultureInfo("en-US");
                        // string strtest = startDate.Substring(0, 10);
                        string strtest = startDate.Substring(0, 9);
                        if (DateTime.TryParse(startDate, out dateTime))
                        {
                            Header.start_date = dateTime;
                        }
                        if (DateTime.TryParseExact(strtest, format, current, DateTimeStyles.None, out dateTime))
                        {
                            Header.start_date = dateTime;
                        }
                        if (DateTime.TryParse(endDate, out dateTime))
                        {
                            Header.end_date = dateTime;
                        }
                        if (DateTime.TryParseExact(strtest, format, current, DateTimeStyles.None, out dateTime))
                        {
                            Header.end_date = dateTime;
                        }
                        if (DateTime.TryParse(datePrepared, out dateTime))
                        {
                            Header.date_completed = dateTime;
                        }

                        if (Header.FacilityCode > 0 &&
                           (DateTime.TryParse(startDate, out dateTime)) &&
                           (DateTime.TryParse(endDate, out dateTime)))
                        {
                            //var objHeader = context.Order_Lab_Header.FirstOrDefault(o => o.FacilityCode == Header.FacilityCode &&
                            //o.start_date == Header.start_date && o.end_date == Header.end_date);
                            string _OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(Header.date_completed), facilitycode, "01", "02");
                            var objHeader = context.Order_Lab_Header.FirstOrDefault(o => o.OrderNumber == _OrderNumber);
                            Header.OrderNumber = _OrderNumber;
                            Header.OrderTypeId = 2;
                            Header.FinalSubmission = true;
                            Header.OrderStatusId = 6;
                            Header.EditedDate = DateTime.Now;

                            if (objHeader == null)
                            {
                                context.Order_Lab_Header.Add(Header);
                            }
                            context.SaveChanges();
                            //for (int row = 14; row <= worksheet.UsedRange.LastRow; row++)
                            for (int row = 14; row <= 159; row++)
                            {
                                if (worksheet.IsRowVisible(row))
                                {
                                    if (row == 23 || row == 31 || row == 41 || row == 45 || row == 126 || row == 146 || row == 153 || row == 154 || row == 155 || row == 159 || (row >= 48 && row <= 103) || (row >= 136 && row <= 138) || (row >= 105 && row <= 111))
                                    {
                                        continue;
                                    }
                                    obj = new Order_Lab();
                                    obj.OrderNumber = _OrderNumber;
                                    obj.FacilityCode = facilitycode;
                                    obj.start_date = Header.start_date;
                                    obj.end_date = Header.end_date;
                                    for (int column = 1; column <= worksheet.UsedRange.LastColumn; column++)
                                    {
                                        switch (column)
                                        {
                                            case 1: // Serial No
                                                    //if (worksheet.Range[1, column].Value.ToLower() == "#")
                                                if (/*(worksheet.Range["A10"].Value.ToUpper().TrimEnd() == "CODE") &&*/ row >= 14 && row <= 159)
                                                {
                                                    string serial = worksheet.Range[row, column].Value.ToString();
                                                    int serialnum;
                                                    if (int.TryParse(serial, out serialnum))
                                                    {
                                                        obj.item_code = serialnum;
                                                    }
                                                }

                                                break;
                                            case 3: // ProductCode
                                                    //if ((worksheet.Range["C10"].Value.ToUpper().TrimEnd() == "DESCRIPTION") && row >= 14 && row <= 159)
                                                    //{
                                                    //    string coltext = worksheet.Range[row, column].Value.ToString();
                                                    //    int parsenum;
                                                    //    if (int.TryParse(coltext, out parsenum))
                                                    //    {
                                                    //        smcobj.ProductCode = parsenum;
                                                    //    }
                                                    //}

                                                break;
                                            case 4:
                                                //if ((worksheet.Range["D10"].Value.ToUpper().TrimEnd() == "UOM") && row >= 14 && row <= 159)
                                                //{
                                                //    smcobj.Desc = worksheet.Range[row, column].Value.ToString();
                                                //}

                                                break;
                                            case 5: // UOM
                                                    //if ((worksheet.Range["E10"].Value.ToUpper().TrimEnd() == "UNIT PRICE") && row >= 14 && row <= 159)
                                                    //{
                                                    //    smcobj.UoM = worksheet.Range[row, column].Value.ToString();
                                                    //}
                                                break;
                                            case 6: // Opening Balance
                                                if (/*(worksheet.Range["F10"].Value.ToUpper().TrimEnd().Substring(1, 15) == "OPENING BALANCE") &&*/ row >= 14 && row <= 159)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        obj.opening_balance = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(obj.opening_balance)))
                                                    {
                                                        obj.opening_balance = null;
                                                    }
                                                }

                                                break;

                                            case 7:
                                                if (/*(worksheet.Range["G10"].Value.ToUpper().TrimEnd().Substring(1, 12) == "QTY RECEIVED") &&*/ row >= 14 && row <= 159)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        obj.quantity_recieved = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(obj.quantity_recieved)))
                                                    {
                                                        obj.quantity_recieved = null;
                                                    }
                                                }

                                                break;

                                            case 8:
                                                if (/*(worksheet.Range["H10"].Value.ToUpper().TrimEnd().Substring(1, 11) == "CONSUMPTION") && */row >= 14 && row <= 159)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        obj.consumption = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(obj.consumption)))
                                                    {
                                                        obj.consumption = null;
                                                    }
                                                }

                                                break;
                                            case 9:
                                                if (/*(worksheet.Range["I10"].Value.ToUpper().TrimEnd().Substring(1, 6) == "LOSSES") &&*/ row >= 14 && row <= 159)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        obj.losses_adjustment = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(obj.losses_adjustment)))
                                                    {
                                                        obj.losses_adjustment = null;
                                                    }
                                                }

                                                break;
                                            case 10:
                                                if (/*(worksheet.Range["J10"].Value.ToUpper().TrimEnd() == "TOTAL CLOSING BALANCE") && */row >= 14 && row <= 159)
                                                {
                                                    string coltext = worksheet.Range[row, column].Value.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        obj.closing_balance = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(obj.closing_balance)))
                                                    {
                                                        obj.closing_balance = null;
                                                    }
                                                }

                                                break;
                                            case 11:
                                                if (/*(worksheet.Range["K10"].Value.ToUpper().TrimEnd().Substring(1, 17) == "QUANTITY TO ORDER") &&*/ row >= 14 && row <= 159)
                                                {
                                                    string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        obj.quantity_to_order = Math.Round(parsenum, 0);
                                                        if (parsenum < 0 || double.IsNaN(parsenum))
                                                        {
                                                            obj.quantity_allocated = 0;
                                                        }
                                                        else
                                                        {
                                                            obj.quantity_allocated = Convert.ToInt32(parsenum);
                                                        }
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(obj.quantity_to_order)))
                                                    {
                                                        obj.quantity_to_order = null;
                                                        obj.quantity_allocated = 0;
                                                    }
                                                }

                                                break;

                                            case 12:
                                                if (/*(worksheet.Range["L10"].Value.ToUpper().TrimEnd().Substring(1, 10) == "TOTAL COST") &&*/ row >= 14 && row <= 159)
                                                {
                                                    string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();

                                                    if (double.TryParse(coltext, out double parsenum))
                                                    {
                                                        obj.total_cost = parsenum;
                                                    }
                                                    if (Double.IsNaN(System.Convert.ToDouble(obj.total_cost)))
                                                    {
                                                        obj.total_cost = null;
                                                    }
                                                }
                                                break;
                                            case 13:
                                                if (/*(worksheet.Range["M10"].Value.ToUpper().TrimEnd() == "COMMENTS") &&*/ row >= 14 && row <= 159)
                                                {
                                                    obj.comments = worksheet.Range[row, column].Value.ToString();
                                                }
                                                break;
                                        }
                                    }
                                    //Find the product code from the A_Product_Lab_Category table related
                                    //by lab_cat_2_code from A_Lab_Category2 table

                                    var Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == 0);


                                    if (row >= 14 && row <= 22)// PARTEC CYFLOW
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 1);
                                    }

                                    if (row >= 24 && row <= 30)//  FACSCOUNT
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 2);
                                    }

                                    if (row >= 32 && row <= 40)//  FACSCALIBUR
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 3);
                                    }

                                    if (row >= 42 && row <= 44)//  PIMA
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 4);
                                    }

                                    if (row >= 46 && row <= 47)//  BD PRESTO --- Not in A_Product_Lab_Category
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 5);
                                    }

                                    if (row >= 104 && row <= 104)//  where lab_cat_1_desc like '%DIAGNOSTIC KITS & REAGENTS%' and lab_cat_2_desc like '%CRYPTOCOCCAL%'
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 20);
                                    }

                                    if (row >= 112 && row <= 125)//  SAMPLE COLLECTION SUPPLIES/ACCESSORIES
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 22);
                                    }

                                    if (row >= 127 && row <= 135)//  INFECTION CONTROL AND WASTE MANAGEMENT
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 23);
                                    }

                                    if (row >= 139 && row <= 145)//  GENERAL LABORATORY SUPPLIES
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 25);
                                    }

                                    if (row >= 147 && row <= 159)//  GENERAL LABORATORY SUPPLIES
                                    {
                                        Lab_Category = context.A_Product_Lab_Category.FirstOrDefault(f => f.product_code == obj.item_code
                                        && f.category_2_code == 26);
                                    }
                                    //var objlab = context.Order_Lab.FirstOrDefault(o => o.FacilityCode == obj.FacilityCode &&
                                    //    o.start_date == obj.start_date  && o.end_date  == obj.end_date  && o.item_code == obj.item_code);
                                    //var objlab = context.Order_Lab.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.item_code == obj.item_code);
                                    //if (string.IsNullOrEmpty(Lab_Category.product_category_code.ToString()) == true)

                                    if (Lab_Category != null)
                                    {
                                        var objlab = context.Order_Lab.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.item_code == (int)Lab_Category.product_category_code);
                                        if (Lab_Category != null)
                                        {
                                            if (objlab != null)
                                            {
                                                objlab.opening_balance = obj.opening_balance;
                                                objlab.quantity_recieved = obj.quantity_recieved;
                                                objlab.consumption = obj.consumption;
                                                objlab.losses_adjustment = obj.losses_adjustment;
                                                objlab.closing_balance = obj.closing_balance;
                                                objlab.quantity_to_order = obj.quantity_to_order;
                                                objlab.quantity_allocated = obj.quantity_allocated;
                                                objlab.total_cost = obj.total_cost;
                                                objlab.comments = obj.comments;
                                                objlab.RFSONotes = obj.RFSONotes;
                                                context.Entry(objlab).State = EntityState.Modified;
                                                context.SaveChanges();
                                            }
                                            else
                                            {
                                                obj.OrderNumber = _OrderNumber;
                                                obj.item_code = (int)Lab_Category.product_category_code;
                                                context.Order_Lab.Add(obj);
                                                context.SaveChanges();
                                            }
                                        }
                                    }


                                }

                            }
                        }

                    }

                }


                #endregion "Import Essential Laboratory & Specialised Lab Supplies Order Form" 

                //Import   "VIRAL LOAD REAGENTS & CONSUMABLES REPORT AND ORDER FORM "
                #region "Import VIRAL LOAD REAGENTS & CONSUMABLES REPORT AND ORDER FORM "
                //              VIRAL LOAD AND EARLY INFANT DIAGNOSIS REAGENTS & CONSUMABLES REPORT AND ORDER FORM 

                //  if (worksheet.Range["F8"].Value.TrimEnd().Length >= 35 && worksheet.Range["G8"].Value.ToUpper().TrimEnd().Length >= 11 &&
                //worksheet.Range["H8"].Value.ToUpper().TrimEnd().Length >= 6)
                //{
                if (/*(worksheet.Range["A2"].Value.ToUpper().TrimEnd() == "VIRAL LOAD REAGENTS & CONSUMABLES REPORT AND ORDER FORM") &&*/
                (worksheet.Range["B8"].Value.Contains("Product code")) &&
                (worksheet.Range["C8"].Value.Contains("Product Description")) &&
                (worksheet.Range["D8"].Value.Contains("Pack Size")) &&
                (worksheet.Range["E8"].Value.Contains("Opening Balance")) &&
                (worksheet.Range["F8"].Value.Contains("Quantity Received")) &&
                (worksheet.Range["G8"].Value.Contains("Consumption")) &&
                (worksheet.Range["H8"].Value.Contains("Losses/Adjustments")) &&
                (worksheet.Range["I8"].Value.Contains("Total Closing Balance")) &&
                (worksheet.Range["J8"].Value.Contains("Quantity Required")) &&
                (worksheet.Range["K8"].Value.Contains("Comments")))
                {
                    mascisEntities context = new mascisEntities();
                    //context.Configuration.ProxyCreationEnabled = false;
                    Order_ViralLoadReagents_Header Header = new Order_ViralLoadReagents_Header();
                    Order_ViralLoadReagents_Detail obj = new Order_ViralLoadReagents_Detail();
                    string facilityName = worksheet.Range["C3"].Value.ToString();
                    string districtName = worksheet.Range["C6"].Value.ToString();

                    string startDate = worksheet.Range["I4"].Value.ToString();
                    string endDate = worksheet.Range["I5"].Value.ToString();
                    string datePrepared = worksheet.Range["I6"].DateTime.ToString();

                    string preparedByName = worksheet.Range["C81"].Value.ToString();
                    string reviewedByName = worksheet.Range["C84"].Value.ToString();
                    string preparedByDesignation = worksheet.Range["F81"].Value.ToString();
                    string reviewedByDesignation = worksheet.Range["F84"].Value.ToString();
                    string preparedByPhoneNo = worksheet.Range["J81"].Value.ToString();
                    string reviewedByPhoneNo = worksheet.Range["J84"].Value.ToString();

                    string viralLoadTest = worksheet.Range["D76"].Value.ToString();
                    string earlyInfantDiagnosisTests = worksheet.Range["D77"].Value.ToString();
                    string COBAS8800 = worksheet.Range["D78"].Value.ToString();
                    string ROCHECAPCTM = worksheet.Range["D79"].Value.ToString();

                    int facilitycode = 0;
                    var facility = context.A_Facilities.FirstOrDefault(f => f.SAP_Code == facilityName);

                    if (facility != null)
                    {
                        Header.FacilityCode = facility.FacilityCode;
                        facilitycode = facility.FacilityCode;
                    }
                    var district = context.A_District.FirstOrDefault(f => f.District_Name == districtName);

                    DateTime dateTime = new DateTime();//

                    if (DateTime.TryParse(startDate, out dateTime))
                    {
                        Header.StartDate = dateTime;
                    }

                    if (DateTime.TryParse(endDate, out dateTime))
                    {
                        Header.EndDate = dateTime;
                    }

                    if (DateTime.TryParse(datePrepared, out dateTime))
                    {
                        Header.DatePrepared = dateTime;
                    }

                    if (Header.FacilityCode > 0 && (DateTime.TryParse(startDate, out dateTime))
                        && (DateTime.TryParse(endDate, out dateTime)))
                    {
                        //var objHeader = context.Order_ViralLoadReagents_Header.FirstOrDefault(o => o.FacilityCode == Header.FacilityCode &&
                        //o.StartDate == Header.StartDate && o.EndDate == Header.EndDate);
                        string _OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(Header.DatePrepared), facilitycode, "11", "02");
                        var objHeader = context.Order_ViralLoadReagents_Header.FirstOrDefault(o => o.OrderNumber == _OrderNumber);
                        Header.OrderNumber = _OrderNumber;
                        Header.OrderTypeId = 2;
                        Header.FinalSubmission = true;
                        Header.OrderStatusId = 6;
                        Header.EditedDate = DateTime.Now;

                        if (objHeader != null)
                        {
                            // context.Entry(objHeader).CurrentValues.SetValues(Header);
                            objHeader.DatePrepared = Header.DatePrepared;
                            objHeader.EditedBy = Header.EditedBy;
                            objHeader.EditedDate = Header.EditedDate;
                            //objHeader.RecordStatus = Header.RecordStatus;
                            objHeader.EmergencyOrder = Header.EmergencyOrder;
                            objHeader.FinalSubmission = true;
                            //objHeader.ExportedToSAP = Header.ExportedToSAP;

                            context.Entry(objHeader).State = EntityState.Modified;
                        }
                        else
                        {
                            //objHeader.OrderNumber = _OrderNumber;
                            //objHeader.FacilityCode = facility.FacilityCode;
                            //objHeader.StartDate = StartDate;
                            //objHeader.OrderNumber = _OrderNumber;

                            //if (DateTime.TryParse(startDate, out dateTime))
                            //{
                            //    Header.StartDate = dateTime;
                            //}

                            //if (DateTime.TryParse(endDate, out dateTime))
                            //{
                            //    Header.EndDate = dateTime;
                            //}
                            context.Order_ViralLoadReagents_Header.Add(Header);
                        }
                        context.SaveChanges();


                        for (int row = 10; row <= 72 /*row <= worksheet.UsedRange.LastRow*/; row++)
                        {
                            if (worksheet.IsRowVisible(row))
                            {
                                if (row == 25 || row == 34 || row == 36 || row == 49 || row == 62 || row == 70)
                                {
                                    continue;
                                }
                                obj = new Order_ViralLoadReagents_Detail();
                                obj.OrderNumber = _OrderNumber;
                                obj.FacilityCode = facilitycode;
                                obj.StartDate = Header.StartDate;
                                obj.EndDate = Header.EndDate;

                                for (int column = 1; column <= worksheet.UsedRange.LastColumn; column++)
                                {
                                    switch (column)
                                    {
                                        case 2:
                                            //if (worksheet.Range[1, column].Value.ToLower() == "#")
                                            if (row >= 10 && row <= 33)
                                            {
                                                string serial = worksheet.Range[row, column].Value.ToString();
                                                int serialnum;
                                                if (int.TryParse(serial, out serialnum))
                                                {
                                                    obj.ProductCode = serialnum;
                                                }
                                            }

                                            break;
                                        case 3:
                                            //if ((worksheet.Range["C8"].Value.TrimEnd() == "Product Description") && row >= 10 && row <= 33)
                                            //{
                                            //    string coltext = worksheet.Range[row, column].Value.ToString();
                                            //    int parsenum;
                                            //    if (int.TryParse(coltext, out parsenum))
                                            //    {
                                            //        smcobj.ProductCode = parsenum;
                                            //    }
                                            //}

                                            break;
                                        case 4:
                                            //if ((worksheet.Range["D8"].Value.TrimEnd() == "Pack Size") && row >= 10 && row <= 33)
                                            //{
                                            //    smcobj.Desc = worksheet.Range[row, column].Value.ToString();
                                            //}

                                            break;
                                        case 5:
                                            if (/*(worksheet.Range["E8"].Value.TrimEnd() == "Opening Balance") && */row >= 10 && row <= 33)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    obj.OpeningBalance = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(obj.OpeningBalance)))
                                                {
                                                    obj.OpeningBalance = null;
                                                }
                                            }
                                            break;
                                        case 6:
                                            if (/*(worksheet.Range["F8"].Value.TrimEnd().Substring(0, 12) == "QTY RECEIVED") &&*/ row >= 10 && row <= 33)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    obj.QuantityRecieved = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(obj.QuantityRecieved)))
                                                {
                                                    obj.QuantityRecieved = null;
                                                }
                                            }

                                            break;

                                        case 7:
                                            if (/*(worksheet.Range["G8"].Value.ToUpper().TrimEnd().Substring(0, 11) == "CONSUMPTION") &&*/ row >= 10 && row <= 33)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    obj.Consumption = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(obj.Consumption)))
                                                {
                                                    obj.Consumption = null;
                                                }
                                            }

                                            break;

                                        case 8:
                                            if (/*(worksheet.Range["H8"].Value.ToUpper().TrimEnd().Substring(0, 6) == "LOSSES") && */row >= 10 && row <= 33)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    obj.LossesAndAdjustments = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(obj.LossesAndAdjustments)))
                                                {
                                                    obj.LossesAndAdjustments = null;
                                                }
                                            }

                                            break;
                                        case 9:
                                            if (/*(worksheet.Range["I8"].Value.ToUpper().TrimEnd() == "TOTAL CLOSING BALANCE") && */row >= 10 && row <= 33)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    obj.TotalClosingBalance = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(obj.TotalClosingBalance)))
                                                {
                                                    obj.TotalClosingBalance = null;
                                                }
                                            }

                                            break;
                                        case 10:
                                            if (/*(worksheet.Range["J8"].Value.ToUpper().TrimEnd() == "CPHL QUANTITY ORDERED") &&*/ row >= 10 && row <= 33)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    obj.QuantityOrdered = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(obj.QuantityOrdered)))
                                                {
                                                    obj.QuantityOrdered = null;
                                                }
                                            }

                                            break;
                                        case 11:
                                            if (/*(worksheet.Range["K8"].Value.ToUpper().TrimEnd() == "COMMENTS") && */row >= 10 && row <= 33)
                                            {
                                                obj.Comment = worksheet.Range[row, column].Value.ToString();
                                            }
                                            break;
                                    }
                                }
                                var objviral = context.Order_ViralLoadReagents_Detail.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.ProductCode == obj.ProductCode);
                                if (objviral != null)
                                {
                                    //context.Entry(objviral).CurrentValues.SetValues(obj);
                                    objviral.OpeningBalance = obj.OpeningBalance;
                                    objviral.QuantityRecieved = obj.QuantityRecieved;
                                    objviral.Consumption = obj.Consumption;
                                    objviral.LossesAndAdjustments = obj.LossesAndAdjustments;
                                    objviral.TotalClosingBalance = obj.TotalClosingBalance;
                                    objviral.QuantityOrdered = obj.QuantityOrdered;
                                    objviral.Comment = obj.Comment;
                                    objviral.Quantity_Allocated = obj.Quantity_Allocated;
                                    objviral.RFSONotes = obj.RFSONotes;

                                    context.Entry(objviral).State = EntityState.Modified;
                                }
                                else
                                {
                                    //objviral.OrderNumber = _OrderNumber;
                                    context.Order_ViralLoadReagents_Detail.Add(obj);
                                }
                                context.SaveChanges();
                            }
                        }
                    }

                }
                //}


                #endregion "Import  VIRAL LOAD REAGENTS & CONSUMABLES REPORT AND ORDER FORM " 

                //Import                  "BIMONTHLY REPORT AND ORDER CALCULATION FORM FOR HIV TEST KITS "
                #region "BIMONTHLY REPORT AND ORDER CALCULATION FORM FOR HIV TEST KITS"

                if (
                (worksheet.Range["B15"].Value.Contains("Serial No.")) &&
                (worksheet.Range["D15"].Value.Contains("Item Description")) &&
                (worksheet.Range["E15"].Value.Contains("Basic Unit")) &&
                (worksheet.Range["F15"].Value.Contains("OPENING BALANCE")) &&
                (worksheet.Range["G15"].Value.Contains("QUANTITY RECEIVED")) &&
                (worksheet.Range["H15"].Value.Contains("HIV Test kits")) &&
                (worksheet.Range["I15"].Value.Contains("LOSSES")) &&
                (worksheet.Range["J15"].Value.TrimEnd().Contains("Days out of stock")) &&
                (worksheet.Range["K15"].Value.TrimEnd().Contains("Adjusted AMC")) &&
                (worksheet.Range["L15"].Value.TrimEnd().Contains("CLOSING BALANCE")) &&
                (worksheet.Range["M15"].Value.TrimEnd().Contains("MONTHS")) &&
                (worksheet.Range["N15"].Value.TrimEnd().Contains("QUANTITY REQUIRED")) &&

                (worksheet.Range["O15"].Value.TrimEnd() == "Notes"))
                {
                    mascisEntities context = new mascisEntities();
                    //context.Configuration.ProxyCreationEnabled = false;
                    order_hiv_rapid_test_kit_header testkitheader = new order_hiv_rapid_test_kit_header();
                    order_hiv_rapid_test_kit testkit = new order_hiv_rapid_test_kit();

                    string facilityName = worksheet.Range["E9"].FormulaStringValue.ToString().TrimEnd();
                    string district = worksheet.Range["G9"].Value.ToString();
                    string HSD = worksheet.Range["G10"].Value.ToString();

                    string reportStartDate = worksheet.Range["J10"].FormulaDateTime.ToString();
                    string reportStartDatestr = worksheet.Range["J10"].Value;
                    string reportEndDate = worksheet.Range["J11"].FormulaDateTime.ToString();
                    //string datePrepared = worksheet.Range["J12"].FormulaDateTime.ToString();
                    string datePrepared = worksheet.Range["J12"].Value.ToString();

                    string preparedByName = worksheet.Range["C39"].Value.ToString();
                    string reviewedByName = worksheet.Range["C42"].Value.ToString();
                    string preparedByDesignation = worksheet.Range["F39"].Value.ToString();
                    string reviewedByDesignation = worksheet.Range["F42"].Value.ToString();
                    string preparedByPhoneNo = worksheet.Range["J39"].Value.ToString();
                    string reviewedByPhoneNo = worksheet.Range["J42"].Value.ToString();

                    DateTime dateTime;
                    if (DateTime.TryParse(reportStartDate, out dateTime))
                    {
                        testkitheader.StartDate = dateTime;
                    }

                    if (DateTime.TryParse(reportEndDate, out dateTime))
                    {
                        testkitheader.EndDate = dateTime;
                    }
                    if (DateTime.TryParse(datePrepared, out dateTime))
                    {
                        testkitheader.DatePrepared = dateTime;
                    }
                    var facility = context.A_Facilities.FirstOrDefault(f => f.SAP_Code == facilityName);
                    int facilitycode = 0;
                    if (facility != null)
                    {
                        facilitycode = facility.FacilityCode;
                        testkitheader.FacilityCode = facility.FacilityCode;
                    }
                    if (testkitheader.FacilityCode > 0 &&
                           (DateTime.TryParse(reportStartDate, out dateTime)) &&
                           (DateTime.TryParse(reportEndDate, out dateTime)))
                    {
                        //var fac = context.order_hiv_rapid_test_kit_header.FirstOrDefault(o => o.FacilityCode == facilitycode && 
                        //o.StartDate == testkitheader.StartDate && o.EndDate == testkitheader.EndDate);
                        string _OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(testkitheader.DatePrepared), facilitycode, "03", "02");
                        var fac = context.order_hiv_rapid_test_kit_header.FirstOrDefault(o => o.OrderNumber == _OrderNumber);
                        testkitheader.OrderNumber = _OrderNumber;
                        testkitheader.OrderTypeId = 2;
                        testkitheader.FinalSubmission = true;
                        testkitheader.OrderStatusId = 6;
                        testkitheader.EditedDate = DateTime.Now;

                        if (fac != null)
                        {
                            context.Entry(fac).CurrentValues.SetValues(testkitheader);
                            context.Entry(fac).State = EntityState.Modified;
                        }
                        else
                        {
                            context.order_hiv_rapid_test_kit_header.Add(testkitheader);
                        }
                        context.SaveChanges();
                        for (int row = 18; row <= 29 /*worksheet.UsedRange.LastRow*/; row++)
                        {
                            if (worksheet.IsRowVisible(row))
                            {
                                if (row == 23)
                                {
                                    continue;
                                }
                                testkit = new order_hiv_rapid_test_kit();
                                testkit.OrderNumber = _OrderNumber;
                                testkit.Facility_Code = facilitycode;
                                testkit.Start_Date = testkitheader.StartDate;
                                testkit.End_Date = testkitheader.EndDate;
                                for (int column = 1; column <= worksheet.UsedRange.LastColumn; column++)
                                {

                                    switch (column)
                                    {
                                        case 3:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string serial = worksheet.Range[row, column].Value.ToString();

                                                if (int.TryParse(serial, out int item_code))
                                                {
                                                    testkit.item_code = item_code;

                                                }
                                            }
                                            break;
                                        case 6:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testkit.No_Test_Start_2Months = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.No_Test_Start_2Months)))
                                                {
                                                    testkit.No_Test_Start_2Months = null;
                                                }
                                            }
                                            break;
                                        case 7:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testkit.Test_Recieved_2Months = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.Test_Recieved_2Months)))
                                                {
                                                    testkit.Test_Recieved_2Months = null;
                                                }
                                            }

                                            break;

                                        case 8:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testkit.Test_Used_2Months = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.Test_Used_2Months)))
                                                {
                                                    testkit.Test_Used_2Months = null;
                                                }
                                            }

                                            break;

                                        case 9:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    testkit.Loss_Adjustment = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.Loss_Adjustment)))
                                                {
                                                    testkit.Loss_Adjustment = null;
                                                }
                                            }

                                            break;
                                        case 10:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testkit.DaysOutOfStockDuring2Months = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.DaysOutOfStockDuring2Months)))
                                                {
                                                    testkit.DaysOutOfStockDuring2Months = null;
                                                }
                                            }

                                            break;
                                        case 11:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testkit.AdjustedAMC = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.AdjustedAMC)))
                                                {
                                                    testkit.AdjustedAMC = null;
                                                }
                                            }

                                            break;
                                        case 12:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testkit.Test_Remaining = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.Test_Remaining)))
                                                {
                                                    testkit.Test_Remaining = null;
                                                }
                                            }
                                            break;
                                        case 13:
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testkit.Qunatity_On_Order = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testkit.Qunatity_On_Order)))
                                                {
                                                    testkit.Qunatity_On_Order = null;
                                                }
                                            }
                                            break;
                                        case 14://
                                            if (row >= 18 && row <= 29)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                //string coltext = worksheet.Range[row, column].Value.ToString();
                                                if (string.IsNullOrEmpty(coltext)) { }
                                                else
                                                {

                                                    double parsenum;
                                                    if (row == 24 || row == 23) { }
                                                    else
                                                    {
                                                        if (double.TryParse(coltext, out parsenum))
                                                        {
                                                            testkit.Quantity_Required = Math.Round(parsenum, 0);
                                                            if (parsenum < 0 || double.IsNaN(parsenum))
                                                            {
                                                                testkit.Quantity_Allocated = 0;
                                                            }
                                                            else
                                                            {
                                                                testkit.Quantity_Allocated = Convert.ToInt32(parsenum);
                                                            }
                                                        }
                                                        if (Double.IsNaN(System.Convert.ToDouble(testkit.Quantity_Required)))
                                                        {
                                                            testkit.Quantity_Required = null;
                                                            testkit.Quantity_Allocated = 0;
                                                        }
                                                    }
                                                }

                                            }
                                            break;
                                    }
                                }

                                //We need to check for existance of the testkit after testing for the header's existance as well

                                var objtestkit = context.order_hiv_rapid_test_kit.FirstOrDefault(o => o.OrderNumber == _OrderNumber
                                && o.item_code == testkit.item_code);
                                if (objtestkit != null)
                                {
                                    //context.Entry(objtestkit).CurrentValues.SetValues(testkit);
                                    objtestkit.No_Test_Start_2Months = testkit.No_Test_Start_2Months;
                                    objtestkit.Test_Recieved_2Months = testkit.Test_Recieved_2Months;
                                    objtestkit.Test_Used_2Months = testkit.Test_Used_2Months;
                                    objtestkit.Loss_Adjustment = testkit.Loss_Adjustment;
                                    objtestkit.Test_Remaining = testkit.Test_Remaining;
                                    objtestkit.Maximum_Stock = testkit.Maximum_Stock;
                                    objtestkit.Qunatity_On_Order = testkit.Qunatity_On_Order;
                                    objtestkit.Quantity_Required = testkit.Quantity_Required;
                                    objtestkit.Quantity_To_Ship = testkit.Quantity_To_Ship;
                                    objtestkit.Quantity_Allocated = testkit.Quantity_Allocated;
                                    objtestkit.RFSONotes = testkit.RFSONotes;
                                    objtestkit.DaysOutOfStockDuring2Months = testkit.DaysOutOfStockDuring2Months;
                                    objtestkit.AdjustedAMC = testkit.AdjustedAMC;
                                    objtestkit.Notes = testkit.Notes;
                                    context.Entry(objtestkit).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                                else
                                {
                                    //context.order_hiv_rapid_test_kit.Add(testkit);
                                    // if (orderDetails.DrugCode > 0)
                                    if (testkit.item_code > 0)
                                    {
                                        //Find out if the item exists
                                        var faccode = context.A_Product.FirstOrDefault(p => p.product_code == testkit.item_code);
                                        if (faccode == null)
                                        {
                                            continue;
                                        }
                                        try
                                        {
                                            //objtestkit.OrderNumber = _OrderNumber; 
                                            context.order_hiv_rapid_test_kit.Add(testkit);
                                            context.SaveChanges();
                                        }
                                        catch (Exception x)
                                        {
                                            throw x;
                                        }
                                    }

                                }

                            }

                        }

                        order_hiv_rapid_test_kit_summary testKitSummary = new order_hiv_rapid_test_kit_summary();
                        //Read the Bimonthly Summary of HIV Test by Purpose of Use
                        for (int row = 33; row <= 37; row++)
                        {
                            if (worksheet.IsRowVisible(row))
                            {
                                testKitSummary = new order_hiv_rapid_test_kit_summary();
                                testKitSummary.OrderNumber = _OrderNumber;
                                testKitSummary.Facility_Code = facilitycode;
                                testKitSummary.Start_Date = testkitheader.StartDate;
                                testKitSummary.End_Date = testkitheader.EndDate;


                                for (int column = 1; column <= worksheet.UsedRange.LastColumn; column++)
                                {
                                    switch (column)
                                    {
                                        case 3:
                                            if (row >= 33 && row <= 37)
                                            {
                                                string serial = worksheet.Range[row, column].Value.ToString();

                                                if (int.TryParse(serial, out int item_code))
                                                {
                                                    testKitSummary.item_code = item_code;
                                                }
                                            }
                                            break;
                                        case 5:
                                            if (row >= 33 && row <= 37)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testKitSummary.HCT = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testKitSummary.HCT)))
                                                {
                                                    testKitSummary.HCT = null;
                                                }
                                            }

                                            break;
                                        case 6:
                                            if (row >= 33 && row <= 37)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testKitSummary.PMTCT = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testKitSummary.PMTCT)))
                                                {
                                                    testKitSummary.PMTCT = null;
                                                }
                                            }
                                            break;
                                        case 7:
                                            if (row >= 33 && row <= 37)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testKitSummary.Clinical_Diagnosis = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testKitSummary.Clinical_Diagnosis)))
                                                {
                                                    testKitSummary.Clinical_Diagnosis = null;
                                                }
                                            }

                                            break;

                                        case 8:
                                            if (row >= 33 && row <= 37)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testKitSummary.SMC = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testKitSummary.SMC)))
                                                {
                                                    testKitSummary.SMC = null;
                                                }
                                            }

                                            break;

                                        case 9:
                                            if (row >= 33 && row <= 37)
                                            {

                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    testKitSummary.Quality_Control = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(testKitSummary.Quality_Control)))
                                                {
                                                    testKitSummary.Quality_Control = null;
                                                }
                                            }

                                            break;
                                        case 10:
                                            if (row >= 33 && row <= 37)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    //Total Column???
                                                }
                                            }

                                            break;

                                    }
                                }
                                var objtestkitsummary = context.order_hiv_rapid_test_kit_summary.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.item_code == testKitSummary.item_code);
                                if (objtestkitsummary != null)
                                {
                                    objtestkitsummary.HCT = testKitSummary.HCT;
                                    objtestkitsummary.PMTCT = testKitSummary.PMTCT;
                                    objtestkitsummary.Clinical_Diagnosis = testKitSummary.Clinical_Diagnosis;
                                    objtestkitsummary.SMC = testKitSummary.SMC;
                                    objtestkitsummary.Quality_Control = testKitSummary.Quality_Control;
                                    context.Entry(objtestkitsummary).State = EntityState.Modified;
                                }
                                else
                                {
                                    if (testKitSummary.item_code > 0)
                                    {
                                        //Find out if the item exists
                                        var faccode = context.A_Product.FirstOrDefault(p => p.product_code == testKitSummary.item_code);
                                        if (faccode == null)
                                        {
                                            continue;
                                        }
                                        // objtestkitsummary.OrderNumber = _OrderNumber;
                                        context.order_hiv_rapid_test_kit_summary.Add(testKitSummary);
                                    }

                                }
                                context.SaveChanges();
                            }

                        }

                    }


                }
                //}
                #endregion "(HMIS Form 018b2) BIMONTHLY REPORT AND ORDER CALCULATION FORM FOR HIV TESTS"  

                //Import                  "ARV and E-MTCT Medicines Order Form and Patient Report"
                #region "ARV and E-MTCT Medicines Order Form and Patient Report"

                if (
                (worksheet.Range["A7"].Value.Contains("Drug Formulation and Strength")) &&
                (worksheet.Range["E8"].Value.Contains("Basic Unit")) &&
                (worksheet.Range["F8"].Value.Contains("Product code")) &&
                (worksheet.Range["G7"].Value.Contains("OPENING BALANCE")) &&
                (worksheet.Range["H7"].Value.Contains("QUANTITY RECEIVED")) &&
                (worksheet.Range["I7"].Value.Contains("ART & e-MTCT CONSUMPTION")) &&
                (worksheet.Range["J7"].Value.Contains("LOSSES")) &&
                (worksheet.Range["K8"].Value.Contains("Days out of stock")) &&
                (worksheet.Range["L8"].Value.Contains("Adjusted AMC")) &&
                (worksheet.Range["M7"].Value.Contains("CLOSING BALANCE")) &&
                (worksheet.Range["N7"].Value.Contains("MONTHS OF STOCK ON-HAND")) &&
                (worksheet.Range["O7"].Value.Contains("QUANTITY REQUIRED FOR CURRENT PATIENTS")) &&
                (worksheet.Range["P7"].Value.Contains("Notes")))
                {
                    mascisEntities context = new mascisEntities();
                    //context.Configuration.ProxyCreationEnabled = false;
                    Order_Header orderHeader = new Order_Header();
                    Order_DrugDetails orderDetails = new Order_DrugDetails();
                    //Order_DrugDetails orderDetails = new Order_DrugDetails();
                    Treatment_PatientSummary treatment_PatientSummary = new Treatment_PatientSummary();
                    bll_A_DrugRegimen objx = new bll_A_DrugRegimen();

                    string facilityName = worksheet.Range["C2"].FormulaStringValue.ToString().TrimEnd();
                    string startDate = worksheet.Range["M3"].FormulaDateTime.ToString();
                    string endDate = worksheet.Range["M4"].FormulaDateTime.ToString();
                    string datePrepared = worksheet.Range["M5"].DateTime.ToString();

                    DateTime dateTime;
                    if (DateTime.TryParse(startDate, out dateTime))
                    {
                        orderHeader.StartDate = dateTime;
                    }

                    if (DateTime.TryParse(endDate, out dateTime))
                    {
                        orderHeader.EndDate = dateTime;
                    }

                    if (DateTime.TryParse(datePrepared, out dateTime))
                    {
                        orderHeader.DatePrepared = dateTime;
                    }

                    var facility = context.A_Facilities.FirstOrDefault(f => f.SAP_Code == facilityName);
                    int facilitycode = 0;
                    if (facility != null)
                    {
                        facilitycode = facility.FacilityCode;
                        orderHeader.FacilityCode = facility.FacilityCode;
                    }
                    string _OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(orderHeader.DatePrepared), facilitycode, "02", "02");
                    var ord = context.Order_Header.FirstOrDefault(o => o.OrderNumber == _OrderNumber);
                    orderHeader.OrderNumber = _OrderNumber;
                    orderHeader.OrderTypeId = 2;
                    orderHeader.FinalSubmission = true;
                    orderHeader.OrderStatusId = 6;
                    orderHeader.EditedDate = DateTime.Now;
                    //orderHeader.DatePrepared = _OrderNumber;
                    if (ord != null)
                    {
                        ord.DatePrepared = orderHeader.DatePrepared;
                        ord.Adults = orderHeader.Adults;
                        ord.Children = orderHeader.Children;
                        //ord.AddedBy = orderHeader.AddedBy;
                        //ord.AddedDate = orderHeader.AddedDate;
                        //ord.EditedBy = orderHeader.EditedBy;
                        //ord.EditedDate = orderHeader.EditedDate;
                        //ord.RecordStatus = orderHeader.RecordStatus;
                        //ord.EmergencyOrder = orderHeader.EmergencyOrder;
                        ord.FinalSubmission = true;
                        //ord.Facility_UserName = orderHeader.Facility_UserName;
                        //ord.RFSO_SentBackTofacility = orderHeader.RFSO_SentBackTofacility;
                        //ord.RFSO_SentBackBy = orderHeader.RFSO_SentBackBy;
                        //ord.RFSO_SentBackDate = orderHeader.RFSO_SentBackDate;
                        //ord.RFSO_ApproveOrder = orderHeader.RFSO_ApproveOrder;
                        //ord.RFSO_ApprovedBy = orderHeader.RFSO_ApprovedBy;
                        //ord.RFSO_DateOfApproval = orderHeader.RFSO_DateOfApproval;
                        //ord.EportedToSAP = orderHeader.EportedToSAP;

                        context.Entry(ord).State = EntityState.Modified;
                    }
                    else
                    {
                        context.Order_Header.Add(orderHeader);
                    }
                    context.SaveChanges();

                    for (int row = 11; row <= 53 /*worksheet.UsedRange.LastRow*/; row++)
                    {
                        if (worksheet.IsRowVisible(row))
                        {
                            if (row == 23 || row == 24 || row == 33 || row == 35 || row == 44 || row == 47)
                            {
                                continue;
                            }
                            orderDetails = new Order_DrugDetails();
                            orderDetails.OrderNumber = _OrderNumber;
                            orderDetails.FacilityCode = facilitycode;
                            orderDetails.StartDate = orderHeader.StartDate;
                            orderDetails.EndDate = orderHeader.EndDate;
                            for (int column = 1; column <= worksheet.UsedRange.LastColumn; column++)
                            {
                                switch (column)
                                {
                                    case 6:
                                        //if (row == 11 && row == 14 && row == 13 && row == 15 && row == 16 && (row != 23 || row != 24 || row != 33 || row != 35 || row != 44 || row != 47))

                                        if (row >= 11 && row <= 53)
                                        {
                                            string serial = worksheet.Range[row, column].Value.ToString();

                                            if (int.TryParse(serial, out int item_code))
                                            {
                                                orderDetails.DrugCode = item_code;
                                            }
                                        }
                                        break;
                                    case 7:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.OpeningBalance = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.OpeningBalance)))
                                            {
                                                orderDetails.OpeningBalance = null;
                                            }
                                        }
                                        break;
                                    case 8:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.QuantityRecieved = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.QuantityRecieved)))
                                            {
                                                orderDetails.QuantityRecieved = null;
                                            }
                                        }

                                        break;

                                    case 9:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.ART_eMTCT_Consumption = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.ART_eMTCT_Consumption)))
                                            {
                                                orderDetails.ART_eMTCT_Consumption = null;
                                            }
                                        }

                                        break;

                                    case 10:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();

                                            if (double.TryParse(coltext, out double parsenum))
                                            {
                                                orderDetails.Losses_Adjustments = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.Losses_Adjustments)))
                                            {
                                                orderDetails.Losses_Adjustments = null;
                                            }
                                        }

                                        break;
                                    case 11:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.DaysOutOfStockDuring2Months = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.DaysOutOfStockDuring2Months)))
                                            {
                                                orderDetails.DaysOutOfStockDuring2Months = null;

                                            }
                                        }

                                        break;
                                    case 12:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.AdjustedAMC = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.AdjustedAMC)))
                                            {
                                                orderDetails.AdjustedAMC = null;

                                            }
                                        }

                                        break;
                                    case 13:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.ClosingBalance = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.ClosingBalance)))
                                            {
                                                orderDetails.ClosingBalance = null;
                                            }
                                        }
                                        break;
                                    case 14:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.Months_Stock_atHand = parsenum;
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.Months_Stock_atHand)))
                                            {
                                                orderDetails.Months_Stock_atHand = null;

                                            }
                                        }
                                        break;
                                    case 15:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            double parsenum;
                                            if (double.TryParse(coltext, out parsenum))
                                            {
                                                orderDetails.Quantity_Required_Current_Patients = Math.Round(parsenum, 0);
                                                if (parsenum < 0 || double.IsNaN(parsenum))
                                                {
                                                    orderDetails.Quantity_Allocated = 0;
                                                }
                                                else
                                                {
                                                    orderDetails.Quantity_Allocated = Math.Round(parsenum, 0);
                                                }
                                            }
                                            if (Double.IsNaN(System.Convert.ToDouble(orderDetails.Quantity_Required_Current_Patients)))
                                            {
                                                orderDetails.Quantity_Required_Current_Patients = null;
                                                orderDetails.Quantity_Allocated = 0;

                                            }
                                        }
                                        break;

                                    case 16:
                                        if (row >= 11 && row <= 53)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();

                                            orderDetails.Notes = worksheet.Range[row, column].Value.ToString();
                                        }
                                        break;
                                }
                            }
                            var objorderDetails = context.Order_DrugDetails.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.DrugCode == orderDetails.DrugCode);
                            if (objorderDetails != null)
                            {
                                objorderDetails.OpeningBalance = orderDetails.OpeningBalance;
                                objorderDetails.QuantityRecieved = orderDetails.QuantityRecieved;
                                objorderDetails.PMTCT_Consumption = orderDetails.PMTCT_Consumption;
                                objorderDetails.ART_Consumption = orderDetails.ART_Consumption;
                                objorderDetails.Losses_Adjustments = orderDetails.Losses_Adjustments;
                                objorderDetails.ClosingBalance = orderDetails.ClosingBalance;
                                objorderDetails.Months_Stock_atHand = orderDetails.Months_Stock_atHand;
                                objorderDetails.Quantity_Required_Current_Patients = orderDetails.Quantity_Required_Current_Patients;
                                objorderDetails.EstimatedNew_ART_Patients = orderDetails.EstimatedNew_ART_Patients;
                                objorderDetails.EstimatedNew_HIV_Pregnant = orderDetails.EstimatedNew_HIV_Pregnant;
                                objorderDetails.DrugsRequired_NewPatients = orderDetails.DrugsRequired_NewPatients;
                                objorderDetails.TotalDrugs_Required = orderDetails.TotalDrugs_Required;
                                objorderDetails.Notes = orderDetails.Notes;
                                objorderDetails.Quantity_Allocated = orderDetails.Quantity_Allocated;
                                objorderDetails.RFSONotes = orderDetails.RFSONotes;
                                objorderDetails.DaysOutOfStockDuring2Months = orderDetails.DaysOutOfStockDuring2Months;
                                objorderDetails.AdjustedAMC = orderDetails.AdjustedAMC;
                                objorderDetails.ART_eMTCT_Consumption = orderDetails.ART_eMTCT_Consumption;
                                context.Entry(objorderDetails).State = EntityState.Modified;

                                context.SaveChanges();
                            }
                            else
                            {
                                context.Order_DrugDetails.Add(orderDetails);
                                if (orderDetails.DrugCode > 0)
                                {
                                    //Find out if the item exists
                                    //if (orderDetails.DrugCode != 110028)
                                    {
                                        var faccode = context.A_Product.FirstOrDefault(p => p.product_code == orderDetails.DrugCode);
                                        if (faccode != null)
                                        {
                                            try
                                            {
                                                orderDetails.OrderNumber = _OrderNumber;
                                                orderDetails.DrugCode = faccode.product_code;
                                                context.SaveChanges();
                                            }
                                            catch (Exception)
                                            {
                                                //throw new Exception ex.Message;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }

                    // Save SUMMARY: ART PATIENTS PER TREATMENT REGIMEN (Existing and New Enrollments): 
                    #region First Line Regimen
                    for (int row = 58; row <= 66; row++)
                    {
                        if (worksheet.IsRowVisible(row))
                        {
                            #region TOTAL ADULT ART & e-MTCT PATIENTS
                            treatment_PatientSummary = new Treatment_PatientSummary();

                            treatment_PatientSummary.OrderNumber = _OrderNumber;
                            treatment_PatientSummary.FacilityCode = facilitycode;
                            treatment_PatientSummary.StartDate = orderHeader.StartDate;
                            treatment_PatientSummary.EndDate = orderHeader.EndDate;

                            //bll_A_DrugRegimen objx = new bll_A_DrugRegimen();
                            for (int column = 2; column <= 4; column++)
                            {
                                switch (column)
                                {
                                    case 2:
                                        string serial = worksheet.Range[row, column].Value.ToString();
                                        string[] b = serial.Split('.');
                                        if (b.Length > 1)
                                        {
                                            if (objx.getRecord(b[1], 1, 1, true) != null)
                                            {
                                                treatment_PatientSummary.RegimenCode = objx.getRecord(b[1], 1, 1, true).RegimenCode;
                                            }
                                        }

                                        break;
                                    case 3:

                                        if (row >= 58 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Patients_Existing = parsenum;
                                            }
                                        }

                                        break;
                                    case 4:
                                        if (row >= 58 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Patients_New = parsenum;
                                            }
                                        }
                                        break;
                                }
                            }

                            var objtreatment_PatientSummary = context.Treatment_PatientSummary.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.RegimenCode == treatment_PatientSummary.RegimenCode);
                            if (objtreatment_PatientSummary != null)
                            {
                                objtreatment_PatientSummary.Patients_Existing = treatment_PatientSummary.Patients_Existing;
                                objtreatment_PatientSummary.Patients_New = treatment_PatientSummary.Patients_New;
                                context.Entry(objtreatment_PatientSummary).State = EntityState.Modified;
                            }
                            else
                            {
                                context.Treatment_PatientSummary.Add(treatment_PatientSummary);
                            }
                            context.SaveChanges();
                            #endregion TOTAL ADULT ART & e-MTCT PATIENTS

                            #region "Pediatric First Line Patients "
                            treatment_PatientSummary = new Treatment_PatientSummary();
                            treatment_PatientSummary.OrderNumber = _OrderNumber;
                            treatment_PatientSummary.FacilityCode = facilitycode;
                            treatment_PatientSummary.StartDate = orderHeader.StartDate;
                            treatment_PatientSummary.EndDate = orderHeader.EndDate;

                            objx = new bll_A_DrugRegimen();
                            for (int column = 7; column <= 16; column++)
                            {
                                switch (column)
                                {
                                    case 7:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string serial = worksheet.Range[row, column].Value.ToString();
                                            string[] b = serial.Split('.');
                                            //if (b.Length > 1)
                                            if (serial != null)
                                            {
                                                //if (objx.getRecord(b[1], 1, 1, true) != null)
                                                if (objx.getRecord(serial, 2, 1, true) != null)
                                                {
                                                    treatment_PatientSummary.RegimenCode = objx.getRecord(serial, 2, 1, true).RegimenCode;
                                                    //                                                treatment_PatientSummary.RegimenCode = objx.getRecord(b[1], 1, 1, true).RegimenCode;
                                                }
                                            }
                                        }
                                        break;
                                    case 9:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Existing_0_3Yrs = parsenum;
                                            }
                                        }

                                        break;

                                    case 10:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_0_3Yrs = parsenum;
                                            }
                                        }

                                        break;

                                    case 11:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();

                                            if (int.TryParse(coltext, out int parsenum))
                                            {
                                                treatment_PatientSummary.Existing_3_10Yrs = parsenum;
                                            }
                                        }

                                        break;
                                    case 12:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_3_10Yrs = parsenum;
                                            }
                                        }

                                        break;
                                    case 13:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Existing_10_15Yrs_Less35Kg = parsenum;
                                            }
                                        }

                                        break;
                                    case 14:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_10_15Yrs_Less35Kg = parsenum;
                                            }
                                        }

                                        break;
                                    case 15:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Existing_10_15Yrs_Greater35Kg = parsenum;
                                            }
                                        }
                                        break;
                                    case 16:
                                        if (row >= 59 && row <= 66)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_10_15Yrs_Greater35Kg = parsenum;
                                            }

                                        }
                                        break;

                                }
                            }
                            if (row >= 59 && row <= 66)
                            {
                                objtreatment_PatientSummary = context.Treatment_PatientSummary.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.RegimenCode == treatment_PatientSummary.RegimenCode);
                                if (objtreatment_PatientSummary != null)
                                {
                                    objtreatment_PatientSummary.Patients_Existing = treatment_PatientSummary.Patients_Existing;
                                    objtreatment_PatientSummary.Patients_New = treatment_PatientSummary.Patients_New;
                                    context.Entry(objtreatment_PatientSummary).State = EntityState.Modified;
                                }
                                else
                                {
                                    //if (objtreatment_PatientSummary.RegimenCode > 0)
                                    //{
                                    //Find out if the item exists
                                    //var faccode = context.A_DrugRegimen.FirstOrDefault(p => p.RegimenCode == objtreatment_PatientSummary.RegimenCode);
                                    //if (faccode == null)
                                    //{
                                    //    continue;
                                    //}
                                    // objtreatment_PatientSummary.OrderNumber = _OrderNumber;
                                    context.Treatment_PatientSummary.Add(treatment_PatientSummary);
                                    //}

                                }
                                //{
                                //    context.Treatment_PatientSummary.Add(treatment_PatientSummary);
                                //}
                                context.SaveChanges();
                            }
                            #endregion Pediatric First Line Patients 
                        }
                        if (worksheet.IsRowVisible(row))
                        { }
                    }
                    #endregion First Line Regimen

                    #region Second Line Regimen

                    for (int row = 71; row <= 78; row++)
                    {
                        if (worksheet.IsRowVisible(row))
                        {
                            #region TOTAL ADULT ART & e-MTCT PATIENTS - Adult Second Line Patients
                            treatment_PatientSummary = new Treatment_PatientSummary();

                            treatment_PatientSummary.OrderNumber = _OrderNumber;
                            treatment_PatientSummary.FacilityCode = facilitycode;
                            treatment_PatientSummary.StartDate = orderHeader.StartDate;
                            treatment_PatientSummary.EndDate = orderHeader.EndDate;

                            // bll_A_DrugRegimen objx = new bll_A_DrugRegimen();
                            for (int column = 2; column <= 4; column++)
                            {
                                switch (column)
                                {
                                    case 2:
                                        if (row >= 71 && row <= 76)
                                        {
                                            string serial = worksheet.Range[row, column].Value.ToString();
                                            string[] b = serial.Split('.');
                                            if (b.Length > 1)
                                            {
                                                if (objx.getRecord(b[1], 1, 2, true) != null)
                                                {
                                                    treatment_PatientSummary.RegimenCode = objx.getRecord(b[1], 1, 1, true).RegimenCode;
                                                }
                                            }
                                        }

                                        break;
                                    case 3:

                                        if (row >= 71 && row <= 76)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Patients_Existing = parsenum;
                                            }
                                        }

                                        break;
                                    case 4:
                                        if (row >= 71 && row <= 76)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Patients_New = parsenum;
                                            }
                                        }
                                        break;
                                }
                            }
                            if (row >= 71 && row <= 76)
                            {
                                var objtreatment_PatientSummary2 = context.Treatment_PatientSummary.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.RegimenCode == treatment_PatientSummary.RegimenCode);
                                if (objtreatment_PatientSummary2 != null)
                                {
                                    objtreatment_PatientSummary2.Patients_Existing = treatment_PatientSummary.Patients_Existing;
                                    objtreatment_PatientSummary2.Patients_New = treatment_PatientSummary.Patients_New;
                                    context.Entry(objtreatment_PatientSummary2).State = EntityState.Modified;
                                }
                                else
                                {
                                    context.Treatment_PatientSummary.Add(treatment_PatientSummary);
                                }
                                context.SaveChanges();

                            }

                            #endregion TOTAL ADULT ART & e-MTCT PATIENTS - Adult Second Line Patients

                            #region "Pediatric Second Line Patients "
                            treatment_PatientSummary = new Treatment_PatientSummary();
                            treatment_PatientSummary.OrderNumber = _OrderNumber;
                            treatment_PatientSummary.FacilityCode = facilitycode;
                            treatment_PatientSummary.StartDate = orderHeader.StartDate;
                            treatment_PatientSummary.EndDate = orderHeader.EndDate;

                            if (row >= 72 && row <= 78)
                            {
                                objx = new bll_A_DrugRegimen();
                                for (int column = 7; column <= 16; column++)
                                {
                                    switch (column)
                                    {
                                        case 7:
                                            //if (row >= 71 && row <= 78)
                                            {
                                                string serial = worksheet.Range[row, column].Value.ToString();
                                                string[] b = serial.Split('.');

                                                if (serial != null)
                                                {
                                                    //if (objx.getRecord(b[1], 1, 1, true) != null)
                                                    if (objx.getRecord(serial, 2, 2, true) != null)
                                                    {
                                                        treatment_PatientSummary.RegimenCode = objx.getRecord(serial, 1, 1, true).RegimenCode;
                                                    }
                                                }
                                            }
                                            break;
                                        case 9:
                                            //if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                int parsenum;
                                                if (int.TryParse(coltext, out parsenum))
                                                {
                                                    treatment_PatientSummary.Existing_0_3Yrs = parsenum;
                                                }
                                            }

                                            break;

                                        case 10:
                                            //if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                int parsenum;
                                                if (int.TryParse(coltext, out parsenum))
                                                {
                                                    treatment_PatientSummary.New_0_3Yrs = parsenum;
                                                }
                                            }

                                            break;

                                        case 11:
                                            // if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (int.TryParse(coltext, out int parsenum))
                                                {
                                                    treatment_PatientSummary.Existing_3_10Yrs = parsenum;
                                                }
                                            }

                                            break;
                                        case 12:
                                            // if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                int parsenum;
                                                if (int.TryParse(coltext, out parsenum))
                                                {
                                                    treatment_PatientSummary.New_3_10Yrs = parsenum;
                                                }
                                            }

                                            break;
                                        case 13:
                                            //if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                int parsenum;
                                                if (int.TryParse(coltext, out parsenum))
                                                {
                                                    treatment_PatientSummary.Existing_10_15Yrs_Less35Kg = parsenum;
                                                }
                                            }

                                            break;
                                        case 14:
                                            // if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                int parsenum;
                                                if (int.TryParse(coltext, out parsenum))
                                                {
                                                    treatment_PatientSummary.New_10_15Yrs_Less35Kg = parsenum;
                                                }
                                            }

                                            break;
                                        case 15:
                                            //if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                int parsenum;
                                                if (int.TryParse(coltext, out parsenum))
                                                {
                                                    treatment_PatientSummary.Existing_10_15Yrs_Greater35Kg = parsenum;
                                                }
                                            }
                                            break;
                                        case 16:
                                            //if (row >= 71 && row <= 78)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                int parsenum;
                                                if (int.TryParse(coltext, out parsenum))
                                                {
                                                    treatment_PatientSummary.New_10_15Yrs_Greater35Kg = parsenum;
                                                }

                                            }
                                            break;

                                    }
                                }

                                var objtreatment_PatientSummary = context.Treatment_PatientSummary.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.RegimenCode == treatment_PatientSummary.RegimenCode);
                                if (objtreatment_PatientSummary != null)
                                {
                                    objtreatment_PatientSummary.Patients_Existing = treatment_PatientSummary.Patients_Existing;
                                    objtreatment_PatientSummary.Patients_New = treatment_PatientSummary.Patients_New;
                                    context.Entry(objtreatment_PatientSummary).State = EntityState.Modified;
                                }
                                else
                                {
                                    context.Treatment_PatientSummary.Add(treatment_PatientSummary);
                                }
                                context.SaveChanges();
                            }
                            #endregion Pediatric Second Line Patients 
                        }

                    }
                    #endregion Second Line Regimen

                    #region Third Line Regimen

                    for (int row = 83; row <= 91; row++)
                    {
                        if (worksheet.IsRowVisible(row))
                        {
                            #region TOTAL ADULT ART & e-MTCT PATIENTS - Adult Second Line Patients
                            treatment_PatientSummary = new Treatment_PatientSummary();

                            treatment_PatientSummary.OrderNumber = _OrderNumber;
                            treatment_PatientSummary.FacilityCode = facilitycode;
                            treatment_PatientSummary.StartDate = orderHeader.StartDate;
                            treatment_PatientSummary.EndDate = orderHeader.EndDate;

                            //bll_A_DrugRegimen objx = new bll_A_DrugRegimen();
                            for (int column = 2; column <= 4; column++)
                            {
                                switch (column)
                                {
                                    case 2:
                                        if (row >= 83 && row <= 91)
                                        {
                                            string serial = worksheet.Range[row, column].Value.ToString();
                                            string[] b = serial.Split('.');
                                            //if (b.Length > 1)
                                            if (serial != null)
                                            {
                                                if (objx.getRecord(serial, 1, 5, true) != null)
                                                {
                                                    treatment_PatientSummary.RegimenCode = objx.getRecord(b[0], 1, 1, true).RegimenCode;
                                                }
                                            }
                                        }

                                        break;
                                    case 3:

                                        if (row >= 83 && row <= 91)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Patients_Existing = parsenum;
                                            }
                                        }

                                        break;
                                    case 4:
                                        if (row >= 83 && row <= 91)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Patients_New = parsenum;
                                            }
                                        }
                                        break;
                                }
                            }
                            if (row >= 83 && row <= 91)
                            {
                                var objtreatment_PatientSummary2 = context.Treatment_PatientSummary.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.RegimenCode == treatment_PatientSummary.RegimenCode);
                                if (objtreatment_PatientSummary2 != null)
                                {
                                    objtreatment_PatientSummary2.Patients_Existing = treatment_PatientSummary.Patients_Existing;
                                    objtreatment_PatientSummary2.Patients_New = treatment_PatientSummary.Patients_New;
                                    context.Entry(objtreatment_PatientSummary2).State = EntityState.Modified;
                                }
                                else
                                {
                                    context.Treatment_PatientSummary.Add(treatment_PatientSummary);
                                }
                                context.SaveChanges();

                            }

                            #endregion TOTAL ADULT ART & e-MTCT PATIENTS - Adult Second Line Patients

                            #region "Pediatric Third Line Patients "

                            treatment_PatientSummary = new Treatment_PatientSummary();
                            treatment_PatientSummary.OrderNumber = _OrderNumber;
                            treatment_PatientSummary.FacilityCode = facilitycode;
                            treatment_PatientSummary.StartDate = orderHeader.StartDate;
                            treatment_PatientSummary.EndDate = orderHeader.EndDate;

                            objx = new bll_A_DrugRegimen();
                            for (int column = 7; column <= 16; column++)
                            {
                                switch (column)
                                {
                                    case 7:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string serial = worksheet.Range[row, column].Value.ToString();
                                            string[] b = serial.Split('.');
                                            if (serial != null)
                                            {
                                                //if (objx.getRecord(b[1], 1, 1, true) != null)
                                                if (objx.getRecord(serial, 2, 5, true) != null)
                                                {
                                                    treatment_PatientSummary.RegimenCode = objx.getRecord(b[0], 1, 1, true).RegimenCode;
                                                }
                                            }
                                        }
                                        break;
                                    case 9:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Existing_0_3Yrs = parsenum;
                                            }
                                        }

                                        break;

                                    case 10:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_0_3Yrs = parsenum;
                                            }
                                        }

                                        break;

                                    case 11:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();

                                            if (int.TryParse(coltext, out int parsenum))
                                            {
                                                treatment_PatientSummary.Existing_3_10Yrs = parsenum;
                                            }
                                        }

                                        break;
                                    case 12:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_3_10Yrs = parsenum;
                                            }
                                        }

                                        break;
                                    case 13:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Existing_10_15Yrs_Less35Kg = parsenum;
                                            }
                                        }

                                        break;
                                    case 14:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_10_15Yrs_Less35Kg = parsenum;
                                            }
                                        }

                                        break;
                                    case 15:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].Value.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.Existing_10_15Yrs_Greater35Kg = parsenum;
                                            }
                                        }
                                        break;
                                    case 16:
                                        if (row >= 84 && row <= 89)
                                        {
                                            string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            int parsenum;
                                            if (int.TryParse(coltext, out parsenum))
                                            {
                                                treatment_PatientSummary.New_10_15Yrs_Greater35Kg = parsenum;
                                            }

                                        }
                                        break;

                                }
                            }
                            if (row >= 84 && row <= 89)
                            {
                                var objtreatment_PatientSummary = context.Treatment_PatientSummary.FirstOrDefault(o => o.OrderNumber == _OrderNumber && o.RegimenCode == treatment_PatientSummary.RegimenCode);
                                if (objtreatment_PatientSummary != null)
                                {
                                    objtreatment_PatientSummary.Patients_Existing = treatment_PatientSummary.Patients_Existing;
                                    objtreatment_PatientSummary.Patients_New = treatment_PatientSummary.Patients_New;
                                    context.Entry(objtreatment_PatientSummary).State = EntityState.Modified;
                                }
                                else
                                {
                                    context.Treatment_PatientSummary.Add(treatment_PatientSummary);
                                }
                                context.SaveChanges();
                            }

                            #endregion Pediatric Third Line Patients 
                        }

                    }
                    #endregion Third Line Regimen



                }
                #endregion "ARV and E-MTCT Medicines Order Form and Patient Report"  

                //Import "ARV and E-MTCT Medicines Order Form and Patient Report"




                #region "HMIS FORM 084: BI-MONTHLY REPORT AND ORDER CALCULATION FORM"

                //if (worksheet.Range["G7"].Value.ToUpper().TrimEnd().Length >= 7 && worksheet.Range["H7"].Value.ToUpper().TrimEnd().Length >= 6  )
                //{
                if (
                (worksheet.Range["A7"].Value.Contains("Item Code")) &&
                (worksheet.Range["B7"].Value.Contains("Item Description")) &&
                (worksheet.Range["C7"].Value.Contains("Basic Unit")) &&
                (worksheet.Range["D7"].Value.Contains("Physical Count")) &&
                (worksheet.Range["E7"].Value.Contains("Quantity Received")) &&
                (worksheet.Range["F7"].Value.Contains("Quantity Used")) &&
                (worksheet.Range["G7"].Value.Contains("Losses")) &&
                (worksheet.Range["H7"].Value.Contains("Number")) &&
                (worksheet.Range["I7"].Value.Contains("This Month Physical Count")) &&
                (worksheet.Range["J7"].Value.Contains("AMC")) &&
                (worksheet.Range["K7"].Value.Contains("Months of Stock")) &&
                (worksheet.Range["L7"].Value.Contains("Maximum Stock Quantity")) &&
                (worksheet.Range["M7"].Value.Contains("Quantity Required")) &&
                (worksheet.Range["N7"].Value.Contains("Notes")))
                {
                    mascisEntities context = new mascisEntities();
                    //context.Configuration.ProxyCreationEnabled = false;
                    Order_OI_STI_Header orderHeader = new Order_OI_STI_Header();
                    Order_OI_STI_Detail orderDetails = new Order_OI_STI_Detail();

                    string facilityName = worksheet.Range["B3"].FormulaStringValue.ToString().TrimEnd();

                    string startDate = worksheet.Range["L4"].FormulaDateTime.ToString();
                    string endDate = worksheet.Range["L5"].FormulaDateTime.ToString();
                    string datePrepared = worksheet.Range["L6"].DateTime.ToString();

                    DateTime dateTime;
                    if (DateTime.TryParse(startDate, out dateTime))
                    {
                        orderHeader.StartDate = dateTime;
                    }

                    if (DateTime.TryParse(endDate, out dateTime))
                    {
                        orderHeader.EndDate = dateTime;
                    }

                    if (DateTime.TryParse(datePrepared, out dateTime))
                    {
                        orderHeader.DatePrepared = dateTime;
                    }

                    var facility = context.A_Facilities.FirstOrDefault(f => f.SAP_Code == facilityName);
                    int facilitycode = 0;
                    if (facility != null)
                    {
                        facilitycode = facility.FacilityCode;
                        orderHeader.FacilityCode = facility.FacilityCode;
                    }

                    //var ord = context.Order_OI_STI_Header.FirstOrDefault(o => o.FacilityCode == facilitycode && o.StartDate == orderHeader.StartDate && o.EndDate == orderHeader.EndDate);
                    string _OrderNumber = new OrderNumber().GetOrderNumber(System.Convert.ToDateTime(orderHeader.DatePrepared), facilitycode, "09", "02");
                    var ord = context.Order_OI_STI_Header.FirstOrDefault(o => o.OrderNumber == _OrderNumber);
                    orderHeader.OrderNumber = _OrderNumber;
                    orderHeader.OrderTypeId = 2;
                    orderHeader.FinalSubmission = true;
                    orderHeader.OrderStatusId = 6;
                    orderHeader.EditedDate = DateTime.Now;
                    // orderHeader.EditedBy = new UserManagement().getCurrentuser();

                    if (ord != null)
                    {
                        ord.DatePrepared = orderHeader.DatePrepared;
                        ord.EditedBy = orderHeader.EditedBy;
                        ord.EditedDate = orderHeader.EditedDate;
                        //ord.RecordStatus = orderHeader.RecordStatus;
                        ord.EmergencyOrder = orderHeader.EmergencyOrder;
                        ord.FinalSubmission = true;
                        //ord.ExportedToSAP = orderHeader.ExportedToSAP;

                        context.Entry(ord).State = EntityState.Modified;
                    }
                    else
                    {
                        orderHeader.OrderNumber = _OrderNumber;
                        context.Order_OI_STI_Header.Add(orderHeader);
                    }

                    if (orderHeader.FacilityCode > 0 &&
                          (DateTime.TryParse(startDate, out dateTime)) &&
                          (DateTime.TryParse(endDate, out dateTime)))
                    {
                        context.SaveChanges();

                        for (int row = 10; row <= 64 /*worksheet.UsedRange.LastRow*/; row++)
                        {
                            if (worksheet.IsRowVisible(row))
                            {
                                if (row == 22 || row == 29 || row == 32 || row == 35 || row == 37 || row == 42 || row == 48 || row == 56)
                                {
                                    continue;
                                }
                                orderDetails = new Order_OI_STI_Detail();
                                orderDetails.OrderNumber = _OrderNumber;
                                orderDetails.FacilityCode = facilitycode;
                                orderDetails.StartDate = orderHeader.StartDate;
                                orderDetails.EndDate = orderHeader.EndDate;
                                for (int column = 1; column <= worksheet.UsedRange.LastColumn; column++)
                                {
                                    switch (column)
                                    {
                                        case 1:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string serial = worksheet.Range[row, column].Value.ToString();
                                                if (int.TryParse(serial, out int item_code))
                                                {
                                                    orderDetails.ProductCode = item_code;
                                                }
                                            }
                                            break;
                                        case 4:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.PhysicalCountAtBeginningOfReviewPeriod = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.PhysicalCountAtBeginningOfReviewPeriod)))
                                                {
                                                    orderDetails.PhysicalCountAtBeginningOfReviewPeriod = null;

                                                }
                                            }
                                            break;
                                        case 5:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.QuantityReceivedDuringTheTwoMonthsCycle = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.QuantityReceivedDuringTheTwoMonthsCycle)))
                                                {
                                                    orderDetails.QuantityReceivedDuringTheTwoMonthsCycle = null;

                                                }
                                            }

                                            break;

                                        case 6:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.QuantityRequired = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.QuantityRequired)))
                                                {
                                                    orderDetails.QuantityRequired = null;

                                                }
                                            }

                                            break;

                                        case 7:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();

                                                if (double.TryParse(coltext, out double parsenum))
                                                {
                                                    orderDetails.QuantityUsedDuringTwoMonths = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.QuantityUsedDuringTwoMonths)))
                                                {
                                                    orderDetails.QuantityUsedDuringTwoMonths = null;

                                                }
                                            }

                                            break;
                                        case 8:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.LossesAndAdjustments = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.LossesAndAdjustments)))
                                                {
                                                    orderDetails.LossesAndAdjustments = null;

                                                }
                                            }

                                            break;
                                        case 9:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.NumberOfDaysOutOfStock = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.NumberOfDaysOutOfStock)))
                                                {
                                                    orderDetails.NumberOfDaysOutOfStock = null;

                                                }

                                            }

                                            break;
                                        case 10:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].Value.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.ThisMonthPhysicalCount = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.ThisMonthPhysicalCount)))
                                                {
                                                    orderDetails.ThisMonthPhysicalCount = null;

                                                }
                                            }
                                            break;
                                        case 11:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.AMC = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.AMC)))
                                                {
                                                    orderDetails.AMC = null;

                                                }

                                            }
                                            break;
                                        case 12:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.MonthsOfStock = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.MonthsOfStock)))
                                                {
                                                    orderDetails.MonthsOfStock = null;

                                                }
                                            }
                                            break;
                                        case 13:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.MaximumStockQuantity = parsenum;
                                                }
                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.MaximumStockQuantity)))
                                                {
                                                    orderDetails.MaximumStockQuantity = null;

                                                }
                                            }
                                            break;

                                        case 14:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                double parsenum;
                                                if (double.TryParse(coltext, out parsenum))
                                                {
                                                    orderDetails.QuantityRequired = parsenum;
                                                }

                                                if (Double.IsNaN(System.Convert.ToDouble(orderDetails.QuantityRequired)))
                                                {
                                                    orderDetails.QuantityRequired = null;

                                                }

                                            }
                                            break;
                                        case 15:
                                            if (row >= 10 && row <= 64)
                                            {
                                                string coltext = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                                orderDetails.Notes = worksheet.Range[row, column].FormulaNumberValue.ToString();
                                            }
                                            break;
                                    }
                                }

                                var objorderDetails = context.Order_OI_STI_Detail.FirstOrDefault(o => o.FacilityCode == orderDetails.FacilityCode &&
                                 o.StartDate == orderDetails.StartDate && o.EndDate == orderDetails.EndDate && o.ProductCode == orderDetails.ProductCode);
                                if (objorderDetails != null)
                                {
                                    //var order_no = TempData.Peek("OrderNumber").ToString();
                                    //context.Entry(objorderDetails).CurrentValues.SetValues(orderDetails);
                                    objorderDetails.ProductCode = orderDetails.ProductCode;
                                    objorderDetails.PhysicalCountAtBeginningOfReviewPeriod = orderDetails.PhysicalCountAtBeginningOfReviewPeriod;
                                    objorderDetails.QuantityReceivedDuringTheTwoMonthsCycle = orderDetails.QuantityReceivedDuringTheTwoMonthsCycle;
                                    objorderDetails.QuantityUsedDuringTwoMonths = orderDetails.QuantityUsedDuringTwoMonths;
                                    objorderDetails.LossesAndAdjustments = orderDetails.LossesAndAdjustments;
                                    objorderDetails.NumberOfDaysOutOfStock = orderDetails.NumberOfDaysOutOfStock;
                                    objorderDetails.ThisMonthPhysicalCount = orderDetails.ThisMonthPhysicalCount;
                                    objorderDetails.AMC = orderDetails.AMC;
                                    objorderDetails.MonthsOfStock = orderDetails.MonthsOfStock;
                                    objorderDetails.MaximumStockQuantity = orderDetails.MaximumStockQuantity;
                                    objorderDetails.QuantityRequired = orderDetails.QuantityRequired;
                                    objorderDetails.Notes = orderDetails.Notes;
                                    objorderDetails.Quantity_Allocated = orderDetails.Quantity_Allocated;
                                    objorderDetails.RFSONotes = orderDetails.RFSONotes;

                                    context.Entry(objorderDetails).State = EntityState.Modified;
                                }
                                else
                                {
                                    context.Order_OI_STI_Detail.Add(orderDetails);
                                }
                                context.SaveChanges();
                            }

                        }

                    }
                }
                // }
                #endregion "ARV and E-MTCT Medicines Order Form and Patient Report"  
                workbook.Close();
                excelEngine.Dispose();
                // return list;
            }
            //catch (Exception ex)
            //{
            //    Console.Write(ex.Message);
            //}
            
        }

        //Rename the file after use
        private static void renametheFile(string file)
        {
            var dateValue = DateTime.Now;
            var dateValuems = dateValue.ToString("dd.MM.yyyy hh.mm.ss.fff tt");

            string filePath = ConfigurationManager.AppSettings["watchDirectoryPath"];
            string originalfile = @file; 
            var extension = Path.GetExtension(file);
            var fileName2 = Path.GetFileNameWithoutExtension(file) + "_" + dateValuems + extension;

            string path2 = filePath+@"\" + fileName2;
            try
            {
                if (!File.Exists(originalfile))
                {
                    // This statement ensures that the file is created,
                    // but the handle is not kept.
                    using (FileStream fs = File.Create(originalfile)) { }
                }

                // Ensure that the target does not exist.
                if (File.Exists(path2))
                    File.Delete(path2);

                // Move the file.
                File.Move(originalfile, path2);
                Console.WriteLine("{0} was moved to {1}.", originalfile, path2);

                // See if the original exists now.
                if (File.Exists(originalfile))
                {
                    Console.WriteLine("The original file still exists, which is unexpected.");
                }
                else
                {
                    Console.WriteLine("The original file no longer exists, which is expected.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        public static bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn = false;
            if (string.IsNullOrEmpty(strFullFileName))
            {
                return blnReturn;
            }
            System.IO.FileStream fs;
            try
            {
                
                fs = System.IO.File.Open(strFullFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
                fs.Close();
            }
            catch (System.IO.IOException ex)
            {
                blnReturn = true;
            }
            return blnReturn;
        }

    }

}