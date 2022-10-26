using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetItCompanies(Guid vacancyId, Guid id)
        {
            var vacancy = _repository.Vacancy.GetVacancy(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompanyDb = _repository.ItCompany.GetItCompany(vacancyId, id, false);
            if (itCompanyDb == null)
            {
                _logger.LogInfo($"ItCompany with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var itCompany = _mapper.Map<ItCompanyDto>(itCompanyDb);
            return Ok(itCompany);
        }
        [HttpGet]
        public IActionResult GetItCompany(Guid vacancyId)
        {
            var vacancy = _repository.Vacancy.GetVacancy(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompaniesFromDb = _repository.ItCompany.GetItCompanies(vacancyId, false);
            var itCompanies = _mapper.Map<IEnumerable<ItCompanyDto>>(itCompaniesFromDb);
            return Ok(itCompanies);
        }
        [HttpPost]
        public IActionResult CreateItCompanies(Guid vacancyId, [FromBody] ItCompanyCreateDto itCompany)
        {
            if (itCompany == null)
            {
                _logger.LogError("ItCompanyCreateDto object sent from client is null.");
                return BadRequest("ItCompanyCreateDto object is null");
            }
            var vacancy = _repository.Vacancy.GetVacancy(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompaniesEntity = _mapper.Map<ItCompany>(itCompany);
            _repository.ItCompany.CreateItCompany(vacancyId, itCompaniesEntity);
            _repository.Save();
            var itCompanyReturn = _mapper.Map<ItCompanyDto>(itCompaniesEntity);
            return CreatedAtRoute("GetItCompanies", new { vacancyId, itCompanyReturn.Id}, itCompanyReturn);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteItCompany(Guid vacancyId, Guid id)
        {
            var vacancy = _repository.Vacancy.GetVacancy(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompany = _repository.ItCompany.GetItCompany(vacancyId, id, false);
            if (itCompany == null)
            {
                _logger.LogInfo($"ItCompany with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.ItCompany.DeleteItCompany(itCompany);
            _repository.Save();
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateItCompany(Guid vacancyId, Guid id, [FromBody] ItCompanyUpdateDto itCompany)
        {
            if (itCompany == null)
            {
                _logger.LogError("ItCompanyCreateDto object sent from client is null.");
                return BadRequest("ItCompanyCreateDto object is null");
            }
            var vacancy = _repository.Vacancy.GetVacancy(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompanyEntity = _repository.ItCompany.GetItCompany(vacancyId, id, true);
            if (itCompanyEntity == null)
            {
                _logger.LogInfo($"ItCompany with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(itCompany, itCompanyEntity);
            _repository.Save();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult UpdateItCompany(Guid vacancyId, Guid id, [FromBody] JsonPatchDocument<ItCompanyUpdateDto> itCompany)
        {
            if (itCompany == null)
            {
                _logger.LogError("ItCompanyCreateDto object sent from client is null.");
                return BadRequest("ItCompanyCreateDto object is null");
            }
            var vacancy = _repository.Vacancy.GetVacancy(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompanyEntity = _repository.ItCompany.GetItCompany(vacancyId, id, true);
            if (itCompanyEntity == null)
            {
                _logger.LogInfo($"ItCompany with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var itCompanyPatch = _mapper.Map<ItCompanyUpdateDto>(itCompanyEntity);
            itCompany.ApplyTo(itCompanyPatch);
            _mapper.Map(itCompanyPatch, itCompanyEntity);
            _repository.Save();
            return NoContent();
        }
    }
}
