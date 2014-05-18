﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using OpenBookPgh;

public partial class Admin_Contributions : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        {
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
				
			GetSearchResults();
		}
	}
	// Page Load
	public void LoadDropDowns()
	{
		// Page Size
		int numResults = Utils.GetIntFromQueryString(Request.QueryString["num"]);
		if (numResults != 0)
		{
			ddlPageSize.SelectedValue = numResults.ToString();
		}
		// Office
		string office = Utils.GetStringFromQueryString(Request.QueryString["office"], true);
		if (!String.IsNullOrEmpty(office))
		{
			ddlOffice.SelectedValue = office;
		}
		// Candidate
		int candidateID = Utils.GetIntFromQueryString(Request.QueryString["candidate"]);
		if (candidateID != 0)
		{
			ddlCandidateName.SelectedValue = candidateID.ToString();
		}
	}	
	public void GetSearchResults()
	{
		// Get SearchParams Class from query string
		SearchParamsContribution sp = SearchContributions.GetQueryStringValues(HttpContext.Current.Request);

		// Update Dropdowns to show # per page, & office or candidate filtered on
		LoadDropDowns();
		
		// Update Pager Results
		GetResultsCount(sp);

		// Fill DataTable from Search Results
		DataTable dt = SearchContributions.GetContributions(sp, PageIndex, PageSize, SortExpression, SortDirection);

		// Load repeater with data
		rptContributions.DataSource = dt;
		rptContributions.DataBind();
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
	public int PageIndex { get { return Utils.GetIntFromQueryString(Request.QueryString["page"]); } }
	public int PageSize
	{
		get
		{
			int numResults = Utils.GetIntFromQueryString(Request.QueryString["num"]);
			if (numResults == 0)
			{
				numResults = 10;
			}
			return numResults;
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

	// Query determination
	public string QueryType { get { return GetQueryType(); } }
	public string GetQueryType()
	{
		string str = string.Empty;
		string strOffice = (Request.QueryString["office"] ?? "").Trim();
		string strCandidate = (Request.QueryString["candidate"] ?? "").Trim();
		// Office search or CandidateID search
		if (!string.IsNullOrEmpty(strOffice) || !string.IsNullOrEmpty(strCandidate))
		{
			if(!string.IsNullOrEmpty(strOffice))
				str = "office=" + strOffice;
			if(!string.IsNullOrEmpty(strCandidate))
				str = "candidate=" + strCandidate;
		}
		// Standard Paging - no search
		else
		{ 
			str = "q=";
		}
		return str;
	}

	// Sorting Constants
	public string SortExpression { get { return GetSortExpression(Request.QueryString["cat"]); } }
	public string SortDirection { get { return GetSortDirection(Request.QueryString["sort"]); } }

	public string GetSortExpression(string str)
	{
		string validList = "ContributorName CandidateID Office Employer Amount DateContribution";
		// Remove leading and trailing spaces
		str = (str ?? "").Trim();

		// Check against known list of values supplied
		if (!validList.Contains(str) || string.IsNullOrEmpty(str))
		{
			str = "ContributionID";
		}
		return str;
	}
	public string GetSortDirection(string str)
	{
		string validList = "ASC DESC";
		// Remove leading and trailing spaces
		str = (str ?? "").Trim();

		// Check against known list of values supplied
		if (!validList.Contains(str) || string.IsNullOrEmpty(str))
		{
			str = "ASC";
		}
		return str;
	}
	public string GenerateQueryString(int page, string category, string sort, int numResults)
	{
		return "Contributions.aspx?" + QueryType + "&page=" + page + "&cat=" + category + "&sort=" + sort + "&num=" + numResults;
	}

	// Pager Controls
	protected void FirstPage_Click(object sender, EventArgs e)
	{
		// Send the user to the first page 
		int page = 0;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize));
	}
	protected void PrevPage_Click(object sender, EventArgs e)
	{
		// Send the user to the previous page 
		int page = PageIndex - 1;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize));
	}
	protected void NextPage_Click(object sender, EventArgs e)
	{
		// Send the user to the next page 
		int page = PageIndex + 1;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize));
	}
	protected void LastPage_Click(object sender, EventArgs e)
	{
		// Send the user to the last page 
		int page = PageCount - 1;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize));
	}
	
	// Sorting Controls
	public void sortContributor(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "ContributorName", sort, PageSize));
	}
	public void sortCandidate(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "CandidateID", sort, PageSize));
	}
	public void sortOffice(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "Office", sort, PageSize));
	}
	public void sortEmployer(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "Employer", sort, PageSize));
	}
	public void sortAmount(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "Amount", sort, PageSize));
	}
	public void sortDate(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "DateContribution", sort, PageSize));
	}
	
	/* Page Actions */
	protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		int page = 0; // reset page
		int numResults = Convert.ToInt32(ddlPageSize.Text);

		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, numResults));
	}
	protected void ddlCandidateName_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect("Contributions.aspx?candidate=" + ddlCandidateName.SelectedValue.ToString() + "&page=0&cat=CandidateID");
	}
	protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect("Contributions.aspx?office=" + ddlOffice.SelectedValue.ToString() + "&page=0&cat=Office");
	}
	protected void rptContributions_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "edit")
		{
			Response.Redirect("EditContribution.aspx?id=" + e.CommandArgument.ToString());
		}

		if (e.CommandName == "delete")
		{
			Admin.DeleteContribution(Convert.ToInt32(e.CommandArgument.ToString()));

			if (Session["PreviousPage"] != null)
				Response.Redirect((string)Session["PreviousPage"]);
			else
				Response.Redirect("~/Admin/Default.aspx");

		}
	}
}
