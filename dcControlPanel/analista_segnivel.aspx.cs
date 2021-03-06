﻿using System;
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
  public partial class analista_segnivel : System.Web.UI.Page
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

      if (!string.IsNullOrEmpty(pAuthenCodUser))
      {
        AuthenticUser(pAuthenCodUser);
      }

      oWeb.ValidaUserPanel();

      if (oConn.Open())
      {
        if (!IsPostBack)
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
              lblSubTitle.Text = dtMPage.Rows[0]["nom_page"].ToString();
            }
          }
          dtMPage = null;

          oCmbTipoConsulta.Value = pTipoUsuario;
          
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

        cAptPagesKpi oAptPagesKpi = new cAptPagesKpi(ref oConn);
        oAptPagesKpi.CodCliente = pCodUsuario;
        oAptPagesKpi.CodPage = pCodPage;
        DataTable dtKPIs = oAptPagesKpi.Get();
        if (dtKPIs != null)
        {
          HtmlTableCell oCell;
          foreach (DataRow oRow in dtKPIs.Rows)
          {
            string iDato = "0";
            string sKeyTipo = string.Empty;
            oCell = new HtmlTableCell();
            oCell.ID = "tdCell_" + oRow["identificador_kpi"].ToString();
            oCell.Controls.Add(new LiteralControl("<div align=\"center\" class=\"tbTit\">" + oRow["nombrekpi"].ToString() + "</div>"));
            HeaderId.Controls.Add(oCell);
          }
        }
        dtKPIs = null;

        cAnalista oAnalista;
        DataTable tblAnalista = null;

        switch (pTipoUsuario) {
          case "A":
            oAnalista = new cAnalista(ref oConn);
            oAnalista.CodMonitor = pCodMonitor;
            oAnalista.CodPage = pCodPage;
            tblAnalista = oAnalista.GetBySecLevelSoc();

            break;
          case "G":
            oAnalista = new cAnalista(ref oConn);
            oAnalista.CodMonitor = pCodMonitor;
            oAnalista.CodPage = pCodPage;
            tblAnalista = oAnalista.GetBySecLevelHol();

            break;
          case "P":
            cAplicador oAplicador = new cAplicador(ref oConn);
            oAplicador.CodMonitor = pCodMonitor;
            oAplicador.CodPage = pCodPage;
            tblAnalista = oAplicador.Get();

            break;
          case "R":
            cCobrador oCobrador = new cCobrador(ref oConn);
            oCobrador.CodMonitor = pCodMonitor;
            oCobrador.CodPage = pCodPage;
            tblAnalista = oCobrador.Get();

            break;
          case "D":
            cDigitador oDigitador = new cDigitador(ref oConn);
            oDigitador.CodMonitor = pCodMonitor;
            oDigitador.CodPage = pCodPage;
            tblAnalista = oDigitador.Get();

            break;
        }

        
        if (tblAnalista != null)
        {
          HtmlTableRow oTbRow;
          HtmlTableCell oCell;
          foreach (DataRow oRow in tblAnalista.Rows)
          {
            
            oTbRow = new HtmlTableRow();
            oTbRow.ID = oRow["nKey_Cliente"].ToString() + "row";

            oCell = new HtmlTableCell();
            oCell.Attributes.Add("class", "clPanel");
            cAppLogoCliente oAppLogoCliente = new cAppLogoCliente(ref oConn);
            oAppLogoCliente.NKey_cliente = oRow["nKey_Cliente"].ToString();
            //oAppLogoCliente.Tipo = "C";
            DataTable dtlogo = oAppLogoCliente.Get();
            if (dtlogo != null)
            {
              if (dtlogo.Rows.Count > 0)
              {
                Image oImage = new Image();
                oImage.CssClass = "imgLogoPanel";
                oImage.ImageUrl = "images/logos/" + dtlogo.Rows[0]["logo_cliente"].ToString();
                oCell.Controls.Add(oImage);
              }
            }
            oTbRow.Controls.Add(oCell);

            oCell = new HtmlTableCell();
            oCell.Attributes.Add("class", "clPanel");
            oCell.Controls.Add(new LiteralControl("<div class=\"tbTitRow\">" + oRow["nombre_analista"].ToString() + "</div>"));
            oTbRow.Controls.Add(oCell);
            

            oAptPagesKpi = new cAptPagesKpi(ref oConn);
            //oAptPagesKpi.CodCliente = oRow["nKey_Cliente"].ToString();
            oAptPagesKpi.CodPage = pCodPage;
            dtKPIs = oAptPagesKpi.Get();

            if (dtKPIs != null)
            {
              foreach (DataRow oRowKPIs in dtKPIs.Rows)
              {
                cIndresultado oIndResultado = new cIndresultado(ref oConn);
                oIndResultado.Tipo = oCmbTipoConsulta.Value;
                oIndResultado.KeyCliente = oRow["nKey_Cliente"].ToString();
                oIndResultado.KeyTipo = oRow["nKey_Analista"].ToString();
                oIndResultado.Indicador = oRowKPIs["identificador_kpi"].ToString();
                oIndResultado.IdFecha = sIdFecha;
                DataTable tblColumn1 = oIndResultado.Get();
                if (tblColumn1 != null)
                {
                  if (tblColumn1.Rows.Count > 0)
                  {
                    string iDato = (!string.IsNullOrEmpty(tblColumn1.Rows[0]["valor"].ToString()) ? tblColumn1.Rows[0]["valor"].ToString() : "0");
                    string sKeyTipo = tblColumn1.Rows[0]["nkey_tipo"].ToString();

                    cIndumbral oIndumbral = new cIndumbral(ref oConn);
                    oIndumbral.KeyCliente = oRow["nKey_Cliente"].ToString();
                    oIndumbral.Indicador = oRowKPIs["identificador_kpi"].ToString();
                    oIndumbral.KeyTipo = sKeyTipo;
                    DataTable tblColumn2 = oIndumbral.Get();
                    if (tblColumn2 != null)
                    {
                      if (tblColumn2.Rows.Count > 0)
                      {
                        StringBuilder sNAvance = new StringBuilder();
                        sNAvance.Append("<div class=\"").Append(oWeb.getColorNumAvance(double.Parse(iDato), tblColumn2));
                        sNAvance.Append("\">").Append(String.Format("{0:0}", double.Parse(iDato))).Append("</div>");

                        oCell = new HtmlTableCell();
                        oCell.Attributes.Add("align", "center");
                        oCell.Attributes.Add("class", "clDtPanel");
                        oCell.Controls.Add(new LiteralControl(sNAvance.ToString()));
                        oTbRow.Controls.Add(oCell);
                      }
                    }
                    tblColumn2 = null;
                  }
                  else {
                    oCell = new HtmlTableCell();
                    oCell.Attributes.Add("align", "center");
                    oCell.Attributes.Add("class", "clDtPanel");
                    //oCell.Controls.Add(new LiteralControl(""));
                    oTbRow.Controls.Add(oCell);
                  }
                }
              }
            }
            idTblSegNiv.Controls.Add(oTbRow);
          }
        }
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