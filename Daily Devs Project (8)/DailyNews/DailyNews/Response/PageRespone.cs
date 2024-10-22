namespace DailyNews.Response
{
    public class PagedResponse<T>
    {
        public int TotalArticles { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedResponse(int totalArticles, int pageNumber, int pageSize, IEnumerable<T> data)
        {
            TotalArticles = totalArticles;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalArticles / pageSize);
            Data = data;
        }
    }
}
