﻿namespace DailyNews.DTO
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
        public int RssCategoryId { get; set; }  
        public string Guid { get; set; }
        public string EnclosureUrl { get; set; }
    }
}