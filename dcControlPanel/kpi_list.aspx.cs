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
  public partial class kpi_list : System.Web.UI.Page
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
        cAppKpi oAppKpi = new cAppKpi(ref oConn);
        if (!string.IsNullOrEmpty(txtKpi.Text))
          oAppKpi.NombreKpi = txtKpi.Text;
        gridKpi.DataSource = oAppKpi.Get();
        gridKpi.DataBind();

        oConn.Close();
      }
    }

    protected void gridKpi_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        string pCodKpi = gridKpi.DataKeys[e.RowIndex].Value.ToString();

        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodKpi = pCodKpi;
        oAptPagesKpi.Accion = "ELIMINAR";
        oAptPagesKpi.Put();

        if (!string.IsNullOrEmpty(oAptPagesKpi.Error))
        {
          Response.Write(oAptPagesKpi.Error);
          Response.End();
        }

        oAptPagesKpi.DelAptKpiPageCliente();
        if (!string.IsNullOrEmpty(oAptPagesKpi.Error))
        {
          Response.Write(oAptPagesKpi.Error);
          Response.End();
        }

        cAppKpi oAppKpi = new cAppKpi(ref oConn);
        oAppKpi.CodKpi = pCodKpi;
        oAppKpi.Accion = "ELIMINAR";
        oAppKpi.Put();

        if (!string.IsNullOrEmpty(oAppKpi.Error))
        {
          Response.Write(oAppKpi.Error);
          Response.End();
        }

        oConn.Close();
        LoadGrid();
      }
    }

    protected void gridKpi_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCodKpi = gridKpi.SelectedDataKey.Value.ToString();
      Response.Redirect("kpi.aspx?CodKpi=" + pCodKpi);
    }

    protected void btnCrear_Click(object sender, EventArgs e)
    {
      Response.Redirect("kpi.aspx");
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("menu.aspx");
    }

    protected void gridKpi_DataBound(object sender, EventArgs e)
    {
    }


    protected void gridKpi_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow) {
        DataRowView row = (DataRowView)e.Row.DataItem;
        if (e.Row.Cells[3].Text == "A")
        {
          e.Row.Cells[3].Text = "Activo";
        }
        else {
          e.Row.Cells[3].Text = "No Activo";
        }
      }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      LoadGrid();
    }

    protected void gridKpi_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      gridKpi.PageIndex = e.NewPageIndex;
      LoadGrid();
    }
  }
}