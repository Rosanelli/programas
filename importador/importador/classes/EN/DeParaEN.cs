using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace importador.classes.EN
{
    public class DeParaEN
    {
        public int _ID_DePara { get; set; }
        public string _Historico_Banco { get; set; }
        public string _Conta_Debito { get; set; }
        public string _Conta_Credito { get; set; }
        public string _Historico_Contabil { get; set; }
        public int _ID_Empresa_Depara { get; set; }
        public string _Cnpj { get; set; }
        public string _Centro_Custo { get; set; }
    }
}