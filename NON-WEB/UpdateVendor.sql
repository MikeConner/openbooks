USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[UpdateVendor]    Script Date: 6/16/2015 7:34:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Zeo
-- Create date: 07/16/09
-- Description:	Update an Vendor
-- =============================================
ALTER PROCEDURE [dbo].[UpdateVendor]
	@QuerystringVendorNo NVARCHAR(10),
	@VendorNo NVARCHAR(10),
	@VendorName NVARCHAR(100),
	@Address1 NVARCHAR(100), 
	@Address2 NVARCHAR(100), 
	@Address3 NVARCHAR(100), 
	@City NVARCHAR(50), 
	@State NVARCHAR(4), 
	@Zip NVARCHAR(15)
 
AS 

BEGIN
	-- Check if a similar record does not exist 
	IF NOT EXISTS(SELECT VendorNo 
				FROM vendors  
				WHERE VendorNo = @QuerystringVendorNo)
		RETURN -1 

	ELSE
		UPDATE vendors
			SET 
				VendorName = @VendorName, 
				Address1 = @Address1, 
				Address2 = @Address2, 
				Address3 = @Address3, 
				Country = NULL,
				Province = NULL,
				City = @City, 
				State = @State, 
				Zip = @Zip 
				
		WHERE VendorNo = @QuerystringVendorNo 

		UPDATE tlk_vendors 
			SET VendorName = @VendorName 
		WHERE VendorNo = @QuerystringVendorNo
END
GO


