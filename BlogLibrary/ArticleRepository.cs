using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
// using System.Data.SqlClient; // Removed obsolete namespace

namespace BlogLibrary
{
    public class ArticleRepository
    {
        private readonly string _connectionString;

        public ArticleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            var articles = new List<Article>();

            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new Microsoft.Data.SqlClient.SqlCommand("SELECT * FROM Articles", connection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    articles.Add(new Article
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Body = reader.GetString(2),
                        CreateDate = reader.GetDateTime(3),
                        StartDate = reader.GetDateTime(4),
                        EndDate = reader.GetDateTime(5),
                        ContributorUsername = reader.GetString(6)
                    });
                }
            }

            return articles;
        }
    }
}