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
  public partial class set_kpi_pages1 : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        CodCliente.Value = oWeb.GetData("CodCliente");
        CodPage.Value = oWeb.GetData("CodPage");
        if (!string.IsNullOrEmpty(CodPage.Value))
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cAppPages oAppPages = new cAppPages(ref oConn);
            oAppPages.CodPage = CodPage.Value;
            DataTable dtPage = oAppPages.Get();
            if (dtPage != null)
            {
              if (dtPage.Rows.Count > 0)
              {
                lblNomPagina.Text = dtPage.Rows[0]["nom_page"].ToString();
              }
            }
            dtPage = null;
          }
          oConn.Close();
        }

        LoadGrid();
      }
    }

    protected void LoadGrid()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodCliente = CodCliente.Value;
        oAptPagesKpi.CodPage = CodPage.Value;
        gridKpiPage.DataSource = oAptPagesKpi.Get();
        gridKpiPage.DataBind();

        GridKPIs.DataSource = oAptPagesKpi.GetKpiNotIn();
        GridKPIs.DataBind();

        oConn.Close();
      }
    }

    protected void gridKpiPage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        string pCodKpi = gridKpiPage.DataKeys[e.RowIndex].Value.ToString();

        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodCliente = CodCliente.Value;
        oAptPagesKpi.CodPage = CodPage.Value;
        oAptPagesKpi.CodKpi = pCodKpi;
        oAptPagesKpi.Accion = "ELIMINAR";
        oAptPagesKpi.Put();

        if (!string.IsNullOrEmpty(oAptPagesKpi.Error))
        {
          Response.Write(oAptPagesKpi.Error);
          Response.End();
        }

        oConn.Close();
        LoadGrid();
        UpdatePanel2.Update();
      }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      string sUrl = "set_kpi_pages_list.aspx";

      if (!string.IsNullOrEmpty(CodCliente.Value))
        sUrl = sUrl + "?CodCliente=" + CodCliente.Value;

      Response.Redirect(sUrl);
    }

    protected void GridKPIs_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCodKpi = GridKPIs.SelectedDataKey.Value.ToString();
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodCliente = CodCliente.Value;
        oAptPagesKpi.CodPage = CodPage.Value;
        oAptPagesKpi.CodKpi = pCodKpi;
        oAptPagesKpi.Accion = "CREAR";
        oAptPagesKpi.Put();

        if (!string.IsNullOrEmpty(oAptPagesKpi.Error))
        {
          Response.Write(oAptPagesKpi.Error);
          Response.End();
        }

        oConn.Close();
        LoadGrid();
      }
      oConn.Close();


    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
      Page.ClientScript.RegisterStartupScript(this.GetType(), "hide", "$(function () { $('#myModal').modal('hide'); });", true);
      LoadGrid();
      UpdatePanel1.Update();
    }

    protected void UpdatePanel1_Load(object sender, EventArgs e)
    {
      if (Request.Form["__EVENTARGUMENT"] != null)
      {
        if (Request.Form["__EVENTARGUMENT"].ToString() == "priorityUpdate")
        {
          lblStatus.Visible = false;
          lblStatus.Text = "Status actualizado satisfactoriamente";
          LoadGrid();
          UpdatePanel1.Update();
        }
      }
    }

    [System.Web.Services.WebMethod]
    public static void webMethodCall(string DataKeyValues, string CodPage, string CodCliente)
    {
      string[] ListID = DataKeyValues.Split('|');

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodCliente = CodCliente;
        oAptPagesKpi.CodPage = CodPage;
        oAptPagesKpi.Accion = "ELIMINAR";
        oAptPagesKpi.Put();

        oAptPagesKpi.Accion = "CREAR";
        for (int i = 0; i < ListID.Length; i++)
        {
          oAptPagesKpi.CodKpi = ListID[i].ToString();
          oAptPagesKpi.Put();
        }

      }
      oConn.Close();

    }

    protected void gridKpiPage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      gridKpiPage.PageIndex = e.NewPageIndex;
      LoadGrid();
    }

    protected void GridKPIs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      GridKPIs.PageIndex = e.NewPageIndex;
      LoadGrid();
    }

    protected void btnClose2_Click(object sender, EventArgs e)
    {
      Page.ClientScript.RegisterStartupScript(this.GetType(), "hide", "$(function () { $('#myModal').modal('hide'); });", true);
      LoadGrid();
      UpdatePanel1.Update();
    }
  }
}