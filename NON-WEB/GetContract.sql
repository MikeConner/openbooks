USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetContract]    Script Date: 10/24/2014 2:09:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 07/09/09
-- Description:	Get a single Contract
-- =============================================
ALTER PROCEDURE [dbo].[GetContract] 
	@ContractID NVARCHAR(50),
	@SupplementalNo INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM contracts WHERE ContractID = @ContractID AND SupplementalNo = @SupplementalNo

END


GO

