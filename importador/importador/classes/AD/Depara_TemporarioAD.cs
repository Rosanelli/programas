using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class Depara_TemporarioAD
    {
        public static List<Depara_TemporarioEN> buscaDeParaTemporario(int idEmpresa)
        {
            List<Depara_TemporarioEN> lista = new List<Depara_TemporarioEN>();
            var query = String.Format(@"SELECT DISTINCT Historico_Banco FROM DEPARA_TEMPORARIO
                          WHERE ID_Empresa_Depara_Temporario = '{0}'", idEmpresa);

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    Depara_TemporarioEN en = new Depara_TemporarioEN();

                    en._Historico_Banco = reader["Historico_Banco"].ToString();

                    lista.Add(en);
                }
            }

            return lista;
        }

        public static bool buscaHistorico(string historico)
        {
            bool result = false;

            List<Depara_TemporarioEN> lista = new List<Depara_TemporarioEN>();
            var query = String.Format(@"SELECT * FROM DEPARA_TEMPORARIO
                          WHERE Historico_Banco = '{0}'", historico);

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        public static void ExcluiDeParaTemporario(string historicoBanco)
        {
            using (var conexao = new Conexao())
            {

                var query = @"DELETE from dePara_Temporario
                               WHERE Historico_Banco = @Historico_Banco";

                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@Historico_Banco", historicoBanco);

                conexao.ExecuteNonQuery(command);
            }
        }

        public static void incluiDeParaTemporario(Depara_TemporarioEN en)
        {
            using (var conexao = new Conexao())
            {
                var query = "INSERT INTO DEPARA_TEMPORARIO VALUES(@Historico_Banco,@ID_Empresa)";
                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@Historico_Banco", en._Historico_Banco);
                command.Parameters.AddWithValue("@ID_Empresa", en.IDEmpresa);

                conexao.ExecuteNonQuery(command);
            }

        }
    }
}