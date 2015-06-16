USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SearchVendorsSQL]    Script Date: 6/16/2015 9:11:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Zeo
-- Create date: 07/16/09
-- Description:	Search Vendors
-- =============================================
ALTER PROCEDURE [dbo].[SearchVendorsSQL]
	@pageIndex INT, 
	@maximumRows INT,
	@sortColumn VARCHAR(25) = 'VendorNo',
	@sortDirection CHAR(4) = 'DESC',
	@VendorID INT = NULL,
	@debug	BIT = 0

AS

BEGIN

DECLARE @startRowIndex INT;
SET @startRowIndex = (@pageIndex * @maximumRows);

DECLARE @sql NVARCHAR(4000), @paramlist NVARCHAR(4000);

SELECT @sql = 'SELECT 
		VendorOrder, 
		VendorNo, VendorName, Address1, Address2, Address3, City, State, Zip, Country, Province
FROM
(
	SELECT ';
	SELECT @sql = @sql + 'ROW_NUMBER() OVER(ORDER BY ' + @sortColumn + ' ' + @sortDirection + ') AS VendorOrder, ';
	SELECT @sql = @sql + ' VendorNo, VendorName, Address1, Address2, Address3, City, State, Zip, Country, Province
 	FROM 
	(
		SELECT v.VendorNo, v.VendorName, v.Address1, v.Address2, v.Address3, v.City, v.State, v.Zip, v.Country, v.Province
		FROM vendors v  
	) AS rows ';

SELECT @sql = @sql + ' WHERE 1 = 1 ';

		/* Candidate ID */
		IF @VendorID IS NOT NULL 
			SELECT @sql = @sql + ' AND VendorNo = @xVendorID ';

SELECT @sql = @sql + ' ) AS results ';


/* Paging Filter */
SELECT @sql = @sql + ' WHERE results.VendorOrder > @xstartRowIndex AND results.VendorOrder <= (@xstartRowIndex + @xmaximumRows)';

/* ORDER BY */
SELECT @sql = @sql + ' ORDER BY VendorOrder ASC ';

IF @debug = 0
   PRINT @sql

SELECT @paramlist = '@xstartRowIndex INT, 
	@xmaximumRows INT,
	@xVendorID INT';

EXEC sp_executesql @sql, @paramlist, 
	@startRowIndex, @maximumRows, 
	@VendorID




END

GO


