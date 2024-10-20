using Quartz;

namespace DailyNews.Services
{
    public class FetchArticlesJob : IJob
    {
        private readonly FetchDataService _articleFetcher;
        private readonly ILogger<FetchArticlesJob> _logger;

        public FetchArticlesJob(FetchDataService articleFetcher, ILogger<FetchArticlesJob> logger)
        {
            _articleFetcher = articleFetcher;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"FetchArticlesJob is running at {DateTime.Now}");
            await _articleFetcher.FetchLatestArticlesFromRssCategories();
        }
    }
}
