using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SkypeBot.BotEngine.Commands
{
    public class ExternalId : ISkypeCommand
    {
        private bool isExternal;
        private string id;
        public string RunCommand()
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                using (var connection = new SqlConnection((ConfigurationManager.ConnectionStrings["strCon"].ConnectionString)))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format("select {0} from externalidentitymap where {1}='{2}'",
                            isExternal ? "InternalId" : "ExternalIdentity",
                            isExternal ? "ExternalIdentity" : "InternalId", 
                            id);
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : null;
                    }
                }
            }
            return null;
        }

        public void Init(string arguments)
        {
            if (Regex.IsMatch(arguments.Trim(), @"^\d+$"))
            {
                id = arguments.Trim();
                isExternal = false;
            }
            else if (Regex.IsMatch(arguments.Trim(), @"^[\d\w]+$"))
            {
                id = arguments.Trim();
                isExternal = true;
            }
        }
    }
}
