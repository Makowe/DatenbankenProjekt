using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api.Database {

    /// <summary>
    /// Class <c>DbConnection</c> handles the communication with the Database.
    /// </summary>
    public class DbConnection {

        private readonly MySqlConnection connection = null;
        private MySqlDataReader reader = null;

        /// <summary>Constructor initializes a DB connection and stores it as a private member</summary>
        /// <remarks>Every DbConnection object should only execute one query to prevent problems.</remarks>
        public DbConnection() {
            this.connection = new MySqlConnection("server = 127.0.0.1; user = root; password = nico; database = project_9275184");
            this.connection.Open();
        }

        /// <summary>Method closes the DB connection and closes the data reader object</summary>
        /// <remarks>This Method must always be called when the object is not needed anymore. 
        /// Otherwise the maximum amount of open connections could be exceeded.</remarks>
        public void CloseConnection() {
            if(this.reader != null && !this.reader.IsClosed) { reader.Close(); }
            if(this.connection != null && this.connection.State == ConnectionState.Open) { this.connection.Close(); }
        }

        /// <summary>Method executes the given mySql command </summary>
        /// <param name="query">Command that should be executed</param>
        /// <returns>MySQLDataReader object to read the response of the DB</returns>       
        public async Task<MySqlDataReader> ExecuteQuery(string query) {
           
            var command = new MySqlCommand(query, connection);
            this.reader = await command.ExecuteReaderAsync();
            return this.reader;
        }
    }
}
