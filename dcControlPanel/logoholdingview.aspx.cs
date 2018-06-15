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
  public partial class logoholdingview : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        oWeb.ValidaSessionAdm();
        LoadGrid();
      }
    }

    protected void LoadGrid()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cCliente oCliente = new cCliente(ref oConn);
        if (!string.IsNullOrEmpty(txtlogo.Text))
        {
          oCliente.SNombre = txtlogo.Text;
        }
        gridLogos.DataSource = oCliente.GetHolding();
        gridLogos.DataBind();

        oConn.Close();
      }
    }
    protected void gridLogos_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCodCliente = gridLogos.SelectedDataKey.Value.ToString();
      Response.Redirect("logocliente.aspx?CodCliente=" + pCodCliente + "&tp=H");
    }
    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("logoclienteview.aspx");
    }

    protected void gridLogos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      gridLogos.PageIndex = e.NewPageIndex;
      LoadGrid();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      LoadGrid();
    }
  }
}