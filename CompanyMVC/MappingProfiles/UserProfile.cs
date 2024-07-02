using AutoMapper;
using DAL.Models;
using CompanyMVC.ViewModels;

namespace CompanyMVC.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<ApplicationUser , UserViewModel>().ReverseMap();
        }
    }
}
