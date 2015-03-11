using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class TerceiroAD
    {
        public static void inseriTerceiro(string cnpj, string nomeTerceiro,int id_Empresa)
        {
            var query = @"INSERT INTO TERCEIRO
                          VALUES (@Cnpj,@Nome_Terceiro,@ID_Empresa)";

            using (var conexao = new Conexao())
            {
                var command = conexao.CreateCommand(query);
                command.Parameters.AddWithValue("@Cnpj", cnpj);
                command.Parameters.AddWithValue("@Nome_Terceiro", nomeTerceiro);
                command.Parameters.AddWithValue("@ID_Empresa", id_Empresa);

                conexao.ExecuteNonQuery(command);
            }
        }

        public static TerceiroEN buscaTerceiro(string cnpj, string nomeTerceiro, int ID_Empresa)
        {
            TerceiroEN en = new TerceiroEN();

            var query = "SELECT * FROM TERCEIRO ";
            var query1 = String.Format("WHERE Cnpj like '{0}%'", cnpj);
            var query2 = String.Format("WHERE Nome_Terceiro like '%{0}%'", nomeTerceiro);
            var query3 = String.Format(" and ID_Empresa = '{0}'", ID_Empresa);

            if (cnpj != "")
                query += query1 + query3;
            if (nomeTerceiro != "")
                query += query2 + query3;

            using (var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                while (reader.Read())
                {
                    en._Cnpj = reader["Cnpj"].ToString();
                    en._Nome_Terceiro = reader["Nome_Terceiro"].ToString();
                }
            }

            return en;
        }
    }
}