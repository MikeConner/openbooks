using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;
using System.Data;
using System.Data.SqlClient;

public partial class SearchExpendituresPage : System.Web.UI.Page
{
    public SearchParamsExpenditures sp;

	protected void Button1_Click(object sender, EventArgs e)
	{
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ""; // used in admin portion only
		string vendor = txtVendor.Text;
		string vendorSearchOptions = rblVendorSearchOptions.SelectedValue;
		string keywords = txtKeywords.Text;

		string queryString = SearchExpenditures.GenerateQueryString(candidateID, office, vendor, vendorSearchOptions, keywords);
		Response.Redirect(queryString);
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Initial sort image state
            //DAS clearImages();
            // Search
            GetSearchResults();
            LoadCandidates();
            txtVendor.Text = sp.vendorKeywords;
            txtKeywords.Text = sp.keywords;            
        }
    }
    private void LoadCandidates()
    {
        DataTable candidates = new DataTable();

        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID], [CandidateName] FROM [tlk_candidate] ORDER BY CandidateName ASC", con);
                adapter.Fill(candidates);

                ddlCandidateName.DataSource = candidates;
                ddlCandidateName.DataTextField = "CandidateName";
                ddlCandidateName.DataValueField = "ID";
                ddlCandidateName.DataBind();
            }
            catch (Exception ex)
            {
                // Handle the error
            }
        }

        // Add the initial item - you can add this even if the options from the
        // db were not successfully loaded
        ddlCandidateName.Items.Insert(0, new ListItem("All", "0"));
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        ddlCandidateName.Items.FindByValue(sp.candidateID.ToString()).Selected = true;
    }

    public void GetSearchResults()
    {
        // Get SearchParams Class from query string
        sp = SearchExpenditures.GetQueryStringValues(HttpContext.Current.Request);

        //Determine the Results Per Page from user
        SetPageSize();

        // Update Pager Results
        GetResultsCount(sp);

        // Fill DataTable from Search Results
        DataTable dt = SearchExpenditures.GetExpenditures(sp, PageIndex, PageSize, SortExpression, SortDirection);

        // Load repeater with data
        rptExpenditures.DataSource = dt;
        rptExpenditures.DataBind();
    }

    public void SetPageSize()
    {
        int numResults = Convert.ToInt32(ddlPageSize.Text);
        if (numResults == 10 || numResults == 25 || numResults == 50 || numResults == 100)
        {
            PageSize = numResults;
        }
    }

    public void GetResultsCount(SearchParamsExpenditures sp)
    {
        // Get total rows
        int totalRows = SearchExpenditures.GetExpendituresCount(sp);

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
            lblCurrentPage.Text = "We couldn't find any expenditures that matched your criteria.";
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
    private const string DEFAULTCOL = "ExpenditureID";
    private const string ASCENDING = "ASC";
    private const string DESCENDING = "DESC";
    private const string IMGDESC = "~/img/downarrow.gif";
    private const string IMGASC = "~/img/uparrow.gif";
    private const string IMGNOSORT = "~/img/placeholder.gif";

    // Sorting Controls
    /*
    public void clearImages()
    {
        imgSortCompany.ImageUrl = IMGNOSORT;
        imgSortCandidate.ImageUrl = IMGNOSORT;
        imgSortOffice.ImageUrl = IMGNOSORT;

        imgSortAmount.ImageUrl = IMGNOSORT;
        imgSortDate.ImageUrl = IMGNOSORT;
    }
    public void sortCompany(object sender, EventArgs e)
    {
        clearImages();

        if (SortExpression == "CompanyName")
        {
            if (SortDirection == ASCENDING)
            {
                SortDirection = DESCENDING;
                imgSortCompany.ImageUrl = IMGDESC;
            }
            else
            {
                SortDirection = ASCENDING;
                imgSortCompany.ImageUrl = IMGASC;
            }
        }
        else
        {
            SortExpression = "CompanyName";
            SortDirection = ASCENDING;
            imgSortCompany.ImageUrl = IMGASC;
        }

        GetSearchResults();
    }
    public void sortCandidate(object sender, EventArgs e)
    {
        clearImages();

        if (SortExpression == "CandidateID")
        {
            if (SortDirection == ASCENDING)
            {
                SortDirection = DESCENDING;
                imgSortCandidate.ImageUrl = IMGDESC;
            }
            else
            {
                SortDirection = ASCENDING;
                imgSortCandidate.ImageUrl = IMGASC;
            }
        }
        else
        {
            SortExpression = "CandidateID";
            SortDirection = ASCENDING;
            imgSortCandidate.ImageUrl = IMGASC;
        }

        GetSearchResults();
    }
    public void sortOffice(object sender, EventArgs e)
    {
        clearImages();

        if (SortExpression == "Office")
        {
            if (SortDirection == ASCENDING)
            {
                SortDirection = DESCENDING;
                imgSortOffice.ImageUrl = IMGDESC;
            }
            else
            {
                SortDirection = ASCENDING;
                imgSortOffice.ImageUrl = IMGASC;
            }
        }
        else
        {
            SortExpression = "Office";
            SortDirection = ASCENDING;
            imgSortOffice.ImageUrl = IMGASC;
        }

        GetSearchResults();
    }
    public void sortAmount(object sender, EventArgs e)
    {
        clearImages();

        if (SortExpression == "Amount")
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
            SortExpression = "Amount";
            SortDirection = ASCENDING;
            imgSortAmount.ImageUrl = IMGASC;
        }

        GetSearchResults();
    }
    public void sortDate(object sender, EventArgs e)
    {
        clearImages();

        if (SortExpression == "DatePaid")
        {
            if (SortDirection == ASCENDING)
            {
                SortDirection = DESCENDING;
                imgSortDate.ImageUrl = IMGDESC;
            }
            else
            {
                SortDirection = ASCENDING;
                imgSortDate.ImageUrl = IMGASC;
            }
        }
        else
        {
            SortExpression = "DatePaid";
            SortDirection = ASCENDING;
            imgSortDate.ImageUrl = IMGASC;
        }

        GetSearchResults();
    }
    */

    /* Page Actions */
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Reset Page Index
        PageIndex = 0;

        // Reload Search
        GetSearchResults();
    }
}
