using System;
using BloggerLibrary;
using Microsoft.EntityFrameworkCore;

namespace SuperBlogger.ApiService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Article>? Articles { get; set; }
    }
} 
