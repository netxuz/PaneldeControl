<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selecciondatos.aspx.cs" Inherits="dcControlPanel.selecciondatos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panel de Control DebtControl</title>
    <link href="style/screen.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="sTipo" runat="server" />
        <table border="0" border="0" cellpadding="0" cellspacing="0" style="width: 100%; align-content: center;">
            <tr>
                <td style="height: 200px;"></td>
            </tr>
            <tr>
                <td align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 30%;">
                        <tr>
                            <td>
                                <div class="tbTit">Cliente</div>
                                <div>
                                    <asp:DropDownList ID="oCmbCliente" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;"></td>
                        </tr>
                        <tr>
                            <td>
                                <div class="tbTit">Tipo Consulta</div>
                                <div>
                                    <asp:DropDownList ID="oCmbTipoConsulta" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;"></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
