﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreTest1.Models
{
    public class PartType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Одиниці виміру")]
        public string Units { get; set; }
    }
}
