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
  public partial class vistaclienteconfig : System.Web.UI.Page
  {
    Web oWeb = new Web();

    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSessionAdm();
      if (!IsPostBack)
      {
        CodCliente.Value = oWeb.GetData("CodCliente");
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
          oSysUsuario.CodUser = CodCliente.Value;
          DataTable dtUser = oSysUsuario.GetSysUsuario();
          if (dtUser != null)
          {
            Label1.Text = dtUser.Rows[0]["nom_user"].ToString() + " " + dtUser.Rows[0]["ape_user"].ToString();
          }
          dtUser = null;
        }
        oConn.Close();
        Load_ListBox();
      }
    }

    protected void Load_ListBox()
    {

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAppVistasCliente oAppVistasCliente = new cAppVistasCliente(ref oConn);
        oAppVistasCliente.CodCliente = CodCliente.Value;
        oAppVistasCliente.NotIn = true;
        DataTable dtVistaCliente = oAppVistasCliente.Get();
        if (dtVistaCliente != null)
        {
          foreach (DataRow oRow in dtVistaCliente.Rows)
          {
            ListBox1.Items.Add(new ListItem(oRow["desc_monitor_view"].ToString(), oRow["cod_monitor"].ToString()));
          }
        }
        dtVistaCliente = null;

        oAppVistasCliente.NotIn = false;
        dtVistaCliente = oAppVistasCliente.Get();
        if (dtVistaCliente != null)
        {
          foreach (DataRow oRow in dtVistaCliente.Rows)
          {
            ListBox2.Items.Add(new ListItem(oRow["desc_monitor_view"].ToString(), oRow["cod_monitor"].ToString()));
          }
        }
        dtVistaCliente = null;
        oConn.Close();
      }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        if (ListBox1.SelectedIndex >= 0)
        {
          cAppVistasCliente oAppVistasCliente = new cAppVistasCliente(ref oConn);
          for (int i = 0; i < ListBox1.Items.Count; i++)
          {
            if (ListBox1.Items[i].Selected)
            {
              oAppVistasCliente.CodCliente = CodCliente.Value;
              oAppVistasCliente.CodMonitor = ListBox1.Items[i].Value;
              oAppVistasCliente.Accion = "CREAR";
              oAppVistasCliente.Put();
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
          cAppVistasCliente oAppVistasCliente = new cAppVistasCliente(ref oConn);
          for (int i = 0; i < ListBox2.Items.Count; i++)
          {
            if (ListBox2.Items[i].Selected)
            {
              oAppVistasCliente.CodCliente = CodCliente.Value;
              oAppVistasCliente.CodMonitor = ListBox2.Items[i].Value;
              oAppVistasCliente.Accion = "ELIMINAR";
              oAppVistasCliente.Put();
            }
          }
          ListBox1.Items.Clear();
          ListBox2.Items.Clear();
          Load_ListBox();
        }
        oConn.Close();
      }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("vistaclienteview.aspx");
    }

    protected void btnPageCliente_Click(object sender, EventArgs e)
    {
      string sUrl = "set_kpi_pages_list.aspx?CodCliente=" + CodCliente.Value;
      Response.Redirect(sUrl);
    }
  }
}