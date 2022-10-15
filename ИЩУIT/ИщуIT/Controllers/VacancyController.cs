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
            var vacancies = _repository.Vacancy.GetVacancies(false);
            var vacanciesDto = _mapper.Map<IEnumerable<VacancyDto>>(vacancies);
            return Ok(vacanciesDto);
        }
        [HttpGet("{id}")]
        public IActionResult GetVacancy(Guid id)
        {
            var vacancy = _repository.Vacancy.GetVacancy(id, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var vacancyDto = _mapper.Map<VacancyDto>(vacancy);
            return Ok(vacancyDto);
        }
    }
}
