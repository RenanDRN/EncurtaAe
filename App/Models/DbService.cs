using MySql.Data.MySqlClient;
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

        public string CreateShortUrl(string url)
        {
            string shortUrl = GenerateRandomHash(6);
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime brasiliaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, brasiliaTimeZone);
            GeneratedShortUrl(url, shortUrl, brasiliaTime);
            return shortUrl;
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("INSERT INTO url_shortener (original_url, short_code, created_at) VALUES (@fullUrl, @shortUrl, @time);", connection))
                {
                    command.Parameters.AddWithValue("@fullUrl", url);
                    command.Parameters.AddWithValue("@shortUrl", shortUrl);
                    command.Parameters.AddWithValue("@time", time);
                    command.ExecuteNonQuery();
                }
            }
        }

        public string GetOriginalUrl(string shortUrl)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT original_url FROM url_shortener WHERE short_code = @shortUrl;", connection))
                {
                    command.Parameters.AddWithValue("@shortUrl", shortUrl);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString("original_url");
                        }
                    }
                }
            }
            return null;
        }

         public int GetShortUrlCount()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT COUNT(*) FROM url_shortener;", connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public List<string> GetLastFiveShortUrls()
        {
            var shortUrls = new List<string>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT short_code FROM url_shortener ORDER BY created_at DESC LIMIT 5;", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shortUrls.Add(reader.GetString("short_code"));
                        }
                    }
                }
            }
            return shortUrls;
        }
    }
}