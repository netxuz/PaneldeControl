using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcControlPanel.Conn;

namespace dcControlPanel.Base
{
  public class cAptPagesKpi
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodKpi;
    public string CodKpi { get { return pCodKpi; } set { pCodKpi = value; } }

    private string pCodPage;
    public string CodPage { get { return pCodPage; } set { pCodPage = value; } }

    private string pCodCliente;
    public string CodCliente { get { return pCodCliente; } set { pCodCliente = value; } }

    private bool bINNOTIN;
    public bool INNOTIN { get { return bINNOTIN; } set { bINNOTIN = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private string pQuery = string.Empty;
    public string Query { get { return pQuery; } set { pQuery = value; } }

    private DBConn oConn;

    public cAptPagesKpi()
    {

    }

    public cAptPagesKpi(ref DBConn oConn)
    {
      this.oConn = oConn;
    }

    public DataTable Get()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select (select nombre_kpi from app_kpi where cod_kpi = a.cod_kpi) nombrekpi, ");
        cSQL.Append(" (select identificador_kpi from app_kpi where cod_kpi = a.cod_kpi) identificador_kpi, a.cod_kpi, a.cod_page ");

        if (string.IsNullOrEmpty(pCodCliente))
          cSQL.Append("from apt_pages_kpi a ");
        else
          cSQL.Append("from apt_kpi_page_cliente a ");

        cSQL.Append(" where a.cod_page = @cod_page ");
        oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);

        if (!string.IsNullOrEmpty(pCodCliente))
        {
          cSQL.Append(" and a.cod_cliente = @cod_cliente ");
          oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Int);
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

    public DataTable GetKpiNotIn()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_kpi, nombre_kpi, identificador_kpi from app_kpi ");
        cSQL.Append(" where not cod_kpi in(select cod_kpi from ");

        if (string.IsNullOrEmpty(pCodCliente))
          cSQL.Append("apt_pages_kpi ");
        else
          cSQL.Append("apt_kpi_page_cliente ");

        cSQL.Append("where cod_page = @cod_page");
        oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);

        if (!string.IsNullOrEmpty(pCodCliente))
        {
          cSQL.Append(" and cod_cliente = @cod_cliente ");
          oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Int);
        }

        cSQL.Append(")");

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

    public void DelAptKpiPageCliente()
    {
      oParam = new DBConn.SQLParameters(10);
      StringBuilder cSQL;
      string sComa = string.Empty;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        try
        {
          cSQL = new StringBuilder();
          cSQL.Append("delete from apt_kpi_page_cliente  ");

          if (!string.IsNullOrEmpty(pCodKpi))
          {
            cSQL.Append(Condicion);
            Condicion = " and ";
            cSQL.Append(" cod_kpi = @cod_kpi ");
            oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
          }
          oConn.Delete(cSQL.ToString(), oParam);

          if (!string.IsNullOrEmpty(oConn.Error))
            pError = oConn.Error;
          
        }
        catch (Exception Ex)
        {
          pError = Ex.Message;
        }
      }
    }

    public void Put()
    {
      oParam = new DBConn.SQLParameters(10);
      StringBuilder cSQL;
      string sComa = string.Empty;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        try
        {
          switch (pAccion)
          {
            case "CREAR":
              cSQL = new StringBuilder();

              if (string.IsNullOrEmpty(pCodCliente))
              {
                cSQL.Append("insert into apt_pages_kpi(cod_kpi, cod_page) values(");
                cSQL.Append("@cod_kpi, @cod_page) ");
                oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
                oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
              }
              else {
                cSQL.Append("insert into apt_kpi_page_cliente(cod_kpi, cod_page, cod_cliente) values(");
                cSQL.Append("@cod_kpi, @cod_page, @cod_cliente) ");
                oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
                oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
                oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Int);
              }

              
              oConn.Insert(cSQL.ToString(), oParam);

              if (!string.IsNullOrEmpty(oConn.Error))
                pError = oConn.Error;
              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();

              if (string.IsNullOrEmpty(pCodCliente))
                cSQL.Append("delete from apt_pages_kpi  ");
              else
                cSQL.Append("delete from apt_kpi_page_cliente  ");

              if (!string.IsNullOrEmpty(pCodKpi))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" cod_kpi = @cod_kpi ");
                oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
              }

              if (!string.IsNullOrEmpty(pCodPage))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" cod_page = @cod_page ");
                oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
              }

              if (!string.IsNullOrEmpty(pCodCliente))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" cod_cliente = @cod_cliente ");
                oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Int);
              }

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
