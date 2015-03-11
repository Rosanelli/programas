using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class ContaAD
    {
        public static ContaEN buscaRegistro(string historicoBanco,int idEmpresa)
        {
            var query = String.Format("select * from dePara where Historico_Banco = '{0}' and ID_Empresa_Depara = '{1}'", historicoBanco,idEmpresa);


            using (var conexao = new Conexao())
            {
                ContaEN bradesco = new ContaEN();

                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    bradesco.ID = Convert.ToInt32(reader["ID_DePara"]);
                    bradesco.Historico_Banco = Convert.ToString(reader["Historico_Banco"]);
                    bradesco.Conta_Debito = Convert.ToString(reader["Conta_Debito"]); bradesco.Conta_Debito = Convert.ToString(reader["Conta_Debito"]);
                    bradesco.Conta_Credito = Convert.ToString(reader["Conta_Credito"]);
                    bradesco.Historico_Contabil = Convert.ToString(reader["Historico_Contabil"]);
                }

                if(historicoBanco == "")
                {
                    bradesco.ID = 0;
                }

                return bradesco;
            }
        }

        public static ContaEN buscaRegistro(string historicoBanco, int idEmpresa, string sqlquery)
        {
            var query = String.Format(sqlquery, historicoBanco, idEmpresa);


            using (var conexao = new Conexao())
            {
                ContaEN bradesco = new ContaEN();

                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    bradesco.ID = Convert.ToInt32(reader["ID_DePara"]);
                    bradesco.Historico_Banco = Convert.ToString(reader["Historico_Banco"]);
                    bradesco.Conta_Debito = Convert.ToString(reader["Conta_Debito"]); bradesco.Conta_Debito = Convert.ToString(reader["Conta_Debito"]);
                    bradesco.Conta_Credito = Convert.ToString(reader["Conta_Credito"]);
                    bradesco.Historico_Contabil = Convert.ToString(reader["Historico_Contabil"]);
                }

                if (historicoBanco == "")
                {
                    bradesco = null;
                    bradesco.ID = 0;
                }

                return bradesco;
            }
        }

        public static void excluirAllContasSemCadastro()
        {
            using (var conexao = new Conexao())
            {

                var query = @"DELETE FROM DEPARA_TEMPORARIO";

                var command = conexao.CreateCommand(query);
                
                conexao.ExecuteNonQuery(command);
            }

        }
    }
}