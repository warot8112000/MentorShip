using Azure;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;
namespace DailyNews
{
    public class ApplicationDbContext : DbContext //lớp tương tác trực tiếp với db
    {
//        public DbSet<User> Users { get; set; }
        public DbSet<RSSSource> RssSources { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RSSCategory> RssCategories { get; set; }
        public DbSet<Article> Articles { get; set; }
//        public DbSet<Tag> Tags { get; set; }
//        public DbSet<ArticleTag> ArticleTags { get; set; }
//        public DbSet<Like> Likes { get; set; }
//        public DbSet<UserTag> UserTags { get; set; }
//        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
