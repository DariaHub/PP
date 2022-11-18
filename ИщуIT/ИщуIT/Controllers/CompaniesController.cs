using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using System;
using ИщуIT.ActionFilters;
using ИщуIT.ModelBinders;
using static System.Collections.Specialized.BitVector32;

namespace ИщуIT.Controllers
{
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Возвращает список компаний
        /// </summary>
        /// <param name="parameters">Параметры возвращаемого списка</param>
        /// <returns></returns>
        [HttpGet(Name = "GetCompanies"), Authorize]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters parameters)
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(false, parameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(companies.MetaData));
            var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDto);
        }
        /// <summary>
        /// Возвращает компанию
        /// </summary>
        /// <param name="id">Id компании</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "CompanyById"), Authorize]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
        }
        /// <summary>
        /// Создает компанию
        /// </summary>
        /// <param name="company">Модель создания компании</param>
        /// <returns></returns>
        [HttpPost, Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync(); 
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id },
            companyToReturn);
        }
        /// <summary>
        /// Возвращает коллекцию
        /// </summary>
        /// <param name="ids">Коллекция ids</param>
        /// <returns></returns>
        [HttpGet("collection/({ids})", Name = "CompanyCollection"), Authorize]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var companyEntities = await _repository.Company.GetByIdsAsync(ids, false);
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var companiesToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return Ok(companiesToReturn);
        }
        /// <summary>
        /// Создает коллекцию
        /// </summary>
        /// <param name="companyCollection">Коллекция компаний</param>
        /// <returns></returns>
        [HttpPost("collection"), Authorize]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null)
            {
                _logger.LogError("Company collection sent from client is null.");
                return BadRequest("Company collection is null");
            }
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CompanyCollection", new { ids }, companyCollectionToReturn);
        }
        /// <summary>
        /// Удаляет компанию
        /// </summary>
        /// <param name="id">Id компании</param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var company = HttpContext.Items["company"] as Company;
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Изменяет компанию
        /// </summary>
        /// <param name="id">Id компании</param>
        /// <param name="company">Модель обновления компании</param>
        /// <returns></returns>
        [HttpPut("{id}"), Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            var companyEntity = HttpContext.Items["company"] as Company;
            _mapper.Map(company, companyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Возвращает список запросов
        /// </summary>
        /// <returns></returns>
        [HttpOptions, Authorize]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
