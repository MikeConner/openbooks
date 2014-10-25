USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[DeleteContract]    Script Date: 10/24/2014 2:09:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 07/09/09
-- Description:	Deletes a contract.
-- =============================================
ALTER PROCEDURE [dbo].[DeleteContract]
	@ContractID NVARCHAR(50),
	@SupplementalNo INT
AS 
BEGIN
	-- Check if record does not exist
	IF @ContractID IS NOT NULL 

	DELETE FROM contracts WHERE ContractID = @ContractID AND SupplementalNo = @SupplementalNo 
END


GO

