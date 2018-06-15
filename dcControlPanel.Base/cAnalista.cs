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
  public class cAnalista
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNCod;
    public string NCod { get { return pNCod; } set { pNCod = value; } }

    private string pSNombre;
    public string SNombre { get { return pSNombre; } set { pSNombre = value; } }

    private string pNKey_analista;
    public string NKey_analista { get { return pNKey_analista; } set { pNKey_analista = value; } }

    private string pTipoAnalista;
    public string TipoAnalista { get { return pTipoAnalista; } set { pTipoAnalista = value; } }

    private string pCodMonitor;
    public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

    private string pCodPage;
    public string CodPage { get { return pCodPage; } set { pCodPage = value; } }

    private string pActivo;
    public string Activo { get { return pActivo; } set { pActivo = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cAnalista()
    {

    }

    public cAnalista(ref DBConn oConn)
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
        //cSQL.Append("select nKey_Analista, nCod, sNombre, sLogin, sLoginAnalista, mail, tipo_analista, anexo, activo, interno, supervisor ");
        //cSQL.Append("from analista where nkey_analista in( select distinct nkey_analista from servicio where nKey_Cliente = @nKey_Cliente  ) ");
        //oParam.AddParameters("@nKey_Cliente", pNCod, TypeSQL.Numeric);
        //cSQL.Append(" order by snombre");

        cSQL.Append("select nKey_Analista, nCod, sNombre, sLogin, sLoginAnalista, mail, tipo_analista, anexo, activo, interno, supervisor ");
        cSQL.Append(" from analista where analista.nkey_analista in( select nkey_analista from servicio, factura where servicio.nKey_Cliente = @nKey_Cliente ");
        cSQL.Append(" and factura.nKey_Cliente = servicio.nKey_Cliente  ");
        cSQL.Append(" and servicio.nKey_Analista = analista.nKey_Analista and servicio.nKey_Deudor = factura.nKey_Deudor and dFechaEmision >= GETDATE() - 180 ) ");
        cSQL.Append(" and analista.activo = 'S' ");
        cSQL.Append(" order by analista.snombre ");

        oParam.AddParameters("@nKey_Cliente", pNCod, TypeSQL.Numeric);
        dtData = oConn.Select(cSQL.ToString(), oParam);
        pError = oConn.Error;
        return dtData;
      }
      else
      {
        pError = "Conexion Cerrada";
        return null;
      }
    }

    public DataTable GetBySecLevelSoc()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select distinct(servicio.nKey_Cliente), servicio.nkey_analista,  ");
        cSQL.Append(" (select sNombre from analista where nKey_Analista = servicio.nkey_analista and activo = 'S') nombre_analista ");
        cSQL.Append(" from servicio, factura where factura.nKey_Cliente = servicio.nKey_Cliente ");
        cSQL.Append(" and servicio.nkey_analista in (select nkey_analista from analista where nKey_Analista = servicio.nkey_analista and activo = 'S') ");
        cSQL.Append(" and servicio.nKey_Cliente in(select cod_cliente from apt_clientes_monitorpages where cod_monitor = @cod_monitor and cod_page = @cod_page )  ");
        cSQL.Append(" and servicio.nKey_Deudor = factura.nKey_Deudor and dFechaEmision >= GETDATE() - 180 ");
        cSQL.Append(" order by servicio.nKey_Cliente ");

        oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
        oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
        dtData = oConn.Select(cSQL.ToString(), oParam);
        pError = oConn.Error;
        return dtData;
      }
      else
      {
        pError = "Conexion Cerrada";
        return null;
      }
    }

    public DataTable GetBySecLevelHol()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select distinct((select ncodholding from cliente where nkey_cliente = servicio.nKey_Cliente)) as 'nKey_Cliente', servicio.nkey_analista,  ");
        cSQL.Append(" (select sNombre from analista where nKey_Analista = servicio.nkey_analista and activo = 'S') nombre_analista ");
        cSQL.Append(" from servicio, factura where factura.nKey_Cliente = servicio.nKey_Cliente ");
        cSQL.Append(" and servicio.nkey_analista in (select nkey_analista from analista where nKey_Analista = servicio.nkey_analista and activo = 'S') ");
        cSQL.Append(" and servicio.nKey_Cliente in(select nkey_cliente from cliente where ncodholding in(select cod_cliente from apt_clientes_monitorpages where cod_monitor = @cod_monitor and cod_page = @cod_page ) and bCuentaCorriente <> 0 ) ");
        cSQL.Append(" and servicio.nKey_Deudor = factura.nKey_Deudor and dFechaEmision >= GETDATE() - 180 ");
        cSQL.Append(" order by servicio.nKey_Cliente ");

        oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
        oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
        dtData = oConn.Select(cSQL.ToString(), oParam);
        pError = oConn.Error;
        return dtData;
      }
      else
      {
        pError = "Conexion Cerrada";
        return null;
      }
    }

  }
}
