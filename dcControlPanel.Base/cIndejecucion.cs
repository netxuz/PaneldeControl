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
    public class cIndejecucion
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private DBConn oConn;

        public cIndejecucion()
        {

        }

        public cIndejecucion(ref DBConn oConn)
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
                cSQL.Append("select top 1 * from indejecucion order by idfecha desc ");

                dtData = oConn.Select(cSQL.ToString());
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
