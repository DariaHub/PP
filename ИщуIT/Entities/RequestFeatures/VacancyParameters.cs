using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class VacancyParameters : RequestParameters
    {
        public VacancyParameters()
        {
            OrderBy = "name";
        }
        public uint MinSalary { get; set; }
        public uint MaxSalary { get; set; } = int.MaxValue;
        public bool ValidSalaryRange => MinSalary < MaxSalary;
    }
}
