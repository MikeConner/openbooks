USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetPaymentDetailsByContractID]    Script Date: 2/10/2017 7:06:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








-- =============================================
-- Author:		Jeff
-- Create date: 2/2/17
-- Description:	Get payments with dates by ContractID
-- =============================================
CREATE PROCEDURE [dbo].[GetPaymentDetailsByContractID] 
	@ContractID NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

SELECT AmountPaid, CAST(DatePaid AS Date) AS PaidDate
  FROM payment_details d
  WHERE d.ContractID=@ContractID
  ORDER BY DatePaid DESC
END








GO


