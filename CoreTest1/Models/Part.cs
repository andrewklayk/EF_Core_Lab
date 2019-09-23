using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreTest1.Models
{
    public class Part
    {
        public int ID { get; set; }
        [Display(Name = "Найменування")]
        [Required(ErrorMessage = "Це поле є обов'язковим")]
        public string Name { get; set; }
        [Display(Name = "Тип")]
        public int Type { get; set; }

        public PartType PartType { get; set; }
    }
}
