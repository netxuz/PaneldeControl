<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logoclienteview.aspx.cs" Inherits="dcControlPanel.logoclienteview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Panel de Control DebtControl</title>
  <link href="style/screen.css" type="text/css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
      <tr>
        <td style="height: 50px; text-align: center; vertical-align: central;">
          <label class="titMantenedor">Mantenedor de Logos de Clientes</label>
        </td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td style="text-align: left">
          <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" />
          <asp:Button ID="btnHolding" runat="server" Text="Holding" OnClick="btnHolding_Click" />
        </td>
      </tr>
      <tr>
        <td style="text-align: left">
          <asp:TextBox ID="txtlogo" CssClass="cTxtBuscar" runat="server"></asp:TextBox>
          <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </td>
      </tr>
      <tr>
        <td>
          <asp:GridView ID="gridLogos" runat="server" CssClass="tbVistaMonitor" BorderStyle="None" DataKeyNames="nkey_cliente"
            BorderWidth="0" GridLines="None" AllowPaging="true"
            OnSelectedIndexChanged="gridLogos_SelectedIndexChanged" OnPageIndexChanging="gridLogos_PageIndexChanging"
            AutoGenerateSelectButton="true"
            AutoGenerateColumns="false">
            <Columns>
              <asp:BoundField DataField="snombre">
                <ItemStyle CssClass="tbTitRow" />
              </asp:BoundField>
            </Columns>
          </asp:GridView>
        </td>
      </tr>
    </table>
  </form>
</body>
</html>
