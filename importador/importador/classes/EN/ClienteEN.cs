using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace importador.classes.EN
{
    public class ClienteEN
    {
        public int _ID_Cliente { get; set; }
        public string _Cnpj { get; set; }
        public string _Razao_Social { get; set; }
        public string _Endereco { get; set; }
        public string _Cidade { get; set; }
        public string _Bairro { get; set; }
        public int _Numero { get; set; }
        public string _Foto { get; set; }
    }
}