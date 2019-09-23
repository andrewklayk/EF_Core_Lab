using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreTest1.Models
{
    public class Customer
    {
        public int ID { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
    }
}
