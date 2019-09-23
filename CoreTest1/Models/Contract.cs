using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CoreTest1.Models
{
    public class Contract
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [CustomDateRange]
        public DateTime SignDate { get; set; }

        [Display(Name = "Замовник")]
        public Customer Customer { get; set; }
        [Display(Name = "Частини")]
        public ICollection<PartInContract> PartsInContr { get; set; }
    }
}
