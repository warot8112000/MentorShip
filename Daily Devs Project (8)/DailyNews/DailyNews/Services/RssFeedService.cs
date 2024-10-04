using DailyNews.Model;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
namespace DailyNews.Services
{
    public class RssFeedService
    {
        private readonly ApplicationDbContext _context;

        public RssFeedService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task FetchAndSaveArticlesFromRssCategories()
        {
            var rssCategories = await _context.RssCategories.ToListAsync();

            using (var httpClient = new HttpClient())
            {
                foreach (var rssCategory in rssCategories)
                {
                    var xml = await httpClient.GetStringAsync(rssCategory.Url);
                    var doc = XDocument.Parse(xml);

                    foreach (var item in doc.Descendants("item"))
                    {
                        var article = new Article
                        {
                            Title = item.Element("title")?.Value ?? "Default Title", 
                            Url = item.Element("link")?.Value ?? "https://default.url", 
                            Content = item.Element("description")?.Value ?? "No content available", 
                            PublishedAt = DateTime.TryParse(item.Element("pubDate")?.Value, out var publishedDate) ? publishedDate : DateTime.Now, 
                            RssCategoryId = rssCategory.Id,
                            Guid = item.Element("guid")?.Value ?? "Default GUID", 
                            EnclosureUrl = item.Element("enclosure")?.Attribute("url")?.Value ?? "https://default.enclosure.url" 

                        };

                        // Lưu bài viết vào cơ sở dữ liệu
                        _context.Articles.Add(article);
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}