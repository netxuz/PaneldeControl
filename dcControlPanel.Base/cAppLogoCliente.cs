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
    public class cAppLogoCliente
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pNKey_cliente;
        public string NKey_cliente { get { return pNKey_cliente; } set { pNKey_cliente = value; } }

        private string pLogoCliente;
        public string LogoCliente { get { return pLogoCliente; } set { pLogoCliente = value; } }

        private string pTipo;
        public string Tipo { get { return pTipo; } set { pTipo = value; } }

        private string pAccion;
        public string Accion { get { return pAccion; } set { pAccion = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private DBConn oConn;

        public cAppLogoCliente()
        {

        }

        public cAppLogoCliente(ref DBConn oConn)
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
                cSQL = new StringBuilder();
                cSQL.Append("select nkey_cliente, logo_cliente, tipo ");
                cSQL.Append("from app_logos ");

                if (!string.IsNullOrEmpty(pNKey_cliente))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" nkey_cliente = @nkey_cliente");
                    oParam.AddParameters("@nkey_cliente", pNKey_cliente, TypeSQL.Numeric);
                }

                if (!string.IsNullOrEmpty(pTipo))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" tipo = @tipo");
                    oParam.AddParameters("@tipo", pTipo, TypeSQL.Char);
                }

                try
                {
                    dtData = oConn.Select(cSQL.ToString(), oParam);
                    pError = oConn.Error;
                    return dtData;
                }
                catch
                {
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
                            cSQL = new StringBuilder();
                            cSQL.Append("insert into app_logos(nkey_cliente, logo_cliente, tipo) values(");
                            cSQL.Append("@nkey_cliente, @logo_cliente, @tipo) ");
                            oParam.AddParameters("@nkey_cliente", pNKey_cliente, TypeSQL.Numeric);
                            oParam.AddParameters("@logo_cliente", pLogoCliente, TypeSQL.Varchar);
                            oParam.AddParameters("@tipo", pTipo, TypeSQL.Char);
                            oConn.Insert(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        case "EDITAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("update app_logos set ");
                            cSQL.Append(" logo_cliente = @logo_cliente, ");
                            oParam.AddParameters("@logo_cliente", pLogoCliente, TypeSQL.Varchar);

                            cSQL.Append(" tipo = @tipo");
                            oParam.AddParameters("@tipo", pTipo, TypeSQL.Char);

                            cSQL.Append(" where nkey_cliente = @nkey_cliente ");
                            oParam.AddParameters("@nkey_cliente", pNKey_cliente, TypeSQL.Numeric);
                            oConn.Update(cSQL.ToString(), oParam);

                            if (!string.IsNullOrEmpty(oConn.Error))
                                pError = oConn.Error;
                            break;
                        case "ELIMINAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("delete from app_logos where nkey_cliente = @nkey_cliente ");
                            oParam.AddParameters("@nkey_cliente", pNKey_cliente, TypeSQL.Numeric);
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
