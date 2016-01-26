USE [AlleghenyCounty]
GO

/****** Object:  StoredProcedure [dbo].[GetContractByContractID]    Script Date: 1/26/2016 6:11:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Zeo
-- Create date: 07/16/09
-- Description:	Get a Contract
-- =============================================
ALTER PROCEDURE [dbo].[GetContractByContractID] 
	@ContractID NVARCHAR(50)
AS
BEGIN
--select * from contracts where vendorno is null
--select * from vendors where vendorno = '0000254011'
--exec GetContractByContractID 48451, 0
	--SET NOCOUNT ON;
SELECT contractID, VendorNo, VendorName, DeptCode, DeptName, Amount, OrderDate, CancelDate, OrderType,
       AggregateDescription
FROM
(
	SELECT DISTINCT 
	    c.ContractID,
		c.VendorNo,
		c.VendorName,
		c.DepartmentID AS DeptCode,
		c.Amount,
		c.OrderDate,
		c.CancelDate,
		ot.OrderType,
		d.DeptName,
            STUFF( (SELECT '','' + acc.Description
                             FROM accounts acc
			     WHERE acc.ContractID = c.contractID
                             FOR XML PATH('')), 
                            1, 1, '') AS AggregateDescription
	FROM contracts c  
    JOIN order_types ot ON c.ContractTypeID = ot.ID
    LEFT OUTER JOIN tlk_department d ON c.DepartmentID = d.DeptCode
	LEFT OUTER JOIN accounts acc ON acc.ContractID = c.contractID
	) AS DescriptionMunge
	WHERE contractID = @ContractID
END




GO

