USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetExpenditures]    Script Date: 2/2/2017 2:25:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Jeff
-- Create date: 02/17/17
-- Description:	Get all Expenditures
-- =============================================
CREATE PROCEDURE [dbo].[GetExpenditures] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT cand.CandidateName, 
		CASE WHEN Office = 'mayor' THEN 'Mayor'
		 WHEN Office = 'council' THEN 'City Council'
		 WHEN Office = 'controller' THEN 'City Controller'
		 ELSE 'Unknown'			
	END AS OfficeName,
	CompanyName, City, State, Zip, Amount, CAST(DatePaid AS date) AS ExpenditureDate, Description
	FROM expenditures e
	LEFT JOIN tlk_candidate cand ON e.CandidateID = cand.ID
	WHERE Approved=1
	ORDER BY CandidateName, Amount DESC;
END




GO


