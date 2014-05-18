using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OpenBookPgh;

public partial class SearchLobbyistsPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
	protected void btnSearch_Click(object sender, EventArgs e)
	{
		// Keywords
		string lobbyistKeywords = txtLobbyist.Text;
		string employerKeywords = txtEmployer.Text;
		
		// If both set use only lobbyist keywords
		if(!string.IsNullOrEmpty(lobbyistKeywords) && !string.IsNullOrEmpty(employerKeywords))
		{
			employerKeywords = string.Empty;
		}
		// If both are empty, do nothing
		
		
		string url = SearchLobbyists.GenerateQueryString(0, lobbyistKeywords, employerKeywords);
		Response.Redirect(url);
		
	}

}
