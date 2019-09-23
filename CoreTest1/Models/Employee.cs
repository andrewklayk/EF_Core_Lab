using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreTest1.Models
{
    public class Employee
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [Required(ErrorMessage = "Це поле є обов'язковим")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }
        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Це поле є обов'язковим")]
        [MinLength(2)]
        [MaxLength(10)]
        public string Surname { get; set; }
        
        [Display(Name = "Місця роботи")]
        public List<Position> Positions { get; set; }
    }
}
