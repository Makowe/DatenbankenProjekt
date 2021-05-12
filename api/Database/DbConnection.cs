using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Database {

    /// <summary>
    /// Class <c>DbConnection</c> handles the communication with the Database.
    /// </summary>
    public class DbConnection {

        /// <summary>
        /// Method <c>ExecuteQuery</c> executes the given mySql command
        /// </summary>
        /// <param name="query">Command that should be executed</param>
        /// <returns>MySQLDataReader object to read the response of the DB</returns>
        public static async Task<MySqlDataReader> ExecuteQuery(string query) {
            var connection = new MySqlConnection("server = 127.0.0.1; user = root; password = nico; database = project_9275184");
            await connection.OpenAsync();
            var command = new MySqlCommand(query, connection);
            var reader = await command.ExecuteReaderAsync();
            return reader;
        }
    }
}
