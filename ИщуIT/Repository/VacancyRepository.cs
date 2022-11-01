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
    public class VacancyRepository : RepositoryBase<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateVacancy(Vacancy vacancy) => Create(vacancy);

        public void DeleteVacancy(Vacancy vacancy) => Delete(vacancy);

        public async Task<PagedList<Vacancy>> GetVacanciesAsync(bool trackChanges, VacancyParameters parameters)
        {
            var vacanies = await FindAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();
            return PagedList<Vacancy>.ToPagedList(vacanies, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Vacancy> GetVacancyAsync(Guid id, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
