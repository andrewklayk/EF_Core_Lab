using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public string Address { get; set; }

        public ICollection<Position> Positions { get; set; }
    }
}
