<%@ Page Title="" Language="C#" MasterPageFile="~/MaterMenu.Master" AutoEventWireup="true" CodeBehind="ContasSemCadastro.aspx.cs" EnableEventValidation="false" Inherits="importador.pages.ContasSemCadastro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="visualizacao">
        <asp:Button ID="btnExcluiAll" Text="Excluir Todos os Registros" OnClick="btnExcluiAll_Click" runat="server" />
        <br />
        <asp:GridView ID="grvLancSemConta" CssClass="Grid" runat="server" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false" BackColor="White" BorderStyle="Inset" BorderColor="Black" RowStyle-BorderStyle="Outset" HeaderStyle-BorderStyle="Outset" HeaderStyle-BorderColor="Black" RowStyle-VerticalAlign="Middle" HeaderStyle-BorderWidth="1pt" AlternatingRowStyle-BorderStyle="Outset" OnRowDataBound="grvLancSemConta_RowDataBound" PageSize="15" AllowPaging="true" PagerSettings-Mode="Numeric" OnPageIndexChanging="grvLancSemConta_PageIndexChanging" HeaderStyle-BackColor="#CCCCCC">
            <Columns>
                <asp:BoundField HeaderText="Histórico Banco" DataField="_Historico_Banco" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnAltera" Text="Incluir" runat="server" OnClick="btnAltera_Click" CommandArgument='<%# Eval("_Historico_Banco") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
