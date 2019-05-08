using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Models
{
    public class Position
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int StockID { get; set; }

        public Stock Stock { get; set; }
        public Employee Employee { get; set; }

    }
}
