<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kpi.aspx.cs" Inherits="dcControlPanel.kpi" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Panel de Control DebtControl</title>
  <link href="style/screen.css" type="text/css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="WebBlue" runat="server"></telerik:RadWindowManager>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
      <tr>
        <td style="height: 50px; text-align: center; vertical-align: central;">
          <label class="titMantenedor">Mantenedor de KPI's</label>
        </td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td style="text-align: left">
          <asp:Button ID="btnGrabar1" runat="server" Text="Grabar" OnClick="btnGrabar1_Click" />
          <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" />
        </td>
      </tr>
      <tr>
        <td style="height: 10px;"></td>
      </tr>
      <tr>
        <td>
          <table border="0" cellpadding="0" cellspacing="0" style="width: 90%">
            <tr>
              <td style="width: 150px">
                <div class="tdTit">Nombre KPI</div>
              </td>
              <td style="text-align: left; width: 450px;">
                <asp:TextBox ID="txt_nombre" CssClass="objTxt" runat="server"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td style="width: 150px">
                <div class="tdTit">Identificador KPI</div>
              </td>
              <td style="text-align: left; width: 450px;">
                <asp:TextBox ID="txt_identificador" CssClass="objTxtIdent" runat="server"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td style="width: 150px">
                <div class="tdTit">Estado</div>
              </td>
              <td style="text-align: left;">
                <asp:DropDownList ID="oCmbEstado" runat="server">
                  <asp:ListItem Value="A" Text="Activo"></asp:ListItem>
                  <asp:ListItem Value="N" Text="No Activo"></asp:ListItem>
                </asp:DropDownList>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <asp:HiddenField ID="CodKpi" runat="server" />
  </form>
</body>
</html>
