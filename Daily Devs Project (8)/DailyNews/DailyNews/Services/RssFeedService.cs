using DailyNews.Model;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace DailyNews.Services
{
    public class RssFeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RssFeedService> _logger;

        public RssFeedService(ApplicationDbContext context, IHttpClientFactory httpClientFactory, ILogger<RssFeedService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task FetchAndSaveArticlesFromRssCategories()
        {
            var rssCategories = await _context.RssCategories.ToListAsync();

            var httpClient = _httpClientFactory.CreateClient();

            foreach (var rssCategory in rssCategories)
            {
                try
                {
                    var xml = await httpClient.GetStringAsync(rssCategory.Url);
                    var doc = XDocument.Parse(xml);

                    foreach (var item in doc.Descendants("item"))
                    {
                        var article = new Articles
                        {
                            Title = item.Element("title")?.Value ?? "Default Title",
                            Url = item.Element("link")?.Value ?? "https://default.url",
                            Content = item.Element("description")?.Value ?? "No content available",
                            PublishedAt = DateTime.TryParse(item.Element("pubDate")?.Value, out var publishedDate) ? publishedDate : DateTime.Now,
                            RssCategoryId = rssCategory.Id,
                            Guid = item.Element("guid")?.Value ?? "Default GUID",
                            EnclosureUrl = item.Element("enclosure")?.Attribute("url")?.Value ?? "https://default.enclosure.url"
                        };

                        // Kiểm tra xem bài viết đã tồn tại chưa
                        try
                        {
                            var existingArticle = await _context.Articles.AnyAsync(a => a.Guid == article.Guid);
                            if (!existingArticle)
                            {
                                await _context.Articles.AddAsync(article);
                                _logger.LogInformation($"Article added: {article.Title}");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error processing article: {article.Title}");
                        }
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Successfully saved articles from {rssCategory.Url}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error saving changes to the database");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error fetching articles from {rssCategory.Url}");
                }
            }
        }
    }
}
