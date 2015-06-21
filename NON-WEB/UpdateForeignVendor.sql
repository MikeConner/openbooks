USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[UpdateForeignVendor]    Script Date: 6/16/2015 7:33:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Zeo
-- Create date: 07/16/09
-- Description:	Update an Vendor
-- =============================================
ALTER PROCEDURE [dbo].[UpdateForeignVendor]
	@QuerystringVendorNo NVARCHAR(10),
	@VendorNo NVARCHAR(10),
	@VendorName NVARCHAR(100),
	@Address1 NVARCHAR(100), 
	@Address2 NVARCHAR(100), 
	@Address3 NVARCHAR(100), 
	@Country NVARCHAR(50),
	@City NVARCHAR(50), 
	@Province NVARCHAR(50), 
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
				City = @City, 
				State = NULL,
				Country = @Country, 
				Province = @Province,
				Zip = @Zip 
				
		WHERE VendorNo = @QuerystringVendorNo 

		UPDATE tlk_vendors 
			SET VendorName = @VendorName 
		WHERE VendorNo = @QuerystringVendorNo
END

GO


