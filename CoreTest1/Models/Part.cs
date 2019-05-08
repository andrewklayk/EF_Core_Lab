using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Models
{
    public class Part
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }

        public PartType PartType { get; set; }
    }
}
