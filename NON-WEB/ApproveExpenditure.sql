USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[ApproveExpenditure]    Script Date: 10/22/2014 6:58:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Zeo
-- Create date: 10/22/14
-- Description:	Approve an Expenditure (assumes validated; i.e., CreatedBy != ApprovedBy)
-- =============================================
CREATE PROCEDURE [dbo].[ApproveExpenditure]
	@UserName NVARCHAR(50),
	@ExpenditureID INT
AS 
BEGIN
    UPDATE expenditures
	SET Approved = 1,
        ApprovedBy = @UserName,
		DateApproved = getdate()
	WHERE ExpenditureID = @ExpenditureID

RETURN 


END





GO

