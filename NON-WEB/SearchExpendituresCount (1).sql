USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SearchExpendituresCount]    Script Date: 9/18/2014 6:46:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Expenditures Count
-- =============================================
CREATE PROCEDURE [dbo].[SearchExpendituresCount]
	@candidateID INT,
	@office NVARCHAR(50) = NULL,
	@datePaid INT = 0, 
	@vendorKeywords VARCHAR(100) = NULL,
	@vendorSearchOptions CHAR(1) = NULL,
	@keywords VARCHAR(100) = NULL,
	@debug	BIT = 0

AS

BEGIN

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = 'SELECT COUNT(ExpenditureOrder) 
FROM
(
	SELECT ROW_NUMBER() OVER(ORDER BY ExpenditureID ASC, DateEntered DESC) AS ExpenditureOrder, 
		ExpenditureID, CandidateID, Office, CompanyName, 
		Address1, City, State, Zip, Description, Amount, DatePaid, DateEntered, 
		CandidateName 
 	FROM 
	(
		SELECT e.ExpenditureID, e.CandidateID, e.Office, e.CompanyName, 
			e.Address1, e.City, e.State, e.Zip, e.Description, e.Amount, e.DatePaid, e.DateEntered, 
			tc.CandidateName 
		FROM expenditures e 
		JOIN tlk_candidate tc ON e.CandidateID = tc.ID 

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

		/* Candidate ID */
		IF @candidateID IS NOT NULL 
			SELECT @sql = @sql + ' AND CandidateID = @xcandidateID ';


SELECT @sql = @sql + ' ) AS results ';


IF @debug = 0
   PRINT @sql

SELECT @paramlist = '
	@xcandidateID INT,
	@xoffice NVARCHAR(50),
	@xdatePaid INT, 
	@xvendorKeywords VARCHAR(100),
	@xvendorSearchOptions CHAR(1),
	@xkeywords VARCHAR(100)';


EXEC sp_executesql @sql, @paramlist, 
	@candidateID, @office, @datePaid, @vendorKeywords, @vendorSearchOptions, @keywords 



END

GO

