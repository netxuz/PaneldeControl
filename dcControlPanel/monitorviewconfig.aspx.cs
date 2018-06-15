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
using Telerik.Web.UI;

namespace dcControlPanel
{
  public partial class monitorviewconfig : System.Web.UI.Page
  {
    Web oWeb = new Web();

    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        CodMonitorView.Value = oWeb.GetData("CodMonitorView");
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          if (string.IsNullOrEmpty(CodMonitorView.Value))
          {
            cAppTipoConnsulta oAppTipoConnsulta = new cAppTipoConnsulta(ref oConn);
            DataTable dtTipoConsulta = oAppTipoConnsulta.Get();

            if (dtTipoConsulta != null)
            {
              if (dtTipoConsulta.Rows.Count > 0)
              {
                oCmbTipoConsulta.Items.Add(new RadComboBoxItem("Seleccione Tipo Consulta", ""));
                foreach (DataRow oRow in dtTipoConsulta.Rows)
                {
                  oCmbTipoConsulta.Items.Add(new RadComboBoxItem(oRow["nom_tipo"].ToString(), oRow["cod_tipo"].ToString()));
                }
              }
            }
            dtTipoConsulta = null;
          }
          else
          {
            cAppTipoConnsulta oAppTipoConnsulta = new cAppTipoConnsulta(ref oConn);
            oAppTipoConnsulta.CodMonitor = CodMonitorView.Value;
            DataTable dtTipoConsulta = oAppTipoConnsulta.GetByMonitor();

            if (dtTipoConsulta != null)
            {
              if (dtTipoConsulta.Rows.Count > 0)
              {
                oCmbTipoConsulta.Items.Add(new RadComboBoxItem("Seleccione Tipo Consulta", ""));
                foreach (DataRow oRow in dtTipoConsulta.Rows)
                {
                  oCmbTipoConsulta.Items.Add(new RadComboBoxItem(oRow["nom_tipo"].ToString(), oRow["cod_tipo"].ToString()));
                }
              }
            }
            dtTipoConsulta = null;

            btnGrabar1.Visible = false;
            cAppMonitorView oAppMonitorView = new cAppMonitorView(ref oConn);
            oAppMonitorView.CodMonitor = CodMonitorView.Value;
            DataTable dtMonitorView = oAppMonitorView.Get();
            if (dtMonitorView != null)
            {
              if (dtMonitorView.Rows.Count > 0)
              {
                txt_nombre.Text = dtMonitorView.Rows[0]["desc_monitor_view"].ToString();
                oCmbEstado.Items.FindByValue(dtMonitorView.Rows[0]["est_monitor_view"].ToString()).Selected = true;
              }
              dtMonitorView.Dispose();
            }
            dtMonitorView = null;

            txt_nombre.Enabled = false;
            oCmbEstado.Enabled = false;
            trPagesView.Visible = true;

            cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
            oAptMonitorPages.CodMonitor = CodMonitorView.Value;
            gridPages.DataSource = oAptMonitorPages.Get();
            gridPages.DataBind();

            hddMonitorCreated.Value = "1";

          }

