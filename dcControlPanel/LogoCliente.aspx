<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogoCliente.aspx.cs" Inherits="dcControlPanel.LogoCliente" %>

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
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" CssClass="titSubControlPanel" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td style="width: 30%;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td>
                                            <telerik:RadUpload ID="RadUpload1" InitialFileInputsCount="1" AllowedFileExtensions=".jpg,.jpeg" runat="server">
                                            </telerik:RadUpload>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnGrabar1" runat="server" Text="Grabar" OnClick="btnGrabar1_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" ClientValidationFunction="validateRadUpload1"><span style="FONT-SIZE: 11px;">Invalid extensions.</span></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateRadUpload1(source, arguments) {
                                                    arguments.IsValid = getRadUpload('<%= RadUpload1.ClientID %>').validateExtensions();
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <div><asp:Image ID="imglogo" runat="server" /></div>
                                <asp:Button ID="btnGrabar2" runat="server" Text="Eliminar Imagen" OnClick="btnGrabar2_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="CodCliente" runat="server" />
        <asp:HiddenField ID="hddAccion" runat="server" />
        <asp:HiddenField ID="hddTipo" runat="server" />
    </form>
</body>
</html>
