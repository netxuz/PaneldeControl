using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcControlPanel.Conn;

namespace dcControlPanel.Base
{
  public class cAppKpi
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodKpi;
    public string CodKpi { get { return pCodKpi; } set { pCodKpi = value; } }

    private string pNombreKpi;
    public string NombreKpi { get { return pNombreKpi; } set { pNombreKpi = value; } }

    private string pIdentificadorKpi;
    public string IdentificadorKpi { get { return pIdentificadorKpi; } set { pIdentificadorKpi = value; } }

    private string pEstadoKpi;
    public string EstadoKpi { get { return pEstadoKpi; } set { pEstadoKpi = value; } }

    private string pSimboloKpi;
    public string SimboloKpi { get { return pSimboloKpi; } set { pSimboloKpi = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private string pQuery = string.Empty;
    public string Query { get { return pQuery; } set { pQuery = value; } }

    private DBConn oConn;

    public cAppKpi() {

    }

    public cAppKpi(ref DBConn oConn) {
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
        cSQL.Append("select cod_kpi, nombre_kpi, identificador_kpi, estado_kpi ");
        cSQL.Append("from app_kpi ");

        if (!string.IsNullOrEmpty(pCodKpi))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_kpi = @cod_kpi ");
          oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pNombreKpi)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nombre_kpi like '%' + @nombre_kpi + '%' ");
          oParam.AddParameters("@nombre_kpi", pNombreKpi, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pIdentificadorKpi))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" identificador_kpi = @identificador_kpi ");
          oParam.AddParameters("@identificador_kpi", pIdentificadorKpi, TypeSQL.Int);
        }

        if (!string.IsNullOrEmpty(pEstadoKpi))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" estado_kpi = @estado_kpi ");
          oParam.AddParameters("@estado_kpi", pEstadoKpi, TypeSQL.Char);
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
              pCodKpi = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
              cSQL = new StringBuilder();
              cSQL.Append("insert into app_kpi(cod_kpi, nombre_kpi, identificador_kpi, estado_kpi) values(");
              cSQL.Append("@cod_kpi, @nombre_kpi, @identificador_kpi, @estado_kpi) ");
              oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
              oParam.AddParameters("@nombre_kpi", pNombreKpi, TypeSQL.Varchar);
              oParam.AddParameters("@identificador_kpi", pIdentificadorKpi, TypeSQL.Int);
              oParam.AddParameters("@estado_kpi", pEstadoKpi, TypeSQL.Char);
              oConn.Insert(cSQL.ToString(), oParam);

              if (!string.IsNullOrEmpty(oConn.Error))
                pError = oConn.Error;
              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update app_kpi set ");

              if (!string.IsNullOrEmpty(pNombreKpi))
              {
                cSQL.Append(sComa);
                cSQL.Append(" nombre_kpi = @nombre_kpi");
                oParam.AddParameters("@nombre_kpi", pNombreKpi, TypeSQL.Varchar);
                sComa = ", ";
              }

              if (!string.IsNullOrEmpty(pIdentificadorKpi))
              {
                cSQL.Append(sComa);
                cSQL.Append(" identificador_kpi = @identificador_kpi");
                oParam.AddParameters("@identificador_kpi", pIdentificadorKpi, TypeSQL.Int);
                sComa = ", ";
              }

              cSQL.Append(" where cod_kpi = @cod_kpi ");
              oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              if (!string.IsNullOrEmpty(oConn.Error))
                pError = oConn.Error;
              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from app_kpi where cod_kpi = @cod_kpi ");
              oParam.AddParameters("@cod_kpi", pCodKpi, TypeSQL.Numeric);
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

