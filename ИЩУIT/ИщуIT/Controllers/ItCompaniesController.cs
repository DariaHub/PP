using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ИщуIT.Controllers
{
    [Route("api/itcompanies")]
    [ApiController]
    public class ItCompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ItCompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetItCompanies()
        {
            var itCompanies = _repository.ItCompany.GetAllItCompanies(false);
            var itCompaniesDto = itCompanies.Select( c => new ItCompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Id_Vacancy = c.Id_Vacancy,
                Phone = c.Phone
            }).ToList();
            return Ok(itCompaniesDto);
        }
    }
}
