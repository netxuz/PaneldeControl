<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="dcControlPanel.WebForm1" %>

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
          <label class="titControlPanel">PANEL DE CONTROL</label>
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
      <tr>
        <td style="height: 10px"></td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td style="height: 400px; vertical-align: top;">
          <asp:GridView ID="gridVistas" CssClass="tbVistaMonitor" BorderStyle="None" AllowPaging="true"
            runat="server" AutoGenerateColumns="false" DataKeyNames="cod_monitor" BorderWidth="0" GridLines="None"
            OnSelectedIndexChanged="gridVistas_SelectedIndexChanged" OnPageIndexChanging="gridVistas_PageIndexChanging" AutoGenerateSelectButton="true">
            <Columns>
              <asp:BoundField ControlStyle-CssClass="tbTitRow" DataField="desc_monitor_view">
                <ItemStyle CssClass="tbTitRow" />
              </asp:BoundField>
            </Columns>
            <EditRowStyle CssClass="EditRowStyle" />
          </asp:GridView>
        </td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
    </table>
  </form>
</body>
</html>
