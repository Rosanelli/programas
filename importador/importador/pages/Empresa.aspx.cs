using importador.classes.AD;
using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace importador.pages
{
    public partial class Empresa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            populaRepArquivos();// popula arquivos originais e formatados

            btnTarget.Enabled = false;// engessa botoes que servem como controlPopUp
            btnContas.Enabled = false;//

            int idEmpresa = (int)Session["Empresa"];
        }

        public int ano;

        public int numero = 1;

        public int tamanhoLancs;

        public static int loop = 0;

        public delegate void MethodInvoker();

        public List<Arquivo_ContasNullEN> listaArquivoContas = new List<Arquivo_ContasNullEN>();

        public List<TerceiroEN> terceirosRepetidos = new List<TerceiroEN>();

        public List<LancamentosEN> lancamentos = new List<LancamentosEN>();

        public List<LancamentosEN> lancamentosSemDePara = new List<LancamentosEN>();

        private void populaRepArquivos()
        {
            int idEmpresa = Convert.ToInt32(Session["Empresa"]); ;

            grvLancs.DataSource = ArquivoAD.buscaArquivosOriginais(idEmpresa);
            grvLancs.DataBind();

            grvLancsFormatado.DataSource = ArquivoAD.buscaArquivosFormatados(idEmpresa);
            grvLancsFormatado.DataBind();
        }

        public void verificaLanc1()
        {
            if (loop == lancamentos.Count())
            {
                terminaFormatação();
            }
            else
            {
                if (lancamentos[loop].contaDebito == "24020" || lancamentos[loop].contaCredito == "24020" || lancamentos[loop].contaDebito == "10062" || lancamentos[loop].contaCredito == "10062")
                {
                    Session["Lancamentos"] = (List<LancamentosEN>)lancamentos;
                    Session["Loop"] = loop;

                    terceirosRepetidos = (List<TerceiroEN>)Session["terceirosRepetidos"];

                    if (terceirosRepetidos != null)
                    {
                        foreach (var t in terceirosRepetidos)
                        {
                            if (lancamentos[loop].dcto == t._Nome_Terceiro)
                            {
                                lancamentos[loop].cnpjDebito = t._Cnpj;
                            }
                        }
                    }

                    if (lancamentos[loop].cnpjDebito == null)
                    {
                        Session["terceirosRepetidos"] = terceirosRepetidos;

                        showPopUp(lancamentos[loop].historicoArquivo, lancamentos[loop].valorLancamento, lancamentos[loop].dcto);
                    }
                    else
                    {
                        loop++;

                        verificaLanc1();
                    }
                }
                else
                {
                    loop++;
                    verificaLanc2();
                }
            }
        }

        private void verificaLanc2()
        {
            if (loop == lancamentos.Count())
            {
                terminaFormatação();
            }
            else
            {
                if (lancamentos[loop].contaDebito == "24020" && lancamentos[loop].contaCredito == "24020" || lancamentos[loop].contaDebito == "10062" || lancamentos[loop].contaCredito == "10062")
                {
                    Session["Lancamentos"] = (List<LancamentosEN>)lancamentos;
                    Session["Loop"] = loop;

                    terceirosRepetidos = (List<TerceiroEN>)Session["terceirosRepetidos"];

                    if (terceirosRepetidos != null)
                    {
                        foreach (var t in terceirosRepetidos)
                        {
                            if (lancamentos[loop].dcto == t._Nome_Terceiro)
                            {
                                lancamentos[loop].cnpjDebito = t._Cnpj;
                            }
                        }
                    }

                    if (lancamentos[loop].cnpjDebito == null)
                    {
                        Session["terceirosRepetidos"] = terceirosRepetidos;
                        showPopUp(lancamentos[loop].historicoArquivo, lancamentos[loop].valorLancamento, lancamentos[loop].dcto);
                    }
                    else
                    {
                        loop++;

                        verificaLanc2();
                    }
                }
                else
                {
                    loop++;
                    verificaLanc1();
                }
            }
        }

        private void terminaFormatação()
        {
            VinculoArquivoEN en = (VinculoArquivoEN)Session["vinculo"];

            ArquivoEN arquivo = new ArquivoEN();
            arquivo.Nome_Arquivo = en.nomeExtensao;
            arquivo.Caminho = en.diretorioArquivo;
            arquivo.Tipo = '1';
            arquivo.Data = DateTime.Today;
            arquivo.IDEmpresa = Convert.ToInt32(Session["Empresa"]); ;
            ArquivoAD.SalvaArquivo(arquivo);

            formataLayout(lancamentos, en.nomeExtensao, en.extensaoArquivo);

            litStatus.Text = "Formatado com Sucesso!";

            populaRepArquivos();

            loop = 0;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                loop = 0; // zera loop usado na verificação de lançamentos que necessitam cnpj para a formatação do proximo arquivo

                VinculoArquivoEN vinculo = new VinculoArquivoEN();
                vinculo.nomeExtensao = fupArquivo.FileName.Split('.').First();
                vinculo.extensaoArquivo = fupArquivo.FileName.Split('.').Last();
                vinculo.diretorioArquivo = String.Format("~/Arquivos/{0}.{1}", vinculo.nomeExtensao, vinculo.extensaoArquivo);
                vinculo.caminhoCompleto = Server.MapPath(vinculo.diretorioArquivo);
                Session["vinculo"] = vinculo;

                fupArquivo.SaveAs(vinculo.caminhoCompleto);

                // concatena as empresas a seus devidos métodos
                int idEmpresa = (int)Session["Empresa"];
                if (idEmpresa == 1)
                {
                    verificaLancamento(vinculo.caminhoCompleto);
                }
                else if (idEmpresa == 2)
                {
                    verificaLancamentoBanrisul(vinculo.caminhoCompleto);
                }
                else if (idEmpresa == 3)
                {
                    verificaLancamentoItau(vinculo.caminhoCompleto);
                }
                else if (idEmpresa == 4)
                {
                    verificaLancamentoIbia(vinculo.caminhoCompleto);
                }
                else
                {
                    litStatus.Text = "Empresa Não configurada";
                }

                //_verifica contas que necessitam ter cnpj___________________
                tamanhoLancs = lancamentos.Count();

                bool check = (bool)Session["CheckCnpj"];

                if (check == true)
                {
                    verificaLanc1();
                }
                else
                {
                    terminaFormatação();
                }
                //____________________________________________________________
            }
            catch (Exception ex)
            {
                litStatus.Text = ex.StackTrace;
            }
        }

        private void Executar()
        {
            Session["terceirosRepetidos"] = terceirosRepetidos;
            ModalPopupExtender1.Show();

            while (loop == 0)
            {
                Thread.Sleep(0001);
            }
        }

        private void showPopUp(string historico, string valorLanc, string dcto)
        {
            lblDescricao.Text = "Historico: " + historico;
            lblValor.Text = "Valor: " + valorLanc;
            txtDcto.Text = dcto;

            ModalPopupExtender1.Show();

            txtCnpj.Focus();

            int idEmpresa = (int)Session["Empresa"];
            if (idEmpresa == 1)
            {
                txtDcto.Focus();
            }
        }

        private string transformaData(string mes)
        {
            string mesNumeral = "";

            if (mes == "JAN")
            {
                mesNumeral = "01";
            }
            if (mes == "FEV")
            {
                mesNumeral = "02";
            }
            if (mes == "MAR")
            {
                mesNumeral = "03";
            }
            if (mes == "ABR")
            {
                mesNumeral = "04";
            }
            if (mes == "MAI")
            {
                mesNumeral = "05";
            }
            if (mes == "JUN")
            {
                mesNumeral = "06";
            }
            if (mes == "JUL")
            {
                mesNumeral = "07";
            } if (mes == "AGO")
            {
                mesNumeral = "08";
            } if (mes == "SET")
            {
                mesNumeral = "09";
            }
            if (mes == "OUT")
            {
                mesNumeral = "10";
            }
            if (mes == "NOV")
            {
                mesNumeral = "11";
            }
            if (mes == "DEZ")
            {
                mesNumeral = "12";
            }

            return mesNumeral;
        }

        public void verificaLancamentoIbia(string arquivo)
        {
            StreamReader rd = new StreamReader(String.Format(@"{0}", arquivo), Encoding.Default);

            //_________________________
            int cont = 1;
            string linha = "";
            int tamLinha = 0;
            int inicio = 0;
            bool contaRepetida = false;
            //_________________________

            Extrair_Dados(rd, cont, linha, tamLinha, inicio);

            int idEmpresa = (int)Session["Empresa"];
            List<LancamentosEN> lancamentos2 = new List<LancamentosEN>();
            foreach (var lanc in lancamentos)
            {
                ContaEN conta = ContaAD.buscaRegistro(lanc.historico, idEmpresa);

                if (lanc.historico == "TRANSFERENCIA ENTRE CONTAS")
                {
                    lancamentos2.Add(lanc);
                }
                else if (conta.ID != 0)
                {
                    string contadebito1 = conta.Conta_Debito;
                    string contaDebito = contadebito1.PadLeft(5, '0');
                    lanc.contaDebito = contaDebito;

                    string cnpj = "";
                    string terceiro = lanc.dcto.TrimStart().TrimEnd();

                    TerceiroEN en = TerceiroAD.buscaTerceiro(cnpj, terceiro, idEmpresa);
                    if (en._Cnpj != null)
                    {
                        lanc.cnpjDebito = en._Cnpj.PadLeft(14, ' ');
                    }

                    string contacredito1 = conta.Conta_Credito;
                    string contaCredito = contacredito1.PadLeft(5, '0');
                    lanc.contaCredito = contaCredito;

                    lanc.historico = conta.Historico_Contabil;

                    lancamentos2.Add(lanc);
                }
                else
                {
                    bool result = Depara_TemporarioAD.buscaHistorico(lanc.historico);

                    if (result == false)
                    {
                        Depara_TemporarioEN en = new Depara_TemporarioEN();
                        en._Historico_Banco = lanc.historico;
                        en.IDEmpresa = Convert.ToInt32(Session["Empresa"]);
                        Depara_TemporarioAD.incluiDeParaTemporario(en);
                    }

                    contaRepetida = false;
                    linkaContasNulas(contaRepetida, lanc);

                    terceirosRepetidos = (List<TerceiroEN>)Session["terceirosRepetidos"];
                }

            }
            lancamentos.Clear();
            lancamentos = lancamentos2;

            Session["arquivoContasNull"] = (List<Arquivo_ContasNullEN>)listaArquivoContas;
        }

        private void linkaContasNulas(bool contaRepetida, LancamentosEN lanc)
        {
            foreach (var item in listaArquivoContas)
            {
                if (lanc.historico == item._Historico)
                {
                    contaRepetida = true;
                }
            }

            if (contaRepetida == false)
            {
               
                Arquivo_ContasNullEN arquivo_Contas = new Arquivo_ContasNullEN();
                arquivo_Contas._Historico = lanc.historico;

                listaArquivoContas.Add(arquivo_Contas);
            }
        }

        private void Extrair_Dados(StreamReader rd, int cont, string linha, int tamLinha, int inicio)
        {
            while (!rd.EndOfStream)
            {
                linha = RemoveAccents(rd.ReadLine()).Replace("º", "").ToUpper();

                if (cont == 4)
                {
                    tamLinha = linha.Count();
                    inicio = tamLinha - 3;
                    tamLinha = tamLinha - inicio + 1;
                    ano = Convert.ToInt32(linha.Substring(inicio - 1, tamLinha));
                }

                if (cont > 4)
                {
                    DateTime temp;
                    string testeData;

                    testeData = linha.Substring(10, 5);

                    if (testeData.Trim() == "")
                    {
                        int tam2 = linha.Count();
                        string terceiroParte2 = linha.Substring(15, linha.Count() - 15).TrimStart().TrimEnd();

                        if (linha.Count() > 113)
                        {
                            tamLinha = linha.Count() - 112;
                            string historicoParte2 = linha.Substring(112, tamLinha).TrimStart().TrimEnd();
                            if (historicoParte2 != "")
                            {
                                if (historicoParte2.Count() <= 22)
                                {
                                    int i = lancamentos.Count() - 1;
                                    lancamentos[i].historico = lancamentos[i].historico + " " + historicoParte2;
                                }
                            }
                        }

                        if (terceiroParte2 != "" && linha.Count() <= 82)
                        {
                            int i = lancamentos.Count() - 1;
                            lancamentos[i].dcto = lancamentos[i].dcto + " " + terceiroParte2;
                        }

                    }
                    else if (DateTime.TryParse(testeData + "/" + ano, out temp))
                    {
                        string tipo = "LC1";

                        string ordem = Convert.ToString(numero).PadLeft(5, '0');

                        string filler = "   ";

                        string modoLanc = "1";

                        string dataEscrituracao = testeData.Replace("/", "") + ano;

                        string terceiro = linha.Substring(15, 38).TrimStart().TrimEnd();

                        if (terceiro == "FOLHA DE PAGAMENTO")
                        {
                            terceiro = linha.Substring(81, 21);
                        }

                        string historicoBanco = linha.Substring(112, 25).TrimStart().TrimEnd();
                        string historicoTeste = linha.Substring(53, 26).TrimStart().TrimEnd();

                        string dcto = terceiro;

                        tamLinha = linha.Count();
                        tamLinha = tamLinha - 149;
                        string valorLanc = linha.Substring(149, tamLinha).Trim().Replace("-", "").Replace(".", "").Replace(",", ".").Trim();

                        LancamentosEN lanc = new LancamentosEN();

                        lanc.tipo = tipo;
                        lanc.ordem = ordem;
                        lanc.filler = filler;
                        lanc.ModoLanc = modoLanc;
                        lanc.dataEscrituracao = dataEscrituracao;
                        lanc.contaDebito = "";
                        lanc.valorLancamento = valorLanc;
                        lanc.historico = historicoBanco;
                        lanc.contaCredito = "";
                        lanc.dcto = dcto;
                        lanc.historicoArquivo = historicoBanco;

                        if (historicoTeste == "TRANSFERENCIA ENTRE CONTAS")
                        {
                            lanc.historico = "TRANSFERENCIA ENTRE CONTAS";
                            lanc.contaCredito = "00014";
                            lanc.contaDebito = "00080";
                        }

                        lancamentos.Add(lanc);

                        Session["Lancamentos"] = lancamentos;

                        numero++; // variavel de contagem para a ORDEM
                    }
                }

                cont++;
            }
            rd.Close();
        }

        public void verificaLancamentoItau(string arquivo)
        {
            StreamReader rd = new StreamReader(String.Format(@"{0}", arquivo), Encoding.Default);
            Session["i"] = 0;
            Session["conta"] = false;
            while (!rd.EndOfStream)
            {
                string linha = RemoveAccents(rd.ReadLine()).Replace("º", "").ToUpper();

                string testeData;

                if (linha.Count() > 10)
                {
                    testeData = linha.Substring(0, 10);
                }
                else
                {
                    testeData = "dgfad";
                }

                DateTime temp;
                if (DateTime.TryParse(testeData, out temp))
                {
                    string tipo = "LC1";

                    string ordem = Convert.ToString(numero).PadLeft(5, '0');

                    string filler = "   ";

                    string modoLanc = "1";

                    string dataEscrituracao = testeData.Replace("/", "");

                    string historicoBanco = linha.Substring(11, 24).TrimStart().TrimEnd();
                    string dcto = "";

                    int idEmpresa = (int)Session["Empresa"];
                    ContaEN bradesco = ContaAD.buscaRegistro(historicoBanco, idEmpresa);

                    if (bradesco.ID != 0)
                    {
                        string contadebito1 = bradesco.Conta_Debito;
                        string contaDebito = contadebito1.PadLeft(5, '0');

                        string contacredito1 = bradesco.Conta_Credito;
                        string contaCredito = contacredito1.PadLeft(5, '0');

                        Session["conta"] = false;
                        if (contaDebito == "24020" || contaCredito == "24020")
                        {
                            Session["conta"] = true;
                        }

                        int tamLinha = linha.Count() + 1;
                        int tam = tamLinha - 37;
                        string valorLanc = linha.Substring(36, tam).Trim().Replace("-", "").Replace(".", "").Replace(",", ".");

                        string historico = bradesco.Historico_Contabil;

                        LancamentosEN lanc = new LancamentosEN();

                        lanc.tipo = tipo;
                        lanc.ordem = ordem;
                        lanc.filler = filler;
                        lanc.ModoLanc = modoLanc;
                        lanc.dataEscrituracao = dataEscrituracao;
                        lanc.contaDebito = contaDebito;
                        lanc.valorLancamento = valorLanc;
                        lanc.historico = historico;
                        lanc.contaCredito = contaCredito;
                        lanc.dcto = "";
                        lanc.historicoArquivo = historicoBanco;

                        lancamentos.Add(lanc);

                        Session["Lancamentos"] = lancamentos;

                        numero++;
                        int i = (int)Session["i"] + 1;
                        Session["i"] = i;

                    }
                    else
                    {
                        Depara_TemporarioEN en = new Depara_TemporarioEN();
                        en._Historico_Banco = linha.Substring(11, 24).TrimStart().TrimEnd();
                        en.IDEmpresa = Convert.ToInt32(Session["Empresa"]);
                        Depara_TemporarioAD.incluiDeParaTemporario(en);

                        Session["conta"] = false;
                    }
                }
                else
                {
                    if ((bool)Session["conta"] == true)
                    {
                        if (linha.Count() > 14)
                        {
                            string historicoBanco = linha.Substring(11, 10).TrimStart().TrimEnd();
                            if (historicoBanco != "")
                            {
                                if (DateTime.TryParse((string)Session["Data"], out temp))
                                {
                                    int i = (int)Session["i"] + 1;

                                    if (i > 0)
                                    {
                                        historicoBanco = linha.Substring(0, linha.Count()).TrimStart().TrimEnd();
                                        lancamentos[(int)Session["i"] - 1].dcto = historicoBanco;
                                    }
                                }

                            }
                        }
                    }

                    Session["conta"] = false;
                }

                if (linha.Count() > 10)
                {
                    Session["Data"] = linha.Substring(0, 10);
                }
                else
                {
                    Session["Data"] = "dgfad";
                }
            }
            rd.Close();
        }

        public void verificaLancamentoBanrisul(string arquivo)
        {
            StreamReader rd = new StreamReader(String.Format(@"{0}", arquivo), Encoding.Default);
            Session["i"] = 0; // variavel para loop 
            Session["conta"] = false;
            string linha;
            string tester;
            bool pegaLanc = false;
            string mes = "";
            string ano = "";
            string data = "";
            string dia = "";
            bool contaRepetida = false;

            while (!rd.EndOfStream)
            {
                linha = RemoveAccents(rd.ReadLine()).Replace("º", "").ToUpper();
                tester = linha.Substring(0, 2);

                if (linha.Trim() == "")
                {
                    pegaLanc = false;
                }

                if (tester == "--")
                    pegaLanc = false;

                if (pegaLanc == true)
                {
                    string tipo = "LC1";

                    string ordem = Convert.ToString(numero).PadLeft(5, '0');

                    string filler = "   ";

                    string modoLanc = "1";

                    tester = linha.Substring(0, 2);
                    if (tester.Trim() != "")
                        dia = tester;

                    string dataEscrituracao = dia + mes + ano;

                    string historicoBanco = linha.Substring(4, 50).TrimStart().TrimEnd();

                    string dcto = "";

                    int idEmpresa = (int)Session["Empresa"];

                    string query = "select * from dePara where Historico_Banco = '{0}' and ID_Empresa_Depara = '{1}'";
                    ContaEN bradesco = ContaAD.buscaRegistro(historicoBanco, idEmpresa, query);

                    LancamentosEN lanc = new LancamentosEN();

                    if (bradesco.ID != 0)
                    {

                        string contadebito1 = bradesco.Conta_Debito;
                        string contaDebito = contadebito1.PadLeft(5, '0');

                        string contacredito1 = bradesco.Conta_Credito;
                        string contaCredito = contacredito1.PadLeft(5, '0');

                        Session["conta"] = false;
                        if (contaDebito == "24020" || contaCredito == "24020")
                        {
                            Session["conta"] = true;
                        }

                        int tamLinha = linha.Count() + 1;
                        int tam = tamLinha - 37;
                        string valorLanc = linha.Substring(61, 18).Trim().Replace("-", "").Replace(".", "").Replace(",", ".");

                        string historico = bradesco.Historico_Contabil;

                        lanc.tipo = tipo;
                        lanc.ordem = ordem;
                        lanc.filler = filler;
                        lanc.ModoLanc = modoLanc;
                        lanc.dataEscrituracao = dataEscrituracao;
                        lanc.contaDebito = contaDebito;
                        lanc.valorLancamento = valorLanc;
                        lanc.historico = historico;
                        lanc.contaCredito = contaCredito;
                        lanc.dcto = "";
                        lanc.historicoArquivo = historicoBanco;

                        lancamentos.Add(lanc);

                        Session["Lancamentos"] = lancamentos;

                        numero++;
                        int i = (int)Session["i"] + 1;
                        Session["i"] = i;

                    }
                    else
                    {
                        Depara_TemporarioEN en = new Depara_TemporarioEN();
                        en._Historico_Banco = linha.Substring(4, 50).TrimStart().TrimEnd();
                        en.IDEmpresa = Convert.ToInt32(Session["Empresa"]);
                        Depara_TemporarioAD.incluiDeParaTemporario(en);

                        Session["conta"] = false;

                        contaRepetida = false;
                        linkaContasNulas(contaRepetida, lanc);

                    }
                }

                tester = linha.Substring(0, 2);

                if (tester == "++")
                {
                    pegaLanc = true;
                    data = linha.Substring(15, 8);
                    mes = data.Split('/').First();
                    mes = transformaData(mes);
                    ano = data.Split('/').Last();
                }

            }
            rd.Close();

            Session["arquivoContasNull"] = (List<Arquivo_ContasNullEN>)listaArquivoContas;
        }

        public void verificaLancamento(string arquivo)
        {
            StreamReader rd = new StreamReader(String.Format(@"{0}", arquivo), Encoding.Default);
            Session["i"] = 0; // variavel de incremento para loop
            Session["conta"] = false; // usado para identificar lancamentos que utilizam conta de fornecedor
            while (!rd.EndOfStream)
            {
                string linha = RemoveAccents(rd.ReadLine()).Replace("º", "").ToUpper();
                string testeData;

                if (linha.Count() > 10)
                {
                    testeData = linha.Substring(0, 10);
                }
                else
                {
                    testeData = "xxxx";
                }

                DateTime temp;
                if (DateTime.TryParse(testeData, out temp))
                {
                    string tipo = "LC1";
                    string ordem = Convert.ToString(numero).PadLeft(5, '0');
                    string filler = "   ";
                    string modoLanc = "1";
                    string dataEscrituracao = testeData.Replace("/", "");
                    string historicoBanco = linha.Substring(11, 25).TrimStart().TrimEnd();
                    string dcto = linha.Substring(41, 8).TrimStart().TrimEnd();

                    int idEmpresa = (int)Session["Empresa"];
                    ContaEN bradesco = ContaAD.buscaRegistro(historicoBanco, idEmpresa);

                    if (bradesco.ID != 0)
                    {
                        string contaDebito = bradesco.Conta_Debito.PadLeft(5, '0');
                        string contaCredito = bradesco.Conta_Credito.PadLeft(5, '0');

                        Session["conta"] = (contaDebito == "24020" || contaCredito == "24020") ? true : false;

                        string valorLanc = linha.Substring(53, 33).Trim().Replace("-", "").Replace(".", "").Replace(",", ".");

                        string historico = bradesco.Historico_Contabil;

                        LancamentosEN lanc = new LancamentosEN();

                        lanc.tipo = tipo;
                        lanc.ordem = ordem;
                        lanc.filler = filler;
                        lanc.ModoLanc = modoLanc;
                        lanc.dataEscrituracao = dataEscrituracao;
                        lanc.contaDebito = contaDebito;
                        lanc.valorLancamento = valorLanc;
                        lanc.historico = historico;
                        lanc.contaCredito = contaCredito;
                        lanc.dcto = "";
                        lanc.historicoArquivo = historicoBanco;

                        lancamentos.Add(lanc);

                        Session["Lancamentos"] = lancamentos;

                        numero++; // variavel de contagem para a ORDEM

                        int i = (int)Session["i"] + 1;
                        Session["i"] = i;

                    }
                    else
                    {
                        Depara_TemporarioEN en = new Depara_TemporarioEN();
                        en._Historico_Banco = linha.Substring(11, 25).TrimStart().TrimEnd();
                        en.IDEmpresa = (int)Session["Empresa"];
                        Depara_TemporarioAD.incluiDeParaTemporario(en);

                        Session["conta"] = false;
                    }
                }
                else
                {

                    if ((bool)Session["conta"] == true)
                    {
                        if (linha.Count() > 14)
                        {
                            string historicoBanco = linha.Substring(11, 10).TrimStart().TrimEnd();
                            if (historicoBanco != "")
                            {
                                if (DateTime.TryParse((string)Session["Data"], out temp))
                                {
                                    int i = (int)Session["i"] + 1;

                                    if (i > 0)
                                    {
                                        historicoBanco = linha.Substring(0, linha.Count()).TrimStart().TrimEnd();
                                        lancamentos[(int)Session["i"] - 1].dcto = historicoBanco;
                                    }
                                }

                            }
                        }
                    }

                    Session["conta"] = false;
                }

                if (linha.Count() > 10)
                {
                    Session["Data"] = linha.Substring(0, 10);
                }
                else
                {
                    Session["Data"] = "xxxxx";
                }
            }
            rd.Close();
        }

        public static string RemoveAccents(string text)
        {
            String normalizedText = text.Normalize(NormalizationForm.FormD);
            StringBuilder textWithoutAccents = new StringBuilder();

            for (int i = 0; i < normalizedText.Length; i++)
            {
                Char c = normalizedText[i];

                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    textWithoutAccents.Append(c);
                }
            }

            return textWithoutAccents.ToString();
        }

        public void formataLayout(List<LancamentosEN> lancamentos, string nome, string extensao)
        {
            string diretorio = String.Format("~/Arquivos/{0}.TXT", nome + "Formatado");

            string caminhoArquivoFormatado = Server.MapPath(diretorio);

            FileStream fs = File.Create(caminhoArquivoFormatado);
            fs.Close();

            for (int i = 0; i < lancamentos.Count; i++)
            {
                if (lancamentos[i].cnpjDebito == null)
                    lancamentos[i].cnpjDebito = "              ";
                StreamWriter wr = new StreamWriter(caminhoArquivoFormatado, true);
                wr.WriteLine(String.Format("{0}{1}{2}{3}{4}                                                {5}{9}     {8}                   {6}{7}", lancamentos[i].tipo, lancamentos[i].ordem, lancamentos[i].filler, lancamentos[i].ModoLanc, lancamentos[i].dataEscrituracao, lancamentos[i].contaDebito, lancamentos[i].valorLancamento.PadLeft(16, '0'), lancamentos[i].historico, lancamentos[i].contaCredito, lancamentos[i].cnpjDebito.PadLeft(14, ' ')));
                wr.Close();
            }

            ArquivoEN arquivo = new ArquivoEN();
            arquivo.Nome_Arquivo = nome + "Formatado";
            arquivo.Caminho = diretorio;
            arquivo.Tipo = '2';
            arquivo.Data = DateTime.Today;
            arquivo.IDEmpresa = Convert.ToInt32(Session["Empresa"]); ;
            ArquivoAD.SalvaArquivo(arquivo);

            int id = ArquivoAD.buscaUltimoID();

            listaArquivoContas = (List<Arquivo_ContasNullEN>)Session["arquivoContasNull"];

            foreach (var i in listaArquivoContas)
            {
                i._ID_Arquivo = id;

                Arquivo_ContasNullAD.Salva_ArquivoContasNull(i);
            }

        }

        protected void btnExclui_Click(object sender, EventArgs e)
        {
            try
            {

                excluiArquivo(sender);
            }
            catch (Exception ex)
            {
                litStatus.Text = ex.Message;
            }
        }

        private void excluiArquivo(object sender)
        {
            var button = sender as Button;
            int idArquivo = Convert.ToInt32(button.CommandArgument);

            string caminho = Server.MapPath(ArquivoAD.buscaArquivo(idArquivo).Caminho);
            System.IO.File.Delete(caminho);

            ArquivoAD.ExcluiArquivo(idArquivo);

            populaRepArquivos();

            litStatus.Text = "Excluido com Sucesso!";
        }

        protected void btnExcluiFormatados_Click(object sender, EventArgs e)
        {
            try
            {

                excluiArquivo(sender);
            }
            catch (Exception ex)
            {
                litStatus.Text = ex.Message;
            }
        }

        protected void grvLancsFormatado_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grvLancs_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        public static bool validaCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool validaCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            string cnpj = txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "");

            if (cnpj.Length == 11)
            {
                if (validaCpf(cnpj) == true)
                {
                    ProsseguiCnpj();
                }
                else
                {
                    lblException.Text = "CPF Invalido";
                    Session["terceirosRepetidos"] = terceirosRepetidos;
                    ModalPopupExtender1.Show();
                }
            }
            else if (cnpj.Length == 14)
            {
                if (validaCnpj(cnpj) == true)
                {
                    ProsseguiCnpj();
                }
                else
                {
                    lblException.Text = "Cnpj Invalido";
                    Session["terceirosRepetidos"] = terceirosRepetidos;
                    ModalPopupExtender1.Show();
                }
            }
            else
            {
                lblException.Text = "Cnpj Invalido";
                Session["terceirosRepetidos"] = terceirosRepetidos;
                ModalPopupExtender1.Show();
            }



        }

        private void ProsseguiCnpj()
        {
            lancamentos = (List<LancamentosEN>)Session["Lancamentos"];
            loop = (int)Session["Loop"];
            int tamanho = lancamentos.Count();
            lancamentos[loop].cnpjDebito = txtCnpj.Text;
            loop++;

            int idEmpresa = (int)Session["Empresa"];

            //if (idEmpresa == 4)
            //{
            string cnpj = txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "");
            TerceiroAD.inseriTerceiro(cnpj, txtDcto.Text, idEmpresa);
            //}

            TerceiroEN en = new TerceiroEN();
            en._Nome_Terceiro = txtDcto.Text;
            en._Cnpj = txtCnpj.Text;

            if ((List<TerceiroEN>)Session["terceirosRepetidos"] != null)
            {
                terceirosRepetidos = (List<TerceiroEN>)Session["terceirosRepetidos"];
            }

            terceirosRepetidos.Add(en);

            Session["terceirosRepetidos"] = terceirosRepetidos;

            verificaLanc1();
            txtCnpj.Text = "";
            //txtDcto.Text = "";
            lblException.Text = "";
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Session["CheckCnpj"] = CheckBox1.Checked;
        }

        protected void grvLancs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvLancs.PageIndex = e.NewPageIndex;
            populaRepArquivos();
        }

        protected void grvLancsFormatado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvLancsFormatado.PageIndex = e.NewPageIndex;
            populaRepArquivos();
        }

        protected void btnContas_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();

            var button = sender as Button;
            int idArquivo = Convert.ToInt32(button.CommandArgument);

            string[] myList = new string[4];

            ListBox1.Items.Clear();

            List<Arquivo_ContasNullEN> lista = Arquivo_ContasNullAD.busca_ArquivoContasNull(idArquivo);

            if (lista.Count() == 0)
            {
                Label2.Text = "Todas as contas foram importadas!";
            }
            else
            {
                Label2.Text = "Contas não Importadas";

                foreach (var item in lista)
                {
                    ListBox1.Items.Add(item._Historico);
                }
            }



        }

    }
}