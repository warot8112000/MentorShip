using Quartz;

namespace DailyNews.Services
{
    public class FetchArticlesJob : IJob
    {
        private readonly FetchDataService _articleFetcher;

        public FetchArticlesJob(FetchDataService articleFetcher)
        {
            _articleFetcher = articleFetcher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _articleFetcher.FetchLatestArticlesFromRssCategories();
        }
    }
}
