/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4224)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [MEDICALACCESS]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Supplier_To_Facility_Store'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Supplier_To_Facility_Store'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Supplier_To_Facility_Store'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Facility_Store_To_DispensingUnit'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Facility_Store_To_DispensingUnit'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Facility_Store_To_DispensingUnit'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Stock_Management'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Stock_Management'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Stock_Management'
GO
/****** Object:  View [dbo].[Chss_M_Traceability_Supplier_To_Facility_Store]    Script Date: 23-Feb-19 4:49:02 PM ******/
DROP VIEW [dbo].[Chss_M_Traceability_Supplier_To_Facility_Store]
GO
/****** Object:  View [dbo].[Chss_M_Traceability_Facility_Store_To_DispensingUnit]    Script Date: 23-Feb-19 4:49:02 PM ******/
DROP VIEW [dbo].[Chss_M_Traceability_Facility_Store_To_DispensingUnit]
GO
/****** Object:  View [dbo].[Chss_M_Stock_Management]    Script Date: 23-Feb-19 4:49:02 PM ******/
DROP VIEW [dbo].[Chss_M_Stock_Management]
GO
/****** Object:  View [dbo].[Chss_M_Stock_Management]    Script Date: 23-Feb-19 4:49:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Chss_M_Stock_Management]
AS
SELECT        dbo.Chss_Stock_Management.facility_code, A_Facilities_1.SAP_Code, A_Facilities_1.DistrrictCode, dbo.A_District.District_Name, A_Facilities_1.ImplimentingPartnerCode,
                             (SELECT        TOP (1) ImplementingPartnerDescription
                               FROM            dbo.A_ImplimentingPartners
                               WHERE        (ImplimentingPartnerCode = A_Facilities_1.ImplimentingPartnerCode)) AS ImplimentingPartnerDescription, A_Facilities_1.Facility, A_Facilities_1.level_of_care,
                             (SELECT        TOP (1) level_of_care
                               FROM            dbo.A_Facility_Level_Of_Care
                               WHERE        (level_of_care_code = A_Facilities_1.level_of_care)) AS Level_desc, A_Facilities_1.CDCRegionId,
                             (SELECT        TOP (1) CDCRegion
                               FROM            dbo.A_CDCRegion
                               WHERE        (CDCRegionId = A_Facilities_1.CDCRegionId)) AS CDCRegion, A_Facilities_1.ComprehensiveImplimentingPartnerCode,
                             (SELECT        TOP (1) ImplementingPartnerDescription
                               FROM            dbo.A_ImplimentingPartners AS A_ImplimentingPartners_1
                               WHERE        (ImplimentingPartnerCode = A_Facilities_1.ComprehensiveImplimentingPartnerCode)) AS ComprehensiveImplimentingPartnerDescription,
                             (SELECT        TOP (1) SCTO
                               FROM            dbo.fo_SCTO
                               WHERE        (SAP_Code = A_Facilities_1.SAP_Code)) AS SCTO, dbo.Chss_Stock_Management.date_of_visit, dbo.Chss_Stock_Management.product_code, A_Product_1.product_description, 
                         A_Product_1.packsize, CASE WHEN dbo.Chss_Stock_Management.Item_Available = 1 THEN 1 ELSE 0 END AS Item_Available, dbo.Chss_Stock_Management.Expired_Quantity, 
                         CASE WHEN dbo.Chss_Stock_Management.Card_Availability = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Stock_Management.Card_Availability = 1 THEN '1' ELSE '0' END END AS Card_Availability, 
                         CASE WHEN dbo.Chss_Stock_Management.Monthly_Physical_Count = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Stock_Management.Monthly_Physical_Count = 1 THEN '1' ELSE '0' END END AS Monthly_Physical_Count,
                          CASE WHEN dbo.Chss_Stock_Management.Correct_Card_Fill = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Stock_Management.Correct_Card_Fill = 1 THEN '1' ELSE '0' END END AS Correct_Card_Fil, 
                         CASE WHEN dbo.Chss_Stock_Management.Stock_Card_Balance = - 1 THEN 'NA' ELSE CONVERT(varchar(1000), dbo.Chss_Stock_Management.Stock_Card_Balance) END AS Stock_Card_Balance, 
                         dbo.Chss_Stock_Management.Store_Quantity, 
                         CASE WHEN dbo.Chss_Stock_Management.Card_Availability = 1 THEN CASE WHEN dbo.Chss_Stock_Management.Stock_Card_Balance = dbo.Chss_Stock_Management.Store_Quantity THEN '1' ELSE '0' END ELSE
                          'NA' END AS Balance_Comparison, 
                         CASE WHEN dbo.Chss_Stock_Management.Stock_Book_Availability = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Stock_Management.Stock_Book_Availability = 1 THEN '1' ELSE '0' END END AS Stock_Book_Availability,
                          CASE WHEN dbo.Chss_Stock_Management.Correct_Stock_Book_Use = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Stock_Management.Correct_Stock_Book_Use = 1 THEN '1' ELSE '0' END END AS Correct_Stock_Book_Use,
                          CASE WHEN dbo.Chss_Stock_Management.AMC = - 1 THEN 'NA' ELSE CASE WHEN dbo.Chss_Stock_Management.AMC = - 2 THEN 'NR' ELSE CONVERT(varchar(100), dbo.Chss_Stock_Management.AMC) 
                         END END AS AMC, dbo.Chss_Stock_Management.Quantity_Issued, dbo.Chss_Stock_Management.Out_Of_Stock_Days, dbo.Chss_Stock_Management.Pharmacy_Quantity, 
                         dbo.Chss_Stock_Management.Calculated_AMC, 
                         CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN 0 ELSE CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = dbo.Chss_Stock_Management.AMC THEN 1 ELSE 0 END END AS AMC_Comparison,
                          dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity AS Facility_Quantity, 
                         CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END AS Month_Of_Stock, 
                         CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END > 4 THEN 'Over' ELSE 'OK' END END AS Stock_Status, 
                         CASE WHEN (CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 4 THEN 'Over' ELSE 'OK' END END) 
                         = 'OK' THEN 'Not Applicable' ELSE (CASE WHEN (CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 4 THEN 'Over' ELSE 'OK' END END) 
                         = 'Under' THEN 'Borrow' ELSE (CASE WHEN CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END > 4 THEN 'Over' ELSE 'OK' END END = 'Over' AND 
                         CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 6 THEN 'Lend Out' ELSE 'Not Applicable' END) 
                         END) END AS Recommendation, 
                         CASE WHEN (CASE WHEN (CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 4 THEN 'Over' ELSE 'OK' END END) 
                         = 'OK' THEN 'Not Applicable' ELSE (CASE WHEN (CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 4 THEN 'Over' ELSE 'OK' END END) 
                         = 'Under' THEN 'Borrow' ELSE (CASE WHEN CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END > 4 THEN 'Over' ELSE 'OK' END END = 'Over' AND 
                         CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 6 THEN 'Lend Out' ELSE 'Not Applicable' END) 
                         END) END) = 'Lend Out' THEN (((CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END) - 6) 
                         * dbo.Chss_Stock_Management.Calculated_AMC) ELSE 0 END AS Quantity_ToLend, 
                         CASE WHEN (CASE WHEN (CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 4 THEN 'Over' ELSE 'OK' END END) 
                         = 'OK' THEN 'Not Applicable' ELSE (CASE WHEN (CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 4 THEN 'Over' ELSE 'OK' END END) 
                         = 'Under' THEN 'Borrow' ELSE (CASE WHEN CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END <= 2 THEN 'Under' ELSE CASE WHEN CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity)
                          / 1) ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) 
                         END > 4 THEN 'Over' ELSE 'OK' END END = 'Over' AND 
                         CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END > 6 THEN 'Lend Out' ELSE 'Not Applicable' END) 
                         END) END) = 'Borrow' THEN CASE WHEN (dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) = 0 THEN (dbo.Chss_Stock_Management.Calculated_AMC * 2) 
                         ELSE ((4 - (CASE WHEN dbo.Chss_Stock_Management.Calculated_AMC = 0 THEN ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / 1) 
                         ELSE ((dbo.Chss_Stock_Management.Store_Quantity + dbo.Chss_Stock_Management.Pharmacy_Quantity) / dbo.Chss_Stock_Management.Calculated_AMC) END)) 
                         * dbo.Chss_Stock_Management.Calculated_AMC) END ELSE 0 END AS Quantity_ToBorrow,
                             (SELECT        COUNT(*) AS ProductCount
                               FROM            dbo.A_Product) AS ProductCount,
                             (SELECT        COUNT(*) AS FacilityCount
                               FROM            dbo.A_Facilities
                               WHERE        (IsActive = 1)) AS FacilityCount
FROM            dbo.A_Facilities AS A_Facilities_1 INNER JOIN
                         dbo.A_District ON A_Facilities_1.DistrrictCode = dbo.A_District.District_Code INNER JOIN
                         dbo.Chss_Stock_Management ON A_Facilities_1.FacilityCode = dbo.Chss_Stock_Management.facility_code INNER JOIN
                         dbo.A_Product AS A_Product_1 ON dbo.Chss_Stock_Management.product_code = A_Product_1.product_code
GO
/****** Object:  View [dbo].[Chss_M_Traceability_Facility_Store_To_DispensingUnit]    Script Date: 23-Feb-19 4:49:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Chss_M_Traceability_Facility_Store_To_DispensingUnit]
AS
SELECT        dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.facility_code, dbo.A_Facilities.SAP_Code, dbo.A_Facilities.Facility, dbo.A_Facilities.DistrrictCode, dbo.A_District.District_Name, 
                         dbo.A_Facilities.ImplimentingPartnerCode,
                             (SELECT        ImplementingPartnerDescription
                               FROM            dbo.A_ImplimentingPartners
                               WHERE        (ImplimentingPartnerCode = dbo.A_Facilities.ImplimentingPartnerCode)) AS ImplimentingPartnerDescription, dbo.A_Facilities.level_of_care,
                             (SELECT        level_of_care
                               FROM            dbo.A_Facility_Level_Of_Care
                               WHERE        (level_of_care_code = dbo.A_Facilities.level_of_care)) AS Level_desc, dbo.A_Facilities.CDCRegionId,
                             (SELECT        CDCRegion
                               FROM            dbo.A_CDCRegion
                               WHERE        (CDCRegionId = dbo.A_Facilities.CDCRegionId)) AS CDCRegion, dbo.A_Facilities.ComprehensiveImplimentingPartnerCode,
                             (SELECT        ImplementingPartnerDescription
                               FROM            dbo.A_ImplimentingPartners AS A_ImplimentingPartners_1
                               WHERE        (ImplimentingPartnerCode = dbo.A_Facilities.ComprehensiveImplimentingPartnerCode)) AS ComprehensiveImplimentingPartnerDescription,
                             (SELECT        TOP (1) SCTO
                               FROM            dbo.fo_SCTO
                               WHERE        (SAP_Code = dbo.A_Facilities.SAP_Code)) AS SCTO, dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.date_of_visit, 
                         dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.product_code, dbo.A_Product.product_description, dbo.A_Product.packsize, 
                         CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Availability = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Availability = 1 THEN '1'
                          ELSE '0' END END AS Item_Availability, dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Issue_Date, 
                         CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Quantity_Issued = - 1 THEN 'NA' ELSE CONVERT(varchar(100), 
                         dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Quantity_Issued) END AS Quantity_Issued, dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Stock_Card_Issue_date, 
                         CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Stock_Card_Quantity_Issued = - 1 THEN 'NA' ELSE CONVERT(varchar(100), 
                         dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Stock_Card_Quantity_Issued) END AS Stock_Card_Quantity_Issued, 
                         CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Availability = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Issue_Date = dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Stock_Card_Issue_date
                          THEN '1' ELSE '0' END END AS Date_Comp, 
                         CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Availability = 2 THEN 'NA' ELSE CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Quantity_Issued = - 1 OR
                         dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Stock_Card_Quantity_Issued = - 1 THEN '0' ELSE CASE WHEN dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Quantity_Issued = dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.Stock_Card_Quantity_Issued
                          THEN '1' ELSE '0' END END END AS Quantity_Comp
FROM            dbo.A_Facilities INNER JOIN
                         dbo.A_District ON dbo.A_Facilities.DistrrictCode = dbo.A_District.District_Code INNER JOIN
                         dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit ON dbo.A_Facilities.FacilityCode = dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.facility_code INNER JOIN
                         dbo.A_Product ON dbo.Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit.product_code = dbo.A_Product.product_code
GO
/****** Object:  View [dbo].[Chss_M_Traceability_Supplier_To_Facility_Store]    Script Date: 23-Feb-19 4:49:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Chss_M_Traceability_Supplier_To_Facility_Store]
AS
SELECT        dbo.Chss_Traceability_Supplier_To_Facility_Store.facility_code, dbo.A_Facilities.SAP_Code, dbo.A_Facilities.Facility, dbo.A_Facilities.DistrrictCode, dbo.A_District.District_Name, 
                         dbo.A_Facilities.ImplimentingPartnerCode,
                             (SELECT        ImplementingPartnerDescription
                               FROM            dbo.A_ImplimentingPartners
                               WHERE        (ImplimentingPartnerCode = dbo.A_Facilities.ImplimentingPartnerCode)) AS ImplimentingPartnerDescription,
                             (SELECT        level_of_care
                               FROM            dbo.A_Facility_Level_Of_Care
                               WHERE        (level_of_care_code = dbo.A_Facilities.level_of_care)) AS Level_desc, dbo.A_Facilities.CDCRegionId,
                             (SELECT        CDCRegion
                               FROM            dbo.A_CDCRegion
                               WHERE        (CDCRegionId = dbo.A_Facilities.CDCRegionId)) AS CDCRegion, dbo.A_Facilities.ComprehensiveImplimentingPartnerCode,
                             (SELECT        ImplementingPartnerDescription
                               FROM            dbo.A_ImplimentingPartners AS A_ImplimentingPartners_1
                               WHERE        (ImplimentingPartnerCode = dbo.A_Facilities.ComprehensiveImplimentingPartnerCode)) AS ComprehensiveImplimentingPartnerDescription,
                             (SELECT        TOP (1) SCTO
                               FROM            dbo.fo_SCTO
                               WHERE        (SAP_Code = dbo.A_Facilities.SAP_Code)) AS SCTO, dbo.Chss_Traceability_Supplier_To_Facility_Store.date_of_visit, dbo.Chss_Traceability_Supplier_To_Facility_Store.product_code, 
                         dbo.A_Product.product_description, dbo.A_Product.packsize, CASE WHEN dbo.Chss_Traceability_Supplier_To_Facility_Store.Recent_Delivery_Note_Quantity = - 1 THEN 'NA' ELSE CONVERT(varchar(100), 
                         dbo.Chss_Traceability_Supplier_To_Facility_Store.Recent_Delivery_Note_Quantity) END AS Recent_Delivery_Note_Quantity, dbo.Chss_Traceability_Supplier_To_Facility_Store.Recent_Delivery_Note_Batch, 
                         CASE WHEN dbo.Chss_Traceability_Supplier_To_Facility_Store.Responding_Goods_Received_Note_Quantity = - 1 THEN 'NA' ELSE CONVERT(varchar(100), 
                         dbo.Chss_Traceability_Supplier_To_Facility_Store.Responding_Goods_Received_Note_Quantity) END AS Responding_Goods_Received_Note_Quantity, 
                         dbo.Chss_Traceability_Supplier_To_Facility_Store.Responding_Goods_Received_Batch, 
                         CASE WHEN dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Quantity = - 1 THEN 'NA' ELSE CONVERT(varchar(100), 
                         dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Quantity) END AS Received_Stock_Card_Quantity, dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Note_Batch, 
                         CASE WHEN dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Note_Batch = NULL OR
                         dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Note_Batch = '' THEN 0 ELSE CASE WHEN dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Note_Batch = dbo.Chss_Traceability_Supplier_To_Facility_Store.Recent_Delivery_Note_Batch
                          AND 
                         dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Note_Batch = dbo.Chss_Traceability_Supplier_To_Facility_Store.Responding_Goods_Received_Batch THEN 1 ELSE 0 END END AS Batch_Comp,
                          CASE WHEN dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Quantity = dbo.Chss_Traceability_Supplier_To_Facility_Store.Recent_Delivery_Note_Quantity AND 
                         dbo.Chss_Traceability_Supplier_To_Facility_Store.Received_Stock_Card_Quantity = dbo.Chss_Traceability_Supplier_To_Facility_Store.Responding_Goods_Received_Note_Quantity THEN 1 ELSE 0 END AS Quantity_Comp
FROM            dbo.A_Facilities INNER JOIN
                         dbo.A_District ON dbo.A_Facilities.DistrrictCode = dbo.A_District.District_Code INNER JOIN
                         dbo.Chss_Traceability_Supplier_To_Facility_Store ON dbo.A_Facilities.FacilityCode = dbo.Chss_Traceability_Supplier_To_Facility_Store.facility_code INNER JOIN
                         dbo.A_Product ON dbo.Chss_Traceability_Supplier_To_Facility_Store.product_code = dbo.A_Product.product_code
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -192
         Left = 0
      End
      Begin Tables = 
         Begin Table = "A_Facilities_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 348
            End
            DisplayFlags = 280
            TopColumn = 17
         End
         Begin Table = "A_District"
            Begin Extent = 
               Top = 6
               Left = 594
               Bottom = 136
               Right = 821
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Chss_Stock_Management"
            Begin Extent = 
               Top = 198
               Left = 38
               Bottom = 328
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "A_Product_1"
            Begin Extent = 
               Top = 138
               Left = 611
               Bottom = 268
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 43
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Stock_Management'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Stock_Management'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Stock_Management'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "A_Facilities"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 348
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "A_District"
            Begin Extent = 
               Top = 6
               Left = 594
               Bottom = 136
               Right = 821
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Chss_Traceability_Facility_Store_To_ART_Dispensing_Unit"
            Begin Extent = 
               Top = 138
               Left = 611
               Bottom = 268
               Right = 848
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "A_Product"
            Begin Extent = 
               Top = 198
               Left = 338
               Bottom = 328
               Right = 569
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 26
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
        ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Facility_Store_To_DispensingUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Facility_Store_To_DispensingUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Facility_Store_To_DispensingUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "A_Facilities"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 348
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "A_District"
            Begin Extent = 
               Top = 6
               Left = 594
               Bottom = 136
               Right = 821
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Chss_Traceability_Supplier_To_Facility_Store"
            Begin Extent = 
               Top = 138
               Left = 611
               Bottom = 268
               Right = 936
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "A_Product"
            Begin Extent = 
               Top = 198
               Left = 338
               Bottom = 328
               Right = 569
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 27
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 15' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Supplier_To_Facility_Store'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'00
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Supplier_To_Facility_Store'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Chss_M_Traceability_Supplier_To_Facility_Store'
GO
