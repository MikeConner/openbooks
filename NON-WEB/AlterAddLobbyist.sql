USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[AddLobbyist]    Script Date: 3/17/2018 1:27:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zeo
-- Create date: 04/20/10
-- Description:	Adds record for Lobbyist 
-- > returns newly created LobbyistID from insert

-- =============================================
ALTER PROCEDURE [dbo].[AddLobbyist]
	@LobbyistID INT OUTPUT,
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

DECLARE @EmployerID INT

	-- Add Lobbyist
	INSERT INTO lobbyists (FullName, Position, LobbyistStatus, ForCity, DateEntered) VALUES (@Lobbyist, @Position, @LobbyistStatus, @ForCity, getdate())

		-- Return Var
		SET @LobbyistID = SCOPE_IDENTITY()

	-- Add Company
	INSERT INTO lobbyistCompanies (FullName, Address, City, State, Zip, DateEntered)
	VALUES(@Employer, @Address, @City, @State, @Zip, getdate())

		-- Internal Var
		SET @EmployerID = SCOPE_IDENTITY()

	-- Update Lobbyist table with CompanyID
	UPDATE lobbyists SET Employer = @EmployerID WHERE ID = @LobbyistID
END
GO


