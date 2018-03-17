USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[ElectedOfficialsSQL]    Script Date: 3/17/2018 2:10:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 05/20/09
-- Description:	Search Expenditures
-- =============================================
CREATE PROCEDURE [dbo].ElectedOfficialsSQL
	@sortColumn VARCHAR(25),
	@sortDirection CHAR(4)
AS

BEGIN

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = 'SELECT 
		Name, Office, Committee, Salary, PersonalPage, DisclosureLink, ImageUrl 
FROM
(
	SELECT ';
	SELECT @sql = @sql + 'ROW_NUMBER() OVER(ORDER BY ' + @sortColumn + ' ' + @sortDirection + ' ) AS OfficialOrder, ';
	SELECT @sql = @sql + ' Name, Office, Committee, Salary, PersonalPage, DisclosureLink, ImageUrl 
 	FROM 
	(
		SELECT e.Name, e.Office, e.Committee, e.Salary, 
			   e.PersonalPage, e.DisclosureLink, e.ImageUrl
		FROM elected_officials e
	) AS rows ';

SELECT @sql = @sql + ' ) AS results ';

/* ORDER BY */
SELECT @sql = @sql + ' ORDER BY OfficialOrder ASC ';

EXEC sp_executesql @sql
END
GO


