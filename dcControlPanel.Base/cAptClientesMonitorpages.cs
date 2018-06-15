using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcControlPanel.Conn;

namespace dcControlPanel.Base
{
  public class cAptClientesMonitorpages
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodMonitor;
    public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

    private string pCodPage;
    public string CodPage { get { return pCodPage; } set { pCodPage = value; } }

    private string pCodCliente;
    public string CodCliente { get { return pCodCliente; } set { pCodCliente = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private string pQuery = string.Empty;
    public string Query { get { return pQuery; } set { pQuery = value; } }

    private DBConn oConn;

    public cAptClientesMonitorpages()
    {

    }

    public cAptClientesMonitorpages(ref DBConn oConn)
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
        try
        {
          cSQL = new StringBuilder();
          cSQL.Append("select cod_monitor, cod_page, cod_cliente ");
          cSQL.Append("from apt_clientes_monitorpages ");

          if (!string.IsNullOrEmpty(pCodMonitor))
          {
            cSQL.Append(Condicion);
            Condicion = " and ";
            cSQL.Append(" cod_monitor  = @cod_monitor ");
            oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
          }

          if (!string.IsNullOrEmpty(pCodPage))
          {
            cSQL.Append(Condicion);
            Condicion = " and ";
            cSQL.Append(" cod_page  = @cod_page ");
            oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
          }

          if (!string.IsNullOrEmpty(pCodCliente))
          {
            cSQL.Append(Condicion);
            Condicion = " and ";
            cSQL.Append(" cod_cliente  = @cod_cliente ");
            oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Numeric);
          }

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
              cSQL = new StringBuilder();
              cSQL.Append("insert into apt_clientes_monitorpages(cod_monitor, cod_page, cod_cliente) values(");
              cSQL.Append("@cod_monitor, @cod_page, @cod_cliente) ");
              oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
              oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
              oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Numeric);
              oConn.Insert(cSQL.ToString(), oParam);

              if (!string.IsNullOrEmpty(oConn.Error))
              {
                pQuery = oConn.ReturnQuery;
                pError = oConn.Error;
              }
              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from apt_clientes_monitorpages where cod_monitor = @cod_monitor ");
              oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
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
