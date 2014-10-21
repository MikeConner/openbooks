
/*
USE [CityController]
GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 10/17/14
-- Description:	Set Permission Roles for User
-- =============================================
CREATE PROCEDURE [dbo].[SetUserRoles]
	@UserName NVARCHAR(50),
	@PermissionGroup NVARCHAR(50)
AS 
BEGIN
    UPDATE users
	SET PermissionGroup = @PermissionGroup
	WHERE UserName = @UserName

RETURN 


END

GO

ALTER TABLE users ADD CandidateID int;



ALTER TABLE contributions ADD CreatedBy nvarchar(50);
ALTER TABLE contributions ADD   ApprovedBy nvarchar(50);
ALTER TABLE contributions ADD   Approved bit default 0;
ALTER TABLE contributions ADD DateApproved datetime;


ALTER TABLE expenditures ADD CreatedBy nvarchar(50);
ALTER TABLE expenditures ADD   ApprovedBy nvarchar(50);
ALTER TABLE expenditures ADD   Approved bit default 0;
ALTER TABLE expenditures ADD DateApproved datetime;



*/