USE [CityController]
GO

/****** Object:  StoredProcedure [dbo].[GetContributions]    Script Date: 3/17/2018 4:08:16 PM ******/
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
	ContributorName, StreetAddress, City, State, Zip, Country, Employer, Occupation, Amount, CAST(DateContribution AS date) AS ContributionDate
	FROM contributions c
	LEFT JOIN tlk_contributionType t ON c.ContributionType = t.ID
	LEFT JOIN tlk_candidate cand ON c.CandidateID = cand.ID
	WHERE Approved=1
	ORDER BY CandidateName, Amount DESC;
END
GO


