using System;
using System.Data;
using OpenBookPgh;

public partial class ElectedOfficials : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetPoliticians();
        }
    }

    public void GetPoliticians()
    {
        DataTable searchPols = SearchElectedOfficials.GetPoliticians(SortExpression, SortDirection);

        DataSet ds = new DataSet("OfficialsDS");
        ds.Tables.Add(searchPols);

        rptOfficials.DataSource = ds.Tables["ElectedOfficials"];
        rptOfficials.DataBind();
    }

    public string WrapFilename(string fname)
    {
        return "img\\politicians\\" + fname;
    }

    // Sort Constants
    public string SortExpression
    {
        get
        {
            string validList = "Name Salary";
            string sortCategory = Utils.GetStringFromQueryString(Request.QueryString["cat"], validList);
            if (string.IsNullOrEmpty(sortCategory))
            {
                sortCategory = "Salary";
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

    public void sortOfficial(object sender, EventArgs e)
    {
        string var = "ASC";
        if (SortDirection == "ASC")
        {
            var = "DESC";
        }

        Response.Redirect(GenerateQueryString("Name", var));
    }

    public void sortSalary(object sender, EventArgs e)
    {
        string var = "ASC";
        if (SortDirection == "ASC")
        {
            var = "DESC";
        }

        Response.Redirect(GenerateQueryString("Salary", var));
    }

    public string GenerateQueryString(string category, string sort)
    {
        return "ElectedOfficials.aspx?" + "&cat=" + category + "&sort=" + sort;
    }
}