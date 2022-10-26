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
    public class VacancyRepository : RepositoryBase<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateVacancy(Vacancy vacancy) => Create(vacancy);

        public void DeleteVacancy(Vacancy vacancy) => Delete(vacancy);

        public IEnumerable<Vacancy> GetVacancies(bool trackChanges) =>
            FindAll(trackChanges).OrderBy(c => c.Name).ToList();

        public Vacancy GetVacancy(Guid id, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault();
    }
}
