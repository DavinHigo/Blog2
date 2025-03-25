// filepath: c:\Users\davin\4870 Workspace\Assignment2\A2\SuperBlogger.ApiService\Controllers\ArticlesController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperBlogger.ApiService.Services;
using BloggerLibrary;

namespace SuperBlogger.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticlesController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Article>>> GetAllActiveArticles()
        {
            var articles = await _articleService.GetAllActiveArticlesAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticleById(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }
    }
}