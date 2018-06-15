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
    public class cAptMonitorMensaje
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pCodMensaje;
        public string CodMensaje { get { return pCodMensaje; } set { pCodMensaje = value; } }

        private string pCodMonitor;
        public string CodMonitor { get { return pCodMonitor; } set { pCodMonitor = value; } }

        private bool pNotIn;
        public bool NotIn { get { return pNotIn; } set { pNotIn = value; } }

        private string pAccion;
        public string Accion { get { return pAccion; } set { pAccion = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private DBConn oConn;

        public cAptMonitorMensaje()
        {

        }

        public cAptMonitorMensaje(ref DBConn oConn)
        {
            this.oConn = oConn;
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
                            cSQL.Append("insert into apt_monitor_mensaje(cod_mensaje, cod_monitor) values(");
                            cSQL.Append("@cod_mensaje, @cod_monitor) ");
                            oParam.AddParameters("@cod_mensaje", pCodMensaje, TypeSQL.Numeric);
                            oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Varchar);
                            oConn.Insert(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;

                        case "ELIMINAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("delete from apt_monitor_mensaje ");

                            if (!string.IsNullOrEmpty(pCodMensaje))
                            {
                                cSQL.Append(Condicion);
                                Condicion = " and ";
                                cSQL.Append(" cod_mensaje = @cod_mensaje");
                                oParam.AddParameters("@cod_mensaje", pCodMensaje, TypeSQL.Numeric);

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

        public DataTable GetMonitorByMsj()
        {
            oParam = new DBConn.SQLParameters(10);
            DataTable dtData;
            StringBuilder cSQL;

            if (oConn.bIsOpen)
            {
                cSQL = new StringBuilder();
                cSQL.Append("select cod_monitor, desc_monitor_view, tipo_usuario, est_monitor_view, cod_cliente  ");
                cSQL.Append("from app_monitor_view where ");
                if (pNotIn)
                    cSQL.Append(" not ");
                cSQL.Append(" cod_monitor in(select cod_monitor from apt_monitor_mensaje where cod_mensaje = @cod_mensaje ) ");
                oParam.AddParameters("@cod_mensaje", pCodMensaje, TypeSQL.Numeric);

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

        public DataTable GeMensajesByMonitor()
        {
            oParam = new DBConn.SQLParameters(10);
            DataTable dtData;
            StringBuilder cSQL;

            if (oConn.bIsOpen)
            {
                cSQL = new StringBuilder();
                cSQL.Append("select a.cod_mensaje, a.desc_mensaje, a.texto_mensaje, a.est_mensaje, a.fch_mensaje  ");
                cSQL.Append("from app_mensaje a, apt_monitor_mensaje b ");
                cSQL.Append(" where a.cod_mensaje = b.cod_mensaje ");
                cSQL.Append(" and b.cod_monitor = @cod_monitor ");
                oParam.AddParameters("@cod_monitor", pCodMonitor, TypeSQL.Numeric);

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
