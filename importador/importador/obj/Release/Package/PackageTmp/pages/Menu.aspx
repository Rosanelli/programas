<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" EnableEventValidation="false" Inherits="importador.pages.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div id="visualizacao">
        <fieldset>
            <legend>Extratos</legend>
            <asp:GridView ID="grvEmpresa" runat="server"  CssClass="Grid" PagerStyle-CssClass="pgr" AutoGenerateColumns="False" BackColor="White" BorderStyle="Dashed" BorderColor="Black" RowStyle-BorderStyle="Dashed" HeaderStyle-BorderColor="Black" RowStyle-VerticalAlign="Middle" HeaderStyle-BorderWidth="1pt" AlternatingRowStyle-BorderStyle="Dashed" OnRowDataBound="grvEmpresa_RowDataBound" ShowHeader="False">
                <AlternatingRowStyle BorderStyle="Outset"></AlternatingRowStyle>
                <Columns>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Image Height="50px" ImageUrl='<%# Eval("_Foto", "{0:MMMM d, yyyy}") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="Button1" Text='<%# Eval("_Nome") %>' runat="server" OnClick="Button1_Click" CommandArgument='<%# Eval("_ID_Empresa") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>


                    <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="Button1" Text="Alterar" runat="server" OnClick="Unnamed_Click" CommandArgument='<%# Eval("Historico_Banco") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>

                <HeaderStyle BorderColor="Black" BorderWidth="1pt" BorderStyle="Outset"></HeaderStyle>

                <RowStyle VerticalAlign="Middle" BorderStyle="Outset"></RowStyle>
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>
