using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ИщуIT.ActionFilters;

namespace ИщуIT.Controllers
{
    [Route("api/vacancy")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<VacancyDto> _dataShaper;
        public VacancyController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<VacancyDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        /// <summary>
        /// Возврящает список вакансий
        /// </summary>
        /// <param name="parameters">Параметры возвращаемого списка</param>
        /// <returns></returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetVacanciesAsync([FromQuery] VacancyParameters parameters)
        {
            var vacancies = await _repository.Vacancy.GetVacanciesAsync(false, parameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(vacancies.MetaData));
            var vacanciesDto = _mapper.Map<IEnumerable<VacancyDto>>(vacancies);
            return Ok(_dataShaper.ShapeData(vacanciesDto, parameters.Fields));
        }
        /// <summary>
        /// Возвращает вакансию
        /// </summary>
        /// <param name="id">Id вакансии</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetVacancy"), Authorize]
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
        /// <summary>
        /// Создает вакансию
        /// </summary>
        /// <param name="vacancy">Модель создания вакансии</param>
        /// <returns></returns>
        [HttpPost, Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateVacancyAsync([FromBody] VacancyCreateDto vacancy)
        {
            var vacancyEntity = _mapper.Map<Vacancy>(vacancy);
            _repository.Vacancy.CreateVacancy(vacancyEntity);
            await _repository.SaveAsync();
            var vacancyReturn = _mapper.Map<VacancyDto>(vacancyEntity);
            return CreatedAtRoute("GetVacancy", new { vacancyReturn.Id }, vacancyReturn);
        }
        /// <summary>
        /// Удаляет вакансию
        /// </summary>
        /// <param name="id">Id вакансии</param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        [ServiceFilter(typeof(ValidateVacancyExistsAttribute))]
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
        /// <summary>
        /// Изменяет вакансию
        /// </summary>
        /// <param name="id">Id вакансии</param>
        /// <param name="vacancy">Модель обновления вакансии</param>
        /// <returns></returns>
        [HttpPut("{id}"), Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateVacancyExistsAttribute))]
        public async Task<IActionResult> UpdateVacancyAsync(Guid id, [FromBody] VacancyUpdateDto vacancy)
        {
            var vacancyEntity = HttpContext.Items["vacancy"] as Vacancy;
            _mapper.Map(vacancy, vacancyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Изменяет вакансию
        /// </summary>
        /// <param name="id">Id вакансии</param>
        /// <param name="vacancy"></param>
        /// <returns></returns>
        [HttpPatch("{id}"), Authorize]
        [ServiceFilter(typeof(ValidateVacancyExistsAttribute))]
        public async Task<IActionResult> UpdateVacancyAsync(Guid id, [FromBody] JsonPatchDocument<VacancyUpdateDto> vacancy)
        {
            if (vacancy == null)
            {
                _logger.LogError("VacancyUpdateDto object sent from client is null.");
                return BadRequest("VacancyUpdateDto object is null");
            }
            var vacancyEntity = HttpContext.Items["vacancy"] as Vacancy;
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
