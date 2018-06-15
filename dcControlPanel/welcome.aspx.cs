using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dcControlPanel.Method;

namespace dcControlPanel
{
  public partial class welcome : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
      HttpCookie cookie = new HttpCookie("uPanelControl");
      cookie.Values.Add("upanel", string.Empty);
      cookie.Values.Add("ppanel", string.Empty);
      cookie.Expires = DateTime.MaxValue;
      Response.Cookies.Add(cookie);

      Session["USUARIO"] = string.Empty;
      Session["AdministradorPanel"] = string.Empty;

      oWeb.ValidaSessionAdm();
    }
  }
}