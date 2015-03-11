using importador.classes;
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
    public partial class CadastroConta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idEmpresa = Convert.ToInt32(Session["Empresa"]);

            EmpresaEN en = EmpresaAD.buscaEmpresa(idEmpresa);

            atualizaGrvDePara();

            if (Session["Historico"] != null)
            {
                txtHistBanco.Text = Session["Historico"].ToString();
                txtContaDebito.Focus();
            }
        }


        private void atualizaGrvDePara()
        {
            int idEmpresa = Convert.ToInt32(Session["Empresa"]);
            List<DeParaEN> lista = DeParaAD.buscaDePara(idEmpresa);
            grvDePara.DataSource = lista;
            grvDePara.DataBind();
        }

        private void limpaCampos()
        {
            txtHistBanco.Text = "";
            txtContaDebito.Text = "";
            txtContaCredito.Text = "";
            txtHistoricoContabil.Text = "";
            txtCnpj.Text = "";
            txtCentroCusto.Text = "";
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                DeParaEN en = new DeParaEN();

                en._Historico_Banco = txtHistBanco.Text.Trim();
                en._Conta_Debito = txtContaDebito.Text.Trim();
                en._Conta_Credito = txtContaCredito.Text.Trim();
                en._Historico_Contabil = txtHistoricoContabil.Text.Trim();
                en._ID_Empresa_Depara = Convert.ToInt32(Session["Empresa"]);
                en._Cnpj = txtCnpj.Text;
                en._Centro_Custo = txtCentroCusto.Text;

                if (en._Historico_Banco == "" || en._Conta_Debito == "" || en._Conta_Credito == "" || en._Historico_Contabil == "")
                {
                    throw new CampoEmBrancoException();
                }

                Session["Historico"] = null;

                int idEmpresa = Convert.ToInt32(Session["Empresa"]);

                DeParaAD.ConfrontaDePara(idEmpresa);

                DeParaAD.salvaDePara(en);

                litException.Text = "Salvo com Sucesso!";

                atualizaGrvDePara();

                limpaCampos();

            }
            catch (CampoEmBrancoException)
            {
                litException.Text = "Preencha todos os Campos!";
            }
            catch (Exception ex)
            {
                litException.Text = ex.Message;
            }
        }

        protected void grvDePara_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes.Add("onMouseOver", "this.style.backgroundColor='#778899'; this.style.cursor='hand';");

                e.Row.Attributes.Add("onMouseOut", "this.style.backgroundColor='#ffffff'");

            }
        }

        protected void btnExclui_Click(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                string historico_Banco = button.CommandArgument.ToString();

                DeParaAD.ExcluiDePara(historico_Banco);

                atualizaGrvDePara();

                litException.Text = "Excluido com Sucesso!";
            }
            catch (Exception ex)
            {
                litException.Text = ex.Message;
            }
        }

        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            string historico_Banco = button.CommandArgument.ToString();
        }

        protected void grvDePara_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grvDePara_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grvDePara_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            grvDePara.PageIndex = e.NewPageIndex;
            atualizaGrvDePara();
        }

       

    }
}