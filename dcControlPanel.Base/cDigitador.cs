using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcControlPanel.Conn;

namespace dcControlPanel.Base
{
  public class cDigitador
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

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

    public cDigitador()
    {

    }

    public cDigitador(ref DBConn oConn)
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

        //cSQL.Append("select a.nKey_Digitador as nKey_Analista, a.snombre as nombre_analista, b.ncodholding as 'nKey_Cliente' ");
        //cSQL.Append(" from digitador a, cliente b where a.nCod = b.nCod ");
        //cSQL.Append(" and b.ncodholding in (select cod_cliente from apt_clientes_monitorpages where cod_monitor = @codmonitor and cod_page = @codpage ) ");
        //cSQL.Append(" and a.activo = 'S' ");
        //cSQL.Append(" and b.bCuentaCorriente <> 0 ");
        //cSQL.Append(" order by a.snombre ");

        cSQL.Append("select distinct((select ncodholding from cliente where nkey_cliente = servicio.nKey_Cliente)) as 'nKey_Cliente', servicio.nkey_digitador as nKey_Analista,  ");
        cSQL.Append("(select sNombre from digitador where nkey_digitador = servicio.nkey_digitador and activo = 'S') nombre_analista ");
        cSQL.Append("from servicio, factura where factura.nKey_Cliente = servicio.nKey_Cliente  ");
        cSQL.Append("and servicio.nkey_digitador in (select nkey_digitador from digitador where nkey_digitador = servicio.nkey_digitador and activo = 'S') ");
        cSQL.Append("and servicio.nKey_Cliente in (select nkey_cliente from cliente where  ncodholding in (select cod_cliente from apt_clientes_monitorpages where cod_monitor = @codmonitor and cod_page = @codpage ) and bCuentaCorriente <> 0) ");
        cSQL.Append("and servicio.nKey_Deudor = factura.nKey_Deudor and dFechaEmision >= GETDATE() - 180 ");
        cSQL.Append("order by nKey_Cliente ");


        oParam.AddParameters("@codmonitor", pCodMonitor, TypeSQL.Numeric);
        oParam.AddParameters("@codpage", pCodPage, TypeSQL.Numeric);
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
