using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using dcControlPanel.Conn;
using dcControlPanel.Method;
using dcControlPanel.Base;

namespace dcControlPanel
{
  public partial class monitoresclientes : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        CodCliente.Value = oWeb.GetData("CodCliente");
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
          oSysUsuario.CodUser = CodCliente.Value;
          DataTable dtUser = oSysUsuario.GetSysUsuario();
          if (dtUser != null)
          {
            Label1.Text = dtUser.Rows[0]["nom_user"].ToString() + " " + dtUser.Rows[0]["ape_user"].ToString();
          }
          dtUser = null;
        }
        oConn.Close();
        LoadGrid();
      }
    }

    protected void LoadGrid()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppVistasCliente oAppVistasCliente = new cAppVistasCliente(ref oConn);
        oAppVistasCliente.CodCliente = CodCliente.Value;
        oAppVistasCliente.NotIn = false;
        gridVistas.DataSource = oAppVistasCliente.Get();
        gridVistas.DataBind();

        oConn.Close();
      }
    }

    protected void gridVistas_SelectedIndexChanged(object sender, EventArgs e)
    {
      string sUrlPage = string.Empty;
      string pCodMonitor = gridVistas.SelectedDataKey.Value.ToString();
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
        oAptMonitorPages.CodMonitor = pCodMonitor;
        DataTable dtmPage = oAptMonitorPages.Get();
        if (dtmPage != null)
        {
          if (dtmPage.Rows.Count > 0)
          {
            if (string.IsNullOrEmpty(dtmPage.Rows[0]["cod_cliente"].ToString()))
            {
              Session["indexcliente"] = 0;
            }

            sUrlPage = dtmPage.Rows[0]["url_page"].ToString();
            sUrlPage = sUrlPage + "?codmonitor=" + pCodMonitor;
            sUrlPage = sUrlPage + "&codusuario=" + CodCliente.Value;
            sUrlPage = sUrlPage + "&order=" + dtmPage.Rows[0]["order_page"].ToString();
          }
        }
        dtmPage = null;
        oConn.Close();
      }
      Response.Redirect(sUrlPage);
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