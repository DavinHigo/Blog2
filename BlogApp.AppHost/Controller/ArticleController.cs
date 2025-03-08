using BlogApp.AppHost.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.AppHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _service;

        public ArticleController(ArticleService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<List<Article>> Get()
        {
            return _service.GetArticlesAsync();
        }
    }
}