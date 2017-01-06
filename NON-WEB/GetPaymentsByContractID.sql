USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetPaymentsByContractID]    Script Date: 1/6/2017 1:48:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zeo
-- Create date: 1/6/17
-- Description:	Get Sum of all payments by ContractID
-- =============================================
CREATE PROCEDURE [dbo].[GetPaymentsByContractID] 
	@ContractID NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

SELECT Fund, CostCenter, ObjectAccount, LineItem, SUM(AmountReceived) AS Total
  FROM payments p
  WHERE p.ContractID=@ContractID
  GROUP BY Fund, CostCenter, ObjectAccount, LineItem
  ORDER BY Total DESC
END

GO


