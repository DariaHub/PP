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
    public class ItCompanyConfiguration : IEntityTypeConfiguration<ItCompany>
    {
        public void Configure(EntityTypeBuilder<ItCompany> builder)
        {
            builder.HasData
            (
                new ItCompany
                {
                    Id = new Guid("3b2c8329-694a-4c53-8bdf-3c42b154d000"),
                    Name = "SimdirSoft",
                    Address = "Saransk",
                    Id_Vacancy = new Guid("94e467fc-e5ae-4005-9f0e-3a05ef051000"),
                    Phone = "88005553535"
                }
            );
        }
    }
}
