USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[UpdateLobbyist]    Script Date: 3/17/2018 2:40:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 04/21/10
-- Description:	Updates Lobbyist info & Deletes Additional Companies
-- > deletes records for additional companies so that these can be created anew 
-- > w/out worrying about positions in datatables and existing items in tables

-- =============================================
ALTER PROCEDURE [dbo].[UpdateLobbyist]
	@LobbyistID INT,
	@Lobbyist NVARCHAR(100),
	@Position NVARCHAR(50),
	@Employer NVARCHAR(100),
	@Address NVARCHAR(100),
	@City NVARCHAR(50),
	@State NVARCHAR(50),
	@Zip NVARCHAR(15),
	@LobbyistStatus NVARCHAR(50),
	@ForCity BIT
 
AS 

BEGIN

	-- Delete additional companies from companies table
	DELETE 
	FROM lobbyistCompanies  
	WHERE EXISTS
		(select * 
			from xref_lobbyist_company x 
			where lobbyistCompanies.ID = x.CompanyID AND x.LobbyistID = @LobbyistID)

	-- Delete ref to additional companies in xref table
	DELETE FROM dbo.xref_lobbyist_company WHERE LobbyistID = @LobbyistID


	-- Update lobbyist's name/position
	UPDATE lobbyists 
		SET FullName = @Lobbyist, 
			Position = @Position,
			LobbyistStatus = @LobbyistStatus,
			ForCity = @ForCity
	--WHERE Employer = @LobbyistID
	WHERE ID = @LobbyistID

	-- Get CompanyID
	DECLARE @CompanyID INT
	SELECT @CompanyID = Employer FROM lobbyists WHERE ID = @LobbyistID

	-- Update employer details (keep same record for employer -> companyID)
	UPDATE lobbyistCompanies 
		SET FullName = @Employer,
			Address = @Address,
			City = @City,
			State = @State,
			Zip = @Zip
	WHERE ID = @CompanyID
END
GO


