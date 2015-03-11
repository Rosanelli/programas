using importador.classes.AD;
using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace importador
{
    public partial class MaterMenu : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ClienteEN> lista = ClienteAD.buscaClientes();

            ClienteEN en = new ClienteEN();

            int IDCliente = Convert.ToInt32(Session["IDCliente"]);

            if (IDCliente != 0)
            {
                foreach (var c in lista)
                {
                    if (c._ID_Cliente == IDCliente)
                    {
                        en._ID_Cliente = c._ID_Cliente;
                        en._Razao_Social = c._Razao_Social;
                        en._Foto = c._Foto;
                    }
                }

                logoEmpresa.ImageUrl = en._Foto;

                lblNomeEmpresa.Text = en._Razao_Social;

                btnLogin.Text = String.Format("Olá {0}!", en._Razao_Social);

                btnSair.Visible = true;

                int idEmpresa = Convert.ToInt32(Session["Empresa"]);

                EmpresaEN empresaEN = EmpresaAD.buscaEmpresa(idEmpresa);

                imgEmpresa.ImageUrl = empresaEN._Foto;

                imgEmpresa.Visible = true;

            }
            else
            {
                imgEmpresa.Visible = false;
                btnSair.Visible = false;
                logoEmpresa.Visible = false;
                lblNomeEmpresa.Text = "MR Software";
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session["IDCliente"] = 0;
            Response.Redirect("Menu.aspx");
            
        }
    }
}