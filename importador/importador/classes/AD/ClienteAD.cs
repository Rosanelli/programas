using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class ClienteAD
    {
        public static List<ClienteEN> buscaClientes()
        {
            List<ClienteEN> lista = new List<ClienteEN>();

            using (var conexao = new Conexao())
            {
                var query = "SELECT * FROM CLIENTE";
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    ClienteEN en = new ClienteEN();
                    en._ID_Cliente = Convert.ToInt32(reader["ID_Cliente"]);
                    en._Cnpj = reader["Cnpj"].ToString();
                    en._Razao_Social = reader["Razao_Social"].ToString();
                    en._Endereco = reader["Endereco"].ToString();
                    en._Cidade = reader["Cidade"].ToString();
                    en._Bairro = reader["Bairro"].ToString();
                    en._Numero = Convert.ToInt32(reader["Numero"]);
                    en._Foto = reader["Foto"].ToString();

                    lista.Add(en);
                }
            }

            return lista;
        }
    }
}