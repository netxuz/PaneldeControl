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
  public partial class set_kpi_pages : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        CodCliente.Value = oWeb.GetData("CodCliente");
        LoadGrid();
      }
    }

    protected void LoadGrid()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppPages oAppPages = new cAppPages(ref oConn);
        gridKpiPage.DataSource = oAppPages.Get();
        gridKpiPage.DataBind();

        oConn.Close();
      }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("menu.aspx");
    }

    protected void gridKpiPage_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCodPage = gridKpiPage.SelectedDataKey.Value.ToString();
      string sUrl = "set_kpi_pages.aspx?CodPage=" + pCodPage;

      if (!string.IsNullOrEmpty(CodCliente.Value))
        sUrl = sUrl + "&CodCliente=" + CodCliente.Value;

      Response.Redirect(sUrl);
    }
  }
}