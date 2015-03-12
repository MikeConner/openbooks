USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[AddContract]    Script Date: 3/10/2015 2:24:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Zeo
-- Create date: 06/08/09
-- Description:	Add a Vendor to vendors table & tlk_vendors. 
-- -> Uses an Return parameter to return the error if a similar contract 
-- -> exists in DB already.

-- =============================================
ALTER PROCEDURE [dbo].[AddContract]
	@ContractNo NVARCHAR(50) = NULL,
	@VendorNo NVARCHAR(10),
	@VendorName NVARCHAR(50),
	@SecondVendorNo NVARCHAR(10),
	@SecondVendorName NVARCHAR(50),
	@DepartmentID INT,
	@SupplementalNo INT,
	@ResolutionNo NVARCHAR(40),
	@Service INT,
	@Amount DECIMAL(14,2),
	@OriginalAmount DECIMAL(14,2),
	@Description NVARCHAR(100),
	@DateDuration DATETIME = NULL,
	@DateCountersigned DATETIME = NULL,
	@DateEntered DATETIME = NULL
 
AS 

BEGIN

	IF @ContractNo IS NULL OR @ContractNo = '' 
		BEGIN
			-- Get next contract no
			-- OK Even though ContractID is a string; this is for new ones generated here
			DECLARE @NextContractID INT 
			SELECT @NextContractID = NextContractNo FROM dbo.tlk_NextContractNo
			
			-- standard insert
			INSERT INTO contracts
			(
				ContractID, VendorNo, VendorName, SecondVendorNo, SecondVendorName, DepartmentID, SupplementalNo, ResolutionNo, [Service], Amount, OriginalAmount, Description,
				DateDuration, DateCountersigned, DateEntered
			)
			VALUES 
			(
				@NextContractID, @VendorNo, @VendorName, 
				(CASE @SecondVendorNo WHEN '' THEN NULL ELSE @SecondVendorNo END),
				(CASE @SecondVendorName WHEN '' THEN NULL ELSE @SecondVendorName END),
				@DepartmentID, @SupplementalNo, @ResolutionNo, @Service, @Amount, @OriginalAmount, @Description,
				@DateDuration, @DateCountersigned, @DateEntered
			)
			-- update counter
			UPDATE dbo.tlk_NextContractNo SET NextContractNo = NextContractNo + 1
			RETURN 0
		END
	ELSE
		BEGIN
			-- check if contract #/sup# in use
			IF EXISTS(SELECT 1 FROM contracts WHERE ContractID = @ContractNo AND SupplementalNo = @SupplementalNo)
			BEGIN
				RETURN -1
			END

			ELSE
				BEGIN
				INSERT INTO contracts
				(
					ContractID, VendorNo, VendorName, SecondVendorNo, SecondVendorName, DepartmentID, SupplementalNo, ResolutionNo, [Service], Amount, OriginalAmount, Description,
					DateDuration, DateCountersigned, DateEntered
				)
				VALUES 
				(
					@ContractNo, @VendorNo, @VendorName, 
				    (CASE @SecondVendorNo WHEN '' THEN NULL ELSE @SecondVendorNo END),
				    (CASE @SecondVendorName WHEN '' THEN NULL ELSE @SecondVendorName END),
					@DepartmentID, @SupplementalNo, @ResolutionNo, @Service, @Amount, @OriginalAmount, @Description,
					@DateDuration, @DateCountersigned, @DateEntered
				)
				-- update counter
				UPDATE dbo.tlk_NextContractNo SET NextContractNo = NextContractNo + 1
				RETURN 0
			END
		END
END


GO

