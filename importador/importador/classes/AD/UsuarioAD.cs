using importador.classes.EN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace importador.classes.AD
{
    public class UsuarioAD
    {
        public static UsuarioEN autenticaUsuario (string usuario, string senha)
        {
            UsuarioEN user = new UsuarioEN();
          
            var query = String.Format(@"select * from Usuario
                                        where Usuario = '{0}' and Senha = '{1}'",usuario,senha);

            using(var conexao = new Conexao())
            {
                SqlDataReader reader = conexao.ExecuteQuery(query);
                if (reader.Read())
                {
                    user._IDUsuario = Convert.ToInt32(reader["ID_Usuario"]);
                    user._Usuario = reader["Usuario"].ToString();
                    user._Senha = reader["Senha"].ToString();
                    user._IDCliente = Convert.ToInt32(reader["IDCliente"]);
                }
                else
                {
                    user = null;
                }
            }

            return user;
        }
    }
}