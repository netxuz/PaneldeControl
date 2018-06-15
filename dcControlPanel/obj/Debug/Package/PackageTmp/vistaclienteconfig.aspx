<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vistaclienteconfig.aspx.cs" Inherits="dcControlPanel.vistaclienteconfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Panel de Control DebtControl</title>
  <link href="style/screen.css" type="text/css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
      <tr>
        <td style="height: 50px; text-align: center; vertical-align: central;">
          <label class="titMantenedor">Mantenedor de Vistas de Cliente</label>
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="Label1" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td style="text-align: left">
          <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" />
          <asp:Button ID="btnPageCliente" runat="server" Text="Asignar KPI's de cliente a vistas" OnClick="btnPageCliente_Click"  />
        </td>
      </tr>
      <tr>
        <td style="height: 10px;"></td>
      </tr>
      <tr id="trMsjsView" runat="server">
        <td>
          <table border="0" cellpadding="0" cellspacing="0" style="width: 90%;">
            <tr>
              <td>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 90%;">
                  <tr>
                    <td></td>
                  </tr>
                  <tr>
                    <td>
                      <asp:ListBox ID="ListBox1" SelectionMode="Multiple" Rows="10" runat="server"></asp:ListBox></td>
                    <td>
                      <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" /><br />
                      <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" />
                    </td>
                    <td>
                      <asp:ListBox ID="ListBox2" SelectionMode="Multiple" Rows="10" runat="server"></asp:ListBox></td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <asp:HiddenField ID="CodCliente" runat="server" />
  </form>
</body>
</html>
