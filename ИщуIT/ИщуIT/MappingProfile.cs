using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace ИщуIT
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
            opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<ItCompany, ItCompanyDto>();
            CreateMap<Vacancy, VacancyDto>();

            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<ItCompanyCreateDto, ItCompany>();
            CreateMap<VacancyCreateDto, Vacancy>();

            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<VacancyUpdateDto, Vacancy>().ReverseMap();
            CreateMap<ItCompanyUpdateDto, ItCompany>().ReverseMap();
        }
    }
}
