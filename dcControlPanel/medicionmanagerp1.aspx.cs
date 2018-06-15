using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using dcControlPanel.Conn;
using dcControlPanel.Method;
using dcControlPanel.Base;
using System.Text;

namespace dcControlPanel
{
  public partial class medicionmanagerp1 : System.Web.UI.Page
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
                  else
                  {
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
        oConn.Close();

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
      StringBuilder sNAvance;
      string pCodMonitor = oWeb.GetData("codmonitor");
      string pCodUsuario = oWeb.GetData("codusuario");
      string pAuthenCodUser = oWeb.GetData("authuser");
      string indexToken = oWeb.GetData("indexToken");

      string pCodPage = string.Empty;
      string pOrderPage = oWeb.GetData("order");
      string pCodCliente = string.Empty;
      string pTipoConsulta = string.Empty;
      string pTipoCliente = string.Empty;
      string sIdFecha = string.Empty;
      string iDato = "0";
      string sKeyTipo = string.Empty;

      if (!string.IsNullOrEmpty(pAuthenCodUser))
      {
        AuthenticUser(pAuthenCodUser);
      }

      oWeb.ValidaUserPanel();

      int maxdt = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1)).Day;
      firtDay.InnerText = "01-" + DateTime.Now.ToString("MM-yyyy");
      lastDay.InnerText = maxdt.ToString() + "-" + DateTime.Now.ToString("MM-yyyy");

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
              pTipoConsulta = dtMPage.Rows[0]["tipo_usuario"].ToString();
              if (!string.IsNullOrEmpty(dtMPage.Rows[0]["cod_cliente"].ToString()))
              {
                pCodCliente = dtMPage.Rows[0]["cod_cliente"].ToString();
                pTipoCliente = "C";
              }
              else if (!string.IsNullOrEmpty(dtMPage.Rows[0]["cod_holding"].ToString()))
              {
                pCodCliente = dtMPage.Rows[0]["cod_holding"].ToString();
                pTipoCliente = "H";
              }
              else
              {
                cCliente oCliente = new cCliente(ref oConn);
                DataTable dtCliente = oCliente.Get();
                if (!string.IsNullOrEmpty(indexToken))
                {
                  pCodCliente = dtCliente.Rows[Convert.ToInt32(Session[indexToken].ToString())]["nkey_cliente"].ToString();
                }
                else
                {
                  pCodCliente = dtCliente.Rows[Convert.ToInt32(Session["indexcliente"].ToString())]["nkey_cliente"].ToString();
                }
                dtCliente = null;
              }
            }
          }
          dtMPage = null;

          oCmbTipoConsulta.Value = pTipoConsulta;
          oCmbCliente.Value = pCodCliente;

          if (oCmbTipoConsulta.Value == "S")
          {
            pie.Attributes.Add("class", "tdPieManagers");
            lblSubTitle.Text = "Medición de Managers";
          }
          else
          {
            pie.Attributes.Add("class", "tdPieEjecutivos");
            lblSubTitle.Text = "Medición de Ejecutivos";
          }

          if (!string.IsNullOrEmpty(oCmbCliente.Value))
          {
            cAppLogoCliente oAppLogoCliente = new cAppLogoCliente(ref oConn);
            oAppLogoCliente.NKey_cliente = pCodCliente;
            oAppLogoCliente.Tipo = (pTipoCliente == "H" ? pTipoCliente : "C");
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
        if (!IsPostBack)
        {
          cCliente oCliente = new cCliente(ref oConn);
          oCliente.NKey_cliente = oCmbCliente.Value;
          DataTable tblCliente = oCliente.Get();
          if (!string.IsNullOrEmpty(oCliente.Error))
          {
            Response.Write(oCliente.Query);
            Response.End();
          }
          if (tblCliente != null)
          {
            if (tblCliente.Rows.Count > 0)
            {
              hddNCod.Value = tblCliente.Rows[0]["ncod"].ToString();
            }
          }
          tblCliente.Dispose();
          tblCliente = null;
        }
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

        HtmlTableCell htmlTblCell;
        cAnalista oAnalista = new cAnalista(ref oConn);
        oAnalista.NCod = hddNCod.Value;
        oAnalista.TipoAnalista = oCmbTipoConsulta.Value;
        DataTable tblAnalista = oAnalista.Get();
        if (tblAnalista != null)
        {
          foreach (DataRow oRow in tblAnalista.Rows)
          {
            htmlTblCell = new HtmlTableCell();
            htmlTblCell.Controls.Add(new LiteralControl("<div class=\"tbTit\">" + oRow["snombre"].ToString() + "</div>"));
            trCabecera.Controls.Add(htmlTblCell);
          }
        }
        tblAnalista.Dispose();
        tblAnalista = null;
        DataTable tblColumn2;

        HtmlGenericControl oDivImg;
        DataTable tblColumn1;
        cIndresultado oIndResultado;
        cIndumbral oIndumbral;

        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodCliente = pCodUsuario;
        oAptPagesKpi.CodPage = pCodPage;
        DataTable dtKPIs = oAptPagesKpi.Get();
        if (dtKPIs != null)
        {
          foreach (DataRow oRowKpi in dtKPIs.Rows)
          {
            iDato = string.Empty;
            sKeyTipo = string.Empty;
            HtmlTableCell HtmlTablecell;

            HtmlTablecell = new HtmlTableCell();
            StringBuilder sNomKpi = new StringBuilder();
            sNomKpi.Append("<td class=\"clPanel\"><div class=\"tbTTitRow\">");
            sNomKpi.Append(oRowKpi["nombrekpi"].ToString());
            sNomKpi.Append("</div></td>");
            HtmlTablecell.Controls.Add(new LiteralControl(sNomKpi.ToString()));
            HtmlTableRow htmlTblRowKpi = new HtmlTableRow();
            htmlTblRowKpi.ID = oRowKpi["cod_kpi"].ToString() + "_KPI";
            htmlTblRowKpi.Controls.Add(HtmlTablecell);
            tbl.Controls.Add(htmlTblRowKpi);

            HtmlTablecell = new HtmlTableCell();
            StringBuilder sCriterioAceptacion = new StringBuilder();
            sCriterioAceptacion.Append("<td class=\"clPanel\"><div class=\"tbTitRow\">Criterio Aceptación</div></td>");
            HtmlTablecell.Controls.Add(new LiteralControl(sCriterioAceptacion.ToString()));
            HtmlTableRow htmlTblRowCriAcep = new HtmlTableRow();
            htmlTblRowCriAcep.ID = oRowKpi["cod_kpi"].ToString() + "_CriAcept";
            htmlTblRowCriAcep.Controls.Add(HtmlTablecell);
            tbl.Controls.Add(htmlTblRowCriAcep);

            HtmlTablecell = new HtmlTableCell();
            StringBuilder sCriterioSLA = new StringBuilder();
            sCriterioSLA.Append("<td class=\"clPanel\"><div class=\"tbTitRow\">SLA</div></td>");
            HtmlTablecell.Controls.Add(new LiteralControl(sCriterioSLA.ToString()));
            HtmlTableRow htmlTblRowSLA = new HtmlTableRow();
            htmlTblRowSLA.ID = oRowKpi["cod_kpi"].ToString() + "_SLA";
            htmlTblRowSLA.Controls.Add(HtmlTablecell);
            tbl.Controls.Add(htmlTblRowSLA);

            HtmlTablecell = new HtmlTableCell();
            StringBuilder sCriterioMesAnte = new StringBuilder();
            sCriterioMesAnte.Append("<td class=\"clPanel\"><div class=\"tbTitRow\">Mes Anterior</div></td>");
            HtmlTablecell.Controls.Add(new LiteralControl(sCriterioMesAnte.ToString()));
            HtmlTableRow htmlTblRowMesAnt = new HtmlTableRow();
            htmlTblRowMesAnt.ID = oRowKpi["cod_kpi"].ToString() + "_MesAnt";
            htmlTblRowMesAnt.Controls.Add(HtmlTablecell);
            tbl.Controls.Add(htmlTblRowMesAnt);

            HtmlTableRow htmlTblRow = new HtmlTableRow();
            htmlTblRow.ID = oRowKpi["cod_kpi"].ToString() + "row";
            HtmlTablecell = new HtmlTableCell();
            HtmlTablecell.Controls.Add(new LiteralControl("<td colspan=\"20\" style=\"height: 10px; \"></td>"));
            htmlTblRow.Controls.Add(HtmlTablecell);
            tbl.Controls.Add(htmlTblRow);

            oAnalista = new cAnalista(ref oConn);
            oAnalista.NCod = hddNCod.Value;
            oAnalista.TipoAnalista = oCmbTipoConsulta.Value;
            tblAnalista = oAnalista.Get();

            if (tblAnalista != null)
            {
              foreach (DataRow oRow in tblAnalista.Rows)
              {
                oIndResultado = new cIndresultado(ref oConn);
                oIndResultado.Tipo = oCmbTipoConsulta.Value;
                oIndResultado.KeyCliente = oCmbCliente.Value;
                oIndResultado.KeyTipo = oRow["nKey_Analista"].ToString();
                oIndResultado.Indicador = oRowKpi["identificador_kpi"].ToString();
                oIndResultado.IdFecha = sIdFecha;
                tblColumn1 = oIndResultado.Get();
                if (tblColumn1 != null)
                {
                  if (tblColumn1.Rows.Count > 0)
                  {
                    iDato = (!string.IsNullOrEmpty(tblColumn1.Rows[0]["valor"].ToString()) ? tblColumn1.Rows[0]["valor"].ToString() : "0");
                    sKeyTipo = tblColumn1.Rows[0]["nkey_tipo"].ToString();

                    oIndumbral = new cIndumbral(ref oConn);
                    oIndumbral.KeyCliente = oCmbCliente.Value;
                    oIndumbral.Indicador = oRowKpi["identificador_kpi"].ToString();
                    oIndumbral.KeyTipo = sKeyTipo;
                    tblColumn2 = oIndumbral.Get();
                    if (tblColumn2 != null)
                    {
                      if (tblColumn2.Rows.Count > 0)
                      {
                        sNAvance = new StringBuilder();
                        sNAvance.Append("<div class=\"").Append(oWeb.getColorNumAvance(double.Parse(iDato), tblColumn2));
                        sNAvance.Append("\">").Append(String.Format("{0:0}", double.Parse(iDato))).Append("</div>");

                        htmlTblCell = new HtmlTableCell();
                        htmlTblCell.Attributes.Add("align", "center");
                        htmlTblCell.Attributes.Add("class", "clDtPanel");
                        htmlTblCell.Controls.Add(new LiteralControl(sNAvance.ToString()));
                        htmlTblRowKpi.Controls.Add(htmlTblCell);

                        htmlTblCell = new HtmlTableCell();
                        htmlTblCell.Attributes.Add("class", "clDtPanel");
                        htmlTblCell.Controls.Add(new LiteralControl("<div class=\"tbDat\">" + tblColumn2.Rows[0]["criterio_aceptacion"].ToString() + " " + tblColumn2.Rows[0]["unidad"].ToString() + "</div>"));
                        htmlTblRowCriAcep.Controls.Add(htmlTblCell);

                        htmlTblCell = new HtmlTableCell();
                        htmlTblCell.Attributes.Add("class", "clDtPanel");
                        htmlTblCell.Controls.Add(new LiteralControl("<div class=\"tbDat\">" + tblColumn2.Rows[0]["sla"].ToString() + "</div>"));
                        htmlTblRowSLA.Controls.Add(htmlTblCell);
                      }
                      tblColumn2.Dispose();
                    }
                    tblColumn2 = null;

                  }
                  tblColumn1.Dispose();
                }
                tblColumn1 = null;
                oIndResultado.IdFecha = idFechaMesAnterior.ToString("yyyyMMddHHmm");
                tblColumn1 = oIndResultado.GetMesAnterior();
                if (tblColumn1 != null)
                {
                  if (tblColumn1.Rows.Count > 0)
                  {
                    htmlTblCell = new HtmlTableCell();
                    htmlTblCell.Attributes.Add("class", "clDtPanel");

                    oDivImg = new HtmlGenericControl("div");
                    oDivImg.Attributes.Add("class", "tbDat");

                    oDivImg.Controls.Add(new LiteralControl("<img src=\"" + oWeb.getColorArrowCalidad(double.Parse(iDato), double.Parse(tblColumn1.Rows[0]["valor"].ToString())) + "\" border=\"0\">"));

                    htmlTblCell.Controls.Add(oDivImg);
                    htmlTblRowMesAnt.Controls.Add(htmlTblCell);
                  }
                  tblColumn1.Dispose();
                }
                tblColumn1 = null;
              }
              oIndResultado = null;
            }
          }
          tblAnalista = null;
        }
        dtKPIs = null;
      }
      oConn.Close();
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