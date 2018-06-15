<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mensajesconfig.aspx.cs" Inherits="dcControlPanel.mensajesconfig" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panel de Control DebtControl</title>
    <link href="style/screen.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
            <tr>
                <td style="height: 50px; text-align: center; vertical-align: central;">
                    <label class="titMantenedor">Mantenedor de Mensaje</label>
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
                                <div class="tdTit">Nombre Mensaje</div>
                            </td>
                            <td style="text-align: left; width: 450px;">
                                <asp:TextBox ID="txt_nombre" CssClass="objTxt" runat="server"></asp:TextBox>
                            </td>
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
                        <tr>
                            <td colspan="4" style="text-align: left;">
                                <div class="tdTitd">Mensaje</div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: left;">
                                <telerik:RadEditor ID="rdMensaje" runat="server" CssClass="rdMensaje">
                                    <Tools>
                                        <telerik:EditorToolGroup Tag="FileManagers">
                                            <telerik:EditorTool Name="ImageManager" />
                                            <telerik:EditorTool Name="FlashManager" />
                                            <telerik:EditorTool Name="MediaManager" />
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorTool Name="Bold" />
                                            <telerik:EditorTool Name="Italic" />
                                            <telerik:EditorTool Name="Underline" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="ForeColor" />
                                            <telerik:EditorTool Name="BackColor" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="FontName" />
                                            <telerik:EditorTool Name="RealFontSize" />
                                        </telerik:EditorToolGroup>
                                    </Tools>
                                    <ImageManager ViewPaths="~/ContentImages" DeletePaths="~/ContentImages" UploadPaths="~/ContentImages" />
                                    <FlashManager ViewPaths="~/ContentFlash" DeletePaths="~/ContentFlash" UploadPaths="~/ContentFlash" />
                                    <MediaManager ViewPaths="~/ContentMedia" DeletePaths="~/ContentMedia" UploadPaths="~/ContentMedia" />
                                    <Languages>
                                        <telerik:SpellCheckerLanguage Code="es-ES" Title="Español" />
                                    </Languages>
                                    <Content></Content>
                                </telerik:RadEditor>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;"></td>
            </tr>
            <tr>
                <td style="background-color: #00619F; height: 5px;"></td>
            </tr>
            <tr id="trMsjsView" runat="server" visible="false">
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
        <asp:HiddenField ID="CodMensaje" runat="server" />
    </form>
</body>
</html>
