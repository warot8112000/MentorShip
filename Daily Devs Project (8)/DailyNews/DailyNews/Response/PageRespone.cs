namespace DailyNews.Response
{
    public class PagedResponse<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedResponse(int totalCount, int pageNumber, int pageSize, IEnumerable<T> data)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            Data = data;
        }
    }
}
