using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OpenBookPgh;

public partial class Admin_Vendors : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        {
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);

			GetSearchResults();
			
			LoadPage();

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
		int startResults = (PageIndex * PageSize) + 1;
		int endResults = (PageIndex * PageSize) + PageSize;

		if (endResults > totalRows)
		{
			endResults = totalRows;
		}
		lblCurrentPage.Text = "Results: " + startResults.ToString() + " - " + endResults.ToString() + " of " + totalRows.ToString();
	}


	//Pager Constants
	public int PageIndex { get { return Utils.GetIntFromQueryString(Request.QueryString["page"]); } }
	public int PageSize { get { return 10; } }
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


	public string GenerateQueryString(int page, string category, string sort)
	{
		return "Vendors.aspx?page=" + page + "&cat=" + category + "&sort=" + sort;
	}
	// Pager Controls
	protected void FirstPage_Click(object sender, EventArgs e)
	{
		// Send the user to the first page 
		int page = 0;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection));
	}
	protected void PrevPage_Click(object sender, EventArgs e)
	{
		// Send the user to the previous page 
		int page = PageIndex - 1;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection));
	}
	protected void NextPage_Click(object sender, EventArgs e)
	{
		// Send the user to the next page 
		int page = PageIndex + 1;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection));
	}
	protected void LastPage_Click(object sender, EventArgs e)
	{
		// Send the user to the last page 
		int page = PageCount - 1;
		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection));
	}

	// Sorting Controls
	public void sortVendorName(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "VendorName", sort));
	}
	public void sortVendorNo(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "VendorNo", sort));
	}


}
