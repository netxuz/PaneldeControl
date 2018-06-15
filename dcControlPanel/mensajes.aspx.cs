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
  public partial class mensajes : System.Web.UI.Page
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
        cAppMensaje oAppMensaje = new cAppMensaje(ref oConn);
        gridMensajes.DataSource = oAppMensaje.Get();
        gridMensajes.DataBind();

        oConn.Close();
      }
    }



    protected void gridMensajes_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCodMensaje = gridMensajes.SelectedDataKey.Value.ToString();
      Response.Redirect("mensajesconfig.aspx?CodMensaje=" + pCodMensaje);
    }

    protected void btnCrear_Click(object sender, EventArgs e)
    {
      Response.Redirect("mensajesconfig.aspx");
    }

    protected void gridMensajes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      try
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          string pCodMensaje = gridMensajes.DataKeys[e.RowIndex].Value.ToString();

          cAptMonitorMensaje oAptMonitorMensaje = new cAptMonitorMensaje(ref oConn);
          oAptMonitorMensaje.CodMensaje = pCodMensaje;
          oAptMonitorMensaje.Accion = "ELIMINAR";
          oAptMonitorMensaje.Put();

          if (!string.IsNullOrEmpty(oAptMonitorMensaje.Error))
          {
            Response.Write(oAptMonitorMensaje.Error);
            Response.End();
          }

          cAppMensaje oAppMensaje = new cAppMensaje(ref oConn);
          oAppMensaje.CodMensaje = pCodMensaje;
          oAppMensaje.Accion = "ELIMINAR";
          oAppMensaje.Put();

          if (!string.IsNullOrEmpty(oAppMensaje.Error))
          {
            Response.Write(oAppMensaje.Error);
            Response.End();
          }

          oConn.Close();
        }
      }
      catch (Exception ex)
      {
        Response.Write(ex.Message + "; " + ex.InnerException.ToString());
      }
      LoadGrid();
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("menu.aspx");
    }

    protected void gridMensajes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      gridMensajes.PageIndex = e.NewPageIndex;
      LoadGrid();
    }
  }
}