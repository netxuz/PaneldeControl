<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="monitorviewconfig.aspx.cs" Inherits="dcControlPanel.monitorviewconfig" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Panel de Control DebtControl</title>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
  <link href="style/screen.css" type="text/css" rel="stylesheet" />
  <script type="text/javascript" src="js/selCombox.js"></script>
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
      <tr>
        <td style="height: 50px; text-align: center; vertical-align: central;">
          <label class="titMantenedor">Mantenedor de Vista</label>
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
                <div class="tdTit">Nombre Vista</div>
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
          </table>
        </td>
      </tr>
      <tr>
        <td style="height: 10px;"></td>
      </tr>
      <tr>
        <td style="background-color: #00619F; height: 5px;"></td>
      </tr>
      <tr>
        <td style="height: 10px;"></td>
      </tr>
      <tr id="trPagesView" runat="server" visible="false">
        <td>
          <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
            <tr>
              <td style="text-align: left">
                <asp:Button ID="btnGrabar2" runat="server" Text="Grabar" OnClick="btnGrabar2_Click" Visible="false" />
              </td>
            </tr>
            <tr>
              <td>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 80%;">
                  <tr>
                    <td style="width: 150px">
                      <div class="tdTit">Tipo Consulta</div>
                    </td>
                    <td style="text-align: left;" colspan="3">
                      <telerik:RadComboBox ID="oCmbTipoConsulta" runat="server" CssClass="objTxt" AutoPostBack="true" OnSelectedIndexChanged="oCmbTipoConsulta_OnClientSelectedIndexChanged">
                      </telerik:RadComboBox>
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" id="tpconsulta_normal" runat="server" visible="false">
                      <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                          <td style="width: 150px">
                            <div class="tdTit">Holding</div>
                          </td>
                          <td style="text-align: left;" colspan="3">
                            <asp:DropDownList ID="oCmbHolding" CssClass="objTxt" runat="server">
                              <asp:ListItem Value="" Text="Selecciona Holding"></asp:ListItem>
                            </asp:DropDownList>
                          </td>
                        </tr>
                        <tr>
                          <td style="width: 150px">
                            <div class="tdTit">Cliente</div>
                          </td>
                          <td style="text-align: left; width: 450px;">
                            <telerik:RadComboBox ID="oCmbCliente" runat="server" CssClass="objTxt" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                              <Items>
                                <telerik:RadComboBoxItem Text="Todos" />
                              </Items>
                            </telerik:RadComboBox>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" id="tdselectvista" runat="server" visible="false">
                      <table border="0" cellpadding="0" cellspacing="0" style="width: 80%;">
                        <tr>
                          <td style="width: 150px">
                            <div class="tdTit">Vistas</div>
                          </td>
                          <td style="text-align: left; width: 450px;">
                            <telerik:RadComboBox ID="oCmbPages" runat="server" CssClass="objTxt" AutoPostBack="true" OnSelectedIndexChanged="oCmbPages_SelectedIndexChanged1">
                            </telerik:RadComboBox>

                            <asp:Label ID="lblPage" runat="server" Visible="false"></asp:Label>
                            <asp:HiddenField ID="hddCodPage" runat="server" />
                          </td>
                          <td style="width: 150px">
                            <div class="tdTit">Tiempo (segundos)</div>
                          </td>
                          <td style="text-align: left;">
                            <asp:TextBox ID="txt_time" runat="server" Text="30" Width="100"></asp:TextBox>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                  <tr>
                    <td style="text-align: left" colspan="2" id="tpconsulta_segundonivel" runat="server" visible="false">
                      <button type="button" id="btnSeleccionar" runat="server" disabled="disabled" data-toggle="modal" data-target="#myModal">Seleccionar Clientes / Holding</button>
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
            <tr>
              <td style="height: 10px;"></td>
            </tr>
            <tr>
              <td align="center">
                <asp:GridView ID="gridPages" CssClass="gridMnt" runat="server" DataKeyNames="cod_monitor, cod_page" AutoGenerateColumns="false"
                  CellSpacing="10" CellPadding="10" BorderStyle="None">
                  <Columns>
                    <asp:BoundField HeaderText="Pagina" HeaderStyle-CssClass="tbHdTitRow" ItemStyle-CssClass="tbTitRow" DataField="nom_page"></asp:BoundField>
                    <asp:BoundField HeaderText="Cliente" HeaderStyle-CssClass="tbHdTitRow" ItemStyle-CssClass="tbTitRow" DataField="snombre"></asp:BoundField>
                    <asp:BoundField HeaderText="Tiempo (s)" HeaderStyle-CssClass="tbHdTitRowData" ItemStyle-CssClass="tbTTitRowData" DataField="time_page"></asp:BoundField>
                  </Columns>
                </asp:GridView>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <div class="container">
      <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-lg">
          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
              <asp:Button ID="btnClose2" Text="&times;" runat="server" CssClass="close" OnClick="btnClose2_Click" />
              <h4 class="modal-title">Seleccione</h4>
            </div>

            <div class="modal-body">
              <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <td>
                    <asp:Button ID="btnChangeClientHolding" runat="server" OnClick="btnChangeClientHolding_Click" Text="Por Holding" />
                  </td>
                  <asp:HiddenField ID="hddMonitorCreated" runat="server" />
                  <asp:HiddenField ID="bVista" runat="server" />
                  <asp:GridView ID="GridClientes" runat="server" CssClass="tbVistaMonitor" BorderStyle="None" AllowPaging="true"
                    DataKeyNames="nkey_cliente" BorderWidth="0" GridLines="None" OnSelectedIndexChanged="GridClientes_SelectedIndexChanged" OnPageIndexChanging="GridClientes_PageIndexChanging"
                    AutoGenerateSelectButton="true"
                    AutoGenerateColumns="false">
                    <Columns>
                      <asp:BoundField ControlStyle-CssClass="tbTitRow" HeaderText="Cliente" DataField="snombre">
                        <HeaderStyle CssClass="tbTitRow" HorizontalAlign="Center" />
                        <ItemStyle CssClass="tbTitRow" HorizontalAlign="Left" />
                      </asp:BoundField>
                    </Columns>
                  </asp:GridView>
                </ContentTemplate>
              </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
              <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="btn btn-default" OnClick="btnClose_Click" />
            </div>
          </div>
        </div>
      </div>
    </div>
    <asp:HiddenField ID="CodMonitorView" runat="server" />
    <asp:HiddenField ID="hddReload" runat="server" />

  </form>
  <script language="javascript" type="text/javascript">
    function OnClientSelectedIndexChanged(sender, eventArgs) {
      var obj = document.getElementById('<%= oCmbHolding.ClientID %>');
      selCombox(obj, '');
    }

    function goReload() {
      document.forms[0].submit();
    }
  </script>
</body>
</html>
