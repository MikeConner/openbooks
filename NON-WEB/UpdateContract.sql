USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[UpdateContract]    Script Date: 3/10/2015 1:44:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Zeo
-- Create date: 06/09/09
-- Description:	Update a contract

-- =============================================
ALTER PROCEDURE [dbo].[UpdateContract]
	@ContractID NVARCHAR(50),
	@VendorNo NVARCHAR(10),
	@VendorName NVARCHAR(50),
	@SecondVendorNo NVARCHAR(10),
	@SecondVendorName NVARCHAR(50),
	@DepartmentID INT,
	@SupplementalNo INT,
	@NewSupplementalNo INT,
	@ResolutionNo NVARCHAR(40),
	@Service INT,
	@Amount DECIMAL(14,2),
	@OriginalAmount DECIMAL(14,2),
	@Description NVARCHAR(100),
	@DateDuration DATETIME = NULL,
	@DateCountersigned DATETIME = NULL
AS 

BEGIN
	-- Check if supplemental no will be changed
	IF @SupplementalNo <> @NewSupplementalNo
	BEGIN
			-- Check if new no would overwrite another record
		IF EXISTS(SELECT 1 FROM contracts WHERE ContractID = @ContractID AND SupplementalNo = @NewSupplementalNo)
			BEGIN
				RETURN -1
			END
		ELSE
			BEGIN
			-- update with new supplemental no
				UPDATE contracts SET 
					VendorNo = @VendorNo, VendorName = @VendorName, 
					SecondVendorNo = (CASE @SecondVendorNo WHEN '' THEN NULL ELSE @SecondVendorNo END), 
					SecondVendorName = (CASE @SecondVendorName WHEN '' THEN NULL ELSE @SecondVendorName END), 
					DepartmentID = @DepartmentID,	SupplementalNo = @NewSupplementalNo,
					ResolutionNo = @ResolutionNo, [Service] = @Service, Amount = @Amount, OriginalAmount = @OriginalAmount,
					[Description] = @Description, DateDuration = @DateDuration, DateCountersigned = @DateCountersigned
				WHERE ContractID = @ContractID AND SupplementalNo = @SupplementalNo
			END
	END
	ELSE
		-- update with current supplemental no
		UPDATE contracts SET 
			VendorNo = @VendorNo, VendorName = @VendorName,
			SecondVendorNo = (CASE @SecondVendorNo WHEN '' THEN NULL ELSE @SecondVendorNo END), 
			SecondVendorName = (CASE @SecondVendorName WHEN '' THEN NULL ELSE @SecondVendorName END), 
			DepartmentID = @DepartmentID,	SupplementalNo = @SupplementalNo,
			ResolutionNo = @ResolutionNo, [Service] = @Service, Amount = @Amount, OriginalAmount = @OriginalAmount,
			[Description] = @Description, DateDuration = @DateDuration, DateCountersigned = @DateCountersigned
		WHERE ContractID = @ContractID AND SupplementalNo = @SupplementalNo	


END



GO

