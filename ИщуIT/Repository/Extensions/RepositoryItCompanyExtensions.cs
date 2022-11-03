using Entities.Models;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryItCompanyExtensions
    {
        public static IQueryable<ItCompany> Search(this IQueryable<ItCompany> itCompanies, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return itCompanies;
            var lowerCaseTerm = search.Trim().ToLower();
            return itCompanies.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<ItCompany> Sort(this IQueryable<ItCompany> itCompanies, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return itCompanies.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<ItCompany>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return itCompanies.OrderBy(e => e.Name);
            return itCompanies.OrderBy(orderQuery);
        }
    }
}
