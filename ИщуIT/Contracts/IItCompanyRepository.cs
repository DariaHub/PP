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
        IEnumerable<ItCompany> GetItCompanies(Guid vacancyId, bool trackChanges);
        ItCompany GetItCompany(Guid vacancyId, Guid id, bool trackChanges);
        void CreateItCompany(Guid vacancyId, ItCompany itCompany);
        void DeleteItCompany(ItCompany itCompany);
    }
}
