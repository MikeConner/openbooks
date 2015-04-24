using System;
using System.Collections.Generic;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using OpenBookPgh;

public partial class Admin_Contributions : PaginatedPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (Auth.EnsureRole(Auth.ADMIN_USER_ROLE))
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
                }
                mPageController = new PagingControls(this);

                GetSearchResults();
            }
        }
        else
        {
            Response.Status = "403 Forbidden";
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.StatusDescription = "Permission failure";
            // Could also redirect to "/Error.aspx"
            Response.Redirect("Default.aspx");
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

        // Approved
        cbApproved.Checked = Utils.GetBooleanFromQueryString(Request.QueryString["approved"]);
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
		DataTable dt = SearchContributions.GetContributions(sp, PageIndex, PageSize, SortExpression, SortDirection, cbApproved.Checked);

		// Load repeater with data
		rptContributions.DataSource = dt;
		rptContributions.DataBind();
	}
	public void GetResultsCount(SearchParamsContribution sp)
	{
		// Get total rows
        int totalRows = SearchContributions.GetContributionsCount(sp, cbApproved.Checked);

        mPageController.setPageCount(totalRows);

        lblCurrentPage.Text = mPageController.getPagingBanner();

		// Disable buttons if necessary
		ibtnFirstPageTop.Enabled = ibtnPrevPageTop.Enabled = PageIndex > 0;
		ibtnNextPageTop.Enabled = ibtnLastPageTop.Enabled = PageIndex < PageCount - 1;
	}

	// Query determination
	public string QueryType { get { return GetQueryType(); } }
	public string GetQueryType()
	{
		string str = string.Empty;
        string strOffice = (ddlOffice.SelectedValue.ToString() ?? Request.QueryString["office"]).Trim();
        string strCandidate = (ddlCandidateName.SelectedValue.ToString() ?? Request.QueryString["candidate"]).Trim();

		// Office search or CandidateID search
		if (!string.IsNullOrEmpty(strOffice) || !string.IsNullOrEmpty(strCandidate))
		{
            if (!string.IsNullOrEmpty(strOffice))
            {
                str = "office=" + strOffice;
            }
            if (!string.IsNullOrEmpty(strCandidate))
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = "candidate=" + strCandidate;
                }
                else
                {
                    str += "&candidate=" + strCandidate;
                }
            }
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
	public string GenerateQueryString(int page, string category, string sort, int numResults, Boolean approved)
	{
		return "Contributions.aspx?" + QueryType + "&page=" + page + "&cat=" + category + "&sort=" + sort + "&num=" + numResults + "&approved=" + approved;
	}

	// Pager Controls
	protected void FirstPage_Click(object sender, EventArgs e)
	{
		// Send the user to the first page 
		int page = 0;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize, cbApproved.Checked));
	}
	protected void PrevPage_Click(object sender, EventArgs e)
	{
		// Send the user to the previous page 
		int page = PageIndex - 1;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize, cbApproved.Checked));
	}
	protected void NextPage_Click(object sender, EventArgs e)
	{
		// Send the user to the next page 
		int page = PageIndex + 1;
        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize, cbApproved.Checked));
	}
	protected void LastPage_Click(object sender, EventArgs e)
	{
		// Send the user to the last page 
		int page = PageCount - 1;
        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize, cbApproved.Checked));
	}
	
	// Sorting Controls
	public void sortContributor(object sender, EventArgs e)
	{
		string sort = "ASC";
        if (SortDirection == "ASC")
        {
            sort = "DESC";
        }

        Response.Redirect(GenerateQueryString(PageIndex, "ContributorName", sort, PageSize, cbApproved.Checked));
	}
	public void sortCandidate(object sender, EventArgs e)
	{
		string sort = "ASC";
        if (SortDirection == "ASC")
        {
            sort = "DESC";
        }

        Response.Redirect(GenerateQueryString(PageIndex, "CandidateID", sort, PageSize, cbApproved.Checked));
	}
	public void sortOffice(object sender, EventArgs e)
	{
		string sort = "ASC";
        if (SortDirection == "ASC")
        {
            sort = "DESC";
        }

        Response.Redirect(GenerateQueryString(PageIndex, "Office", sort, PageSize, cbApproved.Checked));
	}
	public void sortEmployer(object sender, EventArgs e)
	{
		string sort = "ASC";
        if (SortDirection == "ASC")
        {
            sort = "DESC";
        }

        Response.Redirect(GenerateQueryString(PageIndex, "Employer", sort, PageSize, cbApproved.Checked));
	}
	public void sortAmount(object sender, EventArgs e)
	{
		string sort = "ASC";
        if (SortDirection == "ASC")
        {
            sort = "DESC";
        }

        Response.Redirect(GenerateQueryString(PageIndex, "Amount", sort, PageSize, cbApproved.Checked));
	}
	public void sortDate(object sender, EventArgs e)
	{
		string sort = "ASC";
        if (SortDirection == "ASC")
        {
            sort = "DESC";
        }

        Response.Redirect(GenerateQueryString(PageIndex, "DateContribution", sort, PageSize, cbApproved.Checked));
	}
	
	/* Page Actions */
    protected void ddlSelection_Changed(object sender, EventArgs e) {
		int page = 0; // reset page
		int numResults = Convert.ToInt32(ddlPageSize.Text);

        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, numResults, cbApproved.Checked));
    }

	protected void rptContributions_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "edit")
		{
			Response.Redirect("EditContribution.aspx?id=" + e.CommandArgument.ToString());
		}
        else if (e.CommandName == "approve")
        {
            Admin.ApproveContribution(Convert.ToInt32(e.CommandArgument), User.Identity.Name);

            int numResults = Convert.ToInt32(ddlPageSize.Text);
            Response.Redirect(GenerateQueryString(PageIndex, SortExpression, SortDirection, numResults, cbApproved.Checked));
        }
		else if (e.CommandName == "delete")
		{
			Admin.DeleteContribution(Convert.ToInt32(e.CommandArgument.ToString()));

			if (Session["PreviousPage"] != null) {
                Response.Redirect((string)Session["PreviousPage"]);
            }
            else
            {
                Response.Redirect("~/Admin/Contributions.aspx");
            }
		}
	}
    protected void cbApproved_CheckedChanged(object sender, EventArgs e)
    {
        int numResults = Convert.ToInt32(ddlPageSize.Text);

        Response.Redirect(GenerateQueryString(PageIndex, SortExpression, SortDirection, numResults, cbApproved.Checked));
    }

    protected override void updatePageSize(int numResults)
    {
        ddlPageSize.Text = numResults.ToString(); // update page dropdown
    }

    protected override void setPageIndex(int pageIndex)
    {
        // Don't allow setting for admin pages (arbitrary; just how the code was written)
    }

    protected override void setPageSize(int pageSize)
    {
        // Don't allow setting for admin pages (arbitrary; just how the code was written)
    }

    protected override int getPageIndex()
    {
        return Utils.GetIntFromQueryString(Request.QueryString["page"]);
    }

    protected override string getPageCategory()
    {
        return "Contributions";
    }

    private PagingControls mPageController = null;
}
