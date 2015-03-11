using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace importador.classes.EN
{
    public class ArquivoEN
    {
        public int ID_Arquivo { get; set; }

        public string Nome_Arquivo { get; set; }

        public string Caminho { get; set; }

        public char Tipo { get; set; }

        public int IDEmpresa { get; set; }

        public DateTime Data { get; set; }
    }
}