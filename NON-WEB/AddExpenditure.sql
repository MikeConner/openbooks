USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[AddExpenditure]    Script Date: 10/21/2014 12:26:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Zeo
-- Create date: 06/08/09
-- Description:	Add an Expenditure to expenditures table. 
-- -> Uses an Return parameter to return the error if a similar record 
-- -> exists in DB already.
-- =============================================
ALTER PROCEDURE [dbo].[AddExpenditure]
	@CandidateID INT,
	@Office NVARCHAR(50), 
	@CompanyName NVARCHAR(100),
	@Address NVARCHAR(100), 
	@City NVARCHAR(50), 
	@State NVARCHAR(4), 
	@Zip NVARCHAR(15), 
	@Description NVARCHAR(100), 
	@Amount DECIMAL(14,2), 
	@DatePaid DATETIME = NULL,
    @CreatedBy NVARCHAR(50) 
AS 

BEGIN
	-- Check if a similar record exists already with 
	-- same Candidate ID, Contributor, Amount & Date of Contribution
	IF EXISTS(SELECT ExpenditureID  
				FROM expenditures  
				WHERE 
					CandidateID = @CandidateID AND 
					CompanyName = @CompanyName AND 
					Amount = @Amount AND 
					DatePaid = @DatePaid)
		RETURN -1 

	ELSE
		INSERT INTO expenditures
		(
			CandidateID, Office, CompanyName, Address1, City, State, Zip, Description, Amount, DatePaid, DateEntered, CreatedBy	
		)
		VALUES 
		(
			@CandidateID, @Office, @CompanyName, @Address, @City, @State, @Zip, @Description, @Amount, @DatePaid, getdate(), @CreatedBy
		)

END

GO

