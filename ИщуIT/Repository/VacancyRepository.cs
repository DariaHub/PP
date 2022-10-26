using Contracts;
using Entities;
using Entities.Models;
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

        public async Task<IEnumerable<Vacancy>> GetVacanciesAsync(bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

        public async Task<Vacancy> GetVacancyAsync(Guid id, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
