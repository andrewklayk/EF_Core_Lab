using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreTest1.Models
{
    public class Stock
    {
        public int ID { get; set; }
        [Display(Name = "Адреса")]
        public string Address { get; set; }

        [Display(Name = "Працівники")]
        public ICollection<Position> Positions { get; set; }
    }
}
