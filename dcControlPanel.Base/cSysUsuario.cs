using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcControlPanel.Conn;

namespace dcControlPanel.Base
{
  public class cSysUsuario
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodUser;
    public string CodUser { get { return pCodUser; } set { pCodUser = value; } }

    private string pTipoUser;
    public string TipoUser { get { return pTipoUser; } set { pTipoUser = value; } }

    private string pEstUser;
    public string EstUser { get { return pEstUser; } set { pEstUser = value; } }

    private string pNomApeUser;
    public string NomApeUser { get { return pNomApeUser; } set { pNomApeUser = value; } }

    private string pLoginUser;
    public string LoginUser { get { return pLoginUser; } set { pLoginUser = value; } }

    private string pPwdUser;
    public string PwdUser { get { return pPwdUser; } set { pPwdUser = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cSysUsuario()
    {

    }

    public cSysUsuario(ref DBConn oConn)
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
        cSQL.Append("select a.cod_monitor, (select nom_user from sys_usuario where cod_user = a.cod_cliente) sNombre ");
        cSQL.Append("from app_vistas_cliente a ");

        if (!string.IsNullOrEmpty(pCodUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.cod_cliente = @cod_cliente");
          oParam.AddParameters("@cod_cliente", pCodUser, TypeSQL.Numeric);

        }

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

    public DataTable GetSysUsuario()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_user, nom_user, ape_user, eml_user, login_user, pwd_user, est_user, tipo_user, nkey_user, cod_tipo ");
        cSQL.Append("from sys_usuario ");

        if (!string.IsNullOrEmpty(pCodUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_user = @cod_user");
          oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

        }
        if (!string.IsNullOrEmpty(pLoginUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" login_user = @login_user");
          oParam.AddParameters("@login_user", pLoginUser, TypeSQL.Varchar);

        }
        if (!string.IsNullOrEmpty(pPwdUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" pwd_user = @pwd_user");
          oParam.AddParameters("@pwd_user", pPwdUser, TypeSQL.Varchar);

        }
        if (!string.IsNullOrEmpty(pTipoUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" est_user = @est_user");
          oParam.AddParameters("@tipo_user", pTipoUser, TypeSQL.Numeric);

        }
        if (!string.IsNullOrEmpty(pEstUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" est_user = @est_user");
          oParam.AddParameters("@est_user", pEstUser, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pNomApeUser)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nom_user + ' ' + ape_user like '%' + @nombreapellido + '%' ");
          oParam.AddParameters("@nombreapellido", pNomApeUser, TypeSQL.Varchar);
        }

        cSQL.Append(" order by nom_user asc");

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
