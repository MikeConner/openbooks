-- Database Change Log

--NOTE: add the following comments to the relevant sprocs
    --exec SearchExpendituresSQL @pageIndex=0,@maximumRows=100,@sortColumn='ExpenditureID',@sortDirection='DESC',@candidateID=NULL,@office=NULL,@vendorKeywords=NULL,@vendorSearchOptions=NULL,@keywords=NULL
    --exec SearchLobbyistsCompanies 4, 6
    --SELECT * FROM dbo.StrToTable('28394,28395') 


USE [CityController]
GO
/****** Object:  StoredProcedure [dbo].[SearchLobbyistsCompanies]    Script Date: 5/5/2014 12:00:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:                Zeo
-- Create date: 4/29/10
-- Description:        
-- =============================================
CREATE PROCEDURE [dbo].[SearchLobbyistsCompanies]
@LobbyistIDs varchar(max)
AS
BEGIN
--exec SearchLobbyistsCompanies '4, 6'
--select top 10 * from lobbyists
SET NOCOUNT ON;
 
-- Get Lobbyist Companies
SELECT 
	xref.LobbyistID, 
	xref.CompanyID,
	lc.FullName AS CompanyName, 
	lc.Address, 
	lc.City, 
	lc.State, 
	lc.Zip
FROM xref_lobbyist_company xref 
RIGHT JOIN lobbyistCompanies lc ON xref.CompanyID = lc.ID 
WHERE xref.LobbyistID IN (SELECT * FROM StrToTable(@LobbyistIDs))
END
GO


/****** Object:  UserDefinedFunction [dbo].[StrToTable]    Script Date: 7/8/2014 4:19:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[StrToTable](@input AS VARCHAR(MAX) )
RETURNS
      @Result TABLE(Value VARCHAR(MAX))
AS
--SELECT * FROM dbo.StrToTable('28394,28395') 
--SELECT * FROM dbo.StrToTable('') 
BEGIN
      DECLARE @str VARCHAR(MAX)
      DECLARE @ind Int
      IF(@input is not null AND @input <> '')
      BEGIN
            SET @ind = CharIndex(',',@input)
            WHILE @ind > 0
            BEGIN
                  SET @str = SUBSTRING(@input,1,@ind-1)
                  SET @input = SUBSTRING(@input,@ind+1,LEN(@input)-@ind)
                  INSERT INTO @Result values (@str)
                  SET @ind = CharIndex(',',@input)
            END
            SET @str = @input
            INSERT INTO @Result values (@str)
      END
      RETURN
END
GO

USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SearchRangeContractsCount]    Script Date: 7/31/2014 4:52:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Contracts
-- =============================================
CREATE PROCEDURE [dbo].[SearchRangeContractsCount]
	@contractID INT = NULL,
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

SELECT @sql = 'SELECT COUNT(ContractOrder) 
FROM
(
	SELECT ROW_NUMBER() OVER(ORDER BY ContractID ASC, DateEntered DESC) AS ContractOrder, 
		ContractID, VendorNo, DepartmentID, Amount, OriginalAmount, 
		Description, DateSolicitor, DateDuration, DateCountersigned, DateEntered, 
		VendorName,
		ServiceName,
		DepartmentName
 	FROM 
	(
		SELECT c.ContractID, c.VendorNo, c.DepartmentID, 
			c.Service, c.Amount, c.OriginalAmount, c.Description, 
			c.DateSolicitor, c.DateDuration, 
			c.DateCountersigned, c.DateEntered,
			v.VendorName, 
			s.ServiceName, 
			d.DeptName AS DepartmentName 
		FROM contracts c 
		LEFT OUTER JOIN vendors v ON c.VendorNo = v.VendorNo 
		JOIN tlk_service s ON c.Service = s.ID 
		LEFT OUTER JOIN tlk_department d ON c.DepartmentID = d.DeptCode
	) AS rows ';

SELECT @sql = @sql + ' WHERE 1 = 1 ';

		/* Contract # Only Search */
		IF @contractID IS NOT NULL 
			SELECT @sql = @sql + ' AND ContractID = @xcontractID ';

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
		IF @minContractAmt IS NOT NULL AND @maxContractAmt IS NOT NULL
		    SELECT @sql = @sql + ' AND Amount >= @xminContractAmt AND AMOUNT <= @xmaxContractAmt ';

SELECT @sql = @sql + ' ) AS results ';


IF @debug = 0
   PRINT @sql

SELECT @paramlist = '
	@xcityDept INT,
	@xcontractID INT,
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

GO



USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SearchContractsRange]    Script Date: 7/31/2014 4:51:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Contracts
-- =============================================
CREATE PROCEDURE [dbo].[SearchContractsRange]
	@pageIndex INT, 
	@maximumRows INT,
	@sortColumn VARCHAR(25) = 'contractID',
	@sortDirection CHAR(4) = 'DESC',
	--@sortColumn VARCHAR(25),
	--@sortDirection CHAR(4),
	@contractID INT = NULL,
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

DECLARE @startRowIndex INT;
SET @startRowIndex = (@pageIndex * @maximumRows);

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = 'SELECT * 
FROM
(
	SELECT ';
	SELECT @sql = @sql + 'ROW_NUMBER() OVER(ORDER BY ' + @sortColumn + ' ' + @sortDirection + ' ,DateEntered DESC) AS ContractOrder, ';
	SELECT @sql = @sql + ' * 
 	FROM 
	(
		SELECT c.ContractID, c.SupplementalNo, c.VendorNo, c.DepartmentID, 
			c.Service, c.Amount, c.OriginalAmount, c.Description, 
			c.DateSolicitor, c.DateDuration, 
			c.DateCountersigned, c.DateEntered,
			v.VendorName, 
			s.ServiceName, 
			d.DeptName AS DepartmentName,
			CASE
				WHEN oc.ContractID IS NULL THEN ''False''
				ELSE ''True''
			END AS ''HasPdf'' 
		FROM contracts c 
		LEFT OUTER JOIN vendors v ON c.VendorNo = v.VendorNo 
		JOIN tlk_service s ON c.Service = s.ID 
		LEFT OUTER JOIN tlk_department d ON c.DepartmentID = d.DeptCode
		LEFT OUTER JOIN tblOnbaseContracts oc ON c.ContractID = oc.ContractID
	) AS rows ';

SELECT @sql = @sql + ' WHERE 1 = 1 ';

		/* Contract # Only Search */
		IF @contractID IS NOT NULL 
			SELECT @sql = @sql + ' AND ContractID = @xcontractID ';

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
		IF @minContractAmt IS NOT NULL AND @maxContractAmt IS NOT NULL
		    SELECT @sql = @sql + ' AND Amount >= @xminContractAmt AND Amount <= @xmaxContractAmt ';

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
	@xcontractID INT,
	@xvendorID INT,
	@xvendorKeywords VARCHAR(100),
	@xcontractType INT,
	@xkeywords VARCHAR(100),
	@xbeginDate DATETIME, 
	@xendDate DATETIME,
	@xminContractAmt INT,
	@xmaxContractAmt INT';


EXEC sp_executesql @sql, @paramlist, 
	@startRowIndex, @maximumRows, 
	@cityDept, @contractID, @vendorID, @vendorKeywords, @contractType, @keywords, @beginDate, @endDate, @minContractAmt, @maxContractAmt

END


GO
