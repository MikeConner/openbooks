using System;
using System.Collections.Generic;
using System.Net;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OpenBookPgh;
using System.Data.SqlClient;

public partial class SearchContractsPage : System.Web.UI.Page
{
    public SearchRangeParamsContract sp;

    // .NET 2.0 doesn't support ClientIDMode, and name mangling (which JS needs to match) depends on page settings, which can vary
    //   between 
    public int maxContractAmount;
    public int stickyMinContract;
    public int stickyMaxContract;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Initial sort image state
            initializeSorting();
            
            ((Label)Master.FindControl("FlashErrorMessage")).Text = "";

            // Search
            GetSearchResults();
            LoadDepartments();
            LoadContractTypes();
            Vendor.Text = sp.vendorKeywords;
            Keywords.Text = sp.keywords;
            stickyMinContract = sp.minContractAmt;
            stickyMaxContract = sp.maxContractAmt;
        }

        maxContractAmount = SearchContracts.GetMaxContractPrice();
    }
    private void LoadDepartments()
    {
        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [DeptCode], [DeptName] FROM [tlk_department] ORDER BY DeptName", con);
                adapter.Fill(dt);

                CityDepartment.DataSource = dt;
                CityDepartment.DataTextField = "DeptName";
                CityDepartment.DataValueField = "DeptCode";
                CityDepartment.DataBind();
            }
            catch (Exception ex)
            {
                // Handle the error
            }
        }

        // Add the initial item - you can add this even if the options from the
        // db were not successfully loaded
        CityDepartment.Items.Insert(0, new ListItem("All Organizations", "0"));
    }
    private void LoadContractTypes()
    {
        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID], [ServiceName] FROM [tlk_service] ORDER BY ServiceName", con);
                adapter.Fill(dt);

                ContractType.DataSource = dt;
                ContractType.DataTextField = "ServiceName";
                ContractType.DataValueField = "ID";
                ContractType.DataBind();
            }
            catch (Exception ex)
            {
                // Handle the error
            }
        }

        // Add the initial item - you can add this even if the options from the
        // db were not successfully loaded
        ContractType.Items.Insert(0, new ListItem("All Services", "0"));
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (sp != null)
        {
            CityDepartment.Items.FindByValue(sp.cityDept.ToString()).Selected = true;
            ContractType.Items.FindByValue(sp.contractType.ToString()).Selected = true;
        }
    }

	protected void btnSearch_Click(object sender, EventArgs e)
	{
        int minContractAmount = 0;
        int maxContractAmount = 0;
        // Pretty much can't fail, since it's a slider, so don't worry about exceptions
        // Remove commas, though
        Int32.TryParse(Request.Form["dblMinContract"].Replace(",",""), out minContractAmount);
        Int32.TryParse(Request.Form["dblMaxContract"].Replace(",", ""), out maxContractAmount);

        if (minContractAmount > maxContractAmount)
        {
            ((Label)Master.FindControl("FlashErrorMessage")).Text = "Min contract amount must be <= Max contract amount";
            return;
        }

        string startDate = Request.Form["dtmStart"];
        string endDate = Request.Form["dtmFinish"];
        DateTime startDT = string.IsNullOrEmpty(startDate) ? SearchRangeParamsContract.DEFAULT_START_DATE : Convert.ToDateTime(startDate);
        DateTime endDT = string.IsNullOrEmpty(endDate) ? DateTime.Today : Convert.ToDateTime(startDate);

        if (startDT > endDT)
        {
            ((Label)Master.FindControl("FlashErrorMessage")).Text = "Ending date must be >= starting date";
            return;
        }

        // Pretty much can't fail, since it's a dropdown, so don't worry about exceptions
        int cityDept = Convert.ToInt32(CityDepartment.SelectedValue);
        int contractType = Convert.ToInt32(ContractType.SelectedValue);

        string vendorKeywords = Vendor.Text;
        string searchKeywords = Keywords.Text;
        string keywordOptions = rbVendor.SelectedValue;

        string queryString = SearchContracts.GenerateRangeQueryString(0, 0, vendorKeywords, keywordOptions, cityDept, contractType, searchKeywords, startDate, endDate, minContractAmount, maxContractAmount);
		Response.Redirect(queryString);		
		//Response.Write(queryString);
    }

    // Page Load
    public void GetSearchResults()
    {
        // Get SearchParams Class from query string
        //SearchParamsContract sp = SearchContracts.GetQueryStringValues(HttpContext.Current.Request);
        sp = SearchContracts.GetRangeQueryStringValues(HttpContext.Current.Request);

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

    // Formerly SearchParamsContract
    public void GetResultsCount(SearchRangeParamsContract sp)
    {
        // Get total rows
        int totalRows = SearchContracts.GetContractsCount(sp);

        // Update PageCount for pager, using adjustment if necessary
        int addPage = 1;
        if ((totalRows % PageSize) == 0)
        {
            // Whatever this is supposed to do it doesn't work
            // addPage = 0;
        }
        PageCount = (totalRows / PageSize) + addPage;

        // Disable buttons if necessary
        if ((PageIndex == 0))
        {
            ibtnFirstPageTop.Enabled = !(PageIndex == 0);
            ibtnFirstPageTop.CssClass = "button prev";
            ibtnPrevPageTop.Enabled = !(PageIndex == 0);
            ibtnPrevPageTop.CssClass = "button prev";
        }

        if ((PageIndex >= PageCount - 1))
        {
            ibtnNextPageTop.Enabled = !(PageIndex >= PageCount - 1);
            ibtnNextPageTop.CssClass = "button prev";
            ibtnLastPageTop.Enabled = !(PageIndex >= PageCount - 1);
            ibtnLastPageTop.CssClass = "button prev";
        }
       

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
    public void initializeSorting()
    {
        imgSortDirection.ImageUrl = IMGDESC;
        SortDirection = DESCENDING;
        SortExpression = ddlSortContracts.SelectedValue;
    }

    public void toggleSortDirection(object sender, ImageClickEventArgs e)
    {
        if (SortDirection == ASCENDING)
        {
            SortDirection = DESCENDING;
            imgSortDirection.ImageUrl = IMGDESC;
        }
        else
        {
            SortDirection = ASCENDING;
            imgSortDirection.ImageUrl = IMGASC;
        }

        // Reload Search
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
 
    protected void ddlSortContracts_SelectedIndexChanged(object sender, EventArgs e)
    {
        SortExpression = ddlSortContracts.SelectedValue;

        // Reload Search
        GetSearchResults();
    }

    protected void rptContracts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int contractID = Convert.ToInt32(e.CommandArgument.ToString());

        if (e.CommandName == "ViewPDF")
        {
            OpenNewWindow("http://onbaseapp.city.pittsburgh.pa.us/PublicAccess/Contracts.aspx?OBKey__138_1=" + contractID);
        }
        else if (e.CommandName == "ViewCheck")
        {
            OpenNewWindow("http://onbaseapp.city.pittsburgh.pa.us/PublicAccess/Checks.aspx?OBKey__138_1=" + contractID);
        }
        else if (e.CommandName == "ViewInvoice")
        {
            OpenNewWindow("http://onbaseapp.city.pittsburgh.pa.us/PublicAccess/Invoices.aspx?OBKey__138_1=" + contractID);
        }
    }

    public void OpenNewWindow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));

    }
}
