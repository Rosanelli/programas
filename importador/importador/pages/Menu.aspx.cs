using importador.classes.AD;
using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace importador.pages
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ClienteEN> lista = ClienteAD.buscaClientes();

            ClienteEN en = new ClienteEN();

            int IDCliente = Convert.ToInt32(Session["IDCliente"]);

            foreach (var c in lista)
            {
                if (c._ID_Cliente == IDCliente)
                {
                    en._ID_Cliente = c._ID_Cliente;
                    en._Razao_Social = c._Razao_Social;
                    en._Foto = c._Foto;
                }
            }

            List<EmpresaEN> listaEmpresas = EmpresaAD.buscaEmpresas(en._ID_Cliente);

            atualizagrvEmpresa(listaEmpresas);

        }

        private void atualizagrvEmpresa(List<EmpresaEN> lista)
        {
            grvEmpresa.DataSource = lista;
            grvEmpresa.DataBind();
        }

        protected void imgUnimed_Click1(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pages/Bradesco.aspx");
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pages/Unylaser.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            int idEmpresa = Convert.ToInt32(button.CommandArgument);

            Session["Empresa"] = idEmpresa;
            Response.Redirect("Empresa.aspx");
        }

        protected void grvEmpresa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes.Add("onMouseOver", "this.style.backgroundColor='#778899'; this.style.cursor='hand';");

                e.Row.Attributes.Add("onMouseOut", "this.style.backgroundColor='#ffffff'");

            }
        }

    }
}