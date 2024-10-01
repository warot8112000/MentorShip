using DailyNews.Model;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
namespace DailyNews.Services
{
    public class RssFeedService
    {
        private readonly DailyNewsContext _context;

        public RssFeedService(DailyNewsContext context)
        {
            _context = context;
        }

        public async Task FetchAndStoreArticlesAsync()
        {
            var rssSources = await _context.RsSources.ToListAsync(); 

            foreach (var source in rssSources)
            {
                var articles = await FetchRssFeedAsync(source.Url); // Lấy bài viết từ nguồn RSS
                foreach (var article in articles)
                {                   
                    if (!_context.Articles.Any(a => a.Url == article.Url))
                    {
                        article.RssSourceId = source.Id; 
                        _context.Articles.Add(article); 
                    }
                }
            }

            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        //phân tích dữ liệu từ rss và chuyển thành article
        private async Task<List<Article>> FetchRssFeedAsync(string url)
        {
            using HttpClient client = new HttpClient();
            var xml = await client.GetStringAsync(url); // Lấy dữ liệu XML từ URL
            XDocument doc = XDocument.Parse(xml); 

            var articles = doc.Descendants("item") //Descendants("item") trả về tất cả các phần tử con <item> trong tài liệu XML
                .Select(item => new Article
                {
                    Title = item.Element("title")?.Value ?? "No Title",
                    Url = item.Element("link")?.Value ?? string.Empty,
                    Content = item.Element("description")?.Value,
                    PublishedAt = DateTime.Parse(item.Element("pubDate")?.Value ?? DateTime.Now.ToString())
                }).ToList();

            return articles; 
        }
    }
}
