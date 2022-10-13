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

        public IEnumerable<ItCompany> GetAllItCompanies(bool trackChanges) =>
            FindAll(trackChanges).OrderBy(c => c.Name).ToList();
    }
}
