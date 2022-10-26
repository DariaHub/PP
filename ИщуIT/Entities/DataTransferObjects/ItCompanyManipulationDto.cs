using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ItCompanyManipulationDto
    {

        [Required(ErrorMessage = "ItCompany name is a required field.")]
        [MaxLength(30, ErrorMessage = "Максимальное количество символов 30")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address is a required field.")]
        [MaxLength(30, ErrorMessage = "Максимальное количество символов 30")]
        public string Address { get; set; }
        [MinLength(11)]
        [MaxLength(11)]
        public string Phone { get; set; }
    }
}
