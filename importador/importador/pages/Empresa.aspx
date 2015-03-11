<%@ Page Title="" Language="C#" MasterPageFile="~/MaterMenu.Master" AutoEventWireup="true" CodeBehind="Empresa.aspx.cs" EnableEventValidation="false" Inherits="importador.pages.Empresa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"
        type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtDcto.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Service.asmx/GetTerceiros") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[0],
                                    val: item.split('-')[1]
                                    
                                }
                            }))

                           
                            
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hfCustomerId.ClientID %>").val(i.item.val);
                },
                minLength: 1
            });
         
        });
    </script>



    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnTarget" runat="server" Height="0px" />
            <asp:Button ID="btnContas" runat="server" Height="0px" />
            <asp:HiddenField ID="hfCustomerId" runat="server" />

            <div class="visualizacao">
                <fieldset id="fieldEmpresa">
                    <legend>Upload</legend>
                    <asp:Label ID="lblCnpj" Text="Cnpj obrigatório - Fornecedor" runat="server" />
                    <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" OnCheckedChanged="CheckBox1_CheckedChanged" />
                    <br />
                    <div>
                        <asp:FileUpload ID="fupArquivo" runat="server" />
                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                    </div>
                    <div>
                        <asp:Button ID="btnUpload" runat="server" Text="Formatar Arquivo" OnClick="btnUpload_Click" />
                    </div>
                </fieldset>

                <div>
                    <asp:GridView ID="grvLancs" runat="server" AllowPaging="True" CssClass="Grid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderColor="Black" RowStyle-BorderStyle="Outset" HeaderStyle-BorderStyle="Outset" HeaderStyle-BorderColor="Black" RowStyle-VerticalAlign="Middle" HeaderStyle-BorderWidth="1pt" AlternatingRowStyle-BorderStyle="Outset" OnRowDataBound="grvLancs_RowDataBound" PageSize="3" PagerSettings-Mode="Numeric" OnPageIndexChanging="grvLancs_PageIndexChanging" HeaderStyle-BackColor="#CCCCCC">
                        <AlternatingRowStyle BorderStyle="Outset"></AlternatingRowStyle>
                        <Columns>

                            <asp:TemplateField HeaderText="Arquivos Originais">
                                <ItemTemplate>
                                    <asp:HyperLink Text='<%# Eval("Nome_Arquivo") %>' NavigateUrl='<%# Eval("Caminho") %>' Target="_blank" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Data","{0:d}")  %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnExclui" CssClass="excluir" Text="Excluir" runat="server" OnClick="btnExclui_Click" CommandArgument='<%# Eval("ID_Arquivo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <HeaderStyle BorderColor="Black" BorderWidth="1pt" BorderStyle="Outset"></HeaderStyle>

                        <RowStyle VerticalAlign="Middle" BorderStyle="Outset"></RowStyle>
                    </asp:GridView>
                </div>

                <div>
                    <asp:GridView ID="grvLancsFormatado" CssClass="Grid" PagerStyle-CssClass="pgr" PageSize="3" AllowPaging="True" PagerSettings-Mode="Numeric" AlternatingRowStyle-CssClass="alt" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderColor="Black" RowStyle-BorderStyle="Outset" HeaderStyle-BorderStyle="Outset" HeaderStyle-BorderColor="Black" RowStyle-VerticalAlign="Middle" HeaderStyle-BorderWidth="1pt" AlternatingRowStyle-BorderStyle="Outset" OnRowDataBound="grvLancsFormatado_RowDataBound" HeaderStyle-BackColor="#CCCCCC" OnPageIndexChanging="grvLancsFormatado_PageIndexChanging">
                        <AlternatingRowStyle BorderStyle="Outset"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Arquivos Formatados">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink2" Text='<%# Eval("Nome_Arquivo") %>' NavigateUrl='<%# Eval("Caminho") %>' Target="_blank" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" Text='<%# Eval("Data","{0:d}") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnExcluiFormatados" CssClass="excluir" Text="Excluir" runat="server" OnClick="btnExcluiFormatados_Click" CommandArgument='<%# Eval("ID_Arquivo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnConta" CssClass="excluir" Text="Contas" runat="server" OnClick="btnContas_Click" CommandArgument='<%# Eval("ID_Arquivo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>



                        </Columns>

                        <HeaderStyle BorderColor="Black" BorderWidth="1pt" BorderStyle="Outset"></HeaderStyle>

                        <RowStyle VerticalAlign="Middle" BorderStyle="Outset"></RowStyle>
                    </asp:GridView>
                </div>
            </div>

            <asp:Panel ID="Panel1" CssClass="modalPopup1" runat="server">
                

                <asp:Label ID="lblException" runat="server" />
                <br />
                <asp:Label ID="lblDescricao" runat="server" />
                <br />
                <asp:Label ID="lblValor" runat="server" />
                <br />
                <asp:TextBox ID="txtDcto" Width="350px" runat="server" />
                <br />
                <asp:TextBox ID="txtCnpj" PlaceHolder="Cnpj" runat="server" BackColor="White" />
                <br />
                <asp:Button ID="btnEntrar" Text="Prosseguir" runat="server" OnClick="btnEntrar_Click" />
                <asp:Button ID="btnCancel" CssClass="botaoCancela" Text="Cancelar" runat="server" />
            </asp:Panel>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="modalPopup1_Background" TargetControlID="btnTarget" CancelControlID="btnCancel" PopupControlID="Panel1" runat="server">
            </asp:ModalPopupExtender>

            <asp:Panel ID="Panel2" CssClass="modalPopup1" runat="server">
                <asp:Label ID="Label2" Text="Contas não Importadas" runat="server" />
                <br />
                <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
                <br />
                <asp:Button ID="btnCancel2" CssClass="botaoCancela" Text="Cancelar" runat="server" />
            </asp:Panel>
            <asp:ModalPopupExtender ID="ModalPopupExtender2" BackgroundCssClass="modalPopup1_Background" TargetControlID="btnContas" CancelControlID="btnCancel2" PopupControlID="Panel2" runat="server">
            </asp:ModalPopupExtender>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
