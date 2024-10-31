using Npgsql;
using System;

namespace App.Services
{
    public class DbService
    {
        private readonly string _connectionString;

        public DbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateQuery()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS urls (id SERIAL PRIMARY KEY, url TEXT NOT NULL)", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SaveUrl(string url)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO urls (url) VALUES (@url)", connection))
                {
                    command.Parameters.AddWithValue("@url", url);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}