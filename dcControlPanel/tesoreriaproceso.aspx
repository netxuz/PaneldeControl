<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tesoreriaproceso.aspx.cs" Inherits="dcControlPanel.tesoreriaproceso" %>

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
    <asp:HiddenField ID="hddNCod" runat="server" />
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
                <label class="titSubControlPanel">Tesoreria y Proceso</label></td>
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
          <table id="tbl" runat="server" border="0" cellpadding="0" cellspacing="0" class="tblPanel">
            <tr id="trCabecera" runat="server" style="height: 80px">
              <td class="clPanel"></td>
            </tr>
            <tr>
              <td colspan="20" style="background-color: #00619F; height: 5px;"></td>
            </tr>
          </table>
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
              <td class="tdPieTesoreria" style="width: 80%"></td>
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
