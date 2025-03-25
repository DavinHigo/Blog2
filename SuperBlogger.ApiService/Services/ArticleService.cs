using BloggerLibrary;
using Microsoft.EntityFrameworkCore;
using SuperBlogger.ApiService.Data;

namespace SuperBlogger.ApiService.Services
{
    public class ArticleService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<List<Article>> GetAllActiveArticlesAsync()
        {
            return await (_context.Articles?
                .Where(a => a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                .ToListAsync() ?? Task.FromResult(new List<Article>()));
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            if (_context.Articles == null)
            {
                return null;
            }
            return await _context.Articles.FindAsync(id);
        }
    }
}