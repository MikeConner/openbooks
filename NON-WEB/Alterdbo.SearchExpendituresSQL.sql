USE [CityController_PROD]
GO

/****** Object: SqlProcedure [dbo].[SearchExpendituresSQL] Script Date: 3/22/2018 11:29:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Expenditures
-- =============================================
ALTER PROCEDURE [dbo].[SearchExpendituresSQL]
	@pageIndex INT, 
	@maximumRows INT,
	@sortColumn VARCHAR(25),
	@sortDirection CHAR(4),
	@candidateID INT,
	@office NVARCHAR(50) = NULL,
	@datePaid INT = 0, 
	@vendorKeywords VARCHAR(100) = NULL,
	@vendorSearchOptions CHAR(1) = NULL,
	@keywords VARCHAR(100) = NULL,
	@approved BIT = 1,
	@debug	BIT = 0

AS

BEGIN

DECLARE @startRowIndex INT;
SET @startRowIndex = (@pageIndex * @maximumRows);

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = 'SELECT 
		ExpenditureOrder, 
		ExpenditureID, CandidateID, Office, CompanyName, 
		Address1, City, State, Zip, Province, Country, Description, Amount, DatePaid, DateEntered, CreatedBy, Approved,
		CandidateName 
FROM
(
	SELECT ';
	SELECT @sql = @sql + 'ROW_NUMBER() OVER(ORDER BY ' + @sortColumn + ' ' + @sortDirection + ' ,DateEntered DESC) AS ExpenditureOrder, ';
	SELECT @sql = @sql + ' ExpenditureID, CandidateID, Office, CompanyName, 
							Address1, City, State, Zip, Country, Province, Description, Amount, DatePaid, DateEntered, CreatedBy, Approved,
							CandidateName 
 	FROM 
	(
		SELECT e.ExpenditureID, e.CandidateID, e.Office, e.CompanyName, 
			e.Address1, e.City, e.State, e.Zip, e.Country, e.Province, e.Description, e.Amount, e.DatePaid, e.DateEntered, e.CreatedBy, e.Approved,
			tc.CandidateName 
		FROM expenditures e 
		JOIN tlk_candidate tc ON e.CandidateID = tc.ID 
		WHERE e.Approved = @xapproved
	) AS rows ';

SELECT @sql = @sql + ' WHERE 1 = 1 ';
		

		/* Vendor Search */
		IF @vendorKeywords IS NOT NULL AND @vendorSearchOptions IS NOT NULL
		BEGIN
			IF @vendorSearchOptions = 'B'
				SELECT @sql = @sql + ' AND CompanyName LIKE @xvendorKeywords + ''%'' ';
			IF @vendorSearchOptions = 'E'
				SELECT @sql = @sql + ' AND CompanyName = @xvendorKeywords ';	
			IF @vendorSearchOptions = 'C'
				SELECT @sql = @sql + ' AND CompanyName LIKE ''%'' + @xvendorKeywords + ''%'' ';
		END

		/* Keywords Search */
		IF @keywords IS NOT NULL
			SELECT @sql = @sql + ' AND Description LIKE ''%'' + @xkeywords + ''%'' ';

		/* Office Held*/
		IF @office IS NOT NULL
			SELECT @sql = @sql + ' AND Office = @xoffice ';

		/* Year Paid Search */
		IF @datePaid <> 0
			SELECT @sql = @sql + ' AND YEAR(DatePaid) = @xdatePaid ';

		/* Candidate ID */
		IF @candidateID IS NOT NULL 
			SELECT @sql = @sql + ' AND CandidateID = @xcandidateID ';

SELECT @sql = @sql + ' ) AS results ';




/* Paging Filter */
SELECT @sql = @sql + ' WHERE results.ExpenditureOrder > @xstartRowIndex AND results.ExpenditureOrder <= (@xstartRowIndex + @xmaximumRows)';

/* ORDER BY */
SELECT @sql = @sql + ' ORDER BY ExpenditureOrder ASC ';

IF @debug = 1
   PRINT @sql

SELECT @paramlist = '@xstartRowIndex INT, 
	@xmaximumRows INT,
	@xcandidateID INT,
	@xoffice NVARCHAR(50),
	@xdatePaid INT,
	@xvendorKeywords VARCHAR(100),
	@xvendorSearchOptions CHAR(1),
	@xkeywords VARCHAR(100),
	@xapproved BIT';

EXEC sp_executesql @sql, @paramlist, 
	@startRowIndex, @maximumRows, 
	@candidateID, @office, @datePaid, @vendorKeywords, @vendorSearchOptions, @keywords, @approved
END
