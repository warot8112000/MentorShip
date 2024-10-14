using HtmlAgilityPack;

namespace DailyNews.Services
{
    public class TagService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TagService> _logger;

        public TagService(IHttpClientFactory httpClientFactory, ILogger<TagService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
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

                var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a");

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
                   href.StartsWith("/") &&
                   href.StartsWith(new Uri(url).PathAndQuery);
        }
    }
}
