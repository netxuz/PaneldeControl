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
  public partial class mensajesconfig : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        CodMensaje.Value = oWeb.GetData("CodMensaje");
        if (!string.IsNullOrEmpty(CodMensaje.Value))
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            //btnGrabar1.Visible = false;

            cAppMensaje oAppMensaje = new cAppMensaje(ref oConn);
            oAppMensaje.CodMensaje = CodMensaje.Value;
            DataTable dtMensaje = oAppMensaje.Get();
            if (dtMensaje != null)
            {
              if (dtMensaje.Rows.Count > 0)
              {
                txt_nombre.Text = dtMensaje.Rows[0]["desc_mensaje"].ToString();
                //txtMensaje.Text = dtMensaje.Rows[0]["texto_mensaje"].ToString();
                rdMensaje.Content = dtMensaje.Rows[0]["texto_mensaje"].ToString();
                oCmbEstado.Items.FindByValue(dtMensaje.Rows[0]["est_mensaje"].ToString()).Selected = true;
              }
            }
            dtMensaje = null;
            trMsjsView.Visible = true;
            Load_ListBox();
            oConn.Close();
          }
        }
      }
    }

    protected void Load_ListBox()
    {

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAptMonitorMensaje oAptMonitorMensaje = new cAptMonitorMensaje(ref oConn);
        oAptMonitorMensaje.CodMensaje = CodMensaje.Value;
        oAptMonitorMensaje.NotIn = true;
        DataTable dtMonitor = oAptMonitorMensaje.GetMonitorByMsj();
        if (dtMonitor != null)
        {
          foreach (DataRow oRow in dtMonitor.Rows)
          {
            ListBox1.Items.Add(new ListItem(oRow["desc_monitor_view"].ToString(), oRow["cod_monitor"].ToString()));
          }
        }
        dtMonitor = null;

        oAptMonitorMensaje.NotIn = false;
        dtMonitor = oAptMonitorMensaje.GetMonitorByMsj();
        if (dtMonitor != null)
        {
          foreach (DataRow oRow in dtMonitor.Rows)
          {
            ListBox2.Items.Add(new ListItem(oRow["desc_monitor_view"].ToString(), oRow["cod_monitor"].ToString()));
          }
        }
        dtMonitor = null;
        oConn.Close();
      }
    }

    protected void btnGrabar1_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppMensaje oAppMensaje = new cAppMensaje(ref oConn);
        oAppMensaje.CodMensaje = CodMensaje.Value;
        oAppMensaje.DescMensaje = txt_nombre.Text;
        //oAppMensaje.TextoMensaje = txtMensaje.Text;
        oAppMensaje.TextoMensaje = rdMensaje.Content;
        oAppMensaje.EstMensaje = oCmbEstado.SelectedValue.ToString();
        oAppMensaje.Accion = (string.IsNullOrEmpty(CodMensaje.Value) ? "CREAR" : "EDITAR");
        oAppMensaje.Put();

        if (!string.IsNullOrEmpty(oAppMensaje.Error))
        {
          Response.Write("Error : " + oAppMensaje.Error + "<br>");
          Response.End();
        }

        CodMensaje.Value = oAppMensaje.CodMensaje;
        trMsjsView.Visible = true;
        ListBox1.Items.Clear();
        ListBox2.Items.Clear();
        Load_ListBox();

        oConn.Close();
      }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("mensajes.aspx");
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        if (ListBox1.SelectedIndex >= 0)
        {
          cAptMonitorMensaje oAptMonitorMensaje = new cAptMonitorMensaje(ref oConn);
          for (int i = 0; i < ListBox1.Items.Count; i++)
          {
            if (ListBox1.Items[i].Selected)
            {
              oAptMonitorMensaje.CodMensaje = CodMensaje.Value;
              oAptMonitorMensaje.CodMonitor = ListBox1.Items[i].Value;
              oAptMonitorMensaje.Accion = "CREAR";
              oAptMonitorMensaje.Put();
            }
          }
          ListBox1.Items.Clear();
          ListBox2.Items.Clear();
          Load_ListBox();
        }
        oConn.Close();
      }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        if (ListBox2.SelectedIndex >= 0)
        {
          cAptMonitorMensaje oAptMonitorMensaje = new cAptMonitorMensaje(ref oConn);
          for (int i = 0; i < ListBox2.Items.Count; i++)
          {
            if (ListBox2.Items[i].Selected)
            {
              oAptMonitorMensaje.CodMensaje = CodMensaje.Value;
              oAptMonitorMensaje.CodMonitor = ListBox2.Items[i].Value;
              oAptMonitorMensaje.Accion = "ELIMINAR";
              oAptMonitorMensaje.Put();
            }
          }
          ListBox1.Items.Clear();
          ListBox2.Items.Clear();
          Load_ListBox();
        }
        oConn.Close();
      }
    }


  }
}