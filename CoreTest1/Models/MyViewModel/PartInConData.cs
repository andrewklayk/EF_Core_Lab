using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Models.MyViewModel
{
    public class PartInConData
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public string PartTypeName { get; set; }
        public int Quantity { get; set; }
        public bool Assigned { get; set; }
    }
}
