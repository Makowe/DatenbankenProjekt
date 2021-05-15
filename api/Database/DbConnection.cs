﻿using MySqlConnector;
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

        private MySqlConnection connection = null;
        private MySqlDataReader reader = null;

        /// <summary>
        /// Constructor initializes a DB connection and stores it as a private member
        /// </summary>
        public DbConnection() {
            connection = new MySqlConnection("server = 127.0.0.1; user = root; password = nico; database = project_9275184");
            connection.Open();
        }

        /// <summary>
        /// Method closes the DB connection and closes the data reader object
        /// </summary>
        public void CloseConnection() {
            if(reader != null && !reader.IsClosed) { reader.Close(); }
            if(connection != null && connection.State == ConnectionState.Open )connection.Close();
        }

        /// <summary>
        /// Method executes the given mySql command
        /// </summary>
        /// <param name="query">Command that should be executed</param>
        /// <returns>MySQLDataReader object to read the response of the DB</returns>       
        public async Task<MySqlDataReader> ExecuteQuery(string query) {
           
            var command = new MySqlCommand(query, connection);
            reader = await command.ExecuteReaderAsync();
            return reader;
        }
    }
}
