using AutoMapper;
using Microsoft.AspNetCore.Identity;
using CompanyMVC.ViewModels;

namespace CompanyMVC.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(d=> d.RoleName , o=> o.MapFrom(s=>s.Name))
                .ReverseMap();
        }
    }
}
