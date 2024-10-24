using Azure;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;
namespace DailyNews
{
    public class ApplicationDbContext : DbContext //lớp tương tác trực tiếp với db
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<RSS_Sources> RssSources { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RSS_Category> RssCategories { get; set; }
        public DbSet<Articles> Articles { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Article_Tags> ArticleTags { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<User_Tags> UserTags { get; set; }
        public DbSet<Comments> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articles>()
                .HasIndex(a => a.Url)
                .IsUnique();

            modelBuilder.Entity<Articles>()
                .HasIndex(a => a.Guid)
                .IsUnique();

            modelBuilder.Entity<User_Tags>()
                .HasKey(ut => new { ut.UserId, ut.TagId });

            modelBuilder.Entity<User_Tags>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTags)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User_Tags>()
                .HasOne(ut => ut.Tag)
                .WithMany(t => t.UserTags)
                .HasForeignKey(ut => ut.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Article_Tags>()
                .HasKey(at => new { at.ArticleId, at.TagId });

            modelBuilder.Entity<Article_Tags>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTags)
                .HasForeignKey(at => at.ArticleId);

            modelBuilder.Entity<Article_Tags>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId);

            modelBuilder.Entity<Article_Category>()
                .HasKey(ac => new { ac.ArticleId, ac.CategoryId });

            modelBuilder.Entity<Article_Category>()
                .HasOne(ac => ac.Article)
                .WithMany(a => a.ArticleCategory)
                .HasForeignKey(ac => ac.ArticleId);

            modelBuilder.Entity<Article_Category>()
                .HasOne(ac => ac.Category)
                .WithMany(c => c.ArticleCategory)
                .HasForeignKey(ac => ac.CategoryId);

            modelBuilder.Entity<Comments>()
               .HasOne(c => c.User)
               .WithMany(u => u.UserComments)
               .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Comments>()
                .HasOne(c => c.Article)
                .WithMany(a => a.ArticleComment) 
                .HasForeignKey(c => c.ArticleId);

            modelBuilder.Entity<Likes>()
                .HasKey(l => new { l.UserId, l.ArticleId }); 

            modelBuilder.Entity<Likes>()
                .HasOne(l => l.User)
                .WithMany(u => u.UserLikes) 
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Likes>()
                .HasOne(l => l.Article)
                .WithMany() 
                .HasForeignKey(l => l.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
