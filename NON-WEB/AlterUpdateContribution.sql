USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[UpdateContribution]    Script Date: 3/18/2018 12:55:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Zeo
-- Create date: 06/08/09
-- Description:	Update a contribution.
-- =============================================
ALTER PROCEDURE [dbo].[UpdateContribution]
	@ContributionID INT,
	@CandidateID INT,
	@Office NVARCHAR(50), 
	@ContributorType CHAR(2), 
	@ContributorName NVARCHAR(100), 
	@ContributionType INT, 
	@Description NVARCHAR(255),
	@StreetAddress NVARCHAR(50),
	@City NVARCHAR(50), 
	@State NVARCHAR(4), 
	@Province NVARCHAR(50),
	@Zip NVARCHAR(15), 
	@Country NVARCHAR(2),
	@Employer NVARCHAR(100), 
	@Occupation NVARCHAR(100), 
	@Amount DECIMAL(14,2), 
	@DateContribution DATETIME = NULL 
AS 
BEGIN
	-- Check if record does not exist
	IF NOT EXISTS(SELECT ContributionID  
				FROM contributions 
				WHERE ContributionID = @ContributionID)
		RETURN -1 

	ELSE
		UPDATE contributions
		SET 
			CandidateID = @CandidateID, 
			Office = @Office, 
			ContributorType = @ContributorType, 
			ContributionType = @ContributionType, 
			ContributorName = @ContributorName, 
			Description = @Description,
			StreetAddress = @StreetAddress,
			City = @City, 
			State = @State, 
			Province = @Province,
			Zip = @Zip, 
			Country = @Country,
			Employer = @Employer, 
			Occupation = @Occupation, 
			Amount = @Amount, 
			DateContribution = @DateContribution 
		WHERE ContributionID = @ContributionID
END
GO


