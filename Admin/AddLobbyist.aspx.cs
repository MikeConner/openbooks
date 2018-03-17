using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using OpenBookPgh;

public partial class Admin_AddLobbyist : System.Web.UI.Page
{

	protected DataSet dsTemp = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			// Save previous page string
			if (Request.UrlReferrer != null)
			{
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
			}
			// Preload
			PreLoadDataSet();
        }
		else
		{
			// Load Temp DataSet
			dsTemp = (DataSet)Session["DATASET"];
		}
    }
    private void PreLoadDataSet()
	{
		// Create Temporary Data Set for Line Items GridView
		dsTemp = LoadDataSet();
		grvLineItems.DataSource = dsTemp;
		grvLineItems.DataBind();
		Session["DATASET"] = dsTemp;
	}
	private DataSet LoadDataSet()
	{
		DataSet ds = new DataSet("companies");
		DataTable dt = new DataTable("companies.lineitems");

		dt.Columns.Add("company");
		dt.Columns.Add("address");
		dt.Columns.Add("city");
		dt.Columns.Add("state");
		dt.Columns.Add("zip");
		
		ds.Tables.Add(dt);

		return ds;
	}
	protected void grvLineItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		DataTable dt = dsTemp.Tables["companies.lineitems"];
		DataRow dr = dt.Rows[e.RowIndex];
		dt.Rows.Remove(dr);

		grvLineItems.EditIndex = -1;
		grvLineItems.DataSource = dsTemp;
		grvLineItems.DataBind();
	}
	
	protected void btnAddLineItems_Click(object sender, EventArgs e)
	{
		string company = txtCompany.Text;
		string address = txtAddress.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
		string zip = txtZip.Text;

		DataTable dt = dsTemp.Tables["companies.lineitems"];
		DataRow dr = dt.NewRow();

			dr["company"] = company;
			dr["address"] = address;
			dr["city"] = city;
			dr["state"] = state;
			dr["zip"] = zip;
						
		dt.Rows.Add(dr);

		grvLineItems.DataSource = dsTemp;
		grvLineItems.DataBind();

		// Reset entry
		txtCompany.Text = "";
		txtAddress.Text = "";
		txtCity.Text = "";
		ddlState.SelectedValue = "";
		txtZip.Text = "";
		
	}
	protected void Button1_Click(object sender, EventArgs e)
	{
		// Add Lobbyist
		string lobbyist = txtLobbyist.Text;
		string position = txtPosition.Text;
		string employer = txtEmployer.Text;
		string empAddress = txtEmpAddress.Text;
		string empCity = txtEmpCity.Text;
		string empState = ddlEmpState.SelectedValue;
		string empZip = txtEmpZip.Text;
		string lobbyiststatus = txtLobbyistStatus.Text;
        bool forCity = cbForCity.Checked;

        // Add Lobbyist & Get LobbyistID
        int lobbyistID = Admin.AddLobbyist(lobbyist, position, employer, empAddress, empCity, empState, empZip, lobbyiststatus, forCity);
		
		// Add Additional Companies?
		DataTable lineitems = dsTemp.Tables["companies.lineitems"];
		if(lineitems.Rows.Count > 0)
		{
			for(int i = 0; i < lineitems.Rows.Count; i++)
			{
				DataRow row = lineitems.Rows[i];

				string company = row["company"].ToString();
				string address = row["address"].ToString();
				string city = row["city"].ToString();
				string state = row["state"].ToString();
				string zip = row["zip"].ToString();

				Admin.AddLobbyistCompany(lobbyistID, company, address, city, state, zip);
			}
		}
		Response.Redirect("Lobbyists.aspx");

	}
}
