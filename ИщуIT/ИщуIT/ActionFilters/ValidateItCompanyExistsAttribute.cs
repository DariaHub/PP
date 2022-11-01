using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ИщуIT.ActionFilters
{
    public class ValidateItCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateItCompanyExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var vacancyId = (Guid)context.ActionArguments["vacancyid"];
            var vacancy = await _repository.Vacancy.GetVacancyAsync(vacancyId, trackChanges);
            if (vacancy == null)
            {
                _logger.LogInfo($"Vacancy with id: {vacancyId} doesn't exist in the database.");
                return;
                context.Result = new NotFoundResult();
            }
            var id = (Guid)context.ActionArguments["id"];
            var itCompany = await _repository.ItCompany.GetItCompanyAsync(vacancyId, id, trackChanges);
            if (itCompany == null)
            {
                _logger.LogInfo($"ItCompany with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("itcompany", itCompany);
                await next();
            }
        }
    }
}
