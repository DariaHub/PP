using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
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

        public void DeleteItCompany(ItCompany itCompany) => Delete(itCompany);

        public async Task<PagedList<ItCompany>> GetItCompaniesAsync(Guid vacancyId, bool trackChanges, ItCompanyParameters parameters)
        {
            var itCompanies = await FindByCondition(c => c.Id_Vacancy.Equals(vacancyId), trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();
            return PagedList<ItCompany>.ToPagedList(itCompanies, parameters.PageNumber, parameters.PageSize);
        }            

        public async Task<ItCompany> GetItCompanyAsync(Guid vacancyId, Guid id, bool trackChanges) =>
            await FindByCondition(c => c.Id_Vacancy.Equals(vacancyId) && c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
