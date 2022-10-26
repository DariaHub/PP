using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IItCompanyRepository
    {
        Task<IEnumerable<ItCompany>> GetItCompaniesAsync(Guid vacancyId, bool trackChanges);
        Task<ItCompany> GetItCompanyAsync(Guid vacancyId, Guid id, bool trackChanges);
        void CreateItCompany(Guid vacancyId, ItCompany itCompany);
        void DeleteItCompany(ItCompany itCompany);
    }
}
