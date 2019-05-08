using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Models
{
    public class PartInContract
    {
        public int ID { get; set; }
        public int ContractID { get; set; }
        public int PartID { get; set; }
        public int Quantity { get; set; }

        public Contract Contract { get; set; }
        public Part Part { get; set; }
    }
}
