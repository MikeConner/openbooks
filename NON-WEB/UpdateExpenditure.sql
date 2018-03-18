USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[UpdateExpenditure]    Script Date: 3/18/2018 1:43:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 07/09/09
-- Description:	Update an Expenditure
-- =============================================
ALTER PROCEDURE [dbo].[UpdateExpenditure]
	@ExpenditureID INT,
	@CandidateID INT,
	@Office NVARCHAR(50), 
	@CompanyName NVARCHAR(100),
	@Address NVARCHAR(100), 
	@City NVARCHAR(50), 
	@State NVARCHAR(4), 
	@Province NVARCHAR(50),
	@Zip NVARCHAR(15), 
	@Country NVARCHAR(2),
	@Description NVARCHAR(100), 
	@Amount DECIMAL(14,2), 
	@DatePaid DATETIME = NULL 
 
AS 

BEGIN
	-- Check if a similar record does not exist 
	IF NOT EXISTS(SELECT ExpenditureID  
				FROM expenditures  
				WHERE ExpenditureID = @ExpenditureID)
		RETURN -1 

	ELSE
		UPDATE expenditures
			SET 
				CandidateID = @CandidateID,
				Office = @Office, 
				CompanyName = @CompanyName, 
				Address1 = @Address, 
				City = @City, 
				State = @State,
				Province = @Province, 
				Zip = @Zip, 
				Country = @Country,
				Description = @Description, 
				Amount = @Amount, 
				DatePaid = @DatePaid 
		WHERE ExpenditureID = @ExpenditureID
END
GO


