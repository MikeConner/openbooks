USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[AddVendor]    Script Date: 6/12/2015 6:54:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zeo
-- Create date: 06/07/09
-- Description:	Add a Vendor to vendors table & tlk_vendors. 
-- -> Uses an Output parameter to return the error message with the Vendor #.
-- -> A Return parameter could be used instead by 'RETURN -1' and then checking
-- -> within asp.net code for the value
-- =============================================
CREATE PROCEDURE [dbo].[AddForeignVendor]
	@vendorName NVARCHAR(100),
	@vendorNo NVARCHAR(10),
	@address1 NVARCHAR(100),
	@address2 NVARCHAR(100),
	@address3 NVARCHAR(100),
	@country NVARCHAR(50),
	@city NVARCHAR(50),
	@province NVARCHAR(50),
	@zip NVARCHAR(15),
	@message NVARCHAR(35) OUTPUT 
AS 

BEGIN
	-- Check if vendor # already in database, return error message
	IF EXISTS(SELECT VendorNo FROM vendors WHERE VendorNo = @vendorNo)
		/* RETURN -1 */
		SET @message = 'Vendor #: ' + @vendorNo + ' already exists.'

	ELSE
		INSERT INTO vendors 
		(
			VendorNo, VendorName, Address1, Address2, Address3, Country, City, Province, Zip
		)
		VALUES 
		(
			@vendorNo, @vendorName, @address1, @address2, @address3, @country, @city, @province, @zip
		)
		INSERT INTO tlk_vendors
		(
			VendorNo, VendorName
		)
		VALUES
		(
			@vendorNo, @vendorName
		)
END
GO


