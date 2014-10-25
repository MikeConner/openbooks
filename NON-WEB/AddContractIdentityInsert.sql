USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[AddContractIdentityInsert]    Script Date: 10/24/2014 2:08:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Zeo
-- Create date: 11/25/09
-- Description:	Sets Identity Insert to ON 
-- =============================================
ALTER PROCEDURE [dbo].[AddContractIdentityInsert]
	@ContractID NVARCHAR(50),
	@VendorNo NVARCHAR(10),
	@VendorName NVARCHAR(100),
	@DepartmentID INT,
	@SupplementalNo INT,
	@Service INT,
	@Amount DECIMAL(14,2),
	@OriginalAmount DECIMAL(14,2),
	@Description NVARCHAR(100),
	@DateCountersigned DATETIME,
	@DateDuration DATETIME
AS 

BEGIN

INSERT INTO contracts(
	ContractID,VendorNo,VendorName,DepartmentID,
	SupplementalNo,Service,Amount,OriginalAmount,
	Description,DateCountersigned,DateDuration,DateEntered
)
VALUES
(
	@ContractID,@VendorNo,@VendorName,@DepartmentID,
	@SupplementalNo,@Service,@Amount,@OriginalAmount,
	@Description,@DateCountersigned,@DateDuration, GETDATE() 
)


END


GO

