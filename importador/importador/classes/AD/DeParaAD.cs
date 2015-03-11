using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class DeParaAD
    {

        public static void salvaDePara(DeParaEN en)
        {
            using (var conexao = new Conexao())
            {
                var query = "INSERT INTO DEPARA VALUES(@Historico_Banco,@Conta_Debito,@Conta_Credito,@Historico_Contabil,@ID_Empresa_Depara,@Cnpj,@CentroCusto)";

                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@Historico_Banco", en._Historico_Banco);
                command.Parameters.AddWithValue("@Conta_Debito", en._Conta_Debito);
                command.Parameters.AddWithValue("@Conta_Credito", en._Conta_Credito);
                command.Parameters.AddWithValue("@Historico_Contabil", en._Historico_Contabil);
                command.Parameters.AddWithValue("@ID_Empresa_Depara", en._ID_Empresa_Depara);
                command.Parameters.AddWithValue("@Cnpj", en._Cnpj);
                command.Parameters.AddWithValue("@CentroCusto", en._Centro_Custo);

                conexao.ExecuteNonQuery(command);
            }
        }

        public static List<DeParaEN> buscaDePara(int idEmpresa)
        {
            List<DeParaEN> lista = new List<DeParaEN>();
            var query = string.Format(@"SELECT * FROM DEPARA 
                          WHERE ID_Empresa_Depara = '{0}'", idEmpresa);

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    DeParaEN en = new DeParaEN();

                    en._Historico_Banco = reader["Historico_Banco"].ToString();
                    en._Conta_Debito = reader["Conta_Debito"].ToString();
                    en._Conta_Credito = reader["Conta_Credito"].ToString();
                    en._Historico_Contabil = reader["Historico_Contabil"].ToString();
                    en._ID_Empresa_Depara = Convert.ToInt32(reader["ID_Empresa_Depara"]);
                    en._Cnpj = reader["Cnpj"].ToString();
                    en._Centro_Custo = reader["Centro_Custo"].ToString();

                    lista.Add(en);
                }
            }

            return lista;
        }

        public static void ExcluiDePara(string historicoBanco)
        {
            using (var conexao = new Conexao())
            {

                var query = @"DELETE from dePara
                               WHERE Historico_Banco = @Historico_Banco";

                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@Historico_Banco", historicoBanco);

                conexao.ExecuteNonQuery(command);
            }
        }

        public static void ConfrontaDePara(int idEmpresa)
        {
            List<DeParaEN> dePara = DeParaAD.buscaDePara(idEmpresa);
            List<Depara_TemporarioEN> deParaTemporario = Depara_TemporarioAD.buscaDeParaTemporario(idEmpresa);

            foreach (var dt in deParaTemporario)
            {
                foreach (var d in dePara)
                {
                    if (dt._Historico_Banco == d._Historico_Banco)
                        Depara_TemporarioAD.ExcluiDeParaTemporario(dt._Historico_Banco);
                }
            }
        }
    }
}