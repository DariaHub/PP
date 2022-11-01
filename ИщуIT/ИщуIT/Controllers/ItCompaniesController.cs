using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ИщуIT.ActionFilters;

namespace ИщуIT.Controllers
{
    [Route("api/vacancy/{vacancyId}/itcompanies")]
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
        [HttpGet("{id}", Name = "GetItCompanies")]
        public async Task<IActionResult> GetItCompaniesAsync(Guid vacancyId, Guid id)
        {
            var vacancy = await _repository.Vacancy.GetVacancyAsync(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompanyDb = await _repository.ItCompany.GetItCompanyAsync(vacancyId, id, false);
            if (itCompanyDb == null)
            {
                _logger.LogInfo($"ItCompany with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var itCompany = _mapper.Map<ItCompanyDto>(itCompanyDb);
            return Ok(itCompany);
        }
        [HttpGet]
        public async Task<IActionResult> GetItCompanyAsync(Guid vacancyId)
        {
            var vacancy = await _repository.Vacancy.GetVacancyAsync(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompaniesFromDb = await _repository.ItCompany.GetItCompaniesAsync(vacancyId, false);
            var itCompanies = _mapper.Map<IEnumerable<ItCompanyDto>>(itCompaniesFromDb);
            return Ok(itCompanies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateItCompaniesAsync(Guid vacancyId, [FromBody] ItCompanyCreateDto itCompany)
        {
            if (itCompany == null)
            {
                _logger.LogError("ItCompanyCreateDto object sent from client is null.");
                return BadRequest("ItCompanyCreateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ItCompanyCreateDto object");
                return UnprocessableEntity(ModelState);
            }
            var vacancy = await _repository.Vacancy.GetVacancyAsync(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompaniesEntity = _mapper.Map<ItCompany>(itCompany);
            _repository.ItCompany.CreateItCompany(vacancyId, itCompaniesEntity);
            await _repository.SaveAsync();
            var itCompanyReturn = _mapper.Map<ItCompanyDto>(itCompaniesEntity);
            return CreatedAtRoute("GetItCompanies", new { vacancyId, itCompanyReturn.Id}, itCompanyReturn);
        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateItCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteItCompanyAsync(Guid vacancyId, Guid id)
        {
            var itCompany = HttpContext.Items["itcompany"] as ItCompany;
            _repository.ItCompany.DeleteItCompany(itCompany);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateItCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateItCompanyAsync(Guid vacancyId, Guid id, [FromBody] ItCompanyUpdateDto itCompany)
        {

            var itCompanyEntity = HttpContext.Items["itcompany"] as ItCompany;
            _mapper.Map(itCompany, itCompanyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateItCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateItCompanyAsync(Guid vacancyId, Guid id, [FromBody] JsonPatchDocument<ItCompanyUpdateDto> itCompany)
        {
            if (itCompany == null)
            {
                _logger.LogError("ItCompanyCreateDto object sent from client is null.");
                return BadRequest("ItCompanyCreateDto object is null");
            }
            var itCompanyEntity = HttpContext.Items["itcompany"] as ItCompany;
            var itCompanyPatch = _mapper.Map<ItCompanyUpdateDto>(itCompanyEntity);
            itCompany.ApplyTo(itCompanyPatch, ModelState);
            TryValidateModel(itCompanyPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(itCompanyPatch, itCompanyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
