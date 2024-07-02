using AutoMapper;
using CompanyMVC.ViewModels;
using DAL.Models;

namespace CompanyMVC.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel , Employee>().ReverseMap();
        }
    }
}
