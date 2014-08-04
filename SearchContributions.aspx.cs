﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

using System.Data;


public partial class _SearchContributionsPageClass: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Search
            GetSearchResults();
        }
        else
        {
            GetSearchResults();
              
        }
        //else
        //{
             
        //    //sticky criteria
        //    SearchParamsContribution sp = SearchContributions.GetQueryStringValues(HttpContext.Current.Request);
        //    ListItem li = ddlCandidateName.Items.FindByValue(sp.candidateID.ToString());

        //    if (li != null)
        //    {
        //        li.Selected = true;
        //    }
        //}
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

    public void GetSearchResults()
{
    // Get SearchParams Class from query string
    SearchParamsContribution sp = SearchContributions.GetQueryStringValues(HttpContext.Current.Request);
    if (sp.office != null){
    ddlOffice.Items.FindByValue(sp.office).Selected = true;
    }
    //Determine the Results Per Page from user
    SetPageSize();

    // Update Pager Results
    GetResultsCount(sp);

    // Fill DataTable from Search Results
    DataTable dt = SearchContributions.GetContributions(sp, PageIndex, PageSize, SortExpression, SortDirection);

    // Load repeater with data
    rptContributions.DataSource = dt;
    rptContributions.DataBind();



}
public void SetPageSize()
{
    int numResults = Convert.ToInt32(ddlPageSize.Text);
    if (numResults == 10 || numResults == 25 || numResults == 50 || numResults == 100)
    {
        PageSize = numResults;
    }
}
public void GetResultsCount(SearchParamsContribution sp)
{
    // Get total rows
    int totalRows = SearchContributions.GetContributionsCount(sp);

    // Update PageCount for pager, using adjustment if necessary
    int addPage = 1;
    if ((totalRows % PageSize) == 0)
    {
        addPage = 0;
    }
    PageCount = (totalRows / PageSize) + addPage;

    // Disable buttons if necessary
    ibtnFirstPageTop.Enabled = !(PageIndex == 0);
    ibtnPrevPageTop.Enabled = !(PageIndex == 0);
    ibtnNextPageTop.Enabled = !(PageIndex >= PageCount - 1);
    ibtnLastPageTop.Enabled = !(PageIndex >= PageCount - 1);

    // Calculate Results & Update Pager
    int startResults = (PageIndex * PageSize) + addPage;
    int endResults = (PageIndex * PageSize) + PageSize;

    if (endResults > totalRows)
    {
        endResults = totalRows;
    }
    if (startResults != 0)
    {
        lblCurrentPage.Text = "Results: " + startResults.ToString() + " - " + endResults.ToString() + " of " + totalRows.ToString();
    }
    else
    {
        lblCurrentPage.Text = "We couldn't find any contributions that matched your criteria.";
    }
}


//Pager Constants
public int PageIndex
{
    get
    {
        object o = ViewState["_PageIndex"];
        if (o == null)
            return 0; // default page index of 0
        else
            return (int)o;
    }
    set
    {
        ViewState["_PageIndex"] = value;
    }
}
public int PageSize
{
    get
    {
        object o = ViewState["_PageSize"];
        if (o == null)
            return 10; // default 10 rows
        else
            return (int)o;
    }
    set
    {
        ViewState["_PageSize"] = value;
    }
}
public int PageCount
{
    get
    {
        object o = ViewState["_PageCount"];
        if (o == null)
            return 0; // default no pages found
        else
            return (int)o;
    }
    set
    {
        ViewState["_PageCount"] = value;
    }
}

// Pager Controls
protected void FirstPage_Click(object sender, EventArgs e)
{
    // Send the user to the first page 
    PageIndex = 0;
    GetSearchResults();
}
protected void PrevPage_Click(object sender, EventArgs e)
{
    // Send the user to the previous page 
    PageIndex -= 1;
    GetSearchResults();
}
protected void NextPage_Click(object sender, EventArgs e)
{
    // Send the user to the next page 
    PageIndex += 1;
    GetSearchResults();
}
protected void LastPage_Click(object sender, EventArgs e)
{
    // Send the user to the last page 
    PageIndex = PageCount - 1;
    GetSearchResults();
}

