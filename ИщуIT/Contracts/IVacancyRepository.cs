using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IVacancyRepository
    {
        Task<PagedList<Vacancy>> GetVacanciesAsync(bool trackChanges, VacancyParameters parameters);
        Task<Vacancy> GetVacancyAsync(Guid id, bool trackChanges);
        void CreateVacancy(Vacancy vacancy);
        void DeleteVacancy(Vacancy vacancy);
    }
}
