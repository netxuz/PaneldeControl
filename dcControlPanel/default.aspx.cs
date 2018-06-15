using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using dcControlPanel.Conn;
using dcControlPanel.Base;
using dcControlPanel.Method;

namespace dcControlPanel
{
  public partial class _default : System.Web.UI.Page
  {
    Web oWeb = new Web();
    dcControlPanel.Method.Usuario oIsUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oIsUsuario = oWeb.GetObjUsuario();

      HttpCookie oPanelControl = Request.Cookies["uPanelControl"];
      if (oPanelControl != null)
      {
        string susername = Request.Cookies["uPanelControl"].Values["upanel"];
        string spassword = Request.Cookies["uPanelControl"].Values["ppanel"];

        if ((!string.IsNullOrEmpty(susername))&&(!string.IsNullOrEmpty(spassword)))
          goLogin(susername, spassword);
      }
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
      goLogin(txtLogin.Text, txtPassword.Text);
    }

    protected void goLogin(string sLogin, string sPwd) {
      bool bExito = false;
      string sComilla = Convert.ToChar(39).ToString();
      string sComillaDoble = Convert.ToChar(39).ToString() + Convert.ToChar(39).ToString();

      sLogin = sLogin.Replace(sComilla, sComillaDoble);
      sPwd = sPwd.Replace(sComilla, sComillaDoble);


      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
        oSysUsuario.LoginUser = sLogin;
        oSysUsuario.PwdUser = oWeb.Crypt(sPwd);
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

            bExito = true;

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

            HttpCookie cookie = new HttpCookie("uPanelControl");
            cookie.Values.Add("upanel", sLogin);
            cookie.Values.Add("ppanel", sPwd);
            cookie.Expires = DateTime.MaxValue;
            Response.Cookies.Add(cookie);

          }
        }
        dtUser = null;
      }
      oConn.Close();

      if (bExito)
      {
        Session["USUARIO"] = oIsUsuario;
        if ((HttpContext.Current.Session["AdministradorPanel"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Session["AdministradorPanel"].ToString())))
          Response.Redirect("welcome.aspx");
        else
          Response.Redirect("monitoresclientes.aspx?CodCliente=" + oIsUsuario.CodUsuario);
      }
      else
      {
        error.Visible = true;
      }
    }
  }
}