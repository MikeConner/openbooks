USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[AddContribution]    Script Date: 10/21/2014 12:26:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 06/08/09
-- Description:	Add a Contribution to contribution table. 
-- -> Uses an Return parameter to return the error if a similar record 
-- -> exists in DB already.
-- =============================================
ALTER PROCEDURE [dbo].[AddContribution]
	@CandidateID INT,
	@Office NVARCHAR(50), 
	@ContributorType CHAR(2), 
	@ContributorName NVARCHAR(100), 
	@ContributionType INT, 
	@Description NVARCHAR(255),
	@City NVARCHAR(50), 
	@State NVARCHAR(4), 
	@Zip NVARCHAR(15), 
	@Employer NVARCHAR(100), 
	@Occupation NVARCHAR(100), 
	@Amount DECIMAL(14,2), 
	@DateContribution DATETIME = NULL, 
	@CreatedBy NVARCHAR(50)
 
AS 

BEGIN
	-- Check if a similar record exists already with 
	-- same Candidate ID, Contributor, Amount & Date of Contribution
	IF EXISTS(SELECT ContributionID  
				FROM contributions 
				WHERE 
					CandidateID = @CandidateID AND 
					ContributorName = @ContributorName AND 
					Amount = @Amount AND 
					DateContribution = @DateContribution)
		RETURN -1 

	ELSE
		INSERT INTO contributions
		(
			CandidateID, Office, ContributorType, ContributionType, ContributorName, Description, City, State, Zip, 
			Employer, Occupation, Amount, DateContribution, DateEntered, CreatedBy	
		)
		VALUES 
		(
			@CandidateID, @Office, @ContributorType, @ContributionType, @ContributorName, @Description, @City, @State, @Zip, 
			@Employer, @Occupation, @Amount, @DateContribution, getdate(), @CreatedBy
		)

END

GO

