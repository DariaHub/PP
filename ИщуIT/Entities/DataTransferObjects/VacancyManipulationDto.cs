using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class VacancyManipulationDto
    {
        [Required(ErrorMessage = "Vacancy name is a required field.")]
        [MaxLength(30, ErrorMessage = "Максимальное количество символов 30")]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть минимум одна вакансия")]
        public int Quantity { get; set; }
        [Required]
        [Range(90000, int.MaxValue, ErrorMessage = "Зарплата должна быть больше 90тыс")]
        public int Salary { get; set; }
    }
}
