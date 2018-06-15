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
    public class cIndumbral
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pKeyCliente;
        public string KeyCliente { get { return pKeyCliente; } set { pKeyCliente = value; } }

        private string pIndicador;
        public string Indicador { get { return pIndicador; } set { pIndicador = value; } }

        private string pTipo;
        public string Tipo { get { return pTipo; } set { pTipo = value; } }

        private string pKeyTipo;
        public string KeyTipo { get { return pKeyTipo; } set { pKeyTipo = value; } }

        private string pValorBajo;
        public string ValorBajo { get { return pValorBajo; } set { pValorBajo = value; } }

        private string pValorAlto;
        public string ValorAlto { get { return pValorAlto; } set { pValorAlto = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private string pQuery = string.Empty;
        public string Query { get { return pQuery; } set { pQuery = value; } }

        private DBConn oConn;

        public cIndumbral()
        {

        }

        public cIndumbral(ref DBConn oConn)
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
                cSQL.Append("select top 1 indicador, valor_bajo, valor_alto, sla, criterio_aceptacion, unidad, colorbajo, colormedio, coloralto ");
                cSQL.Append("from indumbral ");

                if (!string.IsNullOrEmpty(KeyCliente))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" nkey_cliente = @nkey_cliente");
                    oParam.AddParameters("@nkey_cliente", KeyCliente, TypeSQL.Varchar);

                }

                if (!string.IsNullOrEmpty(pIndicador))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" indicador = @indicador");
                    oParam.AddParameters("@indicador", pIndicador, TypeSQL.Varchar);
                }

                if (!string.IsNullOrEmpty(pTipo))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" tipo  = @tipo ");
                    oParam.AddParameters("@tipo", pTipo, TypeSQL.Varchar);
                }

                if (!string.IsNullOrEmpty(pKeyTipo))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" nkey_tipo  = @nkey_tipo ");
                    oParam.AddParameters("@nkey_tipo", pKeyTipo, TypeSQL.Varchar);
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
    }
}
