USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SetUserCandidateID]    Script Date: 10/21/2014 12:25:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Zeo
-- Create date: 10/19/14
-- Description:	Set CandidateID for a Candidate User
-- =============================================
CREATE PROCEDURE SetUserCandidateID
	@UserName NVARCHAR(50),
	@CandidateID INT
AS 
BEGIN
    UPDATE users
	SET CandidateID = @CandidateID
	WHERE UserName = @UserName AND (PermissionGroup = 'candidate')

RETURN 


END



GO

