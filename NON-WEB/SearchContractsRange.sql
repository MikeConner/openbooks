USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SearchContractsRange]    Script Date: 1/23/2017 6:17:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Contracts
-- =============================================
ALTER PROCEDURE [dbo].[SearchContractsRange]
	@pageIndex INT, 
	@maximumRows INT,
	@sortColumn VARCHAR(25) = 'contractID',
	@sortDirection CHAR(4) = 'DESC',
	--@sortColumn VARCHAR(25),
	--@sortDirection CHAR(4),
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
	@byPaidAmount BIT = 0,
	@debug	BIT = 0

AS

BEGIN

DECLARE @startRowIndex INT;
SET @startRowIndex = (@pageIndex * @maximumRows);

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000), @amountField NVARCHAR(100);

SELECT @sql = 'SELECT * 
FROM
(
	SELECT ';
	SELECT @sql = @sql + 'ROW_NUMBER() OVER(ORDER BY ' + @sortColumn + ' ' + @sortDirection + ' ,DateEntered DESC) AS ContractOrder, ';
	SELECT @sql = @sql + ' * 
 	FROM 
	(
		SELECT c.ContractID, c.SupplementalNo, c.VendorNo, c.DepartmentID, 
			c.Service, c.Amount, c.OriginalAmount, c.AmountReceived, c.Description, 
			c.DateSolicitor, c.DateDuration, 
			c.DateCountersigned, c.DateEntered,
			v.VendorName, 
			sv.VendorName AS SecondVendorName,
			s.ServiceName, 
			d.DeptName AS DepartmentName,
			CASE
				WHEN oc.ContractID IS NULL THEN ''False''
				ELSE ''True''
			END AS ''HasPdf'',
			CASE
				WHEN c.HasCheckPDF IS NULL THEN ''False''
				ELSE ''True''
			END AS ''HasCheck'',
			CASE
				WHEN c.HasInvoicePDF IS NULL THEN ''False''
				ELSE ''True''
			END AS ''HasInvoice'' 			 
		FROM contracts c 
		LEFT OUTER JOIN vendors v ON c.VendorNo = v.VendorNo 
		LEFT OUTER JOIN vendors sv ON c.SecondVendorNo = sv.VendorNo 
		JOIN tlk_service s ON c.Service = s.ID 
		LEFT OUTER JOIN tlk_department d ON c.DepartmentID = d.DeptCode
		LEFT OUTER JOIN tblOnbaseContracts oc ON c.ContractID = oc.ContractID
	) AS rows ';

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
				SELECT @sql = @sql + ' AND ((VendorName LIKE @xvendorKeywords + ''%'') OR (SecondVendorName LIKE @xvendorKeywords + ''%'')) ';
			IF @vendorSearchOptions = 'E'
				SELECT @sql = @sql + ' AND ((VendorName = @xvendorKeywords) OR (SecondVendorName = @xvendorKeywords)) ';	
			IF @vendorSearchOptions = 'C'
				SELECT @sql = @sql + ' AND ((VendorName LIKE ''%'' + @xvendorKeywords + ''%'') OR (SecondVendorName LIKE ''%'' + @xvendorKeywords + ''%'')) ';
		END

		/* Dept */
		IF @cityDept IS NOT NULL
			SELECT @sql = @sql + ' AND DepartmentID = @xcityDept';

		/* Contract Type */
		IF @contractType IS NOT NULL
			SELECT @sql = @sql + ' AND Service = @xcontractType ';

		/* Keywords Search */
		IF @keywords IS NOT NULL
			SELECT @sql = @sql + ' AND Description LIKE ''%'' + @xkeywords + ''%'' ';

		/* Date Filter */
		IF @beginDate IS NOT NULL AND @endDate IS NOT NULL
			SELECT @sql = @sql + ' AND DateEntered BETWEEN @xbeginDate AND @xendDate ';

		/* Amount */
		IF (@byPaidAmount = 1) 
		  SET @amountField = 'AmountReceived';
	    ELSE
		  SET @amountField = 'Amount';

		IF @minContractAmt IS NOT NULL AND @maxContractAmt IS NOT NULL
			SELECT @sql = @sql + ' AND Amount >= @xminContractAmt AND ' + @amountField + ' <= @xmaxContractAmt ';
SELECT @sql = @sql + ' ) AS results ';

/* Paging Filter */
SELECT @sql = @sql + ' WHERE results.ContractOrder > @xstartRowIndex AND results.ContractOrder <= (@xstartRowIndex + @xmaximumRows)';

/* ORDER BY */
SELECT @sql = @sql + ' ORDER BY ContractOrder ASC ';

IF @debug = 0
   PRINT @sql

SELECT @paramlist = '@xstartRowIndex INT, 
	@xmaximumRows INT,
	@xcityDept INT, 
	@xcontractID NVARCHAR(50),
	@xvendorID INT,
	@xvendorKeywords VARCHAR(100),
	@xcontractType INT,
	@xkeywords VARCHAR(100),
	@xbeginDate DATETIME, 
	@xendDate DATETIME,
	@xminContractAmt INT,
	@xmaxContractAmt INT,
	@xbyPaidAmount BIT';


EXEC sp_executesql @sql, @paramlist, 
	@startRowIndex, @maximumRows, 
	@cityDept, @contractID, @vendorID, @vendorKeywords, @contractType, @keywords, @beginDate, @endDate, @minContractAmt, @maxContractAmt, @byPaidAmount

END








GO


