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
  public partial class mensajesview : System.Web.UI.Page
  {
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      Web oWeb = new Web();
      string sTimePage = string.Empty;
      string sUrlPage = string.Empty;
      string pCodMonitor = oWeb.GetData("codmonitor");
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
                sUrlPage = dtMPage.Rows[0]["url_page"].ToString();
                sUrlPage = sUrlPage + "?codmonitor=" + pCodMonitor;
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
        oConn.Close();
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      Web oWeb = new Web();
      string pCodMonitor = oWeb.GetData("codmonitor");
      string pCodUsuario = oWeb.GetData("codusuario");
      string pOrderPage = oWeb.GetData("order");
      string pCodCliente = string.Empty;
      string pTipoUsuario = string.Empty;
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
            }
          }
          dtMPage = null;

          cAptMonitorMensaje oAptMonitorMensaje = new cAptMonitorMensaje(ref oConn);
          oAptMonitorMensaje.CodMonitor = pCodMonitor;
          DataTable dtMntMsj = oAptMonitorMensaje.GeMensajesByMonitor();
          if (dtMntMsj != null)
          {
            if (dtMntMsj.Rows.Count > 0)
            {
              Label oLabel;
              foreach (DataRow oRow in dtMntMsj.Rows)
              {
                oLabel = new Label();
                oLabel.Text = oRow["texto_mensaje"].ToString();
                oLabel.CssClass = "styleMensaje";
                idMarque.Controls.Add(oLabel);
                idMarque.Controls.Add(new LiteralControl("<br><br><br>"));
              }
            }
          }
          dtMntMsj = null;


          oCmbTipoConsulta.Value = pTipoUsuario;
          oCmbCliente.Value = pCodCliente;
          imglogo.ImageUrl = "images/logos/logo.jpg";

        }
        oConn.Close();
      }

    }
  }
}