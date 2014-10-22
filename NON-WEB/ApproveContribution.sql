USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[ApproveContribution]    Script Date: 10/22/2014 6:13:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Zeo
-- Create date: 10/22/14
-- Description:	Approve a Contribution (assumes validated; i.e., CreatedBy != ApprovedBy)
-- =============================================
CREATE PROCEDURE [dbo].[ApproveContribution]
	@UserName NVARCHAR(50),
	@ContributionID INT
AS 
BEGIN
    UPDATE contributions
	SET Approved = 1,
        ApprovedBy = @UserName,
		DateApproved = getdate()
	WHERE ContributionID = @ContributionID

RETURN 


END




GO


