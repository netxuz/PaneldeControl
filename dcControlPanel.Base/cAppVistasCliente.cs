using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcControlPanel.Conn;

namespace dcControlPanel.Base
{
    public class cAppVistasCliente
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pCodMonitor;
        public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

        private string pCodCliente;
        public string CodCliente { get { return pCodCliente; } set { pCodCliente = value; } }

        private bool pNotIn;
        public bool NotIn { get { return pNotIn; } set { pNotIn = value; } }

        private string pAccion;
        public string Accion { get { return pAccion; } set { pAccion = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private string pQuery = string.Empty;
        public string Query { get { return pQuery; } set { pQuery = value; } }

        private DBConn oConn;

        public cAppVistasCliente()
        {

        }

        public cAppVistasCliente(ref DBConn oConn)
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
                cSQL.Append("select cod_monitor, desc_monitor_view  ");
                cSQL.Append("from app_monitor_view where ");
                if (pNotIn)
                    cSQL.Append(" not ");
                cSQL.Append(" cod_monitor in(select cod_monitor from app_vistas_cliente where cod_cliente = @cod_cliente ) ");
                oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Numeric);

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
                            cSQL = new StringBuilder();
                            cSQL.Append("insert into app_vistas_cliente(cod_monitor, cod_cliente) values(");
                            cSQL.Append("@cod_monitor, @cod_cliente) ");
                            oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
                            oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Varchar);
                            oConn.Insert(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        
                        case "ELIMINAR":
                            string Condicion = " where ";

                            cSQL = new StringBuilder();
                            cSQL.Append("delete from app_vistas_cliente ");

                            if (!string.IsNullOrEmpty(pCodCliente))
                            {
                                cSQL.Append(Condicion);
                                Condicion = " and ";
                                cSQL.Append(" cod_cliente = @cod_cliente");
                                oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Numeric);

                            }

                            if (!string.IsNullOrEmpty(pCodMonitor))
                            {
                                cSQL.Append(Condicion);
                                Condicion = " and ";
                                cSQL.Append(" cod_monitor = @cod_monitor");
                                oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);

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
