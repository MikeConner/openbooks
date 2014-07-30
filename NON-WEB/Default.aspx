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