// Sorting Constants
public string SortExpression
{
    get
    {
        object o = ViewState["_SortExpression"];
        if (o == null)
            return DEFAULTCOL; // default sort by Relevance Score
        else
            return (string)o;
    }
    set { ViewState["_SortExpression"] = value; }
}
public string SortDirection
{
    get
    {
        object o = ViewState["_SortDirection"];
        if (o == null)
            return ASCENDING;
        else
            return (string)o;
    }
    set { ViewState["_SortDirection"] = value; }
}
private const string DEFAULTCOL = "ContributionID";
private const string ASCENDING = "ASC";
private const string DESCENDING = "DESC";
private const string IMGDESC = "~/img/downarrow.gif";
private const string IMGASC = "~/img/uparrow.gif";
private const string IMGNOSORT = "~/img/placeholder.gif";

// Sorting Controls
public void sortContributor(object sender, EventArgs e)
{
    if (SortExpression == "ContributorName")
    {
        if (SortDirection == ASCENDING)
        {
            SortDirection = DESCENDING;
            //imgSortContributor.ImageUrl = IMGDESC;
        }
        else
        {
            SortDirection = ASCENDING;
            //imgSortContributor.ImageUrl = IMGASC;
        }
    }
    else
    {
        SortExpression = "ContributorName";
        SortDirection = ASCENDING;
        //imgSortContributor.ImageUrl = IMGASC;
    }

    GetSearchResults();
}
public void sortCandidate(object sender, EventArgs e)
{
    if (SortExpression == "CandidateID")
    {
        if (SortDirection == ASCENDING)
        {
            SortDirection = DESCENDING;
            //imgSortCandidate.ImageUrl = IMGDESC;
        }
        else
        {
            SortDirection = ASCENDING;
            //imgSortCandidate.ImageUrl = IMGASC;
        }
    }
    else
    {
        SortExpression = "CandidateID";
        SortDirection = ASCENDING;
        //imgSortCandidate.ImageUrl = IMGASC;
    }

    GetSearchResults();
}
public void sortOffice(object sender, EventArgs e)
{
    if (SortExpression == "Office")
    {
        if (SortDirection == ASCENDING)
        {
            SortDirection = DESCENDING;
            //imgSortOffice.ImageUrl = IMGDESC;
        }
        else
        {
            SortDirection = ASCENDING;
            //imgSortOffice.ImageUrl = IMGASC;
        }
    }
    else
    {
        SortExpression = "Office";
        SortDirection = ASCENDING;
        //imgSortOffice.ImageUrl = IMGASC;
    }

    GetSearchResults();
}
public void sortEmployer(object sender, EventArgs e)
{
    if (SortExpression == "Employer")
    {
        if (SortDirection == ASCENDING)
        {
            SortDirection = DESCENDING;
            //imgSortEmployer.ImageUrl = IMGDESC;
        }
        else
        {
            SortDirection = ASCENDING;
            //imgSortEmployer.ImageUrl = IMGASC;
        }
    }
    else
    {
        SortExpression = "Employer";
        SortDirection = ASCENDING;
        //imgSortEmployer.ImageUrl = IMGASC;
    }

    GetSearchResults();
}
public void sortAmount(object sender, EventArgs e)
{
    if (SortExpression == "Amount")
    {
        if (SortDirection == ASCENDING)
        {
            SortDirection = DESCENDING;
            //imgSortAmount.ImageUrl = IMGDESC;
        }
        else
        {
            SortDirection = ASCENDING;
            //imgSortAmount.ImageUrl = IMGASC;
        }
    }
    else
    {
        SortExpression = "Amount";
        SortDirection = ASCENDING;
        //imgSortAmount.ImageUrl = IMGASC;
    }

    GetSearchResults();
}
public void sortDate(object sender, EventArgs e)
{
    if (SortExpression == "DateContribution")
    {
        if (SortDirection == ASCENDING)
        {
            SortDirection = DESCENDING;
            //imgSortDate.ImageUrl = IMGDESC;
        }
        else
        {
            SortDirection = ASCENDING;
            //imgSortDate.ImageUrl = IMGASC;
        }
    }
    else
    {
        SortExpression = "DateContribution";
        SortDirection = ASCENDING;
        //imgSortDate.ImageUrl = IMGASC;
    }

    GetSearchResults();
}

/* Page Actions */
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Reset Page Index
        PageIndex = 0;

        // Reload Search
        GetSearchResults();
    }
}
