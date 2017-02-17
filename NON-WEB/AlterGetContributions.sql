USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetContributions]    Script Date: 2/17/2017 12:00:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Jeff
-- Create date: 02/01/17
-- Description:	Get all Contributions
-- =============================================
ALTER PROCEDURE [dbo].[GetContributions] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT cand.CandidateName, 
		CASE WHEN Office = 'mayor' THEN 'Mayor'
		 WHEN Office = 'council' THEN 'City Council'
		 WHEN Office = 'controller' THEN 'City Controller'
		 ELSE 'Unknown'			
	END AS OfficeName,
	t.ContributionType, 
	CASE WHEN ContributorType = 'in' THEN 'Contributor'
		 ELSE 'Committee'			
	END AS ContributorTypeName,
	ContributorName, City, State, Zip, Employer, Occupation, Amount, CAST(DateContribution AS date) AS ContributionDate
	FROM contributions c
	LEFT JOIN tlk_contributionType t ON c.ContributionType = t.ID
	LEFT JOIN tlk_candidate cand ON c.CandidateID = cand.ID
	WHERE Approved=1
	ORDER BY CandidateName, Amount DESC;
END




GO


