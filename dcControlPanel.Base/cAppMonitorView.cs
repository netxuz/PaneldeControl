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
    public class cAppMonitorView
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pCodMonitor;
        public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

        private string pDescMonitorView;
        public string DescMonitorView { get { return pDescMonitorView; } set { pDescMonitorView = value; } }

        private string pTipoUsuario;
        public string TipoUsuario { get { return pTipoUsuario; } set { pTipoUsuario = value; } }

        private string pEstMonitorView;
        public string EstMonitorView { get { return pEstMonitorView; } set { pEstMonitorView = value; } }

        private string pCodCliente;
        public string CodCliente { get { return pCodCliente; } set { pCodCliente = value; } }

        private string pAccion;
        public string Accion { get { return pAccion; } set { pAccion = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private string pQuery = string.Empty;
        public string Query { get { return pQuery; } set { pQuery = value; } }

        private DBConn oConn;

        public cAppMonitorView()
        {

        }

        public cAppMonitorView(ref DBConn oConn)
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
                    cSQL.Append("select cod_monitor, desc_monitor_view, tipo_usuario, est_monitor_view, cod_cliente ");
                    cSQL.Append("from app_monitor_view ");

                    if (!string.IsNullOrEmpty(pCodMonitor))
                    {
                        cSQL.Append(Condicion);
                        Condicion = " and ";
                        cSQL.Append(" cod_monitor  = @cod_monitor ");
                        oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
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
                            pCodMonitor = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                            cSQL = new StringBuilder();
                            cSQL.Append("insert into app_monitor_view(cod_monitor, desc_monitor_view, tipo_usuario, est_monitor_view, cod_cliente) values(");
                            cSQL.Append("@cod_monitor, @desc_monitor_view, @tipo_usuario, @est_monitor_view, @cod_cliente) ");
                            oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
                            oParam.AddParameters("@desc_monitor_view", pDescMonitorView, TypeSQL.Varchar);
                            oParam.AddParameters("@tipo_usuario", pTipoUsuario, TypeSQL.Char);
                            oParam.AddParameters("@est_monitor_view", pEstMonitorView, TypeSQL.Char);
                            oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Numeric);
                            oConn.Insert(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                            {
                                pQuery = oConn.ReturnQuery;
                                pError = oConn.Error;
                            }
                            break;
                        case "EDITAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("update app_monitor_view set ");

                            if (!string.IsNullOrEmpty(pDescMonitorView))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" desc_monitor_view = @desc_monitor_view");
                                oParam.AddParameters("@desc_monitor_view", pDescMonitorView, TypeSQL.Varchar);
                                sComa = ", ";
                            }

                            if (!string.IsNullOrEmpty(pTipoUsuario))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" tipo_usuario = @tipo_usuario");
                                oParam.AddParameters("@tipo_usuario", pTipoUsuario, TypeSQL.Char);
                                sComa = ", ";
                            }

                            if (!string.IsNullOrEmpty(pEstMonitorView))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" est_monitor_view = @est_monitor_view");
                                oParam.AddParameters("@est_monitor_view", pEstMonitorView, TypeSQL.Char);
                                sComa = ", ";
                            }

                            if (!string.IsNullOrEmpty(pCodCliente))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" cod_cliente = @cod_cliente");
                                oParam.AddParameters("@cod_cliente", pCodCliente, TypeSQL.Char);
                                sComa = ", ";
                            }

                            cSQL.Append(" where cod_monitor = @cod_monitor ");
                            oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);
                            oConn.Update(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        case "ELIMINAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("delete from app_monitor_view where cod_monitor = @cod_monitor ");
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
