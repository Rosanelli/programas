using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace importador.classes.EN
{
    public class LancamentosEN
    {
        public string tipo { get; set; }

        public string ordem { get; set; }

        public string filler { get; set; }

        public string ModoLanc { get; set; }

        public string dataEscrituracao { get; set; }

        public string contaDebito { get; set; }

        public string cnpjDebito { get; set; }

        public string valorLancamento { get; set; }

        public string historico { get; set; }

        public string contaCredito { get; set; }

        public string dcto { get; set; }

        public string historicoArquivo { get; set; }
    }
}