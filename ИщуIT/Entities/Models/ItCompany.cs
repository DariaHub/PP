using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ItCompany
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        public string Address { get; set; }
        [Required]
        [ForeignKey(nameof(Vacancy))]
        public Guid Id_Vacancy { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(11)]
        public string Phone { get; set; }
    }
}
