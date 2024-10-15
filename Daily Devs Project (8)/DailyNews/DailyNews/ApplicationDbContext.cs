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
//        public DbSet<Like> Likes { get; set; }
        public DbSet<User_Tags> UserTags { get; set; }
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

            // Định nghĩa khóa chính hợp ghép cho UserTag
            modelBuilder.Entity<User_Tags>()
                .HasKey(ut => new { ut.UserId, ut.TagId });

            // Cấu hình mối quan hệ UserTag với User
            modelBuilder.Entity<User_Tags>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTags)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ UserTag với Tag
            modelBuilder.Entity<User_Tags>()
                .HasOne(ut => ut.Tag)
                .WithMany(t => t.UserTags)
                .HasForeignKey(ut => ut.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            // Thiết lập khóa chính composite cho bảng Article_Tags
            modelBuilder.Entity<Article_Tags>()
                .HasKey(at => new { at.ArticleId, at.TagId });

            // Thiết lập quan hệ giữa bảng Articles và Article_Tags
            modelBuilder.Entity<Article_Tags>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTags)
                .HasForeignKey(at => at.ArticleId);

            // Thiết lập quan hệ giữa bảng Tags và Article_Tags
            modelBuilder.Entity<Article_Tags>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId);
        }
    }
}
