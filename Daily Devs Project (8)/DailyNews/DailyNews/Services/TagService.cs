using DailyNews.Model;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace DailyNews.Services
{
    public class TagService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TagService> _logger;
        private readonly ApplicationDbContext _context;


        public TagService(ApplicationDbContext context, IHttpClientFactory httpClientFactory, ILogger<TagService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

       
    }
}
