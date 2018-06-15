using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using dcControlPanel.Conn;
using dcControlPanel.Method;
using dcControlPanel.Base;

namespace dcControlPanel
{
  public partial class calidadproductiva : System.Web.UI.Page
  {
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      Web oWeb = new Web();
      string sTimePage = string.Empty;
      string sUrlPage = string.Empty;
      string pCodMonitor = oWeb.GetData("codmonitor");
      string indexToken = oWeb.GetData("indexToken");
      string pCodUsuario = oWeb.GetData("codusuario");
      string pOrderPage = oWeb.GetData("order");

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
        oAptMonitorPages.CodMonitor = pCodMonitor;
        oAptMonitorPages.OrderPage = (Convert.ToInt32(pOrderPage) + 1).ToString();
        DataTable dtMPage = oAptMonitorPages.Get();
        if (dtMPage != null)
        {
          if (dtMPage.Rows.Count > 0)
          {
            sUrlPage = dtMPage.Rows[0]["url_page"].ToString();
            sUrlPage = sUrlPage + "?codmonitor=" + pCodMonitor;
            sUrlPage = sUrlPage + "&indexToken=" + indexToken;
            if (!string.IsNullOrEmpty(pCodUsuario))
              sUrlPage = sUrlPage + "&codusuario=" + pCodUsuario;
            sUrlPage = sUrlPage + "&order=" + dtMPage.Rows[0]["order_page"].ToString();
            sTimePage = dtMPage.Rows[0]["time_page"].ToString();
          }
          else
          {
            oAptMonitorPages.OrderPage = string.Empty;
            dtMPage = oAptMonitorPages.Get();
            if (dtMPage != null)
            {
              if (dtMPage.Rows.Count > 0)
              {
                if ((string.IsNullOrEmpty(dtMPage.Rows[0]["cod_cliente"].ToString())) && (string.IsNullOrEmpty(dtMPage.Rows[0]["cod_holding"].ToString())))
                {
                  cCliente oCliente = new cCliente(ref oConn);
                  DataTable dtCliente = oCliente.Get();
                  if (!string.IsNullOrEmpty(indexToken))
                  {
                    if (dtCliente.Rows.Count == (Convert.ToInt32(Session[indexToken].ToString()) + 1))
                      Session[indexToken] = 0;
                    else
                      Session[indexToken] = Convert.ToInt32(Session[indexToken].ToString()) + 1;
                  }
                  else {
                    if (dtCliente.Rows.Count == (Convert.ToInt32(Session["indexcliente"].ToString()) + 1))
                      Session["indexcliente"] = 0;
                    else
                      Session["indexcliente"] = Convert.ToInt32(Session["indexcliente"].ToString()) + 1;
                  }                  
                  dtCliente = null;
                }

                sUrlPage = dtMPage.Rows[0]["url_page"].ToString();
                sUrlPage = sUrlPage + "?codmonitor=" + pCodMonitor;
                sUrlPage = sUrlPage + "&indexToken=" + indexToken;
                if (!string.IsNullOrEmpty(pCodUsuario))
                  sUrlPage = sUrlPage + "&codusuario=" + pCodUsuario;
                sUrlPage = sUrlPage + "&order=" + dtMPage.Rows[0]["order_page"].ToString();
                sTimePage = dtMPage.Rows[0]["time_page"].ToString();
              }
            }
          }
        }
        dtMPage = null;

