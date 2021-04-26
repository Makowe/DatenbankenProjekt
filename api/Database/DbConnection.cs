using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Database {

    public class DbConnection {

        /* Function sets up DB Connection and runs the query command. Function returns reader*/
        public static async Task<MySqlDataReader> ExecuteQuery(string query) {
            var connection = new MySqlConnection("server = 127.0.0.1; user = root; password = nico; database = project_9275184");
            await connection.OpenAsync();
            var command = new MySqlCommand(query, connection);
            var reader = await command.ExecuteReaderAsync();
            return reader;
        }
    }
}
