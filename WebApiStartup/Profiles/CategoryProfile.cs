using AutoMapper;
using WebApiStartup.DTOs;
using WebApiStartup.Entity;

namespace WebApiStartup.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
        }
    }
}
