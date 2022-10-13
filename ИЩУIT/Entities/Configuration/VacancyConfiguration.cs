using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.HasData
            (
                new Vacancy
                {
                    Id = new Guid("94e467fc-e5ae-4005-9f0e-3a05ef051000"),
                    Name = "Программист",
                    Quantity = 2,
                    Salary = 100000000
                },
                new Vacancy
                {
                    Id = new Guid("94e467fc-e5ae-4005-9f0e-3a05ef051001"),
                    Name = "Тестировщик",
                    Quantity = 20,
                    Salary = 20000
                }
            );
        }
    }
}
