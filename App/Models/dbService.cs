using Npgsql;
using System;
using System.Security.Cryptography;
using System.Text;

namespace App.Services
{
    public class DbService
    {
        private readonly string _connectionString;

        public DbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateShortUrl(string url)
        {
            // Gerar o hash da URL encurtada
            string shortUrl = GenerateRandomHash(6);

            // Salvar a URL no banco de dados
            GeneratedShortUrl(url, shortUrl, DateTime.UtcNow);
        }

        private string GenerateRandomHash(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var hash = new char[length];

            for (int i = 0; i < length; i++)
            {
                hash[i] = chars[random.Next(chars.Length)];
            }

            return new string(hash);
        }

        public void GeneratedShortUrl(string url, string shortUrl, DateTime time)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO url_shortener (short_url, long_url, created_at) VALUES (@shortUrl, @fullUrl, @time);", connection))
                {
                    command.Parameters.AddWithValue("@fullUrl", url);
                    command.Parameters.AddWithValue("@shortUrl", shortUrl);
                    command.Parameters.AddWithValue("@time", time);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}