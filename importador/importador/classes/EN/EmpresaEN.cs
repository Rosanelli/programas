using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace importador.classes.EN
{
    public class EmpresaEN
    {
        public int _ID_Empresa { get; set; }
        public string _Foto { get; set; }
        public string _Nome { get; set; }
        public string _Cnpj { get; set; }
        public int _IDCliente { get; set; }
    }
}