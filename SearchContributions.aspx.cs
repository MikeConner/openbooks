using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;
using System.Data;
using System.Data.SqlClient;

public partial class SearchContributionsPage : PaginatedPage
{
    public SearchParamsContribution sp;

    public SearchContributionsPage()
    {
        mPageController = new PagingControls(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Initial sort image state
            initializeSorting();

            // Search
            GetSearchResults("1" == Request.QueryString["click"]);
            LoadCandidates();
            if (sp.office != null)
            {
                ddlOffice.Items.FindByValue(sp.office.ToString()).Selected = true;
            }
            if (sp.contributorSearchOptions != null)
            {
                rblContributorSearch.SelectedValue = sp.contributorSearchOptions.ToString();
                txtContributor.Text = sp.contributorKeywords.ToString();
            }
            LoadYears();
            if (sp.employerKeywords != null)
            {
                txtEmployer.Text = sp.employerKeywords.ToString();
            }
            if (sp.zip != null)
            {
                txtZip.Text = sp.zip.ToString();
            }
        }
    }
    private void LoadYears()
    {
        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [yearName], [yearValue] FROM [tlk_year]", con);
                adapter.Fill(dt);

                ddldateContribution.DataSource = dt;
                ddldateContribution.DataTextField = "yearName";
                ddldateContribution.DataValueField = "yearValue";
                ddldateContribution.DataBind();
            }
            catch (Exception ex)
            {
                // Handle the error
            }
        }

        // Add the initial item - you can add this even if the options from the
        // db were not successfully loaded
        ddldateContribution.Items.Insert(0, new ListItem("All Years", "0"));
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
        ddldateContribution.Items.FindByValue(sp.dateContribution.ToString()).Selected = true;
        ddlDistance.Items.FindByValue(sp.radius.ToString()).Selected = true;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Distance calc
        double distance = Convert.ToDouble(ddlDistance.SelectedValue);
        string zip = txtZip.Text;

        int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
        string office = ddlOffice.SelectedValue;
        int year1 = 0;
        Int32.TryParse(ddldateContribution.SelectedValue, out year1);
        string contributorSearchOptions = rblContributorSearch.SelectedValue;
        string contributorKeywords = txtContributor.Text;
        string employerKeywords = txtEmployer.Text;

        string queryString = SearchContributions.GenerateQueryString(candidateID, office, year1,
        contributorKeywords, contributorSearchOptions, employerKeywords, zip, distance);

        Response.Redirect(queryString + "&click=1");
    }

    public void GetSearchResults()
    {
        GetSearchResults(true);
    }

    public void GetSearchResults(bool executeSearch)
    {
        // Get SearchParams Class from query string
        sp = SearchContributions.GetQueryStringValues(HttpContext.Current.Request);
        if (sp.office != null)
        {
            ddlOffice.Items.FindByValue(sp.office).Selected = true;
        }
        //Determine the Results Per Page from user
        SetResultsPerPage();

        // Update Pager Results
        GetResultsCount(sp);

        if (executeSearch)
        {
            // Fill DataTable from Search Results
            DataTable dt = SearchContributions.GetContributions(sp, PageIndex, PageSize, SortExpression, SortDirection);

            // Load repeater with data
            rptContributions.DataSource = dt;
            rptContributions.DataBind();
        }
    }

    public void SetResultsPerPage()
    {
        int numResults = Convert.ToInt32(ddlPageSize.Text);
        if (numResults == 10 || numResults == 25 || numResults == 50 || numResults == 100)
        {
            PageSize = numResults;
        }
    }
    public void GetResultsCount(SearchParamsContribution sp)
    {
        // Get total rows
        int totalRows = SearchContributions.GetContributionsCount(sp);

        mPageController.setPageCount(totalRows);

        lblCurrentPage.Text = mPageController.getPagingBanner();

        // Disable buttons if necessary
        ibtnFirstPageTop.Enabled = ibtnPrevPageTop.Enabled = PageIndex > 0;
        ibtnNextPageTop.Enabled = ibtnLastPageTop.Enabled = PageIndex < PageCount - 1;
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
                return ASCENDING;
            else
                return (string)o;
        }
        set { ViewState["_SortDirection"] = value; }
    }
    private const string DEFAULTCOL = "ContributionID";
    private const string ASCENDING = "ASC";
    private const string DESCENDING = "DESC";
    private const string IMGDESC = "~/img/downarrow.gif";
    private const string IMGASC = "~/img/uparrow.gif";
    private const string IMGNOSORT = "~/img/placeholder.gif";

    // Sorting Controls
    public void initializeSorting()
    {
        imgSortDirection.ImageUrl = IMGASC;
        SortDirection = ASCENDING;
        SortExpression = ddlSortContributions.SelectedValue;
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

        PageIndex = 0;

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

    protected void ddlSortContributions_SelectedIndexChanged(object sender, EventArgs e)
    {
        SortExpression = ddlSortContributions.SelectedValue;

        SortDirection = (("Amount" == SortExpression) || ("DateContribution" == SortExpression)) ? DESCENDING : ASCENDING;
        imgSortDirection.ImageUrl = (DESCENDING == SortDirection) ? IMGDESC : IMGASC;

        PageIndex = 0;

        GetSearchResults();
    }

    protected override void updatePageSize(int numResults)
    {
        ddlPageSize.Text = numResults.ToString(); // update page dropdown
    }

    protected override string getPageCategory()
    {
        return "Contributions";
    }

    private PagingControls mPageController = null;
}
