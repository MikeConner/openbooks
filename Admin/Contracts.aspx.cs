using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OpenBookPgh;


public partial class Admin_Contracts_new : PaginatedPage
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

                // Load vendor drop down
                ddlVendors.DataSource = Admin.LoadVendors();
                ddlVendors.DataBind();
                // Load contract #s drop down
                ddlContractNos.DataSource = Admin.LoadContractNos();
                ddlContractNos.DataBind();

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
		// Vendor #
		string vendor = Utils.GetStringFromQueryString(Request.QueryString["vendorNo"], true);
		if (!String.IsNullOrEmpty(vendor))
		{
			ddlVendors.SelectedValue = vendor;
		}
		// Contract #
		int contractID = Utils.GetIntFromQueryString(Request.QueryString["contractNo"]);
		if (contractID != 0)
		{
			ddlContractNos.SelectedValue = contractID.ToString();
		}
	}
	public void GetSearchResults()
	{
		// Get SearchParams Class from query string
		SearchParamsContract sp = SearchContracts.GetQueryStringValues(HttpContext.Current.Request);

		// Update ddlPageSize
		LoadDropDowns();

		// Update Pager Results
		GetResultsCount(sp);

		// Fill DataTable from Search Results
		DataTable dt = SearchContracts.GetContracts(sp, PageIndex, PageSize, SortExpression, SortDirection);

		// Load repeater with data
		rptContracts.DataSource = dt;
		rptContracts.DataBind();
	}

	public void GetResultsCount(SearchParamsContract sp)
	{
		// Get total rows
		int totalRows = SearchContracts.GetContractsCount(sp);

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
		string str = (Request.QueryString["VendorNo"]?? "").Trim();
		// VendorNo search
		if (!string.IsNullOrEmpty(str))
			str = "VendorNo=" + str;
		// Standard Paging - no search
		else
			str = "q=";

		return str;
	}

	// Sorting Constants
	public string SortExpression { get { return GetSortExpression(Request.QueryString["cat"]); } }
	public string SortDirection { get { return GetSortDirection(Request.QueryString["sort"]); } }
	
	public string GetSortExpression(string str)
	{
		string validList = "ContractID VendorName DepartmentID Amount OriginalAmount DateSolicitor DateDuration Service DateCountersigned";
		// Remove leading and trailing spaces
		str = (str ?? "").Trim();

		// Check against known list of values supplied
		if (!validList.Contains(str) || string.IsNullOrEmpty(str))
		{
			str = "ContractID";
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
			str = "DESC";
		}
		return str;
	}

	public string GenerateQueryString(int page, string category, string sort, int numResults)
	{
		return "Contracts.aspx?" + QueryType + "&page=" + page + "&cat=" + category + "&sort=" + sort + "&num=" + numResults;
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
	public void sortVendor(object sender, EventArgs e)
	{
		string sort = "ASC";
		if(SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "VendorName", sort, PageSize));
		
	}
    public void sortAgency(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "DepartmentID", sort, PageSize));
	}
	public void sortContractID(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "ContractID", sort, PageSize));
	}
	public void sortAmount(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "Amount", sort, PageSize));
	}
	public void sortOriginalAmount(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "OriginalAmount", sort, PageSize));
	}
	public void sortDescription(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "Description", sort, PageSize));
	}
	public void sortEndDate(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "DateDuration", sort, PageSize));
	}
	public void sortContractType(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "Service", sort, PageSize));
	}
	public void sortApprovalDate(object sender, EventArgs e)
	{
		string sort = "ASC";
		if (SortDirection == "ASC")
			sort = "DESC";

		Response.Redirect(GenerateQueryString(PageIndex, "DateCountersigned", sort, PageSize));
	}
	
	/* Page Actions */
	protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		int page = 0; // reset page
		int numResults = Convert.ToInt32(ddlPageSize.Text);

		Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, numResults));
	}
	protected void ddlVendors_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect("Contracts.aspx?vendorNo=" + ddlVendors.SelectedValue.ToString() + "&page=0&cat=VendorNo");
	}
	protected void ddlContractNos_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect("Contracts.aspx?contractNo=" + ddlContractNos.SelectedValue.ToString());
	}
	protected void rptContracts_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { '@' });
		//var contractID = commandArgs[0];
		//var supplementalNo = commandArgs[1];
		string contractID = commandArgs[0].ToString();
		string supplementalNo = commandArgs[1].ToString();
		if (e.CommandName == "edit")
		{
			Response.Redirect("EditContract.aspx?id=" + contractID + "&sup=" + supplementalNo);
		}

		if (e.CommandName == "delete")
		{
			Admin.DeleteContract(contractID, Convert.ToInt32(supplementalNo));
			if (Session["PreviousPage"] != null) {
                Response.Redirect((string)Session["PreviousPage"]);
            }
            else
            {
                Response.Redirect("~/Admin/Contracts.aspx");
            }
		}
		if (e.CommandName == "view")
		{
			Response.Redirect("http://onbaseapp/publicaccess/Contracts.aspx?OBKey__138_1=" + e.CommandArgument.ToString());
		}
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
        return "Contracts";
    }

    private PagingControls mPageController = null;
}
