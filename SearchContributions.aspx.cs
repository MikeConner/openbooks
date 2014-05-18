using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class _SearchContributionsPageClass: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		// Distance calc
		double distance = Convert.ToDouble(ddlDistance.SelectedValue);
		string zip = txtZip.Text;
		
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ddlOffice.SelectedValue;
		int year1 = 0;
		Int32.TryParse(ddldateContribution.SelectedValue, out year1);
		string contributorSearchOptions = rblContributorSearch.SelectedValue;
		string contributorKeywords = txtContributor.Text;
		string employerKeywords = txtEmployer.Text;

		string queryString = SearchContributions.GenerateQueryString(candidateID, office, year1, 
		contributorKeywords, contributorSearchOptions, employerKeywords, zip, distance);
		
		Response.Redirect(queryString);	
	}
}
