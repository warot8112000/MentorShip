using DailyNews.Model;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Xml;
using HtmlAgilityPack;
using DailyNews.DTO;

namespace DailyNews.Services
{
    public class FetchDataService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FetchDataService> _logger;

        public FetchDataService(ApplicationDbContext context, IHttpClientFactory httpClientFactory, ILogger<FetchDataService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task GetRssCategoriesFromRssSources()
        {
            var rssSources = await _context.RssSources.ToListAsync();

            try
            {
                foreach (var rssSource in rssSources)
                {
                    var rssCategories = await GetRssCategories(rssSource.Url);
                    var newRssCategories = new List<RSS_Category>();
                    rssCategories = rssCategories.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                    foreach (var categoryUrl in rssCategories)
                    {
                        var category = await GetOrCreateCategory(categoryUrl);
                        if (category != null)
                        {
                            var existingRssCategory = await _context.RssCategories
                                .FirstOrDefaultAsync(rc => rc.Url == categoryUrl);

                            if (existingRssCategory == null)
                            {
                                // Tạo mới RssCategory mà không cần sử dụng mapper
                                var newRssCategory = new RSS_Category
                                {
                                    RssSourceId = rssSource.Id,
                                    CategoryId = category.Id,
                                    Url = categoryUrl,
                                    Description = $"RSS feed for category at '{categoryUrl}'"
                                };

                                newRssCategories.Add(newRssCategory);
                            }
                        }
                    }

                    if (newRssCategories.Any())
                    {
                        await _context.RssCategories.AddRangeAsync(newRssCategories);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Đã thêm {newRssCategories.Count} RssCategories mới cho nguồn '{rssSource.Url}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Đã xảy ra lỗi khi xử lý các nguồn RSS: {ex.Message}");
            }
        }



        public async Task<List<string>> GetRssCategories(string url)
        {
            var rssLinks = new List<string>();
            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync(); //đọc nd

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody); //tất cả các liên kết đến RSS feeds

                var links = htmlDoc.DocumentNode.SelectNodes("//a[contains(@href, 'rss')]");

                if (links != null)
                {
                    foreach (var link in links)
                    {
                        string href = link.GetAttributeValue("href", string.Empty);
                        // Kiểm tra và thêm vào danh sách chỉ những đường dẫn hợp lệ
                        if (href.StartsWith("/")) //liên kết tương đối thường bắt đầu bằng /, cho thấy rằng nó sẽ bắt đầu từ gốc của miền hiện tại
                        {
                            href = new Uri(new Uri(url), href).ToString(); // Tạo URL đầy đủ
                        }
                        if (IsValidRssCategory(href))
                        {
                            rssLinks.Add(href);
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Lỗi khi truy cập trang {url}: {e.Message}");
            }
            return rssLinks;
        }

        private bool IsValidRssCategory(string url)
        {
            // Kiểm tra nếu URL chứa '/rss/' và kết thúc với '.rss'
            return url.Contains("/rss/") && url.EndsWith(".rss");
        }

        private async Task<Category> GetOrCreateCategory(string categoryUrl)
        {
            var slug = GetCategorySlugFromUrl(categoryUrl);
            var categoryName = ConvertSlugToCategoryName(slug);

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (category == null)
            {
                category = new Category
                {
                    Name = categoryName,
                    Description = $"RSS feed for category '{categoryName}'"
                };
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            return category;
        }


        private string GetCategorySlugFromUrl(string categoryUrl)
        {
            try
            {
                var uri = new Uri(categoryUrl);
                var segments = uri.Segments;
                var lastSegment = segments.LastOrDefault()?.TrimEnd('/');
                if (string.IsNullOrEmpty(lastSegment))
                    return null;

                // Remove the ".rss" extension
                if (lastSegment.EndsWith(".rss", StringComparison.OrdinalIgnoreCase))
                {
                    return lastSegment.Substring(0, lastSegment.Length - 4); // Remove ".rss"
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error parsing URL '{categoryUrl}': {ex.Message}");
                return null;
            }
        }

        private string ConvertSlugToCategoryName(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return string.Empty;

            var words = slug.Split('-', StringSplitOptions.RemoveEmptyEntries);
            var capitalizedWords = words.Select(word => char.ToUpper(word[0]) + word.Substring(1));
            return string.Join(" ", capitalizedWords);
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

        public async Task AddTagsToArticles()
        {
            // Lấy tất cả các bài viết từ cơ sở dữ liệu
            var articles = await _context.Articles.ToListAsync();

            foreach (var article in articles)
            {
                // Lấy danh sách các tag từ nội dung bài viết (URL hoặc body)
                var tagsFromArticle = await GetArticleTags(article.Url);

                // Thêm các tag vào Article_Tags
                foreach (var tagName in tagsFromArticle)
                {
                    // Kiểm tra xem tag đã tồn tại chưa, nếu chưa có thì thêm vào Tags
                    var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Tags { Name = tagName };
                        await _context.Tags.AddAsync(tag);
                        await _context.SaveChangesAsync();
                    }

                    // Kiểm tra xem mối quan hệ Article_Tags đã tồn tại chưa
                    var articleTagExists = await _context.ArticleTags
                        .AnyAsync(at => at.ArticleId == article.Id && at.TagId == tag.Id);

                    if (!articleTagExists)
                    {
                        var articleTag = new Article_Tags
                        {
                            ArticleId = article.Id,
                            TagId = tag.Id
                        };
                        await _context.ArticleTags.AddAsync(articleTag);
                    }
                }
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }


        public async Task<List<string>> GetArticleTags(string url)
        {
            var tags = new HashSet<string>(); // Sử dụng HashSet để loại bỏ trùng lặp
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);

                var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@title]");

                if (linkNodes != null)
                {
                    foreach (var link in linkNodes)
                    {
                        string title = link.GetAttributeValue("title", string.Empty);
                        string href = link.GetAttributeValue("href", string.Empty);

                        if (IsValidTag(title, href, url))
                        {
                            tags.Add(title.Trim());
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Lỗi khi truy cập trang {url}: {e.Message}");
            }

            return tags.ToList();
        }

        private bool IsValidTag(string title, string href, string url)
        {
            return !string.IsNullOrWhiteSpace(title) &&
                   !string.IsNullOrWhiteSpace(href) &&
                   href.StartsWith("/");
        }
    }
}
