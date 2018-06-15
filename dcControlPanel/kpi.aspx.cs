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
  public partial class kpi : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        CodKpi.Value = oWeb.GetData("CodKpi");

        if (!string.IsNullOrEmpty(CodKpi.Value))
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cAppKpi oAppKpi = new cAppKpi(ref oConn);
            oAppKpi.CodKpi = CodKpi.Value;
            DataTable dtKpi = oAppKpi.Get();
            if (dtKpi != null)
            {
              if (dtKpi.Rows.Count > 0)
              {
                txt_nombre.Text = dtKpi.Rows[0]["nombre_kpi"].ToString();
                txt_identificador.Text = dtKpi.Rows[0]["identificador_kpi"].ToString();
                oCmbEstado.Items.FindByValue(dtKpi.Rows[0]["estado_kpi"].ToString());
              }
            }
            dtKpi = null;
          }
          oConn.Close();

        }
      }
    }

    protected void btnGrabar1_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppKpi oAppKpi = new cAppKpi(ref oConn);
        oAppKpi.CodKpi = CodKpi.Value;
        oAppKpi.NombreKpi = txt_nombre.Text;
        oAppKpi.IdentificadorKpi = txt_identificador.Text;
        oAppKpi.EstadoKpi = oCmbEstado.SelectedValue;
        oAppKpi.Accion = (!string.IsNullOrEmpty(CodKpi.Value) ? "EDITAR" : "CREAR");
        oAppKpi.Put();

        if (!string.IsNullOrEmpty(oAppKpi.Error))
        {
          Response.Write("Error : " + oAppKpi.Error + "<br>");
          Response.End();
        }

        CodKpi.Value = oAppKpi.CodKpi;

      }
      oConn.Close();

      StringBuilder js = new StringBuilder();
      js.Append("function LgRespuestaOK() {");
      js.Append(" window.radalert('KPI creado correctamente'); ");
      js.Append(" Sys.Application.remove_load(LgRespuestaOK); ");
      js.Append("};");
      js.Append("Sys.Application.add_load(LgRespuestaOK);");
      Page.ClientScript.RegisterStartupScript(Page.GetType(), "LgRespuestaOK", js.ToString(), true);
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("kpi_list.aspx");
    }
  }
}