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
  public partial class WebForm1 : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        oWeb.ValidaUserPanel();
        LoadGrid();
      }
    }

    protected void LoadGrid()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppMonitorView oAppMonitorView = new cAppMonitorView(ref oConn);
        gridVistas.DataSource = oAppMonitorView.Get();
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
        //cAppMonitorView oAppMonitorView = new cAppMonitorView(ref oConn);
        //oAppMonitorView.CodMonitor = pCodMonitor;
        //DataTable dtMonitor = oAppMonitorView.Get();
        //if (dtMonitor != null) {
        //    if (dtMonitor.Rows.Count > 0) {
        //        if (string.IsNullOrEmpty(dtMonitor.Rows[0]["cod_cliente"].ToString())) {
        //            Session["indexcliente"] = 0;
        //         }
        //    }
        //}
        //dtMonitor = null;

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
            sUrlPage = sUrlPage + "&order=" + dtmPage.Rows[0]["order_page"].ToString();
          }
        }
        dtmPage = null;
        oConn.Close();
      }
      if (!string.IsNullOrEmpty(sUrlPage))
        Response.Redirect(sUrlPage);
    }

    protected void gridVistas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      gridVistas.PageIndex = e.NewPageIndex;
      LoadGrid();
    }
  }

}