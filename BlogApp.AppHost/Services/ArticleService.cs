using BlogLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.AppHost.Services
{
    public class ArticleService
    {
        private readonly ArticleRepository _repository;

        public ArticleService(ArticleRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Article>> GetArticlesAsync()
        {
            return _repository.GetArticlesAsync();
        }
    }
}