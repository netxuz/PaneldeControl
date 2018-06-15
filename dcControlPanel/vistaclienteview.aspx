<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vistaclienteview.aspx.cs" Inherits="dcControlPanel.vistaclienteview" %>

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
          <label class="titMantenedor">Mantenedor de vistas de clientes</label>
        </td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td>
          <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" />
        </td>
      </tr>
      <tr>
        <td style="text-align: left">
          <asp:TextBox ID="txtNombreApellido" CssClass="cTxtBuscar" runat="server"></asp:TextBox>
          <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </td>
      </tr>
      <tr>
        <td>
          <asp:GridView ID="gridViewClientes" runat="server" CssClass="tbVistaMonitor" 
            BorderStyle="None" DataKeyNames="cod_user" BorderWidth="0" GridLines="None" AllowPaging="true"
            OnSelectedIndexChanged="gridViewClientes_SelectedIndexChanged" OnPageIndexChanging="gridViewClientes_PageIndexChanging"
            AutoGenerateSelectButton="true"
            AutoGenerateColumns="false">
            <Columns>
              <asp:BoundField ControlStyle-CssClass="tbTitRow" DataField="nom_user">
                <ItemStyle CssClass="tbTitRow" />
              </asp:BoundField>
              <asp:BoundField ControlStyle-CssClass="tbTitRow" DataField="ape_user">
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
