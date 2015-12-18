//DAS

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OpenBookAllegheny;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class SearchLobbyistsPage : PaginatedPage
{
    public SearchParamsLobbyists sp;

    public SearchLobbyistsPage()
    {
        mPageController = new PagingControls(this);
    }

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
        Response.Redirect(url + "&page=0&cat=LobbyistName&sort=ASC&num=10&click=1");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            initializeSorting();

            GetSearchResults("1" == Request.QueryString["click"]);
            txtLobbyist.Text = sp.lobbyistKeywords;
            txtEmployer.Text = sp.companyKeywords;
            ddlSortLobbyists.SelectedValue = Request.QueryString["cat"];
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
        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize) + "&click=1");
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        // Send the user to the previous page 
        int page = PageIndex - 1;
        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize) + "&click=1");
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        // Send the user to the next page 
        int page = PageIndex + 1;
        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize) + "&click=1");
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        // Send the user to the last page 
        int page = PageCount - 1;
        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, PageSize) + "&click=1");
    }

    private const string DEFAULTCOL = "LobbyistName";
    private const string ASCENDING = "ASC";
    private const string DESCENDING = "DESC";
    private const string IMGDESC = "~/img/downarrow.gif";
    private const string IMGASC = "~/img/uparrow.gif";
    private const string IMGNOSORT = "~/img/placeholder.gif";

    // Sorting Controls
    public void initializeSorting()
    {
        SortExpression = Request.QueryString["cat"];
        SortDirection = Request.QueryString["sort"];

        imgSortDirection.ImageUrl = (ASCENDING == SortDirection) ? IMGASC : IMGDESC;
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
        Response.Redirect(GenerateQueryString(0, SortExpression, SortDirection, PageSize) + "&click=1");
    }


    /* Page Actions */
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        int page = 0; // reset page
        int numResults = Convert.ToInt32(ddlPageSize.Text);

        Response.Redirect(GenerateQueryString(page, SortExpression, SortDirection, numResults) + "&click=1");
    }

    protected void ddlSortLobbyists_SelectedIndexChanged(object sender, EventArgs e)
    {
        SortExpression = ddlSortLobbyists.SelectedValue;
        SortDirection = ("DateEntered" == SortExpression) ? DESCENDING : ASCENDING;

        Response.Redirect(GenerateQueryString(0, SortExpression, SortDirection, PageSize) + "&click=1");
    }

    protected override void updatePageSize(int numResults)
    {
        ddlPageSize.Text = numResults.ToString(); // update page dropdown
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
