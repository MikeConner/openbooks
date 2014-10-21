
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
*/