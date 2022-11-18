﻿using AutoMapper;
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
    [Route("api/vacancy/{vacancyId}/itcompanies")]
    [ApiController]
    public class ItCompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ItCompanyDto> _dataShaper;
        public ItCompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<ItCompanyDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        /// <summary>
        /// Возврящает список ИТ-компаний
        /// </summary>
        /// <param name="vacancyId">Id вакансии</param>
        /// <param name="id">Id ИТ-компании</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetItCompanies"), Authorize]
        public async Task<IActionResult> GetItCompanyAsync(Guid vacancyId, Guid id)
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
        /// <summary>
        /// Возврящает ИТ-компанию
        /// </summary>
        /// <param name="vacancyId">Id вакансии</param>
        /// <param name="parameters">Параметры возвращаемого списка</param>
        /// <returns></returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetItCompaniesAsync(Guid vacancyId, [FromQuery] ItCompanyParameters parameters)
        {
            var vacancy = await _repository.Vacancy.GetVacancyAsync(vacancyId, false);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return NotFound();
            }
            var itCompaniesFromDb = await _repository.ItCompany.GetItCompaniesAsync(vacancyId, false, parameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(itCompaniesFromDb.MetaData));
            var itCompanies = _mapper.Map<IEnumerable<ItCompanyDto>>(itCompaniesFromDb);
            return Ok(_dataShaper.ShapeData(itCompanies, parameters.Fields));
        }
        /// <summary>
        /// Создает ИТ-компанию
        /// </summary>
        /// <param name="vacancyId">Id вакансии</param>
        /// <param name="itCompany">Модель создания ИТ-компании</param>
        /// <returns></returns>
        [HttpPost, Authorize]
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
        /// <summary>
        /// Удаляет ИТ-компанию
        /// </summary>
        /// <param name="vacancyId">Id вакансии</param>
        /// <param name="id">Id ИТ-компании</param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        [ServiceFilter(typeof(ValidateItCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteItCompanyAsync(Guid vacancyId, Guid id)
        {
            var itCompany = HttpContext.Items["itcompany"] as ItCompany;
            _repository.ItCompany.DeleteItCompany(itCompany);
            await _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Изменяет ИТ-компанию
        /// </summary>
        /// <param name="vacancyId">Id вакансии</param>
        /// <param name="id">Id ИТ-компании</param>
        /// <param name="itCompany">Модель обновления ИТ-компании</param>
        /// <returns></returns>
        [HttpPut("{id}"), Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateItCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateItCompanyAsync(Guid vacancyId, Guid id, [FromBody] ItCompanyUpdateDto itCompany)
        {

            var itCompanyEntity = HttpContext.Items["itcompany"] as ItCompany;
            _mapper.Map(itCompany, itCompanyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Изменяет ИТ-компанию
        /// </summary>
        /// <param name="vacancyId">Id вакансии</param>
        /// <param name="id">Id ИТ-компании</param>
        /// <param name="itCompany"></param>
        /// <returns></returns>
        [HttpPatch("{id}"), Authorize]
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
