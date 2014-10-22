USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SearchContributionsSQL]    Script Date: 10/22/2014 6:16:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO










-- =============================================
-- Author:		Zeo
-- Create date: 07/08/09
-- Description:	Search Contributions
-- =============================================
ALTER PROCEDURE [dbo].[SearchContributionsSQL]
	@pageIndex INT, 
	@maximumRows INT,
	@sortColumn VARCHAR(25) = 'contributionID',
	@sortDirection CHAR(4) = 'DESC',
	@candidateID INT,
	@office NVARCHAR(50) = NULL,
	@dateContribution INT = 0, 
	@contributorKeywords NVARCHAR(100) = NULL,
	@contributorSearchOptions CHAR(2) = NULL,
	@employerKeywords NVARCHAR(100) = NULL,
	@approved BIT = 1,
    @searchZip BIT = 0,
    @boundsSouth DECIMAL(9,6) = NULL, 
    @boundsNorth DECIMAL(9,6) = NULL,
    @boundsWest DECIMAL(9,6) = NULL,
    @boundsEast DECIMAL(9,6) = NULL,
	@debug	BIT = 0

AS

BEGIN

DECLARE @startRowIndex INT;
SET @startRowIndex = (@pageIndex * @maximumRows);

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = 'SELECT 
		ContributionOrder, 
		ContributionID, CandidateID, Office, ContributorType, ContributorName, 
		CASE ContributionType 
			WHEN 0 THEN NULL 
			WHEN 1 THEN ''Standard'' 
			WHEN 2 THEN ''In-Kind'' 
			WHEN 3 THEN ''Loan'' 
			WHEN 4 THEN ''Refund'' 
			WHEN 5 THEN ''Interest'' 
			ELSE NULL 
		END AS ctype,
		Description,
		City, State, Zip, Employer, Occupation, Amount, DateContribution, DateEntered, CreatedBy, Approved,
		CandidateName';
	IF @searchZip = 1
		--SET @sortColumn = 'Zip';
		SELECT @sql = @sql + ', Latitude, Longitude'; 
		
SELECT @sql = @sql + 
' FROM
(
	SELECT 
		ROW_NUMBER() OVER(ORDER BY ' + @sortColumn + ' ' + @sortDirection + ' ,DateEntered DESC) AS ContributionOrder, 
		ContributionID, CandidateID, Office, ContributorType, ContributorName, ContributionType, Description,
		City, State, Zip, Employer, Occupation, Amount, DateContribution, DateEntered, CreatedBy, Approved,
		CandidateName'; 
    IF @searchZip = 1
        SELECT @sql = @sql + ', z.Latitude, z.Longitude';

SELECT @sql = @sql +  
	' FROM 
	(
		SELECT c.ContributionID, c.CandidateID, c.Office, c.ContributorType, c.ContributorName, c.ContributionType, c.Description,
		c.City, c.State, c.Zip, c.Employer, c.Occupation, c.Amount, c.DateContribution, c.DateEntered, c.CreatedBy, c.Approved,
		tc.CandidateName 
		FROM contributions c 

		JOIN tlk_candidate tc ON c.CandidateID = tc.ID
		WHERE c.Approved = @xapproved
	) AS rows ';

IF @searchZip = 1
    SELECT @sql = @sql + ' JOIN zipcodes AS z ON Zip = z.ZIPCode ';

SELECT @sql = @sql + ' WHERE 1 = 1 ';

		/* Contributor Search */
		IF @contributorKeywords IS NOT NULL AND @contributorSearchOptions IS NOT NULL
		BEGIN
			IF @contributorSearchOptions = 'co'
				SELECT @sql = @sql + ' AND ContributorName LIKE ''%'' + @xcontributorKeywords + ''%'' AND ContributorType =''co'' ';
			IF @contributorSearchOptions = 'in'
				SELECT @sql = @sql + ' AND ContributorName LIKE ''%'' + @xcontributorKeywords + ''%'' AND ContributorType =''in'' ';
		END

		/* Employer Search */
		IF @employerKeywords IS NOT NULL 
			SELECT @sql = @sql + ' AND Employer LIKE ''%'' + @xemployerKeywords + ''%''';
		
		/* Office Held*/
		IF @office IS NOT NULL
			SELECT @sql = @sql + ' AND Office = @xoffice ';
			
		/* Year Contribution Search */
		IF @dateContribution <> 0
			SELECT @sql = @sql + ' AND YEAR(DateContribution) = @xdateContribution ';

		/* Candidate ID */
		IF @candidateID IS NOT NULL 
			SELECT @sql = @sql + ' AND CandidateID = @xcandidateID ';

        /* Zip Code Distance */
        IF @searchZip = 1
            SELECT @sql = @sql + ' AND z.Latitude BETWEEN @xboundsSouth AND @xboundsNorth ' +
                                ' AND z.Longitude BETWEEN @xboundsWest AND @xboundsEast ';

SELECT @sql = @sql + ' ) AS results ';

/* Paging Filter */
SELECT @sql = @sql + ' WHERE results.ContributionOrder > @xstartRowIndex AND results.ContributionOrder <= (@xstartRowIndex + @xmaximumRows)';

/* ORDER BY */
SELECT @sql = @sql + ' ORDER BY ContributionOrder ASC ';

IF @debug = 0
   PRINT @sql

SELECT @paramlist = '@xstartRowIndex INT, 
	@xmaximumRows INT,
	@xcandidateID INT,
	@xoffice NVARCHAR(50),
	@xdateContribution INT,
	@xcontributorKeywords NVARCHAR(100),
	@xcontributorSearchOptions CHAR(2),
	@xemployerKeywords NVARCHAR(100),
	@xapproved BIT,
    @xboundsSouth DECIMAL(9,6), 
    @xboundsNorth DECIMAL(9,6), 
    @xboundsWest DECIMAL(9,6),
    @xboundsEast DECIMAL(9,6)';

EXEC sp_executesql @sql, @paramlist, 
	@startRowIndex, @maximumRows, 
	@candidateID, @office, @dateContribution, @contributorKeywords, @contributorSearchOptions, @employerKeywords, @approved,
    @boundsSouth, @boundsNorth, @boundsWest, @boundsEast

END







GO

