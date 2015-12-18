using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using OpenBookAllegheny;

public partial class Admin_Vendors : PaginatedPage
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
                LoadPage();
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

	private void LoadPage()
	{
		// Load vendor drop down
		ddlVendors.DataSource = Admin.LoadVendors();
		ddlVendors.DataBind();
	}
	
	protected void rptVendors_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "edit")
		{
			Response.Redirect("EditVendor.aspx?id=" + e.CommandArgument.ToString());
		}

	}
	protected void ddlVendors_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect("Vendors.aspx?id=" + ddlVendors.SelectedValue.ToString());
	}


	public void GetSearchResults()
	{
		// Get SearchParams Class from query string
		SearchParamsVendor sp = SearchVendors.GetQueryStringValues(HttpContext.Current.Request);

		// Update Pager Results
		GetResultsCount(sp);

		// Fill DataTable from Search Results
		DataTable dt = SearchVendors.GetVendors(sp, PageIndex, PageSize, SortExpression, SortDirection);

		// Load repeater with data
		rptVendors.DataSource = dt;
		rptVendors.DataBind();
	}
	public void GetResultsCount(SearchParamsVendor sp)
	{
		// Get total rows
		int totalRows = SearchVendors.GetVendorsCount(sp);

        mPageController.setPageCount(totalRows);

        lblCurrentPage.Text = mPageController.getPagingBanner();

        // Disable buttons if necessary
        ibtnFirstPageTop.Enabled = ibtnPrevPageTop.Enabled = PageIndex > 0;
        ibtnNextPageTop.Enabled = ibtnLastPageTop.Enabled = PageIndex < PageCount - 1;
	}


	// Sorting Constants
	public string SortExpression { get { return GetSortExpression(Request.QueryString["cat"]); } }
	public string SortDirection { get { return GetSortDirection(Request.QueryString["sort"]); } }

	public string GetSortExpression(string str)
	{
		string validList = "VendorName VendorNo";
		// Remove leading and trailing spaces
		str = (str ?? "").Trim();

		// Check against known list of values supplied
		if (!validList.Contains(str) || string.IsNullOrEmpty(str))
		{
			str = "VendorName";
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


	public string GenerateQueryString(int page, int size, string category, string sort)
	{
		return "Vendors.aspx?page=" + page + "&cat=" + category + "&num=" + size + "&sort=" + sort;
	}
	// Pager Controls
	protected void FirstPage_Click(object sender, EventArgs e)
	{
		// Send the user to the first page 
		int page = 0;
        Response.Redirect(GenerateQueryString(page, PageSize, SortExpression, SortDirection));
	}
	protected void PrevPage_Click(object sender, EventArgs e)
	{
		// Send the user to the previous page 
		int page = PageIndex - 1;
        Response.Redirect(GenerateQueryString(page, PageSize, SortExpression, SortDirection));
	}
	protected void NextPage_Click(object sender, EventArgs e)
	{
		// Send the user to the next page 
		int page = PageIndex + 1;
        Response.Redirect(GenerateQueryString(page, PageSize, SortExpression, SortDirection));
	}
	protected void LastPage_Click(object sender, EventArgs e)
	{
		// Send the user to the last page 
		int page = PageCount - 1;
		Response.Redirect(GenerateQueryString(page, PageSize, SortExpression, SortDirection));
	}

	// Sorting Controls
	public void sortVendorName(object sender, EventArgs e)
	{
		string sort = (SortDirection == "ASC") ? "ASC" : "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, PageSize, "VendorName", sort));
	}
	public void sortVendorNo(object sender, EventArgs e)
	{
        string sort = (SortDirection == "ASC") ? "ASC" : "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, PageSize, "VendorNo", sort));
	}

    protected void ddlSelection_Changed(object sender, EventArgs e)
    {
        int page = 0; // reset page
        int newPageSize = Convert.ToInt32(ddlPageSize.Text);
        string sort = (SortDirection == "ASC") ? "ASC" : "DESC";

        Response.Redirect(GenerateQueryString(page, newPageSize, "VendorName", sort));
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
        return "Vendors";
    }

    private PagingControls mPageController = null;
}
