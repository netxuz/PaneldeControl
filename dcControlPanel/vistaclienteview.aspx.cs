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
  public partial class vistaclienteview : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        LoadGrid();
      }
    }
    protected void LoadGrid()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
        if (!string.IsNullOrEmpty(txtNombreApellido.Text))
          oSysUsuario.NomApeUser = txtNombreApellido.Text;
        oSysUsuario.EstUser = "V";
        gridViewClientes.DataSource = oSysUsuario.GetSysUsuario();
        gridViewClientes.DataBind();

        oConn.Close();
      }
    }
    protected void gridViewClientes_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCodCliente = gridViewClientes.SelectedDataKey.Value.ToString();
      Response.Redirect("vistaclienteconfig.aspx?CodCliente=" + pCodCliente);
    }
    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("menu.aspx");
    }

    protected void gridViewClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      gridViewClientes.PageIndex = e.NewPageIndex;
      LoadGrid();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      LoadGrid();
    }
  }
}