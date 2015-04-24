using System;
using System.Collections.Generic;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OpenBookPgh;


public partial class Admin_Lobbyists : PaginatedPage
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
	public void GetSearchResults()
	{
		// Get SearchParams Class from query string
		SearchParamsLobbyists sp = SearchLobbyists.GetQueryStringValues(HttpContext.Current.Request);

        // Update Dropdowns to show # per page, & office or candidate filtered on
        LoadDropDowns();
		
		// Update Pager Results
		GetResultsCount(sp);

		// Fill DataTable from Search Results
		DataTable searchDT = SearchLobbyists.GetLobbyists(sp, PageIndex, PageSize, SortExpression, SortDirection);
		
		// Fill DataTable of Additional Companies
		//DAS string xmlString = SearchLobbyists.GetXMLString(searchDT);
		//DAS DataTable companiesDT = SearchLobbyists.GetLobbyistsCompanies(xmlString);
        DataTable companiesDT = SearchLobbyists.GetLobbyistsCompanies(searchDT);

		// Create DataSet, Add DataTables, Relate Tables
		DataSet ds = new DataSet("LobbyistsDS");
		ds.Tables.Add(searchDT);
		ds.Tables.Add(companiesDT);
		ds.Relations.Add(new DataRelation("LobbyistID", ds.Tables["Lobbyists"].Columns["LobbyistID"], ds.Tables["Companies"].Columns["LobbyistID"]));

		// Load repeater with data
		rptLobbyists.DataSource = ds.Tables["Lobbyists"];
		rptLobbyists.DataBind();
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
        // Lobbyist
        int lobbyistID = Utils.GetIntFromQueryString(Request.QueryString["id"]);
        if (lobbyistID != 0)
        {
            ddlLobbyists.SelectedValue = lobbyistID.ToString();
        }

        // Company
        string company = Utils.GetStringFromQueryString(Request.QueryString["company"], true);
        if (!String.IsNullOrEmpty(company))
        {
            txtCompanySearch.Text = company;
        }
    }	

	public void GetResultsCount(SearchParamsLobbyists sp)
	{
		// Get total rows
		int totalRows = SearchLobbyists.GetLobbyistsCount(sp);

        mPageController.setPageCount(totalRows);

        lblCurrentPage.Text = mPageController.getPagingBanner();

        // Disable buttons if necessary
        ibtnFirstPageTop.Enabled = ibtnPrevPageTop.Enabled = PageIndex > 0;
        ibtnNextPageTop.Enabled = ibtnLastPageTop.Enabled = PageIndex < PageCount - 1;
    }

	protected void rptLobbyists_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			DataRowView drv = e.Item.DataItem as DataRowView;
			Repeater rptCompanies = e.Item.FindControl("rptCompanies") as Repeater;
			rptCompanies.DataSource = drv.CreateChildView("LobbyistID");
			rptCompanies.DataBind();
		}
	}

	// Sort Constants
    public string SortExpression
    {
		get
		{
			string validList = "LobbyistID LobbyistName EmployerName DateEntered";
			string sortCategory = Utils.GetStringFromQueryString(Request.QueryString["cat"], validList);
			if(string.IsNullOrEmpty(sortCategory))
			{
				sortCategory = "LobbyistID";
			}
			return sortCategory;
		}
    }
    public string SortDirection
    {
		get
		{
			string validList = "ASC DESC";
			string sortDirection = Utils.GetStringFromQueryString(Request.QueryString["sort"], validList);
			if (string.IsNullOrEmpty(sortDirection))
			{
				sortDirection = "DESC";
			}
			return sortDirection;
		}
	}

	// Query determination
	public string QueryType 
	{
		get 
		{
			SearchParamsLobbyists sp = SearchLobbyists.GetQueryStringValues(HttpContext.Current.Request);
			string queryString = string.Empty;
			if(sp.lobbyistID != 0)
			{
				queryString = "id=" + sp.lobbyistID;
			}
			if(!string.IsNullOrEmpty(sp.lobbyistKeywords))
			{
				queryString += "&lobbyist=" + System.Web.HttpUtility.UrlEncode(sp.lobbyistKeywords);
			}
			else if (!string.IsNullOrEmpty(sp.companyKeywords))
			{
				queryString += "&company=" + System.Web.HttpUtility.UrlEncode(sp.companyKeywords);
			}
			else
			{
				// DO NOTHING, Browse Query
			}

			return queryString; 
		}
	}
	
    public string GenerateQueryString(int page, string category, string sort, int numResults)
    {
        return "Lobbyists.aspx?" + QueryType + "&page=" + page + "&cat=" + category + "&sort=" + sort + "&num=" + numResults;
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
	public void sortLobbyist(object sender, EventArgs e)
	{
		string var = "ASC";
		if (SortDirection == "ASC")
			var = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "LobbyistName", var, PageSize));
	}
	public void sortEmployer(object sender, EventArgs e)
	{
		string var = "ASC";
		if (SortDirection == "ASC")
			var = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "EmployerName", var, PageSize));
	}
	public void sortDate(object sender, EventArgs e)
	{
		string var = "ASC";
		if (SortDirection == "ASC")
			var = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "DateEntered", var, PageSize));
	}

/* Page Actions */
	protected void rptLobbyists_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "edit")
		{
			Response.Redirect("EditLobbyist.aspx?id=" + e.CommandArgument.ToString());
		}

		if (e.CommandName == "delete")
		{
			Admin.DeleteLobbyist(Convert.ToInt32(e.CommandArgument.ToString()));
			//if (Session["PreviousPage"] != null)
			//    Response.Redirect((string)Session["PreviousPage"]);
			//else
			//    Response.Redirect("~/Admin/Default.aspx");
			GetSearchResults();
		}
	}

	protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		int page = 0; // reset page
		int numResults = Convert.ToInt32(ddlPageSize.Text);

		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, numResults));
	}
    /*
	protected void ddlLobbyists_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect("Lobbyists.aspx?id=" + ddlLobbyists.SelectedValue.ToString() + "&page=0&cat=LobbyistID");
	}
    */
	protected void btnSearch_Click(object sender, EventArgs e)
	{
        Response.Redirect(SearchLobbyists.GenerateAdminQueryString(Convert.ToInt32(ddlLobbyists.SelectedValue.ToString()), txtCompanySearch.Text));
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
        return "Lobbyists";
    }

    private PagingControls mPageController = null;
}


