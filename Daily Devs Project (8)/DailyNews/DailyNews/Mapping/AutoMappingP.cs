using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;

namespace DailyNews.Mapping
{
    public class AutoMappingP : Profile
    {
        public AutoMappingP()
        {          
            CreateMap<Articles, ArticleDto>().ReverseMap();
            CreateMap<RSS_Sources, RSS_SourcesDto>().ReverseMap();
            CreateMap<RSS_Category, RSS_CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<Users, UserDto>();
        }
    }
}
