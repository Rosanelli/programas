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
    public partial class ContasSemCadastro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            atualizaGrvDePara();
        }

        protected void btnAltera_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            string historico_Banco = button.CommandArgument.ToString();

            Session["Historico"] = historico_Banco;

            Response.Redirect("CadastroConta.aspx");
        }

        private void atualizaGrvDePara()
        {
            int idEmpresa = Convert.ToInt32(Session["Empresa"]);
            List<Depara_TemporarioEN> lista = Depara_TemporarioAD.buscaDeParaTemporario(idEmpresa);
            grvLancSemConta.DataSource = lista;
            grvLancSemConta.DataBind();
        }

        protected void grvLancSemConta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes.Add("onMouseOver", "this.style.backgroundColor='#778899'; this.style.cursor='hand';");

                e.Row.Attributes.Add("onMouseOut", "this.style.backgroundColor='#ffffff'");

            }
        }

        protected void grvLancSemConta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvLancSemConta.PageIndex = e.NewPageIndex;
            atualizaGrvDePara();
        }

        protected void btnExcluiAll_Click(object sender, EventArgs e)
        {
            ContaAD.excluirAllContasSemCadastro();

            atualizaGrvDePara();
        }
    }
}