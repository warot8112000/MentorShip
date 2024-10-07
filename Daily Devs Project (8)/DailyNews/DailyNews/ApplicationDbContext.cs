using Azure;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;
namespace DailyNews
{
    public class ApplicationDbContext : DbContext //lớp tương tác trực tiếp với db
    {
//        public DbSet<User> Users { get; set; }
        public DbSet<RSS_Sources> RssSources { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RSS_Category> RssCategories { get; set; }
        public DbSet<Articles> Articles { get; set; }
//        public DbSet<Tag> Tags { get; set; }
//        public DbSet<ArticleTag> ArticleTags { get; set; }
//        public DbSet<Like> Likes { get; set; }
//        public DbSet<UserTag> UserTags { get; set; }
//        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình các ràng buộc UNIQUE, ví dụ Url phải là duy nhất trong Articles
            modelBuilder.Entity<Articles>()
                .HasIndex(a => a.Url)
                .IsUnique();

            modelBuilder.Entity<Articles>()
                .HasIndex(a => a.Guid)
                .IsUnique();

        }
    }
}
