using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OpenBookPgh;


public partial class Admin_Contracts_new : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (Request.UrlReferrer != null)
			{
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
			}
				
			// Load vendor drop down
			ddlVendors.DataSource = Admin.LoadVendors();
			ddlVendors.DataBind();
			// Load contract #s drop down
			ddlContractNos.DataSource = Admin.LoadContractNos();
			ddlContractNos.DataBind();

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
			lblCurrentPage.Text = "We couldn't find any contracts that matched your criteria.";
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
			Admin.DeleteContract(Convert.ToInt32(contractID), Convert.ToInt32(supplementalNo));
			if (Session["PreviousPage"] != null)
				Response.Redirect((string)Session["PreviousPage"]);
			else
				Response.Redirect("~/Admin/Default.aspx");
		}
		if (e.CommandName == "view")
		{
			Response.Redirect("http://onbaseapp/publicaccess/Contracts.aspx?OBKey__138_1=" + e.CommandArgument.ToString());
		}
	}
}
