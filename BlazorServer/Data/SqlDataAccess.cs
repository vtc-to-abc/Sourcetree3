using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BlazorServer.Data
{
    // accessing sql data using dapper connectionstring
    public class SqlDataAccess: ISqlDataAccess
    {
        private readonly IConfiguration _config;

        // the connection string, usually is the "default" string in appsetting.json
        public string ConnectionStrings { get; set; } = "DefaultConnection";
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            string conn = _config.GetConnectionString(ConnectionStrings);
            using (IDbConnection connection = new SqlConnection(conn))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            string conn = _config.GetConnectionString(ConnectionStrings);
            using (IDbConnection connection = new SqlConnection(conn))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

    }
}