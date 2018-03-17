USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetLobbyist]    Script Date: 3/17/2018 1:33:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zeo
-- Create date: 4/21/10
-- Description:	Get Lobbyist info & employer for edit table on editlobbyists.aspx
-- =============================================
ALTER PROCEDURE [dbo].[GetLobbyist] 
	@LobbyistID INT
AS
BEGIN

	-- Company Table
	SELECT 
		l.ID, l.FullName, l.Position, l.Employer, l.DateEntered, 
		lc.FullName AS EmployerName, lc.Address, lc.City, lc.State, lc.Zip, l.LobbyistStatus, l.ForCity 

	FROM lobbyists l 
	LEFT JOIN lobbyistCompanies lc ON l.Employer = lc.ID  
	WHERE l.ID = @LobbyistID



END
GO


