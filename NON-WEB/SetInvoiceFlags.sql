USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[SetInvoiceFlags]    Script Date: 2/7/2015 11:44:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





/* Create a procedure to receive data for the table-valued parameter. */
CREATE PROCEDURE [dbo].[SetInvoiceFlags]
    @OID OnbaseIDTableType READONLY
    AS 
    SET NOCOUNT ON
    UPDATE contracts
       SET HasInvoicePDF=1
    FROM @OID a
    JOIN contracts b ON b.ContractID=a.ContractID


GO

