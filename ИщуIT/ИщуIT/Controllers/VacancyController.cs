using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet("{id}", Name = "GetVacancy")]
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
        [HttpPost]
        public IActionResult CreateVacancy([FromBody] VacancyCreateDto vacancy)
        {
            if (vacancy == null)
            {
                _logger.LogError("VacancyCreateDto object sent from client is null.");
                return BadRequest("VacancyCreateDto object is null");
            }
            var vacancyEntity = _mapper.Map<Vacancy>(vacancy);
            _repository.Vacancy.CreateVacancy(vacancyEntity);
            _repository.Save();
            var vacancyReturn = _mapper.Map<VacancyDto>(vacancyEntity);
            return CreatedAtRoute("GetVacancy", new { vacancyReturn.Id }, vacancyReturn);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteVacancy(Guid id)
        {
            var vacancy = _repository.Vacancy.GetVacancy(id, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Vacancy.DeleteVacancy(vacancy);
            _repository.Save();
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateVacancy(Guid id, [FromBody] VacancyUpdateDto vacancy)
        {
            if (vacancy == null)
            {
                _logger.LogError("VacancyUpdateDto object sent from client is null.");
                return BadRequest("VacancyUpdateDto object is null");
            }
            var vacancyEntity = _repository.Vacancy.GetVacancy(id, true);
            if (vacancyEntity == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(vacancy, vacancyEntity);
            _repository.Save();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult UpdateVacancy(Guid id, [FromBody] JsonPatchDocument<VacancyUpdateDto> vacancy)
        {
            if (vacancy == null)
            {
                _logger.LogError("VacancyUpdateDto object sent from client is null.");
                return BadRequest("VacancyUpdateDto object is null");
            }
            var vacancyEntity = _repository.Vacancy.GetVacancy(id, true);
            if (vacancyEntity == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var vacancyPatch = _mapper.Map<VacancyUpdateDto>(vacancyEntity);
            vacancy.ApplyTo(vacancyPatch);
            _mapper.Map(vacancyPatch, vacancyEntity);
            _repository.Save();
            return NoContent();
        }
    }
}
