using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class Sync8c311ccb_4254_441c_bdc5_081d0a00be66 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Admin.SSHDownload();
    }
}