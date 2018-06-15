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
    public class cAppMensaje
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pCodMensaje;
        public string CodMensaje { get { return pCodMensaje; } set { pCodMensaje = value; } }

        private string pDescMensaje;
        public string DescMensaje { get { return pDescMensaje; } set { pDescMensaje = value; } }

        private string pTextoMensaje;
        public string TextoMensaje { get { return pTextoMensaje; } set { pTextoMensaje = value; } }

        private string pEstMensaje;
        public string EstMensaje { get { return pEstMensaje; } set { pEstMensaje = value; } }

        private string pFchMensaje;
        public string FchMensaje { get { return pFchMensaje; } set { pFchMensaje = value; } }

        private string pAccion;
        public string Accion { get { return pAccion; } set { pAccion = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private DBConn oConn;

        public cAppMensaje()
        {

        }

        public cAppMensaje(ref DBConn oConn)
        {
            this.oConn = oConn;
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
                            pCodMensaje = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                            cSQL = new StringBuilder();
                            cSQL.Append("insert into app_mensaje(cod_mensaje, desc_mensaje, texto_mensaje, est_mensaje, fch_mensaje) values(");
                            cSQL.Append("@cod_mensaje, @desc_mensaje, @texto_mensaje, @est_mensaje, @fch_mensaje) ");
                            oParam.AddParameters("@cod_mensaje", pCodMensaje, TypeSQL.Numeric);
                            oParam.AddParameters("@desc_mensaje", pDescMensaje, TypeSQL.Varchar);
                            oParam.AddParameters("@texto_mensaje", pTextoMensaje, TypeSQL.Text);
                            oParam.AddParameters("@est_mensaje", pEstMensaje, TypeSQL.Char);
                            oParam.AddParameters("@fch_mensaje", DateTime.Now.ToString(), TypeSQL.DateTime);
                            oConn.Insert(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        case "EDITAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("update app_mensaje set ");

                            if (!string.IsNullOrEmpty(pDescMensaje))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" desc_mensaje = @desc_mensaje");
                                oParam.AddParameters("@desc_mensaje", pDescMensaje, TypeSQL.Varchar);
                                sComa = ", ";
                            }

                            if (!string.IsNullOrEmpty(pTextoMensaje))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" texto_mensaje = @texto_mensaje");
                                oParam.AddParameters("@texto_mensaje", pTextoMensaje, TypeSQL.Text);
                                sComa = ", ";
                            }

                            if (!string.IsNullOrEmpty(pEstMensaje))
                            {
                                cSQL.Append(sComa);
                                cSQL.Append(" est_mensaje = @est_mensaje");
                                oParam.AddParameters("@est_mensaje", pEstMensaje, TypeSQL.Char);
                                sComa = ", ";
                            }
                            cSQL.Append(" where cod_mensaje = @cod_mensaje ");
                            oParam.AddParameters("@cod_mensaje", pCodMensaje, TypeSQL.Numeric);
                            oConn.Update(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        case "ELIMINAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("delete from app_mensaje where cod_mensaje = @cod_mensaje ");
                            oParam.AddParameters("@cod_mensaje", pCodMensaje, TypeSQL.Numeric);
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

        public DataTable Get()
        {
            oParam = new DBConn.SQLParameters(10);
            DataTable dtData;
            StringBuilder cSQL;
            string Condicion = " where ";

            if (oConn.bIsOpen)
            {
                cSQL = new StringBuilder();
                cSQL.Append("select cod_mensaje, desc_mensaje, texto_mensaje, est_mensaje, fch_mensaje ");
                cSQL.Append("from app_mensaje ");

                if (!string.IsNullOrEmpty(pCodMensaje))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" cod_mensaje = @cod_mensaje");
                    oParam.AddParameters("@cod_mensaje", pCodMensaje, TypeSQL.Numeric);

                }

                if (!string.IsNullOrEmpty(pTextoMensaje))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" texto_mensaje = @texto_mensaje");
                    oParam.AddParameters("@texto_mensaje", pTextoMensaje, TypeSQL.Text);

                }

                if (!string.IsNullOrEmpty(pEstMensaje))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" est_mensaje = @est_mensaje");
                    oParam.AddParameters("@est_mensaje", pEstMensaje, TypeSQL.Char);
                }

                cSQL.Append(" order by fch_mensaje desc");

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
