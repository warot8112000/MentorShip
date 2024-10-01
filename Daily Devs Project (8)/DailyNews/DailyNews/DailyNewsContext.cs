using DailyNews.Model;
using Microsoft.EntityFrameworkCore;
namespace DailyNews
{
    public class DailyNewsContext : DbContext
    {
        public DailyNewsContext(DbContextOptions<DailyNewsContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<RssSource> RsSources { get; set; }

    }
}
