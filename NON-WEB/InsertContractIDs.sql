USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[InsertContractIDs]    Script Date: 2/8/2015 7:35:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/* Create a procedure to receive data for the table-valued parameter. */
CREATE PROCEDURE [dbo].[InsertContractIDs]
    @OID OnbaseIDTableType READONLY
    AS 
    SET NOCOUNT ON
	DELETE FROM tblOnbaseContracts
    INSERT INTO tblOnbaseContracts
           (ContractID)
        SELECT ContractID
        FROM @OID;

GO

