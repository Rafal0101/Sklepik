using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        private string ConnectionString { get; set; } = "DBConnection";
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString()
        {
            return _config.GetConnectionString(ConnectionString);
        }
        public List<T> LoadData<T, U>(string sql, U parameters)
        {
            string connectionString = _config.GetConnectionString(ConnectionString);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = connection.Query<T>(sql, parameters);

                return data.ToList();
            }
        }

        public void SaveData<T>(string sql, T parameters)
        {
            string connectionString = _config.GetConnectionString(ConnectionString);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, parameters);
            }
        }
    }
}
