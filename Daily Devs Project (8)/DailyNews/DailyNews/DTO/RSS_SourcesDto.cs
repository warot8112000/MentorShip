namespace DailyNews.DTO
{
    public class RSS_SourcesDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public ICollection<RSS_CategoryDto> RssCategories { get; set; }
    }
}
