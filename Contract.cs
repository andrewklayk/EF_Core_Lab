using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Models
{
    public class Contract
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public DateTime SignDate { get; set; }

        public Customer Customer { get; set; }
        public ICollection<PartInContract> PartsInContr { get; set; }
    }
}
