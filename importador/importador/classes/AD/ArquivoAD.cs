using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class ArquivoAD
    {
        public static void SalvaArquivo(ArquivoEN arquivo)
        {
            var query = @"INSERT INTO ARQUIVO
                          (Nome_Arquivo,Caminho,Tipo,IDEmpresa,Data)
                          VALUES (@Nome_Arquivo,@Caminho,@Tipo,@IDEmpresa,@Data)";

            using (var conexao = new Conexao())
            {
                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@Nome_Arquivo", arquivo.Nome_Arquivo);
                command.Parameters.AddWithValue("@Caminho", arquivo.Caminho);
                command.Parameters.AddWithValue("@Tipo", arquivo.Tipo);
                command.Parameters.AddWithValue("@IDEmpresa", arquivo.IDEmpresa);
                command.Parameters.AddWithValue("@Data", arquivo.Data);
                conexao.ExecuteNonQuery(command);
            }
        }

        public static List<ArquivoEN> buscaArquivosOriginais(int IDempresa)
        {
            List<ArquivoEN> arquivos = new List<ArquivoEN>();

            var query = String.Format("SELECT * FROM ARQUIVO WHERE Tipo = 1 and IDEmpresa = '{0}' order by ID_Arquivo desc", IDempresa);

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    ArquivoEN arquivo = new ArquivoEN();
                    arquivo.ID_Arquivo = Convert.ToInt32(reader["ID_Arquivo"]);
                    arquivo.Nome_Arquivo = reader["Nome_Arquivo"].ToString();
                    arquivo.Caminho = reader["Caminho"].ToString();
                    arquivo.Data = Convert.ToDateTime(reader["Data"]);

                    arquivos.Add(arquivo);
                }
            }

            return arquivos;
        }

        public static ArquivoEN buscaArquivo(int IDarquivo)
        {
            ArquivoEN arquivo = new ArquivoEN();

            var query = String.Format("SELECT * FROM ARQUIVO WHERE ID_Arquivo = '{0}'", IDarquivo);

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {

                    arquivo.ID_Arquivo = Convert.ToInt32(reader["ID_Arquivo"]);
                    arquivo.Nome_Arquivo = reader["Nome_Arquivo"].ToString();
                    arquivo.Caminho = reader["Caminho"].ToString();
                    arquivo.Data = Convert.ToDateTime(reader["Data"]);

                }
            }

            return arquivo;
        }

        public static List<ArquivoEN> buscaArquivosFormatados(int IDempresa)
        {
            List<ArquivoEN> arquivos = new List<ArquivoEN>();

            var query = String.Format("SELECT * FROM ARQUIVO WHERE Tipo = 2 and IDEmpresa = '{0}' order by ID_Arquivo desc", IDempresa);

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    ArquivoEN arquivo = new ArquivoEN();
                    arquivo.ID_Arquivo = Convert.ToInt32(reader["ID_Arquivo"]);
                    arquivo.Nome_Arquivo = reader["Nome_Arquivo"].ToString();
                    arquivo.Caminho = reader["Caminho"].ToString();
                    arquivo.Data = Convert.ToDateTime(reader["Data"]).Date;

                    arquivos.Add(arquivo);
                }
            }

            return arquivos;
        }

        public static void ExcluiArquivo(int idArquivo)
        {
            using (var conexao = new Conexao())
            {

                var query = @"DELETE FROM ARQUIVO
                               WHERE ID_Arquivo = @ID_Arquivo";

                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@ID_Arquivo", idArquivo);

                conexao.ExecuteNonQuery(command);
            }
        }

        public static int buscaUltimoID()
        {
            int ID = 0;

            var query = String.Format("select max(ID_Arquivo) as ID_Arquivo from arquivo");

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                if (reader.Read())
                {
                    ID = Convert.ToInt32(reader["ID_Arquivo"]);
                }
            }

            return ID;
        }
    }
}