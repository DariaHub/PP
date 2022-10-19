using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ItCompanyRepository : RepositoryBase<ItCompany>, IItCompanyRepository
    {
        public ItCompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateItCompany(Guid vacancyId, ItCompany itCompany)
        {
            itCompany.Id_Vacancy = vacancyId;
            Create(itCompany);
        }

        public IEnumerable<ItCompany> GetItCompanies(Guid vacancyId, bool trackChanges) =>
            FindByCondition(c => c.Id_Vacancy.Equals(vacancyId), trackChanges).OrderBy(c => c.Name);

        public ItCompany GetItCompany(Guid vacancyId, Guid id, bool trackChanges) =>
            FindByCondition(c => c.Id_Vacancy.Equals(vacancyId) && c.Id.Equals(id), trackChanges).SingleOrDefault();
    }
}
