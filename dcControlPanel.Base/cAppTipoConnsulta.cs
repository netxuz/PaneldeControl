using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Data;
using dcControlPanel.Conn;

namespace dcControlPanel.Base
{
  public class cAppTipoConnsulta
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodTipo;
    public string CodTipo { get { return pCodTipo; } set { pCodTipo = value; } }

    private string pNomTipo;
    public string NomTipo { get { return pNomTipo; } set { pNomTipo = value; } }

    private string pIndSegNivel;
    public string IndSegNivel { get { return pIndSegNivel; } set { pIndSegNivel = value; } }

    private string pCodMonitor;
    public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cAppTipoConnsulta()
    {

    }

    public cAppTipoConnsulta(ref DBConn oConn)
    {
      this.oConn = oConn;
    }

    public DataTable Get()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_tipo, nom_tipo ");
        cSQL.Append("from app_tipo_connsulta ");

        if (!string.IsNullOrEmpty(pCodTipo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_tipo = @cod_tipo");
          oParam.AddParameters("@cod_tipo", pCodTipo, TypeSQL.Char);
        }

        if (!string.IsNullOrEmpty(pIndSegNivel))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" ind_seg_nivel = @ind_seg_nivel");
          oParam.AddParameters("@ind_seg_nivel", pIndSegNivel, TypeSQL.Char);
        }

        try
        {
          dtData = oConn.Select(cSQL.ToString(), oParam);
          return dtData;
        }
        catch
        {
          pError = oConn.Error;
          return null;
        }
      }
      else
      {
        pError = "Conexion Cerrada";
        return null;
      }
    }

    public DataTable GetByMonitor()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_tipo, nom_tipo ");
        cSQL.Append("from app_tipo_connsulta ");
        //cSQL.Append("where not cod_tipo in(select cod_tipo from apt_page_tipo_usuario where cod_tipo = 'N' and cod_page in(select cod_page from apt_monitor_pages where cod_monitor = @cod_monitor )) ");
        //oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
        
        try
        {
          dtData = oConn.Select(cSQL.ToString());
          //dtData = oConn.Select(cSQL.ToString(), oParam);
          return dtData;
        }
        catch
        {
          pError = oConn.Error;
          return null;
        }
      }
      else
      {
        pError = "Conexion Cerrada";
        return null;
      }
    }

  }
}
