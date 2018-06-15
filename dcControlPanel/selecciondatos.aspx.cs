using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using dcControlPanel.Conn;
using dcControlPanel.Method;
using dcControlPanel.Base;
using System.Text;

namespace dcControlPanel
{
    public partial class selecciondatos : System.Web.UI.Page
    {
        Web oWeb = new Web();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConn oConn = new DBConn();
                if (oConn.Open())
                {
                    cCliente oCliente = new cCliente(ref oConn);
                    DataTable tblCliente = oCliente.Get();
                    if (tblCliente != null)
                    {
                        if (tblCliente.Rows.Count > 0)
                        {
                            oCmbCliente.Items.Add(new ListItem("Seleccione Cliente", ""));
                            foreach (DataRow oRow in tblCliente.Rows)
                            {
                                oCmbCliente.Items.Add(new ListItem(oRow["snombre"].ToString(), oRow["nkey_cliente"].ToString()));
                            }
                        }
                        tblCliente.Dispose();
                    }
                    oCliente = null;
                }
                oConn.Close();
                sTipo.Value = oWeb.GetData("sTipo");
                oCmbTipoConsulta.Items.Add(new ListItem("Seleccione Tipo Consulta", ""));
                switch (sTipo.Value)
                {
                    case "calidadproductiva":
                        oCmbTipoConsulta.Items.Add(new ListItem("Analista", "A"));
                        oCmbTipoConsulta.Items.Add(new ListItem("Cliente", "C"));
                        oCmbTipoConsulta.Items.Add(new ListItem("Supervisor", "S"));
                        break;
                    case "impecabilidad":
                        oCmbTipoConsulta.Items.Add(new ListItem("Analista", "A"));
                        oCmbTipoConsulta.Items.Add(new ListItem("Cliente", "C"));
                        break;
                    case "managersp1":
                    case "managersp2":
                        oCmbTipoConsulta.Items.Add(new ListItem("Supervisor", "S"));
                        break;
                    case "analistap1":
                    case "analistap2":
                        oCmbTipoConsulta.Items.Add(new ListItem("Analista", "A"));
                        break;
                    case "riegooperacional":
                        oCmbTipoConsulta.Items.Add(new ListItem("Manual", "M"));
                        break;
                    case "tesoreriaproceso":
                        oCmbTipoConsulta.Items.Add(new ListItem("Analista", "A"));
                        break;
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            StringBuilder sData = new StringBuilder();
            sData.Append("?oCmbCliente=").Append(oCmbCliente.SelectedValue);
            sData.Append("&oCmbTipoConsulta=").Append(oCmbTipoConsulta.SelectedValue);
            switch (sTipo.Value)
            {
                case "calidadproductiva":
                    Response.Redirect("calidadproductiva.aspx" + sData);
                    break;
                case "impecabilidad":
                    Response.Redirect("impecabilidad.aspx" + sData);
                    break;
                case "managersp1":
                    Response.Redirect("medicionmanager.aspx" + sData);
                    break;
                case "managersp2":
                    Response.Redirect("medicionmanagerp1.aspx" + sData);
                    break;
                case "analistap1":
                    Response.Redirect("medicionmanager.aspx" + sData);
                    break;
                case "analistap2":
                    Response.Redirect("medicionmanagerp1.aspx" + sData);
                    break;
                case "riegooperacional":
                    Response.Redirect("riesgooperacional.aspx" + sData);
                    break;
                case "tesoreriaproceso":
                    Response.Redirect("tesoreriaproceso.aspx" + sData);
                    break;
            }
        }
    }
}