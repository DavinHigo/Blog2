using BloggerBlazorServer.Data;
using BloggerLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggerBlazorServer.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            return await _context.Articles
                .Where(a => a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                .OrderByDescending(a => a.CreateDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesByContributorAsync(string contributorUsername)
        {
            return await _context.Articles
                .Where(a => a.ContributorUsername == contributorUsername)
                .OrderByDescending(a => a.CreateDate)
                .ToListAsync();
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
            {
                throw new Exception($"Article with ID {id} not found.");
            }
            return article;
        }

        public async Task CreateArticleAsync(Article article)
        {
            article.CreateDate = DateTime.UtcNow;
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(Article article)
        {
            var existingArticle = await _context.Articles.FirstOrDefaultAsync(a => a.ArticleId == article.ArticleId);
            if (existingArticle != null)
            {
                existingArticle.Title = article.Title;
                existingArticle.Body = article.Body;
                existingArticle.StartDate = article.StartDate;
                existingArticle.EndDate = article.EndDate;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.ArticleId == id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}