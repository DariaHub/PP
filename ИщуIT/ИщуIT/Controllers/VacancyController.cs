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
        public async Task<IActionResult> GetVacanciesAsync()
        {
            var vacancies = await _repository.Vacancy.GetVacanciesAsync(false);
            var vacanciesDto = _mapper.Map<IEnumerable<VacancyDto>>(vacancies);
            return Ok(vacanciesDto);
        }
        [HttpGet("{id}", Name = "GetVacancy")]
        public async Task<IActionResult> GetVacancyAsync(Guid id)
        {
            var vacancy = await _repository.Vacancy.GetVacancyAsync(id, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var vacancyDto = _mapper.Map<VacancyDto>(vacancy);
            return Ok(vacancyDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateVacancyAsync([FromBody] VacancyCreateDto vacancy)
        {
            if (vacancy == null)
            {
                _logger.LogError("VacancyCreateDto object sent from client is null.");
                return BadRequest("VacancyCreateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the VacancyCreateDto object");
                return UnprocessableEntity(ModelState);
            }
            var vacancyEntity = _mapper.Map<Vacancy>(vacancy);
            _repository.Vacancy.CreateVacancy(vacancyEntity);
            await _repository.SaveAsync();
            var vacancyReturn = _mapper.Map<VacancyDto>(vacancyEntity);
            return CreatedAtRoute("GetVacancy", new { vacancyReturn.Id }, vacancyReturn);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancyAsync(Guid id)
        {
            var vacancy = await _repository.Vacancy.GetVacancyAsync(id, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Vacancy.DeleteVacancy(vacancy);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVacancyAsync(Guid id, [FromBody] VacancyUpdateDto vacancy)
        {
            if (vacancy == null)
            {
                _logger.LogError("VacancyUpdateDto object sent from client is null.");
                return BadRequest("VacancyUpdateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the VacancyUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var vacancyEntity = await _repository.Vacancy.GetVacancyAsync(id, true);
            if (vacancyEntity == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(vacancy, vacancyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateVacancyAsync(Guid id, [FromBody] JsonPatchDocument<VacancyUpdateDto> vacancy)
        {
            if (vacancy == null)
            {
                _logger.LogError("VacancyUpdateDto object sent from client is null.");
                return BadRequest("VacancyUpdateDto object is null");
            }
            var vacancyEntity = await _repository.Vacancy.GetVacancyAsync(id, true);
            if (vacancyEntity == null)
            {
                _logger.LogInfo($"Vacancy with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var vacancyPatch = _mapper.Map<VacancyUpdateDto>(vacancyEntity);
            vacancy.ApplyTo(vacancyPatch, ModelState);
            TryValidateModel(vacancyPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(vacancyPatch, vacancyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
