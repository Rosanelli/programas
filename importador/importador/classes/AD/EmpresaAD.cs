using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class EmpresaAD
    {
        public static List<EmpresaEN> buscaEmpresas(int idCliente)
        {
            List<EmpresaEN> lista = new List<EmpresaEN>();

            using (var conexao = new Conexao())
            {
                var query = string.Format(@"SELECT * FROM EMPRESA
                                            WHERE ID_Cliente = '{0}'", idCliente);

                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    EmpresaEN en = new EmpresaEN();
                    en._ID_Empresa = Convert.ToInt32(reader["ID_Empresa"]);
                    en._IDCliente = idCliente;
                    en._Foto = reader["Foto"].ToString();
                    en._Nome = reader["Nome"].ToString();
                    en._Cnpj = reader["Cnpj"].ToString();

                    lista.Add(en);
                }
            }

            return lista;
        }

        public static EmpresaEN buscaEmpresa(int idEmpresa)
        {
            EmpresaEN en = new EmpresaEN();

            using (var conexao = new Conexao())
            {
                var query = string.Format(@"SELECT * FROM EMPRESA
                                            WHERE ID_Empresa = '{0}'", idEmpresa);

                SqlDataReader reader = conexao.ExecuteQuery(query);
                if (reader.Read())
                {
                    en._ID_Empresa = idEmpresa;
                    en._IDCliente = Convert.ToInt32(reader["ID_Cliente"]);
                    en._Foto = reader["Foto"].ToString();
                    en._Nome = reader["Nome"].ToString();
                    en._Cnpj = reader["Cnpj"].ToString();
                }
            }

            return en;
        }


    }
}