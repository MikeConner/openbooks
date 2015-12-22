using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using OpenBookAllegheny;

public partial class SearchResults : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
            //Admin.UploadAlleghenyContracts(@"Contracts.csv");

			// Set Initial sort image state
			clearImages();
			// Search
			GetSearchResults();
		}
    }
	// Page Load
	public void GetSearchResults()
    {
		// Get SearchParams Class from query string
		//SearchParamsContract sp = SearchContracts.GetQueryStringValues(HttpContext.Current.Request);
        SearchRangeParamsContract sp = SearchContracts.GetRangeQueryStringValues(HttpContext.Current.Request);

		//Determine the Results Per Page from user
		SetPageSize();
		
		// Update Pager Results
		GetResultsCount(sp);

		// Fill DataTable from Search Results
		DataTable dt = SearchContracts.GetContracts(sp, PageIndex, PageSize, SortExpression, SortDirection);

		// Load repeater with data
		rptContracts.DataSource = dt;
		rptContracts.DataBind();
	}
	public void SetPageSize()
	{
		int numResults = Convert.ToInt32(ddlPageSize.Text);
		if (numResults == 10 || numResults == 25 || numResults == 50 || numResults == 100)
		{
			PageSize = numResults;
		}

	}

    // Argument formerly SearchParamsContract
	public void GetResultsCount(SearchRangeParamsContract sp)
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
        // If the count is evenly divisible and we're on the first page, make sure we start at 1!
        if ((0 == startResults) && (totalRows > 0))
        {
            startResults = 1;
        }
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
				return DESCENDING;
			else
				return (string)o;
		}
		set { ViewState["_SortDirection"] = value; }
	}
	private const string DEFAULTCOL = "ContractID";
	private const string ASCENDING = "ASC";
	private const string DESCENDING = "DESC";
	private const string IMGDESC = "~/img/downarrow.gif";
	private const string IMGASC = "~/img/uparrow.gif";
	private const string IMGNOSORT = "~/img/placeholder.gif";


	// Sorting Controls
	public void clearImages()
	{
		imgSortVendor.ImageUrl = IMGNOSORT;
		imgSortAgency.ImageUrl = IMGNOSORT;
		imgSortContractID.ImageUrl = IMGNOSORT;
		imgSortAmount.ImageUrl = IMGNOSORT;
		imgSortOriginalAmount.ImageUrl = IMGNOSORT;
		imgSortDescription.ImageUrl = IMGNOSORT;
		//imgSortStartDate.ImageUrl = IMGNOSORT;
		imgSortEndDate.ImageUrl = IMGNOSORT;
		imgSortContractType.ImageUrl = IMGNOSORT;
		imgSortApproval.ImageUrl = IMGNOSORT;
	}
	public void sortVendor(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "VendorName")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortVendor.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortVendor.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "VendorName";
			SortDirection = ASCENDING;
			imgSortVendor.ImageUrl = IMGASC;
		}

		GetSearchResults();
	}
	public void sortAgency(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "DepartmentID")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortAgency.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortAgency.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "DepartmentID";
			SortDirection = DESCENDING;
			imgSortAgency.ImageUrl = IMGDESC;
		}

		GetSearchResults();
	}
	public void sortContractID(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "contractID")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortContractID.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortContractID.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "contractID";
			SortDirection = DESCENDING;
			imgSortContractID.ImageUrl = IMGDESC;
		}

		GetSearchResults();
	}
	public void sortAmount(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "amount")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortAmount.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortAmount.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "amount";
			SortDirection = DESCENDING;
			imgSortAmount.ImageUrl = IMGDESC;
		}

		GetSearchResults();
	}
	public void sortOriginalAmount(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "OriginalAmount")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortOriginalAmount.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortOriginalAmount.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "OriginalAmount";
			SortDirection = DESCENDING;
			imgSortOriginalAmount.ImageUrl = IMGDESC;
		}

		GetSearchResults();
	}
	public void sortDescription(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "Description")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortDescription.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortDescription.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "Description";
			SortDirection = ASCENDING;
			imgSortDescription.ImageUrl = IMGASC;
		}

		GetSearchResults();
	}
	public void sortEndDate(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "DateDuration")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortEndDate.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortEndDate.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "DateDuration";
			SortDirection = DESCENDING;
			imgSortEndDate.ImageUrl = IMGDESC;
		}

		GetSearchResults();
	}
	public void sortContractType(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "Service")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortContractType.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortContractType.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "Service";
			SortDirection = DESCENDING;
			imgSortContractType.ImageUrl = IMGDESC;
		}

		GetSearchResults();
	}
	public void sortApprovalDate(object sender, EventArgs e)
	{
		clearImages();

		if (SortExpression == "DateCountersigned")
		{
			if (SortDirection == ASCENDING)
			{
				SortDirection = DESCENDING;
				imgSortApproval.ImageUrl = IMGDESC;
			}
			else
			{
				SortDirection = ASCENDING;
				imgSortApproval.ImageUrl = IMGASC;
			}
		}
		else
		{
			SortExpression = "DateCountersigned";
			SortDirection = DESCENDING;
			imgSortApproval.ImageUrl = IMGDESC;
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
	protected void rptContracts_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "ViewPDF")
		{
			int contractID = Convert.ToInt32(e.CommandArgument.ToString());
			OpenNewWindow("http://onbaseapp.city.pittsburgh.pa.us/PublicAccess/Contracts.aspx?OBKey__138_1=" + contractID);
			//Response.Redirect("http://onbaseapp.city.pittsburgh.pa.us/PublicAccess/Contracts.aspx?OBKey__138_1=" + contractID);
		}
	}
	public void OpenNewWindow(string url)
	{

		ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));

	}
}
