using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using dcControlPanel.Conn;
using dcControlPanel.Method;
using dcControlPanel.Base;
using Telerik.Web.UI;

namespace dcControlPanel
{
  public partial class LogoCliente : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        string sFile = string.Empty;
        CodCliente.Value = oWeb.GetData("CodCliente");
        hddTipo.Value = oWeb.GetData("tp");
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cCliente oCliente = new cCliente(ref oConn);
          oCliente.NKey_cliente = CodCliente.Value;
          DataTable dtCliente = oCliente.Get();
          if (dtCliente.Rows.Count > 0)
          {
            Label1.Text = dtCliente.Rows[0]["sNombre"].ToString();
          }

          cAppLogoCliente oAppLogoCliente = new cAppLogoCliente(ref oConn);
          oAppLogoCliente.NKey_cliente = CodCliente.Value;
          DataTable dtLogo = oAppLogoCliente.Get();
          if (dtLogo != null)
          {
            if (dtLogo.Rows.Count > 0)
            {
              hddAccion.Value = "EDITAR";
              if (!string.IsNullOrEmpty(dtLogo.Rows[0]["logo_cliente"].ToString()))
              {
                sFile = dtLogo.Rows[0]["logo_cliente"].ToString();
                getImage(sFile);
              }
              else
                getImage("nada.jpg");
            }
          }
        }
        oConn.Close();
      }
    }

    protected void Customvalidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
      args.IsValid = (RadUpload1.InvalidFiles.Count == 0);
    }

    protected void btnGrabar1_Click(object sender, EventArgs e)
    {
      try
      {
        string sFile = string.Empty;
        StringBuilder sPath = new StringBuilder();
        sPath.Append(Server.MapPath("."));
        sPath.Append(@"\images\logos\");

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          foreach (string fileInputID in Request.Files)
          {
            UploadedFile file = UploadedFile.FromHttpPostedFile(Request.Files[fileInputID]);
            if (file.ContentLength > 0)
            {
              sFile = file.GetName();
              file.SaveAs(sPath.ToString() + file.GetName());
            }
          }
          cAppLogoCliente oAppLogoCliente = new cAppLogoCliente(ref oConn);
          oAppLogoCliente.NKey_cliente = CodCliente.Value;
          oAppLogoCliente.LogoCliente = sFile;
          oAppLogoCliente.Tipo = (string.IsNullOrEmpty(hddTipo.Value) ? "C" : hddTipo.Value);
          oAppLogoCliente.Accion = (string.IsNullOrEmpty(hddAccion.Value) ? "CREAR" : "EDITAR");
          oAppLogoCliente.Put();

          getImage(sFile);
        }
        oConn.Close();
      }
      catch (Exception Ex)
      {
        Response.Write("Error: " + Ex.Message);
      }
    }

    private void getImage(string sImage)
    {
      imglogo.ImageUrl = "images/logos/" + sImage;
    }
    protected void btnVolver_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(hddTipo.Value))
        Response.Redirect("logoclienteview.aspx");
      else
        Response.Redirect("logoholdingview.aspx");
    }

    protected void btnGrabar2_Click(object sender, EventArgs e)
    {
      try
      {

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          string sFile = string.Empty;
          StringBuilder sPath = new StringBuilder();
          sPath.Append(Server.MapPath("."));
          sPath.Append(@"\images\logos\");


          cAppLogoCliente oAppLogoCliente = new cAppLogoCliente(ref oConn);
          oAppLogoCliente.NKey_cliente = CodCliente.Value;
          oAppLogoCliente.LogoCliente = string.Empty;
          oAppLogoCliente.Tipo = string.Empty;
          DataTable dtlogo = oAppLogoCliente.Get();

          if (dtlogo != null)
          {
            sFile = dtlogo.Rows[0]["logo_cliente"].ToString();
            sPath.Append(sFile);
            File.Delete(sPath.ToString());
          }
          dtlogo = null;

          oAppLogoCliente.Accion = "EDITAR";
          oAppLogoCliente.Put();

          getImage("nada.jpg");
        }
        oConn.Close();
      }
      catch (Exception Ex)
      {
        Response.Write("Error: " + Ex.Message);
      }
    }
  }
}