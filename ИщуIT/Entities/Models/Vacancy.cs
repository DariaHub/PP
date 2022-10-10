using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Vacancy
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        public int Salary { get; set; }
    }
}
