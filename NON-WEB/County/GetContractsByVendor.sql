



-- Batch submitted through debugger: SQLQuery1.sql|9|0|C:\Users\JKB\AppData\Local\Temp\~vsCBEE.sql




-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Contracts
-- =============================================
CREATE PROCEDURE [dbo].[GetContractsByVendor]
	@vendorID INT = NULL

AS

BEGIN

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = 'SELECT contractID, VendorNo, VendorName, DeptCode, DeptName, Amount, OrderDate, CancelDate, OrderType, OrderTypeID,
       AggregateDescription
FROM
(
SELECT 
   DISTINCT c.contractID, c.VendorNo, d.DeptCode, d.DeptName, c.Amount, c.OrderDate, c.CancelDate, c.VendorName, ot.OrderType, ot.ID as OrderTypeID,
            STUFF( (SELECT '','' + acc.Description
                             FROM accounts acc
			     WHERE acc.ContractID = c.contractID
                             FOR XML PATH('''')), 
                            1, 1, '''') AS AggregateDescription
   FROM contracts c
   JOIN order_types ot ON c.ContractTypeID = ot.ID
   LEFT OUTER JOIN tlk_department d ON c.DepartmentID = d.DeptCode
   LEFT OUTER JOIN accounts acc ON acc.ContractID = c.contractID
) AS DescriptionMunge';

SELECT @sql = @sql + ' WHERE VendorNo = @xvendorID ';

SELECT @paramlist = '@xvendorID INT';

EXEC sp_executesql @sql, @paramlist, @vendorID

END