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
    public class cAptMonitorPages
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pCodMonitor;
        public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }
        
        private string pCodPage;
        public string CodPage { get { return pCodPage; } set { pCodPage = value; } }

        private string pOrderPage;
        public string OrderPage { get { return pOrderPage; } set { pOrderPage = value; } }

        private string pTimePage;
        public string TimePage { get { return pTimePage; } set { pTimePage = value; } }

        private string pEstPage;
        public string EstPage { get { return pEstPage; } set { pEstPage = value; } }

        private string pTipoUsuario;
        public string TipoUsuario { get { return pTipoUsuario; } set { pTipoUsuario = value; } }

        private string pCodCliente;
        public string CodCliente { get { return pCodCliente; } set { pCodCliente = value; } }

        private string pCodHolding;
        public string CodHolding { get { return pCodHolding; } set { pCodHolding = value; } }

        private string pAccion;
        public string Accion { get { return pAccion; } set { pAccion = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private string pQuery = string.Empty;
        public string Query { get { return pQuery; } set { pQuery = value; } }

        private DBConn oConn;

        public cAptMonitorPages()
        {

        }

        public cAptMonitorPages(ref DBConn oConn)
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
                cSQL.Append("select a.cod_monitor, a.cod_page, b.nom_page, b.url_page, a.order_page, a.time_page, a.est_page, a.tipo_usuario, a.cod_cliente, a.cod_holding, ");
                cSQL.Append(" isnull((select snombre from cliente where nkey_cliente = a.cod_cliente and bCuentaCorriente <> 0), isnull((select distinct holding from cliente where ncodholding = a.cod_holding and bCuentaCorriente <> 0), null)) as snombre ");
                cSQL.Append("from apt_monitor_pages a, app_pages b ");
                cSQL.Append(" where a.cod_page = b.cod_page ");

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

                if (!string.IsNullOrEmpty(pOrderPage))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" order_page  = @order_page ");
                    oParam.AddParameters("@order_page", pOrderPage, TypeSQL.Numeric);
                }

                if (!string.IsNullOrEmpty(pEstPage))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" est_page  = @est_page ");
                    oParam.AddParameters("@est_page", pEstPage, TypeSQL.Char);
                }

                cSQL.Append(" order by a.order_page asc ");

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
            string Condicion = " where ";

            if (oConn.bIsOpen)
            {
                try
                {
                    switch (pAccion)
                    {
                        case "CREAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("insert into apt_monitor_pages(cod_monitor, cod_page, order_page, time_page, est_page, tipo_usuario, cod_cliente, cod_holding) values(");
                            cSQL.Append("@cod_monitor, @cod_page, @order_page, @time_page, @est_page, @tipo_usuario, @cod_cliente, @cod_holding) ");
                            oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
                            oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
                            oParam.AddParameters("@order_page", pOrderPage, TypeSQL.Int);
                            oParam.AddParameters("@time_page", pTimePage, TypeSQL.Int);
                            oParam.AddParameters("@est_page", pEstPage, TypeSQL.Char);
                            oParam.AddParameters("@tipo_usuario", pTipoUsuario, TypeSQL.Varchar);
                            oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Numeric);
                            oParam.AddParameters("@cod_holding", pCodHolding, TypeSQL.Numeric);
                            oConn.Insert(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        case "EDITAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("update apt_monitor_pages set ");

                            if (!string.IsNullOrEmpty(pOrderPage))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" order_page = @order_page");
                                oParam.AddParameters("@order_page", pOrderPage, TypeSQL.Int);
                                sComa = ", ";
                            }
                            if (!string.IsNullOrEmpty(pTimePage))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" time_page = @time_page");
                                oParam.AddParameters("@time_page", pTimePage, TypeSQL.Int);
                                sComa = ", ";
                            }
                            if (!string.IsNullOrEmpty(pEstPage))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" est_page = @est_page");
                                oParam.AddParameters("@est_page", pEstPage, TypeSQL.Char);
                                sComa = ", ";
                            }
                            cSQL.Append(" where cod_monitor = @cod_monitor ");
                            oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);

                            cSQL.Append(" and cod_page = @cod_page ");
                            oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
                            oConn.Update(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        case "ELIMINAR":

                            cSQL = new StringBuilder();
                            cSQL.Append("delete from apt_monitor_pages ");

                            if (!string.IsNullOrEmpty(pCodMonitor))
                            {
                                cSQL.Append(Condicion);
                                Condicion = " and ";
                                cSQL.Append("  cod_monitor = @cod_monitor ");
                                oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
                            }

                            if (!string.IsNullOrEmpty(pCodPage))
                            {
                                cSQL.Append(Condicion);
                                Condicion = " and ";
                                cSQL.Append("  cod_page = @cod_page ");
                                oParam.AddParameters("@cod_page", pCodPage, TypeSQL.Numeric);
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
