using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using BloggerLibrary;

namespace BloggerBlazorServer.Data
{
    public class DataSeeder
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var userId = "specific-user-id";
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = userId,
                    UserName = "a@a.a",
                    Email = "a@a.a",
                    FirstName = "HI",
                    LastName = "THERE",
                    IsAuth = true,
                    Role = "Admin"
                };
                await userManager.CreateAsync(user, "P@$$w0rd");

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await userManager.AddToRoleAsync(user, "Admin");
            }

            var contributorUserId = "specific-contributor-user-id";
            var contributorUser = await userManager.FindByIdAsync(contributorUserId);
            if (contributorUser == null)
            {
                contributorUser = new ApplicationUser
                {
                    Id = contributorUserId,
                    UserName = "c@c.c",
                    Email = "c@c.c",
                    FirstName = "CONG",
                    LastName = "KING",
                    IsAuth = true,
                    Role = "Contributor"
                };
                await userManager.CreateAsync(contributorUser, "P@$$w0rd");

                if (!await roleManager.RoleExistsAsync("Contributor"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Contributor"));
                }

                await userManager.AddToRoleAsync(contributorUser, "Contributor");
            }
        }

        public static async Task SeedArticlesAsync(ApplicationDbContext context)
        {
            if (!context.Articles.Any())
            {
                var articles = new[]
                {
                    new Article
                    {
                        Title = "Oceans: Earths Blue Heart",
                        Body = "Covering 71% of our planets surface, oceans are the lifeblood of Earth. This article dives into the critical role oceans play in regulating climate, supporting biodiversity, and sustaining human life. We explore pressing issues like coral bleaching, overfishing, and plastic pollution, while highlighting marine conservation success stories and how new technologies are helping protect these vital ecosystems.",
                        CreateDate = DateTime.Now,
                        StartDate = DateTime.Now.AddDays(2),
                        EndDate = DateTime.Now.AddDays(2).AddMonths(1),
                        ContributorUsername = "Contributor User"
                    },
                    new Article
                    {
                        Title = "The Mysteries of Deep Space",
                        Body = "The vast expanse of space continues to captivate humanity with its endless mysteries. Recent discoveries of exoplanets in habitable zones and images from the James Webb Space Telescope have revolutionized our understanding of the cosmos. This article examines the latest breakthroughs in astrophysics, ongoing space missions, and what they reveal about the origins of our universe and the potential for extraterrestrial life.",
                        CreateDate = DateTime.Now,
                        StartDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Now.AddMonths(1),
                        ContributorUsername = "Admin User"
                    },
                    new Article
                    {
                        Title = "Wildlife",
                        Body = "Wildlife conservation is crucial for maintaining biodiversity and ecosystem balance. From majestic elephants to tiny pollinators, each species plays a vital role in our planet's health. This article explores current conservation efforts, threats to wildlife habitats, and how individuals can contribute to protecting endangered species through sustainable practices and responsible tourism.",
                        CreateDate = DateTime.Now,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(1),
                        ContributorUsername = "Contributor User"
                    }
                };

                context.Articles.AddRange(articles);
                await context.SaveChangesAsync();
            }
        }
    }
}
