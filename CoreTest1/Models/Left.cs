using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Models
{
    public class Left
    {
        public int ID { get; set; }
        public int PartID { get; set; }
        public int StockID { get; set; }
        public DateTime ArrDate { get; set; }
        public int Quantity { get; set; }

        public Part Part { get; set; }
        public Stock Stock { get; set; }
    }
}
