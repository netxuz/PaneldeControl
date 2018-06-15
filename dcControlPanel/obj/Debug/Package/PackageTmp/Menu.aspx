<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="dcControlPanel.Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Panel de Control DebtControl</title>
  <link href="style/screen.css" type="text/css" rel="stylesheet" />
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</head>
<body>
  <form id="form1" runat="server">
    <table border="0" style="width: 100%; align-content: center;">
      <tr>
        <td style="height: 100px; text-align: center; vertical-align: central;">
          <label class="titControlPanel">MENU PANEL DE CONTROL</label>
        </td>
      </tr>
      <tr>
        <td align="center">
          <table border="0" style="width: 90%; align-content: center;">
            <tr>
              <td align="right">
                <a href="welcome.aspx" class="btn btn-default"><span class="glyphicon glyphicon-home"></span> Home</a>
              </td>
            </tr>
          </table>
        </td>
      </tr>
      <tr><td style="height:10px"></td></tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td style="height: 400px; text-align: left; vertical-align: top;">
          <div class="tbTit"><a href="logoclienteview.aspx" style="text-decoration: none;">Mantenedor de Logos de Clientes</a></div>
          <div class="tbTit"><a href="mensajes.aspx" style="text-decoration: none;">Mantenedor de Mensajes</a></div>
          <div class="tbTit"><a href="monitorview.aspx" style="text-decoration: none;">Mantenedor de Vistas</a></div>
          <div class="tbTit"><a href="vistaclienteview.aspx" style="text-decoration: none;">Mantenedor de Vistas de Clientes</a></div>
          <div class="tbTit"><a href="kpi_list.aspx" style="text-decoration: none;">Mantenedor de KPI's</a></div>
          <div class="tbTit"><a href="set_kpi_pages_list.aspx" style="text-decoration: none;">Mantenedor de KPI's a Páginas</a></div>
        </td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
    </table>

  </form>
</body>
</html>