        Response.AppendHeader("Refresh", sTimePage + "; URL=" + sUrlPage);
      }
      oConn.Close();
    }

    protected void AuthenticUser(string sCodUser)
    {
      Usuario oIsUsuario;
      Web oWeb = new Web();
      
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
        oSysUsuario.CodUser = sCodUser;
        oSysUsuario.EstUser = "V";
        DataTable dtUser = oSysUsuario.GetSysUsuario();
        if (dtUser != null)
        {
          if (dtUser.Rows.Count > 0)
          {
            oIsUsuario = oWeb.GetObjUsuario();
            oIsUsuario.CodUsuario = dtUser.Rows[0]["cod_user"].ToString();
            oIsUsuario.Nombres = (dtUser.Rows[0]["nom_user"].ToString() + " " + dtUser.Rows[0]["ape_user"].ToString()).Trim();
            oIsUsuario.Email = dtUser.Rows[0]["eml_user"].ToString();
            oIsUsuario.CodTipoUsuario = dtUser.Rows[0]["cod_tipo"].ToString();
            oIsUsuario.NKeyUsuario = dtUser.Rows[0]["nkey_user"].ToString();

            Session["USUARIO"] = oIsUsuario;

            cSysPerfilesUsuarios oSysPerfilesUsuarios = new cSysPerfilesUsuarios(ref oConn);
            oSysPerfilesUsuarios.CodUser = dtUser.Rows[0]["cod_user"].ToString();
            DataTable dtUserPerfil = oSysPerfilesUsuarios.Get();
            if (dtUserPerfil != null)
            {
              foreach (DataRow oRow in dtUserPerfil.Rows)
              {
                if ((oRow["cod_perfil"].ToString() == "1") || (oRow["cod_perfil"].ToString() == "2") || (oRow["cod_perfil"].ToString() == "6"))
                {
                  Session["AdministradorPanel"] = 1;
                }
              }
            }
            dtUserPerfil = null;

          }
        }
        dtUser = null;
      }
      oConn.Close();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      Web oWeb = new Web();
      string pCodMonitor = oWeb.GetData("codmonitor");
      string pCodUsuario = oWeb.GetData("codusuario");
      string pAuthenCodUser = oWeb.GetData("authuser");
      string indexToken = oWeb.GetData("indexToken");

      string pCodPage = string.Empty;
      string pOrderPage = oWeb.GetData("order");
      string pCodCliente = string.Empty;
      string pTipoUsuario = string.Empty;
      string sIdFecha = string.Empty;
      int maxdt = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1)).Day;
      firtDay.InnerText = "01-" + DateTime.Now.ToString("MM-yyyy");
      lastDay.InnerText = maxdt.ToString() + "-" + DateTime.Now.ToString("MM-yyyy");

      if (!string.IsNullOrEmpty(pAuthenCodUser)) {
        AuthenticUser(pAuthenCodUser);
      }

      oWeb.ValidaUserPanel();

      if (!IsPostBack)
      {
        if (oConn.Open())
        {
          cAptMonitorPages oAptMonitorPages = new cAptMonitorPages(ref oConn);
          oAptMonitorPages.CodMonitor = pCodMonitor;
          oAptMonitorPages.OrderPage = pOrderPage;
          DataTable dtMPage = oAptMonitorPages.Get();

          if (dtMPage != null)
          {
            if (dtMPage.Rows.Count > 0)
            {
              pCodPage = dtMPage.Rows[0]["cod_page"].ToString();
              pTipoUsuario = dtMPage.Rows[0]["tipo_usuario"].ToString();
              if (!string.IsNullOrEmpty(dtMPage.Rows[0]["cod_cliente"].ToString()))
              {
                pCodCliente = dtMPage.Rows[0]["cod_cliente"].ToString();
              }
              else if (!string.IsNullOrEmpty(dtMPage.Rows[0]["cod_holding"].ToString()))
              {
                pCodCliente = dtMPage.Rows[0]["cod_holding"].ToString();
                pTipoUsuario = "H";
              }
              else
              {
                cCliente oCliente = new cCliente(ref oConn);
                DataTable dtCliente = oCliente.Get();
                if (!string.IsNullOrEmpty(indexToken))
                {
                  pCodCliente = dtCliente.Rows[Convert.ToInt32(Session[indexToken].ToString())]["nkey_cliente"].ToString();
                }
                else {
                  pCodCliente = dtCliente.Rows[Convert.ToInt32(Session["indexcliente"].ToString())]["nkey_cliente"].ToString();
                }
                dtCliente = null;
              }
            }
          }
          dtMPage = null;

          oCmbTipoConsulta.Value = pTipoUsuario;
          oCmbCliente.Value = pCodCliente;
          if (!string.IsNullOrEmpty(oCmbCliente.Value))
          {
            cAppLogoCliente oAppLogoCliente = new cAppLogoCliente(ref oConn);
            oAppLogoCliente.NKey_cliente = pCodCliente;
            oAppLogoCliente.Tipo = (pTipoUsuario == "H" ? pTipoUsuario : "C");
            DataTable dtlogo = oAppLogoCliente.Get();
            if (dtlogo != null)
            {
              if (dtlogo.Rows.Count > 0)
              {
                imglogo.ImageUrl = "images/logos/" + dtlogo.Rows[0]["logo_cliente"].ToString();
              }
            }
          }
          else
            imglogo.Visible = false;
        }
        oConn.Close();
      }

      if (oConn.Open())
      {
        cIndejecucion oIndejecucion = new cIndejecucion(ref oConn);
        DataTable tblEjecucion = oIndejecucion.Get();
        if (tblEjecucion != null)
        {
          if (tblEjecucion.Rows.Count > 0)
          {
            sIdFecha = tblEjecucion.Rows[0]["idfecha"].ToString();
          }
          tblEjecucion.Dispose();
        }
        tblEjecucion = null;

        DateTime idFechaMesAnterior = new DateTime(Int32.Parse(sIdFecha.Substring(0, 4)), Int32.Parse(sIdFecha.Substring(4, 2)), Int32.Parse(sIdFecha.Substring(6, 2)), Int32.Parse(sIdFecha.Substring(8, 2)), Int32.Parse(sIdFecha.Substring(10, 2)), 0);
        idFechaMesAnterior = idFechaMesAnterior.AddMonths(-1);

        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodCliente = pCodUsuario;
        oAptPagesKpi.CodPage = pCodPage;
        DataTable dtKPIs = oAptPagesKpi.Get();
        if (dtKPIs != null) {
          cIndresultado oIndResultado = new cIndresultado(ref oConn);
          DataTable tblColumn1 = null;
          cIndumbral oIndumbral = null;
          HtmlTableCell oCell;

          HtmlTableCell oCelldataColumn;
          HtmlGenericControl dynDiv;

          HtmlTableCell oCelldataAceptacion;
          HtmlGenericControl oDivAceptacion;

          HtmlTableCell oCellSLA;
          HtmlGenericControl oDivSLA;

          HtmlTableCell oCellMesAnterior;
          HtmlGenericControl oDivImg;
          Image oImage;

          foreach (DataRow oRow in dtKPIs.Rows) {
            string iDato = "0";
            string sKeyTipo = string.Empty;

            oCell = new HtmlTableCell();
            oCell.ID = "tdCell_" + oRow["identificador_kpi"].ToString();
            oCell.Controls.Add(new LiteralControl("<div align=\"center\" class=\"tbTit\">" + oRow["nombrekpi"].ToString() + "</div>"));
            HeaderId.Controls.Add(oCell);

            oCelldataColumn = new HtmlTableCell();
            oCelldataColumn.ID = "tdCell_dataColumn" + oRow["identificador_kpi"].ToString();
            oCelldataColumn.Attributes.Add("class", "clDtPanel");

            dynDiv = new HtmlGenericControl("div");
            dynDiv.ID = "divControl_" + oRow["identificador_kpi"].ToString();

            oIndResultado.Tipo = oCmbTipoConsulta.Value;
            oIndResultado.KeyCliente = oCmbCliente.Value;
            oIndResultado.IdFecha = sIdFecha;
            oIndResultado.Indicador = oRow["identificador_kpi"].ToString();
            tblColumn1 = oIndResultado.Get();
            if (tblColumn1 != null)
            {
              if (tblColumn1.Rows.Count > 0)
              {
                iDato = (!string.IsNullOrEmpty(tblColumn1.Rows[0]["valor"].ToString()) ? tblColumn1.Rows[0]["valor"].ToString() : "0");
                sKeyTipo = tblColumn1.Rows[0]["nkey_tipo"].ToString();
                dynDiv.InnerText = String.Format("{0:0}", double.Parse(iDato));
                //oColumn1.InnerText = String.Format("{0:0}", double.Parse(iDato));
              }
              tblColumn1.Dispose();
            }
            tblColumn1 = null;

            oCellMesAnterior = new HtmlTableCell();
            oCellMesAnterior.ID = "tdCell_mesanterior" + oRow["identificador_kpi"].ToString();
            oCellMesAnterior.Attributes.Add("class", "clDtPanel");

            /*******************************************************/
            /***************** Mes Anterior ************************/
            /*******************************************************/
            oIndResultado.IdFecha = idFechaMesAnterior.ToString("yyyyMMddHHmm");
            tblColumn1 = oIndResultado.GetMesAnterior();
            if (tblColumn1 != null)
            {
              if (tblColumn1.Rows.Count > 0)
              {
                oDivImg = new HtmlGenericControl("div");
                oDivImg.Attributes.Add("class", "tbDat");

                oImage = new Image();
                oImage.ImageUrl = oWeb.getColorArrowCalidad(double.Parse(iDato), double.Parse(tblColumn1.Rows[0]["valor"].ToString()));

                oDivImg.Controls.Add(oImage);
                oCellMesAnterior.Controls.Add(oDivImg);
                //oColumnMesAnt1.ImageUrl = oWeb.getColorArrowCalidad(double.Parse(iDato), double.Parse(tblColumn1.Rows[0]["valor"].ToString()));
              }
              tblColumn1.Dispose();
            }
            tblColumn1 = null;

            oCelldataAceptacion = new HtmlTableCell();
            oCelldataAceptacion.ID = "tdCell_aceptacion" + oRow["identificador_kpi"].ToString();
            oCelldataAceptacion.Attributes.Add("class", "clDtPanel");

            oDivAceptacion = new HtmlGenericControl("div");
            oDivAceptacion.ID = "divAceptacion_" + oRow["identificador_kpi"].ToString();
            oDivAceptacion.Attributes.Add("class", "tbDat");

            oCellSLA = new HtmlTableCell();
            oCellSLA.ID = "tdCell_SLA" + oRow["identificador_kpi"].ToString();
            oCellSLA.Attributes.Add("class", "clDtPanel");

            oDivSLA = new HtmlGenericControl("div");
            oDivSLA.ID = "divSLA_" + oRow["identificador_kpi"].ToString();
            oDivSLA.Attributes.Add("class", "tbDat");

            oIndumbral = new cIndumbral(ref oConn);
            oIndumbral.KeyCliente = oCmbCliente.Value;
            oIndumbral.Tipo = oCmbTipoConsulta.Value;
            oIndumbral.Indicador = oRow["identificador_kpi"].ToString();
            oIndumbral.KeyTipo = sKeyTipo;
            tblColumn1 = oIndumbral.Get();
            if (tblColumn1 != null)
            {
              if (tblColumn1.Rows.Count > 0)
              {
                oDivAceptacion.InnerText = tblColumn1.Rows[0]["criterio_aceptacion"].ToString() + tblColumn1.Rows[0]["unidad"].ToString();
                oCelldataAceptacion.Controls.Add(oDivAceptacion);
                //oCAceptacion1.InnerHtml = tblColumn1.Rows[0]["criterio_aceptacion"].ToString() + tblColumn1.Rows[0]["unidad"].ToString();

                oDivSLA.InnerText = tblColumn1.Rows[0]["sla"].ToString();
                oCellSLA.Controls.Add(oDivSLA);
                //oSLA1.InnerHtml = tblColumn1.Rows[0]["sla"].ToString();

                dynDiv.Attributes.Add("class", oWeb.getColorNumAvance(double.Parse(iDato), tblColumn1));
                oCelldataColumn.Controls.Add(dynDiv);
                //oColumn1.Attributes.Add("class", oWeb.getColorNumAvance(double.Parse(iDato), tblColumn1));
              }
              tblColumn1.Dispose();
            }
            tblColumn1 = null;

            dataColumn.Controls.Add(oCelldataColumn);
            dataAceptacion.Controls.Add(oCelldataAceptacion);
            dataSLA.Controls.Add(oCellSLA);
            dataMesAnterior.Controls.Add(oCellMesAnterior);

          }
        }
        dtKPIs = null;
        
        oConn.Close();
      }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
      Web oWeb = new Web();
      dcControlPanel.Method.Usuario oIsUsuario = oWeb.GetObjUsuario();

      if ((Session["AdministradorPanel"] != null) && (!string.IsNullOrEmpty(Session["AdministradorPanel"].ToString())))
        Response.Redirect("index.aspx");
      else
        Response.Redirect("monitoresclientes.aspx?CodCliente=" + oIsUsuario.CodUsuario);
    }
  }
}