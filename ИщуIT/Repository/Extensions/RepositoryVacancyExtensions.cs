using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;
using System.Reflection;

namespace Repository.Extensions
{
    public static class RepositoryVacancyExtensions
    {
        public static IQueryable<Vacancy> Search(this IQueryable<Vacancy> vacancies, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return vacancies;
            var lowerCaseTerm = search.Trim().ToLower();
            return vacancies.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Vacancy> Sort(this IQueryable<Vacancy> vacancies, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return vacancies.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Vacancy>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return vacancies.OrderBy(e => e.Name);
            return vacancies.OrderBy(orderQuery);
        }
    }
}
