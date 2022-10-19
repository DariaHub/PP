using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class VacancyCreateDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Salary { get; set; }
    }
}
