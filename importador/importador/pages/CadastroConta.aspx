<%@ Page Title="" Language="C#" MasterPageFile="~/MaterMenu.Master" AutoEventWireup="true" CodeBehind="CadastroConta.aspx.cs" Inherits="importador.pages.CadastroConta" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {

            $('.excluir').click(function (event) {
                resp = confirm("Deseja Realmente exluir?");
                return resp;
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <br />
        <div class="visualizacao">
            <fieldset id="fieldCadastro">
                <legend>Cadastro</legend>
                <asp:TextBox ID="txtHistBanco" Placeholder="Histórico Banco" Width="250px" runat="server" />
                <br />
                <asp:TextBox ID="txtContaDebito" Placeholder="Conta Débito" Width="250px" runat="server" />
                <br />
                <asp:TextBox ID="txtContaCredito" Placeholder="Conta Crédito" Width="250px" runat="server" />
                <br />
                <asp:TextBox ID="txtHistoricoContabil" Placeholder="Histórico Contábil" Width="250px" runat="server" />
                <br />
                <asp:TextBox ID="txtCnpj" Placeholder="Cnpj" Width="250px" runat="server" />
                <br />
                <asp:TextBox ID="txtCentroCusto" Placeholder="Centro de Custo" Width="250px" runat="server" />
                <%--<br />--%>
                <asp:Button ID="btnSalvar" Text="Salvar" runat="server" OnClick="btnSalvar_Click" />
                <br />
                <asp:Literal ID="litException" Text="" runat="server" />
            </fieldset>
            <br />

            <asp:GridView ID="grvDePara" CssClass="Grid" runat="server" AutoGenerateColumns="False" Width="900px" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" BackColor="White" BorderStyle="Inset" BorderColor="Black" RowStyle-BorderStyle="Outset" HeaderStyle-BorderStyle="Outset" HeaderStyle-BorderColor="Black" RowStyle-VerticalAlign="Middle" HeaderStyle-BorderWidth="1pt" AlternatingRowStyle-BorderStyle="Outset" OnRowDataBound="grvDePara_RowDataBound" PageSize="9" PagerSettings-Mode="Numeric" HeaderStyle-BackColor="#CCCCCC" AllowPaging="True" ShowHeader="true" OnPageIndexChanging="grvDePara_PageIndexChanging1">
                <AlternatingRowStyle BorderStyle="Outset"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField HeaderText="Histórico Banco" DataField="_Historico_Banco" />
                    <asp:BoundField HeaderText="Conta Débito" DataField="_Conta_Debito" ItemStyle-HorizontalAlign="Left">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Conta Crédito" DataField="_Conta_Credito" ItemStyle-HorizontalAlign="Left">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Historico Contábil" DataField="_Historico_Contabil" ItemStyle-HorizontalAlign="Left" HeaderStyle-BorderStyle="OutSet">

                        <HeaderStyle BorderStyle="Outset"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Cnpj" DataField="_Cnpj" />
                    <asp:BoundField HeaderText="Centro de Custo" DataField="_Centro_Custo" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<asp:Button ID="btnAlterar" Text="Alterar" runat="server" OnClick="btnAlterar_Click" CommandArgument='<%# Eval("_Historico_Banco") %>' />--%>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnExclui" CssClass="excluir" Text="Excluir" runat="server" OnClick="btnExclui_Click" CommandArgument='<%# Eval("_Historico_Banco") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <HeaderStyle BorderColor="Black" BorderWidth="1pt" BorderStyle="Outset"></HeaderStyle>

                <RowStyle VerticalAlign="Middle" BorderStyle="Outset"></RowStyle>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
