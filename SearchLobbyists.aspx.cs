//DAS

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OpenBookPgh;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class SearchLobbyistsPage : System.Web.UI.Page
{
    public SearchParamsLobbyists sp;
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Keywords
        string lobbyistKeywords = txtLobbyist.Text;
        string employerKeywords = txtEmployer.Text;

        // If both set use only lobbyist keywords
        if (!string.IsNullOrEmpty(lobbyistKeywords) && !string.IsNullOrEmpty(employerKeywords))
        {
            employerKeywords = string.Empty;
        }
        // If both are empty, do nothing


        string url = SearchLobbyists.GenerateQueryString(0, lobbyistKeywords, employerKeywords);
        Response.Redirect(url + "&click=1");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            initializeSorting();

            GetSearchResults("1" == Request.QueryString["click"]);
            txtLobbyist.Text = sp.lobbyistKeywords;
            txtEmployer.Text = sp.companyKeywords;
        }
    }
    // Page Load
    public void GetSearchResults()
    {
        GetSearchResults(true);
    }

    public void GetSearchResults(bool executeSearch)
    {
        // Get SearchParams Class from query string
         sp = SearchLobbyists.GetQueryStringValues(HttpContext.Current.Request);

        // Update Pager Results
        GetResultsCount(sp);

        // Fill DataTable from Search Results
        DataTable searchDT = SearchLobbyists.GetLobbyists(sp, PageIndex, PageSize, SortExpression, SortDirection);

        // Fill DataTable of Additional Companies
        //DAS string xmlString = SearchLobbyists.GetXMLString(searchDT);
        //DAS DataTable companiesDT = SearchLobbyists.GetLobbyistsCompanies(xmlString);
        DataTable companiesDT = SearchLobbyists.GetLobbyistsCompanies(searchDT);

        if (executeSearch)
        {
            // Create DataSet, Add DataTables, Relate Tables
            DataSet ds = new DataSet("LobbyistsDS");
            ds.Tables.Add(searchDT);
            ds.Tables.Add(companiesDT);
            ds.Relations.Add(new DataRelation("LobbyistID", ds.Tables["Lobbyists"].Columns["LobbyistID"], ds.Tables["Companies"].Columns["LobbyistID"]));

            // Load repeater with data
            rptLobbyists.DataSource = ds.Tables["Lobbyists"];
            rptLobbyists.DataBind();
        }
    }

    public void GetResultsCount(SearchParamsLobbyists sp)
    {
        // Get total rows
        int totalRows = SearchLobbyists.GetLobbyistsCount(sp);

        // Update PageCount for pager, using adjustment if necessary
        int addPage = 1;
        if ((totalRows % PageSize) == 0)
        {
            addPage = 0;
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
            lblCurrentPage.Text = "We couldn't find any lobbyists that matched your criteria.";
        }
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
            ddlPageSize.Text = numResults.ToString(); // update page dropdown
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

    // Sort Constants
    /*
    public string SortExpression
    {
        get
        {
            string validList = "LobbyistID LobbyistName EmployerName DateEntered";
            string sortCategory = Utils.GetStringFromQueryString(Request.QueryString["cat"], validList);
            if (string.IsNullOrEmpty(sortCategory))
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
    }*/
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

    // Query determination
    public string QueryType
    {
        get
        {
            SearchParamsLobbyists sp = SearchLobbyists.GetQueryStringValues(HttpContext.Current.Request);
            string queryString = string.Empty;
            if (sp.lobbyistID != 0)
            {
                queryString = "id=" + sp.lobbyistID;
            }
            else if (!string.IsNullOrEmpty(sp.lobbyistKeywords))
            {
                queryString = "lobbyist=" + System.Web.HttpUtility.UrlEncode(sp.lobbyistKeywords);
            }
            else if (!string.IsNullOrEmpty(sp.companyKeywords))
            {
                queryString = "company=" + System.Web.HttpUtility.UrlEncode(sp.companyKeywords);
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
        return "SearchLobbyists.aspx?" + QueryType + "&page=" + page + "&cat=" + category + "&sort=" + sort + "&num=" + numResults;
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
    /*
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
    }*/

    private const string DEFAULTCOL = "LobbyistName";
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
        SortExpression = ddlSortLobbyists.SelectedValue;
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
        int page = 0; // reset page
        int numResults = Convert.ToInt32(ddlPageSize.Text);

        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, numResults));
    }
    protected void ddlSortLobbyists_SelectedIndexChanged(object sender, EventArgs e)
    {
        SortExpression = ddlSortLobbyists.SelectedValue;

        GetSearchResults();
    }
}
