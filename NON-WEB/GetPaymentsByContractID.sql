USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetPaymentsByContractID]    Script Date: 1/6/2017 1:48:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zeo
-- Create date: 1/6/17
-- Description:	Get Sum of all payments by ContractID
-- =============================================
CREATE PROCEDURE [dbo].[GetPaymentsByContractID] 
	@ContractID NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

SELECT Fund, CostCenter, ObjectAccount, LineItem, SUM(AmountReceived) AS Total
  FROM payments p
  WHERE p.ContractID=@ContractID
  GROUP BY Fund, CostCenter, ObjectAccount, LineItem
  ORDER BY Total DESC
END

GO






-- =============================================
-- Author:		Zeo
-- Create date: 07/16/09
-- Description:	Get a Contract
-- =============================================
ALTER PROCEDURE [dbo].[GetContractByContractID] 
	@ContractID NVARCHAR(50),
	@SupplementalNo INT
AS
BEGIN
--select * from contracts where vendorno is null
--select * from vendors where vendorno = '0000254011'
--exec GetContractByContractID 48451, 0
	SET NOCOUNT ON;

	SELECT c.ContractID,
		c.VendorNo,
		vendors.VendorName,
	    sv.VendorNo AS SecondVendorNo,
		sv.VendorName AS SecondVendorName,
		c.DepartmentID,
		c.IndexCode,
		c.SupplementalNo,
		c.ResolutionNo,
		c.[Service],
		c.Amount,
		c.OriginalAmount,
		c.[Description],
		c.Notes,
		c.Comments,
		c.DateSolicitor,
		c.DateDuration,
		c.DateCountersigned,
		c.DateEntered,
		c.AmountReceived,
		tlk_service.ServiceName, 
		tlk_department.DeptName,
		CASE
			WHEN oc.ContractID IS NULL THEN 'False'
			ELSE 'True'
		END AS 'HasPdf'
	FROM contracts c  
	JOIN tlk_service ON c.Service = tlk_service.ID 
	INNER JOIN vendors ON c.VendorNo = vendors.VendorNo 
    LEFT OUTER JOIN vendors sv ON c.SecondVendorNo = sv.VendorNo 
	LEFT OUTER JOIN tlk_department ON c.DepartmentID = tlk_department.DeptCode 
	LEFT OUTER JOIN tblOnbaseContracts oc ON c.ContractID = oc.ContractID 
	WHERE c.ContractID= @ContractID AND c.SupplementalNo = @SupplementalNo

END



