using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace importador.classes
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetTerceiros(string prefix)
        {
            List<string> terceiros = new List<string>();
            using (var conexao = new Conexao())
            {
                var query = string.Format("select top 10 Cnpj,Nome_Terceiro from terceiro where Nome_Terceiro like '{0}%' ",prefix);

                SqlDataReader reader = conexao.ExecuteQuery(query);

                while (reader.Read())
                {
                    terceiros.Add(string.Format("{0} - {1}", reader["Nome_Terceiro"], reader["Cnpj"]));
                }

                

                return terceiros.ToArray();
            }
        }
    }
}
