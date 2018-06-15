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
  public partial class MonitorView : System.Web.UI.Page
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
        cAppMonitorView oAppMonitorView = new cAppMonitorView(ref oConn);
        gridPages.DataSource = oAppMonitorView.Get();
        gridPages.DataBind();

        oConn.Close();
      }
    }

    protected void btnCrear_Click(object sender, EventArgs e)
    {
      Response.Redirect("monitorviewconfig.aspx");
    }

    protected void gridPages_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      try
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          string pCodMonitor = gridPages.DataKeys[e.RowIndex].Value.ToString();

          cAptMonitorMensaje oAptMonitorMensaje = new cAptMonitorMensaje(ref oConn);
          oAptMonitorMensaje.CodMonitor = pCodMonitor;
          oAptMonitorMensaje.Accion = "ELIMINAR";
          oAptMonitorMensaje.Put();

          if (!string.IsNullOrEmpty(oAptMonitorMensaje.Error))
          {
            Response.Write(oAptMonitorMensaje.Error);
            Response.End();
          }

          cAptClientesMonitorpages oAptClientesMonitorpages = new cAptClientesMonitorpages(ref oConn);
          oAptClientesMonitorpages.CodMonitor = pCodMonitor;
          oAptClientesMonitorpages.Accion = "ELIMINAR";
          oAptClientesMonitorpages.Put();
          if (!string.IsNullOrEmpty(oAptClientesMonitorpages.Error))
          {
            Response.Write(oAptClientesMonitorpages.Error);
            Response.End();
          }

          cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
          oAptMonitorPages.CodMonitor = pCodMonitor;
          oAptMonitorPages.Accion = "ELIMINAR";
          oAptMonitorPages.Put();

          if (!string.IsNullOrEmpty(oAptMonitorPages.Error))
          {
            Response.Write(oAptMonitorPages.Error);
            Response.End();
          }

          cAppVistasCliente oAppVistasCliente = new cAppVistasCliente(ref oConn);
          oAppVistasCliente.CodMonitor = pCodMonitor;
          oAppVistasCliente.Accion = "ELIMINAR";
          oAppVistasCliente.Put();

          if (!string.IsNullOrEmpty(oAppVistasCliente.Error))
          {
            Response.Write(oAppVistasCliente.Error);
            Response.End();
          }

          cAppMonitorView oAppMonitorView = new cAppMonitorView(ref oConn);
          oAppMonitorView.CodMonitor = pCodMonitor;
          oAppMonitorView.Accion = "ELIMINAR";
          oAppMonitorView.Put();

          if (!string.IsNullOrEmpty(oAptMonitorPages.Error))
          {
            Response.Write(oAptMonitorPages.Error);
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

    protected void gridPages_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCodMonitor = gridPages.SelectedDataKey.Value.ToString();
      Response.Redirect("monitorviewconfig.aspx?CodMonitorView=" + pCodMonitor);
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("menu.aspx");
    }

    protected void gridPages_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      gridPages.PageIndex = e.NewPageIndex;
      LoadGrid();
    }
  }
}