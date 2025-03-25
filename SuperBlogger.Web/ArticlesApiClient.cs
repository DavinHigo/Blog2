using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloggerLibrary; // Import the BloggerLibrary namespace

namespace SuperBlogger.Web;

public class ArticlesApiClient
{
    private readonly HttpClient _httpClient;

    public ArticlesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Article>> GetAllActiveArticlesAsync()
    {
        var response = await _httpClient.GetAsync("api/articles");
        response.EnsureSuccessStatusCode();
        var articles = await response.Content.ReadFromJsonAsync<List<Article>>();
        return articles ?? new List<Article>();
    }

    public async Task<Article> GetArticleByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/articles/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Article>() ?? new Article();
    }
}