using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ИщуIT.Controllers
{
    [Route("api/vacancy")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public VacancyController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetVacancies()
        {
            var vacancies = _repository.Vacancy.GetAllVacancy(false);
            var vacanciesDto = vacancies.Select( c => new VacancyDto
            {
                Id = c.Id,
                Name = c.Name,
                Quantity = c.Quantity,
                Salary = c.Salary
            }).ToList();
            return Ok(vacanciesDto);
        }
    }
}