          oConn.Close();
        }
      }
      else
      {
        if (!string.IsNullOrEmpty(oWeb.GetData("hddReload")))
        {
          hddReload.Value = string.Empty;
          oCmbTipoConsulta.FindItemByValue("").Selected = true;
        }
      }
    }

    protected void btnGrabar1_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppMonitorView oAppMonitorView = new cAppMonitorView(ref oConn);
        oAppMonitorView.CodMonitor = CodMonitorView.Value;
        oAppMonitorView.DescMonitorView = txt_nombre.Text;
        //oAppMonitorView.TipoUsuario = oCmbTipoConsulta.SelectedValue;
        oAppMonitorView.EstMonitorView = oCmbEstado.SelectedValue;
        //oAppMonitorView.CodCliente = (!string.IsNullOrEmpty(oCmbCliente.SelectedValue) ? oCmbCliente.SelectedValue : null);
        oAppMonitorView.Accion = (string.IsNullOrEmpty(CodMonitorView.Value) ? "CREAR" : "EDITAR");
        oAppMonitorView.Put();

        if (!string.IsNullOrEmpty(oAppMonitorView.Error))
        {
          Response.Write("Error : " + oAppMonitorView.Error + "<br>");
          Response.End();
        }

        CodMonitorView.Value = oAppMonitorView.CodMonitor;
        btnGrabar1.Visible = false;
        txt_nombre.Enabled = false;
        //oCmbTipoConsulta.Enabled = false;
        oCmbEstado.Enabled = false;
        //oCmbCliente.Enabled = false;
        trPagesView.Visible = true;

        oConn.Close();
      }
    }

    protected void btnGrabar2_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
        oAptMonitorPages.CodMonitor = CodMonitorView.Value;
        DataTable dtMPage = oAptMonitorPages.Get();
        oAptMonitorPages.OrderPage = (dtMPage.Rows.Count + 1).ToString();
        dtMPage = null;
        oAptMonitorPages.CodPage = oCmbPages.SelectedValue;
        oAptMonitorPages.TimePage = txt_time.Text;
        oAptMonitorPages.EstPage = "V";
        oAptMonitorPages.TipoUsuario = oCmbTipoConsulta.SelectedValue;
        oAptMonitorPages.CodCliente = (!string.IsNullOrEmpty(oCmbCliente.SelectedValue) ? oCmbCliente.SelectedValue : null);
        oAptMonitorPages.CodHolding = (!string.IsNullOrEmpty(oCmbHolding.SelectedValue) ? oCmbHolding.SelectedValue : null);
        oAptMonitorPages.Accion = "CREAR";
        oAptMonitorPages.Put();

        if (!string.IsNullOrEmpty(oAptMonitorPages.Error))
        {
          Response.Write(oAptMonitorPages.Error);
          Response.End();
        }

        oAptMonitorPages.CodPage = string.Empty;
        oAptMonitorPages.EstPage = string.Empty;
        oAptMonitorPages.OrderPage = string.Empty;
        gridPages.DataSource = oAptMonitorPages.Get();
        gridPages.DataBind();

        oConn.Close();

        oCmbCliente.Items[0].Selected = true;
        oCmbHolding.Items[0].Selected = true;
        oCmbTipoConsulta.Items[0].Selected = true;
        oCmbPages.Items[0].Selected = true;
        tpconsulta_normal.Visible = false;
        tdselectvista.Visible = true;



      }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("monitorview.aspx");
    }

    protected void oCmbTipoConsulta_OnClientSelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
      DataTable oPages = null;
      tdselectvista.Visible = true;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppPages oAppPages = new cAppPages(ref oConn);
        oAppPages.CodTipo = oCmbTipoConsulta.SelectedValue.ToString();

        if (oCmbTipoConsulta.SelectedValue.ToString() == "N")
        {
          oAppPages.CodMonitor = CodMonitorView.Value;
          oPages = oAppPages.GetByType();
        }
        else {
          oPages = oAppPages.GetByTipo();
        }

        cAppTipoConnsulta oTipoConnsulta = new cAppTipoConnsulta(ref oConn);
        oTipoConnsulta.CodTipo = oCmbTipoConsulta.SelectedValue.ToString();
        oTipoConnsulta.IndSegNivel = "S";
        DataTable dtTpCon = oTipoConnsulta.Get();
        if (dtTpCon != null)
        {
          if (dtTpCon.Rows.Count > 0)
          {
            //tpconsulta_segundonivel.Visible = true;
            tpconsulta_normal.Visible = false;
            //oCmbPages.Visible = false;
            //lblPage.Visible = true;

            //if (oPages != null)
            //{
            //  if (oPages.Rows.Count > 0)
            //  {
            //    lblPage.Text = oPages.Rows[0]["nom_page"].ToString();
            //    hddCodPage.Value = oPages.Rows[0]["cod_page"].ToString();
            //LoadGrid();
            //  }
            //  oPages.Dispose();
            //}
            //oPages = null;
            btnGrabar2.Visible = false;
          }
          else
          {
            lblPage.Visible = false;

            cCliente oCliente = new cCliente(ref oConn);
            DataTable dtcliente = oCliente.Get();
            tpconsulta_normal.Visible = true;
            tpconsulta_segundonivel.Visible = false;
            btnGrabar2.Visible = true;
            //oCmbPages.Items.Add(new ListItem("Selecciona una vista", ""));

            oCmbHolding.Items.Clear();
            oCmbHolding.Items.Add(new ListItem("Selecciona holding", ""));
            DataTable dtholding = oCliente.GetHolding();
            if (dtholding != null)
            {
              foreach (DataRow oRow in dtholding.Rows)
              {
                oCmbHolding.Items.Add(new ListItem(oRow["holding"].ToString(), oRow["ncodholding"].ToString()));
              }
            }
            dtholding = null;

            oCmbCliente.Items.Clear();
            oCmbCliente.Items.Add(new RadComboBoxItem("Selecciona cliente", ""));
            if (dtcliente != null)
            {
              foreach (DataRow oRow in dtcliente.Rows)
              {
                oCmbCliente.Items.Add(new RadComboBoxItem(oRow["snombre"].ToString(), oRow["nkey_cliente"].ToString()));
              }
            }
            dtcliente = null;
          }
        }
        dtTpCon = null;

        oCmbPages.Visible = true;
        oCmbPages.Items.Clear();
        oCmbPages.Items.Add(new RadComboBoxItem("Selecciona una Vista", ""));
        if (oPages != null)
        {
          if (oPages.Rows.Count > 0)
          {
            foreach (DataRow oRow in oPages.Rows)
            {
              oCmbPages.Items.Add(new RadComboBoxItem(oRow["nom_page"].ToString(), oRow["cod_page"].ToString()));
            }
          }
          oPages.Dispose();
        }
        oPages = null;
      }
      oConn.Close();
    }

    protected void LoadGrid()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cCliente oCliente = new cCliente(ref oConn);
        oCliente.CodMonitor = CodMonitorView.Value;
        //oCliente.CodPage = hddCodPage.Value;
        oCliente.CodPage = oCmbPages.SelectedValue;
        if (string.IsNullOrEmpty(bVista.Value))
          GridClientes.DataSource = oCliente.GetClientByPageMonitor();
        else
          GridClientes.DataSource = oCliente.GetHoldingByPageMonitor();
        GridClientes.DataBind();
      }
      oConn.Close();
    }

    protected void btnClose2_Click(object sender, EventArgs e)
    {
      onCLose();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
      onCLose();
    }

    protected void onCLose()
    {
      bVista.Value = string.Empty;
      btnChangeClientHolding.Text = "Por Cliente";

      tpconsulta_normal.Visible = false;
      tdselectvista.Visible = false;
      tpconsulta_segundonivel.Visible = false;
      //oCmbPages.Visible = false;
      lblPage.Visible = false;
      hddCodPage.Value = string.Empty;
      hddReload.Value = "1";

      hddMonitorCreated.Value = string.Empty;
      btnChangeClientHolding.Visible = true;

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
        oAptMonitorPages.CodMonitor = CodMonitorView.Value;
        gridPages.DataSource = oAptMonitorPages.Get();
        gridPages.DataBind();
      }
      oConn.Close();

      Page.ClientScript.RegisterStartupScript(this.GetType(), "hide", "$(function () { $('#myModal').modal('hide'); goReload(); });", true);
    }

    protected void GridClientes_SelectedIndexChanged(object sender, EventArgs e)
    {
      string pCliente = GridClientes.SelectedDataKey.Value.ToString();
      string pCodMonitor = CodMonitorView.Value;
      //string pCodPage = hddCodPage.Value;
      string pCodPage = oCmbPages.SelectedValue.ToString();

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        string TipoConsulta = "A";
        if (string.IsNullOrEmpty(hddMonitorCreated.Value))
        {
          cAppPages oAppPages = new cAppPages(ref oConn);
          oAppPages.CodPage = pCodPage;
          DataTable dtPage = oAppPages.Get();
          if (dtPage != null)
          {
            if (dtPage.Rows.Count > 0)
            {
              if (!string.IsNullOrEmpty(dtPage.Rows[0]["tipo_consulta"].ToString()))
                TipoConsulta = dtPage.Rows[0]["tipo_consulta"].ToString();
            }
          }
          dtPage = null;

          cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
          oAptMonitorPages.CodMonitor = CodMonitorView.Value;
          DataTable dtMPage = oAptMonitorPages.Get();
          oAptMonitorPages.OrderPage = (dtMPage.Rows.Count + 1).ToString();
          dtMPage = null;
          oAptMonitorPages.CodPage = pCodPage;
          oAptMonitorPages.TimePage = txt_time.Text;
          oAptMonitorPages.EstPage = "V";
          if (TipoConsulta != "N")
          {
            oAptMonitorPages.TipoUsuario = TipoConsulta;
          }
          else
          {
            if (string.IsNullOrEmpty(bVista.Value))
              oAptMonitorPages.TipoUsuario = "A";
            else
              oAptMonitorPages.TipoUsuario = "G";
          }
          oAptMonitorPages.Accion = "CREAR";
          oAptMonitorPages.Put();

          if (!string.IsNullOrEmpty(oAptMonitorPages.Error))
          {
            Response.Write(oAptMonitorPages.Error);
            Response.End();
          }

          hddMonitorCreated.Value = "1";
        }

        cAptClientesMonitorpages oAptClientesMonitorpages = new cAptClientesMonitorpages(ref oConn);
        oAptClientesMonitorpages.CodCliente = pCliente;
        oAptClientesMonitorpages.CodMonitor = pCodMonitor;
        oAptClientesMonitorpages.CodPage = pCodPage;
        oAptClientesMonitorpages.Accion = "CREAR";
        oAptClientesMonitorpages.Put();

        btnChangeClientHolding.Visible = false;
      }
      oConn.Close();
      LoadGrid();

    }

    protected void GridClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      GridClientes.PageIndex = e.NewPageIndex;
      LoadGrid();
    }

    protected void oCmbPages_SelectedIndexChanged1(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
      string TipoConsulta = "A";
      if (oCmbTipoConsulta.SelectedValue == "N")
        tpconsulta_segundonivel.Visible = true;

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppPages oAppPages = new cAppPages(ref oConn);
        oAppPages.CodPage = oCmbPages.SelectedValue;
        DataTable dPage = oAppPages.Get();
        if (dPage != null)
        {
          if (dPage.Rows.Count > 0)
          {
            if (!string.IsNullOrEmpty(dPage.Rows[0]["tipo_consulta"].ToString()))
            {
              TipoConsulta = dPage.Rows[0]["tipo_consulta"].ToString();
            }
          }
        }
        dPage = null;
      }
      oConn.Close();
      if (oCmbTipoConsulta.SelectedValue == "N")
      {
        if ((TipoConsulta == "P") || (TipoConsulta == "R") || (TipoConsulta == "D"))
        {
          btnSeleccionar.InnerText = "Seleccionar Holding";
          bVista.Value = "1";
          btnChangeClientHolding.Visible = false;
        }
        else
        {
          btnSeleccionar.InnerText = "Seleccionar Clientes / Holding";
          btnChangeClientHolding.Text = "Por Holding";
          bVista.Value = string.Empty;
          btnChangeClientHolding.Visible = true;
        }

        LoadGrid();
        btnSeleccionar.Disabled = false;
      }
    }

    protected void btnChangeClientHolding_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(bVista.Value))
      {
        bVista.Value = "1";
        btnChangeClientHolding.Text = "Por Cliente";
      }
      else
      {
        bVista.Value = string.Empty;
        btnChangeClientHolding.Text = "Por Holding";
      }
      LoadGrid();
    }
  }
}