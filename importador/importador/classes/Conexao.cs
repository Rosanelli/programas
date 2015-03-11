using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes
{
    public class Conexao:IDisposable
    {

        public SqlConnection Connection { get; private set; }

        public Conexao()
        {
            var connectionString = "Server= localhost\\SQLEXPRESS;Database=Importador;user = sa; password = 025640;";

            Connection = new SqlConnection(connectionString);
        }

        public SqlDataReader ExecuteQuery(string sqlQuery)
        {
            var command = new SqlCommand(sqlQuery, Connection);

            Connection.Open();

            return command.ExecuteReader();
        }

        public SqlDataReader ExecuteQuery(SqlCommand command)
        {
            Connection.Open();

            return command.ExecuteReader();
        }

        public void ExecuteNonQuery(string sqlCommand)
        {
            var command = new SqlCommand(sqlCommand, Connection);

            Connection.Open();

            command.ExecuteNonQuery();
        }

        public void ExecuteNonQuery(SqlCommand command)
        {
            Connection.Open();

            command.ExecuteNonQuery();
        }

        public SqlCommand CreateCommand(string sqlCommand)
        {
            return new SqlCommand(sqlCommand, Connection);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}