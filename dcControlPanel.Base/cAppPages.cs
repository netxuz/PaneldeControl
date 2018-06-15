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
  public class cAppPages
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodPage;
    public string CodPage { get { return pCodPage; } set { pCodPage = value; } }

    private string pNomPage;
    public string NomPage { get { return pNomPage; } set { pNomPage = value; } }

    private string pCodTipo;
    public string CodTipo { get { return pCodTipo; } set { pCodTipo = value; } }

    private string pCodMonitor;
    public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private string pQuery = string.Empty;
    public string Query { get { return pQuery; } set { pQuery = value; } }

    private DBConn oConn;

    public cAppPages()
    {

    }

    public cAppPages(ref DBConn oConn)
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
        cSQL.Append("select cod_page, nom_page, url_page, tipo_consulta ");
        cSQL.Append("from app_pages ");

        if (!string.IsNullOrEmpty(pCodPage))
        {
          cSQL.Append("where cod_page = @cod_page ");
          oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
        }

        try
        {
          pQuery = cSQL.ToString();
          dtData = oConn.Select(cSQL.ToString(), oParam);
          return dtData;
        }
        catch
        {
          pQuery = oConn.ReturnQuery;
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

    public DataTable GetByType()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.cod_page, a.nom_page, a.url_page, a.tipo_consulta, b.cod_tipo  ");
        cSQL.Append("from app_pages a, apt_page_tipo_usuario b ");
        cSQL.Append(" where a.cod_page = b.cod_page ");
        cSQL.Append(" and not a.cod_page in(select cod_page from apt_monitor_pages where cod_monitor = @cod_monitor ) ");
        oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Varchar);

        if (!string.IsNullOrEmpty(pCodTipo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_tipo = @cod_tipo");
          oParam.AddParameters("@cod_tipo", pCodTipo, TypeSQL.Varchar);
        }

        try
        {
          pQuery = cSQL.ToString();
          dtData = oConn.Select(cSQL.ToString(), oParam);
          return dtData;
        }
        catch
        {
          pQuery = oConn.ReturnQuery;
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

    public DataTable GetByTipo()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.cod_page, a.nom_page, a.url_page, a.tipo_consulta, b.cod_tipo  ");
        cSQL.Append("from app_pages a, apt_page_tipo_usuario b ");
        cSQL.Append(" where a.cod_page = b.cod_page ");

        if (!string.IsNullOrEmpty(pCodTipo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_tipo = @cod_tipo");
          oParam.AddParameters("@cod_tipo", pCodTipo, TypeSQL.Varchar);
        }

        try
        {
          pQuery = cSQL.ToString();
          dtData = oConn.Select(cSQL.ToString(), oParam);
          return dtData;
        }
        catch
        {
          pQuery = oConn.ReturnQuery;
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

    public void Put()
    {
      DataTable dtData;
      oParam = new DBConn.SQLParameters(10);
      StringBuilder cSQL;
      string sComa = string.Empty;

      if (oConn.bIsOpen)
      {
        try
        {
          switch (pAccion)
          {
            case "CREAR":
              pCodPage = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
              cSQL = new StringBuilder();
              cSQL.Append("insert into app_pages(cod_page, nom_page) values(");
              cSQL.Append("@cod_page, @nom_page) ");
              oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
              oParam.AddParameters("@nom_page", pNomPage, TypeSQL.Varchar);
              oConn.Insert(cSQL.ToString(), oParam);

              if (!string.IsNullOrEmpty(oConn.Error))
                pError = oConn.Error;
              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update app_pages set ");

              if (!string.IsNullOrEmpty(pNomPage))
              {
                cSQL.Append(sComa);
                cSQL.Append(" nom_page = @nom_page");
                oParam.AddParameters("@nom_page", pNomPage, TypeSQL.Varchar);
                sComa = ", ";
              }

              cSQL.Append(" where cod_page = @cod_page ");
              oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              if (!string.IsNullOrEmpty(oConn.Error))
                pError = oConn.Error;
              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from app_pages where cod_page = @cod_page ");
              oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
              oConn.Delete(cSQL.ToString(), oParam);

              if (!string.IsNullOrEmpty(oConn.Error))
                pError = oConn.Error;
              break;
          }
        }
        catch (Exception Ex)
        {
          pError = Ex.Message;
        }
      }
    }
  }
}
