using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class Arquivo_ContasNullAD
    {
        public static void Salva_ArquivoContasNull(Arquivo_ContasNullEN en)
        {
            using (var conexao = new Conexao())
            {
                var query = "INSERT INTO ARQUIVO_CONTASNULL VALUES(@ID_Arquivo,@Historico)";

                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@ID_Arquivo", en._ID_Arquivo);
                command.Parameters.AddWithValue("@Historico", en._Historico);

                conexao.ExecuteNonQuery(command);
            }
        }

        public static List<Arquivo_ContasNullEN> busca_ArquivoContasNull(int id_Arquivo)
        {
            List<Arquivo_ContasNullEN> lista = new List<Arquivo_ContasNullEN>();
     
            var query = string.Format(@"SELECT * FROM ARQUIVO_CONTASNULL
                                        WHERE ID_Arquivo = '{0}'", id_Arquivo);


            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    Arquivo_ContasNullEN en = new Arquivo_ContasNullEN();

                    en._Historico = reader["Historico"].ToString();

                    lista.Add(en);
                }
            }

            return lista;
        }
    }
}