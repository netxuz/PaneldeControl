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
  public class cCliente
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNCod;
    public string NCod { get { return pNCod; } set { pNCod = value; } }

    private string pSNombre;
    public string SNombre { get { return pSNombre; } set { pSNombre = value; } }

    private string pNKey_cliente;
    public string NKey_cliente { get { return pNKey_cliente; } set { pNKey_cliente = value; } }

    private string pNCodHolding;
    public string NCodHolding { get { return pNCodHolding; } set { pNCodHolding = value; } }

    private string pCodMonitor;
    public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

    private string pCodPage;
    public string CodPage { get { return pCodPage; } set { pCodPage = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private string pQuery = string.Empty;
    public string Query { get { return pQuery; } set { pQuery = value; } }

    private DBConn oConn;

    public cCliente()
    {

    }

    public cCliente(ref DBConn oConn)
    {
      this.oConn = oConn;
    }

    public DataTable Get()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select ncod, snombre, nkey_cliente ");
        cSQL.Append("from cliente where bCuentaCorriente <> 0 ");

        if (!string.IsNullOrEmpty(pNKey_cliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNKey_cliente, TypeSQL.Varchar);
        }

        if (!string.IsNullOrEmpty(pSNombre))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" snombre like '%' + @snombre + '%' ");
          oParam.AddParameters("@snombre", pSNombre, TypeSQL.Varchar);
        }

        cSQL.Append(" order by snombre");

        try
        {
          pQuery = cSQL.ToString();
          dtData = oConn.Select(cSQL.ToString(), oParam);
          pError = oConn.Error;
          return dtData;
        }
        catch
        {
          pQuery = cSQL.ToString();
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

    public DataTable GetHolding()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select distinct ncodholding, holding ");
        cSQL.Append("from cliente where bCuentaCorriente <> 0 ");

        if (!string.IsNullOrEmpty(pNCodHolding))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" ncodholding = @ncodholding");
          oParam.AddParameters("@ncodholding", pNCodHolding, TypeSQL.Varchar);
        }

        if (!string.IsNullOrEmpty(pSNombre))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" snombre like '%' + @snombre + '%' ");
          oParam.AddParameters("@snombre", pSNombre, TypeSQL.Varchar);
        }

        cSQL.Append(" order by holding");

        try
        {
          pQuery = cSQL.ToString();
          dtData = oConn.Select(cSQL.ToString(), oParam);
          pError = oConn.Error;
          return dtData;
        }
        catch
        {
          pQuery = cSQL.ToString();
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

    public DataTable GetClientByPageMonitor()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select ncod, snombre, nkey_cliente ");
        cSQL.Append("from cliente where bCuentaCorriente <> 0 ");
        cSQL.Append("and not nkey_cliente in(select cod_cliente from apt_clientes_monitorpages where cod_monitor = @cod_monitor and cod_page = @cod_page )");
        oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Varchar);
        oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Varchar);
        cSQL.Append(" order by snombre");

        try
        {
          pQuery = cSQL.ToString();
          dtData = oConn.Select(cSQL.ToString(), oParam);
          pError = oConn.Error;
          return dtData;
        }
        catch
        {
          pQuery = cSQL.ToString();
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

    public DataTable GetHoldingByPageMonitor()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select distinct ncodholding as nkey_cliente, holding as snombre ");
        cSQL.Append("from cliente where bCuentaCorriente <> 0 ");
        cSQL.Append("and not ncodholding in(select cod_cliente from apt_clientes_monitorpages where cod_monitor = @cod_monitor and cod_page = @cod_page )");
        oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Varchar);
        oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Varchar);
        cSQL.Append(" order by holding");

        try
        {
          pQuery = cSQL.ToString();
          dtData = oConn.Select(cSQL.ToString(), oParam);
          pError = oConn.Error;
          return dtData;
        }
        catch
        {
          pQuery = cSQL.ToString();
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
