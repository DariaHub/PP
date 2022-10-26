﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IVacancyRepository
    {
        Task<IEnumerable<Vacancy>> GetVacanciesAsync(bool trackChanges);
        Task<Vacancy> GetVacancyAsync(Guid id, bool trackChanges);
        void CreateVacancy(Vacancy vacancy);
        void DeleteVacancy(Vacancy vacancy);
    }
}
