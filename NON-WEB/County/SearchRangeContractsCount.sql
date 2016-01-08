﻿









-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Contracts
-- =============================================
CREATE PROCEDURE [dbo].[SearchRangeContractsCount]
	@contractID NVARCHAR(50) = NULL,
	@vendorID INT = NULL,
	@vendorKeywords VARCHAR(100) = NULL,
	@vendorSearchOptions CHAR(1) = NULL,
	@cityDept INT = NULL,
	@contractType INT = NULL,
	@keywords VARCHAR(100) = NULL,
	@beginDate DATETIME = NULL, 
	@endDate DATETIME = NULL, 
	@minContractAmt INT = NULL,
	@maxContractAmt INT = NULL,
	@debug	BIT = 0

AS

BEGIN

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);
DECLARE @Description varchar(1000)

SELECT @sql = 'SELECT COUNT(ContractOrder) 
FROM
(
SELECT ROW_NUMBER() OVER(ORDER BY ContractID ASC, OrderDate DESC) AS ContractOrder
FROM (
SELECT contractID, VendorNo, VendorName, DeptCode, DeptName, Amount, OrderDate, CancelDate, OrderType, OrderTypeID,
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
) AS DescriptionMunge
) AS unnamed';

SELECT @sql = @sql + ' WHERE 1 = 1 ';

		/* Contract # Only Search */
		IF @contractID IS NOT NULL 
			SELECT @sql = @sql + ' AND ContractID LIKE ''%'' + @xcontractID + ''%'' ';

		/* Vendor Only Search */
		IF @vendorID IS NOT NULL 
			SELECT @sql = @sql + ' AND VendorNo = @xvendorID ';

		/* Vendor Search */
		IF @vendorKeywords IS NOT NULL AND @vendorSearchOptions IS NOT NULL
		BEGIN
			IF @vendorSearchOptions = 'B'
				SELECT @sql = @sql + ' AND VendorName LIKE @xvendorKeywords + ''%'' ';
			IF @vendorSearchOptions = 'E'
				SELECT @sql = @sql + ' AND VendorName = @xvendorKeywords ';	
			IF @vendorSearchOptions = 'C'
				SELECT @sql = @sql + ' AND VendorName LIKE ''%'' + @xvendorKeywords + ''%'' ';
		END

		/* Dept */
		IF @cityDept IS NOT NULL
			SELECT @sql = @sql + ' AND DeptCode = @xcityDept';

		/* Contract Type */
		IF @contractType IS NOT NULL
			SELECT @sql = @sql + ' AND OrderTypeID = @xcontractType ';

		/* Keywords Search */
		IF @keywords IS NOT NULL
			SELECT @sql = @sql + ' AND AggregateDescription LIKE ''%'' + @xkeywords + ''%'' ';

		/* Date Filter */
		IF @beginDate IS NOT NULL AND @endDate IS NOT NULL
			SELECT @sql = @sql + ' AND OrderDate BETWEEN @xbeginDate AND @xendDate ';

		/* Amount */
		IF @minContractAmt IS NOT NULL AND @maxContractAmt IS NOT NULL
		    SELECT @sql = @sql + ' AND Amount >= @xminContractAmt AND AMOUNT <= @xmaxContractAmt ';

SELECT @sql = @sql + ' ) AS results ';


IF @debug = 0
   PRINT @sql

SELECT @paramlist = '
	@xcityDept INT,
	@xcontractID NVARCHAR(50),
	@xvendorID INT,
	@xvendorKeywords VARCHAR(100),
	@xcontractType INT,
	@xkeywords VARCHAR(100),
	@xbeginDate DATETIME, 
	@xendDate DATETIME,
	@xminContractAmt INT,
	@xmaxContractAmt INT';


EXEC sp_executesql @sql, @paramlist, 
	@cityDept, @contractID, @vendorID, @vendorKeywords, @contractType, @keywords, @beginDate, @endDate, @minContractAmt, @maxContractAmt

END