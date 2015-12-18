using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using OpenBookAllegheny;

public partial class Admin_EditLobbyist : System.Web.UI.Page
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
			PreLoadItems();
		}
		else
		{
			// Load Temp DataSet
			dsTemp = (DataSet)Session["DATASET"];
		}
	}
	private void PreLoadItems()
	{
		// TODO: 
		int lobbyistID = Convert.ToInt32(Request.QueryString["ID"].ToString());

		DataTable savedinfo = Admin.GetLobbyist(lobbyistID);
		if (savedinfo.Rows.Count > 0)
		{
			DataRow row = savedinfo.Rows[0];
			txtLobbyist.Text = row["FullName"].ToString();
			txtPosition.Text = row["Position"].ToString();
			txtEmployer.Text = row["EmployerName"].ToString();
			txtEmpAddress.Text = row["Address"].ToString();
			txtEmpCity.Text = row["City"].ToString();
			ddlEmpState.SelectedValue = row["State"].ToString();
			txtEmpZip.Text = row["Zip"].ToString();
			txtLobbyistStatus.Text = row["LobbyistStatus"].ToString();
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
		int lobbyistID = Convert.ToInt32(Request.QueryString["ID"].ToString());

		DataSet ds = new DataSet("companies");
		DataTable dt = new DataTable("companies.lineitems");

		dt.Columns.Add("company");
		dt.Columns.Add("address");
		dt.Columns.Add("city");
		dt.Columns.Add("state");
		dt.Columns.Add("zip");
		

		// Additional Companies
		DataTable companies = Admin.GetLobbyistCompanies(lobbyistID);
		if (companies.Rows.Count > 0)
		{
			for (int i = 0; i < companies.Rows.Count; i++)
			{
				DataRow row = companies.Rows[i];

				string company = row["companyName"].ToString();
				string address = row["address"].ToString();
				string city = row["city"].ToString();
				string state = row["state"].ToString();
				string zip = row["zip"].ToString();

				DataRow dr = dt.NewRow();
				dr["company"] = company;
				dr["address"] = address;
				dr["city"] = city;
				dr["state"] = state;
				dr["zip"] = zip;

				dt.Rows.Add(dr);
			}
		}
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
	}
	protected void Button1_Click(object sender, EventArgs e)
	{
		// Update Lobbyist
		string lobbyist = txtLobbyist.Text;
		string position = txtPosition.Text;
		string employer = txtEmployer.Text;
		string empAddress = txtEmpAddress.Text;
		string empCity = txtEmpCity.Text;
		string empState = ddlEmpState.SelectedValue;
		string empZip = txtEmpZip.Text;
		string lobbyiststatus = txtLobbyistStatus.Text;

		// Get LobbyistID
		int lobbyistID = Convert.ToInt32(Request.QueryString["ID"].ToString());

		// Update Lobbyist Employer info & Delete existing Additional Companies
		Admin.UpdateLobbyist(lobbyistID, lobbyist, position, employer, empAddress, empCity, empState, empZip, lobbyiststatus);


		// Add Additional Companies?
		DataTable lineitems = dsTemp.Tables["companies.lineitems"];
		if (lineitems.Rows.Count > 0)
		{

			for (int i = 0; i < lineitems.Rows.Count; i++)
			{
				DataRow row = lineitems.Rows[i];

				string company = row["company"].ToString();
				string address = row["address"].ToString();
				string city = row["city"].ToString();
				string state = row["state"].ToString();
				string zip = row["zip"].ToString();

				// Aditional Company records
				Admin.AddLobbyistCompany(lobbyistID, company, address, city, state, zip);
			}
		}

		// Redirect
		if (Session["PreviousPage"] != null)
			Response.Redirect((string)Session["PreviousPage"]);
		else
			Response.Redirect("~/Admin/Default.aspx");
		
	}

}
