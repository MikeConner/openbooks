USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SearchLobbyistsSQL]    Script Date: 12/26/2014 12:47:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Zeo
-- Create date: 04/26/10
-- Description:	Search Lobbyists Count
-- =============================================
ALTER PROCEDURE [dbo].[SearchLobbyistsSQL]
	@pageIndex INT, 
	@maximumRows INT,
	@sortColumn VARCHAR(25) = 'lobbyistID',
	@sortDirection CHAR(4) = 'DESC',
	@lobbyistID INT = NULL,
	@lobbyistKeywords VARCHAR(100) = NULL,
	@companyKeywords VARCHAR(100) = NULL,
	@debug	BIT = 0

AS

BEGIN

DECLARE @startRowIndex INT;
SET @startRowIndex = (@pageIndex * @maximumRows);

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = '
SELECT LobbyistOrder, LobbyistID, LobbyistName, Position, DateEntered, 
	EmployerName, Address, City, State, Zip, LobbyistStatus 
FROM
(
	SELECT ';
	SELECT @sql = @sql + 'ROW_NUMBER() OVER(ORDER BY ' + @sortColumn + ' ' + @sortDirection + ' ,DateEntered DESC) AS LobbyistOrder, ';
	SELECT @sql = @sql + ' 
			LobbyistID, LobbyistName, Position, DateEntered, 
			EmployerName, Address, City, State, Zip, LobbyistStatus
 	FROM 
	(
		SELECT DISTINCT LobbyistID, LobbyistName, Position, DateEntered, 
			EmployerName, Address, City, State, Zip, LobbyistStatus
 		FROM 
		(
			SELECT l.ID AS LobbyistID, l.FullName AS LobbyistName, l.Position,  l.DateEntered, 
				lc.FullName AS EmployerName, lc.Address, lc.City, lc.State, lc.Zip, l.LobbyistStatus
			FROM lobbyists l 
			LEFT JOIN lobbyistCompanies lc ON l.Employer = lc.ID 
			RIGHT JOIN xref_lobbyist_company xref ON l.ID = xref.LobbyistID ';

	SELECT @sql = @sql + ' WHERE 1 = 1 ';

		/* Lobbyist # Only Search */
		IF @lobbyistID IS NOT NULL 
		BEGIN
			SELECT @sql = @sql + ' AND l.ID = @xlobbyistID ';
		END

		/* Lobbyist Keyword Search */
		IF @lobbyistKeywords IS NOT NULL 
		BEGIN
			SELECT @sql = @sql + ' AND l.FullName LIKE ''%'' + @xlobbyistKeywords + ''%'' ';
		END

		/* Company Keyword Search */
		IF @companyKeywords IS NOT NULL 
		BEGIN
			SELECT @sql = @sql + ' AND (lc.FullName LIKE ''%'' + @xcompanyKeywords + ''%'' ';
			SELECT @sql = @sql + ' OR ' + 
				'(SELECT lb.FullName 
					FROM lobbyistCompanies lb 
					WHERE ID = xref.CompanyID) LIKE ''%'' + @xcompanyKeywords + ''%'' )';
		END

SELECT @sql = @sql + ') AS allmatches ';
SELECT @sql = @sql + ') AS rows ';
SELECT @sql = @sql + ' ) AS results ';

/* Paging Filter */
SELECT @sql = @sql + ' WHERE results.LobbyistOrder > @xstartRowIndex AND results.LobbyistOrder <= (@xstartRowIndex + @xmaximumRows)';

/* ORDER BY */
SELECT @sql = @sql + ' ORDER BY LobbyistOrder ASC ';

IF @debug = 0
   PRINT @sql

SELECT @paramlist = '@xstartRowIndex INT, 
	@xmaximumRows INT,
	@xlobbyistID INT,
	@xlobbyistKeywords VARCHAR(100),
	@xcompanyKeywords VARCHAR(100)';


EXEC sp_executesql @sql, @paramlist, 
	@startRowIndex, @maximumRows, 
	@lobbyistID, @lobbyistKeywords, @companyKeywords 



END

GO

