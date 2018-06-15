using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using dcControlPanel.Conn;
using dcControlPanel.Method;
using dcControlPanel.Base;

namespace dcControlPanel
{
    public partial class claves : System.Web.UI.Page
    {
        Web oWeb = new Web();

        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn oConn = new DBConn();
            if (oConn.Open()){
                cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
                DataTable dtSysUsuario = oSysUsuario.GetSysUsuario();

                if (dtSysUsuario != null) {
                    if (dtSysUsuario.Rows.Count > 0) {
                        foreach (DataRow oRow in dtSysUsuario.Rows) { 
                            Response.Write(oRow["nom_user"].ToString() + " " + oRow["ape_user"].ToString() + " - "  + fUnCrypt(oRow["pwd_user"].ToString()));
                            Response.Write("<br>");
                        }
                    }
                }

            }
            oConn.Close();
        }

        public string fUnCrypt(string txtInp)
        {
            string txtOut = string.Empty;
            char strCaracter;
            int lngCodigo = 0;
            int lnPos = 1;
            int Acum = 0;
            char cDato = ' ';

            if (txtInp.Length > 0)
            {
                //Acum = Asc(Mid(txtInp, 1, 1)) / 2;
                cDato = Convert.ToChar(txtInp.Substring(0, 1));
                Acum = ((int)cDato) / 2;
                do
                {
                    //lngCodigo = Asc(Mid(txtInp, lnPos, 1)) - Acum
                    lngCodigo = (int)(Convert.ToChar(txtInp.Substring(lnPos, 1))) - Acum;
                    if (lngCodigo < 1)
                    {
                        lngCodigo = lngCodigo + 255;
                    }
                    strCaracter = (char)lngCodigo;
                    txtOut = txtOut + strCaracter.ToString();
                    //Acum = (Asc(Mid(txtInp, lnPos, 1)) - Acum) + Acum;
                    Acum = (int)(Convert.ToChar(txtInp.Substring(lnPos, 1))) + Acum;
                    lnPos++;
                } while (lnPos <= txtInp.Length);
            }

            return txtOut;
        }
    }
}