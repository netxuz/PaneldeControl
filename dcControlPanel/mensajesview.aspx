<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mensajesview.aspx.cs" Inherits="dcControlPanel.mensajesview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panel de Control DebtControl</title>
    <link href="style/screen.css" type="text/css" rel="stylesheet" />
    <script>
        var tick;
        function stop() {
            clearTimeout(tick);
        }
        function simple_reloj() {
            var ut = new Date();
            var h, m, s;
            var time = "        ";
            h = ut.getHours();
            m = ut.getMinutes();
            s = ut.getSeconds();
            if (s <= 9) s = "0" + s;
            if (m <= 9) m = "0" + m;
            if (h <= 9) h = "0" + h;
            time += h + ":" + m + ":" + s;
            document.getElementById('objClock').innerHTML = time;
            tick = setTimeout("simple_reloj()", 1000);
        }
    </script>
</head>
<body onload="simple_reloj();" onunload="stop();">
    <form id="form1" runat="server">
        <asp:HiddenField ID="oCmbTipoConsulta" runat="server" />
        <asp:HiddenField ID="oCmbCliente" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; align-content: center;">
            <tr>
                <td>
                    <asp:Image ID="imglogo" CssClass="imgLogo" runat="server" />
                </td>
                <td class="tdleftTitle">
                    <label class="titControlPanel">PANEL DE CONTROL</label>
                </td>
                <td style="width: 200px;">&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="background-color: #00619F; height: 5px;"></td>
            </tr>
            <tr>
                <td colspan="3"  style="text-align: center; vertical-align: central;">
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset"
                        BorderWidth="0" Width="90%">
                        <marquee id="idMarque" runat="server"  direction="up" onmouseover="this.stop()" onmouseout="this.start()"
                            scrolldelay="50" style="height: 800px; width: 90%"></marquee>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="background-color: #00619F; height: 5px;"></td>
            </tr>
            <tr>
                <td colspan="3">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
                        <tr>
                            <td style="width: 10%; vertical-align: top;">
                                <div class="mdlPeriodo">Periodo</div>
                                <div class="mdlDate">
                                    <div id="firtDay" runat="server"></div>
                                    <div id="lastDay" runat="server"></div>
                                </div>
                            </td>
                            <td class="tdPieMensaje" style="width: 80%"></td>
                            <td style="width: 10%; vertical-align: top;">
                                <div id="objClock">reloj</div>
                                <div class="objFlash">
                                    <asp:Image ID="logo" ImageUrl="~/images/logo.jpg" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr><td></td>
                            <td colspan="2"><div class="leyend">Restricted &copy; debtcontrol <% Response.Write(DateTime.Today.Year.ToString()); %> - All rights reserved</div></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
