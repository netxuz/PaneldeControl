<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="impecabilidad.aspx.cs" Inherits="dcControlPanel.impecabilidad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Panel de Control DebtControl</title>
  <link href="style/screen.css" type="text/css" rel="stylesheet" />
  <script type="text/javascript" src="fancybox/lib/jquery-1.10.1.min.js"></script>
  <script type="text/javascript" src="fancybox/lib/jquery.mousewheel-3.0.6.pack.js"></script>

  <link rel="stylesheet" type="text/css" href="fancybox/source/jquery.fancybox.css?v=2.1.5" media="screen" />
  <script type="text/javascript" src="fancybox/source/jquery.fancybox.pack.js?v=2.1.5"></script>

  <link rel="stylesheet" href="fancybox/source/helpers/jquery.fancybox-buttons.css?v=1.0.5" type="text/css" media="screen" />
  <script type="text/javascript" src="fancybox/source/helpers/jquery.fancybox-buttons.js?v=1.0.5"></script>
  <script type="text/javascript" src="fancybox/source/helpers/jquery.fancybox-media.js?v=1.0.6"></script>

  <link rel="stylesheet" href="fancybox/source/helpers/jquery.fancybox-thumbs.css?v=1.0.7" type="text/css" media="screen" />
  <script type="text/javascript" src="fancybox/source/helpers/jquery.fancybox-thumbs.js?v=1.0.7"></script>

  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
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
        <td class="tdRightTitle">
          <table border="0" style="width: 90%; align-content: center;">
            <tr>
              <td>
                <label class="titSubControlPanel">Impecabilidad</label></td>
              <td align="right">
                <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn btn-default" OnClick="btnLogout_Click">
                  <span class="glyphicon glyphicon-list"></span> Menú
                </asp:LinkButton>
              </td>
            </tr>
          </table>
        </td>
      </tr>
      <tr>
        <td colspan="3" style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td colspan="3" class="tdMarco" align="center">
          <table border="0" cellpadding="0" cellspacing="0" class="tblPanel">
            <tr style="height: 80px" id="HeaderId" runat="server">
              <td class="clPanel"></td>
              <%--<td>
                                <div align="center" class="tbTit">overdue<br />Crítico</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">DSO</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">DBT</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">Litigios<br />Abiertos</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">No identificados</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">Provisión</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">Dctos en<br />Tránsito</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">Efectividad<br />Recaudación</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">Promesa<br />Distribución</div>
                            </td>
                            <td>
                                <div align="center" class="tbTit">Aplicación<br />Tpo Ss</div>
                            </td>--%>
            </tr>
            <tr>
              <td colspan="11" style="background-color: #00619F; height: 5px;"></td>
            </tr>
            <tr id="dataColumn" runat="server">
              <td class="clPanel">
                <div class="tbTitRow">N° de Avance</div>
              </td>
              <%--<td align="center" class="clDtPanel"><div id="oColumn1" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn2" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn3" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn4" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn5" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn6" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn7" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn8" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn10" class="tbDat" runat="server"></div></td>
                            <td align="center" class="clDtPanel"><div id="oColumn9" class="tbDat" runat="server"></div></td>--%>
            </tr>
            <tr id="dataAceptacion" runat="server">
              <td class="clPanel">
                <div class="tbTitRow">Criterio de Aceptación</div>
              </td>
              <%--<td class="clDtPanel"><div id="oCAceptacion1" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion2" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion3" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion4" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion5" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion6" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion7" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion8" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion10" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oCAceptacion9" runat="server" class="tbDat"></div></td>--%>
            </tr>
            <tr id="dataSLA" runat="server">
              <td class="clPanel">
                <div class="tbTitRow">SLA</div>
              </td>
              <%--<td class="clDtPanel"><div id="oSLA1" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA2" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA3" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA4" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA5" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA6" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA7" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA8" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA10" runat="server" class="tbDat"></div></td>
                            <td class="clDtPanel"><div id="oSLA9" runat="server" class="tbDat"></div></td>--%>
            </tr>
            <tr id="dataMesAnterior" runat="server">
              <td class="clPanel">
                <div class="tbTitRow">Mes Anterior</div>
              </td>
              <%--<td class="clDtPanel"><asp:Image id="oColumnMesAnt1" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt2" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt3" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt4" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt5" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt6" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt7" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt8" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt10" runat="server" /></td>
                            <td class="clDtPanel"><asp:Image id="oColumnMesAnt9" runat="server" /></td>--%>
            </tr>
          </table>
        </td>
      </tr>
      <tr>
        <td colspan="3" style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr class="trFoot">
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
              <td class="tdPieImpecabilidad" style="width: 80%"></td>
              <td style="width: 10%; vertical-align: top;">
                <div id="objClock">reloj</div>
                <div class="objFlash">
                  <asp:Image ID="logo" ImageUrl="~/images/logo.jpg" runat="server" />
                </div>
              </td>
            </tr>
            <tr>
              <td></td>
              <td colspan="2">
                <div class="leyend">Restricted &copy; debtcontrol <% Response.Write(DateTime.Today.Year.ToString()); %> - All rights reserved</div>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <a class="open_fancybox" id="open_fancybox" runat="server" data-fancybox-type="iframe" href="" style="text-decoration: none"></a>
  </form>
</body>
</html>
