using AutoMapper;
using CompanyMVC.ViewModels;
using DAL.Models;

namespace CompanyMVC.MappingProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
